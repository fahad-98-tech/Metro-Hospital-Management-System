using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;

namespace MetroHospitalApplication
{
    public partial class BookPackage : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        List<string> allSlots = new List<string>();

        List<TimeSpan[]> breaks = new List<TimeSpan[]>
        {
            new TimeSpan[]{TimeSpan.FromHours(10),TimeSpan.FromHours(10.5)},
            new TimeSpan[]{TimeSpan.FromHours(13),TimeSpan.FromHours(14)},
            new TimeSpan[]{TimeSpan.FromHours(16),TimeSpan.FromHours(16.5)},
            new TimeSpan[]{TimeSpan.FromHours(20),TimeSpan.FromHours(21)}
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int packageId = Convert.ToInt32(Request.QueryString["PackageId"]);

                hfPackageId.Value = packageId.ToString();

                LoadPackage(packageId);

                txtPatientName.Text = Session["UserName"]?.ToString();
                txtPatientMobile.Text = Session["UserMobile"]?.ToString();
            }
        }

        private void LoadPackage(int packageId)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(
                "SELECT PackageName,Doctor_Id FROM Packages WHERE PackageId=@Id", con);

                cmd.Parameters.AddWithValue("@Id", packageId);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    lblPackageName.Text = dr["PackageName"].ToString();

                    hfDoctorId.Value = dr["Doctor_Id"].ToString();

                    txtDoctor.Text = GetDoctorName(Convert.ToInt32(hfDoctorId.Value));
                }

                dr.Close();
            }
        }

        private string GetDoctorName(int id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(
                "SELECT FullName FROM Doctors WHERE DoctorId=@Id", con);

                cmd.Parameters.AddWithValue("@Id", id);

                return cmd.ExecuteScalar()?.ToString();
            }
        }

        protected void txtDate_TextChanged(object sender, EventArgs e)
        {
            ddlFromTime.Items.Clear();
            ddlToTime.Items.Clear();

            if (!DateTime.TryParse(txtDate.Text, out DateTime selectedDate))
                return;

            int doctorId = Convert.ToInt32(hfDoctorId.Value);

            List<string> slots = GetAvailableSlots(selectedDate, doctorId);

            foreach (string s in slots)
            {
                ddlFromTime.Items.Add(new ListItem(s, s));
            }

            ViewState["AvailableSlots"] = slots;
        }
        private List<string> GetAvailableSlots(DateTime date, int doctorId)
        {
            List<string> slots = new List<string>();

            TimeSpan start = new TimeSpan(7, 0, 0);
            TimeSpan end = new TimeSpan(23, 30, 0);

            for (TimeSpan t = start; t <= end; t = t.Add(TimeSpan.FromMinutes(30)))
            {
                slots.Add(t.ToString(@"hh\:mm"));
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(@"
        SELECT AppointmentTime, AppointmentEndTime
        FROM Appointments
        WHERE DoctorId=@DoctorId
        AND AppointmentDate=@Date
        AND Status='Booked'", con);

                cmd.Parameters.AddWithValue("@DoctorId", doctorId);
                cmd.Parameters.AddWithValue("@Date", date);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    TimeSpan from = TimeSpan.Parse(dr["AppointmentTime"].ToString());
                    TimeSpan to = TimeSpan.Parse(dr["AppointmentEndTime"].ToString());

                    slots = slots.Where(s =>
                    {
                        TimeSpan slot = TimeSpan.Parse(s);
                        return slot < from || slot >= to;
                    }).ToList();
                }
            }

            return slots;
        }
        private void GenerateSlots()
        {
            allSlots.Clear();

            TimeSpan start = TimeSpan.FromHours(7);
            TimeSpan end = TimeSpan.FromHours(22);

            for (TimeSpan t = start; t < end; t += TimeSpan.FromMinutes(30))
            {
                bool isBreak = breaks.Any(b => t >= b[0] && t < b[1]);

                if (!isBreak)
                {
                    allSlots.Add(t.ToString(@"hh\:mm"));
                }
            }
        }

        private void RemoveBookedSlots(DateTime date)
        {
            int doctorId = Convert.ToInt32(hfDoctorId.Value);

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(
                "SELECT AppointmentTime,AppointmentEndTime FROM Appointments WHERE DoctorId=@D AND AppointmentDate=@Dt AND Status='Booked'", con);

                cmd.Parameters.AddWithValue("@D", doctorId);
                cmd.Parameters.AddWithValue("@Dt", date);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    TimeSpan from = TimeSpan.Parse(dr["AppointmentTime"].ToString());
                    TimeSpan to = TimeSpan.Parse(dr["AppointmentEndTime"].ToString());

                    allSlots = allSlots.Where(s =>
                    {
                        TimeSpan slot = TimeSpan.Parse(s);
                        return slot < from || slot >= to;
                    }).ToList();
                }

                dr.Close();
            }
        }

        protected void ddlFromTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlToTime.Items.Clear();

            if (ddlFromTime.SelectedIndex < 0)
                return;

            List<string> slots = ViewState["AvailableSlots"] as List<string>;

            if (slots == null)
                return;

            string fromTime = ddlFromTime.SelectedValue;

            int index = slots.IndexOf(fromTime);

            for (int i = index + 1; i < slots.Count; i++)
            {
                ddlToTime.Items.Add(new ListItem(slots[i], slots[i]));
            }
        }

        protected void btnBook_Click(object sender, EventArgs e)
        {
            DateTime date = Convert.ToDateTime(txtDate.Text);

            string from = ddlFromTime.SelectedValue;
            string to = ddlToTime.SelectedValue;

            int doctorId = Convert.ToInt32(hfDoctorId.Value);

            string patientName = txtPatientName.Text;
            string mobile = txtPatientMobile.Text;

            int patientId = Convert.ToInt32(Session["UserId"]);

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(@"

INSERT INTO Appointments
(DoctorId,PatientId,PatientName,PatientMobile,AppointmentDate,AppointmentTime,AppointmentEndTime,Specialization,Status,CreatedAt,IsActive)

VALUES
(@DoctorId,@PatientId,@Name,@Mobile,@Date,@From,@To,'Package Appointment','Booked',GETDATE(),1)

", con);

                cmd.Parameters.AddWithValue("@DoctorId", doctorId);
                cmd.Parameters.AddWithValue("@PatientId", patientId);
                cmd.Parameters.AddWithValue("@Name", patientName);
                cmd.Parameters.AddWithValue("@Mobile", mobile);
                cmd.Parameters.AddWithValue("@Date", date);
                cmd.Parameters.AddWithValue("@From", from);
                cmd.Parameters.AddWithValue("@To", to);

                cmd.ExecuteNonQuery();
            }

            Response.Redirect("PatientAppointmentHistory.aspx");
        }
    }
}
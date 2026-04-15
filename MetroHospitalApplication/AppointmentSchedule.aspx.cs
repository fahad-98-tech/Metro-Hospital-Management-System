using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace MetroHospitalApplication
{
    public partial class AppointmentSchedule : System.Web.UI.Page
    {
        string conStr = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDoctors();

                DateTime today = DateTime.Today;
                DateTime maxDate = today.AddMonths(1);

                txtDate.Text = today.ToString("yyyy-MM-dd");

                // Restrict date picker to today → +1 month
                txtDate.Attributes["min"] = today.ToString("yyyy-MM-dd");
                txtDate.Attributes["max"] = maxDate.ToString("yyyy-MM-dd");
            }
        }

        void LoadDoctors()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT DoctorId, FullName FROM Doctors", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlDoctor.DataSource = dt;
                ddlDoctor.DataTextField = "FullName";
                ddlDoctor.DataValueField = "DoctorId";
                ddlDoctor.DataBind();

                ddlDoctor.Items.Insert(0, new ListItem("--Select Doctor--", "0"));
            }
        }

        protected void ddlDoctor_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlSpecialization.Items.Clear();

            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT Specialization FROM Doctors WHERE DoctorId=@Id", con);
                cmd.Parameters.AddWithValue("@Id", ddlDoctor.SelectedValue);
                con.Open();

                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    foreach (string s in result.ToString().Split(','))
                        ddlSpecialization.Items.Add(s.Trim());
                }
            }
        }

        protected void txtDate_TextChanged(object sender, EventArgs e)
        {
            pnlBooking.Visible = false;
        }

        DataTable GetDoctorShift(DateTime date)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT ShiftStart, ShiftEnd FROM DoctorShifts WHERE DoctorId=@D AND ShiftDate=@Dt AND IsActive=1", con);

                da.SelectCommand.Parameters.AddWithValue("@D", ddlDoctor.SelectedValue);
                da.SelectCommand.Parameters.AddWithValue("@Dt", date.Date);

                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        HashSet<string> GetBookedSlots(DateTime date)
        {
            HashSet<string> set = new HashSet<string>();

            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT AppointmentTime, AppointmentEndTime FROM Appointments WHERE DoctorId=@D AND AppointmentDate=@Dt", con);

                cmd.Parameters.AddWithValue("@D", ddlDoctor.SelectedValue);
                cmd.Parameters.AddWithValue("@Dt", date.Date);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    DateTime start = Convert.ToDateTime(dr["AppointmentTime"]);
                    DateTime end = Convert.ToDateTime(dr["AppointmentEndTime"]);

                    while (start < end)
                    {
                        set.Add(start.ToString("HH:mm"));
                        start = start.AddMinutes(30);
                    }
                }
            }
            return set;
        }

        bool IsBreakTime(DateTime t)
        {
            return t.Hour == 13; // Example: break at 1 PM
        }

        void LoadFromTimes()
        {
            ddlFromTime.Items.Clear();

            DateTime date = DateTime.Parse(txtDate.Text);
            DateTime now = DateTime.Now;

            var shifts = GetDoctorShift(date);
            var booked = GetBookedSlots(date);

            foreach (DataRow r in shifts.Rows)
            {
                DateTime shiftStart = Convert.ToDateTime(r["ShiftStart"]);
                DateTime shiftEnd = Convert.ToDateTime(r["ShiftEnd"]);

                DateTime start = date.Date.AddHours(shiftStart.Hour).AddMinutes(shiftStart.Minute);
                DateTime end = date.Date.AddHours(shiftEnd.Hour).AddMinutes(shiftEnd.Minute);

                while (start <= end)
                {
                    if (date == DateTime.Today && start <= now) { start = start.AddMinutes(30); continue; }
                    if (IsBreakTime(start)) { start = start.AddMinutes(30); continue; }

                    if (!booked.Contains(start.ToString("HH:mm")))
                        ddlFromTime.Items.Add(new ListItem(start.ToString("hh:mm tt"), start.ToString("HH:mm")));

                    start = start.AddMinutes(30);
                }
            }
        }

        protected void ddlFromTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlToTime.Items.Clear();

            if (ddlFromTime.SelectedIndex == -1 || string.IsNullOrEmpty(txtDate.Text))
                return;

            DateTime date = DateTime.Parse(txtDate.Text);
            DateTime from = DateTime.ParseExact(ddlFromTime.SelectedValue, "HH:mm", null);
            DateTime now = DateTime.Now;

            var booked = GetBookedSlots(date);

            // Calculate the next 30-min slot
            DateTime nextSlot = from.AddMinutes(30);

            // Check if next slot is valid
            bool isAvailable = true;

            // Skip past times
            if (date == DateTime.Today && nextSlot <= now)
                isAvailable = false;

            // Skip break time (e.g., 1 PM)
            if (IsBreakTime(nextSlot))
                isAvailable = false;

            // Skip if booked
            if (booked.Contains(nextSlot.ToString("HH:mm")))
                isAvailable = false;

            // Add to To dropdown if available
            if (isAvailable)
            {
                ddlToTime.Items.Add(new ListItem(nextSlot.ToString("hh:mm tt"), nextSlot.ToString("HH:mm")));
                ddlToTime.SelectedIndex = 0;
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            var shifts = GetDoctorShift(DateTime.Parse(txtDate.Text));

            if (shifts.Rows.Count == 0)
            {
                pnlBooking.Visible = false;
                return;
            }

            LoadFromTimes();
            pnlBooking.Visible = true;
        }

        protected void btnFullSchedule_Click(object sender, EventArgs e)
        {
            DateTime date = DateTime.Parse(txtDate.Text);

            DataTable dt = new DataTable();
            dt.Columns.Add("TimeSlot");
            dt.Columns.Add("Status");

            var shifts = GetDoctorShift(date);
            var booked = GetBookedSlots(date);

            foreach (DataRow r in shifts.Rows)
            {
                DateTime shiftStart = Convert.ToDateTime(r["ShiftStart"]);
                DateTime shiftEnd = Convert.ToDateTime(r["ShiftEnd"]);

                DateTime start = date.Date.AddHours(shiftStart.Hour).AddMinutes(shiftStart.Minute);
                DateTime end = date.Date.AddHours(shiftEnd.Hour).AddMinutes(shiftEnd.Minute);

                while (start < end)
                {
                    string status = "Available";

                    if (IsBreakTime(start)) status = "Break";
                    else if (booked.Contains(start.ToString("HH:mm"))) status = "Booked";

                    dt.Rows.Add(start.ToString("hh:mm tt"), status);

                    start = start.AddMinutes(30);
                }
            }

            gvFullSchedule.DataSource = dt;
            gvFullSchedule.DataBind();
            gvFullSchedule.Visible = true;
        }

        protected void gvFullSchedule_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string status = e.Row.Cells[1].Text;

                if (status == "Available") e.Row.CssClass = "available";
                else if (status == "Booked") e.Row.CssClass = "booked";
                else e.Row.CssClass = "break";
            }
        }

        protected void btnBook_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();

                DateTime date = DateTime.Parse(txtDate.Text);
                DateTime from = DateTime.ParseExact(ddlFromTime.SelectedValue, "HH:mm", null);
                DateTime to = DateTime.ParseExact(ddlToTime.SelectedValue, "HH:mm", null);

                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Appointments VALUES (@DoctorId, @SomeNull, @PatientName, @Date, @FromTime, @ToTime, @Specialization, @Status, @CreatedDate, @Flag, @Mobile, @Approval)",
                    con);

                cmd.Parameters.AddWithValue("@DoctorId", ddlDoctor.SelectedValue);
                cmd.Parameters.AddWithValue("@SomeNull", DBNull.Value);
                cmd.Parameters.AddWithValue("@PatientName", txtPatientName.Text);
                cmd.Parameters.AddWithValue("@Date", date);
                cmd.Parameters.AddWithValue("@FromTime", from.ToString("h:mm tt"));
                cmd.Parameters.AddWithValue("@ToTime", to.ToString("h:mm tt"));
                cmd.Parameters.AddWithValue("@Specialization", ddlSpecialization.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@Status", "Booked");
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@Flag", 1);
                cmd.Parameters.AddWithValue("@Mobile", txtPatientMobile.Text);
                cmd.Parameters.AddWithValue("@Approval", "pending");

                cmd.ExecuteNonQuery();
            }

            LoadFromTimes();
        }
    }
}
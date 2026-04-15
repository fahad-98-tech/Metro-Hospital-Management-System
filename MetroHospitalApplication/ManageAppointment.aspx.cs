using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace MetroHospitalApplication
{
    public partial class ManageAppointment : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                txtDate.Attributes["min"] = DateTime.Today.ToString("yyyy-MM-dd");
                txtDate.Attributes["max"] = DateTime.Today.AddDays(30).ToString("yyyy-MM-dd");

                LoadDoctor();
                LoadFromTimeSlots();
            }
        }

        private void LoadDoctor()
        {
            int appointmentId = Convert.ToInt32(Request.QueryString["AppointmentId"]);
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"
                    SELECT D.FullName 
                    FROM Appointments A
                    INNER JOIN Doctors D ON A.DoctorId = D.DoctorId
                    WHERE A.AppointmentId=@Id", con);
                cmd.Parameters.AddWithValue("@Id", appointmentId);
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    lblDoctor.Text = result.ToString();
                }
            }
        }

        private List<DateTime> GenerateTimeSlots(DateTime shiftStart, DateTime shiftEnd)
        {
            List<DateTime> slots = new List<DateTime>();
            DateTime slot = shiftStart;

            while (slot < shiftEnd)
            {
                // Skip lunch break
                if (!(slot.TimeOfDay >= new TimeSpan(13, 0, 0) && slot.TimeOfDay < new TimeSpan(14, 0, 0)))
                {
                    slots.Add(slot);
                }
                slot = slot.AddMinutes(30);
            }
            return slots;
        }

        private void LoadFromTimeSlots()
        {
            ddlFromTime.Items.Clear();
            if (string.IsNullOrEmpty(txtDate.Text)) return;

            int appointmentId = Convert.ToInt32(Request.QueryString["AppointmentId"]);
            int doctorId;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                // Get DoctorId for this appointment
                SqlCommand cmd = new SqlCommand("SELECT DoctorId FROM Appointments WHERE AppointmentId=@Id", con);
                cmd.Parameters.AddWithValue("@Id", appointmentId);
                doctorId = Convert.ToInt32(cmd.ExecuteScalar());

                // Get doctor's shift for selected date
                SqlCommand shiftCmd = new SqlCommand(@"
                    SELECT ShiftStart, ShiftEnd 
                    FROM DoctorShifts
                    WHERE DoctorId=@DoctorId AND ShiftDate=@Date AND IsActive=1", con);
                shiftCmd.Parameters.AddWithValue("@DoctorId", doctorId);
                shiftCmd.Parameters.AddWithValue("@Date", txtDate.Text);

                SqlDataReader shiftReader = shiftCmd.ExecuteReader();
                if (!shiftReader.HasRows)
                {
                    ddlFromTime.Items.Add("No shift available");
                    shiftReader.Close();
                    return;
                }

                shiftReader.Read();
                DateTime shiftStart = DateTime.Parse(txtDate.Text + " " + shiftReader["ShiftStart"].ToString());
                DateTime shiftEnd = DateTime.Parse(txtDate.Text + " " + shiftReader["ShiftEnd"].ToString());
                shiftReader.Close();

                // Generate all possible slots within shift
                var allSlots = GenerateTimeSlots(shiftStart, shiftEnd);

                // Get already booked appointments for this doctor on this date
                SqlCommand bookedCmd = new SqlCommand(@"
                    SELECT AppointmentTime, AppointmentEndTime 
                    FROM Appointments 
                    WHERE DoctorId=@DoctorId AND AppointmentDate=@Date AND AppointmentId<>@AppointmentId AND Status='Booked'", con);
                bookedCmd.Parameters.AddWithValue("@DoctorId", doctorId);
                bookedCmd.Parameters.AddWithValue("@Date", txtDate.Text);
                bookedCmd.Parameters.AddWithValue("@AppointmentId", appointmentId);

                SqlDataReader dr = bookedCmd.ExecuteReader();
                List<Tuple<DateTime, DateTime>> bookedIntervals = new List<Tuple<DateTime, DateTime>>();
                while (dr.Read())
                {
                    DateTime bookedStart = DateTime.Parse(txtDate.Text + " " + dr["AppointmentTime"].ToString());
                    DateTime bookedEnd = DateTime.Parse(txtDate.Text + " " + dr["AppointmentEndTime"].ToString());
                    bookedIntervals.Add(Tuple.Create(bookedStart, bookedEnd));
                }
                dr.Close();

                foreach (var slot in allSlots)
                {
                    if (slot <= DateTime.Now) continue;

                    bool overlap = false;
                    foreach (var interval in bookedIntervals)
                    {
                        if (slot >= interval.Item1 && slot < interval.Item2)
                        {
                            overlap = true;
                            break;
                        }
                    }

                    if (!overlap)
                        ddlFromTime.Items.Add(slot.ToString("hh:mm tt"));
                }

                if (ddlFromTime.Items.Count > 0)
                    ddlFromTime.SelectedIndex = 0;
            }

            ddlFromTime_SelectedIndexChanged(null, null); // Refresh ToTime dropdown
        }

        protected void ddlFromTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlToTime.Items.Clear();
            if (ddlFromTime.SelectedIndex == -1 || string.IsNullOrEmpty(txtDate.Text))
                return;

            DateTime selectedFrom = DateTime.Parse(txtDate.Text + " " + ddlFromTime.SelectedValue);
            DateTime nextSlot = selectedFrom.AddMinutes(30);

            int appointmentId = Convert.ToInt32(Request.QueryString["AppointmentId"]);
            int doctorId;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT DoctorId FROM Appointments WHERE AppointmentId=@Id", con);
                cmd.Parameters.AddWithValue("@Id", appointmentId);
                doctorId = Convert.ToInt32(cmd.ExecuteScalar());

                SqlCommand bookedCmd = new SqlCommand(@"
                    SELECT AppointmentTime, AppointmentEndTime 
                    FROM Appointments 
                    WHERE DoctorId=@DoctorId AND AppointmentDate=@Date AND AppointmentId<>@AppointmentId AND Status='Booked'", con);
                bookedCmd.Parameters.AddWithValue("@DoctorId", doctorId);
                bookedCmd.Parameters.AddWithValue("@Date", txtDate.Text);
                bookedCmd.Parameters.AddWithValue("@AppointmentId", appointmentId);

                SqlDataReader dr = bookedCmd.ExecuteReader();
                List<Tuple<DateTime, DateTime>> bookedIntervals = new List<Tuple<DateTime, DateTime>>();
                while (dr.Read())
                {
                    DateTime bookedStart = DateTime.Parse(txtDate.Text + " " + dr["AppointmentTime"].ToString());
                    DateTime bookedEnd = DateTime.Parse(txtDate.Text + " " + dr["AppointmentEndTime"].ToString());
                    bookedIntervals.Add(Tuple.Create(bookedStart, bookedEnd));
                }
                dr.Close();

                bool isAvailable = true;

                if (nextSlot <= DateTime.Now)
                    isAvailable = false;

                foreach (var interval in bookedIntervals)
                {
                    if (nextSlot > interval.Item1 && nextSlot <= interval.Item2)
                    {
                        isAvailable = false;
                        break;
                    }
                }

                // Skip lunch break
                if (nextSlot.TimeOfDay >= new TimeSpan(13, 0, 0) && nextSlot.TimeOfDay < new TimeSpan(14, 0, 0))
                    isAvailable = false;

                if (isAvailable)
                {
                    ddlToTime.Items.Add(nextSlot.ToString("hh:mm tt"));
                    ddlToTime.SelectedIndex = 0;
                }
            }
        }

        protected void txtDate_TextChanged(object sender, EventArgs e)
        {
            LoadFromTimeSlots();
            ddlToTime.Items.Clear();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDate.Text) || ddlFromTime.SelectedIndex == -1 || ddlToTime.SelectedIndex == -1)
                return;

            DateTime selectedFrom = DateTime.Parse(txtDate.Text + " " + ddlFromTime.SelectedValue);
            DateTime selectedTo = DateTime.Parse(txtDate.Text + " " + ddlToTime.SelectedValue);

            if (selectedTo <= selectedFrom)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('End time must be after start time');", true);
                return;
            }

            int appointmentId = Convert.ToInt32(Request.QueryString["AppointmentId"]);
            int doctorId;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT DoctorId FROM Appointments WHERE AppointmentId=@Id", con);
                cmd.Parameters.AddWithValue("@Id", appointmentId);
                doctorId = Convert.ToInt32(cmd.ExecuteScalar());

                // Check overlap
                SqlCommand overlapCmd = new SqlCommand(@"
                    SELECT COUNT(*) 
                    FROM Appointments
                    WHERE DoctorId=@DoctorId
                      AND AppointmentDate=@Date
                      AND AppointmentId <> @AppointmentId
                      AND Status='Booked'
                      AND NOT (AppointmentEndTime <= @FromTime OR AppointmentTime >= @ToTime)
                ", con);

                overlapCmd.Parameters.AddWithValue("@DoctorId", doctorId);
                overlapCmd.Parameters.AddWithValue("@Date", txtDate.Text);
                overlapCmd.Parameters.AddWithValue("@FromTime", ddlFromTime.SelectedValue);
                overlapCmd.Parameters.AddWithValue("@ToTime", ddlToTime.SelectedValue);
                overlapCmd.Parameters.AddWithValue("@AppointmentId", appointmentId);

                int count = (int)overlapCmd.ExecuteScalar();
                if (count > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Selected interval overlaps with an existing appointment. Please select another time.');", true);
                    return;
                }

                SqlCommand updateCmd = new SqlCommand(@"
                    UPDATE Appointments
                    SET AppointmentDate=@Date,
                        AppointmentTime=@FromTime,
                        AppointmentEndTime=@ToTime
                    WHERE AppointmentId=@Id
                ", con);

                updateCmd.Parameters.AddWithValue("@Date", txtDate.Text);
                updateCmd.Parameters.AddWithValue("@FromTime", ddlFromTime.SelectedValue);
                updateCmd.Parameters.AddWithValue("@ToTime", ddlToTime.SelectedValue);
                updateCmd.Parameters.AddWithValue("@Id", appointmentId);

                updateCmd.ExecuteNonQuery();
            }

            Response.Redirect("PatientAppointmentHistory.aspx");
        }
    }
}
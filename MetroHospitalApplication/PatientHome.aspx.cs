using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetroHospitalApplication
{
    public partial class PatientHome : System.Web.UI.Page
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
                LoadDepartments();
                SetDateLimits();
                LoadTests();

                txtTestDate.Attributes["min"] = DateTime.Today.ToString("yyyy-MM-dd");
                txtTestDate.Attributes["max"] = DateTime.Today.AddMonths(1).ToString("yyyy-MM-dd");
                txtTestDate.Text = DateTime.Today.ToString("yyyy-MM-dd");

                LoadTestTimes();
            }
        }
        private void LoadTests()
        {
            ddlTest.Items.Clear();
            ddlTest.Items.Add("-- Select Test --");

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("SELECT TestId, TestName FROM Tests WHERE IsActive=1", con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ddlTest.Items.Add(new ListItem(
                        dr["TestName"].ToString(),
                        dr["TestId"].ToString()
                    ));
                }
            }
        }

        private void LoadTestTimes()
        {
            ddlTestTime.Items.Clear();

            if (string.IsNullOrEmpty(txtTestDate.Text)) return;

            DateTime date = DateTime.Parse(txtTestDate.Text);
            DateTime now = DateTime.Now;

            DateTime start = date.AddHours(7);   // 7:00 AM
            DateTime end = date.AddHours(12);    // 12:00 PM

            // Get booked times as HashSet<string> in "HH:mm" format
            HashSet<string> booked = GetBookedTestSlots(date);

            while (start <= end)
            {
                // Use 24-hour format for comparison
                string timeForComparison = start.ToString("HH:mm");   // e.g., "07:30"
                string timeForDisplay = start.ToString("h:mm tt");    // e.g., "7:30 AM"

                // Skip past times for today
                if (date == DateTime.Today && start <= now)
                {
                    start = start.AddMinutes(15);
                    continue;
                }

                // Only add if not booked
                if (!booked.Contains(timeForComparison))
                {
                    ddlTestTime.Items.Add(new ListItem(timeForDisplay, timeForComparison));
                }

                start = start.AddMinutes(15);
            }
        }

        private HashSet<string> GetBookedTestSlots(DateTime date)
        {
            HashSet<string> set = new HashSet<string>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT TestTime FROM PatientTests WHERE TestDate=@Dt";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Dt", date);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    // Store as "HH:mm" for easy comparison
                    TimeSpan t = (TimeSpan)dr["TestTime"];
                    set.Add(t.ToString(@"hh\:mm"));
                }
            }

            return set;
        }

        protected void txtTestDate_TextChanged(object sender, EventArgs e)
        {
            LoadTestTimes();
        }

        protected void btnBookTest_Click(object sender, EventArgs e)
        {
            if (ddlTest.SelectedIndex <= 0 || ddlTestTime.SelectedIndex < 0)
            {
                ShowAlert("Select test and time", false);
                return;
            }

            // ✅ FIXED MODAL CALL
            ScriptManager.RegisterStartupScript(this, this.GetType(),
                "openModal", "openTestModal();", true);
        }

        protected void btnConfirmTest_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTestPatientName.Text) ||
                string.IsNullOrWhiteSpace(txtTestContact.Text))
            {
                ShowAlert("Enter patient details", false);
                return;
            }

            DateTime date = DateTime.Parse(txtTestDate.Text);
            string time = ddlTestTime.SelectedValue;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand check = new SqlCommand(
                    "SELECT COUNT(*) FROM PatientTests WHERE TestDate=@D AND TestTime=@T", con);

                check.Parameters.AddWithValue("@D", date);
                check.Parameters.AddWithValue("@T", time);

                if ((int)check.ExecuteScalar() > 0)
                {
                    ShowAlert("Slot already booked", false);
                    return;
                }

                SqlCommand cmd = new SqlCommand(@"
            INSERT INTO PatientTests
            (PatientId, DoctorId, AppointmentId, TestId, TestDate, TestTime, PatientName, Contact, Status)
            VALUES (@P,NULL,NULL,@TId,@Date,@Time,@Name,@Contact,'Pending')", con);

                cmd.Parameters.AddWithValue("@P", Session["UserId"]);
                cmd.Parameters.AddWithValue("@TId", ddlTest.SelectedValue);
                cmd.Parameters.AddWithValue("@Date", date);
                cmd.Parameters.AddWithValue("@Time", time);
                cmd.Parameters.AddWithValue("@Name", txtTestPatientName.Text);
                cmd.Parameters.AddWithValue("@Contact", txtTestContact.Text);

                cmd.ExecuteNonQuery();
            }

            ShowAlert("Test booked successfully!", true);
            LoadTestTimes();
        }
        private void SetDateLimits()
        {
            DateTime today = DateTime.Today;
            DateTime maxDate = today.AddMonths(1);

            txtAppointmentDate.Attributes["min"] = today.ToString("yyyy-MM-dd");
            txtAppointmentDate.Attributes["max"] = maxDate.ToString("yyyy-MM-dd");
            txtAppointmentDate.Text = today.ToString("yyyy-MM-dd");
        }

        private void LoadDepartments()
        {
            HashSet<string> specializations = new HashSet<string>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT Specialization FROM Doctors WHERE IsActive = 1 AND Specialization IS NOT NULL", con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    string[] specs = dr["Specialization"].ToString().Split(',');
                    foreach (string spec in specs)
                        if (!string.IsNullOrWhiteSpace(spec))
                            specializations.Add(spec.Trim());
                }
            }

            ddlDepartment.Items.Clear();
            ddlDepartment.Items.Add("-- Select Department --");

            foreach (string spec in specializations.OrderBy(x => x))
                ddlDepartment.Items.Add(spec);
        }

        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDepartment.SelectedIndex > 0)
                LoadDoctorsByDepartment(ddlDepartment.SelectedValue);
            else
            {
                ddlDoctor.Items.Clear();
                ddlDoctor.Items.Add("-- Select Doctor --");
            }
        }

        private void LoadDoctorsByDepartment(string department)
        {
            ddlDoctor.Items.Clear();
            ddlDoctor.Items.Add("-- Select Doctor --");

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT DoctorId, FullName 
                    FROM Doctors
                    WHERE IsActive = 1 AND Specialization LIKE @Department", con);
                cmd.Parameters.AddWithValue("@Department", "%" + department + "%");
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ddlDoctor.Items.Add(new ListItem(
                        dr["FullName"].ToString(),
                        dr["DoctorId"].ToString()
                    ));
                }
            }
        }

        protected void btnSearchAppointment_Click(object sender, EventArgs e)
        {
            divSchedule.Controls.Clear();

            if (ddlDoctor.SelectedIndex <= 0)
            {
                ShowAlert("Please select a doctor.", false);
                return;
            }

            if (string.IsNullOrEmpty(txtAppointmentDate.Text))
            {
                ShowAlert("Please select a date.", false);
                return;
            }

            LoadFromTimes();
           
        }

        protected void ddlFromTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadToTimes();
        }

        // Get booked slots, only consider appointments with Status='Booked'
        private HashSet<string> GetBookedSlots(DateTime selectedDate)
        {
            HashSet<string> booked = new HashSet<string>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT AppointmentTime, AppointmentEndTime 
                    FROM Appointments 
                    WHERE DoctorId=@D AND AppointmentDate=@Dt AND Status='Booked'", con);

                cmd.Parameters.AddWithValue("@D", ddlDoctor.SelectedValue);
                cmd.Parameters.AddWithValue("@Dt", selectedDate.ToString("yyyy-MM-dd"));

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string startStr = dr["AppointmentTime"]?.ToString();
                    string endStr = dr["AppointmentEndTime"]?.ToString();

                    if (string.IsNullOrWhiteSpace(startStr) || string.IsNullOrWhiteSpace(endStr))
                        continue;

                    DateTime start = DateTime.MinValue;
                    DateTime end = DateTime.MinValue;

                    bool isStartValid = DateTime.TryParseExact(startStr, "h:mm tt", null,
                                           System.Globalization.DateTimeStyles.None, out start);
                    bool isEndValid = DateTime.TryParseExact(endStr, "h:mm tt", null,
                                         System.Globalization.DateTimeStyles.None, out end);

                    if (!isStartValid || !isEndValid)
                        continue;

                    while (start < end)
                    {
                        booked.Add(start.ToString("HH:mm")); // store 24-hour
                        start = start.AddMinutes(30);
                    }
                }
            }

            return booked;
        }

        private bool IsBreakTime(DateTime t)
        {
            // Breaks: Morning 10-10:15, Lunch 13-14, Evening 16-16:15, Dinner 20-21
            return (t.TimeOfDay >= new TimeSpan(10, 0, 0) && t.TimeOfDay < new TimeSpan(10, 15, 0)) ||
                   (t.TimeOfDay >= new TimeSpan(13, 0, 0) && t.TimeOfDay < new TimeSpan(14, 0, 0)) ||
                   (t.TimeOfDay >= new TimeSpan(16, 0, 0) && t.TimeOfDay < new TimeSpan(16, 15, 0)) ||
                   (t.TimeOfDay >= new TimeSpan(20, 0, 0) && t.TimeOfDay < new TimeSpan(21, 0, 0));
        }

        protected void btnBook_Click(object sender, EventArgs e)
        {
            if (ddlDoctor.SelectedIndex <= 0)
            {
                ShowAlert("Please select doctor.", false);
                return;
            }

            if (ddlFromTime.SelectedIndex < 0 || ddlToTime.SelectedIndex < 0)
            {
                ShowAlert("Please select From/To time.", false);
                return;
            }

            // Parse From and To times from dropdown (HH:mm format)
            DateTime from = DateTime.ParseExact(ddlFromTime.SelectedValue, "HH:mm", null);
            DateTime to = DateTime.ParseExact(ddlToTime.SelectedValue, "HH:mm", null);

            int patientId = Session["UserId"] != null ? Convert.ToInt32(Session["UserId"]) : 0;
            string patientName = string.IsNullOrWhiteSpace(txtPatientName.Text) ? "Guest" : txtPatientName.Text.Trim();
            string patientMobile = txtPatientMobile.Text.Trim();
            string specialization = ddlDepartment.SelectedValue;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                // Check for conflicts with already booked appointments
                SqlCommand check = new SqlCommand(@"
            SELECT COUNT(*) 
            FROM Appointments
            WHERE DoctorId=@D AND AppointmentDate=@Dt AND Status='Booked'
            AND ((@FromTime >= AppointmentTime AND @FromTime < AppointmentEndTime)
                OR (@ToTime > AppointmentTime AND @ToTime <= AppointmentEndTime)
                OR (@FromTime <= AppointmentTime AND @ToTime >= AppointmentEndTime))", con);

                check.Parameters.AddWithValue("@D", ddlDoctor.SelectedValue);
                check.Parameters.AddWithValue("@Dt", txtAppointmentDate.Text);
                check.Parameters.AddWithValue("@FromTime", from.ToString("HH:mm")); // 24-hour format
                check.Parameters.AddWithValue("@ToTime", to.ToString("HH:mm"));     // 24-hour format

                int cnt = (int)check.ExecuteScalar();
                if (cnt > 0)
                {
                    ShowAlert("Selected time is already booked", false);
                    return;
                }

                // Insert appointment
                SqlCommand cmd = new SqlCommand(@"
            INSERT INTO Appointments
            (DoctorId, PatientId, PatientName, PatientMobile, AppointmentDate, AppointmentTime, AppointmentEndTime, Status, CreatedAt, Specialization, IsActive)
            VALUES (@D,@P,@Name,@Mobile,@Dt,@From,@To,'Booked',GETDATE(),@Spec,1)", con);

                cmd.Parameters.AddWithValue("@D", ddlDoctor.SelectedValue);
                cmd.Parameters.AddWithValue("@P", patientId);
                cmd.Parameters.AddWithValue("@Name", patientName);
                cmd.Parameters.AddWithValue("@Mobile", patientMobile);
                cmd.Parameters.AddWithValue("@Dt", txtAppointmentDate.Text);
                cmd.Parameters.AddWithValue("@From", from.ToString("h:mm tt")); // e.g., 6:00 PM
                cmd.Parameters.AddWithValue("@To", to.ToString("h:mm tt"));     // e.g., 6:30 PM
                cmd.Parameters.AddWithValue("@Spec", specialization);

                cmd.ExecuteNonQuery();
            }

            ShowAlert("Appointment booked successfully!", true);

            // Reload times and available slots
            LoadFromTimes();
            ShowAvailableSlots();
        }
        private void ShowAlert(string msg, bool success)
        {
            string icon = success ? "✔" : "✖";
            string color = success ? "success" : "danger";

            string script = $@"
                <script>
                    var div = document.createElement('div');
                    div.className = 'alert alert-{color} alert-dismissible fade show alert-custom';
                    div.role = 'alert';
                    div.innerHTML = '{icon} {msg}';
                    var button = document.createElement('button');
                    button.type='button';
                    button.className='btn-close';
                    button.setAttribute('data-bs-dismiss','alert');
                    div.appendChild(button);
                    document.body.appendChild(div);
                    setTimeout(function(){{ div.remove(); }}, 5000);
                </script>";

            ClientScript.RegisterStartupScript(this.GetType(), "alert", script, false);
        }

        private void LoadFromTimes()
        {
            ddlFromTime.Items.Clear();
            ddlToTime.Items.Clear();
            divSchedule.Controls.Clear(); // Clear previous schedule

            if (ddlDoctor.SelectedIndex <= 0 || string.IsNullOrEmpty(txtAppointmentDate.Text))
                return;

            int doctorId = Convert.ToInt32(ddlDoctor.SelectedValue);
            DateTime selectedDate = DateTime.Parse(txtAppointmentDate.Text);
            DateTime now = DateTime.Now;

            var shifts = GetDoctorShiftsForDate(doctorId, selectedDate);

            if (shifts.Count == 0)
            {
                ShowAlert("Selected doctor has no shifts on this date.", false);
                ShowDoctorUnavailable(); // Update schedule panel
                return;
            }

            HashSet<string> booked = GetBookedSlots(selectedDate);

            foreach (var shift in shifts)
            {
                DateTime start = selectedDate.Date.AddHours(shift.Item1.Hour).AddMinutes(shift.Item1.Minute);
                DateTime end = selectedDate.Date.AddHours(shift.Item2.Hour).AddMinutes(shift.Item2.Minute);

                while (start < end)
                {
                    if (!IsBreakTime(start) &&
                        !booked.Contains(start.ToString("HH:mm")) &&
                        (selectedDate > DateTime.Today || start > now))
                    {
                        ddlFromTime.Items.Add(new ListItem(start.ToString("h:mm tt"), start.ToString("HH:mm")));
                    }
                    start = start.AddMinutes(30);
                }
            }

            if (ddlFromTime.Items.Count > 0)
                ddlFromTime.SelectedIndex = 0;

            LoadToTimes();
            ShowAvailableSlots(shifts); // Pass shifts to update grid
        }
        private void ShowAvailableSlots(List<Tuple<DateTime, DateTime>> shifts = null)
        {
            divSchedule.Controls.Clear();

            DateTime selectedDate = DateTime.Parse(txtAppointmentDate.Text);
            DateTime now = DateTime.Now;

            if (shifts == null || shifts.Count == 0)
            {
                ShowDoctorUnavailable();
                return;
            }

            HashSet<string> booked = GetBookedSlots(selectedDate);

            foreach (var shift in shifts)
            {
                DateTime start = selectedDate.Date.AddHours(shift.Item1.Hour).AddMinutes(shift.Item1.Minute);
                DateTime end = selectedDate.Date.AddHours(shift.Item2.Hour).AddMinutes(shift.Item2.Minute);

                while (start < end)
                {
                    if (!IsBreakTime(start) &&
                        !booked.Contains(start.ToString("HH:mm")) &&
                        (selectedDate > DateTime.Today || start > now))
                    {
                        Panel slotCard = new Panel();
                        slotCard.CssClass = "slot-card slot-available";
                        string slotRange = start.ToString("h:mm tt") + " - " + start.AddMinutes(30).ToString("h:mm tt");
                        slotCard.Controls.Add(new Literal { Text = slotRange });
                        divSchedule.Controls.Add(slotCard);
                    }
                    start = start.AddMinutes(30);
                }
            }

            // If no available slots after checking booked & breaks
            if (divSchedule.Controls.Count == 0)
            {
                ShowDoctorUnavailable();
            }
        }
        private void ShowDoctorUnavailable()
{
    divSchedule.Controls.Clear();
    Panel slotCard = new Panel();
    slotCard.CssClass = "slot-card slot-unavailable";
    slotCard.Controls.Add(new Literal { Text = "Doctor not available on this date." });
    divSchedule.Controls.Add(slotCard);
}

        private void LoadToTimes()
        {
            ddlToTime.Items.Clear();
            if (ddlFromTime.SelectedIndex < 0) return;

            // Parse selected date and from time
            DateTime selectedDate = DateTime.Parse(txtAppointmentDate.Text);
            DateTime from = DateTime.ParseExact(ddlFromTime.SelectedValue, "HH:mm", null);
            DateTime now = DateTime.Now;

            // Calculate the next 30-minute slot
            DateTime nextSlot = from.AddMinutes(30);

            // Check if the next slot is valid
            if (!IsBreakTime(nextSlot) &&
                nextSlot > now &&
                !GetBookedSlots(selectedDate).Contains(nextSlot.ToString("HH:mm")))
            {
                ddlToTime.Items.Add(new ListItem(nextSlot.ToString("h:mm tt"), nextSlot.ToString("HH:mm")));
                ddlToTime.SelectedIndex = 0;
            }
        }
        private List<Tuple<DateTime, DateTime>> GetDoctorShiftsForDate(int doctorId, DateTime date)
        {
            var shifts = new List<Tuple<DateTime, DateTime>>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT ShiftStart, ShiftEnd FROM DoctorShifts WHERE DoctorId=@D AND ShiftDate=@Dt AND IsActive=1", con);

                cmd.Parameters.AddWithValue("@D", doctorId);
                cmd.Parameters.AddWithValue("@Dt", date.Date);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    // Read as TimeSpan
                    TimeSpan startTime = (TimeSpan)dr["ShiftStart"];
                    TimeSpan endTime = (TimeSpan)dr["ShiftEnd"];

                    // Combine with date to create DateTime
                    DateTime start = date.Date.Add(startTime);
                    DateTime end = date.Date.Add(endTime);

                    shifts.Add(Tuple.Create(start, end));
                }
            }

            return shifts;
        }
        private void ShowAvailableSlots()
        {
            divSchedule.Controls.Clear();

            DateTime selectedDate = DateTime.Parse(txtAppointmentDate.Text);
            DateTime now = DateTime.Now;

            DateTime start = selectedDate.AddHours(7);
            DateTime end = selectedDate.AddHours(22);

            HashSet<string> booked = GetBookedSlots(selectedDate);

            while (start < end)
            {
                if (!IsBreakTime(start) &&
                    !booked.Contains(start.ToString("HH:mm")) &&
                    (selectedDate > DateTime.Today || start > now))
                {
                    Panel slotCard = new Panel();
                    slotCard.CssClass = "slot-card slot-available";
                    string slotRange = start.ToString("h:mm tt") + " - " + start.AddMinutes(30).ToString("h:mm tt");
                    slotCard.Controls.Add(new Literal { Text = slotRange });
                    divSchedule.Controls.Add(slotCard);
                }
                start = start.AddMinutes(30);
            }
        }
    }
}
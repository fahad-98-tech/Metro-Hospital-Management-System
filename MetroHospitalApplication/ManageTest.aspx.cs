using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetroHospitalApplication
{
    public partial class ManageTest : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;
        int patientTestId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!int.TryParse(Request.QueryString["PatientTestId"], out patientTestId))
            {
                Response.Redirect("PatientAppointmentHistory.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadTestDetails();
                LoadAvailableSlots();
            }
        }

        private void LoadTestDetails()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(@"
            SELECT PT.PatientName, T.TestName, PT.TestDate, PT.TestTime
            FROM PatientTests PT
            INNER JOIN Tests T ON PT.TestId = T.TestId
            WHERE PT.PatientTestId = @Id", con);

                cmd.Parameters.AddWithValue("@Id", Request.QueryString["PatientTestId"]);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    lblPatientName.Text = dr["PatientName"].ToString();
                    lblTestName.Text = dr["TestName"].ToString();

                    // Format the date nicely
                    lblTestDate.Text = Convert.ToDateTime(dr["TestDate"]).ToString("dd-MMM-yyyy");

                    // Format the time nicely (h:mm tt)
                    TimeSpan testTime = (TimeSpan)dr["TestTime"];
                    DateTime dateTimeForDisplay = DateTime.Today.Add(testTime); // Convert TimeSpan to DateTime
                    lblTestTime.Text = dateTimeForDisplay.ToString("h:mm tt");   // e.g., "7:30 AM"
                }
            }
        }

        private void LoadAvailableSlots()
        {
            pnlSlots.Controls.Clear();

            DateTime testDate = Convert.ToDateTime(lblTestDate.Text);
            DateTime start = testDate.AddHours(7);   // 7:00 AM
            DateTime end = testDate.AddHours(12);    // 12:00 PM

            HashSet<string> bookedSlots = GetBookedSlots(testDate);

            while (start <= end)
            {
                string timeStrDisplay = start.ToString("h:mm tt");    // e.g., "7:30 AM"
                string timeStrCompare = start.ToString("HH:mm");      // e.g., "07:30"

                Button slotBtn = new Button();
                slotBtn.Text = timeStrDisplay;
                slotBtn.CssClass = "slot-card " + (bookedSlots.Contains(timeStrCompare) ? "slot-booked" : "slot-available");
                slotBtn.Attributes["onclick"] = bookedSlots.Contains(timeStrCompare)
                    ? "return false;"  // disable click for booked
                    : $"selectSlot(this, '{timeStrDisplay}');";

                pnlSlots.Controls.Add(slotBtn);

                start = start.AddMinutes(15); // 15 mins interval
            }
        }

        private HashSet<string> GetBookedSlots(DateTime testDate)
        {
            HashSet<string> booked = new HashSet<string>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
            SELECT TestTime FROM PatientTests
            WHERE TestDate=@Dt AND PatientTestId!=@Id AND Status='Booked'";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Dt", testDate.Date); // pass as DateTime
                cmd.Parameters.AddWithValue("@Id", patientTestId);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    // Use TimeSpan to string in HH:mm format
                    TimeSpan t = (TimeSpan)dr["TestTime"];
                    booked.Add(t.ToString(@"hh\:mm"));
                }
            }

            return booked;
        }
        protected void btnBookTest_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(hfSelectedTime.Value))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Please select a time slot!');", true);
                return;
            }

            string selectedTime = hfSelectedTime.Value;

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(@"
                    UPDATE PatientTests
                    SET TestTime=@Time, Status='Booked'
                    WHERE PatientTestId=@PatientTestId", con);

                cmd.Parameters.AddWithValue("@Time", selectedTime);
                cmd.Parameters.AddWithValue("@PatientTestId", patientTestId);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Test booking confirmed!'); window.location='PatientAppointmentHistory.aspx';", true);
        }
    }
}
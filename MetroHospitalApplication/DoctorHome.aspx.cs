using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MetroHospitalApplication
{
    public partial class DoctorHome : System.Web.UI.Page
    {
        string conStr = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["DoctorId"] == null)
                Response.Redirect("Login.aspx");

            if (!IsPostBack)
            {
                LoadDashboard();
                LoadTodaysAppointments();
            }
        }

        private void LoadDashboard()
        {
            int doctorId = Convert.ToInt32(Session["DoctorId"]);

            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();

                // Doctor Name
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT ISNULL(FullName,'Doctor') FROM Doctors WHERE DoctorId=@D", con))
                {
                    cmd.Parameters.AddWithValue("@D", doctorId);
                    lblDoctorName.Text = cmd.ExecuteScalar()?.ToString() ?? "Doctor";
                }

                // Today's Appointments Count
                lblToday.Text = GetCount(con, doctorId, "AppointmentDate = CAST(GETDATE() AS DATE)").ToString();

                // Status Counts
                lblPending.Text = GetCount(con, doctorId, "Status='Pending'").ToString();
                litPending.Text = lblPending.Text;

                lblApproved.Text = GetCount(con, doctorId, "Status='Approved'").ToString();
                litApproved.Text = lblApproved.Text;

                lblRejected.Text = GetCount(con, doctorId, "Status='Rejected'").ToString();
                litRejected.Text = lblRejected.Text;
            }
        }

        private int GetCount(SqlConnection con, int doctorId, string condition)
        {
            string query = $"SELECT COUNT(*) FROM Appointments WHERE DoctorId=@D AND IsActive=1 AND {condition}";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@D", doctorId);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private void LoadTodaysAppointments()
        {
            int doctorId = Convert.ToInt32(Session["DoctorId"]);

            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"
                    SELECT ISNULL(PatientName,'Guest') AS PatientName,
                           ISNULL(PatientMobile,'-') AS PatientMobile,
                           AppointmentTime,
                           AppointmentEndTime,
                           Specialization,
                           Status
                    FROM Appointments
                    WHERE DoctorId=@D AND AppointmentDate=CAST(GETDATE() AS DATE) AND IsActive=1
                    ORDER BY AppointmentTime";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@D", doctorId);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        rptAppointments.DataSource = dt;
                        rptAppointments.DataBind();
                    }
                }
            }
        }
    }
}

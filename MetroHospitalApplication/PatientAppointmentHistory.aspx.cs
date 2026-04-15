using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetroHospitalApplication
{
    public partial class PatientAppointmentHistory : System.Web.UI.Page
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
                BindAppointments();
                BindTests();
            }
        }

        // ✅ APPOINTMENTS (hide cancelled)
        private void BindAppointments()
        {
            int patientId = Convert.ToInt32(Session["UserId"]);

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT A.AppointmentId, D.FullName AS DoctorName,
                           A.AppointmentDate, A.AppointmentTime,
                           A.AppointmentEndTime, A.Status
                    FROM Appointments A
                    INNER JOIN Doctors D ON A.DoctorId = D.DoctorId
                    WHERE A.PatientId = @PatientId
                    AND A.Status != 'Cancelled'
                    ORDER BY A.AppointmentDate DESC", con);

                cmd.Parameters.AddWithValue("@PatientId", patientId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptAppointments.DataSource = dt;
                rptAppointments.DataBind();
            }
        }

        // ✅ TESTS (hide cancelled)
        private void BindTests()
        {
            int patientId = Convert.ToInt32(Session["UserId"]);

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(@"
            SELECT PatientTestId, TestDate, TestTime, Status
            FROM PatientTests
            WHERE PatientId = @PatientId
            AND Status != 'Cancelled'
            ORDER BY TestDate DESC", con);

                cmd.Parameters.AddWithValue("@PatientId", patientId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptTests.DataSource = dt;
                rptTests.DataBind();
            }
        }

        // ✅ APPOINTMENT ACTIONS
        protected void rptAppointments_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int appointmentId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "CancelAppt")
            {
                Response.Redirect("CancelFeedback.aspx?AppointmentId=" + appointmentId);
            }
            else if (e.CommandName == "Manage")
            {
                Response.Redirect("ManageAppointment.aspx?AppointmentId=" + appointmentId);
            }
        }

        // ✅ TEST ACTIONS
        protected void rptTests_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int testId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "ManageTest")
            {
                Response.Redirect("ManageTest.aspx?PatientTestId=" + testId);
            }
            else if (e.CommandName == "CancelTest")
            {
                Response.Redirect("CancelTestBooking.aspx?PatientTestId=" + testId);
            }
        }
    }
}
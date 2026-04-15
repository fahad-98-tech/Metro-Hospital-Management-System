using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace MetroHospitalApplication
{
    public partial class CancelTestBooking : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;
        int patientTestId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
                Response.Redirect("Login.aspx");

            if (!int.TryParse(Request.QueryString["PatientTestId"], out patientTestId))
                Response.Redirect("PatientAppointmentHistory.aspx");

            if (!IsPostBack)
                LoadTestDetails();
        }

        private void LoadTestDetails()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT PT.PatientName, T.TestName, PT.TestDate
                    FROM PatientTests PT
                    INNER JOIN Tests T ON PT.TestId = T.TestId
                    WHERE PT.PatientTestId = @Id", con);

                cmd.Parameters.AddWithValue("@Id", patientTestId);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    lblPatientName.Text = dr["PatientName"].ToString();
                    lblTestName.Text = dr["TestName"].ToString();
                    lblTestDate.Text = Convert.ToDateTime(dr["TestDate"]).ToString("dd-MMM-yyyy");
                }
            }
        }

        protected void btnCancelBooking_Click(object sender, EventArgs e)
        {
            string reason = txtReason.Text.Trim();
            string feedback = txtFeedback.Text.Trim();
            int patientId = Convert.ToInt32(Session["UserId"]);

            if (string.IsNullOrEmpty(reason))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                    "alert('Please enter cancel reason');", true);
                return;
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlTransaction tran = con.BeginTransaction();

                try
                {
                    // ✅ 1. Update status
                    SqlCommand cmdUpdate = new SqlCommand(@"
                        UPDATE PatientTests
                        SET Status = 'Cancelled'
                        WHERE PatientTestId = @Id", con, tran);

                    cmdUpdate.Parameters.AddWithValue("@Id", patientTestId);
                    cmdUpdate.ExecuteNonQuery();

                    // ✅ 2. Insert feedback
                    SqlCommand cmdInsert = new SqlCommand(@"
                        INSERT INTO TestCancelFeedback
                        (PatientTestId, PatientId, CancelReason, FeedbackText)
                        VALUES (@PatientTestId, @PatientId, @Reason, @Feedback)", con, tran);

                    cmdInsert.Parameters.AddWithValue("@PatientTestId", patientTestId);
                    cmdInsert.Parameters.AddWithValue("@PatientId", patientId);
                    cmdInsert.Parameters.AddWithValue("@Reason", reason);
                    cmdInsert.Parameters.AddWithValue("@Feedback", feedback);

                    cmdInsert.ExecuteNonQuery();

                    tran.Commit();

                    ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                        "alert('Test booking cancelled successfully!'); window.location='PatientAppointmentHistory.aspx';", true);
                }
                catch
                {
                    tran.Rollback();

                    ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                        "alert('Error occurred while cancelling. Try again.');", true);
                }
            }
        }
    }
}
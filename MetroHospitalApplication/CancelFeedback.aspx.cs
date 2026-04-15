using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetroHospitalApplication
{
    public partial class CancelFeedback : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int appointmentId = Convert.ToInt32(Request.QueryString["AppointmentId"]);
            int patientId = Convert.ToInt32(Session["UserId"]);

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                // ✅ Get DoctorId
                SqlCommand getDoc = new SqlCommand(
                    "SELECT DoctorId FROM Appointments WHERE AppointmentId=@Id", con);
                getDoc.Parameters.AddWithValue("@Id", appointmentId);

                int doctorId = Convert.ToInt32(getDoc.ExecuteScalar());

                // ✅ Insert Feedback
                SqlCommand cmd = new SqlCommand(@"
        INSERT INTO AppointmentFeedback(AppointmentId, PatientId, DoctorId, FeedbackReason)
        VALUES (@AId, @PId, @DId, @Reason)", con);

                cmd.Parameters.AddWithValue("@AId", appointmentId);
                cmd.Parameters.AddWithValue("@PId", patientId);
                cmd.Parameters.AddWithValue("@DId", doctorId);
                cmd.Parameters.AddWithValue("@Reason", rblReason.SelectedValue);

                cmd.ExecuteNonQuery();

                // ✅ UPDATE instead of DELETE
                SqlCommand update = new SqlCommand(@"
        UPDATE Appointments
        SET Status = 'Cancelled'
        WHERE AppointmentId = @Id", con);

                update.Parameters.AddWithValue("@Id", appointmentId);
                update.ExecuteNonQuery();
            }

            Response.Redirect("PatientAppointmentHistory.aspx");
        }
    }
}
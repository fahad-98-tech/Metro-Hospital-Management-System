using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace MetroHospitalApplication
{
    public partial class Appointments : System.Web.UI.Page
    {
        string conStr = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is logged in
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadAppointments();
            }
        }

        private void LoadAppointments()
        {
            int userId = Convert.ToInt32(Session["UserId"]); // Logged-in patient ID

            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"
                    SELECT AppointmentId, DoctorId, PatientName, PatientMobile, AppointmentDate, 
                           AppointmentTime, AppointmentEndTime, Status, CreatedAt, Specialization, PatientId
                    FROM Appointments
                    WHERE PatientId = @PatientId 
                      
                    ORDER BY AppointmentDate, AppointmentTime";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@PatientId", userId);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvAppointments.DataSource = dt;
                    gvAppointments.DataBind();
                }
            }
        }

        protected void gvAppointments_RowEditing(object sender, GridViewEditEventArgs e)
        {
            int appointmentId = Convert.ToInt32(gvAppointments.DataKeys[e.NewEditIndex].Value);
            Response.Redirect("EditAppointment.aspx?AppointmentId=" + appointmentId);
        }

        protected void gvAppointments_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int appointmentId = Convert.ToInt32(gvAppointments.DataKeys[e.RowIndex].Value);
            int userId = Convert.ToInt32(Session["UserId"]); // Ensure only logged-in patient

            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "DELETE FROM Appointments WHERE AppointmentId = @AppointmentId AND PatientId = @PatientId";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);
                    cmd.Parameters.AddWithValue("@PatientId", userId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            LoadAppointments(); // Refresh the GridView
        }
    }
}

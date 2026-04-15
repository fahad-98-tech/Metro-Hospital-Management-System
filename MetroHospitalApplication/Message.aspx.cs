using System;
using System.Data.SqlClient;

namespace MetroHospitalApplication
{
    public partial class Message : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Request.QueryString["NotificationId"] != null)
            {
                int notificationId = Convert.ToInt32(Request.QueryString["NotificationId"]);

                // Step 1: Mark as Read
                MarkNotificationAsRead(notificationId);

                // Step 2: Load Details
                LoadMessageDetails(notificationId);
            }
        }

        private void MarkNotificationAsRead(int notificationId)
        {
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                string query = "UPDATE Notifications SET IsRead = 1 WHERE NotificationId = @NotificationId";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@NotificationId", notificationId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void LoadMessageDetails(int notificationId)
        {
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                string query = @"
                    SELECT n.Message, n.CreatedOn,
                           d.FullName,
                           a.AppointmentDate,
                           a.AppointmentTime,
                           a.AppointmentEndTime
                    FROM Notifications n
                    LEFT JOIN Appointments a ON n.AppointmentId = a.AppointmentId
                    LEFT JOIN Doctors d ON a.DoctorId = d.DoctorId
                    WHERE n.NotificationId = @NotificationId";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@NotificationId", notificationId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblMessage.Text = reader["Message"].ToString();
                            lblCreated.Text = Convert.ToDateTime(reader["CreatedOn"]).ToString("dd-MMM-yyyy HH:mm");

                            lblDoctor.Text = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : "N/A";
                            lblDate.Text = reader["AppointmentDate"] != DBNull.Value
                                ? Convert.ToDateTime(reader["AppointmentDate"]).ToString("dd-MMM-yyyy")
                                : "N/A";

                            if (reader["AppointmentTime"] != DBNull.Value && reader["AppointmentEndTime"] != DBNull.Value)
                            {
                                lblTime.Text = reader["AppointmentTime"].ToString() + " - " + reader["AppointmentEndTime"].ToString();
                            }
                            else
                            {
                                lblTime.Text = "N/A";
                            }
                        }
                    }
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Services;

namespace MetroHospitalApplication
{
    public partial class Patient : System.Web.UI.MasterPage
    {
        public List<Notification> NotificationsList = new List<Notification>();
        public int NotificationCount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Session["UserId"] != null)
            {
                int patientId = Convert.ToInt32(Session["UserId"]);
                GenerateNotifications(patientId);
                LoadNotifications(patientId);
            }
        }
        private void GenerateNotifications(int patientId)
        {
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                // 1️⃣ Delete notifications for cancelled appointments
                string deleteQuery = @"
            DELETE FROM Notifications 
            WHERE PatientId=@PatientId 
            AND AppointmentId IN (
                SELECT AppointmentId FROM Appointments 
                WHERE PatientId=@PatientId AND Status='Cancelled'
            )";
                using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, con))
                {
                    deleteCmd.Parameters.AddWithValue("@PatientId", patientId);
                    deleteCmd.ExecuteNonQuery();
                }

                // 2️⃣ Load active appointments
                string query = @"
            SELECT a.AppointmentId, d.FullName, a.AppointmentDate, a.AppointmentTime, a.AppointmentEndTime
            FROM Appointments a
            INNER JOIN Doctors d ON a.DoctorId = d.DoctorId
            WHERE a.PatientId=@PatientId AND a.IsActive=1 AND a.Status!='Cancelled'";

                var appointments = new List<(int Id, string Doctor, DateTime Date, string Start, string End)>();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@PatientId", patientId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            appointments.Add((
                                Convert.ToInt32(reader["AppointmentId"]),
                                reader["FullName"].ToString(),
                                Convert.ToDateTime(reader["AppointmentDate"]),
                                reader["AppointmentTime"].ToString(),
                                reader["AppointmentEndTime"].ToString()
                            ));
                        }
                    }
                }

                // 3️⃣ Insert notifications for active appointments
                foreach (var appt in appointments)
                {
                    string message = $"Appointment with Dr. {appt.Doctor} on {appt.Date:dd-MMM-yyyy} from {appt.Start} to {appt.End}";
                    int isRead = 0;

                    string checkQuery = "SELECT COUNT(*) FROM Notifications WHERE PatientId=@PatientId AND AppointmentId=@AppointmentId";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, con))
                    {
                        checkCmd.Parameters.AddWithValue("@PatientId", patientId);
                        checkCmd.Parameters.AddWithValue("@AppointmentId", appt.Id);
                        int exists = (int)checkCmd.ExecuteScalar();
                        if (exists == 0)
                        {
                            string insertQuery = @"
                        INSERT INTO Notifications (PatientId, AppointmentId, Message, IsRead, CreatedOn) 
                        VALUES (@PatientId,@AppointmentId,@Message,@IsRead,GETDATE())";
                            using (SqlCommand insertCmd = new SqlCommand(insertQuery, con))
                            {
                                insertCmd.Parameters.AddWithValue("@PatientId", patientId);
                                insertCmd.Parameters.AddWithValue("@AppointmentId", appt.Id);
                                insertCmd.Parameters.AddWithValue("@Message", message);
                                insertCmd.Parameters.AddWithValue("@IsRead", isRead);
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
        }
        //private void GenerateNotifications(int patientId)
        //{
        //    string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        //    using (SqlConnection con = new SqlConnection(connStr))
        //    {
        //        con.Open();
        //        string query = @"
        //            SELECT a.AppointmentId, d.FullName, a.AppointmentDate, a.AppointmentTime, a.AppointmentEndTime
        //            FROM Appointments a
        //            INNER JOIN Doctors d ON a.DoctorId = d.DoctorId
        //            WHERE a.PatientId=@PatientId AND a.IsActive=1 AND a.Status!='Cancelled'";

        //        var appointments = new List<(int Id, string Doctor, DateTime Date, string Start, string End)>();
        //        using (SqlCommand cmd = new SqlCommand(query, con))
        //        {
        //            cmd.Parameters.AddWithValue("@PatientId", patientId);
        //            using (SqlDataReader reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    appointments.Add((
        //                        Convert.ToInt32(reader["AppointmentId"]),
        //                        reader["FullName"].ToString(),
        //                        Convert.ToDateTime(reader["AppointmentDate"]),
        //                        reader["AppointmentTime"].ToString(),
        //                        reader["AppointmentEndTime"].ToString()
        //                    ));
        //                }
        //            }
        //        }

        //        foreach (var appt in appointments)
        //        {
        //            string message = $"Appointment with Dr. {appt.Doctor} on {appt.Date:dd-MMM-yyyy} from {appt.Start} to {appt.End}";
        //            int isRead = 0;

        //            string checkQuery = "SELECT COUNT(*) FROM Notifications WHERE PatientId=@PatientId AND AppointmentId=@AppointmentId";
        //            using (SqlCommand checkCmd = new SqlCommand(checkQuery, con))
        //            {
        //                checkCmd.Parameters.AddWithValue("@PatientId", patientId);
        //                checkCmd.Parameters.AddWithValue("@AppointmentId", appt.Id);
        //                int exists = (int)checkCmd.ExecuteScalar();
        //                if (exists == 0)
        //                {
        //                    string insertQuery = "INSERT INTO Notifications (PatientId, AppointmentId, Message, IsRead, CreatedOn) VALUES (@PatientId,@AppointmentId,@Message,@IsRead,GETDATE())";
        //                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, con))
        //                    {
        //                        insertCmd.Parameters.AddWithValue("@PatientId", patientId);
        //                        insertCmd.Parameters.AddWithValue("@AppointmentId", appt.Id);
        //                        insertCmd.Parameters.AddWithValue("@Message", message);
        //                        insertCmd.Parameters.AddWithValue("@IsRead", isRead);
        //                        insertCmd.ExecuteNonQuery();
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        private void LoadNotifications(int patientId)
        {
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                string query = "SELECT NotificationId, AppointmentId, Message, IsRead, CreatedOn FROM Notifications WHERE PatientId=@PatientId ORDER BY CreatedOn DESC";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@PatientId", patientId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        NotificationsList.Clear();
                        NotificationCount = 0;
                        while (reader.Read())
                        {
                            var notif = new Notification
                            {
                                NotificationId = Convert.ToInt32(reader["NotificationId"]),
                                AppointmentId = reader["AppointmentId"] != DBNull.Value ? (int?)Convert.ToInt32(reader["AppointmentId"]) : null,
                                Message = reader["Message"].ToString(),
                                IsRead = Convert.ToBoolean(reader["IsRead"]),
                                CreatedOn = Convert.ToDateTime(reader["CreatedOn"])
                            };
                            if (!notif.IsRead) NotificationCount++;
                            NotificationsList.Add(notif);
                        }
                    }
                }
            }
        }

        [WebMethod]
        public static void MarkNotificationRead(int notificationId)
        {
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                string updateQuery = "UPDATE Notifications SET IsRead=1 WHERE NotificationId=@NotificationId";
                using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                {
                    cmd.Parameters.AddWithValue("@NotificationId", notificationId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public class Notification
        {
            public int NotificationId { get; set; }
            public int? AppointmentId { get; set; }
            public string Message { get; set; }
            public bool IsRead { get; set; }
            public DateTime CreatedOn { get; set; }
        }
    }
}
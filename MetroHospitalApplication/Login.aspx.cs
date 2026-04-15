using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;

namespace MetroHospitalApplication
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string email = txtEmail.Text.Trim();
                string passwordHash = HashPassword(txtPassword.Text.Trim());
                string connStr = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();

                    // =========================
                    // 🔹 CHECK USERS TABLE
                    // =========================
                    string queryUser = @"SELECT UserId, Role, FailedAttempts, LockUntil
                                         FROM Users
                                         WHERE Email=@Email AND IsActive=1";

                    using (SqlCommand cmd = new SqlCommand(queryUser, con))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr.Read())
                        {
                            int userId = Convert.ToInt32(dr["UserId"]);
                            string role = dr["Role"].ToString();

                            int failedAttempts = dr["FailedAttempts"] != DBNull.Value ? Convert.ToInt32(dr["FailedAttempts"]) : 0;
                            DateTime? lockUntil = dr["LockUntil"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dr["LockUntil"]) : null;

                            dr.Close();

                            // 🔴 Check lock
                            if (lockUntil.HasValue && DateTime.Now < lockUntil.Value)
                            {
                                ShowAlert("Too many failed attempts! Try again after 1 minute.");
                                return;
                            }

                            // 🔐 Check password
                            string queryPass = @"SELECT COUNT(*) FROM Users 
                                                 WHERE Email=@Email AND PasswordHash=@PasswordHash AND IsActive=1";

                            using (SqlCommand cmdPass = new SqlCommand(queryPass, con))
                            {
                                cmdPass.Parameters.AddWithValue("@Email", email);
                                cmdPass.Parameters.AddWithValue("@PasswordHash", passwordHash);

                                int match = (int)cmdPass.ExecuteScalar();

                                if (match == 1)
                                {
                                    // ✅ SUCCESS
                                    ResetFailedAttempts(con, "Users", userId);

                                    Session["UserId"] = userId;
                                    Session["UserRole"] = role;
                                    Session["UserEmail"] = email;

                                    if (role == "Doctor") Session["DoctorId"] = userId;
                                    if (role == "Admin") Session["AdminUserId"] = userId;

                                    if (role == "Admin")
                                        Response.Redirect("~/AdminHome.aspx");
                                    else if (role == "Doctor")
                                        Response.Redirect("~/DoctorHome.aspx");
                                    else
                                        Response.Redirect("~/PatientHome.aspx");

                                    return;
                                }
                                else
                                {
                                    // ❌ FAILED LOGIN
                                    failedAttempts++;

                                    string updateQuery = @"UPDATE Users 
                                                           SET FailedAttempts=@FailedAttempts,
                                                               LockUntil=@LockUntil
                                                           WHERE UserId=@UserId";

                                    using (SqlCommand cmdUpdate = new SqlCommand(updateQuery, con))
                                    {
                                        cmdUpdate.Parameters.AddWithValue("@FailedAttempts", failedAttempts);
                                        cmdUpdate.Parameters.AddWithValue("@UserId", userId);

                                        if (failedAttempts >= 3)
                                            cmdUpdate.Parameters.AddWithValue("@LockUntil", DateTime.Now.AddMinutes(1));
                                        else
                                            cmdUpdate.Parameters.AddWithValue("@LockUntil", DBNull.Value);

                                        cmdUpdate.ExecuteNonQuery();
                                    }

                                    if (failedAttempts >= 3)
                                        ShowAlert("Too many failed attempts! Login blocked for 1 minute.");
                                    else
                                        ShowAlert("Invalid email or password!");

                                    return;
                                }
                            }
                        }
                        else
                        {
                            dr.Close();
                        }
                    }

                    // =========================
                    // 🔹 CHECK DOCTORS TABLE
                    // =========================
                    string queryDoctor = @"SELECT DoctorId, FailedAttempts, LockUntil
                                           FROM Doctors
                                           WHERE Email=@Email AND IsActive=1";

                    using (SqlCommand cmdDoc = new SqlCommand(queryDoctor, con))
                    {
                        cmdDoc.Parameters.AddWithValue("@Email", email);
                        SqlDataReader drDoc = cmdDoc.ExecuteReader();

                        if (drDoc.Read())
                        {
                            int doctorId = Convert.ToInt32(drDoc["DoctorId"]);

                            int failedAttempts = drDoc["FailedAttempts"] != DBNull.Value ? Convert.ToInt32(drDoc["FailedAttempts"]) : 0;
                            DateTime? lockUntil = drDoc["LockUntil"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(drDoc["LockUntil"]) : null;

                            drDoc.Close();

                            if (lockUntil.HasValue && DateTime.Now < lockUntil.Value)
                            {
                                ShowAlert("Too many failed attempts! Try again after 1 minute.");
                                return;
                            }

                            string queryPassDoc = @"SELECT COUNT(*) FROM Doctors 
                                                   WHERE Email=@Email AND PasswordHash=@PasswordHash AND IsActive=1";

                            using (SqlCommand cmdPassDoc = new SqlCommand(queryPassDoc, con))
                            {
                                cmdPassDoc.Parameters.AddWithValue("@Email", email);
                                cmdPassDoc.Parameters.AddWithValue("@PasswordHash", passwordHash);

                                int match = (int)cmdPassDoc.ExecuteScalar();

                                if (match == 1)
                                {
                                    ResetFailedAttempts(con, "Doctors", doctorId);

                                    Session["UserId"] = doctorId;
                                    Session["UserRole"] = "Doctor";
                                    Session["DoctorId"] = doctorId;
                                    Session["UserEmail"] = email;

                                    Response.Redirect("~/DoctorHome.aspx");
                                    return;
                                }
                                else
                                {
                                    failedAttempts++;

                                    string updateQuery = @"UPDATE Doctors 
                                                           SET FailedAttempts=@FailedAttempts,
                                                               LockUntil=@LockUntil
                                                           WHERE DoctorId=@DoctorId";

                                    using (SqlCommand cmdUpdate = new SqlCommand(updateQuery, con))
                                    {
                                        cmdUpdate.Parameters.AddWithValue("@FailedAttempts", failedAttempts);
                                        cmdUpdate.Parameters.AddWithValue("@DoctorId", doctorId);

                                        if (failedAttempts >= 3)
                                            cmdUpdate.Parameters.AddWithValue("@LockUntil", DateTime.Now.AddMinutes(1));
                                        else
                                            cmdUpdate.Parameters.AddWithValue("@LockUntil", DBNull.Value);

                                        cmdUpdate.ExecuteNonQuery();
                                    }

                                    if (failedAttempts >= 3)
                                        ShowAlert("Too many failed attempts! Login blocked for 1 minute.");
                                    else
                                        ShowAlert("Invalid email or password!");

                                    return;
                                }
                            }
                        }
                        else
                        {
                            drDoc.Close();
                        }
                    }

                    // ❌ Not found anywhere
                    ShowAlert("Invalid email or password!");
                }
            }
            catch (Exception ex)
            {
                ShowAlert("Error: " + ex.Message);
            }
        }

        // 🔐 HASH PASSWORD
        private string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        // 🔄 RESET FAILED ATTEMPTS
        private void ResetFailedAttempts(SqlConnection con, string tableName, int id)
        {
            string idColumn = tableName == "Users" ? "UserId" : "DoctorId";

            string query = $"UPDATE {tableName} SET FailedAttempts=0, LockUntil=NULL WHERE {idColumn}=@Id";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }

        // 🔔 ALERT POPUP FUNCTION
        private void ShowAlert(string message)
        {
            string script = $"alert('{message}');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", script, true);
        }
    }
}
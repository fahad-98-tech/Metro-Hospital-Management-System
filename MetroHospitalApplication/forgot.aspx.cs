using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;

namespace MetroHospitalApplication
{
    public partial class Forgot : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMessage.Text = "";
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                lblMessage.Text = "All fields are required!";
                return;
            }

            if (newPassword != confirmPassword)
            {
                lblMessage.Text = "Passwords do not match!";
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;
            string passwordHash = HashPassword(newPassword);

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                // 1️⃣ Check USERS table
                string checkUser = "SELECT UserId FROM Users WHERE Email=@Email AND IsActive=1";
                using (SqlCommand cmd = new SqlCommand(checkUser, con))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        int userId = Convert.ToInt32(result);

                        string updateUser = "UPDATE Users SET PasswordHash=@PasswordHash WHERE UserId=@UserId";
                        using (SqlCommand updateCmd = new SqlCommand(updateUser, con))
                        {
                            updateCmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                            updateCmd.Parameters.AddWithValue("@UserId", userId);
                            updateCmd.ExecuteNonQuery();
                        }

                        lblMessage.CssClass = "fw-semibold text-success";
                        lblMessage.Text = "Password updated successfully!";
                        return;
                    }
                }

                // 2️⃣ If not in Users → check DOCTORS table
                string checkDoctor = "SELECT DoctorId FROM Doctors WHERE Email=@Email AND IsActive=1";
                using (SqlCommand cmdDoctor = new SqlCommand(checkDoctor, con))
                {
                    cmdDoctor.Parameters.AddWithValue("@Email", email);
                    object resultDoc = cmdDoctor.ExecuteScalar();

                    if (resultDoc != null)
                    {
                        int doctorId = Convert.ToInt32(resultDoc);

                        string updateDoctor = "UPDATE Doctors SET PasswordHash=@PasswordHash WHERE DoctorId=@DoctorId";
                        using (SqlCommand updateCmd = new SqlCommand(updateDoctor, con))
                        {
                            updateCmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                            updateCmd.Parameters.AddWithValue("@DoctorId", doctorId);
                            updateCmd.ExecuteNonQuery();
                        }

                        lblMessage.CssClass = "fw-semibold text-success";
                        lblMessage.Text = "Doctor password updated successfully!";
                        return;
                    }
                }

                // 3️⃣ If email not found
                lblMessage.Text = "Email not registered!";
            }
        }

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
    }
}
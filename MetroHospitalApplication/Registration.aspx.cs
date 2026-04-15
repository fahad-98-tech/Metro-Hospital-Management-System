using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace MetroHospitalApplication
{
    public partial class Registration : System.Web.UI.Page
    {
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                string fullName = txtFullName.Text.Trim();
                string email = txtEmail.Text.Trim();
                string mobile = txtPhone.Text.Trim();
                string gender = ddlGender.SelectedValue;
                string password = txtPassword.Text;
                string confirmPassword = txtConfirmPassword.Text;

                // ✅ Required fields
                if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) ||
                    string.IsNullOrEmpty(mobile) || string.IsNullOrEmpty(gender) ||
                    string.IsNullOrEmpty(txtDOB.Text))
                {
                    ShowAlert("❌ Please fill all fields!");
                    return;
                }

                // ✅ Email format
                if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    ShowAlert("❌ Invalid email format!");
                    return;
                }

                // ✅ Kuwait mobile validation (8 digits starting with 5,6,9)
                if (!Regex.IsMatch(mobile, @"^[569]\d{7}$"))
                {
                    ShowAlert("❌ Invalid Kuwait mobile number! (Must be 8 digits & start with 5,6,9)");
                    return;
                }

                // ✅ Password match
                if (password != confirmPassword)
                {
                    ShowAlert("❌ Passwords do not match!");
                    return;
                }

                // ✅ Strong password
                if (!IsStrongPassword(password))
                {
                    ShowAlert("❌ Password must be at least 8 characters with uppercase, lowercase, number & special character!");
                    return;
                }

                string connStr = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();

                    // ✅ Check email uniqueness
                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE Email=@Email";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, con))
                    {
                        checkCmd.Parameters.AddWithValue("@Email", email);
                        int count = (int)checkCmd.ExecuteScalar();

                        if (count > 0)
                        {
                            ShowAlert("❌ Email already exists!");
                            return;
                        }
                    }

                    // ✅ Insert user
                    string query = @"INSERT INTO Users
                        (FullName, Email, MobileNumber, Gender, DateOfBirth, PasswordHash, Role)
                        VALUES
                        (@FullName, @Email, @Mobile, @Gender, @DOB, @PasswordHash, @Role)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@FullName", fullName);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Mobile", mobile);
                        cmd.Parameters.AddWithValue("@Gender", gender);
                        cmd.Parameters.AddWithValue("@DOB", DateTime.Parse(txtDOB.Text));
                        cmd.Parameters.AddWithValue("@PasswordHash", HashPassword(password));
                        cmd.Parameters.AddWithValue("@Role", "Patient");

                        cmd.ExecuteNonQuery();
                    }
                }

                // ✅ Success alert
                ClientScript.RegisterStartupScript(
                    this.GetType(),
                    "alert",
                    "alert('✅ Registration Successful! Please login.'); window.location='Login.aspx';",
                    true
                );
            }
            catch (Exception ex)
            {
                ShowAlert("❌ Error: " + ex.Message);
            }
        }

        // ✅ Strong password checker
        private bool IsStrongPassword(string password)
        {
            return Regex.IsMatch(password,
                @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$");
        }

        // ✅ Hash password
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

        // ✅ Alert helper
        private void ShowAlert(string message)
        {
            ClientScript.RegisterStartupScript(
                this.GetType(),
                "alert",
                $"alert('{message}');",
                true
            );
        }
    }
}
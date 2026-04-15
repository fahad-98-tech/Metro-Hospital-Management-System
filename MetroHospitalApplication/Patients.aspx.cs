using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetroHospitalApplication
{
    public partial class Patients : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPatients();
            }
        }

        private void LoadPatients()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT UserId, FullName, Email, MobileNumber, Gender, DateOfBirth FROM Users WHERE Role='Patient'", con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                dt.Columns.Add("Age", typeof(int));

                foreach (DataRow row in dt.Rows)
                {
                    if (row["DateOfBirth"] != DBNull.Value)
                    {
                        DateTime dob = Convert.ToDateTime(row["DateOfBirth"]);
                        int age = DateTime.Today.Year - dob.Year;
                        if (dob > DateTime.Today.AddYears(-age)) age--;
                        row["Age"] = age;
                    }
                    else
                    {
                        row["Age"] = 0;
                    }
                }

                gvPatients.DataSource = dt;
                gvPatients.DataBind();
            }
        }

        // ADD NEW PATIENT
        protected void btnAddPatient_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(
                    @"INSERT INTO Users
                    (FullName, Email, MobileNumber, Gender, DateOfBirth, PasswordHash, Role, IsActive, CreatedDate)
                    VALUES
                    (@FullName, @Email, @Mobile, @Gender, @DOB, @PasswordHash, 'Patient', 1, @CreatedDate)", con);

                cmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text.Trim());
                cmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedValue);
                cmd.Parameters.AddWithValue("@DOB", Convert.ToDateTime(txtDOB.Text));
                cmd.Parameters.AddWithValue("@PasswordHash", txtPassword.Text.Trim());
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            ClearFields();
            LoadPatients();
        }

        private void ClearFields()
        {
            txtFullName.Text = "";
            txtEmail.Text = "";
            txtPassword.Text = "";
            txtMobile.Text = "";
            ddlGender.SelectedIndex = 0;
            txtDOB.Text = "";
        }

        // EDIT
        protected void gvPatients_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvPatients.EditIndex = e.NewEditIndex;
            LoadPatients();
        }

        protected void gvPatients_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvPatients.EditIndex = -1;
            LoadPatients();
        }

        // UPDATE
        protected void gvPatients_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int userId = Convert.ToInt32(gvPatients.DataKeys[e.RowIndex].Value);

            string fullName = ((TextBox)gvPatients.Rows[e.RowIndex].Cells[1].Controls[0]).Text;
            string mobile = ((TextBox)gvPatients.Rows[e.RowIndex].Cells[3].Controls[0]).Text;

            // Get gender from dropdown
            DropDownList ddlGenderEdit = (DropDownList)gvPatients.Rows[e.RowIndex].FindControl("ddlGenderEdit");
            string gender = ddlGenderEdit.SelectedValue;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(
                    @"UPDATE Users
                      SET FullName=@FullName,
                          MobileNumber=@Mobile,
                          Gender=@Gender
                      WHERE UserId=@UserId", con);

                cmd.Parameters.AddWithValue("@FullName", fullName);
                cmd.Parameters.AddWithValue("@Mobile", mobile);
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@UserId", userId);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            gvPatients.EditIndex = -1;
            LoadPatients();
        }

        // DELETE
        protected void gvPatients_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int userId = Convert.ToInt32(gvPatients.DataKeys[e.RowIndex].Value);

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Users WHERE UserId=@UserId", con);

                cmd.Parameters.AddWithValue("@UserId", userId);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            LoadPatients();
        }

        // Bind dropdown in edit mode
        protected void gvPatients_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex == gvPatients.EditIndex)
            {
                DropDownList ddlGenderEdit = (DropDownList)e.Row.FindControl("ddlGenderEdit");
                if (ddlGenderEdit != null)
                {
                    string gender = DataBinder.Eval(e.Row.DataItem, "Gender").ToString();
                    ddlGenderEdit.SelectedValue = gender;
                }
            }
        }
    }
}
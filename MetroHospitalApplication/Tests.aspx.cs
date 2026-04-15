using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace MetroHospitalApplication
{
    public partial class Tests : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTests();
            }
        }

        private void LoadTests()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Tests", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvTests.DataSource = dt;
                gvTests.DataBind();
            }
        }

        // ADD
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTestName.Text)) return;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Tests (TestName, Department, Price, IsActive) VALUES (@Name, @Dept, @Price, 1)", con);

                cmd.Parameters.AddWithValue("@Name", txtTestName.Text.Trim());
                cmd.Parameters.AddWithValue("@Dept", txtDepartment.Text.Trim());
                cmd.Parameters.AddWithValue("@Price", txtPrice.Text.Trim());

                cmd.ExecuteNonQuery();
            }

            txtTestName.Text = "";
            txtDepartment.Text = "";
            txtPrice.Text = "";

            LoadTests();
        }

        // EDIT
        protected void gvTests_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvTests.EditIndex = e.NewEditIndex;
            LoadTests();
        }

        protected void gvTests_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvTests.EditIndex = -1;
            LoadTests();
        }

        protected void gvTests_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(gvTests.DataKeys[e.RowIndex].Value);

            GridViewRow row = gvTests.Rows[e.RowIndex];

            string name = ((TextBox)row.FindControl("txtEditName")).Text;
            string dept = ((TextBox)row.FindControl("txtEditDept")).Text;
            string price = ((TextBox)row.FindControl("txtEditPrice")).Text;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(
                    "UPDATE Tests SET TestName=@Name, Department=@Dept, Price=@Price WHERE TestId=@Id", con);

                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Dept", dept);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@Id", id);

                cmd.ExecuteNonQuery();
            }

            gvTests.EditIndex = -1;
            LoadTests();
        }

        // DELETE
        protected void gvTests_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvTests.DataKeys[e.RowIndex].Value);

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("DELETE FROM Tests WHERE TestId=@Id", con);
                cmd.Parameters.AddWithValue("@Id", id);

                cmd.ExecuteNonQuery();
            }

            LoadTests();
        }
    }
}
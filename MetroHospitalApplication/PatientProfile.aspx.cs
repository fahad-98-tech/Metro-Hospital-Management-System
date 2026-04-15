using System;
using System.Data.SqlClient;
using System.Configuration;

namespace MetroHospitalApplication
{
    public partial class PatientProfile : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadProfile();
            }
        }

        void LoadProfile()
        {
            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT FullName,Email,MobileNumber,Gender,DateOfBirth,Role FROM Users WHERE UserId=@id", con);
            cmd.Parameters.AddWithValue("@id", Session["UserId"]);

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                txtFullName.Text = dr["FullName"].ToString();
                txtEmail.Text = dr["Email"].ToString();
                txtMobile.Text = dr["MobileNumber"].ToString();
                ddlGender.SelectedValue = dr["Gender"].ToString();
                txtDOB.Text = Convert.ToDateTime(dr["DateOfBirth"]).ToString("yyyy-MM-dd");
                txtRole.Text = dr["Role"].ToString();
            }

            dr.Close();
            con.Close();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            con.Open();

            SqlCommand cmd = new SqlCommand(@"UPDATE Users 
            SET FullName=@name,
                Email=@email,
                MobileNumber=@mobile,
                Gender=@gender,
                DateOfBirth=@dob
            WHERE UserId=@id", con);

            cmd.Parameters.AddWithValue("@name", txtFullName.Text);
            cmd.Parameters.AddWithValue("@email", txtEmail.Text);
            cmd.Parameters.AddWithValue("@mobile", txtMobile.Text);
            cmd.Parameters.AddWithValue("@gender", ddlGender.SelectedValue);
            cmd.Parameters.AddWithValue("@dob", txtDOB.Text);
            cmd.Parameters.AddWithValue("@id", Session["UserId"]);

            cmd.ExecuteNonQuery();

            con.Close();

            lblMsg.Text = "Profile Updated Successfully!";
        }
    }
}
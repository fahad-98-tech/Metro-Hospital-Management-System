using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace MetroHospitalApplication
{
    public partial class Packages : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindPackages();
        }

        private void BindPackages(string searchTerm = "")
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                    SELECT * FROM Packages
                    WHERE IsActive = 1
                    AND PackageName LIKE @Search
                    AND StartDate IS NOT NULL
                    AND EndDate IS NOT NULL
                    AND EndDate >= CAST(GETDATE() AS DATE)
                    ORDER BY IsPopular DESC, PackageName";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@Search", "%" + searchTerm + "%");

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvPackages.DataSource = dt;
                gvPackages.DataBind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindPackages(txtSearch.Text.Trim());
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            BindPackages();
        }
    }
}
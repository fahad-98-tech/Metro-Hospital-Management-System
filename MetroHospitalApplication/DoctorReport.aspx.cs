using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MetroHospitalApplication
{
    public partial class DoctorReport : System.Web.UI.Page
    {
        string conStr = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSpecialization();
                LoadDoctors();
            }
        }

        private void LoadSpecialization()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT Specialization FROM Doctors WHERE Specialization IS NOT NULL", con);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                HashSet<string> specializations = new HashSet<string>();

                while (reader.Read())
                {
                    string value = reader["Specialization"].ToString();

                    // Split if comma separated
                    string[] splitValues = value.Split(',');

                    foreach (string item in splitValues)
                    {
                        specializations.Add(item.Trim());
                    }
                }

                ddlSpecialization.DataSource = specializations.ToList();
                ddlSpecialization.DataBind();
            }

            ddlSpecialization.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", ""));
        }

        private void LoadDoctors()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("sp_GetDoctorReport", con);
                cmd.CommandType = CommandType.StoredProcedure;

                if (!string.IsNullOrEmpty(ddlSpecialization.SelectedValue))
                    cmd.Parameters.AddWithValue("@Specialization", ddlSpecialization.SelectedValue);

                if (!string.IsNullOrEmpty(txtFromDate.Text))
                    cmd.Parameters.AddWithValue("@FromDate", txtFromDate.Text);

                if (!string.IsNullOrEmpty(txtToDate.Text))
                    cmd.Parameters.AddWithValue("@ToDate", txtToDate.Text);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvDoctors.DataSource = dt;
                gvDoctors.DataBind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadDoctors();
        }

        protected void gvDoctors_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            gvDoctors.PageIndex = e.NewPageIndex;
            LoadDoctors();
        }
    }
}

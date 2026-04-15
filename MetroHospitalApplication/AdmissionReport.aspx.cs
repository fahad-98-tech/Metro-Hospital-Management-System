using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace MetroHospitalApplication
{
    public partial class AdmissionReport : System.Web.UI.Page
    {
        string conStr = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAdmissions();
            }
        }

        private void LoadAdmissions()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("sp_GetAdmissionReport", con);
                cmd.CommandType = CommandType.StoredProcedure;

                if (!string.IsNullOrEmpty(txtPatientName.Text))
                    cmd.Parameters.AddWithValue("@PatientName", txtPatientName.Text.Trim());

                if (!string.IsNullOrEmpty(txtFromDate.Text))
                    cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(txtFromDate.Text));

                if (!string.IsNullOrEmpty(txtToDate.Text))
                    cmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(txtToDate.Text));

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvAdmissions.DataSource = dt;
                gvAdmissions.DataBind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadAdmissions();
        }

        protected void gvAdmissions_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAdmissions.PageIndex = e.NewPageIndex;
            LoadAdmissions();
        }
    }
}

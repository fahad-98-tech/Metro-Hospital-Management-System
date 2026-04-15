using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetroHospitalApplication
{
    public partial class DepartmentReport : System.Web.UI.Page
    {
        string conStr = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDepartments();
                LoadReport();
            }
        }

        private void LoadDepartments()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT DISTINCT Specialization FROM Doctors WHERE IsActive=1", con);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                ddlSpecialization.Items.Clear();

                while (reader.Read())
                {
                    ddlSpecialization.Items.Add(reader["Specialization"].ToString());
                }
            }

            ddlSpecialization.Items.Insert(0, new ListItem("All", ""));
        }

        private void LoadReport()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"
                    SELECT 
                        d.Specialization,
                        COUNT(d.DoctorId) AS TotalDoctors,
                        STRING_AGG(d.FullName, ', ') AS DoctorNames,
                        MIN(d.CreatedDate) AS FirstDoctorAdded,
                        MAX(d.CreatedDate) AS LastDoctorAdded
                    FROM Doctors d
                    WHERE d.IsActive = 1
                    AND (@Specialization = '' OR d.Specialization = @Specialization)
                    AND (@FromDate IS NULL OR d.CreatedDate >= @FromDate)
                    AND (@ToDate IS NULL OR d.CreatedDate <= @ToDate)
                    GROUP BY d.Specialization
                    ORDER BY d.Specialization;
                ";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Specialization",
                    string.IsNullOrEmpty(ddlSpecialization.SelectedValue) ? "" : ddlSpecialization.SelectedValue);

                cmd.Parameters.AddWithValue("@FromDate",
                    string.IsNullOrEmpty(txtFromDate.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtFromDate.Text));

                cmd.Parameters.AddWithValue("@ToDate",
                    string.IsNullOrEmpty(txtToDate.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtToDate.Text));

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvDepartments.DataSource = dt;
                gvDepartments.DataBind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadReport();
        }

        protected void gvDepartments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDepartments.PageIndex = e.NewPageIndex;
            LoadReport();
        }

        // Export to Excel
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            LoadReport(); // reload data

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=DepartmentReport.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "";

            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            gvDepartments.AllowPaging = false;
            gvDepartments.DataBind();
            gvDepartments.RenderControl(hw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            // Required for exporting GridView to Excel
        }
    }
}
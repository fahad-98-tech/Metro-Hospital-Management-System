using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetroHospitalApplication
{
    public partial class PatientReport : System.Web.UI.Page
    {
        string conString = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPatients();
            }
        }

        private void LoadPatients()
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                string query = @"
                    SELECT 
                        U.UserId AS PatientID,
                        U.FullName,
                        U.Gender,
                        DATEDIFF(YEAR, U.DateOfBirth, GETDATE()) AS Age,
                        U.MobileNumber,
                        ISNULL(A.AppointmentDate, U.CreatedDate) AS AppointmentDate,
                        ISNULL(DO.FullName,'N/A') AS DoctorName,
                        ISNULL(A.Specialization,'N/A') AS Specialization,
                        ISNULL(A.Status, 'Not Found') AS Status
                    FROM Users U
                    LEFT JOIN 
                        (SELECT * FROM Appointments) A ON U.UserId = A.PatientId
                    LEFT JOIN Doctors DO ON A.DoctorId = DO.DoctorId
                    WHERE U.Role = 'Patient'
                        AND (@PatientName IS NULL OR U.FullName LIKE '%' + @PatientName + '%')
                        AND (@Gender IS NULL OR U.Gender = @Gender)
                        AND (@FromDate IS NULL OR A.AppointmentDate >= @FromDate)
                        AND (@ToDate IS NULL OR A.AppointmentDate <= @ToDate)
                        AND (@Status IS NULL OR ISNULL(A.Status,'Not Found') = @Status)
                    ORDER BY U.FullName, A.AppointmentDate DESC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@PatientName",
                        string.IsNullOrEmpty(txtPatientName.Text) ? (object)DBNull.Value : txtPatientName.Text);

                    cmd.Parameters.AddWithValue("@Gender",
                        string.IsNullOrEmpty(ddlGender.SelectedValue) ? (object)DBNull.Value : ddlGender.SelectedValue);

                    cmd.Parameters.AddWithValue("@FromDate",
                        string.IsNullOrEmpty(txtFromDate.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtFromDate.Text));

                    cmd.Parameters.AddWithValue("@ToDate",
                        string.IsNullOrEmpty(txtToDate.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtToDate.Text));

                    cmd.Parameters.AddWithValue("@Status",
                        string.IsNullOrEmpty(ddlStatus.SelectedValue) ? (object)DBNull.Value : ddlStatus.SelectedValue);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvPatients.DataSource = dt;
                    gvPatients.DataBind();
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadPatients();
        }

        protected void gvPatients_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPatients.PageIndex = e.NewPageIndex;
            LoadPatients();
        }

        protected void gvPatients_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                if (lblStatus != null)
                {
                    string status = lblStatus.Text;
                    switch (status)
                    {
                        case "Done":
                            lblStatus.CssClass = "status-badge status-done";
                            break;
                        case "Approved":
                            lblStatus.CssClass = "status-badge status-approved";
                            break;
                        case "Booked":
                            lblStatus.CssClass = "status-badge status-booked";
                            break;
                        case "Not Found":
                            lblStatus.CssClass = "status-badge status-notfound";
                            break;
                        default:
                            lblStatus.CssClass = "status-badge";
                            break;
                    }
                }
            }
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            LoadPatients();

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=PatientReport.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "";

            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            gvPatients.AllowPaging = false;
            gvPatients.DataBind();
            gvPatients.RenderControl(hw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            // Required for export
        }
    }
}
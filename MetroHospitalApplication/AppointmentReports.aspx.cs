using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace MetroHospitalApplication
{
    public partial class AppointmentReports : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;
        int appointmentId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["aid"] == null)
            {
                Response.Write("AppointmentId missing!");
                Response.End();
            }

            appointmentId = Convert.ToInt32(Request.QueryString["aid"]);

            if (!IsPostBack)
            {
                lblAppointment.Text = $"Appointment ID: {appointmentId}";
                LoadReports();
            }
        }

        void LoadReports()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"SELECT ReportId, ReportType, FilePath, UploadedAt
                                 FROM AppointmentReports
                                 WHERE AppointmentId=@AppointmentId
                                 ORDER BY UploadedAt DESC";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvReports.DataSource = dt;
                gvReports.DataBind();
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtReportType.Text.Trim()))
            {
                lblMsg.Text = "⚠ Please enter the report type.";
                return;
            }

            if (!fuReport.HasFile)
            {
                lblMsg.Text = "⚠ Please select a file to upload.";
                return;
            }

            string fileName = Path.GetFileName(fuReport.PostedFile.FileName);
            string folderPath = Server.MapPath("~/Uploads/Reports/");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, fileName);
            fuReport.SaveAs(filePath);

            // Save in database
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(@"INSERT INTO AppointmentReports
                                                  (AppointmentId, ReportType, FilePath)
                                                  VALUES (@AppointmentId, @ReportType, @FilePath)", con);
                cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);
                cmd.Parameters.AddWithValue("@ReportType", txtReportType.Text.Trim());
                cmd.Parameters.AddWithValue("@FilePath", "~/Uploads/Reports/" + fileName);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            lblMsg.Text = "✔ Report uploaded successfully!";
            txtReportType.Text = "";
            LoadReports();
        }

        protected void gvReports_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteReport")
            {
                int reportId = Convert.ToInt32(e.CommandArgument);

                // Delete file from server
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string getFileQuery = "SELECT FilePath FROM AppointmentReports WHERE ReportId=@Id";
                    SqlCommand cmdGet = new SqlCommand(getFileQuery, con);
                    cmdGet.Parameters.AddWithValue("@Id", reportId);
                    con.Open();
                    string filePath = cmdGet.ExecuteScalar()?.ToString();
                    con.Close();

                    if (!string.IsNullOrEmpty(filePath))
                    {
                        string serverPath = Server.MapPath(filePath);
                        if (System.IO.File.Exists(serverPath))
                        {
                            System.IO.File.Delete(serverPath);
                        }
                    }

                    // Delete from database
                    SqlCommand cmdDel = new SqlCommand("DELETE FROM AppointmentReports WHERE ReportId=@Id", con);
                    cmdDel.Parameters.AddWithValue("@Id", reportId);
                    con.Open();
                    cmdDel.ExecuteNonQuery();
                    con.Close();
                }

                lblMsg.Text = "✔ Report deleted successfully!";
                LoadReports();
            }
        }
    }
}

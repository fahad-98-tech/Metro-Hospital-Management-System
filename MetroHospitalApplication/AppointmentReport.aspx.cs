using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetroHospitalApplication
{
    public partial class AppointmentReport : System.Web.UI.Page
    {
        string conStr = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDoctors();
                LoadAppointments();
            }
        }

        private void LoadDoctors()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT DoctorId, FullName FROM Doctors", con);
                con.Open();

                ddlDoctor.DataSource = cmd.ExecuteReader();
                ddlDoctor.DataTextField = "FullName";
                ddlDoctor.DataValueField = "DoctorId";
                ddlDoctor.DataBind();
            }

            ddlDoctor.Items.Insert(0, new ListItem("All", ""));
        }

        private void LoadAppointments()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("sp_GetAppointmentReport", con);
                cmd.CommandType = CommandType.StoredProcedure;

                if (!string.IsNullOrEmpty(ddlDoctor.SelectedValue))
                    cmd.Parameters.AddWithValue("@DoctorId", ddlDoctor.SelectedValue);

                if (!string.IsNullOrEmpty(ddlStatus.SelectedValue))
                    cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue);

                if (!string.IsNullOrEmpty(txtFromDate.Text))
                    cmd.Parameters.AddWithValue("@FromDate", txtFromDate.Text);

                if (!string.IsNullOrEmpty(txtToDate.Text))
                    cmd.Parameters.AddWithValue("@ToDate", txtToDate.Text);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvAppointments.DataSource = dt;
                gvAppointments.DataBind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadAppointments();
        }

        protected void gvAppointments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAppointments.PageIndex = e.NewPageIndex;
            LoadAppointments();
        }

        protected void gvAppointments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                if (lblStatus != null)
                {
                    string status = lblStatus.Text;

                    switch (status)
                    {
                        case "Booked":
                            lblStatus.CssClass = "status-badge status-booked";
                            break;
                        case "Completed":
                            lblStatus.CssClass = "status-badge status-completed";
                            break;
                        case "Cancelled":
                            lblStatus.CssClass = "status-badge status-cancelled";
                            break;
                        case "Approved":
                            lblStatus.CssClass = "status-badge status-approved";
                            break;
                        default:
                            lblStatus.CssClass = "status-badge status-other";
                            break;
                    }
                }
            }
        }
    }
}
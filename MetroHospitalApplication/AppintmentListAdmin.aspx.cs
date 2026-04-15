using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace MetroHospitalApplication
{
    public partial class AppintmentListAdmin : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDoctors();
                LoadSpecializations();
                LoadAppointments();
            }
        }

        // LOAD DOCTORS DROPDOWN
        private void LoadDoctors()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT DoctorId, FullName FROM Doctors", con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlDoctor.DataSource = dt;
                ddlDoctor.DataTextField = "FullName";
                ddlDoctor.DataValueField = "DoctorId";
                ddlDoctor.DataBind();

                ddlDoctor.Items.Insert(0, new ListItem("All", "0"));
            }
        }

        // LOAD SPECIALIZATION DROPDOWN
        private void LoadSpecializations()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT DISTINCT Specialization FROM Appointments", con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlSpecialization.DataSource = dt;
                ddlSpecialization.DataTextField = "Specialization";
                ddlSpecialization.DataValueField = "Specialization";
                ddlSpecialization.DataBind();

                ddlSpecialization.Items.Insert(0, new ListItem("All", "All"));
            }
        }

        protected void ddlDoctor_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAppointments();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadAppointments();
        }

        // LOAD APPOINTMENTS WITH JOIN
        private void LoadAppointments()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = @"
        SELECT a.*,
               d.FullName AS DoctorName,
               ISNULL(u.FullName, a.PatientName) AS PatientNameDisplay
        FROM Appointments a
        LEFT JOIN Doctors d ON a.DoctorId = d.DoctorId
        LEFT JOIN Users u ON a.PatientId = u.UserId
        WHERE 1=1";

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                if (ddlDoctor.SelectedValue != "0")
                {
                    query += " AND a.DoctorId=@DoctorId";
                    cmd.Parameters.AddWithValue("@DoctorId", ddlDoctor.SelectedValue);
                }

                if (ddlSpecialization.SelectedValue != "All")
                {
                    query += " AND a.Specialization=@Specialization";
                    cmd.Parameters.AddWithValue("@Specialization", ddlSpecialization.SelectedValue);
                }

                if (!string.IsNullOrEmpty(txtFromDate.Text))
                {
                    query += " AND a.AppointmentDate >= @FromDate";
                    cmd.Parameters.AddWithValue("@FromDate", txtFromDate.Text);
                }

                if (!string.IsNullOrEmpty(txtToDate.Text))
                {
                    query += " AND a.AppointmentDate <= @ToDate";
                    cmd.Parameters.AddWithValue("@ToDate", txtToDate.Text);
                }

                query += " ORDER BY a.AppointmentDate DESC";

                cmd.CommandText = query;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvAppointments.DataSource = dt;
                gvAppointments.DataBind();
            }
        }
        // ROW DATA BOUND TO COLOR STATUS BADGES
        protected void gvAppointments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");

                if (lblStatus != null)
                {
                    string status = lblStatus.Text.ToLower();

                    if (status == "approved" || status == "done")
                        lblStatus.CssClass = "badge bg-success p-2";
                    else if (status == "rejected" || status == "cancelled")
                        lblStatus.CssClass = "badge bg-danger p-2";
                    else if (status == "pending")
                        lblStatus.CssClass = "badge bg-warning text-dark p-2";
                    else
                        lblStatus.CssClass = "badge bg-secondary p-2";
                }
            }
        }
    }
}

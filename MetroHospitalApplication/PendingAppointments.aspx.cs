using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace MetroHospitalApplication
{
    public partial class PendingAppointments : System.Web.UI.Page
    {

        string connStr = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAppointments();
            }
        }


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
            WHERE a.Status <> 'Done'  -- Exclude completed appointments
            ORDER BY a.AppointmentDate DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvAppointments.DataSource = dt;
                gvAppointments.DataBind();
            }
        }



        protected void gvAppointments_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Label lblStatus = (Label)e.Row.FindControl("lblStatus");

                if (lblStatus != null)
                {
                    string status = lblStatus.Text.ToLower();

                    if (status == "booked")
                    {
                        lblStatus.CssClass = "badge bg-warning text-dark p-2";
                    }
                    else if (status == "pending")
                    {
                        lblStatus.CssClass = "badge bg-info text-dark p-2";
                    }
                    else if (status == "approved")
                    {
                        lblStatus.CssClass = "badge bg-success p-2";
                    }
                    else if (status == "completed")
                    {
                        lblStatus.CssClass = "badge bg-primary p-2";
                    }
                    else if (status == "rejected")
                    {
                        lblStatus.CssClass = "badge bg-danger p-2";
                    }
                    else if (status == "cancelled")
                    {
                        lblStatus.CssClass = "badge bg-dark p-2";
                    }
                    else
                    {
                        lblStatus.CssClass = "badge bg-secondary p-2";
                    }
                }

            }

        }

    }
}
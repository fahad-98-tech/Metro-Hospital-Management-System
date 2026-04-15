using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace MetroHospitalApplication
{
    public partial class DoctorAppointmentHistoryReport : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }
        public string GetTotalTime(object startTimeObj, object endTimeObj)
        {
            string startTime = startTimeObj?.ToString() ?? "";
            string endTime = endTimeObj?.ToString() ?? "";

            if (TimeSpan.TryParse(startTime, out TimeSpan start) && TimeSpan.TryParse(endTime, out TimeSpan end))
            {
                TimeSpan duration = end - start;
                if (duration.TotalMinutes < 0) duration += new TimeSpan(24, 0, 0);
                return string.Format("{0}h {1}m", (int)duration.TotalHours, duration.Minutes);
            }
            return "-";
        }
        private void BindGrid()
        {
            int doctorId = Convert.ToInt32(Session["DoctorId"]); // Logged-in doctor

            DateTime? fromDate = string.IsNullOrEmpty(txtFromDate.Text) ? (DateTime?)null : Convert.ToDateTime(txtFromDate.Text);
            DateTime? toDate = string.IsNullOrEmpty(txtToDate.Text) ? (DateTime?)null : Convert.ToDateTime(txtToDate.Text);
            string status = ddlStatus.SelectedValue;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT 
                        AppointmentId,
                        PatientName,
                        PatientMobile,
                        AppointmentDate,
                        AppointmentTime,
                        AppointmentEndTime,
                        Status
                    FROM Appointments
                    WHERE DoctorId = @DoctorId
                ";

                if (fromDate.HasValue) query += " AND AppointmentDate >= @FromDate";
                if (toDate.HasValue) query += " AND AppointmentDate <= @ToDate";
                if (!string.IsNullOrEmpty(status)) query += " AND Status = @Status";

                query += " ORDER BY AppointmentDate DESC, AppointmentTime ASC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@DoctorId", doctorId);
                if (fromDate.HasValue) cmd.Parameters.AddWithValue("@FromDate", fromDate.Value.Date);
                if (toDate.HasValue) cmd.Parameters.AddWithValue("@ToDate", toDate.Value.Date);
                if (!string.IsNullOrEmpty(status)) cmd.Parameters.AddWithValue("@Status", status);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvAppointments.DataSource = dt;
                gvAppointments.DataBind();

                // Summary
                lblTotalAppointments.Text = dt.Rows.Count.ToString();
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void gvAppointments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string status = DataBinder.Eval(e.Row.DataItem, "Status").ToString();
                if (status == "Booked") e.Row.CssClass = "status-Booked";
                else if (status == "Completed") e.Row.CssClass = "status-Completed";
                else if (status == "Cancelled") e.Row.CssClass = "status-Cancelled";
            }
        }

        // Calculate Total Time
        public string GetTotalTime(string startTime, string endTime)
        {
            if (TimeSpan.TryParse(startTime, out TimeSpan start) && TimeSpan.TryParse(endTime, out TimeSpan end))
            {
                TimeSpan duration = end - start;
                if (duration.TotalMinutes < 0) duration += new TimeSpan(24, 0, 0);
                return string.Format("{0}h {1}m", (int)duration.TotalHours, duration.Minutes);
            }
            return "-";
        }
    }
}
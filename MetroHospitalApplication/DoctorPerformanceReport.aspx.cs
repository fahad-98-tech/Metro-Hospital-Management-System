using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace MetroHospitalApplication
{
    public partial class DoctorPerformanceReport : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void BindGrid()
        {
            int doctorId = Convert.ToInt32(Session["DoctorId"]); // logged-in doctor

            DateTime? fromDate = string.IsNullOrEmpty(txtFromDate.Text) ? (DateTime?)null : Convert.ToDateTime(txtFromDate.Text);
            DateTime? toDate = string.IsNullOrEmpty(txtToDate.Text) ? (DateTime?)null : Convert.ToDateTime(txtToDate.Text);
            string status = ddlStatus.SelectedValue;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT a.AppointmentId, a.PatientName, a.PatientMobile, 
                           a.AppointmentDate, a.AppointmentTime, a.AppointmentEndTime, a.Status,
                           ISNULL(i.ConsultationFee,0) AS ConsultationFee,
                           ISNULL(i.MedicineCharges,0) AS MedicineCharges,
                           ISNULL(i.TestCharges,0) AS TestCharges,
                           ISNULL(i.ConsultationFee,0)+ISNULL(i.MedicineCharges,0)+ISNULL(i.TestCharges,0) AS TotalAmount
                    FROM Appointments a
                    LEFT JOIN Invoices i ON a.AppointmentId = i.AppointmentId
                    WHERE a.DoctorId=@DoctorId";

                if (fromDate.HasValue) query += " AND a.AppointmentDate>=@FromDate";
                if (toDate.HasValue) query += " AND a.AppointmentDate<=@ToDate";
                if (!string.IsNullOrEmpty(status)) query += " AND a.Status=@Status";

                query += " ORDER BY a.AppointmentDate DESC, a.AppointmentTime ASC";

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

                // ✅ Summary counts
                lblTotalAppointments.Text = dt.Rows.Count.ToString();
                lblCompletedAppointments.Text = dt.Select("Status='Completed'").Length.ToString();
                lblBookedAppointments.Text = dt.Select("Status='Booked'").Length.ToString();
                lblCancelledAppointments.Text = dt.Select("Status='Cancelled'").Length.ToString();
                lblDoneAppointments.Text = dt.Select("Status='Done'").Length.ToString();

                // Total revenue calculation
                decimal totalRevenue = 0;
                foreach (DataRow row in dt.Rows)
                {
                    totalRevenue += Convert.ToDecimal(row["TotalAmount"]);
                }
                lblTotalRevenue.Text = totalRevenue.ToString("C");
            }
        }

        // Calculate total appointment time
        protected string GetTotalTime(object startTimeObj, object endTimeObj)
        {
            string startTime = startTimeObj?.ToString() ?? "";
            string endTime = endTimeObj?.ToString() ?? "";

            if (TimeSpan.TryParse(startTime, out TimeSpan start) && TimeSpan.TryParse(endTime, out TimeSpan end))
            {
                TimeSpan duration = end - start;
                if (duration.TotalMinutes < 0) duration += new TimeSpan(24, 0, 0);
                return $"{(int)duration.TotalHours}h {duration.Minutes}m";
            }
            return "-";
        }

        protected void gvAppointments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string status = DataBinder.Eval(e.Row.DataItem, "Status").ToString();
                if (status == "Booked") e.Row.CssClass = "status-Booked";
                else if (status == "Completed") e.Row.CssClass = "status-Completed";
                else if (status == "Cancelled") e.Row.CssClass = "status-Cancelled";
                else if (status == "Done") e.Row.CssClass = "status-Done";
            }
        }
    }
}
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetroHospitalApplication
{
    public partial class DoctorEarningsReport : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDoctorEarnings();
            }
        }

        private void LoadDoctorEarnings(int? doctorId = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                    SELECT 
                        d.DoctorId,
                        d.FullName AS DoctorName,
                        SUM(i.ConsultationFee) AS TotalConsultationFee,
                        SUM(i.TestCharges) AS TotalTestCharges,
                        SUM(i.MedicineCharges) AS TotalMedicineCharges,
                        SUM(i.ConsultationFee + i.TestCharges + i.MedicineCharges) AS TotalEarnings
                    FROM Invoices i
                    INNER JOIN Appointments a ON i.AppointmentId = a.AppointmentId
                    INNER JOIN Doctors d ON a.DoctorId = d.DoctorId
                    WHERE 1=1";

                if (doctorId.HasValue)
                    query += " AND d.DoctorId = @DoctorId";
                if (fromDate.HasValue)
                    query += " AND i.CreatedAt >= @FromDate";
                if (toDate.HasValue)
                    query += " AND i.CreatedAt <= @ToDate";

                query += " GROUP BY d.DoctorId, d.FullName ORDER BY TotalEarnings DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                if (doctorId.HasValue) cmd.Parameters.AddWithValue("@DoctorId", doctorId.Value);
                if (fromDate.HasValue) cmd.Parameters.AddWithValue("@FromDate", fromDate.Value);
                if (toDate.HasValue) cmd.Parameters.AddWithValue("@ToDate", toDate.Value);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvDoctorEarnings.DataSource = dt;
                gvDoctorEarnings.DataBind();
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            int? doctorId = string.IsNullOrEmpty(txtDoctorId.Text) ? (int?)null : Convert.ToInt32(txtDoctorId.Text);
            DateTime? fromDate = string.IsNullOrEmpty(txtFromDate.Text) ? (DateTime?)null : Convert.ToDateTime(txtFromDate.Text);
            DateTime? toDate = string.IsNullOrEmpty(txtToDate.Text) ? (DateTime?)null : Convert.ToDateTime(txtToDate.Text);

            LoadDoctorEarnings(doctorId, fromDate, toDate);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtDoctorId.Text = "";
            txtFromDate.Text = "";
            txtToDate.Text = "";
            LoadDoctorEarnings();
        }

        protected void gvDoctorEarnings_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                decimal total = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TotalEarnings"));
                if (total > 0)
                    e.Row.CssClass = "highlight";
            }
        }
    }
}
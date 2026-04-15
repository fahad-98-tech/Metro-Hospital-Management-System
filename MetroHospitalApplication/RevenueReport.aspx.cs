using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetroHospitalApplication
{
    public partial class RevenueReport : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadRevenue();
            }
        }

        private void LoadRevenue(DateTime? fromDate = null, DateTime? toDate = null)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                    SELECT 
                        i.InvoiceId,
                        i.AppointmentId,
                        a.PatientName,
                        i.CreatedAt,
                        i.ConsultationFee,
                        i.TestCharges,
                        i.MedicineCharges,
                        (i.ConsultationFee + i.TestCharges + i.MedicineCharges) AS TotalAmount,
                        i.PaymentStatus
                    FROM Invoices i
                    INNER JOIN Appointments a ON i.AppointmentId = a.AppointmentId
                    WHERE 1=1";

                if (fromDate.HasValue)
                    query += " AND i.CreatedAt >= @FromDate";
                if (toDate.HasValue)
                    query += " AND i.CreatedAt <= @ToDate";

                query += " ORDER BY i.CreatedAt DESC";

                SqlCommand cmd = new SqlCommand(query, con);

                if (fromDate.HasValue)
                    cmd.Parameters.AddWithValue("@FromDate", fromDate.Value);
                if (toDate.HasValue)
                    cmd.Parameters.AddWithValue("@ToDate", toDate.Value);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvRevenue.DataSource = dt;
                gvRevenue.DataBind();

                // Calculate total revenue
                decimal totalRevenue = 0;
                foreach (DataRow row in dt.Rows)
                {
                    if (decimal.TryParse(row["TotalAmount"].ToString(), out decimal amount))
                        totalRevenue += amount;
                }

                lblTotalRevenue.Text = "Total Revenue: " + totalRevenue.ToString("C");
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            DateTime? fromDate = string.IsNullOrEmpty(txtFromDate.Text) ? (DateTime?)null : Convert.ToDateTime(txtFromDate.Text);
            DateTime? toDate = string.IsNullOrEmpty(txtToDate.Text) ? (DateTime?)null : Convert.ToDateTime(txtToDate.Text);

            LoadRevenue(fromDate, toDate);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtFromDate.Text = "";
            txtToDate.Text = "";
            LoadRevenue();
        }

        protected void gvRevenue_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string status = DataBinder.Eval(e.Row.DataItem, "PaymentStatus").ToString();
                if (status == "Paid")
                    e.Row.CssClass = "table-success"; // Bootstrap green row
                else if (status == "Pending")
                    e.Row.CssClass = "table-warning"; // Bootstrap yellow row
            }
        }
    }
}
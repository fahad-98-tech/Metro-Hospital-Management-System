using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetroHospitalApplication
{
    public partial class PatientBillingReport : System.Web.UI.Page
    {

        string cs = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadBilling();
            }
        }


        private void LoadBilling(int? patientId = null, DateTime? fromDate = null, DateTime? toDate = null)
        {

            using (SqlConnection con = new SqlConnection(cs))
            {

                string query = @"
                SELECT i.InvoiceId,
                       i.AppointmentId,
                       a.PatientName,
                       i.CreatedAt,
                       i.ConsultationFee,
                       i.TestCharges,
                       i.MedicineCharges,
                       (i.ConsultationFee + i.TestCharges + i.MedicineCharges) AS TotalAmount,
                       i.PaymentStatus
                FROM Invoices i
                INNER JOIN Appointments a ON i.AppointmentId=a.AppointmentId
                WHERE 1=1";


                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                if (patientId.HasValue)
                {
                    query += " AND a.PatientId=@PatientId";
                    cmd.Parameters.AddWithValue("@PatientId", patientId.Value);
                }

                if (fromDate.HasValue)
                {
                    query += " AND i.CreatedAt>=@FromDate";
                    cmd.Parameters.AddWithValue("@FromDate", fromDate.Value);
                }

                if (toDate.HasValue)
                {
                    query += " AND i.CreatedAt<=@ToDate";
                    cmd.Parameters.AddWithValue("@ToDate", toDate.Value);
                }

                query += " ORDER BY i.CreatedAt DESC";

                cmd.CommandText = query;

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvPatientBilling.DataSource = dt;
                gvPatientBilling.DataBind();

            }

        }


        protected void btnFilter_Click(object sender, EventArgs e)
        {

            int? patientId = string.IsNullOrEmpty(txtPatientId.Text)
                ? (int?)null : Convert.ToInt32(txtPatientId.Text);

            DateTime? fromDate = string.IsNullOrEmpty(txtFromDate.Text)
                ? (DateTime?)null : Convert.ToDateTime(txtFromDate.Text);

            DateTime? toDate = string.IsNullOrEmpty(txtToDate.Text)
                ? (DateTime?)null : Convert.ToDateTime(txtToDate.Text);

            LoadBilling(patientId, fromDate, toDate);

        }


        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtPatientId.Text = "";
            txtFromDate.Text = "";
            txtToDate.Text = "";

            LoadBilling();
        }


        protected void btnPayment_Click(object sender, EventArgs e)
        {

            Button btn = (Button)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            string invoiceId = row.Cells[0].Text;
            string appointmentId = row.Cells[1].Text;
            string consultationFee = row.Cells[4].Text.Replace("$", "");
            string testCharges = row.Cells[5].Text.Replace("$", "");
            string medicineCharges = row.Cells[6].Text.Replace("$", "");

            Response.Redirect("AdminPaymentRecived.aspx?invoiceId=" + invoiceId +
                              "&appointmentId=" + appointmentId +
                              "&consultationFee=" + consultationFee +
                              "&testCharges=" + testCharges +
                              "&medicineCharges=" + medicineCharges);

        }



        protected void gvPatientBilling_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string status = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "PaymentStatus"));

                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                Button btn = (Button)e.Row.FindControl("btnPayment");

                if (status.ToLower() == "paid")
                {
                    lblStatus.CssClass = "badge bg-success";
                    btn.Enabled = false;
                    btn.Text = "Paid";
                }
                else
                {
                    lblStatus.CssClass = "badge bg-warning text-dark";
                }

            }

        }

    }
}
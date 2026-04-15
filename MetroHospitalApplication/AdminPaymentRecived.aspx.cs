using System;
using System.Configuration;
using System.Data.SqlClient;

namespace MetroHospitalApplication
{
    public partial class AdminPaymentReceived : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadInvoiceDetails();
            }
        }

        private void LoadInvoiceDetails()
        {
            // Get invoice details from query string
            string invoiceId = Request.QueryString["invoiceId"];
            string appointmentId = Request.QueryString["appointmentId"];
            string consultationFee = Request.QueryString["consultationFee"];
            string testCharges = Request.QueryString["testCharges"];
            string medicineCharges = Request.QueryString["medicineCharges"];

            if (!string.IsNullOrEmpty(invoiceId))
            {
                txtInvoiceId.Text = invoiceId;
                txtAppointmentId.Text = appointmentId;
                txtConsultationFee.Text = consultationFee;
                txtTestCharges.Text = testCharges;
                txtMedicineCharges.Text = medicineCharges;

                // Calculate total
                decimal total = 0;
                decimal.TryParse(consultationFee, out decimal cfee);
                decimal.TryParse(testCharges, out decimal tfee);
                decimal.TryParse(medicineCharges, out decimal mfee);

                total = cfee + tfee + mfee;
                txtTotalAmount.Text = total.ToString("F2");
            }
        }

        protected void btnSavePayment_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtInvoiceId.Text)) return;

            string invoiceId = txtInvoiceId.Text;
            string paymentStatus = ddlPaymentStatus.SelectedValue;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "UPDATE Invoices SET PaymentStatus=@PaymentStatus WHERE InvoiceId=@InvoiceId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@PaymentStatus", paymentStatus);
                cmd.Parameters.AddWithValue("@InvoiceId", invoiceId);

                con.Open();
                int rows = cmd.ExecuteNonQuery();
                con.Close();

                if (rows > 0)
                {
                    lblMessage.Text = "Payment status updated successfully!";
                    lblMessage.CssClass = "text-success";
                }
                else
                {
                    lblMessage.Text = "Failed to update payment status.";
                    lblMessage.CssClass = "text-danger";
                }
            }
        }
    }
}
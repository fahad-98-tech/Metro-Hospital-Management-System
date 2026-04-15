using System;
using System.Configuration;
using System.Data.SqlClient;

namespace MetroHospitalApplication
{
    public partial class PaymentReceived : System.Web.UI.Page
    {

        string cs = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                txtInvoiceId.Text = Request.QueryString["invoiceId"];
                txtAppointmentId.Text = Request.QueryString["appointmentId"];

                txtConsultationFee.Text = Request.QueryString["consultationFee"];
                txtTestCharges.Text = Request.QueryString["testCharges"];
                txtMedicineCharges.Text = Request.QueryString["medicineCharges"];


                CalculateTotal();

            }

        }


        private void CalculateTotal()
        {

            decimal consultation = Convert.ToDecimal(txtConsultationFee.Text);
            decimal test = Convert.ToDecimal(txtTestCharges.Text);
            decimal medicine = Convert.ToDecimal(txtMedicineCharges.Text);

            decimal total = consultation + test + medicine;

            txtTotalAmount.Text = total.ToString();

        }



        protected void btnSavePayment_Click(object sender, EventArgs e)
        {

            using (SqlConnection con = new SqlConnection(cs))
            {

                string query = @"UPDATE Invoices
                                 SET PaymentStatus='Paid'
                                 WHERE InvoiceId=@InvoiceId";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@InvoiceId", txtInvoiceId.Text);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

            }

            Response.Redirect("PatientBillingReport.aspx");

        }

    }
}
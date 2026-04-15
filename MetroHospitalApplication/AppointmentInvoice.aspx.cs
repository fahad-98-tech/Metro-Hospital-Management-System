using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace MetroHospitalApplication
{
    public partial class AppointmentInvoice : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;
        int appointmentId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["aid"] == null)
            {
                Response.Write("AppointmentId missing!");
                Response.End();
            }

            appointmentId = Convert.ToInt32(Request.QueryString["aid"]);

            if (!IsPostBack)
            {
                lblAppointment.Text = $"Appointment ID: {appointmentId}";
                BindInvoicesGrid();
            }
        }

        #region GridView Methods
        private void BindInvoicesGrid()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(@"SELECT InvoiceId, ConsultationFee, TestCharges, MedicineCharges, PaymentStatus
                                                         FROM Invoices WHERE AppointmentId=@AppointmentId", con);
                da.SelectCommand.Parameters.AddWithValue("@AppointmentId", appointmentId);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    gvInvoices.DataSource = dt;
                    gvInvoices.DataBind();
                    lblGridMsg.Text = "";
                }
                else
                {
                    gvInvoices.DataSource = null;
                    gvInvoices.DataBind();
                    lblGridMsg.Text = "⚠ Invoice not yet generated.";
                }
                con.Close();
            }
        }

        protected void gvInvoices_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvInvoices.EditIndex = e.NewEditIndex;
            BindInvoicesGrid();
        }

        protected void gvInvoices_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvInvoices.EditIndex = -1;
            BindInvoicesGrid();
        }

        protected void gvInvoices_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int invoiceId = Convert.ToInt32(gvInvoices.DataKeys[e.RowIndex].Value);
            GridViewRow row = gvInvoices.Rows[e.RowIndex];

            decimal consultationFee = Convert.ToDecimal(((TextBox)row.Cells[1].Controls[0]).Text.Trim());
            decimal testCharges = Convert.ToDecimal(((TextBox)row.Cells[2].Controls[0]).Text.Trim());
            decimal medicineCharges = Convert.ToDecimal(((TextBox)row.Cells[3].Controls[0]).Text.Trim());
            string paymentStatus = ((TextBox)row.Cells[4].Controls[0]).Text.Trim();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"UPDATE Invoices SET ConsultationFee=@ConsultationFee, TestCharges=@TestCharges, 
                                                  MedicineCharges=@MedicineCharges, PaymentStatus=@PaymentStatus
                                                  WHERE InvoiceId=@InvoiceId", con);
                cmd.Parameters.AddWithValue("@ConsultationFee", consultationFee);
                cmd.Parameters.AddWithValue("@TestCharges", testCharges);
                cmd.Parameters.AddWithValue("@MedicineCharges", medicineCharges);
                cmd.Parameters.AddWithValue("@PaymentStatus", paymentStatus);
                cmd.Parameters.AddWithValue("@InvoiceId", invoiceId);
                cmd.ExecuteNonQuery();
                con.Close();
            }

            gvInvoices.EditIndex = -1;
            BindInvoicesGrid();
            lblMsg.Text = "✔ Invoice updated successfully!";
        }

        protected void gvInvoices_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int invoiceId = Convert.ToInt32(gvInvoices.DataKeys[e.RowIndex].Value);
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Invoices WHERE InvoiceId=@InvoiceId", con);
                cmd.Parameters.AddWithValue("@InvoiceId", invoiceId);
                cmd.ExecuteNonQuery();
                con.Close();
            }

            BindInvoicesGrid();
            lblMsg.Text = "✔ Invoice deleted successfully!";
        }
        #endregion

        #region Save / View Invoice
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveInvoice();
            lblMsg.Text = "✔ Invoice saved successfully!";
            BindInvoicesGrid();
        }

        void SaveInvoice()
        {
            decimal consultationFee = string.IsNullOrEmpty(txtConsultationFee.Text.Trim()) ? 0 : Convert.ToDecimal(txtConsultationFee.Text.Trim());
            decimal testCharges = string.IsNullOrEmpty(txtTestCharges.Text.Trim()) ? 0 : Convert.ToDecimal(txtTestCharges.Text.Trim());
            decimal medicineCharges = string.IsNullOrEmpty(txtMedicineCharges.Text.Trim()) ? 0 : Convert.ToDecimal(txtMedicineCharges.Text.Trim());

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Invoices WHERE AppointmentId=@AppointmentId", con);
                checkCmd.Parameters.AddWithValue("@AppointmentId", appointmentId);
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                SqlCommand cmd;
                if (count > 0)
                {
                    cmd = new SqlCommand(@"UPDATE Invoices SET ConsultationFee=@ConsultationFee, TestCharges=@TestCharges, 
                                           MedicineCharges=@MedicineCharges, PaymentStatus=@PaymentStatus
                                           WHERE AppointmentId=@AppointmentId", con);
                }
                else
                {
                    cmd = new SqlCommand(@"INSERT INTO Invoices(AppointmentId, ConsultationFee, TestCharges, MedicineCharges, PaymentStatus) 
                                           VALUES(@AppointmentId,@ConsultationFee,@TestCharges,@MedicineCharges,@PaymentStatus)", con);
                }

                cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);
                cmd.Parameters.AddWithValue("@ConsultationFee", consultationFee);
                cmd.Parameters.AddWithValue("@TestCharges", testCharges);
                cmd.Parameters.AddWithValue("@MedicineCharges", medicineCharges);
                cmd.Parameters.AddWithValue("@PaymentStatus", ddlPaymentStatus.SelectedValue);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT i.InvoiceId, i.ConsultationFee, i.TestCharges, i.MedicineCharges,
                                                         a.PatientName, a.PatientMobile, a.Specialization, a.AppointmentDate
                                                  FROM Invoices i
                                                  INNER JOIN Appointments a ON a.AppointmentId=i.AppointmentId
                                                  WHERE i.AppointmentId=@AppointmentId", con);
                cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    pnlInvoiceReport.Visible = true;
                    lblInvoiceNumber.Text = dr["InvoiceId"].ToString();
                    lblPatientName.Text = dr["PatientName"].ToString();
                    lblPatientMobile.Text = dr["PatientMobile"].ToString();
                    lblSpecialization.Text = dr["Specialization"].ToString();
                    lblAppointmentDate.Text = Convert.ToDateTime(dr["AppointmentDate"]).ToString("dd-MMM-yyyy");

                    lblInvConsultationFee.Text = Convert.ToDecimal(dr["ConsultationFee"]).ToString("C");
                    lblInvTestCharges.Text = Convert.ToDecimal(dr["TestCharges"]).ToString("C");
                    lblInvMedicineCharges.Text = Convert.ToDecimal(dr["MedicineCharges"]).ToString("C");

                    decimal total = Convert.ToDecimal(dr["ConsultationFee"]) + Convert.ToDecimal(dr["TestCharges"]) + Convert.ToDecimal(dr["MedicineCharges"]);
                    lblInvTotalAmount.Text = total.ToString("C");

                    dr.Close();

                    // Load medicines
                    SqlDataAdapter da = new SqlDataAdapter("SELECT MedicineName,Dosage,Duration,Instructions FROM AppointmentMedicines WHERE AppointmentId=@AppointmentId", con);
                    da.SelectCommand.Parameters.AddWithValue("@AppointmentId", appointmentId);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvInvoiceMedicines.Controls.Clear();
                    foreach (DataRow row in dt.Rows)
                    {
                        TableRow tr = new TableRow();
                        tr.Cells.Add(new TableCell() { Text = row["MedicineName"].ToString() });
                        tr.Cells.Add(new TableCell() { Text = row["Dosage"].ToString() });
                        tr.Cells.Add(new TableCell() { Text = row["Duration"].ToString() });
                        tr.Cells.Add(new TableCell() { Text = row["Instructions"].ToString() });
                        gvInvoiceMedicines.Controls.Add(tr);
                    }
                }
                else
                {
                    pnlInvoiceReport.Visible = false;
                    lblMsg.Text = "⚠ Invoice not generated yet.";
                }
                con.Close();
            }
        }
        #endregion
    }
}

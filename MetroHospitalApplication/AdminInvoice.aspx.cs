using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace MetroHospitalApplication
{
    public partial class AdminInvoice : System.Web.UI.Page
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
                LoadAppointmentDetails();
                LoadMedicines();
                LoadTreatments();
                LoadReports();
                LoadInvoice();
            }
        }

        private void LoadAppointmentDetails()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"
                    SELECT a.PatientName, a.PatientMobile, d.FullName AS DoctorName,
                           a.Specialization, a.AppointmentDate
                    FROM Appointments a
                    LEFT JOIN Doctors d ON a.DoctorId = d.DoctorId
                    WHERE a.AppointmentId=@AppointmentId", con);

                cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    lblAppointmentId.Text = "Appointment ID: " + appointmentId;
                    lblPatient.Text = "Patient: " + dr["PatientName"];
                    lblMobile.Text = "Mobile: " + dr["PatientMobile"];
                    lblDoctor.Text = "Doctor: " + dr["DoctorName"];
                    lblDepartment.Text = "Department: " + dr["Specialization"];
                    lblAppointmentDate.Text = "Date: " + Convert.ToDateTime(dr["AppointmentDate"]).ToString("dd-MMM-yyyy");
                }
                dr.Close();
            }
        }

        private void LoadMedicines()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(@"
                    SELECT MedicineName, Dosage, Duration, Instructions
                    FROM AppointmentMedicines
                    WHERE AppointmentId=@AppointmentId", con);
                da.SelectCommand.Parameters.AddWithValue("@AppointmentId", appointmentId);

                DataTable dt = new DataTable();
                da.Fill(dt);
                gvMedicines.DataSource = dt;
                gvMedicines.DataBind();
            }
        }

        private void LoadTreatments()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(@"
                    SELECT Symptoms, Diagnosis, Notes
                    FROM AppointmentTreatments
                    WHERE AppointmentId=@AppointmentId", con);
                da.SelectCommand.Parameters.AddWithValue("@AppointmentId", appointmentId);

                DataTable dt = new DataTable();
                da.Fill(dt);
                gvTreatments.DataSource = dt;
                gvTreatments.DataBind();
            }
        }

        private void LoadReports()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlDataAdapter da = new SqlDataAdapter(@"
            SELECT ReportType, FilePath, UploadedAt
            FROM AppointmentReports
            WHERE AppointmentId = @AppointmentId", con);

                da.SelectCommand.Parameters.AddWithValue("@AppointmentId", appointmentId);

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvReports.DataSource = dt;
                gvReports.DataBind();
            }
        }

        private void LoadInvoice()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"
                    SELECT ConsultationFee, TestCharges, MedicineCharges, PaymentStatus
                    FROM Invoices
                    WHERE AppointmentId=@AppointmentId", con);
                cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    decimal consultationFee = Convert.ToDecimal(dr["ConsultationFee"]);
                    decimal testCharges = Convert.ToDecimal(dr["TestCharges"]);
                    decimal medicineCharges = Convert.ToDecimal(dr["MedicineCharges"]);
                    decimal total = consultationFee + testCharges + medicineCharges;

                    lblConsultationFee.Text = "Consultation Fee: " + consultationFee.ToString("C");
                    lblTestCharges.Text = "Test Charges: " + testCharges.ToString("C");
                    lblMedicineCharges.Text = "Medicine Charges: " + medicineCharges.ToString("C");
                    lblTotalAmount.Text = "Total Amount: " + total.ToString("C");
                    lblPaymentStatus.Text = "Payment Status: " + dr["PaymentStatus"].ToString();
                }
            }
        }
    }
}
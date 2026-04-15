using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MetroHospitalApplication
{
    public partial class AppointmentTreatment : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;
        int appointmentId = 0;
        int doctorId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["aid"] == null)
            {
                Response.Write("AppointmentId missing!");
                Response.End();
            }

            appointmentId = Convert.ToInt32(Request.QueryString["aid"]);
            doctorId = Convert.ToInt32(Session["DoctorId"]);

            if (!IsPostBack)
            {
                lblAppointment.Text = $"Appointment ID: {appointmentId}";
                LoadTreatment();
            }
        }

        void LoadTreatment()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"SELECT TOP 1 TreatmentId, Symptoms, Diagnosis, Notes 
                                 FROM AppointmentTreatments
                                 WHERE AppointmentId=@AppointmentId AND DoctorId=@DoctorId
                                 ORDER BY CreatedAt DESC";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);
                cmd.Parameters.AddWithValue("@DoctorId", doctorId);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    hfTreatmentId.Value = dr["TreatmentId"].ToString();
                    txtSymptoms.Text = dr["Symptoms"].ToString();
                    txtDiagnosis.Text = dr["Diagnosis"].ToString();
                    txtNotes.Text = dr["Notes"].ToString();
                }
                con.Close();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd;

                if (!string.IsNullOrEmpty(hfTreatmentId.Value))
                {
                    // Update existing treatment
                    cmd = new SqlCommand(@"UPDATE AppointmentTreatments
                                           SET Symptoms=@Symptoms, Diagnosis=@Diagnosis, Notes=@Notes
                                           WHERE TreatmentId=@Id", con);
                    cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(hfTreatmentId.Value));
                }
                else
                {
                    // Insert new treatment
                    cmd = new SqlCommand(@"INSERT INTO AppointmentTreatments
                                           (AppointmentId, DoctorId, Symptoms, Diagnosis, Notes)
                                           VALUES (@AppointmentId, @DoctorId, @Symptoms, @Diagnosis, @Notes)", con);
                    cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);
                    cmd.Parameters.AddWithValue("@DoctorId", doctorId);
                }

                cmd.Parameters.AddWithValue("@Symptoms", txtSymptoms.Text.Trim());
                cmd.Parameters.AddWithValue("@Diagnosis", txtDiagnosis.Text.Trim());
                cmd.Parameters.AddWithValue("@Notes", txtNotes.Text.Trim());

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            lblMsg.Text = "✔ Treatment saved successfully!";
        }
    }
}

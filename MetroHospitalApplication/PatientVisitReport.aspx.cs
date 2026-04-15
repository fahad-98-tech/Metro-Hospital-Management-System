using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace MetroHospitalApplication
{
    public partial class PatientVisitReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPatientVisits(); // Load report for logged-in doctor
            }
        }

        private void LoadPatientVisits()
        {
            if (Session["DoctorId"] == null)
            {
                Response.Redirect("Login.aspx"); // Ensure doctor is logged in
                return;
            }

            int doctorId = Convert.ToInt32(Session["DoctorId"]);
            string connStr = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetAppointmentReport", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Only the logged-in doctor
                    cmd.Parameters.AddWithValue("@DoctorId", doctorId);

                    // Optional filters
                    cmd.Parameters.AddWithValue("@Status", DBNull.Value);
                    cmd.Parameters.AddWithValue("@FromDate", string.IsNullOrEmpty(txtFromDate.Text) ? (object)DBNull.Value : DateTime.Parse(txtFromDate.Text));
                    cmd.Parameters.AddWithValue("@ToDate", string.IsNullOrEmpty(txtToDate.Text) ? (object)DBNull.Value : DateTime.Parse(txtToDate.Text));

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvPatientVisits.DataSource = dt;
                    gvPatientVisits.DataBind();
                }
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            LoadPatientVisits();
        }
    }
}
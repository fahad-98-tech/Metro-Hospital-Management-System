using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MetroHospitalApplication
{
    public partial class Doctors : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDoctors();
            }
        }

        private void LoadDoctors()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"SELECT DoctorId,
                                        FullName,
                                        Email,
                                        MobileNumber,
                                        Specialization,
                                        CreatedDate,
                                        Age,
                                        DoctorImage
                                 FROM Doctors
                                 WHERE IsActive = 1
                                 ORDER BY FullName";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        rptDoctors.DataSource = dt;
                        rptDoctors.DataBind();
                    }
                }
            }
        }

        protected string GetDoctorImage(object imagePath)
        {
            if (imagePath == null || imagePath == DBNull.Value || string.IsNullOrEmpty(imagePath.ToString()))
            {
                return ResolveUrl("~/Images/default-doctor.png");
            }
            return ResolveUrl(imagePath.ToString());
        }
    }
}

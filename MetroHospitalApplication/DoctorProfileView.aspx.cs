using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace MetroHospitalApplication
{
    public partial class DoctorProfileView : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    int doctorId;
                    if (int.TryParse(Request.QueryString["id"], out doctorId))
                    {
                        LoadDoctorProfile(doctorId);
                    }
                }
            }
        }

        private void LoadDoctorProfile(int doctorId)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Doctors WHERE DoctorId=@DoctorId AND IsActive=1", con);

                cmd.Parameters.AddWithValue("@DoctorId", doctorId);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    lblName.Text = dr["FullName"].ToString();
                    lblEmail.Text = dr["Email"].ToString();
                    lblMobile.Text = dr["MobileNumber"].ToString();
                    lblGender.Text = dr["Gender"].ToString();
                    lblAge.Text = dr["Age"].ToString();

                    if (dr["DateOfBirth"] != DBNull.Value)
                        lblDOB.Text = Convert.ToDateTime(dr["DateOfBirth"]).ToString("dd MMM yyyy");

                    if (dr["CreatedDate"] != DBNull.Value)
                        lblCreatedAt.Text = Convert.ToDateTime(dr["CreatedDate"]).ToString("dd MMM yyyy");

                    // Load doctor image
                    if (dr["DoctorImage"] != DBNull.Value && !string.IsNullOrEmpty(dr["DoctorImage"].ToString()))
                        imgDoctor.ImageUrl = ResolveUrl(dr["DoctorImage"].ToString());
                    else
                        imgDoctor.ImageUrl = ResolveUrl("~/Images/default-doctor.png");

                    // Load specializations (if multiple, separated by commas)
                    if (dr["Specialization"] != DBNull.Value)
                    {
                        string[] specs = dr["Specialization"].ToString().Split(',');
                        StringBuilder sb = new StringBuilder();
                        foreach (var spec in specs)
                        {
                            sb.Append("<span class='badge-spec text-white'>" + spec.Trim() + "</span>");
                        }
                        divSpecialization.InnerHtml = sb.ToString();
                    }
                }
                else
                {
                    // Redirect if doctor not found
                    Response.Redirect("~/Doctors.aspx");
                }
            }
        }
    }
}

using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.IO;

namespace MetroHospitalApplication
{
    public partial class DoctorProfile : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSpecializations();
                LoadDoctorProfile();
            }
        }

        private void LoadDoctorProfile()
        {
            if (Session["DoctorId"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            int doctorId = Convert.ToInt32(Session["DoctorId"]);

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Doctors WHERE DoctorId=@DoctorId", con);
                cmd.Parameters.AddWithValue("@DoctorId", doctorId);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    txtName.Text = dr["FullName"].ToString();
                    txtEmail.Text = dr["Email"].ToString();
                    txtMobile.Text = dr["MobileNumber"].ToString();
                    txtAge.Text = dr["Age"].ToString();
                    ddlGender.SelectedValue = dr["Gender"].ToString();

                    if (dr["DateOfBirth"] != DBNull.Value)
                        txtDOB.Text = Convert.ToDateTime(dr["DateOfBirth"])
                            .ToString("yyyy-MM-dd");

                    imgDoctor.ImageUrl = dr["DoctorImage"] != DBNull.Value
                        ? dr["DoctorImage"].ToString()
                        : "~/Images/default-doctor.png";

                    // MULTI SPECIALIZATION
                    if (dr["Specialization"] != DBNull.Value)
                    {
                        string specString = dr["Specialization"].ToString();
                        string[] selectedSpecs = specString.Split(',');

                        foreach (var item in ddlSpecialization.Items
                            .Cast<System.Web.UI.WebControls.ListItem>())
                        {
                            if (selectedSpecs.Contains(item.Value))
                                item.Selected = true;
                        }

                        // DISPLAY BELOW IMAGE
                        rptSpecialization.DataSource = selectedSpecs;
                        rptSpecialization.DataBind();
                    }
                }
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int doctorId = Convert.ToInt32(Session["DoctorId"]);

            // IMAGE UPLOAD
            string imagePath = imgDoctor.ImageUrl;
            if (fuDoctorImage.HasFile)
            {
                string folder = "~/Uploads/Doctors/";
                if (!Directory.Exists(Server.MapPath(folder)))
                    Directory.CreateDirectory(Server.MapPath(folder));

                string fileName = Guid.NewGuid() +
                    Path.GetExtension(fuDoctorImage.FileName);

                imagePath = folder + fileName;
                fuDoctorImage.SaveAs(Server.MapPath(imagePath));
            }

            string selectedSpecializations =
                string.Join(",",
                ddlSpecialization.Items
                .Cast<System.Web.UI.WebControls.ListItem>()
                .Where(i => i.Selected)
                .Select(i => i.Value));

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(@"
                    UPDATE Doctors SET
                        FullName=@FullName,
                        MobileNumber=@Mobile,
                        Gender=@Gender,
                        Age=@Age,
                        DateOfBirth=@DOB,
                        Specialization=@Specialization,
                        DoctorImage=@DoctorImage,
                        CreatedDate=GETDATE()
                    WHERE DoctorId=@DoctorId", con);

                cmd.Parameters.AddWithValue("@FullName", txtName.Text);
                cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text);
                cmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedValue);
                cmd.Parameters.AddWithValue("@Age", txtAge.Text);
                cmd.Parameters.AddWithValue("@DOB",
                    string.IsNullOrEmpty(txtDOB.Text)
                    ? (object)DBNull.Value
                    : txtDOB.Text);
                cmd.Parameters.AddWithValue("@Specialization", selectedSpecializations);
                cmd.Parameters.AddWithValue("@DoctorImage", imagePath);
                cmd.Parameters.AddWithValue("@DoctorId", doctorId);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            lblMsg.Text = "Profile updated successfully!";
            lblMsg.CssClass = "text-success fw-semibold";

            LoadDoctorProfile();
        }

        private void LoadSpecializations()
        {
            string[] specializations = {
                "Cardiology","Dermatology","Neurology","Oncology","Pediatrics",
                "Psychiatry","Radiology","Orthopedics","Gastroenterology","Endocrinology",
                "Urology","Nephrology","Pulmonology","Rheumatology","Ophthalmology",
                "ENT","Allergy & Immunology","Hematology","Infectious Disease",
                "Obstetrics & Gynecology","General Surgery","Plastic Surgery",
                "Vascular Surgery","Thoracic Surgery","Pathology","Anesthesiology",
                "Emergency Medicine","Internal Medicine","Family Medicine",
                "Critical Care","Neurosurgery","Cardiothoracic Surgery",
                "Pediatric Cardiology","Interventional Cardiology"
            };

            ddlSpecialization.Items.Clear();
            foreach (var spec in specializations)
            {
                ddlSpecialization.Items.Add(spec);
            }
        }
    }
}

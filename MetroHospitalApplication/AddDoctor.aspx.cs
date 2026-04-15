using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;
using System.Windows;

namespace MetroHospitalApplication
{
    public partial class AddDoctor : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadSpecializations();
            }
        }

        private void LoadSpecializations()
        {
            string[] specializations = new string[]
            {
   "Cardiology","Dermatology","Neurology","Oncology","Pediatrics",
   "Psychiatry","Radiology","Orthopedics","Gastroenterology","Endocrinology",
   "Urology","Nephrology","Pulmonology","Rheumatology","Ophthalmology",
   "ENT","Allergy & Immunology","Hematology","Infectious Disease","Obstetrics & Gynecology",
   "General Surgery","Plastic Surgery","Vascular Surgery","Thoracic Surgery","Pathology",
   "Anesthesiology","Critical Care","Emergency Medicine","Family Medicine","Internal Medicine",
   "Nuclear Medicine","Occupational Medicine","Sports Medicine","Geriatrics","Palliative Care",
   "Rehabilitation","Sleep Medicine","Pain Management","Preventive Medicine","Transplant Surgery",
   "Dermatologic Surgery","Neonatology","Cardiothoracic Surgery","Pediatric Surgery","Endocrine Surgery",
   "Ophthalmic Surgery","Oral & Maxillofacial","Hand Surgery","Microbiology","Clinical Genetics",
   "Medical Oncology","Radiation Oncology","Interventional Radiology","Pediatric Cardiology","Pediatric Neurology",
   "Pediatric Oncology","Psychotherapy","Clinical Pharmacology","Hospitalist","Addiction Medicine",
   "Emergency Pediatrics","Critical Care Pediatrics","Pediatric Surgery","Geriatric Psychiatry","Sleep Pediatrics",
   "Sports Cardiology","Reproductive Endocrinology","Maternal-Fetal Medicine","Pediatric Endocrinology","Pediatric Pulmonology",
   "Pediatric Gastroenterology","Pediatric Nephrology","Pediatric Rheumatology","Pediatric Infectious Disease","Pediatric Hematology",
   "Pediatric Dermatology","Pediatric Ophthalmology","Pediatric Orthopedics","Pediatric ENT","Transplant Hepatology",
   "Cardiac Electrophysiology","Interventional Cardiology","Structural Heart Disease","Heart Failure","Congenital Heart Disease",
   "Cardiac Imaging","Vascular Medicine","Vascular Surgery","Thoracic Surgery","Endovascular Surgery",
   "Plastic & Reconstructive","Colorectal Surgery","Surgical Oncology","Bariatric Surgery","Trauma Surgery",
   "Neurosurgery","Spine Surgery","Craniofacial Surgery","Pediatric Neurosurgery","Pediatric Surgery Specialties"
            };

            ddlSpecialization.Items.Clear();
            ddlSpecialization.Items.Add(new System.Web.UI.WebControls.ListItem("Select Specialization", ""));
            foreach (var spec in specializations)
            {
                ddlSpecialization.Items.Add(spec);
            }

            // Disable first item
            ddlSpecialization.Items[0].Attributes["disabled"] = "disabled";
            ddlSpecialization.Items[0].Attributes["hidden"] = "hidden";
        }

        protected void txtDOB_TextChanged(object sender, EventArgs e)
        {
            if (DateTime.TryParse(txtDOB.Text, out DateTime dob))
            {
                txtAge.Text = CalculateAge(dob).ToString();
            }
        }

        protected void btnSaveDoctor_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime dob = DateTime.Parse(txtDOB.Text);
                int age = CalculateAge(dob);

                string imagePath = null;

                if (fuDoctorImage.HasFile)
                {
                    string ext = System.IO.Path.GetExtension(fuDoctorImage.FileName);
                    string fileName = "Doctor_" + Guid.NewGuid() + ext;
                    imagePath = "/DoctorImages/" + fileName;
                    fuDoctorImage.SaveAs(Server.MapPath(imagePath));
                }

                string connStr = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string query = @"INSERT INTO Doctors
                        (FullName, Email, MobileNumber, Gender, DateOfBirth, Age,
                         PasswordHash, Specialization, DoctorImage, CreatedBy)
                        VALUES
                        (@FullName,@Email,@Mobile,@Gender,@DOB,@Age,
                         @Password,@Spec,@Image,@CreatedBy)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@Mobile", txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedValue);
                        cmd.Parameters.AddWithValue("@DOB", dob);
                        cmd.Parameters.AddWithValue("@Age", age);
                        cmd.Parameters.AddWithValue("@Password", HashPassword(txtPassword.Text));
                        cmd.Parameters.AddWithValue("@Spec", ddlSpecialization.SelectedValue);
                        cmd.Parameters.AddWithValue("@Image", (object)imagePath ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@CreatedBy", Session["UserId"]);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                ClientScript.RegisterStartupScript(this.GetType(), "ok",
                    "alert('Doctor added successfully'); window.location='ManageDoctors.aspx';", true);
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        private int CalculateAge(DateTime dob)
        {
            int age = DateTime.Today.Year - dob.Year;
            if (dob > DateTime.Today.AddYears(-age)) age--;
            return age;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
    }
}

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI.WebControls;

namespace MetroHospitalApplication
{
    public partial class DoctorList : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
                Response.Redirect("~/Login.aspx");

            if (!IsPostBack)
            {
                LoadSpecializations();
                LoadDoctors();
            }
        }

        private string[] GetSpecializations()
        {
            return new string[]
            {
                "Cardiology","Dermatology","Neurology","Pediatrics",
                "Orthopedics","Radiology","Psychiatry","General Surgery"
            };
        }

        private void LoadSpecializations()
        {
            ddlFilterSpecialization.Items.Clear();
            ddlFilterSpecialization.Items.Add(new ListItem("All Specializations", ""));
            foreach (var s in GetSpecializations())
                ddlFilterSpecialization.Items.Add(s);
        }

        private void LoadDoctors()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM Doctors WHERE 1=1";

                if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                    query += " AND (FullName LIKE @Search OR Email LIKE @Search)";

                if (!string.IsNullOrWhiteSpace(ddlFilterSpecialization.SelectedValue))
                    query += " AND Specialization=@Spec";

                if (!string.IsNullOrWhiteSpace(ddlFilterStatus.SelectedValue))
                    query += " AND IsActive=@Status";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                        cmd.Parameters.AddWithValue("@Search", "%" + txtSearch.Text.Trim() + "%");

                    if (!string.IsNullOrWhiteSpace(ddlFilterSpecialization.SelectedValue))
                        cmd.Parameters.AddWithValue("@Spec", ddlFilterSpecialization.SelectedValue);

                    if (!string.IsNullOrWhiteSpace(ddlFilterStatus.SelectedValue))
                        cmd.Parameters.AddWithValue("@Status",
                            Convert.ToBoolean(Convert.ToInt32(ddlFilterStatus.SelectedValue)));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        gvDoctors.DataSource = dt;
                        gvDoctors.DataBind();
                    }
                }
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            LoadDoctors();
        }

        protected void gvDoctors_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvDoctors.EditIndex = e.NewEditIndex;
            LoadDoctors();

            GridViewRow row = gvDoctors.Rows[e.NewEditIndex];

            DropDownList ddlSpec = (DropDownList)row.FindControl("ddlSpecEdit");
            ddlSpec.Items.Clear();
            foreach (var s in GetSpecializations())
                ddlSpec.Items.Add(s);

            ddlSpec.SelectedValue = row.Cells[6].Text;

            DropDownList ddlGender = (DropDownList)row.FindControl("ddlGenderEdit");
            ddlGender.SelectedValue = row.Cells[5].Text;
        }

        protected void gvDoctors_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvDoctors.EditIndex = -1;
            LoadDoctors();
        }

        protected void gvDoctors_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int doctorId = Convert.ToInt32(gvDoctors.DataKeys[e.RowIndex].Value);
            GridViewRow row = gvDoctors.Rows[e.RowIndex];

            DropDownList ddlSpec = (DropDownList)row.FindControl("ddlSpecEdit");
            DropDownList ddlGender = (DropDownList)row.FindControl("ddlGenderEdit");
            DropDownList ddlStatus = (DropDownList)row.FindControl("ddlStatusEdit");
            FileUpload fuImage = (FileUpload)row.FindControl("fuEditImage");

            string imagePath = null;

            if (fuImage.HasFile)
            {
                string ext = Path.GetExtension(fuImage.FileName);
                string fileName = "Doctor_" + Guid.NewGuid() + ext;
                imagePath = "/DoctorImages/" + fileName;
                fuImage.SaveAs(Server.MapPath(imagePath));
            }

            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = @"UPDATE Doctors SET
                                Gender=@Gender,
                                Specialization=@Spec,
                                IsActive=@Status" +
                                (imagePath != null ? ", DoctorImage=@Image" : "") +
                                " WHERE DoctorId=@DoctorId";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedValue);
                    cmd.Parameters.AddWithValue("@Spec", ddlSpec.SelectedValue);
                    cmd.Parameters.AddWithValue("@Status",
                        Convert.ToBoolean(Convert.ToInt32(ddlStatus.SelectedValue)));
                    cmd.Parameters.AddWithValue("@DoctorId", doctorId);

                    if (imagePath != null)
                        cmd.Parameters.AddWithValue("@Image", imagePath);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            gvDoctors.EditIndex = -1;
            LoadDoctors();
        }

        protected void gvDoctors_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int doctorId = Convert.ToInt32(gvDoctors.DataKeys[e.RowIndex].Value);
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Doctors WHERE DoctorId=@Id", con);
                cmd.Parameters.AddWithValue("@Id", doctorId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            LoadDoctors();
        }
    }
}

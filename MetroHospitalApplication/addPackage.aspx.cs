using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetroHospitalApplication
{
    public partial class addPackage : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDoctors();
                LoadPackages();
            }
        }

        // Load doctors into dropdown
        private void LoadDoctors()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = "SELECT DoctorId, FullName FROM Doctors WHERE IsActive=1";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlDoctor.DataSource = dt;
                ddlDoctor.DataTextField = "FullName";
                ddlDoctor.DataValueField = "DoctorId";
                ddlDoctor.DataBind();
                ddlDoctor.Items.Insert(0, new ListItem("Select Doctor", "0"));
            }
        }

        // Save or Update package
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int packageId = string.IsNullOrEmpty(hfPackageId.Value) ? 0 : Convert.ToInt32(hfPackageId.Value);

            string packageName = txtPackageName.Text.Trim();
            string description = txtDescription.Text.Trim();
            decimal price = Convert.ToDecimal(txtPrice.Text.Trim());
            int duration = Convert.ToInt32(txtDuration.Text.Trim());
            int doctorId = Convert.ToInt32(ddlDoctor.SelectedValue);
            DateTime? startDate = string.IsNullOrEmpty(txtStartDate.Text) ? (DateTime?)null : Convert.ToDateTime(txtStartDate.Text);
            DateTime? endDate = string.IsNullOrEmpty(txtEndDate.Text) ? (DateTime?)null : Convert.ToDateTime(txtEndDate.Text);
            bool isPopular = chkIsPopular.Checked;
            bool isActive = chkIsActive.Checked;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                SqlCommand cmd;
                if (packageId == 0)
                {
                    // INSERT
                    cmd = new SqlCommand(@"
                        INSERT INTO Packages (PackageName, Description, Price, DurationDays, Doctor_Id, StartDate, EndDate, IsPopular, IsActive, CreatedAt)
                        VALUES (@PackageName, @Description, @Price, @DurationDays, @Doctor_Id, @StartDate, @EndDate, @IsPopular, @IsActive, GETDATE())", con);
                }
                else
                {
                    // UPDATE
                    cmd = new SqlCommand(@"
                        UPDATE Packages SET 
                            PackageName=@PackageName,
                            Description=@Description,
                            Price=@Price,
                            DurationDays=@DurationDays,
                            Doctor_Id=@Doctor_Id,
                            StartDate=@StartDate,
                            EndDate=@EndDate,
                            IsPopular=@IsPopular,
                            IsActive=@IsActive
                        WHERE PackageId=@PackageId", con);
                    cmd.Parameters.AddWithValue("@PackageId", packageId);
                }

                cmd.Parameters.AddWithValue("@PackageName", packageName);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@DurationDays", duration);
                cmd.Parameters.AddWithValue("@Doctor_Id", doctorId);
                cmd.Parameters.AddWithValue("@StartDate", (object)startDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@EndDate", (object)endDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IsPopular", isPopular);
                cmd.Parameters.AddWithValue("@IsActive", isActive);

                cmd.ExecuteNonQuery();
            }

            ClearForm();
            LoadPackages();
        }

        // Load all packages
        private void LoadPackages()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT p.*, d.FullName AS DoctorName
                    FROM Packages p
                    LEFT JOIN Doctors d ON p.Doctor_Id = d.DoctorId
                    ORDER BY p.PackageId DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvPackages.DataSource = dt;
                gvPackages.DataBind();
            }
        }

        // Clear form
        private void ClearForm()
        {
            hfPackageId.Value = "";
            txtPackageName.Text = "";
            txtDescription.Text = "";
            txtPrice.Text = "";
            txtDuration.Text = "";
            ddlDoctor.SelectedIndex = 0;
            txtStartDate.Text = "";
            txtEndDate.Text = "";
            chkIsPopular.Checked = false;
            chkIsActive.Checked = true;
            btnSave.Text = "Save";
        }

        // GridView actions: Edit / Delete
        protected void gvPackages_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int packageId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditPackage")
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Packages WHERE PackageId=@PackageId", con);
                    cmd.Parameters.AddWithValue("@PackageId", packageId);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        hfPackageId.Value = dt.Rows[0]["PackageId"].ToString();
                        txtPackageName.Text = dt.Rows[0]["PackageName"].ToString();
                        txtDescription.Text = dt.Rows[0]["Description"].ToString();
                        txtPrice.Text = dt.Rows[0]["Price"].ToString();
                        txtDuration.Text = dt.Rows[0]["DurationDays"].ToString();
                        ddlDoctor.SelectedValue = dt.Rows[0]["Doctor_Id"].ToString();
                        txtStartDate.Text = dt.Rows[0]["StartDate"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[0]["StartDate"]).ToString("yyyy-MM-dd");
                        txtEndDate.Text = dt.Rows[0]["EndDate"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[0]["EndDate"]).ToString("yyyy-MM-dd");
                        chkIsPopular.Checked = Convert.ToBoolean(dt.Rows[0]["IsPopular"]);
                        chkIsActive.Checked = Convert.ToBoolean(dt.Rows[0]["IsActive"]);

                        btnSave.Text = "Update";
                    }
                }
            }
            else if (e.CommandName == "DeletePackage")
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Packages WHERE PackageId=@PackageId", con);
                    cmd.Parameters.AddWithValue("@PackageId", packageId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                LoadPackages();
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
    }
}
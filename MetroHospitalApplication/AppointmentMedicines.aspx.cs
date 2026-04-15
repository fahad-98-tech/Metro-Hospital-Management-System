using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace MetroHospitalApplication
{
    public partial class AppointmentMedicines : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;
        int appointmentId = 0;
        int doctorId = 0; // from session

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
                LoadDoctorMedicines(); // Load dropdown first
                LoadMedicinesGrid();   // Then load existing medicines
            }
        }

        void LoadMedicinesGrid()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string q = @"SELECT MedicineId, MedicineName, Dosage, Duration, Instructions 
                             FROM AppointmentMedicines 
                             WHERE AppointmentId=@AppointmentId
                             ORDER BY MedicineId";
                SqlCommand cmd = new SqlCommand(q, con);
                cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvMedicines.DataSource = dt;
                gvMedicines.DataBind();
            }

            lblAppointment.Text = $"Appointment ID: {appointmentId}";
        }

        void LoadDoctorMedicines()
        {
            ddlMedicineName.Items.Clear();
            ddlMedicineName.Items.Add(new ListItem("-- Select or type medicine --", ""));

            using (SqlConnection con = new SqlConnection(cs))
            {
                string specQuery = "SELECT Specialization FROM Appointments WHERE AppointmentId=@AppointmentId";
                SqlCommand cmdSpec = new SqlCommand(specQuery, con);
                cmdSpec.Parameters.AddWithValue("@AppointmentId", appointmentId);

                con.Open();
                string specialization = cmdSpec.ExecuteScalar()?.ToString() ?? "";
                con.Close();

                if (!string.IsNullOrEmpty(specialization))
                {
                    string medQuery = "SELECT MedicineName FROM Medicines ";
                    SqlCommand cmdMed = new SqlCommand(medQuery, con);
                    cmdMed.Parameters.AddWithValue("@Spec", specialization);

                    SqlDataAdapter da = new SqlDataAdapter(cmdMed);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        string medName = row["MedicineName"].ToString();
                        ddlMedicineName.Items.Add(new ListItem(medName, medName));
                    }
                }
            }
        }

        protected void gvMedicines_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int medicineId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditRow")
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string q = "SELECT * FROM AppointmentMedicines WHERE MedicineId=@Id";
                    SqlCommand cmd = new SqlCommand(q, con);
                    cmd.Parameters.AddWithValue("@Id", medicineId);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        hfMedicineId.Value = medicineId.ToString();
                        string medName = dr["MedicineName"].ToString();

                        ListItem item = ddlMedicineName.Items.FindByText(medName);
                        if (item != null)
                        {
                            ddlMedicineName.SelectedValue = medName;
                            txtManualMedicine.Text = "";
                        }
                        else
                        {
                            ddlMedicineName.SelectedIndex = 0;
                            txtManualMedicine.Text = medName;
                        }

                        txtDosage.Text = dr["Dosage"].ToString();
                        txtDuration.Text = dr["Duration"].ToString();
                        txtInstructions.Text = dr["Instructions"].ToString();
                    }
                    con.Close();
                }
            }
            else if (e.CommandName == "DeleteRow")
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM AppointmentMedicines WHERE MedicineId=@Id", con);
                    cmd.Parameters.AddWithValue("@Id", medicineId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                lblMsg.Text = "✔ Medicine deleted successfully";
                LoadMedicinesGrid();
            }
        }

        protected void gvMedicines_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["class"] = "table-light";
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string medName = !string.IsNullOrEmpty(txtManualMedicine.Text.Trim())
                ? txtManualMedicine.Text.Trim()
                : ddlMedicineName.SelectedValue;

            if (string.IsNullOrEmpty(medName))
            {
                lblMsg.Text = "⚠ Please enter or select a medicine name";
                return;
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd;

                if (!string.IsNullOrEmpty(hfMedicineId.Value))
                {
                    cmd = new SqlCommand(@"UPDATE AppointmentMedicines 
                                           SET MedicineName=@Name, Dosage=@Dosage, Duration=@Duration, Instructions=@Instructions 
                                           WHERE MedicineId=@Id", con);
                    cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(hfMedicineId.Value));
                }
                else
                {
                    cmd = new SqlCommand(@"INSERT INTO AppointmentMedicines 
                                           (AppointmentId, MedicineName, Dosage, Duration, Instructions) 
                                           VALUES (@AppointmentId, @Name, @Dosage, @Duration, @Instructions)", con);
                    cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);
                }

                cmd.Parameters.AddWithValue("@Name", medName);
                cmd.Parameters.AddWithValue("@Dosage", txtDosage.Text.Trim());
                cmd.Parameters.AddWithValue("@Duration", txtDuration.Text.Trim());
                cmd.Parameters.AddWithValue("@Instructions", txtInstructions.Text.Trim());

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            lblMsg.Text = "✔ Medicine saved successfully";
            ClearForm();
            LoadMedicinesGrid();
            LoadDoctorMedicines();
        }

        void ClearForm()
        {
            hfMedicineId.Value = "";
            ddlMedicineName.SelectedIndex = 0;
            txtManualMedicine.Text = "";
            txtDosage.Text = "";
            txtDuration.Text = "";
            txtInstructions.Text = "";
        }
    }
}

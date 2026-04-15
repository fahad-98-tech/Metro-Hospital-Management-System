using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace MetroHospitalApplication
{
    public partial class shift_system : System.Web.UI.Page
    {
        string conStr = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDoctors();
                txtShiftDate.Text = DateTime.Today.ToString("yyyy-MM-dd");

             

                // ✅ Disable past dates
                txtShiftDate.Attributes["min"] = DateTime.Today.ToString("yyyy-MM-dd");
                LoadShifts();
            }
        }

        void LoadDoctors()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT DoctorId, FullName FROM Doctors WHERE IsActive=1", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlDoctor.DataSource = dt;
                ddlDoctor.DataTextField = "FullName";
                ddlDoctor.DataValueField = "DoctorId";
                ddlDoctor.DataBind();
                ddlDoctor.Items.Insert(0, new ListItem("-- Select Doctor --", "0"));
            }
        }

        protected void btnSaveShift_Click(object sender, EventArgs e)
        {
            if (ddlDoctor.SelectedValue == "0")
            {
                ShowAlert("Please select a doctor.", false);
                return;
            }

            string shiftStart = "", shiftEnd = "", shiftType = "";

            if (rbMorning.Checked) { shiftType = "Morning"; shiftStart = "07:00 AM"; shiftEnd = "01:00 PM"; }
            else if (rbAfternoon.Checked) { shiftType = "Afternoon"; shiftStart = "01:00 PM"; shiftEnd = "05:00 PM"; }
            else if (rbEvening.Checked) { shiftType = "Evening"; shiftStart = "05:00 PM"; shiftEnd = "10:00 PM"; }
            else if (rbFullDay.Checked) { shiftType = "Full Day"; shiftStart = "07:00 AM"; shiftEnd = "10:00 PM"; }
            else { ShowAlert("Select shift type.", false); return; }

            DateTime shiftDate = DateTime.Parse(txtShiftDate.Text);

            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();

                SqlCommand cmd;

                if (!string.IsNullOrEmpty(hfShiftId.Value))
                {
                    // UPDATE
                    cmd = new SqlCommand(@"
                        UPDATE DoctorShifts 
                        SET DoctorId=@D, ShiftDate=@Dt, ShiftStart=@Start, ShiftEnd=@End, ShiftType=@Type
                        WHERE ShiftId=@Id", con);

                    cmd.Parameters.AddWithValue("@Id", hfShiftId.Value);
                }
                else
                {
                    // INSERT
                    cmd = new SqlCommand(@"
                        INSERT INTO DoctorShifts 
                        (DoctorId, ShiftDate, ShiftStart, ShiftEnd, ShiftType, IsActive)
                        VALUES (@D,@Dt,@Start,@End,@Type,1)", con);
                }

                cmd.Parameters.AddWithValue("@D", ddlDoctor.SelectedValue);
                cmd.Parameters.AddWithValue("@Dt", shiftDate.Date);
                cmd.Parameters.AddWithValue("@Start", shiftStart);
                cmd.Parameters.AddWithValue("@End", shiftEnd);
                cmd.Parameters.AddWithValue("@Type", shiftType);

                cmd.ExecuteNonQuery();
            }

            hfShiftId.Value = "";
            btnSaveShift.Text = "Save Shift";

            ShowAlert("Saved successfully!", true);
            LoadShifts();
        }

        void LoadShifts()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(@"
                    SELECT ds.ShiftId, ds.ShiftDate, d.FullName AS DoctorName,
                           ds.ShiftType, ds.ShiftStart, ds.ShiftEnd
                    FROM DoctorShifts ds
                    INNER JOIN Doctors d ON ds.DoctorId = d.DoctorId
                    WHERE ds.IsActive=1
                    ORDER BY ds.ShiftDate", con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvShifts.DataSource = dt;
                gvShifts.DataBind();
            }
        }

        protected void gvShifts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditShift")
                LoadShiftForEdit(id);

            else if (e.CommandName == "DeleteShift")
            {
                DeleteShift(id);
                LoadShifts();
            }
        }

        void LoadShiftForEdit(int id)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM DoctorShifts WHERE ShiftId=@Id", con);
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    hfShiftId.Value = id.ToString();
                    ddlDoctor.SelectedValue = dr["DoctorId"].ToString();
                    txtShiftDate.Text = Convert.ToDateTime(dr["ShiftDate"]).ToString("yyyy-MM-dd");

                    string type = dr["ShiftType"].ToString();

                    rbMorning.Checked = type == "Morning";
                    rbAfternoon.Checked = type == "Afternoon";
                    rbEvening.Checked = type == "Evening";
                    rbFullDay.Checked = type == "Full Day";

                    btnSaveShift.Text = "Update Shift";
                }
            }
        }

        void DeleteShift(int id)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("UPDATE DoctorShifts SET IsActive=0 WHERE ShiftId=@Id", con);
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        void ShowAlert(string msg, bool success)
        {
            string color = success ? "success" : "danger";

            string script = $@"
            <script>
                var div = document.createElement('div');
                div.className = 'alert alert-{color} position-fixed top-0 end-0 m-3';
                div.innerHTML = '{msg}';
                document.body.appendChild(div);
                setTimeout(() => div.remove(), 3000);
            </script>";

            ClientScript.RegisterStartupScript(this.GetType(), "alert", script, false);
        }
    }
}
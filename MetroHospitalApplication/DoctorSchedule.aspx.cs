using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetroHospitalApplication
{
    public partial class DoctorSchedule : System.Web.UI.Page
    {
        string conStr = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Session["DoctorId"] != null)
                this.MasterPageFile = "~/Doctor.Master";
            else
                this.MasterPageFile = "~/Admin.Master";
        }

        int GetDoctorId()
        {
            if (Session["DoctorId"] != null)
                return Convert.ToInt32(Session["DoctorId"]);

            if (ddlDoctor.SelectedValue != "")
                return Convert.ToInt32(ddlDoctor.SelectedValue);

            return 0;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDate.Text = DateTime.Today.ToString("yyyy-MM-dd");

                if (Session["DoctorId"] == null)
                {
                    pnlDoctorSelect.Visible = true;
                    LoadDoctors();
                }

                BindSpecializationDropdown();
                LoadSpecializationBoxes();
                LoadFullDaySchedule();
            }
        }

        void LoadDoctors()
        {
            ddlDoctor.Items.Clear();

            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT DoctorId,FullName FROM Doctors", con);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                ddlDoctor.Items.Add(new ListItem("--Select Doctor--", ""));

                while (dr.Read())
                {
                    ddlDoctor.Items.Add(
                        new ListItem(
                        dr["FullName"].ToString(),
                        dr["DoctorId"].ToString()));
                }
            }
        }

        protected void gvFullSchedule_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string status = e.Row.Cells[1].Text.Trim();

                if (status.Equals("AVAILABLE", StringComparison.OrdinalIgnoreCase))
                {
                    e.Row.Cells[1].CssClass = "available";
                }
                else if (status.Equals("BOOKED", StringComparison.OrdinalIgnoreCase))
                {
                    e.Row.Cells[1].CssClass = "booked";
                }
                else if (status.Equals("BREAK", StringComparison.OrdinalIgnoreCase))
                {
                    e.Row.Cells[1].CssClass = "break";
                }
            }
        }
        void BindSpecializationDropdown()
        {
            ddlSpecialization.Items.Clear();

            int doctorId = GetDoctorId();

            if (doctorId == 0) return;

            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(
                "SELECT Specialization FROM Doctors WHERE DoctorId=@D", con);

                cmd.Parameters.AddWithValue("@D", doctorId);

                con.Open();

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    ddlSpecialization.Items.Add(new ListItem("--Select--", ""));

                    string[] specs = result.ToString().Split(',');

                    foreach (string s in specs)
                    {
                        string spec = s.Trim();

                        if (spec != "")
                            ddlSpecialization.Items.Add(new ListItem(spec, spec));
                    }
                }
            }
        }

        void LoadSpecializationBoxes()
        {
            specContainer.Controls.Clear();

            int doctorId = GetDoctorId();

            if (doctorId == 0) return;

            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(
                "SELECT Specialization FROM Doctors WHERE DoctorId=@D", con);

                cmd.Parameters.AddWithValue("@D", doctorId);

                con.Open();

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    string[] specs = result.ToString().Split(',');

                    foreach (string spec in specs)
                    {
                        System.Web.UI.HtmlControls.HtmlGenericControl div =
                        new System.Web.UI.HtmlControls.HtmlGenericControl("div");

                        div.InnerHtml = spec.Trim();

                        div.Attributes["class"] = "spec-box bg-primary";

                        specContainer.Controls.Add(div);
                    }
                }
            }
        }

        protected void ddlDoctor_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSpecializationDropdown();
            LoadSpecializationBoxes();
            LoadFullDaySchedule();
        }

        protected void ddlSpecialization_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFullDaySchedule();
        }

        protected void txtDate_TextChanged(object sender, EventArgs e)
        {
            LoadFullDaySchedule();
        }

        void LoadFullDaySchedule()
        {
            int doctorId = GetDoctorId();

            if (doctorId == 0 || ddlSpecialization.SelectedValue == "")
                return;

            DateTime selectedDate = DateTime.Parse(txtDate.Text);

            DataTable dt = new DataTable();

            dt.Columns.Add("TimeSlot");
            dt.Columns.Add("Status");

            HashSet<string> bookedSlots = new HashSet<string>();

            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(@"
                SELECT AppointmentTime,AppointmentEndTime
                FROM Appointments
                WHERE DoctorId=@D
                AND Specialization=@S
                AND AppointmentDate=@Dt", con);

                cmd.Parameters.AddWithValue("@D", doctorId);
                cmd.Parameters.AddWithValue("@S", ddlSpecialization.SelectedValue);
                cmd.Parameters.AddWithValue("@Dt", selectedDate.ToString("yyyy-MM-dd"));

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    DateTime start = DateTime.ParseExact(dr["AppointmentTime"].ToString(), "HH:mm", null);
                    DateTime end = DateTime.ParseExact(dr["AppointmentEndTime"].ToString(), "HH:mm", null);

                    while (start < end)
                    {
                        bookedSlots.Add(start.ToString("HH:mm"));
                        start = start.AddMinutes(30);
                    }
                }
            }

            DateTime t = selectedDate.AddHours(7);
            DateTime endDay = selectedDate.AddDays(1).AddHours(6);

            while (t < endDay)
            {
                string range = t.ToString("HH:mm") + " - " + t.AddMinutes(30).ToString("HH:mm");

                string status;

                if (IsBreakTime(t))
                    status = "BREAK";
                else if (bookedSlots.Contains(t.ToString("HH:mm")))
                    status = "BOOKED";
                else
                    status = "AVAILABLE";

                dt.Rows.Add(range, status);

                t = t.AddMinutes(30);
            }

            gvFullSchedule.DataSource = dt;
            gvFullSchedule.DataBind();
        }

        bool IsBreakTime(DateTime t)
        {
            if (t.TimeOfDay >= new TimeSpan(10, 0, 0) && t.TimeOfDay < new TimeSpan(10, 15, 0))
                return true;

            if (t.TimeOfDay >= new TimeSpan(13, 0, 0) && t.TimeOfDay < new TimeSpan(14, 0, 0))
                return true;

            if (t.TimeOfDay >= new TimeSpan(16, 0, 0) && t.TimeOfDay < new TimeSpan(16, 15, 0))
                return true;

            if (t.TimeOfDay >= new TimeSpan(20, 0, 0) && t.TimeOfDay < new TimeSpan(21, 0, 0))
                return true;

            return false;
        }

        
    }
}
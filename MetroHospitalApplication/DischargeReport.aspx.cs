using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetroHospitalApplication
{
    public partial class DischargeReport : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDischargeData();
            }
        }

        private void LoadDischargeData()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                    SELECT a.AppointmentId, a.PatientName, a.PatientMobile,
                           a.Specialization, a.AppointmentDate, a.DischargeStatus,
                           d.FullName AS DoctorName
                    FROM Appointments a
                    LEFT JOIN Doctors d ON a.DoctorId = d.DoctorId
                    WHERE a.DischargeStatus='Done'
                    ORDER BY a.AppointmentDate DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvDischarge.DataSource = dt;
                gvDischarge.DataBind();
            }
        }

        protected void gvDischarge_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string status = DataBinder.Eval(e.Row.DataItem, "DischargeStatus").ToString();
                if (status == "Done")
                {
                    e.Row.CssClass = "discharge-done";
                }
            }
        }

        protected void btnViewAppointment_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int appointmentId = Convert.ToInt32(btn.CommandArgument);

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"
                    SELECT a.PatientName, a.PatientMobile, a.Specialization,
                           a.AppointmentDate, a.AppointmentTime, a.AppointmentEndTime,
                           d.FullName AS DoctorName
                    FROM Appointments a
                    LEFT JOIN Doctors d ON a.DoctorId = d.DoctorId
                    WHERE a.AppointmentId=@AppointmentId", con);
                cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    lblDetails.Text = $@"
                        <b>Patient:</b> {dr["PatientName"]}<br/>
                        <b>Mobile:</b> {dr["PatientMobile"]}<br/>
                        <b>Doctor:</b> {dr["DoctorName"]}<br/>
                        <b>Department:</b> {dr["Specialization"]}<br/>
                        <b>Date:</b> {Convert.ToDateTime(dr["AppointmentDate"]).ToString("dd-MMM-yyyy")}<br/>
                        <b>Time:</b> {dr["AppointmentTime"]} - {dr["AppointmentEndTime"]}";
                }
            }

            pnlAppointmentDetails.Style["display"] = "block";
            modalBackground.Style["display"] = "block";
        }

        protected void btnViewInvoice_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int appointmentId = Convert.ToInt32(btn.CommandArgument);
            Response.Redirect("AdminInvoice.aspx?aid=" + appointmentId);
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            pnlAppointmentDetails.Style["display"] = "none";
            modalBackground.Style["display"] = "none";
        }
    }
}
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetroHospitalApplication
{
    public partial class DoctorAppointments : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                LoadAppointments();
            }
        }

        void LoadAppointments()
        {
            int doctorId = Convert.ToInt32(Session["DoctorId"]);

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                    SELECT AppointmentId, PatientName, PatientMobile,
                           AppointmentTime, AppointmentEndTime,
                           Status, Specialization
                    FROM Appointments
                    WHERE DoctorId = @DoctorId
                      AND AppointmentDate = @Date
                    ORDER BY AppointmentTime";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@DoctorId", doctorId);
                cmd.Parameters.AddWithValue("@Date", txtDate.Text);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvAppointments.DataSource = dt;
                gvAppointments.DataBind();

                lblMsg.Text = dt.Rows.Count == 0
                    ? "ℹ No appointments scheduled for selected date"
                    : "";
            }
        }

        protected void txtDate_TextChanged(object sender, EventArgs e)
        {
            LoadAppointments();
        }

        protected void gvAppointments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string status = DataBinder.Eval(e.Row.DataItem, "Status")?.ToString();
                DropDownList ddl = (DropDownList)e.Row.FindControl("ddlStatus");

                if (!string.IsNullOrEmpty(status) && ddl.Items.FindByText(status) != null)
                    ddl.SelectedValue = status;
                else
                    ddl.SelectedIndex = 0;
            }
        }

        protected void gvAppointments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int appointmentId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "UpdateStatus")
            {
                GridViewRow row = (GridViewRow)((Button)e.CommandSource).NamingContainer;
                DropDownList ddl = (DropDownList)row.FindControl("ddlStatus");

                UpdateStatus(appointmentId, ddl.SelectedValue);
                LoadAppointments();
            }
            else if (e.CommandName == "Treatment")
            {
                Response.Redirect("AppointmentTreatment.aspx?aid=" + appointmentId);
            }
            else if (e.CommandName == "Medicines")
            {
                Response.Redirect("AppointmentMedicines.aspx?aid=" + appointmentId);
            }
            else if (e.CommandName == "Reports")
            {
                Response.Redirect("AppointmentReports.aspx?aid=" + appointmentId);
            }
            else if (e.CommandName == "Invoice")
            {
                Response.Redirect("AppointmentInvoice.aspx?aid=" + appointmentId);
            }
        }

        void UpdateStatus(int appointmentId, string status)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                if (status == "Done")
                {


                    SqlCommand cmd = new SqlCommand(
                        "UPDATE Appointments SET Status=@Status , DischargeStatus='Done' WHERE AppointmentId=@Id", con);

                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@Id", appointmentId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    SqlCommand cmd = new SqlCommand(
                        "UPDATE Appointments SET Status=@Status, DischargeStatus='Pending' WHERE AppointmentId=@Id", con);

                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@Id", appointmentId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            lblMsg.Text = "✔ Appointment status updated successfully";
            lblMsg.CssClass = "text-success";
        }
    }
}

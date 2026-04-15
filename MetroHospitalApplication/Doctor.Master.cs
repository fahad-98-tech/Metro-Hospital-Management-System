using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetroHospitalApplication
{
	public partial class Doctor : System.Web.UI.MasterPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
           
                if (!IsPostBack)
                {
                    UpdateAppointmentNotifications();
                }
            

        }
        private void UpdateAppointmentNotifications()
        {
            if (Session["DoctorId"] == null) return;

            int doctorId = Convert.ToInt32(Session["DoctorId"]);
            string cs = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"SELECT COUNT(*) 
                         FROM Appointments 
                         WHERE DoctorId=@DoctorId 
                           AND AppointmentDate=@Today 
                           AND IsActive=1";  // Optionally, filter pending only with AND Status='Pending'

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@DoctorId", doctorId);
                cmd.Parameters.AddWithValue("@Today", DateTime.Today);

                con.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();

                notifCount.InnerText = count.ToString(); // Updates the sidebar badge
            }
        }

    }
}
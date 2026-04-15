using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Web.UI;

namespace MetroHospitalApplication
{
    public partial class AdminHome : Page
    {
        string conStr = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        // JSON strings for Chart.js
        protected string DoctorNamesJson = "[]";
        protected string DoctorAppointmentsJson = "[]";
        protected string DepartmentNamesJson = "[]";
        protected string DepartmentCountsJson = "[]";
        protected string PatientsSpecializationNamesJson = "[]";
        protected string PatientsSpecializationCountsJson = "[]";
        protected string DailyAdmissionsJson = "[0,0,0,0,0,0,0]";
        protected string DailyDischargesJson = "[0,0,0,0,0,0,0]";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadKPIs();
                LoadPatientsPerDoctorChart();
                LoadDoctorsBySpecializationChart();
                LoadPatientsPerSpecializationChart();
                LoadDailyAdmissionsDischargesChart();
                LoadDoctorsOnDuty();
            }
        }
        private void LoadDoctorsOnDuty()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "SELECT COUNT(*) FROM Doctors WHERE Isactive = 1";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    Label1.Text = count.ToString();
                }
            }
        }
        private void LoadKPIs()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                lblTotalPatients.Text = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Role='Patient'", con).ExecuteScalar().ToString();
                lblTodayPatients.Text = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Role='Patient' AND CAST(CreatedDate AS DATE)=CAST(GETDATE() AS DATE)", con).ExecuteScalar().ToString();
                lblTotalDoctors.Text = new SqlCommand("SELECT COUNT(*) FROM Doctors", con).ExecuteScalar().ToString();
                lblUpcomingAppointments.Text = new SqlCommand("SELECT COUNT(*) FROM Appointments WHERE AppointmentDate >= CAST(GETDATE() AS DATE)", con).ExecuteScalar().ToString();
                lblCompletedAppointments.Text = new SqlCommand("SELECT COUNT(*) FROM Appointments WHERE Status='Done'", con).ExecuteScalar().ToString();
                lblPendingAppointments.Text = new SqlCommand("SELECT COUNT(*) FROM Appointments WHERE Status<>'Done'", con).ExecuteScalar().ToString();
                lblDischargedPatients.Text = new SqlCommand("SELECT COUNT(*) FROM Appointments WHERE DischargeStatus='Done'", con).ExecuteScalar().ToString();
            }
        }

        private void LoadPatientsPerDoctorChart()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"
                    SELECT d.FullName, COUNT(a.AppointmentId) AS Total
                    FROM Doctors d
                    LEFT JOIN Appointments a ON a.DoctorId=d.DoctorId AND CAST(a.AppointmentDate AS DATE)=CAST(GETDATE() AS DATE)
                    GROUP BY d.FullName
                    ORDER BY d.FullName";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.Fill(dt);
            }

            StringBuilder names = new StringBuilder("[");
            StringBuilder counts = new StringBuilder("[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                names.Append("'" + dt.Rows[i]["FullName"] + "'");
                counts.Append(dt.Rows[i]["Total"]);
                if (i < dt.Rows.Count - 1) { names.Append(","); counts.Append(","); }
            }
            names.Append("]"); counts.Append("]");
            DoctorNamesJson = names.ToString();
            DoctorAppointmentsJson = counts.ToString();
        }

        private void LoadDoctorsBySpecializationChart()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "SELECT Specialization, COUNT(DoctorId) AS TotalDoctors FROM Doctors GROUP BY Specialization ORDER BY Specialization";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.Fill(dt);
            }

            StringBuilder names = new StringBuilder("[");
            StringBuilder counts = new StringBuilder("[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                names.Append("'" + dt.Rows[i]["Specialization"] + "'");
                counts.Append(dt.Rows[i]["TotalDoctors"]);
                if (i < dt.Rows.Count - 1) { names.Append(","); counts.Append(","); }
            }
            names.Append("]"); counts.Append("]");
            DepartmentNamesJson = names.ToString();
            DepartmentCountsJson = counts.ToString();
        }

        private void LoadPatientsPerSpecializationChart()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"
                    SELECT d.Specialization, COUNT(a.AppointmentId) AS TotalPatients
                    FROM Appointments a
                    INNER JOIN Doctors d ON a.DoctorId=d.DoctorId
                    GROUP BY d.Specialization
                    ORDER BY d.Specialization";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.Fill(dt);
            }

            StringBuilder names = new StringBuilder("[");
            StringBuilder counts = new StringBuilder("[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                names.Append("'" + dt.Rows[i]["Specialization"] + "'");
                counts.Append(dt.Rows[i]["TotalPatients"]);
                if (i < dt.Rows.Count - 1) { names.Append(","); counts.Append(","); }
            }
            names.Append("]"); counts.Append("]");
            PatientsSpecializationNamesJson = names.ToString();
            PatientsSpecializationCountsJson = counts.ToString();
        }

        private void LoadDailyAdmissionsDischargesChart()
        {
            DataTable dtAdmissions = new DataTable();
            DataTable dtDischarges = new DataTable();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string queryAdmissions = @"
            SELECT DATENAME(WEEKDAY, AppointmentDate) AS DayName, COUNT(AppointmentId) AS Total
            FROM Appointments
            WHERE AppointmentDate >= DATEADD(DAY, -6, CAST(GETDATE() AS DATE))
            GROUP BY DATENAME(WEEKDAY, AppointmentDate), AppointmentDate
            ORDER BY AppointmentDate";
                SqlDataAdapter da1 = new SqlDataAdapter(queryAdmissions, con);
                da1.Fill(dtAdmissions);

                string queryDischarges = @"
            SELECT DATENAME(WEEKDAY, AppointmentDate) AS DayName, COUNT(AppointmentId) AS Total
            FROM Appointments
            WHERE DischargeStatus='Done' AND AppointmentDate >= DATEADD(DAY, -6, CAST(GETDATE() AS DATE))
            GROUP BY DATENAME(WEEKDAY, AppointmentDate), AppointmentDate
            ORDER BY AppointmentDate";
                SqlDataAdapter da2 = new SqlDataAdapter(queryDischarges, con);
                da2.Fill(dtDischarges);
            }

            int[] admissions = new int[7];
            int[] discharges = new int[7];
            string[] weekDays = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };

            // Fill admissions array
            foreach (DataRow dr in dtAdmissions.Rows)
            {
                string dayName = dr["DayName"].ToString().Substring(0, 3);
                int index = Array.IndexOf(weekDays, dayName);
                if (index >= 0)
                    admissions[index] = Convert.ToInt32(dr["Total"]);
            }

            // Fill discharges array
            foreach (DataRow dr in dtDischarges.Rows)
            {
                string dayName = dr["DayName"].ToString().Substring(0, 3);
                int index = Array.IndexOf(weekDays, dayName);
                if (index >= 0)
                    discharges[index] = Convert.ToInt32(dr["Total"]);
            }

            DailyAdmissionsJson = "[" + string.Join(",", admissions) + "]";
            DailyDischargesJson = "[" + string.Join(",", discharges) + "]";
        }
    }
}
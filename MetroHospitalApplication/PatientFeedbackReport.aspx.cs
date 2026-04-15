using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace MetroHospitalApplication
{
    public partial class PatientFeedbackReport : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void BindGrid()
        {
            int doctorId = Convert.ToInt32(Session["DoctorId"]); // Assuming you store logged-in DoctorId in Session
            DateTime? fromDate = string.IsNullOrEmpty(txtFromDate.Text) ? (DateTime?)null : Convert.ToDateTime(txtFromDate.Text);
            DateTime? toDate = string.IsNullOrEmpty(txtToDate.Text) ? (DateTime?)null : Convert.ToDateTime(txtToDate.Text);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT f.FeedbackId, u.FullName AS PatientName, 
                           a.AppointmentDate, f.Rating, f.Comments
                    FROM PatientFeedback f
                    INNER JOIN Users u ON f.PatientId=u.UserId
                    LEFT JOIN Appointments a ON f.AppointmentId=a.AppointmentId
                    WHERE f.DoctorId=@DoctorId";

                if (fromDate.HasValue)
                    query += " AND CAST(f.CreatedAt AS DATE)>=@FromDate";
                if (toDate.HasValue)
                    query += " AND CAST(f.CreatedAt AS DATE)<=@ToDate";

                query += " ORDER BY f.CreatedAt DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@DoctorId", doctorId);
                if (fromDate.HasValue) cmd.Parameters.AddWithValue("@FromDate", fromDate.Value.Date);
                if (toDate.HasValue) cmd.Parameters.AddWithValue("@ToDate", toDate.Value.Date);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Convert numeric rating to stars
                dt.Columns.Add("Stars", typeof(string));
                dt.Columns.Add("RatingCssClass", typeof(string));

                foreach (DataRow row in dt.Rows)
                {
                    int rating = Convert.ToInt32(row["Rating"]);
                    string stars = new string('★', rating) + new string('☆', 5 - rating);
                    row["Stars"] = stars;

                    // Blink for 5-star rating
                    row["RatingCssClass"] = rating == 5 ? "star-rating blink" : "star-rating";
                }

                gvFeedback.DataSource = dt;
                gvFeedback.DataBind();

                lblTotalFeedbacks.Text = dt.Rows.Count.ToString();
                lblAverageRating.Text = dt.Rows.Count > 0 ? Math.Round(Convert.ToDecimal(dt.Compute("AVG(Rating)", "")), 2).ToString() : "0";
            }
        }

        protected void gvFeedback_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Row styling handled by RatingCssClass in template
        }
    }
}
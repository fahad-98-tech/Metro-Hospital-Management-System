using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;

namespace MetroHospitalApplication
{
    public partial class TestList : System.Web.UI.Page
    {
        string conStr = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTests();
                txtPaymentDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            }
        }

        void LoadTests()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"
                SELECT 
                    pt.PatientTestId,
                    u.FullName AS PatientName,
                    u.MobileNumber,
                    u.Email,
                    t.TestName,
                    t.Price AS Amount,
                    pt.TestDate,
                    pt.TestTime,
                    pt.Status,
                    pt.Result,
                    ISNULL(tp.TotalPaid, 0) AS PaidAmount,
                    ISNULL(tp.LastPaymentMode, '') AS PaymentMode,
                    ISNULL(tp.LastPaymentDate, GETDATE()) AS PaymentDate
                FROM PatientTests pt
                INNER JOIN Users u ON pt.PatientId = u.UserId
                INNER JOIN Tests t ON pt.TestId = t.TestId
                LEFT JOIN (
                    SELECT PatientTestId,
                           SUM(Amount) AS TotalPaid,
                           MAX(PaymentMode) AS LastPaymentMode,
                           MAX(PaymentDate) AS LastPaymentDate
                    FROM TestPayments
                    GROUP BY PatientTestId
                ) tp ON pt.PatientTestId = tp.PatientTestId
                ORDER BY pt.TestDate DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvTests.DataSource = dt;
                gvTests.DataBind();
            }
        }

        protected void gvTests_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "PayNow")
            {
                hfPaymentTestId.Value = id.ToString();

                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand(@"
                        SELECT t.Price - ISNULL(SUM(p.Amount),0)
                        FROM PatientTests pt
                        INNER JOIN Tests t ON pt.TestId = t.TestId
                        LEFT JOIN TestPayments p ON pt.PatientTestId = p.PatientTestId
                        WHERE pt.PatientTestId=@Id
                        GROUP BY t.Price", con);

                    cmd.Parameters.AddWithValue("@Id", id);
                    con.Open();
                    txtAmount.Text = cmd.ExecuteScalar().ToString();
                }

                txtPaymentDate.Text = DateTime.Today.ToString("yyyy-MM-dd");

                ScriptManager.RegisterStartupScript(this, this.GetType(),
                    "popup", "var myModal = new bootstrap.Modal(document.getElementById('paymentModal')); myModal.show();", true);
            }
            else if (e.CommandName == "EditRow")
            {
                hfEditTestId.Value = id.ToString();

                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand(@"
                        SELECT TestDate, TestTime, Status, Result
                        FROM PatientTests
                        WHERE PatientTestId=@Id", con);

                    cmd.Parameters.AddWithValue("@Id", id);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        txtEditTestDate.Text = Convert.ToDateTime(dr["TestDate"]).ToString("yyyy-MM-dd");
                        txtEditTestTime.Text = dr["TestTime"].ToString();
                        ddlEditStatus.SelectedValue = dr["Status"].ToString();
                        txtEditResult.Text = dr["Result"].ToString();
                    }
                    dr.Close();
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(),
                    "editModal", "var myModal = new bootstrap.Modal(document.getElementById('editModal')); myModal.show();", true);
            }
            else if (e.CommandName == "DeleteRow")
            {
                DeleteTest(id);
                LoadTests();
            }
        }

        protected void btnSavePayment_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(@"
                    INSERT INTO TestPayments
                    (PatientTestId, Amount, PaymentMode, TransactionDetails, PaymentDate)
                    VALUES (@TestId, @Amt, @Mode, @Details, @PayDate)", con);

                cmd.Parameters.AddWithValue("@TestId", hfPaymentTestId.Value);
                cmd.Parameters.AddWithValue("@Amt", txtAmount.Text);
                cmd.Parameters.AddWithValue("@Mode", ddlPaymentMode.SelectedValue);
                cmd.Parameters.AddWithValue("@Details", txtDetails.Text);
                cmd.Parameters.AddWithValue("@PayDate", txtPaymentDate.Text);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(),
                "hidePayment", "bootstrap.Modal.getInstance(document.getElementById('paymentModal')).hide();", true);

            LoadTests();
        }

        protected void btnSaveEdit_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(@"
                    UPDATE PatientTests
                    SET TestDate=@Date,
                        TestTime=@Time,
                        Status=@Status,
                        Result=@Result
                    WHERE PatientTestId=@Id", con);

                cmd.Parameters.AddWithValue("@Date", txtEditTestDate.Text);
                cmd.Parameters.AddWithValue("@Time", txtEditTestTime.Text);
                cmd.Parameters.AddWithValue("@Status", ddlEditStatus.SelectedValue);
                cmd.Parameters.AddWithValue("@Result", txtEditResult.Text);
                cmd.Parameters.AddWithValue("@Id", hfEditTestId.Value);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(),
                "hideEdit", "bootstrap.Modal.getInstance(document.getElementById('editModal')).hide();", true);

            LoadTests();
        }

        void DeleteTest(int id)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM PatientTests WHERE PatientTestId=@Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
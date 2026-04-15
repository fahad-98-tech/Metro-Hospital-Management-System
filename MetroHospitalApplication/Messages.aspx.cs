using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetroHospitalApplication
{
    public partial class Messages : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMessages();
            }
        }

        private void LoadMessages()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT MessageId, Name, Email, Subject, Message, CreatedDate, IsRead FROM ContactMessages ORDER BY CreatedDate DESC", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvMessages.DataSource = dt;
                gvMessages.DataBind();
            }
        }

        protected void gvMessages_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chk = (CheckBox)e.Row.FindControl("chkRead");
                HiddenField hf = (HiddenField)e.Row.FindControl("hfMessageId");

                DataRowView drv = (DataRowView)e.Row.DataItem;
                chk.Checked = Convert.ToBoolean(drv["IsRead"]);
            }
        }

        protected void chkRead_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            GridViewRow row = (GridViewRow)chk.NamingContainer;
            HiddenField hf = (HiddenField)row.FindControl("hfMessageId");
            int messageId = Convert.ToInt32(hf.Value);

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE ContactMessages SET IsRead=@isRead WHERE MessageId=@id", con);
                cmd.Parameters.AddWithValue("@isRead", chk.Checked ? 1 : 0);
                cmd.Parameters.AddWithValue("@id", messageId);
                cmd.ExecuteNonQuery();
            }

            string status = chk.Checked ? "marked as read" : "marked as unread";
            ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Message {status}');", true);
        }
    }
}
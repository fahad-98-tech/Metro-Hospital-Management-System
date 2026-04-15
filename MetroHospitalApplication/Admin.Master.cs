using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace MetroHospitalApplication
{
    public partial class Admin : MasterPage
    {
        string cs = ConfigurationManager.ConnectionStrings["MetroHospitalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Redirect if not logged in
                if (Session["UserId"] == null && !Request.Url.AbsolutePath.EndsWith("Login.aspx"))
                {
                    Response.Redirect("~/Login.aspx");
                }

                // Load unread messages count
                LoadUnreadMessagesCount();
            }
        }

        private void LoadUnreadMessagesCount()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM ContactMessages WHERE IsRead = 0", con);
                    int unreadCount = Convert.ToInt32(cmd.ExecuteScalar());

                    if (unreadCount > 0)
                    {
                        lblUnreadCount.InnerText = unreadCount.ToString();
                    }
                    else
                    {
                        lblUnreadCount.InnerText = "";
                    }
                }
            }
            catch (Exception ex)
            {
                // Optionally log error
                lblUnreadCount.InnerText = "";
            }
        }
    }
}
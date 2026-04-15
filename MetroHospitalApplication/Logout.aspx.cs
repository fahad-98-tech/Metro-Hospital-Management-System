using System;
using System.Web;
using System.Web.Security;

namespace MetroHospitalApplication
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Clear all session data
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            // Optionally clear authentication cookie if using FormsAuthentication
            if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName)
                {
                    Expires = DateTime.Now.AddDays(-1),
                    HttpOnly = true
                };
                Response.Cookies.Add(cookie);
            }

            // Redirect to login page
            Response.Redirect("Login.aspx");
        }
    }
}
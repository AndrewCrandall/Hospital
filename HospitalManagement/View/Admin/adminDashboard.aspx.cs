using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace HospitalManagement.View
{
    public partial class adminDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Debugging output
            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (!User.Identity.IsAuthenticated)
            {
                Response.Write("<script>alert('User is not authenticated. Redirecting to login.');</script>");
                Response.Redirect("~/View/Login.aspx");
            }
            else
            {
                string username = User.Identity.Name;
                welcomeLabel.Text = $"Welcome, {username}!";
            }
        }

        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            Response.Redirect("~/View/Login.aspx");
        }

    }
}

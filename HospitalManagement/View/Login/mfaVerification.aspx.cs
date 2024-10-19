using System;
using System.Web;
using System.Web.UI;

namespace HospitalManagement
{
    public partial class MfaVerification : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void mfaVerifyBtn_Click(object sender, EventArgs e)
        {
            var storedMfaCode = Session["MfaCode"]?.ToString();
            var username = Session["Username"]?.ToString();
            var userType = Session["UserType"]?.ToString();

            if (mfaCodeTxt.Text == storedMfaCode)
            {
                // Clear the session
                Session.Remove("MfaCode");
                Session.Remove("Username");
                Session.Remove("UserType");

                // Create an instance of LoginManager
                LoginManager userDataAccess = new LoginManager();

                // Create the authentication cookie
                userDataAccess.CreateAuthCookie(username, userType, HttpContext.Current);

                // Redirect to the appropriate dashboard
                userDataAccess.RedirectUser(userType, HttpContext.Current);
            }
            else
            {
                Response.Write("<script>alert('Invalid MFA code.');</script>");
            }
        }
    }
}

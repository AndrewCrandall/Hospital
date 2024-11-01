using HospitalManagement.DataAccess;
using HospitalManagement.Utilities; // Include the utility namespace
using System;
using System.Web;

namespace HospitalManagement.View.Login
{
    public partial class mfaVerification : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if the session variables are null
            if (Session["Username"] == null || Session["UserType"] == null)
            {
                // Redirect to the login page or an error page
                Response.Redirect("~/View/Login.aspx");
            }
        }

        protected void mfaVerifyBtn_Click(object sender, EventArgs e)
        {
            var storedMfaCode = Session["MfaCode"]?.ToString();
            var username = Session["Username"]?.ToString();
            var userType = Session["UserType"]?.ToString();

            // Sanitize and validate the MFA code
            string mfaCodeInput = InputValidator.SanitizeInput(mfaCodeTxt.Text);

            if (!InputValidator.IsValidMfaCode(mfaCodeInput))
            {
                Response.Write("<script>alert('MFA code must be exactly six digits.');</script>");
                return;
            }

            if (mfaCodeInput == storedMfaCode)
            {
                // Create an instance of LoginManager
                LoginManager userDataAccess = new LoginManager();

                // Create the authentication cookie
                userDataAccess.CreateAuthCookie(username, userType, HttpContext.Current);

                // Redirect to the appropriate dashboard
                userDataAccess.RedirectUser(userType, HttpContext.Current);

                // Clear the session
                Session.Remove("MfaCode");
                Session.Remove("Username");
                Session.Remove("UserType");
            }
            else
            {
                Response.Write("<script>alert('Invalid MFA code.');</script>");
            }
        }
        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/View/Login.aspx");
        }

    }
}

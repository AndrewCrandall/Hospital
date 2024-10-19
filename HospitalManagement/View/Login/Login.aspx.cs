using HospitalManagement.DataAccess;
using System;
using System.EnterpriseServices;
using System.Web;
using System.Web.Security;

namespace HospitalManagement
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void loginBtn_Click(object sender, EventArgs e)
        {
            LoginManager userDataAccess = new LoginManager();
            var (isValidUser, userType) = userDataAccess.ValidateUser(usernameTxt.Text, passwordTxt.Text);

            if (isValidUser)
            {
                var mfaCode = userDataAccess.GenerateMfaCode();
                userDataAccess.SendMfaCodeToUser(usernameTxt.Text, mfaCode);

                // Store the MFA code and user information in session
                Session["MfaCode"] = mfaCode;
                Session["Username"] = usernameTxt.Text;
                Session["UserType"] = userType;

                // Redirect to MFA verification page
                Response.Redirect("~/View/Login/MfaVerification.aspx");
            }
            else
            {
                Response.Write("<script>alert('Login Unsuccessful');</script>");
            }
        }


    }

}
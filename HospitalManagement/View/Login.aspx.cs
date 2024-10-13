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
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,
                    usernameTxt.Text,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(130),
                    false,
                    userType, // Store the userType in the ticket
                    FormsAuthentication.FormsCookiePath);

                string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
                {
                    Path = "/",
                    Secure = false, // Set to true for HTTPS
                    HttpOnly = true
                };

                Response.Cookies.Add(authCookie);

                // Redirect based on userType
                switch (userType)
                {
                    case "Admin":
                        Response.Redirect("~/View/adminDashboard.aspx");
                        break;
                    case "Doctor":
                        Response.Redirect("~/View/doctorDashboard.aspx"); // Redirect to the doctor's dashboard
                        break;
                    case "Patient":
                        Response.Redirect("~/View/patientDashboard.aspx"); // Redirect to the patient's dashboard
                        break;
                    default:
                        Response.Write("<script>alert('Unknown user type.');</script>");
                        break;
                }
            }
            else
            {
                Response.Write("<script>alert('Login Unsuccessful');</script>");
            }
        }


    }

}
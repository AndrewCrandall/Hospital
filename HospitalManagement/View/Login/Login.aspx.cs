using HospitalManagement.DataAccess;
using HospitalManagement.Utilities; // Include the utility namespace
using System;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace HospitalManagement
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected async void loginBtn_Click(object sender, EventArgs e)
        {
            bool redirectToMfa = await HandleUserLoginAsync();

            if (redirectToMfa)
            {
                try
                {
                    Response.Redirect("~/View/Login/mfaVerification.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                catch (Exception ex)
                {
                    Response.Write($"<script>alert('An error occurred when navigating to MFA: {ex.Message}');</script>");
                }
            }
        }

        private async Task<bool> HandleUserLoginAsync()
        {
            LoginManager userDataAccess = new LoginManager();

            string username = InputValidator.SanitizeInput(usernameTxt.Text);
            string password = InputValidator.SanitizeInput(passwordTxt.Text);

            if (!InputValidator.IsValidInput(username, password))
            {
                Response.Write("<script>alert('Invalid username or password format.');</script>");
                return false;
            }

            try
            {
                var (isValidUser, userType) = userDataAccess.ValidateUser(username, password);

                if (isValidUser)
                {
                    var mfaCode = userDataAccess.GenerateMfaCode();
                    string email = userDataAccess.GetEmailForMfa(username);

                    if (string.IsNullOrEmpty(email))
                    {
                        Response.Write("<script>alert('Email address not found.');</script>");
                        return false;
                    }

                    try
                    {
                        await userDataAccess.SendMfaCodeViaEmail(email, mfaCode);
                        Session["MfaCode"] = mfaCode;
                        Session["Username"] = username;
                        Session["UserType"] = userType;
                        return true;
                    }
                    catch (SmtpException smtpEx)
                    {
                        Response.Write($"<script>alert('Email sending failed: {smtpEx.Message}');</script>");
                    }
                    catch (Exception ex)
                    {
                        Response.Write($"<script>alert('An error occurred while sending the MFA code: {ex.Message}');</script>");
                    }
                }
                else
                {
                    Response.Write("<script>alert('Login Unsuccessful');</script>");
                }
            }
            catch (ArgumentNullException argEx)
            {
                Response.Write($"<script>alert('Invalid input: {argEx.Message}');</script>");
            }
            catch (InvalidOperationException invOpEx)
            {
                Response.Write($"<script>alert('An error occurred during the operation: {invOpEx.Message}');</script>");
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('An unexpected error occurred: {ex.Message}');</script>");
            }

            return false;
        }
        protected void RegisterBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/View/Login/Register.aspx"); // Redirects to the registration page
            Context.ApplicationInstance.CompleteRequest();

        }
    }
}

using HospitalManagement.DataAccess;
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
            // Call the new function and check if it returns true
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
                    // Handle any other exceptions that occur during redirection
                    Response.Write($"<script>alert('An error occurred when navigating to MFA: {ex.Message}');</script>");
                }
            }
        }

        private async Task<bool> HandleUserLoginAsync()
        {
            LoginManager userDataAccess = new LoginManager();

            try
            {
                // Validate the user

                var (isValidUser, userType) = userDataAccess.ValidateUser(usernameTxt.Text, passwordTxt.Text);

                if (isValidUser)
                {
                    // Generate the MFA code
                    var mfaCode = userDataAccess.GenerateMfaCode();

                    // Get the user's email for MFA
                    string email = userDataAccess.GetEmailForMfa(usernameTxt.Text);

                    if (string.IsNullOrEmpty(email))
                    {
                        Response.Write("<script>alert('Email address not found.');</script>");
                        return false;
                    }

                    try
                    {
                        // Send the MFA code via email asynchronously
                        await userDataAccess.SendMfaCodeViaEmail(email, mfaCode);

                        // Store MFA code and user information in session
                        Session["MfaCode"] = mfaCode;
                        Session["Username"] = usernameTxt.Text;
                        Session["UserType"] = userType;

                        // Return true to indicate the user should be redirected
                        return true;
                    }
                    catch (SmtpException smtpEx)
                    {
                        // Handle SMTP-specific exceptions
                        Response.Write($"<script>alert('Email sending failed: {smtpEx.Message}');</script>");
                    }
                    catch (Exception ex)
                    {
                        // Handle any other exceptions during email sending
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
                // Handle argument null exceptions (e.g., username or password is null)
                Response.Write($"<script>alert('Invalid input: {argEx.Message}');</script>");
            }
            catch (InvalidOperationException invOpEx)
            {
                // Handle invalid operation exceptions
                Response.Write($"<script>alert('An error occurred during the operation: {invOpEx.Message}');</script>");
            }
            catch (Exception ex)
            {
                // Handle any other general exceptions
                Response.Write($"<script>alert('An unexpected error occurred: {ex.Message}');</script>");
            }
                

            // Return false to indicate the user should not be redirected
            return false;
        }
    }
                

}
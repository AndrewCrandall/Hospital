using System;
using System.Web.UI;

namespace HospitalManagement
{
    public partial class Register : System.Web.UI.Page
    {
        protected void RegisterUser(object sender, EventArgs e)
        {
            string username = usernameTxt.Text;
            string password = passwordTxt.Text;
            string email = emailTxt.Text;
            string firstName = firstNameTxt.Text;  // First name input
            string lastName = lastNameTxt.Text;    // Last name input

            try
            {
                var loginManager = new LoginManager();
                loginManager.RegisterNewUser(username, password, email, firstName, lastName);
                // Display alert
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Registration successful!'); setTimeout(function() { window.location = '" + ResolveUrl("~/View/Login/Login.aspx") + "'; }, 2000);", true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Error: {ex.Message}');", true);
            }
        }

        protected void ReturnUser(object sender, EventArgs e)
        {
            Response.Redirect("~/View/Login/Login.aspx");
        }
    }
}

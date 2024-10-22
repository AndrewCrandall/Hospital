using System;
using System.Web.UI;
using HospitalManagement.Utilities; // Add namespace for InputValidator

namespace HospitalManagement.View.Admin
{
    public partial class userManagement : System.Web.UI.Page
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

        protected void Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/View/Admin/adminDashboard.aspx");
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            // Clear input fields
            firstNameInput.Text = string.Empty;
            lastNameInput.Text = string.Empty;
            userIdInput.Text = string.Empty;
            displayFirstName.Text = string.Empty;
            displayLastName.Text = string.Empty;
            displayUsername.Text = string.Empty;
            displayAddress.Text = string.Empty;
            displayUserType.SelectedIndex = -1; // Reset the dropdown to no selection
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            // Get the updated values from the input fields
            string firstName = InputValidator.SanitizeInput(displayFirstName.Text);
            string lastName = InputValidator.SanitizeInput(displayLastName.Text);
            string username = InputValidator.SanitizeInput(displayUsername.Text);
            string userType = displayUserType.SelectedValue; // Get selected value from dropdown
            string userID = userIdInput.Text;

            // Validate user ID
            if (!int.TryParse(userID, out _))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "InvalidUserId", "alert('User ID must be a number.');", true);
                return;
            }

            // Create an instance of AdminManager
            AdminManager adminManager = new AdminManager();

            // Call the UpdateUser method
            bool updateSuccess = adminManager.UpdateUser(userID, firstName, lastName, username, userType);

            // Check if the update was successful
            if (updateSuccess)
            {
                // Display success message and refresh the page
                string script = "alert('User details updated successfully.'); window.location.href = window.location.href;";
                ClientScript.RegisterStartupScript(this.GetType(), "UpdateSuccess", script, true);
            }
            else
            {
                // Display error message and refresh the page
                string script = "alert('Failed to update user details.'); window.location.href = window.location.href;";
                ClientScript.RegisterStartupScript(this.GetType(), "UpdateFailed", script, true);
            }
        }

        protected void Search_Click(object sender, EventArgs e)
        {
            // Get input values
            string firstName = InputValidator.SanitizeInput(firstNameInput.Text);
            string lastName = InputValidator.SanitizeInput(lastNameInput.Text);
            string userId = InputValidator.SanitizeInput(userIdInput.Text);

            // Validate user ID
            if (!int.TryParse(userId, out _))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "InvalidUserId", "alert('User ID must be a number.');", true);
                return;
            }

            // Create an instance of AdminManager
            AdminManager adminManager = new AdminManager();

            // Call the SearchUser method
            User foundUser = adminManager.SearchUser(firstName, lastName, userId);

            // Check if a user was found
            if (foundUser != null)
            {
                // Populate the readonly fields with the found user's details
                displayFirstName.Text = foundUser.FirstName;
                displayLastName.Text = foundUser.LastName;
                displayUsername.Text = foundUser.Username;
                displayAddress.Text = foundUser.Email;
                displayUserType.SelectedValue = foundUser.UserType; // Set the selected value based on found user
            }
            else
            {
                // Display a JavaScript alert if no user is found
                string script = "alert('No user found with the provided details.');";
                ClientScript.RegisterStartupScript(this.GetType(), "UserNotFound", script, true);
            }
        }
    }
}

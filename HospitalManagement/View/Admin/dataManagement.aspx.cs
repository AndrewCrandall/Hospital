using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Twilio.Http;

namespace HospitalManagement.View.Admin
{
    public partial class dataManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

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
            displayDate.Text = string.Empty;
            displayNotes.Text = string.Empty;

        }
        protected void Search_Click(object sender, EventArgs e)
        {
            // Get input values
            string firstName = firstNameInput.Text;
            string lastName = lastNameInput.Text;
            string userId = userIdInput.Text;

            // Create an instance of AdminManager
            AdminManager adminManager = new AdminManager();

            // Call the SearchData method
            UserData foundUser = adminManager.SearchData(firstName, lastName, userId);

            // Clear input fields
            firstNameInput.Text = string.Empty;
            lastNameInput.Text = string.Empty;
            userIdInput.Text = string.Empty;

            // Check if a user was found
            if (foundUser != null)
            {
                // Populate display fields with found user data
                displayFirstName.Text = foundUser.FirstName;
                displayLastName.Text = foundUser.LastName;
                displayDate.Text = foundUser.VisitDate; // Assuming this is the right field
                displayNotes.Text = foundUser.Notes;
            }
            else
            {
                // Optionally, handle the case where no user was found
                string script = "alert('No user found.');";
                ClientScript.RegisterStartupScript(this.GetType(), "UserNotFound", script, true);
            }
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            return;
        }
    }
}
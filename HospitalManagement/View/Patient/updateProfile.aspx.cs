using System;
using System.Data;
using System.Web.UI;
using HospitalManagement.Model;

namespace HospitalManagement.View.Patient
{
    public partial class updateProfile : System.Web.UI.Page
    {
        patientManager patientManager = new patientManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUserData(); // Load existing user data
            }
        }

        private void LoadUserData()
        {
            string username = Session["Username"].ToString();
            DataTable userData = patientManager.GetPatientRecordsByUsername(username);

            if (userData.Rows.Count > 0)
            {
                txtUsername.Text = userData.Rows[0]["username"].ToString();
                txtFirstName.Text = userData.Rows[0]["firstName"].ToString();
                txtLastName.Text = userData.Rows[0]["lastName"].ToString();
                txtEmail.Text = userData.Rows[0]["email"].ToString();
            }
            else
            {
                // Handle case where no data is returned
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            // Get the current username from the session
            string currentUsername = Session["Username"].ToString();
            int userID = patientManager.GetUserIdByUsername(currentUsername);

            // Update the user profile with new details
            bool success = patientManager.UpdateUserProfile(
                userID,
                txtUsername.Text,
                txtPassword.Text,
                txtFirstName.Text,
                txtLastName.Text,
                txtEmail.Text
            );

            if (success)
            {
                // Update the session with the new username
                Session["Username"] = txtUsername.Text;

                lblMessage.Text = "Profile updated successfully!";
            }
            else
            {
                lblMessage.Text = "Update failed. Please try again.";
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // Clear all fields
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtEmail.Text = "";
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            // Redirect to the previous page
            Response.Redirect("/View/Patient/patientDashboard.aspx"); // Change to your desired URL
        }
    }
}

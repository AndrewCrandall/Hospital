using HospitalManagement.Model; // Ensure the correct namespace for doctorManager
using HospitalManagement.Utilities; // Add namespace for InputValidator
using System;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace HospitalManagement.View.Doctor
{
    public partial class addNotes : System.Web.UI.Page
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

        protected void SaveNotes_Click(object sender, EventArgs e)
        {
            try
            {
                // Sanitize input for other fields
                string firstName = InputValidator.SanitizeInput(patientFirstName.Text);
                string lastName = InputValidator.SanitizeInput(patientLastName.Text);
                string appointmentDateText = InputValidator.SanitizeInput(appointmentDate.Text);

                // Get raw notesText and trim it
                string notesText = notes.Text.Trim();

                // Check if notesText is empty
                if (string.IsNullOrWhiteSpace(notesText))
                {
                    throw new Exception("Notes cannot be empty.");
                }

                // Get the username from the session
                string doctorUsername = Session["Username"]?.ToString();
                if (string.IsNullOrEmpty(doctorUsername))
                {
                    throw new Exception("Doctor username is not found in the session.");
                }

                // Create an instance of EncryptionManager
                var encryptionManager = new EncryptionManager();

                // Retrieve the latest key and IV for encryption
                var (key, iv) = encryptionManager.RetrieveLatestKey();
                if (key == null || iv == null)
                {
                    throw new Exception("No encryption key or IV found for encryption.");
                }

                // Encrypt the notes
                string encryptedNotes = encryptionManager.Encrypt(notesText, key, iv);
                Console.WriteLine($"Encrypted Notes: {encryptedNotes}"); // Log encrypted notes for debugging

                // Call the AddAppointment method with the encrypted notes
                doctorManager doctor = new doctorManager();
                bool isAdded = doctor.AddAppointment(doctorUsername, firstName, lastName, appointmentDateText, encryptedNotes);

                if (isAdded)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Success", "alert('Appointment added successfully.');", true);
                    ClearFields(); // Optional: Clear the fields after successful addition
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Failure", "alert('Failed to add appointment.');", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Error", $"alert('An error occurred: {ex.Message}');", true);
            }
        }





        // Method to check if a string is a valid Base64 string
        private bool IsBase64String(string base64)
        {
            // Check if the string length is valid
            if (base64.Length % 4 != 0)
                return false;

            // Check if the string is properly formatted
            return Regex.IsMatch(base64, @"^[a-zA-Z0-9+/]*={0,2}$");
        }

        // Optional: Method to clear text fields
        private void ClearFields()
        {
            patientFirstName.Text = string.Empty;
            patientLastName.Text = string.Empty;
            notes.Text = string.Empty;
            appointmentDate.Text = string.Empty;
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            // Clear all text fields
            ClearFields();
        }

        protected void Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/View/Doctor/doctorDashboard.aspx");

            // Clear the session
            Session.Clear();
        }
    }
}

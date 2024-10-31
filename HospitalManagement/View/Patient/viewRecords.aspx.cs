using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using HospitalManagement.Model; // Adjust the namespace as necessary

namespace HospitalManagement.View.Patient
{
    public partial class viewRecords : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if the session variables are null
            if (Session["Username"] == null || Session["UserType"] == null)
            {
                // Redirect to the login page or an error page
                Response.Redirect("~/View/Login.aspx");
            }
            if (!IsPostBack)
            {
                LoadPatientRecords();
            }
        }

        private void LoadPatientRecords()
        {
            try
            {
                // Get the patient's username from the session
                string patientUsername = Session["Username"]?.ToString();

                if (!string.IsNullOrEmpty(patientUsername))
                {
                    patientManager patManager = new patientManager();
                    DataTable patientRecords = patManager.GetPatientRecordsByUsername(patientUsername);

                    // Check if any records were retrieved
                    if (patientRecords.Rows.Count > 0)
                    {
                        // Create an instance of EncryptionManager
                        EncryptionManager encryptionManager = new EncryptionManager();

                        // Decrypt the notes in the DataTable
                        foreach (DataRow row in patientRecords.Rows)
                        {
                            string encryptedNote = row["notes"].ToString(); // Assuming "notes" is the column name
                            string decryptedNote = encryptionManager.Decrypt(encryptedNote); // Decrypt the note
                            row["notes"] = decryptedNote; // Replace the encrypted note with the decrypted note
                        }

                        // Bind the data to the GridView
                        RecordsGridView.DataSource = patientRecords;
                        RecordsGridView.DataBind();
                    }
                    else
                    {
                        // Handle case where no records are found
                        ClientScript.RegisterStartupScript(this.GetType(), "NoRecords", "alert('No patient records found.');", true);
                    }
                }
                else
                {
                    // Handle case where username is not available
                    ClientScript.RegisterStartupScript(this.GetType(), "Error", "alert('Patient username not found in session.');", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Error", $"alert('An error occurred: {ex.Message}');", true);
            }
        }


        protected void BackButton_Click(object sender, EventArgs e)
        {
            // Redirect user back to the dashboard
            Response.Redirect("~/View/Patient/patientDashboard.aspx");

            // Clear the session if needed
            // Session.Clear();
        }
    }
}

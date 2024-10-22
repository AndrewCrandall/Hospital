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
            if (!IsPostBack)
            {
                LoadPatientRecords();
            }
        }

        private void LoadPatientRecords()
        {
            try
            {
                // Get the patient's username from the session (assuming it's stored there)
                string patientUsername = Session["Username"]?.ToString();

                if (!string.IsNullOrEmpty(patientUsername))
                {
                    patientManager patManager = new patientManager();
                    DataTable patientRecords = patManager.GetPatientRecordsByUsername(patientUsername);

                    // Bind the data to the GridView
                    RecordsGridView.DataSource = patientRecords;
                    RecordsGridView.DataBind();
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

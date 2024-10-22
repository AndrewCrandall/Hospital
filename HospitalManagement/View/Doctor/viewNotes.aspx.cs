using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using HospitalManagement.Model; // Adjust the namespace as necessary

namespace HospitalManagement.View.Doctor
{
    public partial class viewNotes : System.Web.UI.Page
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
                LoadAppointmentNotes();
            }
        }

        private void LoadAppointmentNotes()
        {
            try
            {
                // Get the doctor's username from the session (assuming it's stored there)
                string doctorUsername = Session["Username"]?.ToString();

                if (!string.IsNullOrEmpty(doctorUsername))
                {
                    doctorManager docManager = new doctorManager();
                    DataTable appointmentNotes = docManager.GetAppointmentNotesByDoctorUsername(doctorUsername);

                    // Bind the data to the GridView
                    NotesGridView.DataSource = appointmentNotes;
                    NotesGridView.DataBind();
                }
                else
                {
                    // Handle case where username is not available
                    ClientScript.RegisterStartupScript(this.GetType(), "Error", "alert('Doctor username not found in session.');", true);
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
            Response.Redirect("~/View/Doctor/doctorDashboard.aspx");

            // Clear the session
            Session.Clear();
        }
    }
}

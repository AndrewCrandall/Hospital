using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using HospitalManagement.Utilities; // Add namespace for InputValidator

namespace HospitalManagement.View.Admin
{
    public partial class dataManagement : System.Web.UI.Page
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
                BindGrid();
            }
        }

        protected void Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/View/Admin/adminDashboard.aspx");
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            // Clear the grid by setting the DataSource to null and re-binding
            AppointmentGridView.DataSource = null;
            AppointmentGridView.DataBind();

            // Optionally, clear the user ID input
            userIdInput.Text = string.Empty;
        }

        protected void Search_Click(object sender, EventArgs e)
        {
            // Sanitize user ID input
            string userID = InputValidator.SanitizeInput(userIdInput.Text);

            if (int.TryParse(userID, out int userId))
            {
                AdminManager adminManager = new AdminManager();
                List<UserData> appointments = adminManager.SearchAppointments(userId);

                // Bind the appointments to the GridView
                AppointmentGridView.DataSource = appointments;
                AppointmentGridView.DataBind();
            }
            else
            {
                // Handle invalid userId
                ClientScript.RegisterStartupScript(this.GetType(), "InvalidUserId", "alert('Invalid User ID.');", true);
            }
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            AdminManager adminManager = new AdminManager();
            bool anyUpdateFailed = false; // Flag to track if any updates fail
            List<string> failedUpdates = new List<string>(); // List to store failure messages

            foreach (GridViewRow row in AppointmentGridView.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    // Retrieve the IDs or keys you need for the update
                    int appointmentId = Convert.ToInt32(AppointmentGridView.DataKeys[row.RowIndex].Value);

                    // Sanitize inputs from the row
                    string visitDate = InputValidator.SanitizeInput(((TextBox)row.Cells[4].Controls[0]).Text);
                    string notes = InputValidator.SanitizeInput(((TextBox)row.Cells[5].Controls[0]).Text);

                    // Call the update function to update the database
                    bool success = adminManager.UpdateAppointment(appointmentId, visitDate, notes);
                    if (!success)
                    {
                        anyUpdateFailed = true; // Mark that an update failed
                        failedUpdates.Add($"Appointment ID {appointmentId} could not be updated.");
                    }
                }
            }

            // Check if any updates failed and handle accordingly
            if (anyUpdateFailed)
            {
                string errorMessage = "Some appointments could not be updated:\n" + string.Join("\n", failedUpdates);
                ClientScript.RegisterStartupScript(this.GetType(), "UpdateFailed", $"alert('{errorMessage}');", true);
            }
            else
            {
                // Optionally, show a success message
                ClientScript.RegisterStartupScript(this.GetType(), "UpdateSuccess", "alert('All appointments updated successfully.');", true);
            }

            // Optionally, rebind the grid to refresh data
            BindGrid();
        }

        protected void AppointmentGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Set the row to edit
            AppointmentGridView.EditIndex = e.NewEditIndex;
            BindGrid(); // Rebind data to the grid
        }

        protected void AppointmentGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Get the row being edited
            GridViewRow row = AppointmentGridView.Rows[e.RowIndex];

            // Get values from the edited row and sanitize
            string patientFirstName = InputValidator.SanitizeInput(((TextBox)row.Cells[0].Controls[0]).Text);
            string patientLastName = InputValidator.SanitizeInput(((TextBox)row.Cells[1].Controls[0]).Text);
            string doctorFirstName = InputValidator.SanitizeInput(((TextBox)row.Cells[2].Controls[0]).Text);
            string doctorLastName = InputValidator.SanitizeInput(((TextBox)row.Cells[3].Controls[0]).Text);
            string visitDate = InputValidator.SanitizeInput(((TextBox)row.Cells[4].Controls[0]).Text);
            string notes = InputValidator.SanitizeInput(((TextBox)row.Cells[5].Controls[0]).Text);

            // Here you would typically update the database with the new values
            // UpdateDatabase(patientFirstName, patientLastName, doctorFirstName, doctorLastName, visitDate, notes);

            // Reset the edit index and rebind the data
            AppointmentGridView.EditIndex = -1;
            BindGrid();
        }

        protected void AppointmentGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Cancel the edit operation
            AppointmentGridView.EditIndex = -1;
            BindGrid(); // Rebind data to the grid
        }

        private void BindGrid()
        {
            // This method should bind the data to the GridView
            // Example: AppointmentGridView.DataSource = GetData();
            AppointmentGridView.DataBind();
        }
    }
}

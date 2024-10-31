using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using HospitalManagement.Utilities; // Add namespace for InputValidator
using HospitalManagement.Model; // Add this to access UserData
using HospitalManagement.View.Admin; // Ensure this namespace is included for AdminManager

namespace HospitalManagement.View.Admin
{
    public partial class dataManagement : System.Web.UI.Page
    {
        private AdminManager adminManager;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null || Session["UserType"] == null)
            {
                Response.Redirect("~/View/Login.aspx");
            }

            if (!IsPostBack)
            {
                adminManager = new AdminManager();
                BindGrid();
            }
        }

        protected void Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/View/Admin/adminDashboard.aspx");
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            AppointmentGridView.DataSource = null;
            AppointmentGridView.DataBind();
            userIdInput.Text = string.Empty;
        }

        protected void Search_Click(object sender, EventArgs e)
        {
            // Sanitize user ID input
            string userID = InputValidator.SanitizeInput(userIdInput.Text);

            if (int.TryParse(userID, out int userId))
            {
                AdminManager adminManager = new AdminManager(); // Ensure this line is present
                List<UserData> appointments = adminManager.SearchAppointments(userId);

                // Create an instance of EncryptionManager to decrypt the notes
                EncryptionManager encryptionManager = new EncryptionManager();

                // Decrypt notes for each appointment
                foreach (var appointment in appointments)
                {
                    appointment.Notes = encryptionManager.Decrypt(appointment.Notes); // Decrypt the notes
                }

                if (appointments != null && appointments.Count > 0) // Check if appointments is not null and has items
                {
                    AppointmentGridView.DataSource = appointments;
                    AppointmentGridView.DataBind();
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "NoAppointments", "alert('No appointments found for this user.');", true);
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "InvalidUserId", "alert('Invalid User ID.');", true);
            }
        }



        protected void Save_Click(object sender, EventArgs e)
        {
            bool anyUpdateFailed = false;
            List<string> failedUpdates = new List<string>();

            foreach (GridViewRow row in AppointmentGridView.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    int appointmentId = Convert.ToInt32(AppointmentGridView.DataKeys[row.RowIndex].Value);
                    string visitDate = InputValidator.SanitizeInput(((TextBox)row.Cells[4].Controls[0]).Text);
                    string notes = InputValidator.SanitizeInput(((TextBox)row.Cells[5].Controls[0]).Text);

                    // Encrypt the notes before saving
                    string encryptedNotes = adminManager.EncryptNotes(notes);

                    bool success = adminManager.UpdateAppointment(appointmentId, visitDate, encryptedNotes);
                    if (!success)
                    {
                        anyUpdateFailed = true;
                        failedUpdates.Add($"Appointment ID {appointmentId} could not be updated.");
                    }
                }
            }

            if (anyUpdateFailed)
            {
                string errorMessage = "Some appointments could not be updated:\n" + string.Join("\n", failedUpdates);
                ClientScript.RegisterStartupScript(this.GetType(), "UpdateFailed", $"alert('{errorMessage}');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "UpdateSuccess", "alert('All appointments updated successfully.');", true);
            }

            BindGrid();
        }

        protected void AppointmentGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            AppointmentGridView.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void AppointmentGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = AppointmentGridView.Rows[e.RowIndex];

            string patientFirstName = InputValidator.SanitizeInput(((TextBox)row.Cells[0].Controls[0]).Text);
            string patientLastName = InputValidator.SanitizeInput(((TextBox)row.Cells[1].Controls[0]).Text);
            string doctorFirstName = InputValidator.SanitizeInput(((TextBox)row.Cells[2].Controls[0]).Text);
            string doctorLastName = InputValidator.SanitizeInput(((TextBox)row.Cells[3].Controls[0]).Text);
            string visitDate = InputValidator.SanitizeInput(((TextBox)row.Cells[4].Controls[0]).Text);
            string notes = InputValidator.SanitizeInput(((TextBox)row.Cells[5].Controls[0]).Text);

            AppointmentGridView.EditIndex = -1;
            BindGrid();
        }

        protected void AppointmentGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            AppointmentGridView.EditIndex = -1;
            BindGrid();
        }

        private void BindGrid()
        {
            // Fetch and display appointments
            string userID = InputValidator.SanitizeInput(userIdInput.Text);
            if (int.TryParse(userID, out int userId))
            {
                List<UserData> appointments = adminManager.SearchAppointments(userId);

                // Decrypt notes for display
                foreach (var appointment in appointments)
                {
                    appointment.Notes = adminManager.DecryptNotes(appointment.Notes); // Ensure notes are decrypted
                }

                AppointmentGridView.DataSource = appointments;
                AppointmentGridView.DataBind();
            }
        }
    }
}

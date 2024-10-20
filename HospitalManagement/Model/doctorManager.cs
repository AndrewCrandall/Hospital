using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HospitalManagement.Model
{
    public class doctorManager : SqlConnectionManager
    {
        public doctorManager() : base() // Calls the base constructor
        {
        }


        public bool AddAppointment(string doctorUsername, string patientFirstName, string patientLastName, string appointmentDate, string notes)
        {
            try
            {
                // Get the patient's username based on first and last name
                string patientUsername = GetPatientUsername(patientFirstName, patientLastName);
                if (string.IsNullOrEmpty(patientUsername))
                {
                    throw new Exception("Patient not found.");
                }

                // Open the database connection
                OpenConnection();

                // Insert a new appointment
                string query = @"
            INSERT INTO HealthManagement.dbo.Appointments (doctorID, patientID, appointmentDate, notes, status)
            VALUES (
                (SELECT d.doctorID 
                 FROM HealthManagement.dbo.Users AS u
                 JOIN HealthManagement.dbo.Doctors AS d ON u.userID = d.userID
                 WHERE u.username = @DoctorUsername),
                (SELECT p.patientID 
                 FROM HealthManagement.dbo.Users AS u
                 JOIN HealthManagement.dbo.Patients AS p ON u.userID = p.userID
                 WHERE u.username = @PatientUsername),
                @AppointmentDate,
                @Notes,
                'completed'
            );";

                using (SqlCommand command = new SqlCommand(query, GetConnection()))
                {
                    command.Parameters.AddWithValue("@DoctorUsername", doctorUsername);
                    command.Parameters.AddWithValue("@PatientUsername", patientUsername);
                    command.Parameters.AddWithValue("@AppointmentDate", DateTime.Parse(appointmentDate)); // Ensure the date is in the correct format
                    command.Parameters.AddWithValue("@Notes", notes);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0; // Return true if the insert was successful
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while adding appointment: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }


        private string GetPatientUsername(string firstName, string lastName)
        {
            string username = null;
            try
            {
                OpenConnection();
                string query = "SELECT u.username FROM Users AS u " +
                               "JOIN Patients AS p ON u.userID = p.userID " +
                               "WHERE u.firstName = @FirstName AND u.lastName = @LastName";

                using (SqlCommand command = new SqlCommand(query, GetConnection()))
                {
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    username = command.ExecuteScalar()?.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving patient username: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
            return username;
        }

    }
}
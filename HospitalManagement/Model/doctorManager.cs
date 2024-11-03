// Author : Andrew Crandall
// Date Modified : 11/3/2024
// Title : doctorManager
// Purpose : Provide the logic for backend management of all doctor actions

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace HospitalManagement.Model
{
    public class doctorManager : SqlConnectionManager
    {
        private readonly EncryptionManager _encryptionManager;

        public doctorManager() : base()
        {
            _encryptionManager = new EncryptionManager(); // Instantiate EncryptionManager
        }

        public bool AddAppointment(string doctorUsername, string patientFirstName, string patientLastName, string appointmentDate, string encryptedNotes)
        {
            try
            {
                string patientUsername = GetPatientUsername(patientFirstName, patientLastName);
                if (string.IsNullOrEmpty(patientUsername))
                {
                    throw new Exception("Patient not found.");
                }

                OpenConnection();

                // Step 1: Insert the appointment with encrypted notes
                string insertQuery = @"
        INSERT INTO HealthManagement.dbo.Appointments (doctorID, patientID, appointmentDate, notes, status)
        OUTPUT INSERTED.appointmentID  -- Get the new appointment ID
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
            @Notes,  -- Use encrypted notes directly
            'Confirmed'
        );";

                int appointmentId;

                using (var command = new SqlCommand(insertQuery, GetConnection()))
                {
                    command.Parameters.AddWithValue("@DoctorUsername", doctorUsername);
                    command.Parameters.AddWithValue("@PatientUsername", patientUsername);
                    command.Parameters.AddWithValue("@AppointmentDate", DateTime.Parse(appointmentDate));
                    command.Parameters.AddWithValue("@Notes", encryptedNotes); // Insert encrypted notes

                    // Retrieve the new appointment ID
                    appointmentId = (int)command.ExecuteScalar();
                }

                // No need for further steps, as notes are already encrypted during insertion
                return true; // Return true since the appointment was added successfully
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

        public DataTable GetAppointmentNotesByDoctorUsername(string doctorUsername)
        {
            DataTable notesTable = new DataTable();
            try
            {
                OpenConnection();

                // Query to get the doctorID
                string doctorIdQuery = @"
        SELECT d.doctorID 
        FROM HealthManagement.dbo.Users AS u
        JOIN HealthManagement.dbo.Doctors AS d ON u.userID = d.userID
        WHERE u.username = @DoctorUsername";

                int doctorID;
                using (SqlCommand idCommand = new SqlCommand(doctorIdQuery, GetConnection()))
                {
                    idCommand.Parameters.AddWithValue("@DoctorUsername", doctorUsername);
                    var result = idCommand.ExecuteScalar();
                    if (result != null)
                    {
                        doctorID = Convert.ToInt32(result);
                    }
                    else
                    {
                        throw new Exception("Doctor not found with the specified username.");
                    }
                }

                // Updated query to include appointmentID
                string appointmentQuery = @"
        SELECT 
            a.appointmentID,  
            a.appointmentDate, 
            a.notes, 
            u.firstName AS PatientFirstName, 
            u.lastName AS PatientLastName, 
            a.status 
        FROM HealthManagement.dbo.Appointments AS a
        JOIN HealthManagement.dbo.Patients AS p ON a.patientID = p.patientID
        JOIN HealthManagement.dbo.Users AS u ON p.userID = u.userID
        WHERE a.doctorID = @DoctorID";

                using (SqlCommand appointmentCommand = new SqlCommand(appointmentQuery, GetConnection()))
                {
                    appointmentCommand.Parameters.AddWithValue("@DoctorID", doctorID);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(appointmentCommand))
                    {
                        adapter.Fill(notesTable);
                    }
                }
            }

            catch (Exception ex)
            {
                throw new Exception("Error retrieving appointment notes: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
            return notesTable;
        }


        public void EncryptExistingNotes()
        {
            if (!OpenConnection())
                throw new Exception("Could not open database connection.");

            try
            {
                // Fetch existing appointments
                var appointments = FetchExistingAppointments();

                // Retrieve the latest key and IV from the database
                var (key, iv) = _encryptionManager.RetrieveLatestKey();
                if (key == null || iv == null)
                {
                    throw new Exception("No encryption key or IV found in the database.");
                }

                // Loop through each appointment and encrypt the notes
                foreach (var appointment in appointments)
                {
                    // Encrypt the notes using the retrieved key and IV
                    var encryptedNotes = _encryptionManager.Encrypt(appointment.Notes, key, iv);

                    // Update the appointment notes in the database
                    UpdateAppointmentNotes(appointment.AppointmentID, encryptedNotes);
                }
            }
            finally
            {
                CloseConnection();
            }
        }

        private List<Appointment> FetchExistingAppointments()
        {
            var appointments = new List<Appointment>();

            using (var command = new SqlCommand("SELECT [appointmentID], [notes] FROM [HealthManagement].[dbo].[Appointments]", GetConnection()))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        appointments.Add(new Appointment
                        {
                            AppointmentID = reader.GetInt32(0),
                            Notes = reader.GetString(1)
                        });
                    }
                }
            }

            return appointments;
        }

        public bool UpdateAppointmentNotes(int appointmentId, string encryptedNotes)
        {
            try
            {
                OpenConnection();
                using (var command = new SqlCommand("UPDATE Appointments SET notes = @Notes WHERE appointmentID = @AppointmentID", GetConnection()))
                {
                    command.Parameters.AddWithValue("@Notes", encryptedNotes);
                    command.Parameters.AddWithValue("@AppointmentID", appointmentId);
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating appointment notes: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public void SaveEncryptionKey(byte[] key)
        {
            // Generate a new IV
            byte[] iv = new byte[16]; // 128 bits for AES
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(iv);
            }

            try
            {
                // Open the database connection
                if (!OpenConnection())
                    throw new Exception("Could not open database connection.");

                using (var command = new SqlCommand("INSERT INTO EncryptionKeys (EncryptionKey, IV, CreatedAt) VALUES (@EncryptionKey, @IV, @CreatedAt)", GetConnection()))
                {
                    command.Parameters.AddWithValue("@EncryptionKey", key);
                    command.Parameters.AddWithValue("@IV", iv);
                    command.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow); // Store creation time
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving encryption key: " + ex.Message);
            }
            finally
            {
                CloseConnection(); // Ensure the connection is closed after the operation
            }
        }

        public class Appointment
        {
            public int AppointmentID { get; set; }
            public string Notes { get; set; }
        }
    }
}

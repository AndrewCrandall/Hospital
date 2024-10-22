using System;
using System.Data;
using System.Data.SqlClient;

namespace HospitalManagement.Model
{
    public class patientManager : SqlConnectionManager
    {
        public patientManager() : base() // Calls the base constructor
        {
        }

        public DataTable GetPatientRecordsByUsername(string username)
        {
            DataTable recordsTable = new DataTable();
            try
            {
                OpenConnection();

                // Step 1: Get userID from Users table
                string userIdQuery = "SELECT userID FROM HealthManagement.dbo.Users WHERE username = @Username";
                int userID = 0;

                using (SqlCommand userIdCommand = new SqlCommand(userIdQuery, GetConnection()))
                {
                    userIdCommand.Parameters.AddWithValue("@Username", username);
                    object result = userIdCommand.ExecuteScalar();
                    if (result != null)
                    {
                        userID = Convert.ToInt32(result);
                    }
                    else
                    {
                        // Handle case where username does not exist
                        return recordsTable; // Return an empty DataTable
                    }
                }

                // Step 2: Get patientID from Patients table using userID
                string patientIdQuery = "SELECT patientID FROM HealthManagement.dbo.Patients WHERE userID = @UserID";
                int patientID = 0;

                using (SqlCommand patientIdCommand = new SqlCommand(patientIdQuery, GetConnection()))
                {
                    patientIdCommand.Parameters.AddWithValue("@UserID", userID);
                    object result = patientIdCommand.ExecuteScalar();
                    if (result != null)
                    {
                        patientID = Convert.ToInt32(result);
                    }
                    else
                    {
                        // Handle case where userID does not exist in Patients table
                        return recordsTable; // Return an empty DataTable
                    }
                }

                // Step 3: Get appointment records using patientID
                string recordsQuery = @"
            SELECT 
                u.username, 
                u.firstName, 
                u.lastName, 
                u.email, 
                a.appointmentID, 
                a.appointmentDate, 
                a.notes,
                du.firstName AS DoctorFirstName, 
                du.lastName AS DoctorLastName
            FROM HealthManagement.dbo.Appointments AS a
            JOIN HealthManagement.dbo.Patients AS p ON a.patientID = p.patientID
            JOIN HealthManagement.dbo.Users AS u ON p.userID = u.userID
            JOIN HealthManagement.dbo.Doctors AS d ON a.doctorID = d.doctorID
            JOIN HealthManagement.dbo.Users AS du ON d.userID = du.userID
            WHERE p.patientID = @PatientID";

                using (SqlCommand recordsCommand = new SqlCommand(recordsQuery, GetConnection()))
                {
                    recordsCommand.Parameters.AddWithValue("@PatientID", patientID);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(recordsCommand))
                    {
                        adapter.Fill(recordsTable);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving patient records: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }

            return recordsTable;
        }




        public bool UpdateUserProfile(int userID, string username, string password, string firstName, string lastName, string email)
        {
            try
            {
                OpenConnection();

                string query = @"
                    UPDATE Users 
                    SET username = @Username, password = @Password, firstName = @FirstName, lastName = @LastName, email = @Email 
                    WHERE userID = @UserID";

                using (SqlCommand command = new SqlCommand(query, GetConnection()))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password); // Remember to hash passwords in production
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@UserID", userID);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0; // Returns true if the update was successful
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating user profile: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }
        public int GetUserIdByUsername(string username)
        {
            int userID = 0;
            try
            {
                OpenConnection();

                string query = @"
            SELECT userID 
            FROM HealthManagement.dbo.Users 
            WHERE username = @Username";

                using (SqlCommand command = new SqlCommand(query, GetConnection()))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        userID = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving user ID: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }

            return userID;
        }

    }
}

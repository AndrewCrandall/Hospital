// Author : Andrew Crandall
// Date Modified : 11/3/2024
// Title : adminManager
// Purpose : Provide the logic for backend management of all admin actions

using HospitalManagement.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public class AdminManager : SqlConnectionManager
{
    private readonly EncryptionManager _encryptionManager;

    public AdminManager() : base() // Calls the base constructor
    {
        _encryptionManager = new EncryptionManager(); // Instantiate the EncryptionManager for handling encryption tasks
    }

    public DataTable GetAllUsers()
    {
        DataTable userTable = new DataTable(); // Initialize a DataTable to hold user data
        try
        {
            OpenConnection(); // Open a connection to the database
            string query = "SELECT firstName, lastName, userID, username, userType FROM Users"; // SQL query to retrieve all users

            using (SqlCommand command = new SqlCommand(query, GetConnection()))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(userTable); // Fill the DataTable with user data
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving user data: {ex.Message}"); // Log error message
        }
        finally
        {
            CloseConnection(); // Ensure the connection is closed in all circumstances
        }

        return userTable; // Return the populated DataTable
    }

    public DataTable GetUsersByType(string userType)
    {
        DataTable userTable = new DataTable(); // Initialize a DataTable for filtered user data
        try
        {
            OpenConnection(); // Open a connection to the database
            string query = "SELECT firstName, lastName, userID, username, userType FROM Users WHERE userType = @userType"; // SQL query to retrieve users by type

            using (SqlCommand command = new SqlCommand(query, GetConnection()))
            {
                command.Parameters.AddWithValue("@userType", userType); // Parameterized query to prevent SQL injection
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(userTable); // Fill the DataTable with filtered user data
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving user data by type: {ex.Message}"); // Log error message
        }
        finally
        {
            CloseConnection(); // Ensure the connection is closed
        }

        return userTable; // Return the populated DataTable
    }

    public User SearchUser(string firstName, string lastName, string userId)
    {
        User user = null; // Initialize the User object
        try
        {
            // Convert userId to int if possible
            int? userIdInt = null; // Nullable int for userId
            if (int.TryParse(userId, out int parsedId))
            {
                userIdInt = parsedId; // Parse userId to int
            }

            OpenConnection(); // Open a connection to the database
            string query = @"
            SELECT firstName, lastName, userID, email, userType, username
            FROM Users 
            WHERE (@firstName IS NULL OR firstName = @firstName)
              AND (@lastName IS NULL OR lastName = @lastName)
              AND (@userID IS NULL OR userID = @userID);"; // SQL query with conditions to search for a user

            using (SqlCommand command = new SqlCommand(query, GetConnection()))
            {
                // Add parameters to the SQL command
                command.Parameters.AddWithValue("@firstName", firstName);
                command.Parameters.AddWithValue("@lastName", lastName);
                command.Parameters.AddWithValue("@userID", userId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read()) // Check if a record is returned
                    {
                        user = new User
                        {
                            FirstName = reader["firstName"].ToString(),
                            LastName = reader["lastName"].ToString(),
                            UserId = reader["userID"].ToString(),
                            Username = reader["username"].ToString(),
                            UserType = reader["userType"].ToString(),
                            Email = reader["email"].ToString() // Retrieve email from Users table
                        };
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error searching for user: {ex.Message}"); // Log error message
        }
        finally
        {
            CloseConnection(); // Ensure the connection is closed
        }

        return user; // Return the populated User object
    }

    public List<UserData> SearchAppointments(int userId)
    {
        List<UserData> appointments = new List<UserData>(); // Initialize the list to hold UserData objects
        try
        {
            OpenConnection(); // Open a connection to the database
            string query = @"
                    SELECT 
                        u.firstName AS FirstName, 
                        u.lastName AS LastName, 
                        d.doctorID,
                        du.firstName AS DoctorFirstName,
                        du.lastName AS DoctorLastName,
                        a.appointmentDate,
                        a.notes
                    FROM 
                        HealthManagement.dbo.Users AS u
                    JOIN 
                        HealthManagement.dbo.Patients AS p ON u.userID = p.userID
                    JOIN 
                        HealthManagement.dbo.Appointments AS a ON p.patientID = a.patientID
                    JOIN 
                        HealthManagement.dbo.Doctors AS d ON a.doctorID = d.doctorID
                    JOIN 
                        HealthManagement.dbo.Users AS du ON d.userID = du.userID  
                    WHERE 
                        u.userID = @userId;"; // SQL query to retrieve appointments for a user

            using (SqlCommand command = new SqlCommand(query, GetConnection()))
            {
                command.Parameters.AddWithValue("@userId", userId); // Parameterized query to prevent SQL injection

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // Retrieve the latest key and IV for decryption
                    var (key, iv) = _encryptionManager.RetrieveLatestKey(); // Use instance here
                    if (key == null || iv == null)
                    {
                        throw new Exception("No encryption key or IV found in the database."); // Handle missing key or IV
                    }

                    while (reader.Read()) // Loop through the results
                    {
                        var userData = new UserData
                        {
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            DoctorID = reader["doctorID"].ToString(),
                            DoctorFirstName = reader["DoctorFirstName"].ToString(),
                            DoctorLastName = reader["DoctorLastName"].ToString(),
                            VisitDate = Convert.ToDateTime(reader["appointmentDate"]).ToString("MM/dd/yyyy"), // Format appointment date
                            Notes = _encryptionManager.Decrypt(reader["notes"].ToString()) // Decrypt the notes for display
                        };

                        appointments.Add(userData); // Add UserData object to the list
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error searching for appointments: {ex.Message}"); // Log error message
        }
        finally
        {
            CloseConnection(); // Ensure the connection is closed
        }

        return appointments; // Return the list of UserData
    }

    internal bool UpdateUser(string userID, string firstName, string lastName, string username, string userType)
    {
        try
        {
            int userIdInt;
            if (!int.TryParse(userID, out userIdInt))
            {
                return false; // Handle invalid userId if necessary
            }
            OpenConnection(); // Open a connection to the database
            string query = @"
                UPDATE Users
                SET firstName = @firstName, lastName = @lastName, username = @username, userType = @userType
                WHERE userID = @userId;"; // SQL query to update user information

            using (SqlCommand command = new SqlCommand(query, GetConnection()))
            {
                // Add parameters to the SQL command
                command.Parameters.AddWithValue("@firstName", firstName);
                command.Parameters.AddWithValue("@lastName", lastName);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@userType", userType);
                command.Parameters.AddWithValue("@userId", userIdInt);

                int rowsAffected = command.ExecuteNonQuery(); // Execute the update command
                return rowsAffected > 0; // Return true if at least one row was updated
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating user: {ex.Message}"); // Log error message
            return false; // Return false in case of error
        }
        finally
        {
            CloseConnection(); // Ensure the connection is closed
        }
    }

    public bool UpdateAppointment(int appointmentId, string visitDate, string notes)
    {
        try
        {
            OpenConnection();
            string query = @"
                UPDATE Appointments
                SET appointmentDate = @visitDate,
                    notes = @notes
                WHERE appointmentID = @appointmentId;";

            using (SqlCommand command = new SqlCommand(query, GetConnection()))
            {
                command.Parameters.AddWithValue("@appointmentId", appointmentId);
                command.Parameters.AddWithValue("@visitDate", visitDate);
                command.Parameters.AddWithValue("@notes", notes);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0; // Return true if at least one row was updated
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating appointment: {ex.Message}");
            return false;
        }
        finally
        {
            CloseConnection();
        }
    }
    public string EncryptNotes(string notes)
    {
        // Retrieve the latest key and IV for encryption
        var (key, iv) = _encryptionManager.RetrieveLatestKey();
        if (key == null || iv == null)
        {
            throw new Exception("No encryption key or IV found in the database.");
        }

        // Call the Encrypt method with the key and IV
        return _encryptionManager.Encrypt(notes, key, iv);
    }

    public string DecryptNotes(string encryptedNotes)
    {
        return _encryptionManager.Decrypt(encryptedNotes); // Decrypt notes for display
    }

}


// Helper Classes
public class User
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string UserType { get; set; } // Include UserType
}

public class UserData
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DoctorID { get; set; }
    public string DoctorFirstName { get; set; }
    public string DoctorLastName { get; set; }
    public string VisitDate { get; set; }
    public string Notes { get; set; }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public class AdminManager : SqlConnectionManager
{
    public AdminManager() : base() // Calls the base constructor
    {
    }

    public DataTable GetAllUsers()
    {
        DataTable userTable = new DataTable();
        try
        {
            OpenConnection();
            string query = "SELECT firstName, lastName, userID, username, userType FROM Users";

            using (SqlCommand command = new SqlCommand(query, GetConnection()))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(userTable);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving user data: {ex.Message}");
        }
        finally
        {
            CloseConnection();
        }

        return userTable; // Return the DataTable containing user data
    }

    public DataTable GetUsersByType(string userType)
    {
        DataTable userTable = new DataTable();
        try
        {
            OpenConnection();
            string query = "SELECT firstName, lastName, userID, username, userType FROM Users WHERE userType = @userType";

            using (SqlCommand command = new SqlCommand(query, GetConnection()))
            {
                command.Parameters.AddWithValue("@userType", userType);
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(userTable);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving user data by type: {ex.Message}");
        }
        finally
        {
            CloseConnection();
        }

        return userTable; // Return the DataTable containing filtered user data
    }

    public User SearchUser(string firstName, string lastName, string userId)
    {
        User user = null; // Initialize user object
        try
        {

            // Convert userId to int if possible
            int? userIdInt = null;
            if (int.TryParse(userId, out int parsedId))
            {
                userIdInt = parsedId;
            }

            OpenConnection();
            string query = @"
            SELECT firstName, lastName, userID, email, userType, username
            FROM Users 
            WHERE (@firstName IS NULL OR firstName = @firstName)
              AND (@lastName IS NULL OR lastName = @lastName)
              AND (@userID IS NULL OR userID = @userID);";

            using (SqlCommand command = new SqlCommand(query, GetConnection()))
            {
                command.Parameters.AddWithValue("@firstName", firstName);
                command.Parameters.AddWithValue("@lastName", lastName);
                command.Parameters.AddWithValue("@userID", userId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User
                        {
                            FirstName = reader["firstName"].ToString(),
                            LastName = reader["lastName"].ToString(),
                            UserId = reader["userID"].ToString(),
                            Username = reader["username"].ToString(), // If userName is still in UserLogin, adjust as needed
                            UserType = reader["userType"].ToString(),
                            Email = reader["email"].ToString() // Include email from Users table
                        };
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error searching for user: {ex.Message}");
        }
        finally
        {
            CloseConnection();
        }

        return user; // Return the populated user object
    }


    public List<UserData> SearchAppointments(int userId)
    {
        List<UserData> appointments = new List<UserData>(); // Initialize the list
        try
        {
            OpenConnection();
            string query = @"
                        SELECT 
                            u.firstName AS FirstName,  -- Change to match UserData
                            u.lastName AS LastName,     -- Change to match UserData
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
                            u.userID = @userId;";



            using (SqlCommand command = new SqlCommand(query, GetConnection()))
            {
                command.Parameters.AddWithValue("@userId", userId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var userData = new UserData
                        {
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            DoctorID = reader["doctorID"].ToString(),
                            DoctorFirstName = reader["DoctorFirstName"].ToString(),
                            DoctorLastName = reader["DoctorLastName"].ToString(),
                            VisitDate = Convert.ToDateTime(reader["appointmentDate"]).ToString("MM/dd/yyyy"),
                            Notes = reader["notes"].ToString()
                        };

                        appointments.Add(userData);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error searching for appointments: {ex.Message}");
        }
        finally
        {
            CloseConnection();
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
            OpenConnection();
            string query = @"
                UPDATE Users
                SET firstName = @firstName, lastName = @lastName, username = @username, userType = @userType
                WHERE userID = @userId;";

            using (SqlCommand command = new SqlCommand(query, GetConnection()))
            {
                command.Parameters.AddWithValue("@firstName", firstName);
                command.Parameters.AddWithValue("@lastName", lastName);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@userType", userType);
                command.Parameters.AddWithValue("@userId", userIdInt);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0; // Return true if at least one row was updated
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating user: {ex.Message}");
            return false;
        }
        finally
        {
            CloseConnection();
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

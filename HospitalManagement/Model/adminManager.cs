using System;
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
            string query = "SELECT u.firstName, u.lastName, u.userID, u.address, u.phone, ul.userName, ul.userType FROM Users u INNER JOIN UserLogin ul ON u.userID = ul.userID";

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
            string query = @"
            SELECT u.firstName, u.lastName, u.userID, u.address, u.phone, ul.userName, ul.userType 
            FROM Users u 
            INNER JOIN UserLogin ul ON u.userID = ul.userID 
            WHERE ul.userType = @userType";

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
            OpenConnection();

            // First query to get basic user details
            string query = @"
                SELECT u.firstName, u.lastName, u.userID, u.address, u.phone 
                FROM Users u
                WHERE (@firstName IS NULL OR u.firstName = @firstName)
                  AND (@lastName IS NULL OR u.lastName = @lastName)
                  AND (@userId IS NULL OR u.userID = @userId);";

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
                            Address = reader["address"].ToString(),
                            PhoneNumber = reader["phone"].ToString()
                        };
                    }
                }
            }

            // If user is found, retrieve username and user type
            if (user != null)
            {
                string secondQuery = @"
                    SELECT ul.userName, ul.userType 
                    FROM UserLogin ul 
                    WHERE ul.userID = @userID;";

                using (SqlCommand command = new SqlCommand(secondQuery, GetConnection()))
                {
                    command.Parameters.AddWithValue("@userID", user.UserId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user.Username = reader["userName"].ToString();
                            user.UserType = reader["userType"].ToString();
                        }
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

    public UserData SearchData(string firstName, string lastName, string userId)
    {
            UserData userData = null; // Initialize userData object
            try
            {
                OpenConnection();

                // SQL query to get user data details
                string query = @"
            SELECT u.firstName, u.lastName, u.visitDate, u.notes
            FROM UserData u
            WHERE (@firstName IS NULL OR u.firstName = @firstName)
              AND (@lastName IS NULL OR u.lastName = @lastName)
              AND (@userId IS NULL OR u.userID = @userId);";

                using (SqlCommand command = new SqlCommand(query, GetConnection()))
                {
                    command.Parameters.AddWithValue("@firstName",firstName);
                    command.Parameters.AddWithValue("@lastName",lastName);
                    command.Parameters.AddWithValue("@userId", userId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userData = new UserData
                            {
                                FirstName = reader["firstName"].ToString(),
                                LastName = reader["lastName"].ToString(),
                                VisitDate = reader["visitDate"].ToString(), // Keep as string
                                Notes = reader["notes"].ToString()
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching for user data: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }

            return userData; // Return the populated UserData object
        }
    internal bool UpdateUser(string userId, string firstName, string lastName, string address, string phoneNumber, string username, string userType)
    {
            try
            {

                // Convert the userId to an integer
                int userIdInt;
                if (!int.TryParse(userId, out userIdInt))
                {
                    // Handle invalid userId if necessary
                    return false;
                }
            OpenConnection();
                string query = @"
                UPDATE Users
                SET firstName = @firstName, lastName = @lastName, address = @address, phone = @phone
                WHERE userID = (SELECT userID FROM UserLogin WHERE userName = @userName);

                UPDATE UserLogin
                SET userName = @userName, userType = @userType
                WHERE userName = @userName;";

                using (SqlCommand command = new SqlCommand(query, GetConnection()))
                {
                    command.Parameters.AddWithValue("@firstName", firstName);
                    command.Parameters.AddWithValue("@lastName", lastName);
                    command.Parameters.AddWithValue("@address", address);
                    command.Parameters.AddWithValue("@phone", phoneNumber);
                    command.Parameters.AddWithValue("@userName", username);
                    command.Parameters.AddWithValue("@userType", userType);

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
}

// Helper Classes
public class User
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserId { get; set; }
    public string Username { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string UserType { get; set; } // Include UserType
}

public class UserData
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string VisitDate { get; set; }
    public string Notes { get; set; }

}
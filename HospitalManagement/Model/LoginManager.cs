using System;
using System.Data.SqlClient;

public class LoginManager : SqlConnectionManager
{
    public LoginManager() : base() // Calls the base constructor
    {
    }

    public (bool isValid, string userType) ValidateUser(string username, string password)
    {
        try
        {
            OpenConnection();
            string query = "SELECT userType FROM userLogin WHERE username=@username AND password=@password;";

            using (SqlCommand command = new SqlCommand(query, GetConnection())) 
            {
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);

                var result = command.ExecuteScalar();
                if (result!= null)
                {
                    string userType = result.ToString();
                    return (true,userType);
                }
                else
                    return (false, null);
            }
        }

        catch (Exception ex)
        {
            Console.WriteLine($"Error validating user: {ex.Message}");
            return (false, null);
        }
        finally
        {
            CloseConnection();
        }
    }
}

// Author : Andrew Crandall
// Date Modified : 11/3/2024
// Title : DatabaseConnectionManager
// Purpose : Establish connection to database

using System;
using System.Data.SqlClient;

public class SqlConnectionManager
{
    // Store the connection string as a constant
    private const string ConnectionString = "Server=LAPTOP-ETH969NJ;Database=HealthManagement;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";
    private SqlConnection _sqlConnection;

    // Constructor
    public SqlConnectionManager()
    {
        _sqlConnection = new SqlConnection(ConnectionString);
    }

    // Method to return sqlConnection status
    protected SqlConnection GetConnection()
    {
        return _sqlConnection;
    }


    // Method to establish a connection to the database
    public bool OpenConnection()
    {
        try
        {
            if (_sqlConnection.State != System.Data.ConnectionState.Open)
            {
                _sqlConnection.Open();
            }
            return true;
        }
        catch (Exception ex)
        {
            // Log the exception or handle it accordingly
            Console.WriteLine($"Error opening connection, try again later: {ex.Message}");
            return false;
        }
    }

    // Method to close the connection
    public void CloseConnection()
    {
        if (_sqlConnection.State == System.Data.ConnectionState.Open)
        {
            _sqlConnection.Close();
        }
    }

    // Method to check if the connection is open
    public bool IsConnectionOpen()
    {
        return _sqlConnection.State == System.Data.ConnectionState.Open;
    }

    // Optional: Dispose method to clean up resources
    public void Displose()
    {
        CloseConnection();
        _sqlConnection.Dispose();
    }
}

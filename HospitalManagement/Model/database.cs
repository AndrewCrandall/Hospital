// Author : Andrew Crandall
// Date Modified : 11/3/2024
// Title : database
// Purpose : Provide the connection string for the database
using System.Data.SqlClient;

namespace HospitalManagement.DataAccess
{
    public class database
    {
        private string connectionString = "Server=LAPTOP-ETH969NJ;Database=HealthManagement;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

    }
}

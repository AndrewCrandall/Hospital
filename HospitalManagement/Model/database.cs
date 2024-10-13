using System;
using System.Data.SqlClient;

namespace HospitalManagement.DataAccess
{
    public class database
    {
        private string connectionString = "Server=LAPTOP-ETH969NJ;Database=Healthcare;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

        public bool ValidateUser(string username, string password)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string loginQuery = "SELECT COUNT(*) FROM userLogin WHERE username=@username AND password=@password;";
                using (SqlCommand cmd = new SqlCommand(loginQuery, con))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
    }
}

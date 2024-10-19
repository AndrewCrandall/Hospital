using System;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

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
                if (result != null)
                {
                    string userType = result.ToString();
                    return (true, userType);
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

    public string GenerateMfaCode()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString(); // Generate a 6-digit code
    }

    public void SendMfaCodeToUser(string username, string mfaCode)
    {
        // Your Twilio credentials
        const string accountSid = "YOUR_ACCOUNT_SID"; // Replace with your Account SID
        const string authToken = "YOUR_AUTH_TOKEN";   // Replace with your Auth Token

        // Initialize the Twilio client
        TwilioClient.Init(accountSid, authToken);

        // Send the SMS
        var message = MessageResource.Create(
            body: $"Your MFA code is: {mfaCode}",
            from: new PhoneNumber("+phonenumber"), // Your Twilio phone number
            to: new PhoneNumber("+phonenumber") // The user's phone number
        ); // Added c
    }

    public void CreateAuthCookie(string username, string userType, HttpContext context)
    {
        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
            1,
            username,
            DateTime.Now,
            DateTime.Now.AddMinutes(130),
            false,
            userType,
            FormsAuthentication.FormsCookiePath);

        string encryptedTicket = FormsAuthentication.Encrypt(ticket);
        HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
        {
            Path = "/",
            Secure = false, // Set to true for HTTPS
            HttpOnly = true
        };

        context.Response.Cookies.Add(authCookie);
    }

    public void RedirectUser(string userType, HttpContext context)
    {
        switch (userType)
        {
            case "Admin":
                context.Response.Redirect("~/View/Admin/adminDashboard.aspx");
                break;
            case "Doctor":
                context.Response.Redirect("~/View/Doctor/doctorDashboard.aspx");
                break;
            case "Patient":
                context.Response.Redirect("~/View/Patient/patientDashboard.aspx");
                break;
            default:
                context.Response.Write("<script>alert('Unknown user type.');</script>");
                context.Response.Redirect("~/View/Login/login.aspx");
                break;
        }
    }
}

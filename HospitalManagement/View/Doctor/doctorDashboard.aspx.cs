using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HospitalManagement.View
{
    public partial class doctorDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Retrieve the username from the session
                var username = Session["Username"]?.ToString();

                // Display the username if it exists
                if (!string.IsNullOrEmpty(username))
                {
                    usernameLabel.Text = $"Welcome, {username}!";
                }
                else
                {
                    usernameLabel.Text = "Welcome to the Doctor Dashboard";
                }
            }
        }


        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            // Clear the session
            Session.Clear();

            // Optionally, you can redirect to the login page or homepage
            Response.Redirect("~/View/Login/Login.aspx");
        }
    }
}
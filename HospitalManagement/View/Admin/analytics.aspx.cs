using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HospitalManagement.View.Admin
{
    public partial class analytics : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData("All");
            }
        }

        private void LoadData(string userType)
        {
            AdminManager adminManager = new AdminManager();
            DataTable userData;

            if (userType == "All")
            {
                userData = adminManager.GetAllUsers(); // Get all users
            }
            else
            {
                userData = adminManager.GetUsersByType(userType); // Get users by type
            }

            YourGridView.DataSource = userData;
            YourGridView.DataBind();
        }

        protected void FilterData(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string userType = btn.CommandArgument; // Get the command argument (All, Admin, Doctor, Patient)
            LoadData(userType); // Load data based on the selected user type
        }

        protected void RefreshData(object sender, EventArgs e)
        {
            // Refresh the currently displayed data
            string currentType = ViewState["CurrentUserType"] as string ?? "All"; // Get the current user type from ViewState
            LoadData(currentType); // Load data based on the current user type
        }

        protected void GoBack(object sender, EventArgs e)
        {
            // Navigate back to the previous page
            Response.Redirect("~/View/Admin/adminDashboard.aspx");
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            // Store the current user type in ViewState for refreshing
            ViewState["CurrentUserType"] = ViewState["CurrentUserType"] ?? "All";
        }

    }
}

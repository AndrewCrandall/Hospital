<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="adminDashboard.aspx.cs" Inherits="HospitalManagement.View.adminDashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin Dashboard</title>
    <link rel="stylesheet" type="text/css" href="../styles.css" />
    <style type="text/css">
        .logout-button {
            height: 35px;
        }
    </style>
</head>
<body>
    <form id="form2" runat="server">
        <asp:Label ID="welcomeLabel" runat="server" Text="Welcome to the Admin Dashboard"></asp:Label>

        <!-- Content area for additional features or information -->
        <div id="content">
            <h2>Dashboard Overview</h2>
            <p>This is where you can manage users, oversee data, and analyze reports.</p>
            <!-- Additional dashboard content can go here -->
        </div>
        <div>
            <nav>
                <ul>
                    <li><a href="userManagement.aspx">User Updating</a></li>
                    <li><a href="dataManagement.aspx">Data Management</a></li>
                    <li><a href="analytics.aspx">Analytics</a></li>
                </ul>
            </nav>
        </div>

        <!-- Logout Button -->
        <div style="text-align: center; margin-top: 20px;">
            <asp:Button ID="logoutButton" runat="server" Text="Logout" OnClick="LogoutButton_Click" CssClass="logout-button" />
        </div>
    </form>
</body>
</html>

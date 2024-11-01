<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="doctorDashboard.aspx.cs" Inherits="HospitalManagement.View.doctorDashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Doctor Dashboard</title>
    <link rel="stylesheet" type="text/css" href="../styles.css" />
</head>
<body>
    <form id="form2" runat="server">
        <div class="container">
            <asp:Label ID="welcomeLabel" runat="server" Text="Welcome to the Doctor Dashboard" CssClass="welcome-label"></asp:Label>
            <asp:Label ID="usernameLabel" runat="server" Text="" CssClass="username-label"></asp:Label>

            <!-- Content area for additional features or information -->
            <div id="content" class="content-area">
                <h2>Dashboard Overview</h2>
                <p>This is where you can manage your appointments and notes.</p>
                <!-- Additional dashboard content can go here -->
            </div>

            <nav class="navigation">
                <ul>
                    <li><a href="addNotes.aspx">Add Notes</a></li>
                    <li><a href="viewNotes.aspx">View Notes</a></li>
                </ul>
            </nav>

            <!-- Logout Button -->
            <div class="button-section">
                <asp:Button ID="logoutButton" runat="server" Text="Logout" OnClick="LogoutButton_Click" CssClass="logout-button" />
            </div>
        </div>
    </form>
</body>
</html>

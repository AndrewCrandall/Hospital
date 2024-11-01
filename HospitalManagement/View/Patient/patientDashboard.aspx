<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="patientDashboard.aspx.cs" Inherits="HospitalManagement.View.patientDashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Patient Dashboard</title>
    <link rel="stylesheet" type="text/css" href="../styles.css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:Label ID="welcomeLabel" runat="server" Text="Welcome to the Patient Dashboard" CssClass="welcome-label"></asp:Label>
        <asp:Label ID="usernameLabel" runat="server" Text="" CssClass="username-label"></asp:Label>

        <!-- Content area for additional features or information -->
        <div id="content" class="content-area">
            <h2>Dashboard Overview</h2>
            <p>This is where you can manage your appointments and view your medical records.</p>
            <!-- Additional dashboard content can go here -->
        </div>

        <nav class="navigation">
            <ul>
                <li><a href="viewRecords.aspx">View Appointments</a></li>
                <li><a href="updateProfile.aspx">Update Profile</a></li>
            </ul>
        </nav>

        <!-- Logout Button -->
        <div style="text-align: center; margin-top: 20px;">
            <asp:Button ID="logoutButton" runat="server" Text="Logout" OnClick="LogoutButton_Click" CssClass="logout-button" />
        </div>
    </form>
</body>
</html>

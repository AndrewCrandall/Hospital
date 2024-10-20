<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="doctorDashboard.aspx.cs" Inherits="HospitalManagement.View.doctorDashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Doctor Dashboard</title>
    <link rel="stylesheet" type="text/css" href="styles.css" />
    <style type="text/css">
        .logout-button {
            height: 35px;
        }
    </style>
</head>
<body>
    <form id="form2" runat="server">
        <asp:Label ID="welcomeLabel" runat="server" Text="Welcome to the Doctor Dashboard"></asp:Label>
        <asp:Label ID="usernameLabel" runat="server" Text=""></asp:Label>


        <!-- Content area for additional features or information -->
        <div id="content">
            <!-- Additional dashboard content can go here -->
        </div>
        <div>
            <nav>
                <ul>
                    <li><a href="addNotes.aspx">User Updating</a></li>
                    <li><a href="viewNotes.aspx">Data Management</a></li>
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
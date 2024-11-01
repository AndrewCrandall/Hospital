<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="userManagement.aspx.cs" Inherits="HospitalManagement.View.Admin.userManagement" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Management</title>
    <link rel="stylesheet" type="text/css" href="adminstyles2.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="user-management-container" class="container">
            <h1>User Management</h1>

            <section id="search-section" class="upper-half">
                <h2>Search</h2>
                <div class="editable-fields">
                    <label for="firstNameInput">First Name:</label>
                    <asp:TextBox ID="firstNameInput" runat="server" />

                    <label for="lastNameInput">Last Name:</label>
                    <asp:TextBox ID="lastNameInput" runat="server" />

                    <label for="userIdInput">User ID:</label>
                    <asp:TextBox ID="userIdInput" runat="server" />
                </div>
                <asp:Button ID="searchBtn" runat="server" Text="Search" OnClick="Search_Click" />
            </section>

            <section id="user-details-section" class="lower-half">
                <h2>User Details</h2>
                <div class="readonly-fields">
                    <label>First Name:</label>
                    <asp:TextBox ID="displayFirstName" runat="server" />

                    <label>Last Name:</label>
                    <asp:TextBox ID="displayLastName" runat="server" />

                    <label>Username:</label>
                    <asp:TextBox ID="displayUsername" runat="server" ReadOnly="true"/>

                    <label>Email Address:</label>
                    <asp:TextBox ID="displayAddress" runat="server" />

                    <label>User Type:</label>
                    <asp:DropDownList ID="displayUserType" runat="server" CssClass="text-box">
                        <asp:ListItem Text="Admin" Value="Admin" />
                        <asp:ListItem Text="Doctor" Value="Doctor" />
                        <asp:ListItem Text="Patient" Value="Patient" />
                    </asp:DropDownList>
                </div>
            </section>

            <div id="button-section" class="button-section">
                <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="Back_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="Cancel_Click" />
                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="Save_Click" />
            </div>
        </div>
    </form>
</body>
</html>

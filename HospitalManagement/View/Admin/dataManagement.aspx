<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dataManagement.aspx.cs" Inherits="HospitalManagement.View.Admin.dataManagement" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="upper-half">
                <h2>Search</h2>
                <div class="search-section">
                    <div class="editable-fields">
                        <label for="firstNameInput">First Name:</label>
                        <asp:TextBox ID="firstNameInput" runat="server" />

                        <label for="lastNameInput">Last Name:</label>
                        <asp:TextBox ID="lastNameInput" runat="server" />

                        <label for="userIdInput">User ID:</label>
                        <asp:TextBox ID="userIdInput" runat="server" />
                    </div>
                    <asp:Button ID="searchBtn" runat="server" Text="Search" OnClick="Search_Click" />
                </div>
            </div>

            <div class="lower-half">
                <h2>User Details</h2>
                <div class="readonly-fields">
                    <label>First Name:</label>
                    <asp:TextBox ID="displayFirstName" runat="server" ReadOnly="true" />

                    <label>Last Name:</label>
                    <asp:TextBox ID="displayLastName" runat="server" ReadOnly="true" />

                    <label>Visit Date:</label>
                    <asp:TextBox ID="displayDate" runat="server" />

                    <label>Notes:</label>
                    <asp:TextBox ID="displayNotes" runat="server" />
                </div>
            </div>

            <div class="button-section">
                <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="Back_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="Cancel_Click" />
                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="Save_Click" />
            </div>
        </div>
    </form>
</body>
</html>

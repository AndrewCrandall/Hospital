<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="updateProfile.aspx.cs" Inherits="HospitalManagement.View.Patient.updateProfile" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Update Profile</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Update Profile</h2>
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
            <br />

            <asp:Label ID="lblUsername" runat="server" Text="Username:" AssociatedControlID="txtUsername"></asp:Label>
            <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
            <br />

            <asp:Label ID="lblPassword" runat="server" Text="Password:" AssociatedControlID="txtPassword"></asp:Label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
            <br />

            <asp:Label ID="lblFirstName" runat="server" Text="First Name:" AssociatedControlID="txtFirstName"></asp:Label>
            <asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox>
            <br />

            <asp:Label ID="lblLastName" runat="server" Text="Last Name:" AssociatedControlID="txtLastName"></asp:Label>
            <asp:TextBox ID="txtLastName" runat="server"></asp:TextBox>
            <br />

            <asp:Label ID="lblEmail" runat="server" Text="Email:" AssociatedControlID="txtEmail"></asp:Label>
            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
            <br />

            <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
            <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
        </div>
    </form>
</body>
</html>

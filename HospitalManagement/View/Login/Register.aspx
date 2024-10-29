<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="HospitalManagement.Register" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Register</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <label for="usernameTxt">Username:</label>
            <asp:TextBox ID="usernameTxt" runat="server"></asp:TextBox><br />

            <label for="passwordTxt">Password:</label>
            <asp:TextBox ID="passwordTxt" runat="server" TextMode="Password"></asp:TextBox><br />

            <label for="emailTxt">Email:</label>
            <asp:TextBox ID="emailTxt" runat="server"></asp:TextBox><br />

            <label for="firstNameTxt">First Name:</label>
            <asp:TextBox ID="firstNameTxt" runat="server"></asp:TextBox><br />

            <label for="lastNameTxt">Last Name:</label>
            <asp:TextBox ID="lastNameTxt" runat="server"></asp:TextBox><br />

            <asp:Button ID="registerBtn" runat="server" Text="Register" OnClick="RegisterUser" />
            <asp:Button ID="backBtn" runat="server" Text="Back" OnClick="ReturnUser" />
        </div>
    </form>
</body>
</html>

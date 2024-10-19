<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="HospitalManagement.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <link rel="stylesheet" type="text/css" href="styles.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2><asp:Label ID="headerLabel" runat="server" Text="Login"></asp:Label></h2>
        </div>
        <fieldset>
            <div>
                <asp:Label ID="usernameLabel" runat="server" Text="Username"></asp:Label>
                <asp:TextBox ID="usernameTxt" runat="server"></asp:TextBox>
            </div>
            <div>
                <asp:Label ID="passwordLabel" runat="server" Text="Password"></asp:Label>
                <asp:TextBox ID="passwordTxt" runat="server" TextMode="Password"></asp:TextBox>
            </div>
            <div>
                <asp:Button ID="cancelBtn" runat="server" Text="Cancel" CssClass="btn" />
                <asp:Button ID="loginBtn" runat="server" Text="Login" CssClass="btn" OnClick="loginBtn_Click" />
            </div>
        </fieldset>
    </form>
</body>
</html>

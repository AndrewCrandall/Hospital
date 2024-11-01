<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="HospitalManagement.Login" Async="true" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <title>Login</title>
    <link rel="stylesheet" type="text/css" href="../styles.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="content">
            <fieldset>
                <legend>Login</legend>
                <div class="form-group">
                    <label for="usernameTxt">Username:</label>
                    <asp:TextBox ID="usernameTxt" runat="server" CssClass="text-box" />
                </div>
                <div class="form-group">
                    <label for="passwordTxt">Password:</label>
                    <asp:TextBox ID="passwordTxt" runat="server" TextMode="Password" CssClass="text-box" />
                </div>
                <div class="button-section">
                    <asp:Button ID="cancelBtn" runat="server" Text="Cancel" CssClass="action-button" />
                    <asp:Button ID="loginBtn" runat="server" Text="Login" CssClass="action-button" OnClick="loginBtn_Click" />
                    <asp:Button ID="registerBtn" runat="server" Text="Register" CssClass="action-button" OnClick="RegisterBtn_Click" />
                </div>
            </fieldset>
        </div>
    </form>
</body>
</html>

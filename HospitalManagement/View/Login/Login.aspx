<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="HospitalManagement.Login" Async="true" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <title>Login</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <fieldset>
                <legend>Login</legend>
                <div>
                    <label for="usernameTxt">Username:</label>
                    <asp:TextBox ID="usernameTxt" runat="server" CssClass="form-control" />
                </div>
                <div>
                    <label for="passwordTxt">Password:</label>
                    <asp:TextBox ID="passwordTxt" runat="server" TextMode="Password" CssClass="form-control" />
                </div>
                <div>
                    <asp:Button ID="cancelBtn" runat="server" Text="Cancel" CssClass="btn" />
                    <asp:Button ID="loginBtn" runat="server" Text="Login" CssClass="btn" OnClick="loginBtn_Click" />
                </div>
            </fieldset>
        </div>
    </form>
</body>
</html>

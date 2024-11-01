<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mfaVerification.aspx.cs" Inherits="HospitalManagement.View.Login.mfaVerification" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MFA Verification</title>
    <link rel="stylesheet" type="text/css" href="../styles.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="content">
            <h2>Enter MFA Code</h2>
            <div class="form-group">
                <asp:TextBox ID="mfaCodeTxt" runat="server" CssClass="text-box" placeholder="Enter MFA code"></asp:TextBox>
            </div>
            <div class="button-section">
                <asp:Button ID="mfaVerifyBtn" runat="server" OnClick="mfaVerifyBtn_Click" Text="Verify" CssClass="action-button" />
                <asp:Button ID="cancelBtn" runat="server" Text="Cancel" OnClick="Cancel_Click" CssClass="button-back" />
            </div>
        </div>
    </form>
</body>
</html>

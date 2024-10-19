<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MfaVerification.aspx.cs" Inherits="HospitalManagement.MfaVerification" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MFA Verification</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Enter MFA Code</h2>
            <asp:TextBox ID="mfaCodeTxt" runat="server" placeholder="Enter MFA code"></asp:TextBox>
            <asp:Button ID="mfaVerifyBtn" runat="server" OnClick="mfaVerifyBtn_Click" Text="Verify" />
        </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="analytics.aspx.cs" Inherits="HospitalManagement.View.Admin.analytics" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Analytics</title>
    <link rel="stylesheet" type="text/css" href="../styles.css" />

</head>
<body>
    <form id="form1" runat="server">
        <div style="text-align:center; margin-bottom: 20px;">
            <asp:Button ID="btnAll" runat="server" Text="All" OnClick="FilterData" CommandArgument="All" />
            <asp:Button ID="btnAdmin" runat="server" Text="Admin" OnClick="FilterData" CommandArgument="Admin" />
            <asp:Button ID="btnDoctor" runat="server" Text="Doctor" OnClick="FilterData" CommandArgument="Doctor" />
            <asp:Button ID="btnPatient" runat="server" Text="Patient" OnClick="FilterData" CommandArgument="Patient" />
            <br /><br />
            <asp:Button ID="btnRefresh" runat="server" Text="Refresh" OnClick="RefreshData" />
            <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="GoBack" />
        </div>
        <asp:GridView ID="YourGridView" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="firstName" HeaderText="First Name" />
                <asp:BoundField DataField="lastName" HeaderText="Last Name" />
                <asp:BoundField DataField="userID" HeaderText="User ID" />
                <asp:BoundField DataField="userName" HeaderText="Username" />
                <asp:BoundField DataField="userType" HeaderText="User Type" />
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>

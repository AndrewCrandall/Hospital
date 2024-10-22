<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="viewRecords.aspx.cs" Inherits="HospitalManagement.View.Patient.viewRecords" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View Records</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Your Medical Records</h2>
                <asp:GridView ID="RecordsGridView" runat="server" AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField DataField="appointmentID" HeaderText="Appointment ID" />
                        <asp:BoundField DataField="appointmentDate" HeaderText="Appointment Date" />
                        <asp:BoundField DataField="notes" HeaderText="Notes" />
                        <asp:BoundField DataField="DoctorFirstName" HeaderText="Doctor First Name" />
                        <asp:BoundField DataField="DoctorLastName" HeaderText="Doctor Last Name" />
                    </Columns>
                </asp:GridView>
            <br />
            <asp:Button ID="BackButton" runat="server" Text="Back to Dashboard" OnClick="BackButton_Click" />
        </div>
    </form>
</body>
</html>

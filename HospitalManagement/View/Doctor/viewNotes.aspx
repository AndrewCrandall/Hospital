<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="viewNotes.aspx.cs" Inherits="HospitalManagement.View.Doctor.viewNotes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View Appointment Notes</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Appointment Notes</h2>

            <asp:GridView ID="NotesGridView" runat="server" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="PatientFirstName" HeaderText="Patient First Name" />
                    <asp:BoundField DataField="PatientLastName" HeaderText="Patient Last Name" />
                    <asp:BoundField DataField="AppointmentDate" HeaderText="Appointment Date" />
                    <asp:BoundField DataField="Notes" HeaderText="Notes" />
                    <asp:BoundField DataField="Status" HeaderText="Status" />
                </Columns>
            </asp:GridView>

            <div>
                <asp:Button ID="BackButton" runat="server" Text="Back" OnClick="BackButton_Click" />
            </div>
        </div>
    </form>
</body>
</html>

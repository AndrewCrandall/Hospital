<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="viewNotes.aspx.cs" Inherits="HospitalManagement.View.Doctor.viewNotes" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View Appointment Notes</title>
    <link rel="stylesheet" type="text/css" href="../styles.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="content">
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

            <div class="button-section" style="margin-top: 20px;">
                <asp:Button ID="BackButton" runat="server" Text="Back" OnClick="BackButton_Click" CssClass="button-back" />
            </div>
        </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="addNotes.aspx.cs" Inherits="HospitalManagement.View.Doctor.addNotes" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Appointment Notes</title>
    <link rel="stylesheet" type="text/css" href="../styles.css" />
</head>
<body>
    <form id="addNotes" runat="server">
        <div class="container">
            <h2>Add Appointment Notes</h2>
            <asp:Label ID="usernameLabel" runat="server" Text="" CssClass="username-label"></asp:Label>

            <div class="appointment-details">
                <div class="form-group">
                    <label for="patientFirstName">Patient First Name:</label>
                    <asp:TextBox ID="patientFirstName" runat="server" CssClass="text-box" />
                </div>

                <div class="form-group">
                    <label for="patientLastName">Patient Last Name:</label>
                    <asp:TextBox ID="patientLastName" runat="server" CssClass="text-box" />
                </div>

                <div class="form-group">
                    <label for="appointmentDate">Appointment Date:</label>
                    <asp:TextBox ID="appointmentDate" runat="server" TextMode="Date" CssClass="text-box" />
                </div>

                <div class="form-group">
                    <label for="notes">Notes:</label>
                    <asp:TextBox ID="notes" runat="server" TextMode="MultiLine" Rows="5" CssClass="text-box" />
                </div>
            </div>

            <div class="button-section">
                <asp:Button ID="btnSave" runat="server" Text="Save Notes" OnClick="SaveNotes_Click" CssClass="action-button" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="Cancel_Click" CssClass="action-button" />
                <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="Back_Click" CssClass="button-back" />
            </div>
        </div>
    </form>
</body>
</html>

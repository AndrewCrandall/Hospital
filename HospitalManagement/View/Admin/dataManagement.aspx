<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dataManagement.aspx.cs" Inherits="HospitalManagement.View.Admin.dataManagement" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Appointment Management</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="upper-half">
                <h2>Search</h2>
                <div class="search-section">
                    <label for="userIdInput">User ID:</label>
                    <asp:TextBox ID="userIdInput" runat="server" />
                    <asp:Button ID="searchBtn" runat="server" Text="Search" OnClick="Search_Click" />
                </div>
            </div>

            <div class="lower-half">
                <h2>User Appointments</h2>
                <div class="appointment-details">
                    <asp:GridView ID="AppointmentGridView" runat="server" AutoGenerateColumns="False" OnRowEditing="AppointmentGridView_RowEditing" 
                        OnRowUpdating="AppointmentGridView_RowUpdating" OnRowCancelingEdit="AppointmentGridView_RowCancelingEdit">
                        <Columns>
                            <asp:BoundField DataField="FirstName" HeaderText="Patient First Name" />
                            <asp:BoundField DataField="LastName" HeaderText="Patient Last Name" />
                            <asp:BoundField DataField="DoctorFirstName" HeaderText="Doctor First Name" />
                            <asp:BoundField DataField="DoctorLastName" HeaderText="Doctor Last Name" />
                            <asp:BoundField DataField="VisitDate" HeaderText="Appointment Date" />
                            <asp:BoundField DataField="Notes" HeaderText="Notes" />
                        </Columns>

                    </asp:GridView>
                </div>
            </div>

            <div class="button-section">
                <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="Back_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Clear" OnClick="Cancel_Click" />
                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="Save_Click" />
            </div>
        </div>
    </form>
</body>
</html>

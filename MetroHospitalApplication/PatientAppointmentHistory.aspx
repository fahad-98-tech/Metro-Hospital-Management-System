<%@ Page Title="Appointment History" Language="C#" MasterPageFile="~/Patient.Master"
AutoEventWireup="true" CodeBehind="PatientAppointmentHistory.aspx.cs"
Inherits="MetroHospitalApplication.PatientAppointmentHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .page-title {
            font-size: 26px;
            font-weight: 600;
            margin-bottom: 20px;
        }

        .card-box {
            border-radius: 10px;
            padding: 15px;
            margin-bottom: 15px;
            background: #ffffff;
            box-shadow: 0 2px 6px rgba(0,0,0,0.1);
        }

        .card-header {
            font-weight: 600;
            font-size: 18px;
            margin-bottom: 10px;
        }

        .status {
            font-weight: bold;
        }

        .done { color: green; }
        .pending { color: orange; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container mt-4">

    <h3 class="page-title">My Appointment History</h3>

    <!-- APPOINTMENTS -->
    <asp:Repeater ID="rptAppointments" runat="server" OnItemCommand="rptAppointments_ItemCommand">
        <ItemTemplate>
            <div class="card-box">

                <div class="card-header">
                    Dr. <%# Eval("DoctorName") %>
                </div>

                <p><b>Date:</b> <%# Eval("AppointmentDate", "{0:dd-MMM-yyyy}") %></p>
                <p><b>Time:</b> <%# Eval("AppointmentTime") %> - <%# Eval("AppointmentEndTime") %></p>

                <p class="status <%# Eval("Status").ToString().ToLower() %>">
                    Status: <%# Eval("Status") %>
                </p>

                <asp:Button ID="btnManage" runat="server" Text="Manage Booking"
                    CssClass="btn btn-primary btn-sm"
                    CommandName="Manage"
                    CommandArgument='<%# Eval("AppointmentId") %>' />

                <asp:Button ID="btnCancel" runat="server" Text="Cancel Appointment"
                    CssClass="btn btn-danger btn-sm"
                    CommandName="CancelAppt"
                    CommandArgument='<%# Eval("AppointmentId") %>'
                    OnClientClick="return confirm('Cancel this appointment?');" />

            </div>
        </ItemTemplate>
    </asp:Repeater>


    <hr />

    <h3 class="page-title">My Test History</h3>

    <!-- TESTS -->
    <!-- TESTS -->
<asp:Repeater ID="rptTests" runat="server" OnItemCommand="rptTests_ItemCommand">
    <ItemTemplate>
        <div class="card-box">

            <div class="card-header">
                Test Booking
            </div>

            <p><b>Date:</b> <%# Eval("TestDate", "{0:dd-MMM-yyyy}") %></p>
            <p><b>Time:</b> <%# Eval("TestTime") %></p>

            <p class="status <%# Eval("Status").ToString().ToLower() %>">
                Status: <%# Eval("Status") %>
            </p>

            <asp:Button ID="btnManageTest" runat="server"
                Text="Manage Test Booking"
                CssClass="btn btn-primary btn-sm"
                CommandName="ManageTest"
                CommandArgument='<%# Eval("PatientTestId") %>' />

            <asp:Button ID="btnCancelTest" runat="server"
                Text="Cancel Test Booking"
                CssClass="btn btn-danger btn-sm"
                CommandName="CancelTest"
                CommandArgument='<%# Eval("PatientTestId") %>'
                OnClientClick="return confirm('Cancel this test booking?');" />

        </div>
    </ItemTemplate>
</asp:Repeater>

</div>

</asp:Content>
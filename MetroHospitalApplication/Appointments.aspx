<%@ Page Title="My Appointments" Language="C#" MasterPageFile="~/Patient.Master"
    AutoEventWireup="true" CodeBehind="Appointments.aspx.cs" Inherits="MetroHospitalApplication.Appointments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        .btn-edit {
            background-color: #4CAF50;
            color: white;
            border: none;
            padding: 4px 8px;
            border-radius: 4px;
            cursor: pointer;
        }
        .btn-edit:hover {
            background-color: #45a049;
        }
        .btn-delete {
            background-color: #f44336;
            color: white;
            border: none;
            padding: 4px 8px;
            border-radius: 4px;
            cursor: pointer;
        }
        .btn-delete:hover {
            background-color: #d32f2f;
        }
        .table th, .table td {
            vertical-align: middle !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="mb-3">My Appointments</h2>
    <asp:GridView ID="gvAppointments" runat="server" AutoGenerateColumns="False"
        DataKeyNames="AppointmentId"
        CssClass="table table-striped table-bordered"
        OnRowEditing="gvAppointments_RowEditing"
        OnRowDeleting="gvAppointments_RowDeleting">
        <Columns>
            <asp:BoundField DataField="AppointmentId" HeaderText="ID" ReadOnly="True" />
            <asp:BoundField DataField="DoctorId" HeaderText="Doctor ID" />
            <asp:BoundField DataField="PatientName" HeaderText="Patient Name" />
            <asp:BoundField DataField="PatientMobile" HeaderText="Mobile" />
            <asp:BoundField DataField="AppointmentDate" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}" />
            <asp:BoundField DataField="AppointmentTime" HeaderText="Time" />
            <asp:BoundField DataField="AppointmentEndTime" HeaderText="End Time" />
            <asp:BoundField DataField="Status" HeaderText="Status" />
            <asp:BoundField DataField="CreatedAt" HeaderText="Created At" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
            <asp:BoundField DataField="Specialization" HeaderText="Specialization" />
            <asp:BoundField DataField="PatientId" HeaderText="Patient ID" />

            <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>
                    <asp:LinkButton ID="btnEdit" runat="server" Text="Edit" CssClass="btn-edit"
                        CommandName="Edit" />
                    <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" CssClass="btn-delete"
                        CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this appointment?');" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>

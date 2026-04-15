<%@ Page Title="Pending Appointments" Language="C#" MasterPageFile="~/Admin.Master"
    AutoEventWireup="true" CodeBehind="PendingAppointments.aspx.cs"
    Inherits="MetroHospitalApplication.PendingAppointments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<style>

.card{
    background:#fff;
    padding:20px;
    border-radius:8px;
    box-shadow:0 0 10px rgba(0,0,0,0.1);
}

.table th{
    background:#f4f6f9;
}

</style>

</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container mt-4">

    <div class="card">

        <h3 class="mb-4">Appointment List</h3>

        <asp:GridView ID="gvAppointments" runat="server"
            CssClass="table table-bordered table-hover"
            AutoGenerateColumns="False"
            OnRowDataBound="gvAppointments_RowDataBound">

            <Columns>

                <asp:BoundField DataField="AppointmentId" HeaderText="ID" />

                <asp:BoundField DataField="DoctorName" HeaderText="Doctor Name" />

                <asp:BoundField DataField="PatientNameDisplay" HeaderText="Patient Name" />

                <asp:BoundField DataField="PatientMobile" HeaderText="Mobile" />

                <asp:BoundField DataField="Specialization" HeaderText="Specialization" />

                <asp:BoundField DataField="AppointmentDate"
                    HeaderText="Date"
                    DataFormatString="{0:dd-MMM-yyyy}" />

                <asp:BoundField DataField="AppointmentTime" HeaderText="Start Time" />

                <asp:BoundField DataField="AppointmentEndTime" HeaderText="End Time" />

             

                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>

                        <asp:Label ID="lblStatus"
                            runat="server"
                            Text='<%# Eval("Status") %>'
                            CssClass="badge p-2">
                        </asp:Label>

                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>

        </asp:GridView>

    </div>

</div>

</asp:Content>
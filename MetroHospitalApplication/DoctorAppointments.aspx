<%@ Page Title="Doctor Appointments"
    Language="C#"
    MasterPageFile="~/Doctor.Master"
    AutoEventWireup="true"
    CodeBehind="DoctorAppointments.aspx.cs"
    Inherits="MetroHospitalApplication.DoctorAppointments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .status-ddl { width: 140px; }
        .table th { background-color: #f1f3f5; }
        .btn-action { margin-right: 5px; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container-fluid mt-3">

    <div class="row mb-3">
        <div class="col-md-4">
            <label class="fw-bold">Select Date</label>
            <asp:TextBox ID="txtDate" runat="server"
                CssClass="form-control"
                TextMode="Date"
                AutoPostBack="true"
                OnTextChanged="txtDate_TextChanged" />
        </div>
    </div>

    <asp:GridView ID="gvAppointments" runat="server"
        CssClass="table table-bordered table-hover"
        AutoGenerateColumns="False"
        DataKeyNames="AppointmentId"
        OnRowCommand="gvAppointments_RowCommand"
        OnRowDataBound="gvAppointments_RowDataBound"
        EmptyDataText="No appointments found for selected date">

        <Columns>

            <asp:BoundField DataField="PatientName" HeaderText="Patient Name" />
            <asp:BoundField DataField="PatientMobile" HeaderText="Mobile" />
            <asp:BoundField DataField="Specialization" HeaderText="Specialization" />
            <asp:BoundField DataField="AppointmentTime" HeaderText="Start Time" />
            <asp:BoundField DataField="AppointmentEndTime" HeaderText="End Time" />

            <asp:TemplateField HeaderText="Status">
                <ItemTemplate>
                    <asp:DropDownList ID="ddlStatus" runat="server"
                        CssClass="form-select status-ddl">
                        <asp:ListItem Text="Approved" />
                        <asp:ListItem Text="Done" />
                        <asp:ListItem Text="Hold" />
                        <asp:ListItem Text="Cancelled" />
                        <asp:ListItem Text="Rejected" />
                    </asp:DropDownList>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Action">
                <ItemTemplate>
                    <asp:Button ID="btnUpdate" runat="server"
                        Text="Update Status"
                        CssClass="btn btn-sm btn-success btn-action"
                        CommandName="UpdateStatus"
                        CommandArgument='<%# Eval("AppointmentId") %>' />

                    <asp:Button ID="btnTreatment" runat="server"
                        Text="Treatment"
                        CssClass="btn btn-sm btn-primary btn-action"
                        CommandName="Treatment"
                        CommandArgument='<%# Eval("AppointmentId") %>' />

                    <asp:Button ID="btnMedicines" runat="server"
                        Text="Medicines"
                        CssClass="btn btn-sm btn-warning btn-action"
                        CommandName="Medicines"
                        CommandArgument='<%# Eval("AppointmentId") %>' />

                    <asp:Button ID="btnReports" runat="server"
                        Text="Reports"
                        CssClass="btn btn-sm btn-info btn-action"
                        CommandName="Reports"
                        CommandArgument='<%# Eval("AppointmentId") %>' />

                    <asp:Button ID="btnInvoice" runat="server"
                        Text="Invoice"
                        CssClass="btn btn-sm btn-dark btn-action"
                        CommandName="Invoice"
                        CommandArgument='<%# Eval("AppointmentId") %>' />
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>
    </asp:GridView>

    <asp:Label ID="lblMsg" runat="server" CssClass="fw-bold mt-2"></asp:Label>

</div>

</asp:Content>

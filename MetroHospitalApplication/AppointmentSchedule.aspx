<%@ Page Title="Appointment Schedule"
    Language="C#"
    MasterPageFile="~/Admin.Master"
    AutoEventWireup="true"
    CodeBehind="AppointmentSchedule.aspx.cs"
    Inherits="MetroHospitalApplication.AppointmentSchedule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .schedule-table th, .schedule-table td {
            text-align: center;
        }

        .available { background: #28a745; color: #fff; }
        .booked { background: #dc3545; color: #fff; }
        .break { background: #fd7e14; color: #fff; }

        .booking-panel {
            border: 1px solid #ccc;
            padding: 20px;
            border-radius: 8px;
            background: #f8f9fa;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container mt-4">
    <h3>Doctor Appointment Schedule</h3>

    <!-- FILTER -->
    <div class="row mb-3">
        <div class="col-md-4">
            <asp:DropDownList ID="ddlDoctor" runat="server"
                CssClass="form-control"
                AutoPostBack="true"
                OnSelectedIndexChanged="ddlDoctor_SelectedIndexChanged" />
        </div>

        <div class="col-md-4">
            <asp:DropDownList ID="ddlSpecialization" runat="server"
                CssClass="form-control" />
        </div>

        <div class="col-md-4">
            <asp:TextBox ID="txtDate" runat="server"
                CssClass="form-control"
                TextMode="Date"
                AutoPostBack="true"
                OnTextChanged="txtDate_TextChanged" />
        </div>
    </div>

    <asp:Button ID="btnSearch" runat="server"
        Text="Search"
        CssClass="btn btn-primary"
        OnClick="btnSearch_Click" />

    <asp:Button ID="btnFullSchedule" runat="server"
        Text="Full Schedule"
        CssClass="btn btn-secondary"
        OnClick="btnFullSchedule_Click" />

    <!-- GRID -->
    <asp:GridView ID="gvFullSchedule" runat="server"
        CssClass="table table-bordered schedule-table mt-3"
        AutoGenerateColumns="false"
        Visible="false"
        OnRowDataBound="gvFullSchedule_RowDataBound">
        <Columns>
            <asp:BoundField DataField="TimeSlot" HeaderText="Time" />
            <asp:BoundField DataField="Status" HeaderText="Status" />
        </Columns>
    </asp:GridView>

    <!-- BOOKING -->
    <asp:Panel ID="pnlBooking" runat="server" Visible="false" CssClass="booking-panel mt-4">

        <h5>Book Appointment</h5>

        <asp:DropDownList ID="ddlFromTime" runat="server"
            CssClass="form-control mb-2"
            AutoPostBack="true"
            OnSelectedIndexChanged="ddlFromTime_SelectedIndexChanged" />

        <asp:DropDownList ID="ddlToTime" runat="server"
            CssClass="form-control mb-2" />

        <asp:TextBox ID="txtPatientName" runat="server"
            CssClass="form-control mb-2"
            Placeholder="Patient Name" />

        <asp:TextBox ID="txtPatientMobile" runat="server"
            CssClass="form-control mb-2"
            Placeholder="Mobile" />

        <asp:Button ID="btnBook" runat="server"
            Text="Book"
            CssClass="btn btn-success"
            OnClick="btnBook_Click" />
    </asp:Panel>

</div>

</asp:Content>
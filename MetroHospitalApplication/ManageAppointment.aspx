<%@ Page Title="Manage Appointment" Language="C#" MasterPageFile="~/Patient.Master"
    AutoEventWireup="true" CodeBehind="ManageAppointment.aspx.cs"
    Inherits="MetroHospitalApplication.ManageAppointment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container mt-4">
    <h3>Manage Appointment</h3>

    <div class="mb-3">
        <label>Doctor:</label>
        <asp:Label ID="lblDoctor" runat="server" CssClass="fw-bold text-primary"></asp:Label>
    </div>

    <div class="mb-3">
        <label>Select Date:</label>
        <asp:TextBox ID="txtDate"
            runat="server"
            CssClass="form-control"
            TextMode="Date"
            AutoPostBack="true"
            OnTextChanged="txtDate_TextChanged"></asp:TextBox>
    </div>

    <div class="mb-3">
        <label>From Time:</label>
        <asp:DropDownList ID="ddlFromTime"
            runat="server"
            CssClass="form-control"
            AutoPostBack="true"
            OnSelectedIndexChanged="ddlFromTime_SelectedIndexChanged">
        </asp:DropDownList>
    </div>

    <div class="mb-3">
        <label>To Time:</label>
        <asp:DropDownList ID="ddlToTime"
            runat="server"
            CssClass="form-control">
        </asp:DropDownList>
    </div>

    <div class="mb-3">
        <asp:Button ID="btnUpdate"
            runat="server"
            Text="Update Appointment"
            CssClass="btn btn-success"
            OnClick="btnUpdate_Click" />
    </div>
</div>

</asp:Content>
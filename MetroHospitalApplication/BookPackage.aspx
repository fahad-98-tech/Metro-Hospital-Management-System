<%@ Page Title="Book Package" Language="C#" AutoEventWireup="true"
    CodeBehind="BookPackage.aspx.cs"
    Inherits="MetroHospitalApplication.BookPackage"
    MasterPageFile="~/Patient.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<style>

.container-box{
    background:#fff;
    padding:30px;
    border-radius:12px;
    box-shadow:0 5px 20px rgba(0,0,0,0.1);
}

.form-label{
    font-weight:600;
}

.btn-book{
    background:#198754;
    color:#fff;
    padding:10px 25px;
    border-radius:30px;
}

.btn-book:hover{
    background:#157347;
}

</style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container mt-4">

<div class="container-box">

<h3 class="mb-3">Book Health Package</h3>

<asp:Label ID="lblPackageName" runat="server" CssClass="h5 text-primary"></asp:Label>

<hr />

<asp:HiddenField ID="hfPackageId" runat="server"/>
<asp:HiddenField ID="hfDoctorId" runat="server"/>

<div class="mb-3">

<label class="form-label">Doctor</label>

<asp:TextBox
ID="txtDoctor"
runat="server"
CssClass="form-control"
ReadOnly="true">
</asp:TextBox>

</div>

<div class="mb-3">

<label class="form-label">Select Date</label>

<asp:TextBox
ID="txtDate"
runat="server"
CssClass="form-control"
TextMode="Date"
AutoPostBack="true"
OnTextChanged="txtDate_TextChanged">
</asp:TextBox>

</div>

<div class="row mb-3">

<div class="col-md-6">

<label class="form-label">From Time</label>

<asp:DropDownList
ID="ddlFromTime"
runat="server"
CssClass="form-select"
AutoPostBack="true"
OnSelectedIndexChanged="ddlFromTime_SelectedIndexChanged">
</asp:DropDownList>

</div>

<div class="col-md-6">

<label class="form-label">To Time</label>

<asp:DropDownList
ID="ddlToTime"
runat="server"
CssClass="form-select">
</asp:DropDownList>

</div>

</div>

<div class="mb-3">

<label class="form-label">Patient Name</label>

<asp:TextBox
ID="txtPatientName"
runat="server"
CssClass="form-control">
</asp:TextBox>

</div>

<div class="mb-3">

<label class="form-label">Patient Mobile</label>

<asp:TextBox
ID="txtPatientMobile"
runat="server"
CssClass="form-control">
</asp:TextBox>

</div>

<asp:Button
ID="btnBook"
runat="server"
Text="Book Appointment"
CssClass="btn btn-book"
OnClick="btnBook_Click"/>

</div>

</div>

</asp:Content>
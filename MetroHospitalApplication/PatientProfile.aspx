<%@ Page Title="My Profile" Language="C#" MasterPageFile="~/Patient.Master" AutoEventWireup="true" CodeBehind="PatientProfile.aspx.cs" Inherits="MetroHospitalApplication.PatientProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container mt-4">

<h3 class="mb-4">My Profile</h3>

<asp:Label ID="lblMsg" runat="server" CssClass="text-success"></asp:Label>

<div class="card p-4 shadow">

<div class="row mb-3">
<div class="col-md-6">
<label>Full Name</label>
<asp:TextBox ID="txtFullName" runat="server" CssClass="form-control"></asp:TextBox>
</div>

<div class="col-md-6">
<label>Email</label>
<asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
</div>
</div>

<div class="row mb-3">
<div class="col-md-6">
<label>Mobile Number</label>
<asp:TextBox ID="txtMobile" runat="server" CssClass="form-control"></asp:TextBox>
</div>

<div class="col-md-6">
<label>Gender</label>
<asp:DropDownList ID="ddlGender" runat="server" CssClass="form-select">
<asp:ListItem Text="Male" Value="Male"/>
<asp:ListItem Text="Female" Value="Female"/>
</asp:DropDownList>
</div>
</div>

<div class="row mb-3">
<div class="col-md-6">
<label>Date Of Birth</label>
<asp:TextBox ID="txtDOB" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
</div>

<div class="col-md-6">
<label>Role</label>
<asp:TextBox ID="txtRole" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
</div>
</div>

<div class="mt-3">
<asp:Button ID="btnUpdate" runat="server" Text="Update Profile"
CssClass="btn btn-primary"
OnClick="btnUpdate_Click" />
</div>

</div>
</div>

</asp:Content>
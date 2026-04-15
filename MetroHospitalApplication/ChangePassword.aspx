<%@ Page Title="Change Password" Language="C#" MasterPageFile="~/Patient.Master"
    AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs"
    Inherits="MetroHospitalApplication.ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5" style="max-width: 500px;">
        <h3 class="mb-4">Change Password</h3>
        <asp:Label ID="lblMessage" runat="server" CssClass="mb-3 d-block"></asp:Label>
        
        <div class="mb-3">
            <label for="txtNewPassword" class="form-label">New Password</label>
            <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
        </div>

        <div class="mb-3">
            <label for="txtConfirmPassword" class="form-label">Confirm New Password</label>
            <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
        </div>

        <asp:Button ID="btnChangePassword" runat="server" Text="Change Password" CssClass="btn btn-primary w-100" OnClick="btnChangePassword_Click" />
    </div>
</asp:Content>
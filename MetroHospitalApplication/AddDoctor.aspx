<%@ Page Title="Add Doctor" Language="C#" MasterPageFile="~/Admin.Master"
    AutoEventWireup="true" CodeBehind="AddDoctor.aspx.cs"
    Inherits="MetroHospitalApplication.AddDoctor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="container-fluid mt-4">
    <h3 class="mb-4 text-primary">Add Doctor</h3>

    <asp:Label ID="lblMessage" runat="server" CssClass="text-danger fw-semibold"></asp:Label>

    <div class="card shadow p-4">
        <div class="row g-3">

            <div class="col-md-6">
                <label>Full Name</label>
                <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" />
            </div>

            <div class="col-md-6">
                <label>Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
            </div>

            <div class="col-md-6">
                <label>Mobile Number</label>
                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" />
            </div>

            <div class="col-md-6">
                <label>Gender</label>
                <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-select">
                    <asp:ListItem Text="Select Gender" Value="" Disabled="True" Selected="True" />
                    <asp:ListItem Text="Male" />
                    <asp:ListItem Text="Female" />
                    <asp:ListItem Text="Other" />
                </asp:DropDownList>
            </div>

            <div class="col-md-6">
                <label>Date of Birth</label>
                <asp:TextBox ID="txtDOB" runat="server" CssClass="form-control" TextMode="Date"
                    AutoPostBack="true" OnTextChanged="txtDOB_TextChanged" />
            </div>

            <div class="col-md-6">
                <label>Age</label>
                <asp:TextBox ID="txtAge" runat="server" CssClass="form-control" ReadOnly="true" />
            </div>

            <div class="col-md-6">
                <label>Password</label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
            </div>

            <div class="col-md-6">
                <label>Specialization</label>
                <asp:DropDownList ID="ddlSpecialization" runat="server" CssClass="form-select" />
            </div>

            <div class="col-md-6">
                <label>Doctor Image</label>
                <asp:FileUpload ID="fuDoctorImage" runat="server" CssClass="form-control" />
            </div>

            <div class="col-12 mt-3">
                <asp:Button ID="btnSaveDoctor" runat="server"
                    CssClass="btn btn-primary"
                    Text="Save Doctor"
                    OnClick="btnSaveDoctor_Click" />
            </div>

        </div>
    </div>
</div>
</asp:Content>

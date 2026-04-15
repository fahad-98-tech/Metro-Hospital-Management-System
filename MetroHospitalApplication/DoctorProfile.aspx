<%@ Page Title="Doctor Profile" Language="C#" MasterPageFile="~/Doctor.Master"
    AutoEventWireup="true" CodeBehind="DoctorProfile.aspx.cs"
    Inherits="MetroHospitalApplication.DoctorProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />

    <style>
        .profile-card {
            border-radius: 15px;
            box-shadow: 0 10px 25px rgba(0,0,0,0.1);
            padding: 30px;
            background: #fff;
        }
        .profile-img {
            width: 140px;
            height: 140px;
            object-fit: cover;
            border-radius: 50%;
            border: 4px solid #198754;
        }
        select[multiple] {
            height: 180px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4 mb-5">
        <div class="row justify-content-center">
            <div class="col-lg-9">
                <div class="profile-card">

                    <!-- PROFILE IMAGE + SPECIALIZATION DISPLAY -->
                    <div class="text-center mb-4">
                        <asp:Image ID="imgDoctor" runat="server" CssClass="profile-img" />

                        <div class="mt-2">
                            <asp:FileUpload ID="fuDoctorImage" runat="server"
                                CssClass="form-control form-control-sm" />
                        </div>

                        <h4 class="mt-3">Doctor Profile</h4>

                        <!-- SPECIALIZATION BADGES -->
                        <div class="mt-2">
                            <asp:Repeater ID="rptSpecialization" runat="server">
                                <ItemTemplate>
                                    <span class="badge bg-success me-1 mb-1">
                                        <%# Container.DataItem %>
                                    </span>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>

                    <div class="row g-3">

                        <div class="col-md-6">
                            <label class="form-label">Full Name</label>
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
                        </div>

                        <div class="col-md-6">
                            <label class="form-label">Email</label>
                            <asp:TextBox ID="txtEmail" runat="server"
                                CssClass="form-control" ReadOnly="true" />
                        </div>

                        <div class="col-md-6">
                            <label class="form-label">Mobile</label>
                            <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" />
                        </div>

                        <div class="col-md-6">
                            <label class="form-label">Gender</label>
                            <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-select">
                                <asp:ListItem Text="Select Gender" Value="" />
                                <asp:ListItem>Male</asp:ListItem>
                                <asp:ListItem>Female</asp:ListItem>
                                <asp:ListItem>Other</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="col-md-6">
                            <label class="form-label">Age</label>
                            <asp:TextBox ID="txtAge" runat="server" CssClass="form-control" />
                        </div>

                        <div class="col-md-6">
                            <label class="form-label">Date of Birth</label>
                            <asp:TextBox ID="txtDOB" runat="server"
                                TextMode="Date" CssClass="form-control" />
                        </div>

                        <div class="col-12">
                            <label class="form-label">
                                Specializations
                                <small class="text-muted">(Ctrl + Click for multiple)</small>
                            </label>
                            <asp:ListBox ID="ddlSpecialization" runat="server"
                                CssClass="form-select"
                                SelectionMode="Multiple">
                            </asp:ListBox>
                        </div>

                        <div class="col-12 text-end mt-4">
                            <asp:Button ID="btnUpdate" runat="server"
                                Text="Update Profile"
                                CssClass="btn btn-success px-4"
                                OnClick="btnUpdate_Click" />
                        </div>

                        <div class="col-12">
                            <asp:Label ID="lblMsg" runat="server"></asp:Label>
                        </div>

                    </div>

                </div>
            </div>
        </div>
    </div>
</asp:Content>

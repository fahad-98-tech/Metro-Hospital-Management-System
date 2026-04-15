<%@ Page Title="Doctor Profile" Language="C#" MasterPageFile="~/Patient.Master"
    AutoEventWireup="true" CodeBehind="DoctorProfileView.aspx.cs"
    Inherits="MetroHospitalApplication.DoctorProfileView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />

    <style>
        .profile-card {
            border-radius: 20px;
            box-shadow: 0 12px 30px rgba(0,0,0,0.08);
            padding: 40px;
            background: #fff;
        }

        .profile-img {
            width: 160px;
            height: 160px;
            object-fit: cover;
            border-radius: 50%;
            border: 5px solid #0d6efd;
        }

        .info-label {
            font-weight: 600;
            color: #0D1B2A;
        }

        .badge-spec {
            background-color: #0d6efd;
            margin: 4px;
            padding: 6px 12px;
            border-radius: 20px;
            font-size: 0.8rem;
        }
    </style>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container mt-5 mb-5">
        <div class="row justify-content-center">
            <div class="col-lg-8">

                <div class="profile-card">

                    <div class="text-center mb-4">
                        <asp:Image ID="imgDoctor" runat="server" CssClass="profile-img" />
                        <h3 class="mt-3">
                            <asp:Label ID="lblName" runat="server" />
                        </h3>
                        <div id="divSpecialization" runat="server"></div>
                    </div>

                    <div class="row g-3">

                        <div class="col-md-6">
                            <span class="info-label">E-mail:</span><br />
                            <asp:Label ID="lblEmail" runat="server" />
                        </div>

                        <div class="col-md-6">
                            <span class="info-label">Contact:</span><br />
                            <asp:Label ID="lblMobile" runat="server" />
                        </div>

                        <div class="col-md-6">
                            <span class="info-label">Gender:</span><br />
                            <asp:Label ID="lblGender" runat="server" />
                        </div>

                        <div class="col-md-6">
                            <span class="info-label">Age:</span><br />
                            <asp:Label ID="lblAge" runat="server" />
                        </div>

                        <div class="col-md-6">
                            <span class="info-label">Date of Birth:</span><br />
                            <asp:Label ID="lblDOB" runat="server" />
                        </div>

                        <div class="col-md-6">
                            <span class="info-label">Joined:</span><br />
                            <asp:Label ID="lblCreatedAt" runat="server" />
                        </div>

                    </div>

                </div>

            </div>
        </div>
    </div>

</asp:Content>

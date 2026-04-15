<%@ Page Title="Our Doctors" Language="C#" MasterPageFile="~/Patient.Master" 
    AutoEventWireup="true" CodeBehind="Doctors.aspx.cs" 
    Inherits="MetroHospitalApplication.Doctors" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .page-title {
            text-align: center;
            margin: 40px 0;
        }
        .page-title h1 {
            font-weight: 700;
            color: #0D1B2A;
        }
        .page-title p {
            color: #555;
            font-size: 1rem;
        }

        .doctor-card {
            background: #fff;
            border-radius: 18px;
            padding: 25px 20px;
            text-align: center;
            box-shadow: 0 8px 25px rgba(0,0,0,0.08);
            transition: all 0.3s ease;
            height: 100%;
        }
        .doctor-card:hover {
            transform: translateY(-8px);
            box-shadow: 0 15px 35px rgba(0,0,0,0.15);
        }

        .doctor-card img {
            width: 130px;
            height: 130px;
            border-radius: 50%;
            object-fit: cover;
            margin-bottom: 15px;
            border: 4px solid #00A3FF;
        }

        .doctor-card h5 {
            font-weight: 600;
            margin-bottom: 5px;
            color: #0D1B2A;
        }

        .doctor-card .specialization {
            color: #00A3FF;
            font-weight: 500;
            margin-bottom: 10px;
        }

        .doctor-info {
            font-size: 0.9rem;
            color: #555;
            margin-bottom: 10px;
        }

        .doctor-desc {
            font-size: 0.9rem;
            color: #666;
            margin-bottom: 15px;
        }

        .doctor-card .btn {
            border-radius: 50px;
            padding: 8px 22px;
            font-size: 0.9rem;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="page-title">
        <h1>Meet Our Doctors</h1>
        <p>Experienced specialists dedicated to your health and well-being</p>
    </div>

    <div class="container mb-5">
        <div class="row g-4">

            <asp:Repeater ID="rptDoctors" runat="server">
                <ItemTemplate>
                    <div class="col-lg-3 col-md-6">
                        <div class="doctor-card">

                            <img src='<%# GetDoctorImage(Eval("DoctorImage")) %>' alt="Doctor" />

                            <h5><%# Eval("FullName") %></h5>

                            <div class="specialization">
                                <%# Eval("Specialization") %>
                            </div>

                            <div class="doctor-info">
                                Age: <%# Eval("Age") %> |
                                Joined: <%# Convert.ToDateTime(Eval("CreatedDate")).ToString("yyyy") %>
                            </div>

                            <p class="doctor-desc">
                                <strong>E-mail:</strong> <%# Eval("Email") %><br />
                                <strong>Contact:</strong> <%# Eval("MobileNumber") %>
                            </p>

                            <a href='DoctorProfileView.aspx?id=<%# Eval("DoctorId") %>' 
                               class="btn btn-primary">
                                View Profile
                            </a>

                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

        </div>
    </div>

</asp:Content>

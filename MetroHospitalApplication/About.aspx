<%@ Page Title="About Us" Language="C#" MasterPageFile="~/Patient.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="MetroHospitalApplication.About" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* Hero Section */
        .about-hero {
            background: url('https://images.unsplash.com/photo-1588776814546-3b7b2617b9f1?auto=format&fit=crop&w=1600&q=90') center/cover no-repeat;
            color: #fff;
            text-align: center;
            padding: 120px 20px;
            border-radius: 15px;
            margin-top: 20px;
            position: relative;
        }
        .about-hero::after {
            content: '';
            position: absolute;
            top:0; left:0; width:100%; height:100%;
            background: rgba(0,0,0,0.4);
            border-radius: 15px;
            z-index:1;
        }
        .about-hero h1, .about-hero p {
            position: relative;
            z-index:2;
        }
        .about-hero h1 { font-size: 3.5rem; font-weight: 700; margin-bottom: 15px; }
        .about-hero p { font-size: 1.3rem; margin-bottom: 25px; }

        /* About Info Section */
        .about-info {
            padding: 80px 20px;
            background-color: #f1f5f9;
        }
        .about-info h2 { font-weight: 700; margin-bottom: 20px; color: #0D1B2A; }
        .about-info p { font-size: 1rem; color: #555; line-height: 1.8; }

        /* Highlight Cards */
        .highlight-card {
            border-radius: 15px;
            box-shadow: 0 5px 20px rgba(0,0,0,0.08);
            padding: 35px 25px;
            text-align: center;
            transition: all 0.3s ease;
            background-color: #fff;
        }
        .highlight-card:hover {
            transform: translateY(-8px);
            box-shadow: 0 20px 40px rgba(0,0,0,0.15);
        }
        .highlight-card i {
            font-size: 3.5rem;
            margin-bottom: 15px;
            color: #007bff;
        }
        .highlight-card h5 { font-weight: 600; margin-bottom: 12px; font-size: 1.15rem; }
        .highlight-card p { font-size: 0.95rem; color: #555; }

        /* Responsive Images */
        .about-info img {
            width: 100%;
            border-radius: 15px;
            object-fit: cover;
        }

        @media (max-width: 768px) {
            .about-hero h1 { font-size: 2.5rem; }
            .about-hero p { font-size: 1.1rem; }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Hero Section -->
    <div class="about-hero">
        <h1>About Metro Medical Group</h1>
        <p>Providing exceptional healthcare services with compassion, expertise, and innovation.</p>
    </div>

    <!-- About Info Section -->
    <div class="about-info">
        <div class="container">
            <div class="row align-items-center mb-5">
                <div class="col-md-6">
                    <h2>Our Mission</h2>
                    <p>At Metro Medical Group, our mission is to deliver patient-centered care with the highest standards of medical excellence. We combine state-of-the-art technology with compassionate care to foster a healthier community.</p>
                </div>
                <div class="col-md-6">
                   <img src="Images/images (1).jfif" class="img-fluid rounded" alt="Our Mission">
                    </div>
            </div>

            <div class="row align-items-center mb-5 flex-md-row-reverse">
                <div class="col-md-6">
                    <h2>Our Vision</h2>
                    <p>To be a leading healthcare provider recognized for innovation, patient satisfaction, and excellence in every medical service we offer.</p>
                </div>
                <div class="col-md-6">
                    <img src="Images/images (2).jfif" alt="Our Vision">
                </div>
            </div>

            <!-- Highlight Cards -->
            <div class="row g-4 text-center">
                <div class="col-md-3">
                    <div class="highlight-card">
                        <i class="bi bi-hospital"></i>
                        <h5>Advanced Facilities</h5>
                        <p>Equipped with modern technology and specialized medical departments.</p>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="highlight-card">
                        <i class="bi bi-people-fill"></i>
                        <h5>Expert Doctors</h5>
                        <p>Highly trained and compassionate medical professionals for every specialty.</p>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="highlight-card">
                        <i class="bi bi-clock-history"></i>
                        <h5>24/7 Services</h5>
                        <p>Emergency care, pharmacy, and support available round the clock.</p>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="highlight-card">
                        <i class="bi bi-award"></i>
                        <h5>Quality Care</h5>
                        <p>Committed to excellence and patient satisfaction in every service.</p>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<%@ Page Title="Hospital Services" Language="C#" MasterPageFile="~/Patient.Master" AutoEventWireup="true" CodeBehind="HospitalServices.aspx.cs" Inherits="MetroHospitalApplication.HospitalServices" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .service-card {
            transition: all 0.3s ease;
            border: none;
            box-shadow: 0 4px 6px rgba(0,0,0,0.1);
            border-radius: 12px;
            height: 100%;
            text-align: center;
            padding: 25px;
        }
        .service-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 8px 25px rgba(0,0,0,0.15);
        }
        .service-icon {
            width: 80px; height: 80px;
            border-radius: 50%;
            display: flex; align-items: center; justify-content: center;
            font-size: 2rem;
            margin: 0 auto 1rem;
        }
        .service-card h4 { font-weight: 700; margin-bottom: 1rem; }
        .service-card p { color: #555; font-size: 0.95rem; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row mb-5 text-center">
        <div class="col-12">
            <h1 class="display-4 fw-bold text-primary mb-3">Our Hospital Services</h1>
            <p class="lead text-muted mb-0">Comprehensive healthcare solutions with state-of-the-art facilities and expert medical professionals</p>
        </div>
    </div>

    <div class="row g-4 mb-5">
        <div class="col-lg-4 col-md-6"><div class="card service-card"><div class="service-icon bg-danger bg-opacity-10 text-danger"><i class="bi bi-exclamation-triangle-fill"></i></div><h4>24/7 Emergency Care</h4><p>Immediate medical attention for critical conditions.</p></div></div>
        <div class="col-lg-4 col-md-6"><div class="card service-card"><div class="service-icon bg-info bg-opacity-10 text-info"><i class="bi bi-heart-fill"></i></div><h4>Cardiology</h4><p>Expert heart care including diagnostics & treatments.</p></div></div>
        <div class="col-lg-4 col-md-6"><div class="card service-card"><div class="service-icon bg-warning bg-opacity-10 text-warning"><i class="bi bi-activity"></i></div><h4>Orthopedics</h4><p>Advanced orthopedic treatments and surgeries.</p></div></div>
        <div class="col-lg-4 col-md-6"><div class="card service-card"><div class="service-icon bg-success bg-opacity-10 text-success"><i class="bi bi-capsule"></i></div><h4>General Medicine</h4><p>Primary care services and chronic disease management.</p></div></div>
        <div class="col-lg-4 col-md-6"><div class="card service-card"><div class="service-icon bg-primary bg-opacity-10 text-primary"><i class="bi bi-baby"></i></div><h4>Pediatrics</h4><p>Specialized care for infants, children, and adolescents.</p></div></div>
        <div class="col-lg-4 col-md-6"><div class="card service-card"><div class="service-icon bg-purple bg-opacity-10 text-purple"><i class="bi bi-gender-ambiguous"></i></div><h4>Gynecology</h4><p>Women's health services including prenatal care.</p></div></div>
        <div class="col-lg-4 col-md-6"><div class="card service-card"><div class="service-icon bg-secondary bg-opacity-10 text-secondary"><i class="bi bi-scanner"></i></div><h4>Diagnostics</h4><p>Laboratory and imaging services including MRI, X-ray.</p></div></div>
        <div class="col-lg-4 col-md-6"><div class="card service-card"><div class="service-icon bg-dark bg-opacity-10 text-dark"><i class="bi bi-heart-pulse-fill"></i></div><h4>ICU</h4><p>Critical care with 24/7 monitoring by specialists.</p></div></div>
        <div class="col-lg-4 col-md-6"><div class="card service-card"><div class="service-icon bg-light border border-secondary-subtle text-secondary"><i class="bi bi-capsules"></i></div><h4>Pharmacy</h4><p>24-hour pharmacy services with professional counseling.</p></div></div>
    </div>
</asp:Content>

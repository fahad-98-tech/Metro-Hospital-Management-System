<%@ Page Title="Patient Home" Language="C#" MasterPageFile="~/Patient.Master" AutoEventWireup="true" CodeBehind="PatientHome.aspx.cs" Inherits="MetroHospitalApplication.PatientHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Google Fonts -->
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@400;600;700&display=swap" rel="stylesheet">
    <!-- Bootstrap Icons -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.1/font/bootstrap-icons.css" rel="stylesheet">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
    <style>
        body { font-family: 'Poppins', sans-serif; background-color: #f8f9ff; }

        /* ---------- Hero Section ---------- */
        .hero {
            position: relative;
            background: url('https://images.unsplash.com/photo-1588776814546-3b7b2617b9f1?auto=format&fit=crop&w=1600&q=80') center/cover no-repeat;
            color: #fff;
            text-align: center;
            padding: 140px 20px 100px 20px;
            border-radius: 15px;
            margin-top: 20px;
            overflow: hidden;
        }
        .hero::after {
            content: "";
            position: absolute;
            inset: 0;
            background: rgba(0,0,0,0.5);
            border-radius: 15px;
        }
        .hero h1, .hero p, .hero .btn-primary {
            position: relative;
            z-index: 1;
        }
        .hero h1 { font-size: 3rem; font-weight: 700; margin-bottom: 15px; }
        .hero p { font-size: 1.25rem; margin-bottom: 25px; }
        .hero .btn-primary {
            border-radius: 50px; padding: 14px 35px; font-size: 1.15rem;
            transition: all 0.3s ease;
        }
        .hero .btn-primary:hover { transform: translateY(-3px); box-shadow: 0 10px 25px rgba(0,0,0,0.2); }

        /* ---------- Feature Cards ---------- */
        .feature-section { background-color: #ffffff; padding: 60px 0; }
        .feature-card {
            border-radius: 20px;
            background: linear-gradient(145deg, #ffffff, #f8f9ff);
            text-align: center;
            padding: 35px 25px;
            transition: all 0.3s ease;
            box-shadow: 0 8px 30px rgba(0,0,0,0.05);
        }
        .feature-card:hover i { transform: scale(1.2); color: #00A3FF; }
        .feature-card i { font-size: 4rem; margin-bottom: 15px; color: #007BFF; transition: all 0.3s ease; }
        .feature-card h5 { font-weight: 600; margin-bottom: 12px; }
        .feature-card p { font-size: 0.95rem; color: #555; }

        /* ---------- Appointment & Booking Forms ---------- */
        .form-floating > label { font-size: 0.95rem; color: #555; }
        .form-control, .form-select {
            height: 56px;
            border-radius: 12px;
            font-size: 0.95rem;
            padding: 0 1rem;
            box-shadow: 0 2px 6px rgba(0,0,0,0.05);
            transition: all 0.2s ease-in-out;
        }
        .form-control:focus, .form-select:focus {
            box-shadow: 0 0 0 0.2rem rgba(0,123,255,0.25);
        }
        .btn-primary, .btn-success, .btn-warning {
            border-radius: 50px; font-weight: 600; padding: 12px 28px; transition: all 0.3s ease;
        }
        .btn-primary:hover { background-color: #007BFF; }
        .btn-success:hover { background-color: #28a745; }
        .btn-warning:hover { background-color: #ffc107; }

        /* ---------- Doctor Cards ---------- */
        .doctor-card {
            background: #fff;
            border-radius: 15px;
            text-align: center;
            padding: 25px 20px;
            color: #0D1B2A;
            transition: all 0.3s ease;
            box-shadow: 0 5px 20px rgba(0,0,0,0.1);
        }
        .doctor-card:hover { transform: translateY(-5px); box-shadow: 0 15px 35px rgba(0,0,0,0.2); }
        .doctor-card img {
            width: 120px; height: 120px; border-radius: 50%; margin-bottom: 15px; object-fit: cover; border: 3px solid #00A3FF;
        }
        .doctor-card h5 { font-weight: 600; margin-bottom: 5px; }
        .doctor-card p { font-size: 0.9rem; margin-bottom: 10px; color: #555; }

        /* ---------- Schedule Grid ---------- */
        .schedule-grid { display: flex; flex-wrap: wrap; gap: 15px; justify-content: center; margin-top: 20px; }
        .slot-card {
            flex: 0 1 120px;
            border-radius: 12px;
            padding: 15px;
            font-size: 0.85rem;
            font-weight: 600;
            text-align: center;
            color: #fff;
            cursor: pointer;
            transition: all 0.2s ease-in-out;
        }
        .slot-card:hover { transform: scale(1.05); }
        .slot-available { background-color: #28a745; }
        .slot-booked { background-color: #dc3545; }
        .slot-break { background-color: #fd7e14; }

        /* ---------- Modal ---------- */
        .modal-content { border-radius: 15px; padding: 20px; }
        .modal-title { font-weight: 600; }
        .modal-footer .btn { border-radius: 50px; font-weight: 600; padding: 10px 25px; }

        /* Responsive adjustments */
        @media (max-width: 768px) {
            .hero h1 { font-size: 2rem; }
            .hero p { font-size: 1rem; }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- ---------- Hero Section ---------- -->
    <div class="hero">
        <h1>Explore and Access Reliable Health Services Easily</h1>
        <p>Your trusted health companion. Book appointments, consult doctors, and access services with ease.</p>
       
    </div>

    <!-- ---------- Feature Section ---------- -->
    <div class="feature-section">
        <div class="container">
            <div class="row g-4 justify-content-center text-center">
                <div class="col-md-3">
                    <div class="feature-card"><i class="bi bi-hospital"></i><h5>Hospital Services</h5><p>Inpatient & Outpatient Care</p></div>
                </div>
                <div class="col-md-3">
                    <div class="feature-card"><i class="bi bi-person-badge"></i><h5>Find a Doctor</h5><p>Schedule Consultations Easily</p></div>
                </div>
                <div class="col-md-3">
                    <div class="feature-card"><i class="bi bi-heart-pulse"></i><h5>Health Packages</h5><p>Preventive Checkups</p></div>
                </div>
                <div class="col-md-3">
                    <div class="feature-card"><i class="bi bi-headset"></i><h5>Support Center</h5><p>24/7 Assistance</p></div>
                </div>
            </div>
        </div>
    </div>

    <!-- ---------- Appointment Section ---------- -->
    <div class="container appointment-section mt-5 p-5 rounded-4" style="background-color:#0D1B2A; color:white;">
        <h3 class="mb-3">Check an Appointment</h3>
        <p class="mb-4">Find the right doctor and book your appointment easily.</p>

        <div class="row g-3 justify-content-center mb-5">
            <div class="col-md-3">
                <div class="form-floating">
                    <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged"></asp:DropDownList>
                    <label for="ddlDepartment">Select Department</label>
                </div>
            </div>
            <div class="col-md-3">
                <div class="form-floating">
                    <asp:DropDownList ID="ddlDoctor" runat="server" CssClass="form-select"></asp:DropDownList>
                    <label for="ddlDoctor">Select Doctor</label>
                </div>
            </div>
            <div class="col-md-3">
                <div class="form-floating position-relative">
                    <asp:TextBox ID="txtAppointmentDate" runat="server" CssClass="form-control pe-5" TextMode="Date"></asp:TextBox>
                    <label for="txtAppointmentDate">Select Date</label>
                    <i class="bi bi-calendar-date position-absolute top-50 end-0 translate-middle-y me-3 text-muted"></i>
                </div>
            </div>
            <div class="col-md-2 d-grid">
                <asp:Button ID="btnSearchAppointment" runat="server" CssClass="btn btn-primary btn-lg" Text="Search" OnClick="btnSearchAppointment_Click" />
            </div>
        </div>

        <!-- Doctor Cards -->
        <div class="row g-4" id="divDoctorResults"></div>

        <!-- Schedule Grid -->
        <asp:Panel ID="divSchedule" runat="server" CssClass="schedule-grid"></asp:Panel>
    </div>

    <!-- ---------- Book Appointment Section ---------- -->
    <div class="container mt-5">
        <div class="card p-4 shadow-sm rounded-4">
            <h5 class="mb-3 text-primary">Book Appointment</h5>
            <div class="row g-3">
                <div class="col-md-3">
                    <label for="ddlFromTime" class="form-label">From</label>
                    <asp:DropDownList ID="ddlFromTime" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlFromTime_SelectedIndexChanged"></asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <label for="ddlToTime" class="form-label">To</label>
                    <asp:DropDownList ID="ddlToTime" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <label for="txtPatientName" class="form-label">Patient Name</label>
                    <asp:TextBox ID="txtPatientName" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-md-3">
                    <label for="txtPatientMobile" class="form-label">Mobile</label>
                    <asp:TextBox ID="txtPatientMobile" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-12 text-end mt-2">
                    <asp:Button ID="btnBook" runat="server" CssClass="btn btn-success" Text="Book Appointment" OnClick="btnBook_Click" />
                </div>
            </div>
        </div>
    </div>

    <!-- ---------- Test Booking Section ---------- -->
    <div class="container mt-5">
        <div class="card p-4 shadow-sm rounded-4">
            <h5 class="mb-3 text-warning">Book Lab Test / X-Ray (Morning Only)</h5>
            <div class="row g-3">
                <div class="col-md-3"><asp:DropDownList ID="ddlTest" runat="server" CssClass="form-select"></asp:DropDownList></div>
                <div class="col-md-3"><asp:TextBox ID="txtTestDate" runat="server" CssClass="form-control" TextMode="Date" AutoPostBack="true" OnTextChanged="txtTestDate_TextChanged"></asp:TextBox></div>
                <div class="col-md-3"><asp:DropDownList ID="ddlTestTime" runat="server" CssClass="form-select"></asp:DropDownList></div>
                <div class="col-md-3 d-grid"><asp:Button ID="btnBookTest" runat="server" Text="Book Test" CssClass="btn btn-warning w-100" OnClick="btnBookTest_Click" /></div>
            </div>
        </div>
    </div>

    <!-- ---------- Modal ---------- -->
    <div class="modal fade" id="testModal" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Confirm Booking</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">
                    <asp:TextBox ID="txtTestPatientName" runat="server" CssClass="form-control mb-2" placeholder="Patient Name"></asp:TextBox>
                    <asp:TextBox ID="txtTestContact" runat="server" CssClass="form-control" placeholder="Contact"></asp:TextBox>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnConfirmTest" runat="server" Text="Confirm" CssClass="btn btn-success" OnClick="btnConfirmTest_Click" />
                </div>
            </div>
        </div>
    </div>

    <script>
        function openTestModal() {
            var modal = new bootstrap.Modal(document.getElementById('testModal'));
            modal.show();
        }
    </script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</asp:Content>
<%@ Page Title="Admin Dashboard" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="AdminHome.aspx.cs" Inherits="MetroHospitalApplication.AdminHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; }

        .card { border-radius: 12px; box-shadow: 0 4px 12px rgba(0,0,0,0.08); transition: transform 0.2s, box-shadow 0.2s; }
        .card:hover { transform: translateY(-5px); box-shadow: 0 8px 20px rgba(0,0,0,0.12); }
        .card-title { font-weight: 600; font-size: 1.1rem; }
        h3 { font-weight: 700; margin-top: 0.5rem; }
        .card-body { color: white; }
        .chart-card { border-radius: 12px; box-shadow: 0 4px 12px rgba(0,0,0,0.08); padding: 1.5rem !important; background-color: #fff; }
        .chart-card h5 { font-weight: 600; margin-bottom: 1rem; color: #333; }
        canvas { width: 100% !important; height: 300px !important; }

        /* KPI Colors */
        .bg-primary { background-color: #4e73df !important; }
        .bg-success { background-color: #1cc88a !important; }
        .bg-warning { background-color: #f6c23e !important; }
        .bg-danger { background-color: #e74a3b !important; }
        .bg-info { background-color: #36b9cc !important; }
        .bg-purple { background-color: #6f42c1 !important; }
        .bg-orange { background-color: #fd7e14 !important; }
        .bg-teal { background-color: #20c997 !important; }
        .bg-indigo { background-color: #6610f2 !important; }
        .bg-pink { background-color: #e83e8c !important; }

        .row.g-4 > [class*="col-"] { display: flex; }
        .row.g-4 > [class*="col-"] .card { flex: 1; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="container-fluid py-4">

    <!-- ===== KPI Cards ===== -->
    <div class="row mb-4 g-4">
        <div class="col-md-2 col-sm-6"><div class="card bg-primary text-center p-3"><h5 class="card-title">Total Patients</h5><h3><asp:Label ID="lblTotalPatients" runat="server" Text="0"></asp:Label></h3></div></div>
        <div class="col-md-2 col-sm-6"><div class="card bg-success text-center p-3"><h5 class="card-title">New Patients Today</h5><h3><asp:Label ID="lblTodayPatients" runat="server" Text="0"></asp:Label></h3></div></div>
        <div class="col-md-2 col-sm-6"><div class="card bg-warning text-center p-3"><h5 class="card-title">Appointments Upcoming</h5><h3><asp:Label ID="lblUpcomingAppointments" runat="server" Text="0"></asp:Label></h3></div></div>
        <div class="col-md-2 col-sm-6"><div class="card bg-purple text-center p-3"><h5 class="card-title">Total Doctors</h5><h3><asp:Label ID="lblTotalDoctors" runat="server" Text="0"></asp:Label></h3></div></div>
        <div class="col-md-2 col-sm-6"><div class="card bg-orange text-center p-3"><h5 class="card-title">Doctors On Duty</h5><h3><asp:Label ID="Label1" runat="server" Text="0"></asp:Label></h3></div></div>
        <div class="col-md-2 col-sm-6"><div class="card bg-teal text-center p-3"><h5 class="card-title">Appointments Completed</h5><h3><asp:Label ID="lblCompletedAppointments" runat="server" Text="0"></asp:Label></h3></div></div>
        <div class="col-md-2 col-sm-6"><div class="card bg-indigo text-center p-3"><h5 class="card-title">Pending Appointments</h5><h3><asp:Label ID="lblPendingAppointments" runat="server" Text="0"></asp:Label></h3></div></div>
        <div class="col-md-2 col-sm-6"><div class="card bg-pink text-center p-3"><h5 class="card-title">Discharged Patients</h5><h3><asp:Label ID="lblDischargedPatients" runat="server" Text="0"></asp:Label></h3></div></div>
    </div>

    <!-- ===== Charts ===== -->
    <div class="row g-4 mb-4">
        <div class="col-md-6"><div class="card chart-card"><h5>Doctors by Specialization</h5><canvas id="doctorsPieChart"></canvas></div></div>
        <div class="col-md-6"><div class="card chart-card"><h5>Patients per Doctor (Today)</h5><canvas id="patientsPerDoctorBarChart"></canvas></div></div>
        <div class="col-md-6"><div class="card chart-card"><h5>Patients per Specialization</h5><canvas id="patientsBySpecializationChart"></canvas></div></div>
        <div class="col-md-6"><div class="card chart-card"><h5>Daily Admissions vs Discharges</h5><canvas id="patientsLineChart"></canvas></div></div>
    </div>

</div>

<script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
<script>
    // ===== Doctors by Specialization =====
    new Chart(document.getElementById('doctorsPieChart'), {
        type: 'pie',
        data: {
            labels: <%= DepartmentNamesJson %>,
            datasets: [{
                data: <%= DepartmentCountsJson %>,
                backgroundColor: ['rgba(231,74,59,0.7)', 'rgba(78,115,223,0.7)', 'rgba(246,194,62,0.7)', 'rgba(28,200,138,0.7)', 'rgba(111,66,193,0.7)']
            }]
        },
        options: { responsive: true, maintainAspectRatio: false }
    });

    // ===== Patients per Doctor =====
    new Chart(document.getElementById('patientsPerDoctorBarChart'), {
        type: 'bar',
        data: {
            labels: <%= DoctorNamesJson %>,
            datasets: [{
                label: 'Appointments Today',
                data: <%= DoctorAppointmentsJson %>,
                backgroundColor: 'rgba(255,159,64,0.7)',
                borderRadius: 6
            }]
        },
        options: { responsive: true, maintainAspectRatio: false, scales: { y: { beginAtZero: true, precision:0 } } }
    });

    // ===== Patients per Specialization =====
    new Chart(document.getElementById('patientsBySpecializationChart'), {
        type: 'bar',
        data: {
            labels: <%= PatientsSpecializationNamesJson %>,
            datasets: [{
                label: 'Total Patients',
                data: <%= PatientsSpecializationCountsJson %>,
                backgroundColor: 'rgba(54,185,204,0.7)',
                borderRadius: 6
            }]
        },
        options: { responsive: true, maintainAspectRatio: false, scales: { y: { beginAtZero: true, precision:0 } } }
    });

    // ===== Daily Admissions vs Discharges =====
    new Chart(document.getElementById('patientsLineChart'), {
        type: 'line',
        data: {
            labels: ['Mon','Tue','Wed','Thu','Fri','Sat','Sun'],
            datasets: [
                { label:'Admissions', data:<%= DailyAdmissionsJson %>, borderColor:'rgba(28,200,138,1)', backgroundColor:'rgba(28,200,138,0.2)', fill:true, tension:0.4 },
                { label:'Discharges', data:<%= DailyDischargesJson %>, borderColor: 'rgba(231,74,59,1)', backgroundColor: 'rgba(231,74,59,0.2)', fill: true, tension: 0.4 }
            ]
        },
        options: { responsive: true, maintainAspectRatio: false }
    });
</script>
</asp:Content>
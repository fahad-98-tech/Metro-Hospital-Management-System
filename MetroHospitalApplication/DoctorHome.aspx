<%@ Page Title="Doctor Dashboard" Language="C#" MasterPageFile="~/Doctor.Master" 
    AutoEventWireup="true" CodeBehind="DoctorHome.aspx.cs" Inherits="MetroHospitalApplication.DoctorHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <style>
        .welcome {
            font-size: 1.6rem;
            font-weight: 700;
            margin-bottom: 20px;
            color: #2c3e50;
        }

        .cards-container {
            display: flex;
            flex-wrap: wrap;
            gap: 20px;
            margin-bottom: 30px;
        }

        .cards-container .card {
            flex: 1 1 20%;
            min-width: 150px;
            border-radius: 15px;
            box-shadow: 0 6px 15px rgba(0,0,0,0.1);
            text-align: center;
            padding: 20px 0;
            transition: transform 0.2s;
            color: white;
        }

        .cards-container .card:hover {
            transform: translateY(-5px);
            box-shadow: 0 12px 25px rgba(0,0,0,0.15);
        }

        .cards-container .card h6 { font-size: 0.85rem; font-weight: 600; text-transform: uppercase; margin-bottom: 10px; opacity: 0.9; }
        .cards-container .card h3 { font-size: 1.8rem; font-weight: 700; }

        .card-today { background: linear-gradient(135deg, #f39c12, #f1c40f); }
        .card-pending { background: linear-gradient(135deg, #e67e22, #d35400); }
        .card-approved { background: linear-gradient(135deg, #27ae60, #2ecc71); }
        .card-rejected { background: linear-gradient(135deg, #c0392b, #e74c3c); }

        .chart-card {
            border-radius: 15px;
            box-shadow: 0 6px 20px rgba(0,0,0,0.1);
            padding: 25px;
            background: white;
            margin-bottom: 50px;
        }

        #apptChart { width: 100% !important; max-width: 100%; }

        .appointments-table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 30px;
        }

        .appointments-table th, .appointments-table td {
            border: 1px solid #ddd;
            padding: 8px;
            text-align: center;
        }

        .appointments-table th {
            background-color: #f8f8f8;
        }

        @media (max-width: 992px) { .cards-container .card { flex: 1 1 45%; } .welcome { font-size: 1.4rem; } }
        @media (max-width: 480px) { .cards-container .card { flex: 1 1 100%; } }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="welcome">
        Hello, <asp:Label ID="lblDoctorName" runat="server" />! Here’s your dashboard.
    </div>

    <div class="cards-container">
        <div class="card card-today">
            <h6>Today</h6>
            <h3><asp:Label ID="lblToday" runat="server" /></h3>
        </div>
        <div class="card card-pending">
            <h6>Pending</h6>
            <h3><asp:Label ID="lblPending" runat="server" /></h3>
        </div>
        <div class="card card-approved">
            <h6>Approved</h6>
            <h3><asp:Label ID="lblApproved" runat="server" /></h3>
        </div>
        <div class="card card-rejected">
            <h6>Rejected</h6>
            <h3><asp:Label ID="lblRejected" runat="server" /></h3>
        </div>
    </div>

    <div class="chart-card">
        <h5>Appointment Status Overview</h5>
        <div style="position: relative; height: 250px; width: 100%;">
            <canvas id="apptChart"></canvas>
        </div>
    </div>

    <asp:Literal ID="litPending" runat="server" Visible="false" />
    <asp:Literal ID="litApproved" runat="server" Visible="false" />
    <asp:Literal ID="litRejected" runat="server" Visible="false" />

    <!-- Today's Appointments -->
    <h5>Today's Appointments</h5>
    <asp:Repeater ID="rptAppointments" runat="server">
        <HeaderTemplate>
            <table class="appointments-table">
                <tr>
                    <th>Patient Name</th>
                    <th>Mobile</th>
                    <th>Time</th>
                    <th>End Time</th>
                    <th>Specialization</th>
                    <th>Status</th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td><%# Eval("PatientName") %></td>
                <td><%# Eval("PatientMobile") %></td>
                <td><%# Eval("AppointmentTime") %></td>
                <td><%# Eval("AppointmentEndTime") %></td>
                <td><%# Eval("Specialization") %></td>
                <td><%# Eval("Status") %></td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>

    <script>
        document.addEventListener('DOMContentLoaded', function () {
            var ctx = document.getElementById('apptChart').getContext('2d');

            new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: ['Pending', 'Approved', 'Rejected'],
                    datasets: [{
                        label: 'Appointments',
                        data: [
                            parseInt('<%= litPending.Text %>'),
                            parseInt('<%= litApproved.Text %>'),
                            parseInt('<%= litRejected.Text %>')
                        ],
                        backgroundColor: ['#e67e22', '#27ae60', '#c0392b'],
                        borderRadius: 10
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: { display: false },
                        title: { display: true, text: 'Appointments Overview', font: { size: 18 } }
                    },
                    animation: { duration: 500 },
                    scales: { y: { beginAtZero: true, ticks: { stepSize: 1 } } },
                    layout: { padding: 0 }
                }
            });

            window.scrollTo(0, 0);
        });
    </script>
</asp:Content>

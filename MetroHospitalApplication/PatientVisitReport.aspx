<%@ Page Title="Patient Visit Report" Language="C#" MasterPageFile="~/Doctor.Master" AutoEventWireup="true" CodeBehind="PatientVisitReport.aspx.cs" Inherits="MetroHospitalApplication.PatientVisitReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .report-card {
            border-radius: 12px;
            background-color: #fff;
            padding: 20px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.1);
            margin-top: 20px;
        }

        .filter-section {
            margin-bottom: 20px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <h3>🧾 My Patient Visit Report</h3>
        <hr />

        <!-- Filters -->
        <div class="row filter-section">
            <div class="col-md-3">
                <label>From Date:</label>
                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            </div>
            <div class="col-md-3">
                <label>To Date:</label>
                <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            </div>
            <div class="col-md-3 d-flex align-items-end">
                <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-primary w-100" OnClick="btnFilter_Click" />
            </div>
        </div>

        <!-- Report Table -->
        <div class="report-card">
            <asp:GridView ID="gvPatientVisits" runat="server" CssClass="table table-striped table-bordered table-hover"
                AutoGenerateColumns="false" EmptyDataText="No patient visits found">
                <Columns>
                    <asp:BoundField HeaderText="Appointment ID" DataField="AppointmentId" />
                    <asp:BoundField HeaderText="Patient Name" DataField="PatientName" />
                    <asp:BoundField HeaderText="Patient Mobile" DataField="PatientMobile" />
                    <asp:BoundField HeaderText="Specialization" DataField="Specialization" />
                    <asp:BoundField HeaderText="Appointment Date" DataField="AppointmentDate" DataFormatString="{0:dd-MMM-yyyy}" />
                    <asp:BoundField HeaderText="Time" DataField="AppointmentTime" />
                    <asp:BoundField HeaderText="Status" DataField="Status" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
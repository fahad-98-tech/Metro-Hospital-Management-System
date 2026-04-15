<%@ Page Title="Doctor Performance Report" Language="C#" MasterPageFile="~/Doctor.Master"
    AutoEventWireup="true" CodeBehind="DoctorPerformanceReport.aspx.cs"
    Inherits="MetroHospitalApplication.DoctorPerformanceReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        /* GridView status colors */
        .status-Booked { background-color: #fff3cd !important; }        /* Orange */
        .status-Completed { background-color: #d4edda !important; }     /* Green */
        .status-Cancelled { background-color: #f8d7da !important; }     /* Red */
        .status-Done { background-color: #cce5ff !important; }          /* Blue */

        /* Filter panel styling */
        .filter-panel { padding: 20px; border: 1px solid #dee2e6; border-radius: 10px; margin-bottom: 20px; background-color: #f8f9fa; }

        /* Summary card styling */
        .summary-card {
            border: 1px solid #dee2e6;
            border-radius: 10px;
            padding: 15px;
            background-color: #ffffff;
            box-shadow: 0 0 10px rgba(0,0,0,0.05);
        }
        .summary-card h5 { font-weight: bold; font-size: 1.1rem; margin-bottom: 10px; }
        .summary-item { display: flex; justify-content: space-between; padding: 5px 0; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container my-4">
        <h2 class="mb-3">Doctor Performance Report</h2>

        <!-- Filter Panel -->
        <div class="filter-panel row g-3">
            <div class="col-md-3">
                <label for="txtFromDate" class="form-label">From Date:</label>
                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            </div>
            <div class="col-md-3">
                <label for="txtToDate" class="form-label">To Date:</label>
                <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            </div>
            <div class="col-md-3">
                <label for="ddlStatus" class="form-label">Status:</label>
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select">
                    <asp:ListItem Text="All" Value=""></asp:ListItem>
                    <asp:ListItem Text="Booked" Value="Booked"></asp:ListItem>
                    <asp:ListItem Text="Completed" Value="Completed"></asp:ListItem>
                    <asp:ListItem Text="Cancelled" Value="Cancelled"></asp:ListItem>
                    <asp:ListItem Text="Done" Value="Done"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-md-3 d-flex align-items-end">
                <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-primary w-100" OnClick="btnFilter_Click" />
            </div>
        </div>

        <!-- GridView -->
        <asp:GridView ID="gvAppointments" runat="server" AutoGenerateColumns="False"
            CssClass="table table-bordered table-striped table-hover"
            OnRowDataBound="gvAppointments_RowDataBound">
            <Columns>
                <asp:BoundField DataField="AppointmentId" HeaderText="ID" />
                <asp:BoundField DataField="PatientName" HeaderText="Patient" />
                <asp:BoundField DataField="PatientMobile" HeaderText="Mobile" />
                <asp:BoundField DataField="AppointmentDate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:BoundField DataField="AppointmentTime" HeaderText="Start Time" />
                <asp:BoundField DataField="AppointmentEndTime" HeaderText="End Time" />
                <asp:TemplateField HeaderText="Total Time">
                    <ItemTemplate>
                        <%# GetTotalTime(Eval("AppointmentTime"), Eval("AppointmentEndTime")) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Status" HeaderText="Status" />
                <asp:BoundField DataField="ConsultationFee" HeaderText="Consultation Fee" DataFormatString="{0:C}" />
                <asp:BoundField DataField="MedicineCharges" HeaderText="Medicine Charges" DataFormatString="{0:C}" />
                <asp:BoundField DataField="TestCharges" HeaderText="Test Charges" DataFormatString="{0:C}" />
                <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount" DataFormatString="{0:C}" />
            </Columns>
        </asp:GridView>

        <!-- Summary Panel -->
        <div class="summary-card mt-4">
            <h5>Summary</h5>
            <div class="summary-item">
                <span>Total Appointments:</span>
                <span><asp:Label ID="lblTotalAppointments" runat="server" Text="0"></asp:Label></span>
            </div>
            <div class="summary-item">
                <span>Completed:</span>
                <span><asp:Label ID="lblCompletedAppointments" runat="server" Text="0"></asp:Label></span>
            </div>
            <div class="summary-item">
                <span>Booked:</span>
                <span><asp:Label ID="lblBookedAppointments" runat="server" Text="0"></asp:Label></span>
            </div>
            <div class="summary-item">
                <span>Cancelled:</span>
                <span><asp:Label ID="lblCancelledAppointments" runat="server" Text="0"></asp:Label></span>
            </div>
            <div class="summary-item">
                <span>Done:</span>
                <span><asp:Label ID="lblDoneAppointments" runat="server" Text="0"></asp:Label></span>
            </div>
            <div class="summary-item">
                <span>Total Revenue:</span>
                <span><asp:Label ID="lblTotalRevenue" runat="server" Text="$0.00"></asp:Label></span>
            </div>
        </div>
    </div>
</asp:Content>
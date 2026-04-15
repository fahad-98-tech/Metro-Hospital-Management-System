<%@ Page Title="Patient Report" Language="C#" MasterPageFile="~/Admin.Master"
    AutoEventWireup="true" CodeBehind="PatientReport.aspx.cs"
    Inherits="MetroHospitalApplication.PatientReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .report-title {
            font-weight: 700;
            font-size: 24px;
        }

        .filter-box {
            background: #ffffff;
            padding: 20px;
            border-radius: 12px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.08);
        }

        .table thead {
            background-color: #4e73df;
            color: white;
        }

        .card-box {
            background: #ffffff;
            padding: 20px;
            border-radius: 12px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.08);
        }

        .status-badge {
            display: inline-block;
            padding: 4px 10px;
            border-radius: 12px;
            font-weight: 600;
            color: #000;
        }

        .status-done { background-color: #90ee90; }       /* LightGreen */
        .status-approved { background-color: #add8e6; }   /* LightBlue */
        .status-booked { background-color: #f0e68c; }     /* Khaki */
        .status-notfound { background-color: #d3d3d3; }   /* LightGray */
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container-fluid">

    <!-- Header -->
    <div class="d-flex justify-content-between align-items-center mb-4">
        <h2 class="report-title">
            <i class="fa-solid fa-file-medical me-2"></i> Patient Report
        </h2>

        <asp:Button ID="btnExportExcel" runat="server"
            Text="Export to Excel"
            CssClass="btn btn-success"
            OnClick="btnExportExcel_Click" />
    </div>

    <!-- Filter Section -->
    <div class="filter-box mb-4">
        <div class="row g-3">

            <div class="col-md-3">
                <label>Patient Name</label>
                <asp:TextBox ID="txtPatientName" runat="server" CssClass="form-control" Placeholder="Enter patient name"></asp:TextBox>
            </div>

            <div class="col-md-3">
                <label>From Date</label>
                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            </div>

            <div class="col-md-3">
                <label>To Date</label>
                <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            </div>

            <div class="col-md-3">
                <label>Gender</label>
                <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-select">
                    <asp:ListItem Text="All" Value="" />
                    <asp:ListItem Text="Male" Value="Male" />
                    <asp:ListItem Text="Female" Value="Female" />
                </asp:DropDownList>
            </div>

            <div class="col-md-3">
                <label>Status</label>
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select">
                    <asp:ListItem Text="All" Value="" />
                    <asp:ListItem Text="Done" Value="Done" />
                    <asp:ListItem Text="Approved" Value="Approved" />
                    <asp:ListItem Text="Booked" Value="Booked" />
                    <asp:ListItem Text="Not Found" Value="Not Found" />
                </asp:DropDownList>
            </div>

            <div class="col-md-3 d-flex align-items-end">
                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary w-100" OnClick="btnSearch_Click" />
            </div>

        </div>
    </div>

    <!-- Grid Section -->
    <div class="card-box">
        <div class="table-responsive">
            <asp:GridView ID="gvPatients" runat="server"
                CssClass="table table-bordered table-hover"
                AutoGenerateColumns="False"
                EmptyDataText="No records found."
                AllowPaging="true"
                PageSize="10"
                OnPageIndexChanging="gvPatients_PageIndexChanging"
                OnRowDataBound="gvPatients_RowDataBound">

                <Columns>
                    <asp:BoundField DataField="PatientID" HeaderText="ID" />
                    <asp:BoundField DataField="FullName" HeaderText="Full Name" />
                    <asp:BoundField DataField="Gender" HeaderText="Gender" />
                    <asp:BoundField DataField="Age" HeaderText="Age" />
                    <asp:BoundField DataField="MobileNumber" HeaderText="Mobile" />
                    <asp:BoundField DataField="AppointmentDate" HeaderText="Appointment Date" DataFormatString="{0:dd-MM-yyyy}" />
                    <asp:BoundField DataField="DoctorName" HeaderText="Doctor" />
                    <asp:BoundField DataField="Specialization" HeaderText="Specialization" />

                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <asp:Label ID="lblStatus" runat="server"
                                Text='<%# Eval("Status") != DBNull.Value && Eval("Status").ToString() != "" ? Eval("Status") : "Not Found" %>'
                                CssClass="status-badge" />
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>

            </asp:GridView>
        </div>
    </div>

</div>

</asp:Content>
<%@ Page Title="Appointment Report" Language="C#" MasterPageFile="~/Admin.Master"
    AutoEventWireup="true" CodeBehind="AppointmentReport.aspx.cs"
    Inherits="MetroHospitalApplication.AppointmentReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .card {
            background: #fff;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
        }
        .form-control {
            margin-bottom: 10px;
        }
        .gridview {
            margin-top: 20px;
        }
        .status-badge {
            display: inline-block;
            padding: 5px 10px;
            border-radius: 12px;
            font-weight: 600;
            color: #000;
            text-align: center;
            width: 100%;
        }
        .status-booked { background-color: #f0e68c; }      /* Khaki */
        .status-completed { background-color: #90ee90; }   /* LightGreen */
        .status-cancelled { background-color: #ff7f7f; }   /* LightCoral */
        .status-approved { background-color: #87cefa; }    /* LightBlue */
        .status-other { background-color: #d3d3d3; }       /* LightGray */
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="card">
        <h3>Appointment Report</h3>
        <hr />

        <div class="row">

            <div class="col-md-3">
                <asp:Label Text="Doctor" runat="server" />
                <asp:DropDownList ID="ddlDoctor" CssClass="form-control" runat="server" />
            </div>

            <div class="col-md-3">
                <asp:Label Text="Status" runat="server" />
                <asp:DropDownList ID="ddlStatus" CssClass="form-control" runat="server">
                    <asp:ListItem Text="All" Value="" />
                    <asp:ListItem Text="Booked" Value="Booked" />
                    <asp:ListItem Text="Completed" Value="Completed" />
                    <asp:ListItem Text="Cancelled" Value="Cancelled" />
                    <asp:ListItem Text="Approved" Value="Approved" />
                </asp:DropDownList>
            </div>

            <div class="col-md-3">
                <asp:Label Text="From Date" runat="server" />
                <asp:TextBox ID="txtFromDate" TextMode="Date" CssClass="form-control" runat="server" />
            </div>

            <div class="col-md-3">
                <asp:Label Text="To Date" runat="server" />
                <asp:TextBox ID="txtToDate" TextMode="Date" CssClass="form-control" runat="server" />
            </div>

            <div class="col-md-12" style="margin-top:10px;">
                <asp:Button ID="btnSearch" runat="server"
                    Text="Search"
                    CssClass="btn btn-primary"
                    OnClick="btnSearch_Click" />
            </div>

        </div>

        <asp:GridView ID="gvAppointments"
            runat="server"
            CssClass="table table-bordered gridview"
            AutoGenerateColumns="false"
            AllowPaging="true"
            PageSize="10"
            OnPageIndexChanging="gvAppointments_PageIndexChanging"
            OnRowDataBound="gvAppointments_RowDataBound">

            <Columns>
                <asp:BoundField DataField="AppointmentId" HeaderText="ID" />
                <asp:BoundField DataField="DoctorName" HeaderText="Doctor" />
                <asp:BoundField DataField="PatientName" HeaderText="Patient" />
                <asp:BoundField DataField="PatientMobile" HeaderText="Mobile" />
                <asp:BoundField DataField="Specialization" HeaderText="Specialization" />
                <asp:BoundField DataField="AppointmentDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}" />
                <asp:BoundField DataField="AppointmentTime" HeaderText="Start" />
                <asp:BoundField DataField="AppointmentEndTime" HeaderText="End" />

                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <asp:Label ID="lblStatus" runat="server" CssClass="status-badge"
                            Text='<%# Eval("Status") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>

        </asp:GridView>

    </div>

</asp:Content>
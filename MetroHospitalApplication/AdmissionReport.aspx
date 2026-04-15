<%@ Page Title="Admission Report" Language="C#" MasterPageFile="~/Admin.Master"
    AutoEventWireup="true" CodeBehind="AdmissionReport.aspx.cs"
    Inherits="MetroHospitalApplication.AdmissionReport" %>

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
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="card">
        <h3>Admission Report</h3>
        <hr />

        <div class="row">
            <div class="col-md-4">
                <asp:Label Text="Patient Name" runat="server" />
                <asp:TextBox ID="txtPatientName" CssClass="form-control" runat="server" />
            </div>

            <div class="col-md-3">
                <asp:Label Text="From Date" runat="server" />
                <asp:TextBox ID="txtFromDate" TextMode="Date" CssClass="form-control" runat="server" />
            </div>

            <div class="col-md-3">
                <asp:Label Text="To Date" runat="server" />
                <asp:TextBox ID="txtToDate" TextMode="Date" CssClass="form-control" runat="server" />
            </div>

            <div class="col-md-2" style="margin-top:25px;">
                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
            </div>
        </div>

        <asp:GridView ID="gvAdmissions" runat="server" CssClass="table table-bordered gridview"
            AutoGenerateColumns="false" AllowPaging="true" PageSize="10"
            OnPageIndexChanging="gvAdmissions_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="AppointmentId" HeaderText="ID" />
                <asp:BoundField DataField="PatientName" HeaderText="Patient" />
                <asp:BoundField DataField="PatientMobile" HeaderText="Mobile" />
                <asp:BoundField DataField="DoctorName" HeaderText="Doctor" />
                <asp:BoundField DataField="Specialization" HeaderText="Department" />
                <asp:BoundField DataField="AdmissionDate" HeaderText="Admission Date" DataFormatString="{0:dd-MMM-yyyy}" />
                <asp:BoundField DataField="Status" HeaderText="Status" />
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>

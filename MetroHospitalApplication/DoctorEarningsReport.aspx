<%@ Page Title="Doctor Earnings Report" Language="C#" MasterPageFile="~/Admin.Master" 
    AutoEventWireup="true" CodeBehind="DoctorEarningsReport.aspx.cs" 
    Inherits="MetroHospitalApplication.DoctorEarningsReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .card {
            background: #fff;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
        }
        .highlight {
            background-color: #f1f9f1;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card">
        <h3>Doctor Earnings Report</h3>
        <hr />

        <div class="row mb-3">
            <div class="col-md-4">
                <asp:Label ID="lblDoctorId" runat="server" Text="Doctor ID:"></asp:Label>
                <asp:TextBox ID="txtDoctorId" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-md-3">
                <asp:Label ID="lblFromDate" runat="server" Text="From Date:"></asp:Label>
                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            </div>
            <div class="col-md-3">
                <asp:Label ID="lblToDate" runat="server" Text="To Date:"></asp:Label>
                <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            </div>
            <div class="col-md-2 mt-4">
                <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-primary"
                    OnClick="btnFilter_Click" />
                <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-secondary"
                    OnClick="btnReset_Click" />
            </div>
        </div>

       <asp:GridView ID="gvDoctorEarnings" runat="server" CssClass="table table-bordered"
    AutoGenerateColumns="false" EmptyDataText="No earnings found."
    OnRowDataBound="gvDoctorEarnings_RowDataBound">
    <Columns>
        <asp:BoundField DataField="DoctorId" HeaderText="Doctor ID" />
        <asp:BoundField DataField="DoctorName" HeaderText="Doctor Name" />

        
        <asp:BoundField DataField="TotalConsultationFee" HeaderText="Consultation Fee"
            DataFormatString="KWD {0:N3}" HtmlEncode="false" />
        <asp:BoundField DataField="TotalTestCharges" HeaderText="Test Charges"
            DataFormatString="KWD {0:N3}" HtmlEncode="false" />
        <asp:BoundField DataField="TotalMedicineCharges" HeaderText="Medicine Charges"
            DataFormatString="KWD {0:N3}" HtmlEncode="false" />
        <asp:BoundField DataField="TotalEarnings" HeaderText="Total Earnings"
            DataFormatString="KWD {0:N3}" HtmlEncode="false" />
    </Columns>
</asp:GridView>
    </div>
</asp:Content>
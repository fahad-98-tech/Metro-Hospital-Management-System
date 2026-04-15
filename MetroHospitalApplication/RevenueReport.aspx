<%@ Page Title="Revenue Report" Language="C#" MasterPageFile="~/Admin.Master"
    AutoEventWireup="true" CodeBehind="RevenueReport.aspx.cs"
    Inherits="MetroHospitalApplication.RevenueReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .card { background: #fff; padding: 20px; border-radius: 8px; box-shadow: 0 0 10px rgba(0,0,0,0.1); margin-bottom: 20px; }
        .total-revenue { font-weight: bold; font-size: 18px; margin-top: 10px; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container mt-4">
    <div class="card">
        <h3>Revenue Report</h3>
        <hr />

        <div class="row mb-3">
            <div class="col-md-3">
                <label>From Date</label>
                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            </div>
            <div class="col-md-3">
                <label>To Date</label>
                <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            </div>
            <div class="col-md-3 mt-4">
                <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-primary w-100" OnClick="btnFilter_Click" />
            </div>
            <div class="col-md-3 mt-4">
                <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-secondary w-100" OnClick="btnReset_Click" />
            </div>
        </div>

       <asp:GridView ID="gvRevenue" runat="server" CssClass="table table-bordered table-hover" 
    AutoGenerateColumns="false" EmptyDataText="No revenue found." OnRowDataBound="gvRevenue_RowDataBound">
    <Columns>
        <asp:BoundField DataField="InvoiceId" HeaderText="Invoice ID" />
        <asp:BoundField DataField="AppointmentId" HeaderText="Appointment ID" />
        <asp:BoundField DataField="PatientName" HeaderText="Patient Name" />
        <asp:BoundField DataField="CreatedAt" HeaderText="Invoice Date" DataFormatString="{0:dd-MMM-yyyy}" />
        
        <asp:BoundField DataField="ConsultationFee" HeaderText="Consultation Fee" 
            DataFormatString="د.ك {0:N3}" HtmlEncode="false" />
        <asp:BoundField DataField="TestCharges" HeaderText="Test Charges" 
            DataFormatString="د.ك {0:N3}" HtmlEncode="false" />
        <asp:BoundField DataField="MedicineCharges" HeaderText="Medicine Charges" 
            DataFormatString="د.ك {0:N3}" HtmlEncode="false" />
        <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount" 
            DataFormatString="د.ك {0:N3}" HtmlEncode="false" />
        
        <asp:BoundField DataField="PaymentStatus" HeaderText="Payment Status" />
    </Columns>
</asp:GridView>

        <asp:Label ID="lblTotalRevenue" runat="server" CssClass="total-revenue"></asp:Label>

    </div>
</div>

</asp:Content>
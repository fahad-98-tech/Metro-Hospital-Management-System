<%@ Page Title="Completed Payments Report" Language="C#" MasterPageFile="~/Admin.Master" 
    AutoEventWireup="true" CodeBehind="CompletedPaymentsReport.aspx.cs" 
    Inherits="MetroHospitalApplication.CompletedPaymentsReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .card {
            background: #fff;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
        }
        .completed {
            background-color: #d4edda; /* light green */
            font-weight: 600;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card">
        <h3>Completed Payments Report</h3>
        <hr />

        <div class="row mb-3">
            <div class="col-md-3">
                <asp:Label ID="lblFromDate" runat="server" Text="From Date:"></asp:Label>
                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            </div>
            <div class="col-md-3">
                <asp:Label ID="lblToDate" runat="server" Text="To Date:"></asp:Label>
                <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            </div>
            <div class="col-md-3 mt-4">
                <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-primary"
                    OnClick="btnFilter_Click" />
                <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-secondary"
                    OnClick="btnReset_Click" />
            </div>
        </div>

        <asp:GridView ID="gvCompletedPayments" runat="server" CssClass="table table-bordered"
    AutoGenerateColumns="false" EmptyDataText="No completed payments found."
    OnRowDataBound="gvCompletedPayments_RowDataBound">
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
    </div>
</asp:Content>
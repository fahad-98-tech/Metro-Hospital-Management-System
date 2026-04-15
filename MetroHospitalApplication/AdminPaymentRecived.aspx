<%@ Page Title="Payment Received" Language="C#" MasterPageFile="~/Admin.Master"
    AutoEventWireup="true" CodeBehind="AdminPaymentReceived.aspx.cs"
    Inherits="MetroHospitalApplication.AdminPaymentReceived" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .card { background:#fff; padding:20px; border-radius:8px; box-shadow:0 0 10px rgba(0,0,0,0.1); }
        .form-label { font-weight:bold; margin-top:10px; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container mt-4">
    <div class="card">
        <h3>Payment Received</h3>
        <hr />

        <asp:Label ID="lblMessage" runat="server" CssClass="text-success"></asp:Label>

        <div class="row">
            <div class="col-md-4">
                <label class="form-label">Invoice ID</label>
                <asp:TextBox ID="txtInvoiceId" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>
            <div class="col-md-4">
                <label class="form-label">Appointment ID</label>
                <asp:TextBox ID="txtAppointmentId" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>
        </div>

        <div class="row mt-3">
            <div class="col-md-4">
                <label class="form-label">Consultation Fee</label>
                <asp:TextBox ID="txtConsultationFee" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>
            <div class="col-md-4">
                <label class="form-label">Test Charges</label>
                <asp:TextBox ID="txtTestCharges" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>
            <div class="col-md-4">
                <label class="form-label">Medicine Charges</label>
                <asp:TextBox ID="txtMedicineCharges" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>
        </div>

        <div class="row mt-3">
            <div class="col-md-4">
                <label class="form-label">Total Amount</label>
                <asp:TextBox ID="txtTotalAmount" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>
            <div class="col-md-4">
                <label class="form-label">Payment Status</label>
                <asp:DropDownList ID="ddlPaymentStatus" runat="server" CssClass="form-select">
                    <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                    <asp:ListItem Text="Paid" Value="Paid"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>

        <div class="row mt-4">
            <div class="col-md-3">
                <asp:Button ID="btnSavePayment" runat="server" Text="Receive Payment" CssClass="btn btn-success w-100"
                    OnClick="btnSavePayment_Click" />
            </div>
        </div>

    </div>
</div>

</asp:Content>
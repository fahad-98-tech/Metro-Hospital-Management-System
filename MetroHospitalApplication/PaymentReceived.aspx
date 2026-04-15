<%@ Page Title="Receive Payment" Language="C#" MasterPageFile="~/Admin.Master"
    AutoEventWireup="true" CodeBehind="PaymentReceived.aspx.cs"
    Inherits="MetroHospitalApplication.PaymentReceived" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<style>

.card{
    background:#fff;
    padding:25px;
    border-radius:8px;
    box-shadow:0 0 10px rgba(0,0,0,0.1);
}

label{
    font-weight:600;
}

</style>

</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container mt-4">

<div class="card">

<h3>Receive Payment</h3>

<hr />

<div class="row">

<div class="col-md-6 mb-3">

<label>Invoice ID</label>

<asp:TextBox ID="txtInvoiceId"
runat="server"
CssClass="form-control"
ReadOnly="true"></asp:TextBox>

</div>


<div class="col-md-6 mb-3">

<label>Appointment ID</label>

<asp:TextBox ID="txtAppointmentId"
runat="server"
CssClass="form-control"
ReadOnly="true"></asp:TextBox>

</div>


<div class="col-md-4 mb-3">

<label>Consultation Fee</label>

<asp:TextBox ID="txtConsultationFee"
runat="server"
CssClass="form-control"
ReadOnly="true"></asp:TextBox>

</div>


<div class="col-md-4 mb-3">

<label>Test Charges</label>

<asp:TextBox ID="txtTestCharges"
runat="server"
CssClass="form-control"
ReadOnly="true"></asp:TextBox>

</div>


<div class="col-md-4 mb-3">

<label>Medicine Charges</label>

<asp:TextBox ID="txtMedicineCharges"
runat="server"
CssClass="form-control"
ReadOnly="true"></asp:TextBox>

</div>


<div class="col-md-6 mb-3">

<label>Total Amount</label>

<asp:TextBox ID="txtTotalAmount"
runat="server"
CssClass="form-control"
ReadOnly="true"></asp:TextBox>

</div>


<div class="col-md-6 mb-3">

<label>Received Amount</label>

<asp:TextBox ID="txtReceivedAmount"
runat="server"
CssClass="form-control"></asp:TextBox>

</div>

</div>


<br />

<asp:Button ID="btnSavePayment"
runat="server"
Text="Save Payment"
CssClass="btn btn-success"
OnClick="btnSavePayment_Click" />

<asp:Button ID="btnCancel"
runat="server"
Text="Cancel"
CssClass="btn btn-secondary"
PostBackUrl="PatientBillingReport.aspx" />

</div>

</div>

</asp:Content>
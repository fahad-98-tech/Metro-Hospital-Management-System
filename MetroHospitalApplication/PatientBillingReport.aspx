<%@ Page Title="Patient Billing Report" Language="C#" MasterPageFile="~/Admin.Master"
AutoEventWireup="true" CodeBehind="PatientBillingReport.aspx.cs"
Inherits="MetroHospitalApplication.PatientBillingReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<style>

.card{
    background:#fff;
    padding:20px;
    border-radius:8px;
    box-shadow:0 0 10px rgba(0,0,0,0.1);
}

</style>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container mt-4">

<div class="card">

<h3>Patient Billing Report</h3>
<hr />

<div class="row">

<div class="col-md-3">
<label>Patient ID</label>
<asp:TextBox ID="txtPatientId" runat="server" CssClass="form-control"></asp:TextBox>
</div>

<div class="col-md-3">
<label>From Date</label>
<asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
</div>

<div class="col-md-3">
<label>To Date</label>
<asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
</div>

<div class="col-md-3 mt-4">

<asp:Button ID="btnFilter"
runat="server"
Text="Filter"
CssClass="btn btn-primary"
OnClick="btnFilter_Click" />

<asp:Button ID="btnReset"
runat="server"
Text="Reset"
CssClass="btn btn-secondary"
OnClick="btnReset_Click" />

</div>

</div>

<br />

<asp:GridView ID="gvPatientBilling"
runat="server"
CssClass="table table-bordered table-hover"
AutoGenerateColumns="False"
OnRowDataBound="gvPatientBilling_RowDataBound">

<Columns>

<asp:BoundField DataField="InvoiceId" HeaderText="Invoice ID" />

<asp:BoundField DataField="AppointmentId" HeaderText="Appointment ID" />

<asp:BoundField DataField="PatientName" HeaderText="Patient Name" />

<asp:BoundField DataField="CreatedAt"
HeaderText="Invoice Date"
DataFormatString="{0:dd-MMM-yyyy}" />

<asp:BoundField DataField="ConsultationFee"
HeaderText="Consultation Fee"
DataFormatString="{0:C}" />

<asp:BoundField DataField="TestCharges"
HeaderText="Test Charges"
DataFormatString="{0:C}" />

<asp:BoundField DataField="MedicineCharges"
HeaderText="Medicine Charges"
DataFormatString="{0:C}" />

<asp:BoundField DataField="TotalAmount"
HeaderText="Total Amount"
DataFormatString="{0:C}" />

<asp:TemplateField HeaderText="Payment Status">
<ItemTemplate>

<asp:Label ID="lblStatus"
runat="server"
Text='<%# Eval("PaymentStatus") %>'
CssClass="badge p-2"></asp:Label>

</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Payment">

<ItemTemplate>

<asp:Button ID="btnPayment"
runat="server"
Text="Receive Payment"
CssClass="btn btn-success btn-sm"
CommandArgument='<%# Eval("InvoiceId") %>'
OnClick="btnPayment_Click" />

</ItemTemplate>

</asp:TemplateField>

</Columns>

</asp:GridView>

</div>

</div>

</asp:Content>
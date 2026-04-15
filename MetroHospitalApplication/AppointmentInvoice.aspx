<%@ Page Title="Appointment Invoice"
    Language="C#" MasterPageFile="~/Doctor.Master"
    AutoEventWireup="true"
    CodeBehind="AppointmentInvoice.aspx.cs"
    Inherits="MetroHospitalApplication.AppointmentInvoice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2pdf.js/0.10.1/html2pdf.bundle.min.js"></script>
    <style>
        .invoice-card { 
            background: #fff; 
            padding: 30px; 
            border-radius: 10px; 
            box-shadow: 0 0 20px rgba(0,0,0,0.1); 
            max-width: 900px; 
            margin: 20px auto;
        }
        .invoice-header { display: flex; justify-content: space-between; align-items: center; }
        .invoice-header h2 { margin: 0; }
        .invoice-header img { height: 60px; }
        .invoice-table th, .invoice-table td { padding: 10px; text-align: left; }
        .invoice-table th { background-color: #f1f3f5; }
        .total-row td { font-weight: bold; }
        .btn-pdf { margin-top: 20px; }
        .text-muted { color: #6c757d; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container mt-3">
    <asp:Label ID="lblAppointment" runat="server" CssClass="fw-bold mb-3 d-block"></asp:Label>

    <!-- GridView for Existing Invoices -->
    <asp:Label ID="lblGridMsg" runat="server" CssClass="fw-bold text-danger"></asp:Label>
    <asp:GridView ID="gvInvoices" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered mb-3"
        OnRowEditing="gvInvoices_RowEditing" OnRowUpdating="gvInvoices_RowUpdating"
        OnRowCancelingEdit="gvInvoices_RowCancelingEdit" OnRowDeleting="gvInvoices_RowDeleting"
        DataKeyNames="InvoiceId">
        <Columns>
            <asp:BoundField DataField="InvoiceId" HeaderText="Invoice ID" ReadOnly="True" />
            <asp:BoundField DataField="ConsultationFee" HeaderText="Consultation Fee" />
            <asp:BoundField DataField="TestCharges" HeaderText="Test Charges" />
            <asp:BoundField DataField="MedicineCharges" HeaderText="Medicine Charges" />
            <asp:BoundField DataField="PaymentStatus" HeaderText="Payment Status" />
            <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
        </Columns>
    </asp:GridView>

    <!-- Input fields for new invoice -->
    <div class="row mb-3">
        <div class="col-md-3">
            <label class="fw-bold">Consultation Fee</label>
            <asp:TextBox ID="txtConsultationFee" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="col-md-3">
            <label class="fw-bold">Test Charges</label>
            <asp:TextBox ID="txtTestCharges" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="col-md-3">
            <label class="fw-bold">Medicine Charges</label>
            <asp:TextBox ID="txtMedicineCharges" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="col-md-3">
            <label class="fw-bold">Payment Status</label>
            <asp:DropDownList ID="ddlPaymentStatus" runat="server" CssClass="form-select">
                <asp:ListItem Text="Pending" />
                <asp:ListItem Text="Paid" />
            </asp:DropDownList>
        </div>
    </div>

    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success" Text="Save Invoice" OnClick="btnSave_Click" />
    <asp:Button ID="btnView" runat="server" CssClass="btn btn-primary ms-2" Text="View Invoice" OnClick="btnView_Click" />
    <asp:Label ID="lblMsg" runat="server" CssClass="fw-bold mt-2 d-block text-danger"></asp:Label>

    <!-- Invoice Display Panel -->
    <asp:Panel ID="pnlInvoiceReport" runat="server" CssClass="invoice-card" Visible="false">
        <div class="invoice-header">
            <h2>Medical Invoice</h2>
            <img src='<%= ResolveUrl("~/Images/MetroLogo.jpg") %>' alt="Hospital Logo" />

        </div>

        <div class="row mt-3 mb-3">
            <div class="col-md-6">
                <strong>Invoice To:</strong><br />
                <asp:Label ID="lblPatientName" runat="server"></asp:Label><br />
                <asp:Label ID="lblPatientMobile" runat="server"></asp:Label><br />
                <asp:Label ID="lblSpecialization" runat="server"></asp:Label>
            </div>
            <div class="col-md-6 text-end">
                <strong>Invoice Number:</strong><br />
                <asp:Label ID="lblInvoiceNumber" runat="server"></asp:Label><br />
                <strong>Appointment Date:</strong><br />
                <asp:Label ID="lblAppointmentDate" runat="server"></asp:Label>
            </div>
        </div>

        <table class="table table-bordered invoice-table">
            <thead>
                <tr>
                    <th>Medicine</th>
                    <th>Dosage</th>
                    <th>Duration</th>
                    <th>Instructions</th>
                </tr>
            </thead>
            <tbody id="gvInvoiceMedicines" runat="server">
                <!-- Medicines will be loaded here dynamically -->
            </tbody>
        </table>

        <div class="mb-3">
            <strong>Consultation Fee:</strong> <asp:Label ID="lblInvConsultationFee" runat="server"></asp:Label><br />
            <strong>Test Charges:</strong> <asp:Label ID="lblInvTestCharges" runat="server"></asp:Label><br />
            <strong>Medicine Charges:</strong> <asp:Label ID="lblInvMedicineCharges" runat="server"></asp:Label><br />
            <strong>Total:</strong> <asp:Label ID="lblInvTotalAmount" runat="server"></asp:Label>
        </div>

        <div class="mt-3">
            <strong>Hospital Rules & Regulations:</strong><br />
            1. Please arrive 15 minutes before your appointment.<br />
            2. Carry all previous medical reports.<br />
            3. Payment must be made before discharge.<br />
            4. No smoking inside the hospital premises.<br />
            5. Follow doctor's advice for medication and treatment.<br />
            <strong>Thank You!</strong>
        </div>

        <button type="button" class="btn btn-danger btn-pdf" onclick="downloadInvoice()">Download PDF</button>
    </asp:Panel>
</div>

<script>
    function downloadInvoice() {
        var element = document.querySelector('.invoice-card');
        html2pdf().from(element).save('Invoice.pdf');
    }
</script>

</asp:Content>

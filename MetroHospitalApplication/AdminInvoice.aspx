<%@ Page Title="Admin Invoice" Language="C#" MasterPageFile="~/Admin.Master"
    AutoEventWireup="true" CodeBehind="AdminInvoice.aspx.cs" Inherits="MetroHospitalApplication.AdminInvoice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .card { background: #fff; padding: 20px; border-radius: 8px; box-shadow: 0 0 10px rgba(0,0,0,0.1); margin-bottom:20px; }
        .table th, .table td { vertical-align: middle !important; }
        .section-title { margin-top: 20px; font-weight: bold; font-size: 18px; }
        .file-link { text-decoration: none; color: #007bff; }
        .file-link:hover { text-decoration: underline; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="card">
        <h3>Appointment Invoice Details</h3>
        <hr />
        <asp:Label ID="lblAppointmentId" runat="server" Text="" CssClass="mb-2"></asp:Label>
        <br />

        <asp:Label ID="lblPatient" runat="server" Text="" /><br />
        <asp:Label ID="lblMobile" runat="server" Text="" /><br />
        <asp:Label ID="lblDoctor" runat="server" Text="" /><br />
        <asp:Label ID="lblDepartment" runat="server" Text="" /><br />
        <asp:Label ID="lblAppointmentDate" runat="server" Text="" /><br />
    </div>

    <!-- Medicines -->
    <div class="card">
        <h4 class="section-title">Medicines</h4>
        <asp:GridView ID="gvMedicines" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered">
            <Columns>
                <asp:BoundField DataField="MedicineName" HeaderText="Medicine Name" />
                <asp:BoundField DataField="Dosage" HeaderText="Dosage" />
                <asp:BoundField DataField="Duration" HeaderText="Duration" />
                <asp:BoundField DataField="Instructions" HeaderText="Instructions" />
            </Columns>
        </asp:GridView>
    </div>

    <!-- Treatments -->
    <div class="card">
        <h4 class="section-title">Treatments</h4>
        <asp:GridView ID="gvTreatments" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered">
            <Columns>
                <asp:BoundField DataField="Symptoms" HeaderText="Symptoms" />
                <asp:BoundField DataField="Diagnosis" HeaderText="Diagnosis" />
                <asp:BoundField DataField="Notes" HeaderText="Notes" />
            </Columns>
        </asp:GridView>
    </div>

    <!-- Reports -->
   <!-- Reports -->
<div class="card">
    <h4 class="section-title">Reports</h4>
    <asp:GridView ID="gvReports" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered">
        <Columns>

            <asp:BoundField DataField="ReportType" HeaderText="Report Type" />

            <asp:TemplateField HeaderText="File">
                <ItemTemplate>
                    <a href='<%# ResolveUrl(Eval("FilePath").ToString()) %>' target="_blank" class="file-link">
                        View File
                    </a>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="UploadedAt" HeaderText="Uploaded At" DataFormatString="{0:dd-MMM-yyyy}" />

        </Columns>
    </asp:GridView>
</div>

    <!-- Invoice -->
    <div class="card">
        <h4 class="section-title">Invoice Charges</h4>
        <asp:Label ID="lblConsultationFee" runat="server" Text="" /><br />
        <asp:Label ID="lblTestCharges" runat="server" Text="" /><br />
        <asp:Label ID="lblMedicineCharges" runat="server" Text="" /><br />
        <asp:Label ID="lblTotalAmount" runat="server" Text="" /><br />
        <asp:Label ID="lblPaymentStatus" runat="server" Text="" /><br />
    </div>

</asp:Content>
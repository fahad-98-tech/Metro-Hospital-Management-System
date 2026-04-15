<%@ Page Title="Appointment Treatment"
    Language="C#"
    MasterPageFile="~/Doctor.Master"
    AutoEventWireup="true"
    CodeBehind="AppointmentTreatment.aspx.cs"
    Inherits="MetroHospitalApplication.AppointmentTreatment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .mb-3 { margin-bottom: 1rem; }
        .fw-bold { font-weight: bold; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-3">
        <asp:HiddenField ID="hfTreatmentId" runat="server" />

        <asp:Label ID="lblAppointment" runat="server" CssClass="fw-bold mb-3 d-block"></asp:Label>

        <div class="mb-3">
            <label class="fw-bold">Symptoms</label>
            <asp:TextBox ID="txtSymptoms" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
        </div>

        <div class="mb-3">
            <label class="fw-bold">Diagnosis</label>
            <asp:TextBox ID="txtDiagnosis" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
        </div>

        <div class="mb-3">
            <label class="fw-bold">Notes</label>
            <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
        </div>

        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success" Text="Save" OnClick="btnSave_Click" />
        <asp:Label ID="lblMsg" runat="server" CssClass="fw-bold mt-2 d-block"></asp:Label>
    </div>
</asp:Content>

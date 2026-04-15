<%@ Page Title="Cancel Test Booking" Language="C#" MasterPageFile="~/Patient.Master"
    AutoEventWireup="true" CodeBehind="CancelTestBooking.aspx.cs"
    Inherits="MetroHospitalApplication.CancelTestBooking" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style>
.page-title { font-size:26px; font-weight:600; margin-bottom:20px; }

.card-box {
    border-radius: 10px;
    padding: 20px;
    background: #ffffff;
    box-shadow: 0 2px 6px rgba(0,0,0,0.1);
}

.label-title {
    font-weight: 600;
}
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="container mt-4">

    <h3 class="page-title">Cancel Test Booking</h3>

    <asp:Panel ID="pnlCancel" runat="server" CssClass="card-box">

        <p><span class="label-title">Patient Name:</span> 
            <asp:Label ID="lblPatientName" runat="server" /></p>

        <p><span class="label-title">Test Name:</span> 
            <asp:Label ID="lblTestName" runat="server" /></p>

        <p><span class="label-title">Date:</span> 
            <asp:Label ID="lblTestDate" runat="server" /></p>

        <!-- Cancel Reason -->
        <div class="mb-3">
            <label class="form-label">Cancel Reason:</label>
            <asp:TextBox ID="txtReason" runat="server"
                CssClass="form-control" Placeholder="Enter reason"></asp:TextBox>
        </div>

        <!-- Feedback -->
        <div class="mb-3">
            <label class="form-label">Additional Feedback:</label>
            <asp:TextBox ID="txtFeedback" runat="server" TextMode="MultiLine"
                CssClass="form-control" Rows="4" Placeholder="Optional feedback"></asp:TextBox>
        </div>

        <asp:Button ID="btnCancelBooking" runat="server"
            Text="Cancel Booking"
            CssClass="btn btn-danger"
            OnClick="btnCancelBooking_Click"
            OnClientClick="return confirm('Are you sure you want to cancel this test booking?');" />

    </asp:Panel>

</div>
</asp:Content>
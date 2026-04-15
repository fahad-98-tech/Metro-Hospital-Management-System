<%@ Page Title="Manage Test Booking" Language="C#" MasterPageFile="~/Patient.Master"
    AutoEventWireup="true" CodeBehind="ManageTest.aspx.cs" Inherits="MetroHospitalApplication.ManageTest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style>
.page-title { font-size: 26px; font-weight: 600; margin-bottom: 20px; }
.card { border-radius: 12px; padding: 20px; margin-bottom: 15px; box-shadow: 0 3px 10px rgba(0,0,0,0.05); }
.slot-card { display:inline-block; margin:5px; padding:10px 15px; border-radius:8px; font-weight:600; cursor:pointer; }
.slot-available { background-color:#28a745; color:white; }
.slot-booked { background-color:#dc3545; color:white; }
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container mt-4">
    <h3 class="page-title">Manage Test Booking</h3>

    <asp:Panel ID="pnlTestDetails" runat="server" CssClass="card">
        <p><b>Patient Name:</b> <asp:Label ID="lblPatientName" runat="server" /></p>
        <p><b>Test Name:</b> <asp:Label ID="lblTestName" runat="server" /></p>
        <p><b>Date:</b> <asp:Label ID="lblTestDate" runat="server" /></p>
    </asp:Panel>

    <h5>Available Time Slots</h5>
    <asp:Panel ID="pnlSlots" runat="server"></asp:Panel>
    <p><b>Time:</b> <asp:Label ID="lblTestTime" runat="server" /></p>
    <div class="mt-3">
        <asp:Button ID="btnBookTest" runat="server" Text="Book Selected Slot" CssClass="btn btn-success"
                    OnClick="btnBookTest_Click" />
        <asp:HiddenField ID="hfSelectedTime" runat="server" />
    </div>
</div>

<script type="text/javascript">
    function selectSlot(slot, time) {
        // Remove previous selection
        var slots = document.getElementsByClassName('slot-card');
        for (var i = 0; i < slots.length; i++) {
            slots[i].classList.remove('selected');
        }

        slot.classList.add('selected');
        document.getElementById('<%= hfSelectedTime.ClientID %>').value = time;
    }
</script>

</asp:Content>
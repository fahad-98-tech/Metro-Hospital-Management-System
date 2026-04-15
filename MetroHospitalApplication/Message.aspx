<%@ Page Title="Message Details" Language="C#" MasterPageFile="~/Patient.Master" AutoEventWireup="true" CodeBehind="Message.aspx.cs" Inherits="MetroHospitalApplication.Message" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .message-card {
            border-radius: 10px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.05);
            padding: 20px;
            background-color: #fff;
        }
        .label-title {
            font-weight: 600;
            color: #0D1B2A;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container">
        <div class="message-card">
            <h4 class="mb-3">📩 Notification Details</h4>

            <p><span class="label-title">Message:</span><br />
                <asp:Label ID="lblMessage" runat="server" />
            </p>

            <p><span class="label-title">Doctor:</span>
                <asp:Label ID="lblDoctor" runat="server" />
            </p>

            <p><span class="label-title">Appointment Date:</span>
                <asp:Label ID="lblDate" runat="server" />
            </p>

            <p><span class="label-title">Time:</span>
                <asp:Label ID="lblTime" runat="server" />
            </p>

            <p><span class="label-title">Created On:</span>
                <asp:Label ID="lblCreated" runat="server" />
            </p>

            <a href="PatientHome.aspx" class="btn btn-primary mt-3">
                Back to Home
            </a>
        </div>
    </div>

</asp:Content>
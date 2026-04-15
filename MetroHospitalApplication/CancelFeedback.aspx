<%@ Page Title="Cancel Appointment" Language="C#" MasterPageFile="~/Patient.Master" AutoEventWireup="true" CodeBehind="CancelFeedback.aspx.cs" Inherits="MetroHospitalApplication.CancelFeedback" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* Custom styling for cancel feedback form */
        .cancel-card {
            max-width: 500px;
            margin: 40px auto;
            padding: 30px;
            box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
            border-radius: 12px;
            background-color: #ffffff;
        }
        .cancel-card h3 {
            font-weight: 600;
            margin-bottom: 20px;
            color: #333;
        }
        .cancel-card .form-check {
            margin-bottom: 10px;
        }
        .cancel-card .btn-submit {
            width: 100%;
            font-size: 16px;
            padding: 10px;
        }
        @media (max-width: 576px) {
            .cancel-card {
                margin: 20px;
                padding: 20px;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="cancel-card">
        <h3 class="text-center">Cancel Appointment</h3>
        <p class="text-muted text-center">Please select the reason for cancellation</p>

        <asp:RadioButtonList ID="rblReason" runat="server" CssClass="form-check">
            <asp:ListItem CssClass="form-check-input">Feeling better</asp:ListItem>
            <asp:ListItem CssClass="form-check-input">Schedule conflict</asp:ListItem>
            <asp:ListItem CssClass="form-check-input">Doctor not available</asp:ListItem>
            <asp:ListItem CssClass="form-check-input">Too expensive</asp:ListItem>
            <asp:ListItem CssClass="form-check-input">Booked by mistake</asp:ListItem>
            <asp:ListItem CssClass="form-check-input">Long waiting time</asp:ListItem>
            <asp:ListItem CssClass="form-check-input">Other</asp:ListItem>
        </asp:RadioButtonList>

        <asp:Button ID="btnSubmit" runat="server" Text="Submit"
            CssClass="btn btn-danger btn-submit mt-3"
            OnClick="btnSubmit_Click" />
    </div>
</asp:Content>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="MetroHospitalApplication.Registration" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Patient Registration</title>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />

    <style>
        body {
            background: linear-gradient(135deg, #eef2f7, #dfe7f2);
            min-height: 100vh;
        }
        .center-box {
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
        }
        .card {
            width: 100%;
            max-width: 520px;
            border-radius: 12px;
        }
    </style>
</head>

<body>
<form id="form1" runat="server">
    <div class="center-box">
        <div class="card shadow p-4">

            <h3 class="text-center text-primary mb-1">Metro Hospital</h3>
            <p class="text-center text-muted mb-4">Patient Registration</p>

            <!-- Full Name -->
            <div class="mb-3">
                <label>Full Name</label>
                <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" />
            </div>

            <!-- Email -->
            <div class="mb-3">
                <label>Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
            </div>

            <!-- Mobile -->
            <div class="mb-3">
                <label>Mobile Number (Kuwait)</label>
                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="e.g. 51234567" />
            </div>

            <!-- Gender -->
            <div class="mb-3">
                <label>Gender</label>
                <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-select">
                    <asp:ListItem Text="Select Gender" Value="" />
                    <asp:ListItem Text="Male" />
                    <asp:ListItem Text="Female" />
                    <asp:ListItem Text="Other" />
                </asp:DropDownList>
            </div>

            <!-- DOB -->
            <div class="mb-3">
                <label>Date of Birth</label>
                <asp:TextBox ID="txtDOB" runat="server" CssClass="form-control" TextMode="Date" />
            </div>

            <!-- Password -->
            <div class="mb-3">
                <label>Password</label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
            </div>

            <!-- Confirm Password -->
            <div class="mb-4">
                <label>Confirm Password</label>
                <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" />
            </div>

            <!-- Button -->
            <div class="d-grid mb-2">
                <asp:Button ID="btnRegister"
                    runat="server"
                    Text="Create Account"
                    CssClass="btn btn-primary"
                    OnClick="btnRegister_Click" />
            </div>

            <div class="text-center">
                Already have an account?
                <a href="Login.aspx" class="fw-semibold">Login</a>
            </div>

        </div>
    </div>
</form>
</body>
</html>
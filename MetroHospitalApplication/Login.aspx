<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MetroHospitalApplication.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login | Metro Hospital</title>

    <!-- Bootstrap CSS -->
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
        .login-card {
            width: 100%;
            max-width: 420px;
            border-radius: 14px;
        }
        .hospital-logo {
            font-size: 28px;
            font-weight: 600;
            color: #0d6efd;
        }
        .subtitle {
            font-size: 14px;
            color: #6c757d;
        }
    </style>
</head>

<body>
<form id="form1" runat="server">

    <!-- ✅ Required for alert -->
    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <div class="center-box">
        <div class="card shadow-lg p-4 login-card">

            <div class="text-center mb-4">
                <div class="hospital-logo">🏥 Metro Hospital</div>
                <div class="subtitle">Secure Login Portal</div>
            </div>

            <asp:Label ID="lblMessage" runat="server" CssClass="fw-semibold text-danger"></asp:Label>

            <div class="mb-3">
                <label class="form-label">Email Address</label>
                <asp:TextBox ID="txtEmail" runat="server"
                    CssClass="form-control"
                    TextMode="Email"
                    placeholder="Enter email" />
            </div>

            <div class="mb-4">
                <label class="form-label">Password</label>
                <asp:TextBox ID="txtPassword" runat="server"
                    CssClass="form-control"
                    TextMode="Password"
                    placeholder="Enter password" />
            </div>

            <div class="d-grid mb-3">
                <asp:Button ID="btnLogin"
                    runat="server"
                    Text="Login"
                    CssClass="btn btn-primary"
                    OnClick="btnLogin_Click" />
            </div>

            <div class="text-center">
                <span class="subtitle">Don’t have an account?</span>
                <a href="Registration.aspx" class="fw-semibold">Register</a>
            </div>

            <div class="text-center">
                <span class="subtitle">Forgot Password?</span>
                <a href="forgot.aspx" class="fw-semibold">Forgot Password?</a>
            </div>

        </div>
    </div>
</form>
</body>
</html>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Forgot.aspx.cs" Inherits="MetroHospitalApplication.Forgot" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reset Password | Metro Hospital</title>

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
            padding: 30px;
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
        <div class="center-box">
            <div class="card shadow-lg login-card">

                <!-- Logo / Title -->
                <div class="text-center mb-4">
                    <div class="hospital-logo">🏥 Metro Hospital</div>
                    <div class="subtitle">Reset Your Password</div>
                </div>

                <!-- Message -->
                <asp:Label ID="lblMessage" runat="server" CssClass="fw-semibold text-danger mb-3 d-block"></asp:Label>

                <!-- Email -->
                <div class="mb-3">
                    <label class="form-label">Registered Email Address</label>
                    <asp:TextBox ID="txtEmail" runat="server"
                        CssClass="form-control"
                        TextMode="Email"
                        placeholder="Enter your registered email" />
                </div>

                <!-- New Password -->
                <div class="mb-3">
                    <label class="form-label">New Password</label>
                    <asp:TextBox ID="txtNewPassword" runat="server"
                        CssClass="form-control"
                        TextMode="Password"
                        placeholder="Enter new password" />
                </div>

                <!-- Confirm Password -->
                <div class="mb-4">
                    <label class="form-label">Confirm Password</label>
                    <asp:TextBox ID="txtConfirmPassword" runat="server"
                        CssClass="form-control"
                        TextMode="Password"
                        placeholder="Re-enter new password" />
                </div>

                <!-- Reset Button -->
                <div class="d-grid mb-3">
                    <asp:Button ID="btnReset" runat="server" Text="Reset Password"
                        CssClass="btn btn-primary"
                        OnClick="btnReset_Click" />
                </div>

                <!-- Back to Login -->
                <div class="text-center">
                    <a href="Login.aspx" class="fw-semibold">Back to Login</a>
                </div>

            </div>
        </div>
    </form>
</body>
</html>
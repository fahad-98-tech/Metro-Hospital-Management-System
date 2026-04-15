<%@ Page Title="Patient Feedback Report" Language="C#" MasterPageFile="~/Doctor.Master"
    AutoEventWireup="true" CodeBehind="PatientFeedbackReport.aspx.cs"
    Inherits="MetroHospitalApplication.PatientFeedbackReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        .star-rating { color: gold; font-size: 1.2rem; }
        .blink { animation: blink 1s infinite; }
        @keyframes blink {
            0%, 50%, 100% { opacity: 1; }
            25%, 75% { opacity: 0; }
        }
        .filter-panel { padding: 15px; border: 1px solid #dee2e6; border-radius: 8px; margin-bottom: 15px; background-color: #f8f9fa; }
        .summary { margin-top: 15px; font-weight: bold; font-size: 1.1rem; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <h2 class="my-3">Patient Feedback Report</h2>

        <!-- Filter Panel -->
        <div class="filter-panel row g-3">
            <div class="col-md-3">
                <label for="txtFromDate" class="form-label">From Date:</label>
                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            </div>
            <div class="col-md-3">
                <label for="txtToDate" class="form-label">To Date:</label>
                <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            </div>
            <div class="col-md-2 d-flex align-items-end">
                <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-primary w-100" OnClick="btnFilter_Click" />
            </div>
        </div>

        <!-- Feedback Grid -->
        <asp:GridView ID="gvFeedback" runat="server" AutoGenerateColumns="False"
            CssClass="table table-bordered table-striped" OnRowDataBound="gvFeedback_RowDataBound">
            <Columns>
                <asp:BoundField DataField="PatientName" HeaderText="Patient" />
                <asp:BoundField DataField="AppointmentDate" HeaderText="Appointment Date" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:TemplateField HeaderText="Rating">
                    <ItemTemplate>
                        <span class='<%# Eval("RatingCssClass") %>'><%# Eval("Stars") %></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Comments" HeaderText="Comments" />
            </Columns>
        </asp:GridView>

        <!-- Summary -->
        <div class="summary">
            Total Feedbacks: <asp:Label ID="lblTotalFeedbacks" runat="server" Text="0"></asp:Label><br />
            Average Rating: <asp:Label ID="lblAverageRating" runat="server" Text="0"></asp:Label>
        </div>
    </div>
</asp:Content>
<%@ Page Title="Appointment Reports"
    Language="C#"
    MasterPageFile="~/Doctor.Master"
    AutoEventWireup="true"
    CodeBehind="AppointmentReports.aspx.cs"
    Inherits="MetroHospitalApplication.AppointmentReports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .mb-3 { margin-bottom: 1rem; }
        .fw-bold { font-weight: bold; }
        .table th { background-color: #f1f3f5; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-3">

        <asp:HiddenField ID="hfReportId" runat="server" />

        <asp:Label ID="lblAppointment" runat="server" CssClass="fw-bold mb-3 d-block"></asp:Label>

        <div class="row mb-3">
            <div class="col-md-4">
                <label class="fw-bold">Report Type</label>
                <asp:TextBox ID="txtReportType" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-md-4">
                <label class="fw-bold">Select File</label>
                <asp:FileUpload ID="fuReport" runat="server" CssClass="form-control" />
            </div>
        </div>

        <asp:Button ID="btnUpload" runat="server" CssClass="btn btn-success" Text="Upload Report" OnClick="btnUpload_Click" />
        <asp:Label ID="lblMsg" runat="server" CssClass="fw-bold mt-2 d-block"></asp:Label>

        <hr />

        <asp:GridView ID="gvReports" runat="server"
            CssClass="table table-bordered table-hover"
            AutoGenerateColumns="False"
            DataKeyNames="ReportId"
            OnRowCommand="gvReports_RowCommand"
            EmptyDataText="No reports uploaded yet">

            <Columns>
                <asp:BoundField DataField="ReportType" HeaderText="Report Type" />
                <asp:HyperLinkField DataNavigateUrlFields="FilePath" 
                                    DataTextField="FilePath" 
                                    HeaderText="File"
                                    Target="_blank" />
                <asp:BoundField DataField="UploadedAt" HeaderText="Uploaded At" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-sm btn-danger"
                                    Text="Delete" CommandName="DeleteReport"
                                    CommandArgument='<%# Eval("ReportId") %>'
                                    OnClientClick="return confirm('Are you sure you want to delete this report?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>

        </asp:GridView>

    </div>
</asp:Content>

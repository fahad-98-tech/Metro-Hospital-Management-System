<%@ Page Title="Department Report" Language="C#" MasterPageFile="~/Admin.Master"
    AutoEventWireup="true" CodeBehind="DepartmentReport.aspx.cs"
    Inherits="MetroHospitalApplication.DepartmentReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .card {
            background: #fff;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
        }
        .form-control {
            margin-bottom: 10px;
        }
        .gridview {
            margin-top: 20px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="card">
        <h3>Department Report</h3>
        <hr />

        <div class="row mb-3">

            <div class="col-md-4">
                <asp:Label Text="Department" runat="server" />
                <asp:DropDownList ID="ddlSpecialization" CssClass="form-control" runat="server"></asp:DropDownList>
            </div>

            <div class="col-md-3">
                <asp:Label Text="From Date" runat="server" />
                <asp:TextBox ID="txtFromDate" TextMode="Date" CssClass="form-control" runat="server" />
            </div>

            <div class="col-md-3">
                <asp:Label Text="To Date" runat="server" />
                <asp:TextBox ID="txtToDate" TextMode="Date" CssClass="form-control" runat="server" />
            </div>

            <div class="col-md-2 d-flex align-items-end">
                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary me-2" OnClick="btnSearch_Click" />
                <asp:Button ID="btnExportExcel" runat="server" Text="Export to Excel" CssClass="btn btn-success" OnClick="btnExportExcel_Click" />
            </div>

        </div>

        <asp:GridView ID="gvDepartments" runat="server" CssClass="table table-bordered gridview"
            AutoGenerateColumns="false" AllowPaging="true" PageSize="10"
            OnPageIndexChanging="gvDepartments_PageIndexChanging">

            <Columns>
                <asp:BoundField DataField="Specialization" HeaderText="Department" />
                <asp:BoundField DataField="TotalDoctors" HeaderText="Total Doctors" />
                <asp:BoundField DataField="DoctorNames" HeaderText="Doctor Names" />
                <asp:BoundField DataField="FirstDoctorAdded" HeaderText="First Doctor Added" DataFormatString="{0:dd-MMM-yyyy}" />
                <asp:BoundField DataField="LastDoctorAdded" HeaderText="Last Doctor Added" DataFormatString="{0:dd-MMM-yyyy}" />
            </Columns>

        </asp:GridView>

    </div>

</asp:Content>
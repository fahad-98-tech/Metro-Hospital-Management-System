<%@ Page Title="Manage Packages" Language="C#" MasterPageFile="~/Admin.Master" 
    AutoEventWireup="true" CodeBehind="addPackage.aspx.cs" 
    Inherits="MetroHospitalApplication.addPackage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        .card { padding:20px; border-radius:8px; box-shadow:0 0 10px rgba(0,0,0,0.1); margin-bottom:20px; }
        .table th, .table td { vertical-align: middle !important; }
        .form-label { font-weight:bold; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="container mt-4">

    <div class="card">
        <h3>Manage Packages</h3>
        <hr />

        <asp:HiddenField ID="hfPackageId" runat="server" />

        <div class="row mb-2">
            <div class="col-md-4">
                <label class="form-label">Package Name</label>
                <asp:TextBox ID="txtPackageName" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-md-4">
                <label class="form-label">Price</label>
                <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-md-4">
                <label class="form-label">Duration (Days)</label>
                <asp:TextBox ID="txtDuration" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>

        <div class="row mb-2">
            <div class="col-md-4">
                <label class="form-label">Doctor</label>
                <asp:DropDownList ID="ddlDoctor" runat="server" CssClass="form-select"></asp:DropDownList>
            </div>
            <div class="col-md-4">
                <label class="form-label">Start Date</label>
                <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            </div>
            <div class="col-md-4">
                <label class="form-label">End Date</label>
                <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            </div>
        </div>

        <div class="row mb-2">
            <div class="col-md-12">
                <label class="form-label">Description</label>
                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
            </div>
        </div>

        <div class="row mb-3">
            <div class="col-md-3">
                <div class="form-check mt-2">
                    <asp:CheckBox ID="chkIsPopular" runat="server" CssClass="form-check-input" />
                    <label class="form-check-label">Is Popular</label>
                </div>
            </div>
            <div class="col-md-3">
                <div class="form-check mt-2">
                    <asp:CheckBox ID="chkIsActive" runat="server" CssClass="form-check-input" Checked="true"/>
                    <label class="form-check-label">Is Active</label>
                </div>
            </div>
        </div>

        <div class="row mb-3">
            <div class="col-md-3">
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-success w-100" OnClick="btnSave_Click" />
            </div>
            <div class="col-md-3">
                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-secondary w-100" OnClick="btnClear_Click" />
            </div>
        </div>
    </div>

    <div class="card">
        <h4>All Packages</h4>
        <hr />
        <asp:GridView ID="gvPackages" runat="server" CssClass="table table-bordered table-hover" AutoGenerateColumns="False"
            OnRowCommand="gvPackages_RowCommand">
            <Columns>
                <asp:BoundField DataField="PackageId" HeaderText="ID" />
                <asp:BoundField DataField="PackageName" HeaderText="Name" />
                <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" />
                <asp:BoundField DataField="DurationDays" HeaderText="Duration (Days)" />
                <asp:BoundField DataField="DoctorName" HeaderText="Doctor" />
                <asp:BoundField DataField="StartDate" HeaderText="Start Date" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:BoundField DataField="EndDate" HeaderText="End Date" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:BoundField DataField="IsPopular" HeaderText="Popular" DataFormatString="{0}" />
                <asp:BoundField DataField="IsActive" HeaderText="Active" DataFormatString="{0}" />
                <asp:TemplateField HeaderText="Actions">
    <ItemTemplate>
        <asp:Button ID="btnEdit" runat="server" CommandName="EditPackage" CommandArgument='<%# Eval("PackageId") %>'
            CssClass="btn btn-primary btn-sm me-1" Text="Edit" />
        <asp:Button ID="btnDelete" runat="server" CommandName="DeletePackage" CommandArgument='<%# Eval("PackageId") %>'
            CssClass="btn btn-danger btn-sm" Text="Delete"
            OnClientClick="return confirm('Are you sure you want to delete this package?');" />
    </ItemTemplate>
</asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

</div>
</asp:Content>
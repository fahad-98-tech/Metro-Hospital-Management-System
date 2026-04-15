<%@ Page Title="Health Packages" Language="C#" MasterPageFile="~/Patient.Master"
    AutoEventWireup="true" CodeBehind="Packages.aspx.cs"
    Inherits="MetroHospitalApplication.Packages" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .star { color: gold; font-size: 1.2rem; animation: blink 1s infinite alternate; }
        @keyframes blink { 0% {opacity:1;} 50% {opacity:0.3;} 100% {opacity:1;} }
        .container-packages { padding-top: 20px; }
        .search-row { margin-bottom: 15px; }
        .gvPackages th { background-color: #0d6efd; color: #fff; text-align: center; }
        .gvPackages td { vertical-align: middle; text-align: center; }
        .btn-book { background-color: #198754; color: #fff; }
        .btn-book:hover { background-color: #157347; color: #fff; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container container-packages">
        <h3 class="mb-4">Available Health Packages</h3>

        <!-- Search Row -->
        <div class="row search-row">
            <div class="col-md-6 mb-2">
                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" Placeholder="Search by package name"></asp:TextBox>
            </div>
            <div class="col-md-2 mb-2">
                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary w-100" OnClick="btnSearch_Click" />
            </div>
            <div class="col-md-2 mb-2">
                <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-secondary w-100" OnClick="btnReset_Click" />
            </div>
        </div>

        <!-- Packages Grid -->
        <asp:GridView ID="gvPackages" runat="server" AutoGenerateColumns="False"
            CssClass="table table-bordered table-striped gvPackages w-100"
            EmptyDataText="No active packages available" GridLines="None">
            <Columns>
                <asp:BoundField DataField="PackageName" HeaderText="Package Name" />
                <asp:BoundField DataField="Description" HeaderText="Description" />
                <asp:BoundField DataField="Price" HeaderText="Price ($)" DataFormatString="{0:C}" />
                <asp:BoundField DataField="DurationDays" HeaderText="Duration (Days)" />
                <asp:BoundField DataField="StartDate" HeaderText="Start Date" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:BoundField DataField="EndDate" HeaderText="End Date" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:TemplateField HeaderText="Popular">
                    <ItemTemplate>
                        <asp:Label ID="lblPopular" runat="server" Text="★"
                            ForeColor="gold" Visible='<%# Convert.ToBoolean(Eval("IsPopular")) %>' CssClass="star"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlBook" runat="server" CssClass="btn btn-sm btn-book w-100"
                            NavigateUrl='<%# "BookPackage.aspx?PackageId=" + Eval("PackageId") %>'>
                            Book Now
                        </asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
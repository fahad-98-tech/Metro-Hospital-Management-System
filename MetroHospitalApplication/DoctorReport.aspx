<%@ Page Title="Doctor Report" Language="C#" MasterPageFile="~/Admin.Master"
    AutoEventWireup="true" CodeBehind="DoctorReport.aspx.cs"
    Inherits="MetroHospitalApplication.DoctorReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .card {
            background: #ffffff;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
        }
        .form-control {
            margin-bottom: 10px;
        }
        .btn-primary {
            background-color: #007bff;
            border: none;
        }
        .gridview {
            margin-top: 20px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="card">
        <h3>Doctor Report</h3>
        <hr />

        <div class="row">

            <div class="col-md-3">
                <asp:Label Text="Specialization" runat="server" />
                <asp:DropDownList ID="ddlSpecialization" CssClass="form-control"
                    runat="server">
                </asp:DropDownList>
            </div>

            <div class="col-md-3">
                <asp:Label Text="From Date" runat="server" />
                <asp:TextBox ID="txtFromDate" TextMode="Date"
                    CssClass="form-control" runat="server" />
            </div>

            <div class="col-md-3">
                <asp:Label Text="To Date" runat="server" />
                <asp:TextBox ID="txtToDate" TextMode="Date"
                    CssClass="form-control" runat="server" />
            </div>

            <div class="col-md-3" style="margin-top:25px;">
                <asp:Button ID="btnSearch" runat="server"
                    Text="Search"
                    CssClass="btn btn-primary"
                    OnClick="btnSearch_Click" />
            </div>

        </div>

        <asp:GridView ID="gvDoctors"
            runat="server"
            CssClass="table table-bordered gridview"
            AutoGenerateColumns="false"
            AllowPaging="true"
            PageSize="10"
            OnPageIndexChanging="gvDoctors_PageIndexChanging">

            <Columns>
                <asp:BoundField DataField="DoctorId" HeaderText="ID" />
                <asp:BoundField DataField="FullName" HeaderText="Name" />
                <asp:BoundField DataField="Email" HeaderText="Email" />
                <asp:BoundField DataField="MobileNumber" HeaderText="Mobile" />
                <asp:BoundField DataField="Gender" HeaderText="Gender" />
                <asp:BoundField DataField="Age" HeaderText="Age" />
                <asp:BoundField DataField="Specialization" HeaderText="Specialization" />
                <asp:BoundField DataField="CreatedDate" HeaderText="Created Date"
                    DataFormatString="{0:dd-MMM-yyyy}" />
                <asp:CheckBoxField DataField="IsActive" HeaderText="Active" />
            </Columns>

        </asp:GridView>

    </div>

</asp:Content>

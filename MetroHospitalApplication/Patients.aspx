<%@ Page Title="Patients" Language="C#" MasterPageFile="~/Admin.Master"
    AutoEventWireup="true" CodeBehind="Patients.aspx.cs"
    Inherits="MetroHospitalApplication.Patients" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .table td, .table th {
            vertical-align: middle;
        }
        .card {
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container mt-4">


    <div class="card mb-4">
        <div class="card-header bg-primary text-white">
            <h5 class="mb-0">Add New Patient</h5>
        </div>
        <div class="card-body">

            <div class="row">
                <div class="col-md-4 mb-3">
                    <label>Full Name</label>
                    <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" />
                </div>

                <div class="col-md-4 mb-3">
                    <label>Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
                </div>

                <div class="col-md-4 mb-3">
                    <label>Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
                </div>

                <div class="col-md-3 mb-3">
                    <label>Mobile</label>
                    <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" />
                </div>

                <div class="col-md-3 mb-3">
                    <label>Gender</label>
                    <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-select">
                        <asp:ListItem Text="Select" Value="" />
                        <asp:ListItem Text="Male" Value="Male" />
                        <asp:ListItem Text="Female" Value="Female" />
                        <asp:ListItem Text="Other" Value="Other" />
                    </asp:DropDownList>
                </div>

                <div class="col-md-3 mb-3">
                    <label>Date of Birth</label>
                    <asp:TextBox ID="txtDOB" runat="server" CssClass="form-control" TextMode="Date" />
                </div>

                <div class="col-md-3 mb-3 d-flex align-items-end">
                    <asp:Button ID="btnAddPatient" runat="server"
                        Text="Add Patient"
                        CssClass="btn btn-success w-100"
                        OnClick="btnAddPatient_Click" />
                </div>
            </div>

        </div>
    </div>

   
    <h3 class="mb-3">Patient List</h3>

    <asp:GridView ID="gvPatients" runat="server"
        CssClass="table table-bordered table-hover align-middle"
        AutoGenerateColumns="False"
        DataKeyNames="UserId"
        OnRowEditing="gvPatients_RowEditing"
        OnRowCancelingEdit="gvPatients_RowCancelingEdit"
        OnRowUpdating="gvPatients_RowUpdating"
        OnRowDeleting="gvPatients_RowDeleting"
        OnRowDataBound="gvPatients_RowDataBound">

        <Columns>
            <asp:BoundField DataField="UserId" HeaderText="ID" ReadOnly="true" />

            <asp:BoundField DataField="FullName" HeaderText="Name" />

            <asp:BoundField DataField="Email" HeaderText="Email" ReadOnly="true" />

            <asp:BoundField DataField="MobileNumber" HeaderText="Mobile" />

        
            <asp:TemplateField HeaderText="Gender">
                <ItemTemplate>
                    <%# Eval("Gender") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlGenderEdit" runat="server" CssClass="form-select">
                        <asp:ListItem Text="Male" Value="Male" />
                        <asp:ListItem Text="Female" Value="Female" />
                        <asp:ListItem Text="Other" Value="Other" />
                    </asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="Age" HeaderText="Age" ReadOnly="true" />

            <asp:CommandField ShowEditButton="true"
                ButtonType="Button"
                ControlStyle-CssClass="btn btn-sm btn-warning me-1" />

            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Button ID="btnDelete" runat="server"
                        Text="Delete"
                        CssClass="btn btn-sm btn-danger"
                        CommandName="Delete"
                        OnClientClick="return confirm('Are you sure?');" />
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>

    </asp:GridView>

</div>

</asp:Content>
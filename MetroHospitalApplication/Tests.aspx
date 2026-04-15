<%@ Page Title="Tests" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="Tests.aspx.cs" Inherits="MetroHospitalApplication.Tests" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .card {
            padding:20px;
            border-radius:10px;
            box-shadow:0 2px 10px rgba(0,0,0,0.1);
            background:#fff;
        }
        .table th {
            background:#007bff;
            color:white;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h2>Test Management</h2>

    <!-- Add Test Card -->
    <div class="card">
        <div class="row">
            <div class="col-md-3">
                <asp:TextBox ID="txtTestName" runat="server" CssClass="form-control" placeholder="Test Name" />
            </div>

            <div class="col-md-3">
                <asp:TextBox ID="txtDepartment" runat="server" CssClass="form-control" placeholder="Department" />
            </div>

            <div class="col-md-2">
                <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" placeholder="Price" />
            </div>

            <div class="col-md-2">
                <asp:Button ID="btnAdd" runat="server" Text="Add Test" CssClass="btn btn-success w-100" OnClick="btnAdd_Click" />
            </div>
        </div>
    </div>

    <br />

    <!-- Grid -->
    <div class="card">
        <asp:GridView ID="gvTests" runat="server" AutoGenerateColumns="false"
            CssClass="table table-bordered table-hover"
            DataKeyNames="TestId"
            OnRowEditing="gvTests_RowEditing"
            OnRowCancelingEdit="gvTests_RowCancelingEdit"
            OnRowUpdating="gvTests_RowUpdating"
            OnRowDeleting="gvTests_RowDeleting">

            <Columns>

                <asp:BoundField DataField="TestId" HeaderText="ID" ReadOnly="true" />

                <asp:TemplateField HeaderText="Test Name">
                    <ItemTemplate>
                        <%# Eval("TestName") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditName" runat="server" Text='<%# Bind("TestName") %>' CssClass="form-control" />
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Department">
                    <ItemTemplate>
                        <%# Eval("Department") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditDept" runat="server" Text='<%# Bind("Department") %>' CssClass="form-control" />
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Price">
                    <ItemTemplate>
                        <%# Eval("Price") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditPrice" runat="server" Text='<%# Bind("Price") %>' CssClass="form-control" />
                    </EditItemTemplate>
                </asp:TemplateField>

               
                <asp:CommandField ShowEditButton="true" ButtonType="Button" ControlStyle-CssClass="btn btn-primary btn-sm" />

                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="btnDelete" runat="server"
                            CommandName="Delete"
                            CssClass="btn btn-danger btn-sm"
                            OnClientClick="return confirm('Are you sure you want to delete this test?');">
                            Delete
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
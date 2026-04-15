<%@ Page Title="Admin Messages" Language="C#" MasterPageFile="~/Admin.Master" 
    AutoEventWireup="true" CodeBehind="Messages.aspx.cs" Inherits="MetroHospitalApplication.Messages" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Bootstrap CSS CDN (optional if your master page already has it) -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />

    <style>
        .gridview {
            width: 100%;
            margin-top: 20px;
        }

        .gridview th {
            background-color: #0d6efd; /* Bootstrap primary */
            color: white;
            text-align: center;
        }

        .gridview td {
            vertical-align: middle;
            padding: 8px;
        }

        .gridview tr:nth-child(even) {
            background-color: #f8f9fa; /* light gray */
        }

        .gridview tr:hover {
            background-color: #e2e6ea; /* hover effect */
        }

        .chkRead {
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .table-container {
            overflow-x: auto;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="text-primary">Contact Messages</h2>
    <div class="table-container">
        <asp:GridView ID="gvMessages" runat="server" AutoGenerateColumns="False" CssClass="table gridview table-bordered table-striped table-hover"
            OnRowDataBound="gvMessages_RowDataBound">
            <Columns>
                <asp:BoundField DataField="MessageId" HeaderText="ID" ReadOnly="True" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Name" HeaderText="Name" />
                <asp:BoundField DataField="Email" HeaderText="Email" />
                <asp:BoundField DataField="Subject" HeaderText="Subject" />
                <asp:BoundField DataField="Message" HeaderText="Message" />
                <asp:BoundField DataField="CreatedDate" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Center" />
                <asp:TemplateField HeaderText="Read">
                    <ItemTemplate>
                        <div class="chkRead">
                            <asp:CheckBox ID="chkRead" runat="server" AutoPostBack="true"
                                OnCheckedChanged="chkRead_CheckedChanged" />
                            <asp:HiddenField ID="hfMessageId" runat="server" Value='<%# Eval("MessageId") %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
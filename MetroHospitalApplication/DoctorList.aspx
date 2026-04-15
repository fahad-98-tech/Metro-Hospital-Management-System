<%@ Page Title="Doctor List" Language="C#" MasterPageFile="~/Admin.Master"
    AutoEventWireup="true" CodeBehind="DoctorList.aspx.cs"
    Inherits="MetroHospitalApplication.DoctorList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container-fluid mt-4">
    <h3 class="mb-4 text-primary">Doctor List</h3>

    <!-- Filters -->
    <div class="row mb-3 g-2">
        <div class="col-md-4">
            <asp:TextBox ID="txtSearch" runat="server"
                CssClass="form-control"
                Placeholder="Search by Name or Email" />
        </div>
        <div class="col-md-3">
            <asp:DropDownList ID="ddlFilterSpecialization" runat="server"
                CssClass="form-select" />
        </div>
        <div class="col-md-3">
            <asp:DropDownList ID="ddlFilterStatus" runat="server"
                CssClass="form-select">
                <asp:ListItem Text="All Status" Value="" />
                <asp:ListItem Text="Active" Value="1" />
                <asp:ListItem Text="Inactive" Value="0" />
            </asp:DropDownList>
        </div>
        <div class="col-md-2">
            <asp:Button ID="btnFilter" runat="server"
                CssClass="btn btn-success w-100"
                Text="Filter"
                OnClick="btnFilter_Click" />
        </div>
    </div>

    <!-- Scrollable GridView -->
    <div class="table-responsive"
         style="max-height:450px; overflow-y:auto; border:1px solid #dee2e6; border-radius:8px;">

        <asp:GridView ID="gvDoctors" runat="server"
            AutoGenerateColumns="False"
            CssClass="table table-bordered table-hover align-middle mb-0"
            DataKeyNames="DoctorId"
            OnRowEditing="gvDoctors_RowEditing"
            OnRowCancelingEdit="gvDoctors_RowCancelingEdit"
            OnRowUpdating="gvDoctors_RowUpdating"
            OnRowDeleting="gvDoctors_RowDeleting">

            <Columns>

                <asp:BoundField DataField="DoctorId" HeaderText="ID" ReadOnly="True" />

                <asp:TemplateField HeaderText="Photo">
                    <ItemTemplate>
                        <img src='<%# Eval("DoctorImage") == DBNull.Value || string.IsNullOrEmpty(Eval("DoctorImage").ToString())
                            ? "/DoctorImages/default.png"
                            : Eval("DoctorImage").ToString() %>'
                            style="width:60px;height:60px;border-radius:50%;object-fit:cover;" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:FileUpload ID="fuEditImage" runat="server"
                            CssClass="form-control form-control-sm" />
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="FullName" HeaderText="Name" />
                <asp:BoundField DataField="Email" HeaderText="Email" />
                <asp:BoundField DataField="MobileNumber" HeaderText="Mobile" />

                <asp:TemplateField HeaderText="Gender">
                    <ItemTemplate><%# Eval("Gender") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlGenderEdit" runat="server"
                            CssClass="form-select form-select-sm">
                            <asp:ListItem>Male</asp:ListItem>
                            <asp:ListItem>Female</asp:ListItem>
                            <asp:ListItem>Other</asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Specialization">
                    <ItemTemplate><%# Eval("Specialization") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlSpecEdit" runat="server"
                            CssClass="form-select form-select-sm" />
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <span class='<%# Convert.ToBoolean(Eval("IsActive")) ? "badge bg-success" : "badge bg-danger" %>'>
                            <%# Convert.ToBoolean(Eval("IsActive")) ? "Active" : "Inactive" %>
                        </span>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlStatusEdit" runat="server"
                            CssClass="form-select form-select-sm">
                            <asp:ListItem Text="Active" Value="1" />
                            <asp:ListItem Text="Inactive" Value="0" />
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:Button runat="server" CommandName="Edit"
                            CssClass="btn btn-primary btn-sm me-1" Text="Edit" />
                        <asp:Button runat="server" CommandName="Delete"
                            CssClass="btn btn-danger btn-sm"
                            Text="Delete"
                            OnClientClick="return confirm('Delete this doctor?');" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Button runat="server" CommandName="Update"
                            CssClass="btn btn-success btn-sm me-1" Text="Update" />
                        <asp:Button runat="server" CommandName="Cancel"
                            CssClass="btn btn-secondary btn-sm" Text="Cancel" />
                    </EditItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>

    </div>

    <asp:Label ID="lblMessage" runat="server"
        CssClass="text-danger fw-semibold mt-3 d-block"></asp:Label>

</div>
</asp:Content>
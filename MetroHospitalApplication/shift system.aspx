<%@ Page Title="Doctor Shift Management" Language="C#" MasterPageFile="~/Admin.Master"
    AutoEventWireup="true" CodeBehind="shift_system.aspx.cs"
    Inherits="MetroHospitalApplication.shift_system" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .card {
            border-radius: 10px;
        }
        .table th, .table td {
            text-align: center;
            vertical-align: middle;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container mt-4">
    <div class="card shadow-lg">
        <div class="card-header bg-primary text-white">
            <h4>Doctor Shift Management</h4>
        </div>

        <div class="card-body">

            <div class="row g-3">
                <div class="col-md-4">
                    <label class="form-label fw-bold">Doctor</label>
                    <asp:DropDownList ID="ddlDoctor" runat="server" CssClass="form-select" />
                </div>

                <div class="col-md-4">
                    <label class="form-label fw-bold">Date</label>
                    <asp:TextBox ID="txtShiftDate" runat="server" CssClass="form-control" TextMode="Date" />
                </div>

                <div class="col-md-4">
                    <label class="form-label fw-bold">Shift Type</label><br />
                    <asp:RadioButton ID="rbMorning" runat="server" GroupName="ShiftType" Text="Morning" /><br />
                    <asp:RadioButton ID="rbAfternoon" runat="server" GroupName="ShiftType" Text="Afternoon" /><br />
                    <asp:RadioButton ID="rbEvening" runat="server" GroupName="ShiftType" Text="Evening" /><br />
                    <asp:RadioButton ID="rbFullDay" runat="server" GroupName="ShiftType" Text="Full Day" />
                </div>

                <div class="col-12 text-end mt-3">
                    <asp:HiddenField ID="hfShiftId" runat="server" />
                    <asp:Button ID="btnSaveShift" runat="server" Text="Save Shift"
                        CssClass="btn btn-success px-4" OnClick="btnSaveShift_Click" />
                </div>
            </div>

            <hr />

            <h5>Existing Shifts</h5>

            <asp:GridView ID="gvShifts" runat="server"
                CssClass="table table-bordered table-hover"
                AutoGenerateColumns="false"
                OnRowCommand="gvShifts_RowCommand">

                <Columns>
                    <asp:BoundField DataField="ShiftId" HeaderText="ID" />
                    <asp:BoundField DataField="ShiftDate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="DoctorName" HeaderText="Doctor" />
                    <asp:BoundField DataField="ShiftType" HeaderText="Shift" />
                    <asp:BoundField DataField="ShiftStart" HeaderText="Start" />
                    <asp:BoundField DataField="ShiftEnd" HeaderText="End" />

                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-warning btn-sm"
                                CommandName="EditShift"
                                CommandArgument='<%# Eval("ShiftId") %>'>Edit</asp:LinkButton>

                            <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-danger btn-sm"
                                CommandName="DeleteShift"
                                CommandArgument='<%# Eval("ShiftId") %>'
                                OnClientClick="return confirm('Delete this shift?');">Delete</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>

        </div>
    </div>
</div>

</asp:Content>
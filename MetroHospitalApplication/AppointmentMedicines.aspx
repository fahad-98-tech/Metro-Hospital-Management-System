<%@ Page Title="Appointment Medicines"
    Language="C#"
    MasterPageFile="~/Doctor.Master"
    AutoEventWireup="true"
    CodeBehind="AppointmentMedicines.aspx.cs"
    Inherits="MetroHospitalApplication.AppointmentMedicines" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .table th { background-color: #f1f3f5; }
        .form-inline .form-control { display: inline-block; width: auto; margin-right: 5px; }
        .mb-3 { margin-bottom: 1rem; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid mt-3">

        <!-- HiddenField to store current editing MedicineId -->
        <asp:HiddenField ID="hfMedicineId" runat="server" />

        <asp:Label ID="lblAppointment" runat="server" CssClass="fw-bold mb-3 d-block"></asp:Label>

        <div class="row mb-3">
            <div class="col-md-3">
                <label class="fw-bold">Medicine</label>
                <asp:DropDownList ID="ddlMedicineName" runat="server" CssClass="form-select"></asp:DropDownList>
                <asp:TextBox ID="txtManualMedicine" runat="server" CssClass="form-control mt-1" placeholder="Or enter manually"></asp:TextBox>
            </div>
            <div class="col-md-2">
                <label class="fw-bold">Dosage</label>
                <asp:TextBox ID="txtDosage" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-md-2">
                <label class="fw-bold">Duration</label>
                <asp:TextBox ID="txtDuration" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-md-3">
                <label class="fw-bold">Instructions</label>
                <asp:TextBox ID="txtInstructions" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-md-2 d-flex align-items-end">
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success" Text="Save" OnClick="btnSave_Click" />
            </div>
        </div>

        <asp:Label ID="lblMsg" runat="server" CssClass="fw-bold mb-3"></asp:Label>

        <asp:GridView ID="gvMedicines" runat="server"
            CssClass="table table-bordered table-hover"
            AutoGenerateColumns="False"
            DataKeyNames="MedicineId"
            OnRowCommand="gvMedicines_RowCommand"
            OnRowDataBound="gvMedicines_RowDataBound"
            EmptyDataText="No medicines added yet.">

            <Columns>
                <asp:BoundField DataField="MedicineName" HeaderText="Medicine Name" />
                <asp:BoundField DataField="Dosage" HeaderText="Dosage" />
                <asp:BoundField DataField="Duration" HeaderText="Duration" />
                <asp:BoundField DataField="Instructions" HeaderText="Instructions" />

                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:Button ID="btnEdit" runat="server"
                            Text="Edit"
                            CssClass="btn btn-sm btn-primary me-1"
                            CommandName="EditRow"
                            CommandArgument='<%# Eval("MedicineId") %>' />

                        <asp:Button ID="btnDelete" runat="server"
                            Text="Delete"
                            CssClass="btn btn-sm btn-danger"
                            CommandName="DeleteRow"
                            CommandArgument='<%# Eval("MedicineId") %>'
                            OnClientClick="return confirm('Are you sure you want to delete this medicine?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

    </div>
</asp:Content>

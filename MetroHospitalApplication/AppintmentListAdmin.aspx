<%@ Page Title="Appointment List" Language="C#" MasterPageFile="~/Admin.Master"
    AutoEventWireup="true" CodeBehind="AppintmentListAdmin.aspx.cs"
    Inherits="MetroHospitalApplication.AppintmentListAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container mt-4">

    <h3 class="mb-4">Appointment List</h3>

    <!-- FILTER CARD -->
    <div class="card mb-4">
        <div class="card-body">
            <div class="row">

                <div class="col-md-3">
                    <label>Doctor</label>
                    <asp:DropDownList ID="ddlDoctor" runat="server"
                        CssClass="form-select"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlDoctor_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>

                <div class="col-md-3">
                    <label>Specialization</label>
                    <asp:DropDownList ID="ddlSpecialization" runat="server"
                        CssClass="form-select">
                    </asp:DropDownList>
                </div>

                <div class="col-md-2">
                    <label>From Date</label>
                    <asp:TextBox ID="txtFromDate" runat="server"
                        CssClass="form-control"
                        TextMode="Date" />
                </div>

                <div class="col-md-2">
                    <label>To Date</label>
                    <asp:TextBox ID="txtToDate" runat="server"
                        CssClass="form-control"
                        TextMode="Date" />
                </div>

                <div class="col-md-2 d-flex align-items-end">
                    <asp:Button ID="btnSearch" runat="server"
                        Text="Search"
                        CssClass="btn btn-primary w-100"
                        OnClick="btnSearch_Click" />
                </div>

            </div>
        </div>
    </div>

    <!-- GRIDVIEW -->
   <asp:GridView ID="gvAppointments" runat="server"
    CssClass="table table-bordered table-hover"
    AutoGenerateColumns="False"
    OnRowDataBound="gvAppointments_RowDataBound">

    <Columns>

        <asp:BoundField DataField="AppointmentId" HeaderText="ID" />

        <asp:BoundField DataField="DoctorName" HeaderText="Doctor Name" />

        <asp:BoundField DataField="PatientNameDisplay" HeaderText="Patient Name" />

        <asp:BoundField DataField="PatientMobile" HeaderText="Mobile" />

        <asp:BoundField DataField="Specialization" HeaderText="Specialization" />

        <asp:BoundField DataField="AppointmentDate"
            HeaderText="Date"
            DataFormatString="{0:dd-MMM-yyyy}" />

        <asp:BoundField DataField="AppointmentTime" HeaderText="Start Time" />

        <asp:BoundField DataField="AppointmentEndTime" HeaderText="End Time" />

        <asp:TemplateField HeaderText="Status">
            <ItemTemplate>
                <asp:Label ID="lblStatus" runat="server"
                    Text='<%# Eval("Status") %>'
                    CssClass="badge p-2">
                </asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:BoundField DataField="IsActive" HeaderText="Active" />

        <asp:BoundField DataField="CreatedAt"
            HeaderText="Created At"
            DataFormatString="{0:dd-MMM-yyyy hh:mm tt}" />

    </Columns>

</asp:GridView>


</div>

</asp:Content>

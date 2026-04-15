<%@ Page Title="Discharge Report" Language="C#" MasterPageFile="~/Admin.Master"
    AutoEventWireup="true" CodeBehind="DischargeReport.aspx.cs"
    Inherits="MetroHospitalApplication.DischargeReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .card {
            background: #fff;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
        }
        .discharge-done {
            background-color: #d4edda !important;
            font-weight: 600;
        }
        .modalBackground {
            display:none;
            position:fixed;
            top:0; left:0; width:100%; height:100%;
            background-color: rgba(0,0,0,0.6);
            z-index:1000;
        }
        .modalPopup {
            display:none;
            position:fixed;
            top:50%; left:50%;
            transform:translate(-50%,-50%);
            background-color:white;
            padding:20px;
            width:600px;
            border-radius:8px;
            z-index:1001;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="card">
        <h3>Discharged Patients Report</h3>
        <hr />

        <asp:GridView ID="gvDischarge" runat="server"
            CssClass="table table-bordered"
            AutoGenerateColumns="false"
            OnRowDataBound="gvDischarge_RowDataBound">

            <Columns>
                <asp:BoundField DataField="AppointmentId" HeaderText="Appointment ID" />
                <asp:BoundField DataField="PatientName" HeaderText="Patient Name" />
                <asp:BoundField DataField="PatientMobile" HeaderText="Mobile" />
                <asp:BoundField DataField="Specialization" HeaderText="Department" />
                <asp:BoundField DataField="DoctorName" HeaderText="Doctor" />
                <asp:BoundField DataField="AppointmentDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}" />
                <asp:BoundField DataField="DischargeStatus" HeaderText="Discharge Status" />

                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:Button ID="btnViewAppointment" runat="server"
                            Text="View Appointment"
                            CssClass="btn btn-info btn-sm"
                            CommandArgument='<%# Eval("AppointmentId") %>'
                            OnClick="btnViewAppointment_Click" />
                        <asp:Button ID="btnViewInvoice" runat="server"
                            Text="View Invoice"
                            CssClass="btn btn-success btn-sm"
                            CommandArgument='<%# Eval("AppointmentId") %>'
                            OnClick="btnViewInvoice_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

    <div id="modalBackground" runat="server" class="modalBackground"></div>
    <asp:Panel ID="pnlAppointmentDetails" runat="server" CssClass="modalPopup">
        <h4>Appointment Details</h4>
        <hr />
        <asp:Label ID="lblDetails" runat="server" />
        <br /><br />
        <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-danger"
            OnClick="btnClose_Click" />
    </asp:Panel>

</asp:Content>
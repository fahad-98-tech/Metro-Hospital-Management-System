<%@ Page Title="Doctor Schedule" Language="C#" AutoEventWireup="true"
CodeBehind="DoctorSchedule.aspx.cs"
Inherits="MetroHospitalApplication.DoctorSchedule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<style>


    .available{
background-color:#28a745 !important;
color:white !important;
font-weight:bold;
}

.booked{
background-color:#dc3545 !important;
color:white !important;
font-weight:bold;
}

.break{
background-color:#fd7e14 !important;
color:white !important;
font-weight:bold;
}


.spec-container{
display:flex;
gap:10px;
flex-wrap:wrap;
margin-bottom:20px;
}

.spec-box{
padding:12px 20px;
border-radius:8px;
color:#fff;
cursor:pointer;
font-weight:bold;
}

.available{
background:#28a745;
color:white;
}

.booked{
background:#dc3545;
color:white;
}

.break{
background:#fd7e14;
color:white;
}

</style>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container mt-4">

<h3>Doctor Schedule</h3>

<!-- ADMIN DOCTOR DROPDOWN -->

<div class="row mb-3">

<div class="col-md-4">

<asp:Panel ID="pnlDoctorSelect" runat="server" Visible="false">

<label>Select Doctor</label>

<asp:DropDownList
ID="ddlDoctor"
runat="server"
CssClass="form-select"
AutoPostBack="true"
OnSelectedIndexChanged="ddlDoctor_SelectedIndexChanged">
</asp:DropDownList>

</asp:Panel>

</div>


<div class="col-md-4">

<label>Specialization</label>

<asp:DropDownList
ID="ddlSpecialization"
runat="server"
CssClass="form-select"
AutoPostBack="true"
OnSelectedIndexChanged="ddlSpecialization_SelectedIndexChanged">
</asp:DropDownList>

</div>


<div class="col-md-4">

<label>Date</label>

<asp:TextBox
ID="txtDate"
runat="server"
TextMode="Date"
CssClass="form-control"
AutoPostBack="true"
OnTextChanged="txtDate_TextChanged">
</asp:TextBox>

</div>

</div>


<!-- SPECIALIZATION BOXES -->

<div class="spec-container" id="specContainer" runat="server"></div>


<!-- SCHEDULE GRID -->

<asp:GridView
ID="gvFullSchedule"
runat="server"
CssClass="table table-bordered"
AutoGenerateColumns="false"
OnRowDataBound="gvFullSchedule_RowDataBound">

<Columns>

<asp:BoundField DataField="TimeSlot" HeaderText="Time"/>

<asp:BoundField DataField="Status" HeaderText="Status"/>

</Columns>

</asp:GridView>

</div>

</asp:Content>
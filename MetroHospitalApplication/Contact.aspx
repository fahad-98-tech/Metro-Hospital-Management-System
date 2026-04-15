<%@ Page Title="Contact Us" Language="C#" MasterPageFile="~/Patient.Master"
AutoEventWireup="true" CodeBehind="Contact.aspx.cs"
Inherits="MetroHospitalApplication.Contact" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<style>

.contact-box{
background:#f8f9fa;
padding:25px;
border-radius:10px;
box-shadow:0 2px 8px rgba(0,0,0,0.1);
}

.contact-title{
font-size:28px;
font-weight:bold;
margin-bottom:20px;
}

.info-box{
background:#ffffff;
padding:20px;
border-radius:10px;
box-shadow:0 2px 6px rgba(0,0,0,0.1);
}

</style>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container mt-4">

<div class="row">

<!-- CONTACT FORM -->
<div class="col-md-7">

<div class="contact-box">

<div class="contact-title">Contact Us</div>

<div class="mb-3">
<label class="form-label">Name</label>
<asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
</div>

<div class="mb-3">
<label class="form-label">Email</label>
<asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
</div>

<div class="mb-3">
<label class="form-label">Subject</label>
<asp:TextBox ID="txtSubject" runat="server" CssClass="form-control"></asp:TextBox>
</div>

<div class="mb-3">
<label class="form-label">Message</label>
<asp:TextBox ID="txtMessage" runat="server" CssClass="form-control"
TextMode="MultiLine" Rows="5"></asp:TextBox>
</div>

<asp:Button ID="btnSend" runat="server"
Text="Send Message"
CssClass="btn btn-primary"
OnClick="btnSend_Click" />

</div>

</div>


<!-- HOSPITAL INFO -->
<div class="col-md-5">

<div class="info-box">

<h4>Hospital Contact</h4>

<p><b>Address:</b><br/>
Metro Hospital<br/>
Kuwait City, Kuwait</p>

<p><b>Phone:</b><br/>
+965 9999 8888</p>

<p><b>Email:</b><br/>
info@metrohospital.com</p>

<p><b>Working Hours:</b><br/>
Sunday - Thursday<br/>
8:00 AM - 10:00 PM</p>

</div>

<br/>

<div class="info-box">

<h4>Location Map</h4>

<iframe
width="100%"
height="250"
style="border:0"
loading="lazy"
allowfullscreen
src="https://maps.google.com/maps?q=Kuwait%20City&t=&z=13&ie=UTF8&iwloc=&output=embed">
</iframe>

</div>

</div>

</div>

</div>

</asp:Content>
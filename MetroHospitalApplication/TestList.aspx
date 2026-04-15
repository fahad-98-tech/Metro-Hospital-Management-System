<%@ Page Title="Test Management" Language="C#" MasterPageFile="~/Admin.Master"
    AutoEventWireup="true" CodeBehind="TestList.aspx.cs"
    Inherits="MetroHospitalApplication.TestList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .card { border-radius: 10px; }
        .table th, .table td { text-align: center; vertical-align: middle; }
        .badge { font-size: 12px; padding: 5px 10px; }
    </style>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container mt-4">

    <!-- GRID -->
    <div class="card shadow mb-4">
        <div class="card-header bg-primary text-white">
            <h4>Patient Test Management</h4>
        </div>
        <div class="card-body">

            <asp:GridView ID="gvTests" runat="server"
                CssClass="table table-bordered table-hover"
                AutoGenerateColumns="false"
                OnRowCommand="gvTests_RowCommand">

                <Columns>
                    <asp:BoundField DataField="PatientName" HeaderText="Patient" />
                    <asp:BoundField DataField="MobileNumber" HeaderText="Mobile" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:BoundField DataField="TestName" HeaderText="Test" />
                    <asp:BoundField DataField="Amount" HeaderText="Price" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="TestDate" HeaderText="Test Date" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="TestTime" HeaderText="Test Time" />
                    <asp:BoundField DataField="Status" HeaderText="Status" />
                    <asp:BoundField DataField="Result" HeaderText="Result" />
                    <asp:BoundField DataField="PaidAmount" HeaderText="Received Amount" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="PaymentMode" HeaderText="Payment Type" />
                    <asp:BoundField DataField="PaymentDate" HeaderText="Payment Date" DataFormatString="{0:yyyy-MM-dd}" />

                    <asp:TemplateField HeaderText="Payment Status">
                        <ItemTemplate>
                            <asp:Label ID="lblPaymentStatus" runat="server"
                                CssClass='<%# Convert.ToDecimal(Eval("PaidAmount")) >= Convert.ToDecimal(Eval("Amount")) ? "badge bg-success" : "badge bg-warning" %>'
                                Text='<%# Convert.ToDecimal(Eval("PaidAmount")) >= Convert.ToDecimal(Eval("Amount")) ? "Paid" : "Pending" %>'>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:LinkButton runat="server"
                                CssClass="btn btn-success btn-sm"
                                CommandName="PayNow"
                                CommandArgument='<%# Eval("PatientTestId") %>'>Pay</asp:LinkButton>

                            <asp:LinkButton runat="server"
                                CssClass="btn btn-info btn-sm"
                                CommandName="EditRow"
                                CommandArgument='<%# Eval("PatientTestId") %>'>Edit</asp:LinkButton>

                            <asp:LinkButton runat="server"
                                CssClass="btn btn-danger btn-sm"
                                CommandName="DeleteRow"
                                CommandArgument='<%# Eval("PatientTestId") %>'
                                OnClientClick="return confirm('Delete?');">Delete</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

</div>

<!-- PAYMENT MODAL -->
<div class="modal fade" id="paymentModal" tabindex="-1">
    <div class="modal-dialog">
        <div class="modal-content">

            <div class="modal-header bg-success text-white">
                <h5>Make Payment</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
            </div>

            <div class="modal-body">

                <asp:HiddenField ID="hfPaymentTestId" runat="server" />

                <div class="mb-2">
                    <label>Received Amount</label>
                    <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" />
                </div>

                <div class="mb-2">
                    <label>Payment Type</label>
                    <asp:DropDownList ID="ddlPaymentMode" runat="server" CssClass="form-control" onchange="toggleDetails()">
                        <asp:ListItem>Cash</asp:ListItem>
                        <asp:ListItem>Card</asp:ListItem>
                        <asp:ListItem>Online</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="mb-2" id="extraDetails" style="display:none;">
                    <label>Transaction Details</label>
                    <asp:TextBox ID="txtDetails" runat="server" CssClass="form-control" />
                </div>

                <div class="mb-2">
                    <label>Payment Date</label>
                    <asp:TextBox ID="txtPaymentDate" runat="server" CssClass="form-control" TextMode="Date" />
                </div>

            </div>

            <div class="modal-footer">
                <asp:Button ID="btnSavePayment" runat="server" Text="Submit Payment" CssClass="btn btn-success" OnClick="btnSavePayment_Click" />
            </div>

        </div>
    </div>
</div>

<!-- EDIT TEST MODAL -->
<div class="modal fade" id="editModal" tabindex="-1">
    <div class="modal-dialog">
        <div class="modal-content">

            <div class="modal-header bg-info text-white">
                <h5>Edit Test Details</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
            </div>

            <div class="modal-body">

                <asp:HiddenField ID="hfEditTestId" runat="server" />

                <div class="mb-2">
                    <label>Test Date</label>
                    <asp:TextBox ID="txtEditTestDate" runat="server" CssClass="form-control" TextMode="Date" />
                </div>

                <div class="mb-2">
                    <label>Test Time</label>
                    <asp:TextBox ID="txtEditTestTime" runat="server" CssClass="form-control" />
                </div>

                <div class="mb-2">
                    <label>Status</label>
                    <asp:DropDownList ID="ddlEditStatus" runat="server" CssClass="form-control">
                        <asp:ListItem>Pending</asp:ListItem>
                        <asp:ListItem>Completed</asp:ListItem>
                        <asp:ListItem>Cancelled</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="mb-2">
                    <label>Result</label>
                    <asp:TextBox ID="txtEditResult" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                </div>

            </div>

            <div class="modal-footer">
                <asp:Button ID="btnSaveEdit" runat="server" Text="Update Test" CssClass="btn btn-info" OnClick="btnSaveEdit_Click" />
            </div>

        </div>
    </div>
</div>

<script>
    function toggleDetails() {
        var mode = document.getElementById('<%= ddlPaymentMode.ClientID %>').value;
        document.getElementById('extraDetails').style.display = (mode === 'Cash') ? 'none' : 'block';
    }
</script>

</asp:Content>
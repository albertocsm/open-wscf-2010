<%@ Page MasterPageFile="~/Shared/QuickStarts.master" Language="C#" AutoEventWireup="true" Codebehind="ApproveCustomerView.aspx.cs"
    Inherits="CustomersApproveCustomerView" Title="Approve Customers - Module QuickStart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" runat="Server">
    <h2>Customer Details</h2>
    <asp:Label ID="NoCustomersLabel" Text="There are no more customers to approve." runat="server" Visible="false" />
    <asp:Panel ID="CustomerDetailsPanel" runat="server">
        <strong>Id:</strong> <asp:Label runat="server" ID="IdLabel" /><br />
        <strong>Name:</strong> <asp:Label runat="server" ID="NameLabel" /><br />
        <strong>Last Name:</strong> <asp:Label runat="server" ID="LastNameLabel" /><br />
        <strong>Approved:</strong> <asp:Label runat="server" ID="ApprovedLabel" />
    </asp:Panel>
    <br />
    <asp:Button ID="ApproveCustomerButton" runat="server" Text="Approve customer" OnClick="ApproveCustomerButton_Click" />
</asp:Content>

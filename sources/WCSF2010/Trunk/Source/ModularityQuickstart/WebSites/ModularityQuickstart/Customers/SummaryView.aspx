<%@ Page MasterPageFile="~/Shared/QuickStarts.master" Title="Summary - Module QuickStart" Language="C#" AutoEventWireup="true" Codebehind="SummaryView.aspx.cs" Inherits="CustomersSummaryView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" runat="Server">
    <h2>Summary</h2>
    <p>You have successfully approved the following customer:</p>
    <strong>Id:</strong> <asp:Label runat="server" ID="IdLabel" /><br />
    <strong>Name:</strong> <asp:Label runat="server" ID="NameLabel" /><br />
    <strong>Last Name:</strong> <asp:Label runat="server" ID="LastNameLabel" /><br /><br />
    <asp:Button ID="ApproveAnotherCustomerButton" Text="View next customer" runat="server" OnClick="ApproveAnotherCustomerButton_Click" />
</asp:Content>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderConfirmation.ascx.cs" Inherits="Orders_OrderConfirmation" %>

<h1>
    Confirmation
</h1>
<p>
    <asp:Label ID="ConfirmationMessage" runat="server"></asp:Label>&nbsp;</p>
<p>
    <asp:LinkButton ID="CreateOrderButton" runat="server" OnClick="CreateOrderButton_Click">Create New Order</asp:LinkButton>
</p>
<p>
    <asp:HyperLink ID="HomeLink" runat="server" NavigateUrl="~/Default.aspx">Home</asp:HyperLink>
    
</p>

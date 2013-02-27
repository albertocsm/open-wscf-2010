<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderPreview.ascx.cs" Inherits="Orders_OrderPreview" %>

<table border="0" cellpadding="0" cellspacing="0" style="font-size: 10.2pt; color: #666666">
    <tr>
        <td style="width: 116px">
            Order No:</td>
        <td>
            <asp:Label ID="OrderNo" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td style="width: 116px">
            Order Name:</td>
        <td>
            <asp:Label ID="OrderName" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td style="width: 116px">
            Order Status:</td>
        <td>
            <asp:Label ID="OrderStatus" runat="server"></asp:Label></td>
    </tr>
    <tr style="color: #666666">
        <td style="width: 116px">
            Description:</td>
        <td>
            <asp:Label ID="Description" runat="server"></asp:Label></td>
    </tr>
    <tr style="color: #666666">
        <td style="width: 116px">
            Customer:</td>
        <td>
            <asp:Label ID="Customer" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td style="width: 116px">
            Approver:</td>
        <td>
            <asp:Label ID="ApprovedBy" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td style="height: 24px; width: 116px;">
            Street:</td>
        <td style="height: 24px">
            <asp:Label ID="Street" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td style="width: 116px">
            State</td>
        <td>
            <asp:Label ID="State" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td style="width: 116px">
            City:</td>
        <td>
            <asp:Label ID="City" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td style="height: 24px; width: 116px;">
            Zip:</td>
        <td style="height: 24px">
            <asp:Label ID="PostalCode" runat="server"></asp:Label></td>
    </tr>
</table>
<p>
    &nbsp;<asp:GridView ID="OrderDetailsGridView" runat="server" AutoGenerateColumns="False"
        OnRowDataBound="OrderDetailsGridView_RowDataBound" ShowFooter="True">
        <Columns>
            <asp:BoundField DataField="Sku" HeaderText="Sku">
                <HeaderStyle Width="140px" />
            </asp:BoundField>
            <asp:BoundField DataField="Name" HeaderText="Name">
                <HeaderStyle Width="200px" />
            </asp:BoundField>
            <asp:TemplateField SortExpression="Price" HeaderText="Price">
                <ItemTemplate>
                    <asp:Label ID="PriceLabel" runat="server" Text='<%# Eval("Price", "{0:C2}") %>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle Width="60px" />
            </asp:TemplateField>
            <asp:BoundField DataField="Quantity" HeaderText="Quantity">
                <HeaderStyle Width="50px" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="Total">
                <FooterTemplate>
                    <asp:Label ID="TotalLabel" runat="server" Font-Bold="True"></asp:Label><strong>
                </FooterTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Total", "{0:C2}") %>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle Width="80px" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</p>
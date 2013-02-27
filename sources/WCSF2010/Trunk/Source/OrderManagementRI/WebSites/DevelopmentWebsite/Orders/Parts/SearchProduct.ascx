<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchProduct.ascx.cs"
    Inherits="Orders_SearchProduct" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="Microsoft.Practices.Web.UI.WebControls" Namespace="Microsoft.Practices.Web.UI.WebControls"
    TagPrefix="WebControls" %>
<asp:Panel ID="content" runat="Server" Width="600px">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy" runat="server">
        <Scripts>
            <asp:ScriptReference Path="~/Orders/Parts/SearchProductBehavior.js" IgnoreScriptPath="True" />
        </Scripts>
    </asp:ScriptManagerProxy>
    <h1>
        Search Product</h1>
    <asp:UpdateProgress ID="ProductsUpdateProgress" runat="server" DisplayAfter="50"
        DynamicLayout="False">
        <ProgressTemplate>
            Retrieving product list...
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="SearchUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <ContentTemplate>
            <asp:Label ID="ProductLabel" runat="server" Text="Product:" />
            <asp:TextBox ID="ProductTextBox" runat="server" OnTextChanged="ProductTextBox_TextChanged"></asp:TextBox><br />
            Text will be searched at sku, name and description. ("<strong>s</strong>" retrieves all products).
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="ScrollPanel" runat="server" ScrollBars="Vertical" Height="300px">
        <asp:UpdatePanel ID="ProductsGridViewUpdatePanel" runat="server" UpdateMode="Always">
            <ContentTemplate>
				<asp:Label ID="NoResultsLabel" runat="server" Visible="false"></asp:Label>
                <asp:GridView ID="ProductsGridView" runat="server" AutoGenerateColumns="False" DataSourceID="ProductContainerDataSource"
                    RowStyle-CssClass="selectableObject" DataKeyNames="ProductSKU" AllowSorting="True" OnSorting="ProductsGridView_Sorting" OnRowCreated="ProductsGridView_RowCreated">
                    <Columns>
                        <asp:BoundField DataField="ProductSKU" HeaderText="SKU" SortExpression="ProductSKU">
                            <HeaderStyle Width="130px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ProductName" HeaderText="Product" SortExpression="ProductName">
                            <HeaderStyle Width="250px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description">
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>
                        <asp:TemplateField SortExpression="UnitPrice" HeaderText="Price">
                            <ItemTemplate>
                                <asp:Label ID="PriceLabel" runat="server" Text='<%# Eval("UnitPrice", "{0:C2}") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="60px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <WebControls:ObjectContainerDataSource ID="ProductContainerDataSource" runat="server"
                    DataObjectTypeName="OrdersRepository.BusinessEntities.Product">
                </WebControls:ObjectContainerDataSource>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    &nbsp;</asp:Panel>

<%@ Control Language="C#" AutoEventWireup="true" Codebehind="OrderDetails.ascx.cs"
    Inherits="Orders_OrderDetails" %>
<%@ Register Src="~/Orders/Parts/SearchProduct.ascx" TagName="SearchProductView"
    TagPrefix="search" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"  Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="Microsoft.Practices.Web.UI.WebControls" Namespace="Microsoft.Practices.Web.UI.WebControls"
    TagPrefix="WebControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>
<%@ Register Assembly="OrderManagement.CustomExtenders" Namespace="CustomExtenders" TagPrefix="ce" %>
<asp:ScriptManagerProxy ID="OrderDetailsScriptManagerProxy" runat="server">
    <Scripts>
        <asp:ScriptReference Path="~/Orders/OrderEntry/OrderDetailsBehavior.js" IgnoreScriptPath="True" />
    </Scripts>
    <Services>
        <asp:ServiceReference Path="~/Orders/ProductServiceProxy.asmx" />
    </Services>
</asp:ScriptManagerProxy>
<h1>
    Order Details
</h1>
<asp:Panel ID="HelpPanel" runat="server" Width="230px" BackColor="#FFFFC0" BorderColor="#FFC080"
    BorderStyle="Solid" BorderWidth="1px" ForeColor="Gray" Style="padding: 5px;">
    <asp:Panel ID="HelpHeaderPanel" runat="server" Height="30px" BorderStyle="Outset"
        BorderWidth="1px">
        <div style="padding: 5px; cursor: pointer; vertical-align: middle;">
            <div style="float: left;">
                Click for help</div>
        </div>
    </asp:Panel>
    <asp:Panel ID="HelpContentPanel" runat="server">
        <strong>Add:</strong> Inserts a new empty row.<br />
        <strong>Delete marked:</strong> Removes checked items.<br />
        <br />
        <strong>Previous:</strong> Shows Order general information.<br />
        <strong>Preview Order:</strong> Shows order preview before submitting it.<br />
        <strong>Save:</strong> Saves Order as draft.<br />
        <strong>Cancel:</strong> Discards Order creation or changes if an order is being
        edited.<br />
    </asp:Panel>
    <ajaxtoolkit:CollapsiblePanelExtender ID="cpe" runat="Server" TargetControlID="HelpContentPanel"
        ExpandControlID="HelpHeaderPanel" CollapseControlID="HelpHeaderPanel" Collapsed="true"
        SuppressPostBack="true" />
</asp:Panel>
<ajaxtoolkit:AlwaysVisibleControlExtender ID="avce" runat="server" TargetControlID="HelpPanel"
    VerticalSide="Top" VerticalOffset="5" HorizontalSide="Right" HorizontalOffset="0"
    ScrollEffectDuration=".1" />
<asp:Panel ID="Panel1" runat="server" BorderColor="Black" BorderWidth="1px">
    <asp:UpdatePanel ID="OrderItemsGridUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel Style="display: none" ID="SearchProductModalPopupPanel" runat="server"
                BorderStyle="Outset" BackColor="white">
                <search:SearchProductView ID="searchProductView" runat="server"></search:SearchProductView>
                <asp:ImageButton ID="SelectButton" OnClick="SelectButton_Click" runat="server" CausesValidation="False"
                    ImageUrl="~/Images/select_light.png" />
                <asp:ImageButton ID="CancelSearchButton" runat="server" OnClick="CancelSearchButton_Click"
                    ImageUrl="~/Images/cancel_light.png" />
            </asp:Panel>
            <asp:GridView ID="OrderDetailsGridView" runat="server" DataSourceID="OrderItemContainerDataSource"
                ShowFooter="True" RowStyle-CssClass="selectableObject" AutoGenerateColumns="False"
                OnRowDataBound="OrderDetailsGridView_RowDataBound" OnRowEditing="OrderDetailsGridView_RowEditing"
                OnRowCreated="OrderDetailsGridView_RowCreated" DataKeyNames="Id">
                <Columns>
                    <asp:BoundField DataField="Id" Visible="False"></asp:BoundField>
                    <asp:TemplateField SortExpression="Sku" HeaderText="Sku">
                        <EditItemTemplate>
                            <asp:TextBox ID="SkuTextBox" runat="server" Text='<%# Bind("Sku") %>' MaxLength="15"
                                Width="79px"></asp:TextBox>
                            <asp:ImageButton ID="SearchSkuButton" runat="server" ImageUrl="~/Images/ZoomHS.png" />
                            <asp:RegularExpressionValidator ID="SkuRegexValidator" runat="server" ControlToValidate="SkuTextBox"
                                Display="Dynamic" Text="*" ErrorMessage="The sku number entered is invalid" ValidationExpression="^[\w',.\s\-]{1,700}$"></asp:RegularExpressionValidator>
                            <asp:CustomValidator ID="SkuCustomValidator" runat="server" Display="Dynamic" ControlToValidate="SkuTextBox"
                                Text="*" OnServerValidate="SkuCustomValidator_ServerValidate">
                            </asp:CustomValidator>
                            <ajaxtoolkit:ModalPopupExtender ID="ProductModalPopupExtender" runat="server" TargetControlID="SearchSkuButton"
                                BackgroundCssClass="modalBackground" PopupControlID="SearchProductModalPopupPanel"
                                DropShadow="true" OkControlID="SelectButton">
                            </ajaxtoolkit:ModalPopupExtender>
                            <ce:ClientScriptCallExtender ID="SkuTextBoxCallExtender" CustomScript="GetProduct"
                                TargetControlID="SkuTextBox" runat="server">
                                <CustomScriptParameters>
                                    <ce:CustomScriptParameter ControlId="ProductNameLabel" />
                                    <ce:CustomScriptParameter ControlId="PriceLabel" />
                                </CustomScriptParameters>
                            </ce:ClientScriptCallExtender>
                        </EditItemTemplate>
                        <ItemStyle Wrap="False"></ItemStyle>
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("Sku") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="170px" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Name" HeaderText="Name">
                        <EditItemTemplate>
                            <asp:Label ID="ProductNameLabel" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="ProductNameLabel" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="200px" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Price" HeaderText="Price">
                        <EditItemTemplate>
                            <asp:Literal ID="Literal1" runat="server" Text='<%# System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol %>'>
                            </asp:Literal><asp:Label ID="PriceLabel" runat="server" Text='<%# Eval("Price") %>'></asp:Label>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="PriceLabel" runat="server" Text='<%# Eval("Price", "{0:C2}") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="70px" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Quantity" HeaderText="Quantity" ItemStyle-Wrap="false">
                        <EditItemTemplate>
                            <ce:ClientScriptCallExtender ID="QtyTextBoxCallExtender" CustomScript="CalculateTotal"
                                TargetControlID="QtyTextBox" runat="server">
                                <CustomScriptParameters>
                                    <ce:CustomScriptParameter ControlId="PriceLabel" />
                                    <ce:CustomScriptParameter ControlId="ItemTotalLabel" />
                                    <ce:CustomScriptParameter Value="$" />
                                </CustomScriptParameters>
                            </ce:ClientScriptCallExtender>
                            <asp:TextBox ID="QtyTextBox" runat="server" Text='<%# Bind("Quantity") %>' Columns="5"></asp:TextBox>
                            <asp:RangeValidator ID="QuantityRangeValidator" ControlToValidate="QtyTextBox" runat="server"
                                Text="*" ErrorMessage="The quantity must be an integer value" Type="Integer"
                                Display="Dynamic" MinimumValue="1" MaximumValue="32767"></asp:RangeValidator>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("Quantity") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="50px" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Total" HeaderText="Total">
                        <EditItemTemplate>
                            <asp:Label ID="ItemTotalLabel" runat="server" Text='<%# Eval("Total", "{0:C2}") %>' />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="TotalLabel" runat="server" Text="0" />
                        </FooterTemplate>
                        <ItemTemplate>
                            <asp:Label ID="ItemTotalLabel" runat="server" Text='<%# Eval("Total", "{0:C2}") %>' />
                        </ItemTemplate>
                        <HeaderStyle Width="90px" />
                    </asp:TemplateField>
                    <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" ButtonType="Image"
                        DeleteImageUrl="~/Images/DeleteHS.png" CancelImageUrl="~/Images/Edit_UndoHS.png"
                        EditImageUrl="~/Images/EditTableHS.png" UpdateImageUrl="~/Images/SychronizeListHS.png">
                    </asp:CommandField>
                    <asp:TemplateField SortExpression="Selected">
                        <EditItemTemplate>
                            <asp:CheckBox ID="SelectedCheckBox" runat="server" Checked='<%# Eval("Selected") %>' />
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                        <ItemTemplate>
                            <asp:CheckBox ID="SelectedCheckBox" runat="server" Checked='<%# Eval("Selected") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    No items added
                </EmptyDataTemplate>
            </asp:GridView>
            <WebControls:ObjectContainerDataSource ID="OrderItemContainerDataSource" runat="server"
                OnUpdated="OrderItemContainerDataSource_Updated" OnDeleted="OrderItemContainerDataSource_Deleted"
                DataObjectTypeName="Orders.PresentationEntities.OrderItemLine"></WebControls:ObjectContainerDataSource>
            <p>
                <table>
                    <tr>
                        <td>
                            <asp:ImageButton ID="AddOrderItemLineButton" runat="server" OnCommand="AddOrderItemLineButton_Command"
                                CausesValidation="true" ImageUrl="~/Images/add_light.png" />
                            <asp:ImageButton ID="DeleteOrderItemLineButton" runat="server" OnCommand="DeleteOrderItemLineButton_Command"
                                ImageUrl="~/Images/delete_marked_light.png" />
                        </td>
                        <td>
                            <asp:ValidationSummary ID="ValidationSummary" DisplayMode="List" runat="server" />
                        </td>
                    </tr>
                </table>
            </p>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="DeleteOrderItemLineButton" EventName="Command" />
            <asp:AsyncPostBackTrigger ControlID="AddOrderItemLineButton" EventName="Command" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Panel>
<br />
<br />
<table border="0">
    <tr>
        <td style="width: 125px">
            <asp:ImageButton ID="PreviousButton" runat="server" OnClick="PreviousButton_Click"
                SkinID="NavButton" ImageUrl="~/Images/previous_light.png" />
        </td>
        <td style="width: 125px">
            <asp:ImageButton ID="PreviewOrderButton" runat="server" OnClick="NextButton_Click"
                SkinID="NavButtonDefault" ImageUrl="~/Images/preview_order_light.png" />
        </td>
        <td style="width: 125px">
            <asp:ImageButton ID="SaveButton" runat="server" OnClick="SaveButton_Click" SkinID="NavButton"
                ImageUrl="~/Images/save_light.png" />
        </td>
        <td style="width: 125px">
            <asp:ImageButton ID="CancelButton" runat="server" OnClick="CancelButton_Click" SkinID="NavButton"
                OnClientClick="javascript:document.forms[0].reset();" CausesValidation="False"
                ImageUrl="~/Images/cancel_light.png" />
        </td>
    </tr>
</table>

<%@ Page Language="C#" AutoEventWireup="true" Codebehind="SearchOrders.aspx.cs" Inherits="Orders_SearchOrders"
    Title="Search Orders" MasterPageFile="~/Shared/Default.master" %>

<%@ Register Src="~/Orders/Parts/OrderPreview.ascx" TagName="OrderPreview" TagPrefix="uc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="Microsoft.Practices.Web.UI.WebControls" Namespace="Microsoft.Practices.Web.UI.WebControls"
    TagPrefix="WebControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <asp:ScriptManagerProxy ID="SearchOrderScriptManagerProxy" runat="server">
    </asp:ScriptManagerProxy>
    <h1>
        Search Orders</h1>
    <asp:Label ID="OrderLabel" runat="server" Text="Search:"></asp:Label>
    <asp:TextBox ID="SearchTextBox" runat="server"></asp:TextBox>
    <asp:ImageButton ID="SearchButton" runat="server" OnClick="SearchButton_Click" ImageUrl="~/Images/search_light.png"
        ImageAlign="AbsMiddle" />
    <asp:RegularExpressionValidator ID="SearchRegularExpressionValidator" runat="server"
        ControlToValidate="SearchTextBox" ErrorMessage="Invalid search query string"
        ValidationExpression="^[\w',.\s\-]+$"></asp:RegularExpressionValidator>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="SearchButton" EventName="click" />
        </Triggers>
        <ContentTemplate>
            <br />
            <asp:GridView ID="OrdersGridView" runat="server" AutoGenerateColumns="False" DataSourceID="OrdersContainerDataSource"
                OnRowCommand="OrdersGridView_RowCommand" DataKeyNames="OrderId" AllowPaging="true"
                PageSize="3" EnableViewState="false">
                <PagerSettings FirstPageText="<<" PreviousPageText="<" NextPageText=">" LastPageText=">>"
                    Position="Bottom" Visible="true" Mode="Numeric" />
                <Columns>
                    <asp:ButtonField CommandName="ShowOrderDetails" HeaderText="Details" Text="<span style='text-decoration: none;'>[+]</span>" />
                    <asp:BoundField DataField="OrderId" HeaderText="ID" SortExpression="OrderId" />
                    <asp:BoundField DataField="OrderName" HeaderText="Name" SortExpression="OrderName" />
                    <asp:TemplateField SortExpression="OrderTotal" HeaderText="Amount">
                        <ItemTemplate>
                            <asp:Label ID="AmountLabel" runat="server" Text='<%# Eval("OrderTotal", "{0:C2}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="OrderStatus" HeaderText="Status" SortExpression="OrderStatus" />
                    <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                </Columns>
            </asp:GridView>
            <br />
            <asp:Panel ID="PagesPanel" runat="server">
                Search results for:
                <asp:Label ID="SearchedTextLabel" runat="server" Font-Bold="True" />
                <br />
                Page
                <asp:Label ID="ActualPage" Text="1" runat="server" />
                of
                <asp:Label ID="TotalPages" Text="9" runat="server" />
                <asp:TextBox ID="GoToPageTextBox" runat="server" Width="30px" EnableViewState="False" />&nbsp;
                <asp:ImageButton ID="GoToPageButton" runat="server" OnClick="GoToPageButton_Click"
                    ValidationGroup="Paging" ImageUrl="~/Images/go_light.png" ImageAlign="AbsMiddle" />
                <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="GoToPageTextBox"
                    Display="Dynamic" ErrorMessage="*" MaximumValue="99999999" MinimumValue="1" Type="Integer"
                    ValidationGroup="Paging"></asp:RangeValidator><asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                        runat="server" ControlToValidate="GoToPageTextBox" Display="Dynamic" ErrorMessage="*"
                        ValidationGroup="Paging"></asp:RequiredFieldValidator></asp:Panel>
            <asp:Label ID="NullControl" runat="server" /><%-- Non operational control required for the TargetControlID property of popup extender --%>
            <ajax:ModalPopupExtender ID="DetailsModalPopupExtender" runat="server" TargetControlID="NullControl"
                PopupControlID="DetailsPopupPanel" OkControlID="OkButton" DropShadow="true" BackgroundCssClass="modalBackground" />
            <asp:Panel ID="DetailsPopupPanel" runat="server" Width="570px" BorderStyle="Outset"
                BackColor="white">
                <div>
                    <uc1:OrderPreview ID="OrderPreviewPart" runat="server" />
                </div>
                <div style="text-align: center; padding: 10px;">
                    <asp:ImageButton ID="OkButton" runat="server" Width="120px" ImageUrl="~/Images/ok_light.png" />
                </div>
            </asp:Panel>
            <WebControls:ObjectContainerDataSource ID="OrdersContainerDataSource" runat="server"
                DataObjectTypeName="Orders.PresentationEntities.OrderInfo" UsingServerPaging="true"
                OnSelecting="OrdersContainerDataSource_Selecting"></WebControls:ObjectContainerDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="OrdersUpdateProgress" runat="server" DisplayAfter="10" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            Retrieving order list...
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>

<%@ Control Language="C#" AutoEventWireup="true" Codebehind="SavedDrafts.ascx.cs"
	Inherits="Orders_SavedDrafts" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
	Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="OrderPreview.ascx" TagName="OrderPreview" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
	<ContentTemplate>
		<asp:GridView ID="MySavedDraftsGridView" runat="server" AutoGenerateColumns="False"
			OnRowCommand="MySavedDraftsGridView_RowCommand" DataKeyNames="OrderId">
			<Columns>
				<asp:BoundField DataField="OrderId" HeaderText="ID" />
				<asp:BoundField DataField="OrderName" HeaderText="Name" />
				<asp:TemplateField HeaderText="Amount">
                    <ItemTemplate>
                        <asp:Label ID="AmountLabel" runat="server" Text='<%# Eval("OrderTotal", "{0:C2}") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
				<asp:BoundField DataField="Description" HeaderText="Description" />
				<asp:ButtonField CommandName="EditOrder" Text="Edit" />
				<asp:ButtonField CommandName="DeleteOrder" Text="Delete" />
			</Columns>
			<EmptyDataTemplate>
				<emptydatatemplate>
</emptydatatemplate>
				No saved draft orders.
			</EmptyDataTemplate>
		</asp:GridView>
		<asp:Label ID="NullControl" runat="server" /><%-- Non operational control required for the TargetControlID property of popup extender --%>
		<ajax:ModalPopupExtender ID="DetailsModalPopupExtender" runat="server" TargetControlID="NullControl"
			PopupControlID="DetailsPopupPanel" DropShadow="true" BackgroundCssClass="modalBackground"
			CancelControlID="CloseButton" />
		<asp:Panel ID="DetailsPopupPanel" runat="server" Width="570px" BorderStyle="Outset"
			BackColor="white">
			<table border="0">
				<tr>
					<td>
						<uc1:OrderPreview ID="OrderPreviewPart" runat="server" />
					</td>
				</tr>
				<tr>
					<td align="center">
						<asp:ImageButton ID="CloseButton" runat="server" ImageUrl="~/Images/close_light.png" />
						<asp:ImageButton ID="EditOrderButton" runat="server" OnCommand="EditDeleteOrder_Command"
							CommandName="EditOrder" ImageUrl="~/Images/edit_light.png"/>
						<asp:ImageButton ID="DeleteOrderButton" runat="server" OnCommand="EditDeleteOrder_Command"
							CommandName="DeleteOrder" ImageUrl="~/Images/delete_light.png" />
					</td>
				</tr>
			</table>
		</asp:Panel>
	</ContentTemplate>
</asp:UpdatePanel>

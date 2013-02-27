<%@ Control Language="C#" AutoEventWireup="true" Codebehind="SearchCustomer.ascx.cs"
	Inherits="Customers_SearchCustomer" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
	Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="Microsoft.Practices.Web.UI.WebControls" Namespace="Microsoft.Practices.Web.UI.WebControls"
	TagPrefix="WebControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:ScriptManagerProxy ID="SearchCustomerScriptManagerProxy" runat="server">
</asp:ScriptManagerProxy>
<asp:Panel ID="Panel1" runat="server">
	<table>
		<tr>
			<td>
				<asp:Label ID="CompanyNameLabel" runat="server" Text="Company Name:"></asp:Label>
			</td>
			<td>
				<asp:TextBox ID="CompanyNameTextBox" runat="server" />
				<asp:RegularExpressionValidator ID="CompanyNameRegexValidator" runat="server" ControlToValidate="CompanyNameTextBox"
					ErrorMessage="Invalid search query string" ValidationExpression="^[\w',.\s\-]+$"
					ValidationGroup="SearchCustomerValidation" />
			</td>
		</tr>
		<tr>
			<td>
				<asp:Label ID="CityLabel" runat="server" Text="City:"></asp:Label>
			</td>
			<td>
				<asp:TextBox ID="CityTextBox" runat="server"></asp:TextBox>
				<asp:RegularExpressionValidator ID="CityRegexValidator" runat="server" ControlToValidate="CityTextBox"
					ErrorMessage="Invalid search query string" ValidationExpression="^[\w',.\s\-]+$"
					ValidationGroup="SearchCustomerValidation" />
			</td>
		</tr>
		<tr>
			<td>
				<asp:Label ID="StateLabel" runat="server" Text="State:"></asp:Label>
			</td>
			<td>
				<asp:DropDownList ID="StateDropDown" runat="server" AppendDataBoundItems="True" DataTextField="Name"
					DataValueField="Id">
					<asp:ListItem Selected="True"></asp:ListItem>
				</asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td>
				<asp:Label ID="ZipCodeLabel" runat="server" Text="Postal Code:"></asp:Label>
			</td>
			<td>
				<asp:TextBox ID="ZipCodeTextBox" runat="server"></asp:TextBox>
				<asp:RegularExpressionValidator ID="ZipCodeRegexValidator" runat="server" ControlToValidate="ZipCodeTextBox"
					ErrorMessage="Invalid search query string" ValidationExpression="^[\w\s\-]+$"
					ValidationGroup="SearchCustomerValidation" />
			</td>
		</tr>
		<tr>
			<td>
				<asp:Label ID="AddressLabel" runat="server" Text="Address:"></asp:Label>
			</td>
			<td>
				<asp:TextBox ID="AddressTextBox" runat="server"></asp:TextBox>
				<asp:RegularExpressionValidator ID="AddressRegexValidator" runat="server" ControlToValidate="AddressTextBox"
					ErrorMessage="Invalid search query string" ValidationExpression="^[\w',.\s\-]+$"
					ValidationGroup="SearchCustomerValidation" />
			</td>
		</tr>
	</table>
</asp:Panel>
<asp:ImageButton ID="SearchButton" runat="server" OnClick="SearchButton_Click" ValidationGroup="SearchCustomerValidation"
	ImageUrl="~/Images/search_light.png" />
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="True">
	<Triggers>
		<asp:AsyncPostBackTrigger ControlID="SearchButton" EventName="click" />
	</Triggers>
	<ContentTemplate>
		<br />
		<asp:Label ID="NoResultsLabel" runat="server" Visible="false"></asp:Label>
		<asp:GridView ID="CustomersGridView" runat="server" AutoGenerateColumns="False" DataSourceID="CustomersContainerDataSource"
			RowStyle-CssClass="selectableObject" DataKeyNames="CustomerId" OnRowCreated="CustomersGridView_RowCreated">
			<Columns>
				<asp:BoundField DataField="CustomerId" HeaderText="ID" SortExpression="CustomerId" />
				<asp:BoundField DataField="CompanyName" HeaderText="Company Name" SortExpression="CompanyName" />
				<asp:BoundField DataField="City" HeaderText="City" SortExpression="City" />
				<asp:BoundField DataField="Region" HeaderText="State" SortExpression="Region" />
				<asp:BoundField DataField="PostalCode" HeaderText="Zip Code" SortExpression="PostalCode" />
				<asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
			</Columns>
		</asp:GridView>
		<br />
		<WebControls:ObjectContainerDataSource ID="CustomersContainerDataSource" runat="server"
			DataObjectTypeName="OrdersRepository.BusinessEntities.Customer"></WebControls:ObjectContainerDataSource>
	</ContentTemplate>
</asp:UpdatePanel>

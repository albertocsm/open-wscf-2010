<%@ Control Language="C#" AutoEventWireup="true" Codebehind="OrderInformation.ascx.cs"
	Inherits="Orders_OrderInformation" %>
<%@ Register Src="~/SharedUserControls/SearchCustomer.ascx" TagName="SearchCustomer"
	TagPrefix="customermodule" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
	Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>
<%@ Register Assembly="AjaxControlToolkit.WCSFExtensions" Namespace="AjaxControlToolkit.WCSFExtensions"
	TagPrefix="ajaxtoolkitwcsfextensions" %>
<h1>
	Order Information</h1>
<table border="0" cellpadding="0" cellspacing="0" style="font-size: 10.2pt; color: #666666">
	<tr>
		<td style="width: 106px">
			<span class="inputDescriptor">Order No:</span>
		</td>
		<td>
			<asp:TextBox ID="OrderNoTextBox" runat="server" ReadOnly="True" Width="320px"></asp:TextBox>
		</td>
	</tr>
	<tr>
		<td style="width: 106px">
			<span class="inputDescriptor">Order Name:</span>
		</td>
		<td>
			<asp:TextBox ID="OrderNameTextBox" runat="server" Width="320px" MaxLength="50"></asp:TextBox>
			<asp:RequiredFieldValidator ID="OrderNameRequiredValidator" runat="server" ControlToValidate="OrderNameTextBox"
				Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
					ID="OrderNameRegexValidator" runat="server" Display="Dynamic" ErrorMessage="*"
					ValidationExpression="^[\w',.\s\-]{1,50}$" ControlToValidate="OrderNameTextBox"></asp:RegularExpressionValidator></td>
	</tr>
	<tr>
		<td style="width: 106px; height: 166px;">
			<span class="inputDescriptor">Description:</span>
		</td>
		<td style="height: 166px">
			<asp:TextBox ID="DescriptionTextArea" runat="server" Rows="10" TextMode="MultiLine"
				MaxLength="700" Width="320px"></asp:TextBox>
			<asp:RegularExpressionValidator ID="DescriptionRegexValidator" runat="server" ControlToValidate="DescriptionTextArea"
				Display="Dynamic" ErrorMessage="*" ValidationExpression="^[\w',.\s\-]{1,700}$"></asp:RegularExpressionValidator></td>
	</tr>
	<tr>
		<td style="width: 106px">
			<span class="inputDescriptor">Customer:</span>
		</td>
		<td>
			<asp:UpdatePanel ID="CustomerUpdatePanel" runat="server" RenderMode="Inline" UpdateMode="Conditional"
				ChildrenAsTriggers="False">
				<ContentTemplate>
					<asp:TextBox ID="customerTextBox" runat="server" Width="320px"></asp:TextBox>
					<asp:ImageButton ID="SearchCustomerButton" runat="server" ImageUrl="~/Images/ZoomHS.png"
						CausesValidation="False" />
					<ajaxtoolkit:ModalPopupExtender ID="CustomerModalPopupExtender" runat="server" TargetControlID="SearchCustomerButton"
						BackgroundCssClass="modalBackground" PopupControlID="SearchCustomerModalPopupPanel"
						OkControlID="SelectButton" DropShadow="true">
					</ajaxtoolkit:ModalPopupExtender>
					<asp:Panel Style="display: none" ID="SearchCustomerModalPopupPanel" runat="server"
						BorderStyle="Outset" BackColor="white">
						<customermodule:SearchCustomer ID="SearchCustomerControl" runat="server" />
						<asp:ImageButton ID="SelectButton" OnClick="SelectButton_Click" runat="server" 
							CausesValidation="False" ImageUrl="~/Images/select_light.png" OnClientClick="javascript:ClearSearchCustomerView();" />
						<asp:ImageButton ID="CancelSearchButton" runat="server" OnClick="CancelSearchButton_Click"
							CausesValidation="False" ImageUrl="~/Images/cancel_light.png" OnClientClick="javascript:ClearSearchCustomerView();" /></asp:Panel>
					<asp:RequiredFieldValidator ID="CustomerRequiredValidator" runat="server" ControlToValidate="customerTextBox"
						Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator><asp:CustomValidator
							ID="ExistenceCustomerValidator" runat="server" ControlToValidate="customerTextBox"
							ErrorMessage="Invalid customer" OnServerValidate="ExistenceCustomerValidator_ServerValidate"></asp:CustomValidator>
					<ajaxtoolkitwcsfextensions:ServerSideValidationExtender ID="CustomerServerSideValidation"
						runat="server" TargetControlID="ExistenceCustomerValidator" />
					<ajaxtoolkit:AutoCompleteExtender ID="CustomerAutoComplete" runat="server" TargetControlID="customerTextBox"
						CompletionSetCount="30" CompletionInterval="400" MinimumPrefixLength="1" ServiceMethod="GetCustomersName"
						ServicePath="~/Customers/CustomerAutoCompleteService.asmx" EnableCaching="true">
					</ajaxtoolkit:AutoCompleteExtender>
				</ContentTemplate>
				<Triggers>
					<asp:AsyncPostBackTrigger ControlID="SelectButton" />
					<asp:AsyncPostBackTrigger ControlID="CancelSearchButton" />
					<asp:AsyncPostBackTrigger ControlID="SearchCustomerButton" />
				</Triggers>
			</asp:UpdatePanel>
		</td>
	</tr>
	<tr>
		<td style="width: 106px">
			<span class="inputDescriptor">Approver:</span>
		</td>
		<td>
			<asp:DropDownList ID="ApprovedByDropDown" runat="server" Width="320px" AppendDataBoundItems="True"
				DataTextField="Name" DataValueField="Id">
				<asp:ListItem></asp:ListItem>
			</asp:DropDownList>
			<asp:RequiredFieldValidator ID="ApproverRequiredValidator" runat="server" ControlToValidate="ApprovedByDropDown"
				Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator><asp:CustomValidator
					ID="ExistenceApproverValidator" runat="server" ControlToValidate="ApprovedByDropDown"
					Display="Dynamic" ErrorMessage="Invalid approver" OnServerValidate="ExistenceApproverValidator_ServerValidate"></asp:CustomValidator></td>
	</tr>
	<tr>
		<td style="height: 24px; width: 106px;">
			<span class="inputDescriptor">Street:</span>
		</td>
		<td style="height: 24px">
			<asp:TextBox ID="StreetTextBox" runat="server" MaxLength="60" Width="320px" Rows="2"
				TextMode="MultiLine"></asp:TextBox>
			<asp:RequiredFieldValidator ID="StreetRequiredValidator" runat="server" ControlToValidate="StreetTextBox"
				Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
					ID="StreetRegexValidator" runat="server" ControlToValidate="StreetTextBox" Display="Dynamic"
					ErrorMessage="*" ValidationExpression="^[\w',.\s\-]{1,60}$"></asp:RegularExpressionValidator></td>
	</tr>
	<tr>
		<td style="width: 106px; height: 22px;">
			<span class="inputDescriptor">State:</span>
		</td>
		<td style="height: 22px">
			<asp:DropDownList ID="StateDropDown" runat="server" AppendDataBoundItems="True" DataTextField="Name"
				DataValueField="Id" Width="320px">
				<asp:ListItem Selected="True"></asp:ListItem>
			</asp:DropDownList>
			<asp:RequiredFieldValidator ID="StateRequiredValidator" runat="server" ControlToValidate="StateDropDown"
				Display="Dynamic" ErrorMessage="*" Width="5px"></asp:RequiredFieldValidator><asp:CustomValidator
					ID="ExistenceStateValidator" runat="server" ControlToValidate="StateDropDown"
					Display="Dynamic" ErrorMessage="Invalid state" OnServerValidate="ExistenceStateValidator_ServerValidate"></asp:CustomValidator></td>
	</tr>
	<tr>
		<td style="width: 106px">
			<span class="inputDescriptor">City:</span>
		</td>
		<td>
			<asp:TextBox ID="CityTextBox" runat="server" Width="320px"></asp:TextBox>
			<asp:RequiredFieldValidator ID="CityRequiredValidator" runat="server" ControlToValidate="CityTextBox"
				Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
					ID="CityRegexValidator" runat="server" ControlToValidate="CityTextBox" Display="Dynamic"
					ErrorMessage="*" ValidationExpression="^[\w',.\s\-]{1,50}$"></asp:RegularExpressionValidator>
			<ajaxtoolkitwcsfextensions:ContextSensitiveAutoCompleteExtender ID="CityAutoComplete"
				runat="server" TargetControlID="CityTextBox" CompletionSetCount="30" CompletionInterval="400"
				MinimumPrefixLength="1" ServiceMethod="GetCities" ServicePath="../../PostalCodeAutoCompleteService.asmx"
				EnableCaching="true">
				<CompletionContextItems>
					<ajaxtoolkitwcsfextensions:CompletionContextItem Key="State" ControlId="StateDropDown" />
				</CompletionContextItems>
			</ajaxtoolkitwcsfextensions:ContextSensitiveAutoCompleteExtender>
		</td>
	</tr>
	<tr>
		<td style="height: 24px; width: 106px;">
			<span class="inputDescriptor">Zip:</span>
		</td>
		<td style="height: 24px">
			<asp:TextBox ID="PostalCodeTextBox" runat="server" Width="320px"></asp:TextBox>
			<asp:RequiredFieldValidator ID="PostalCodeRequiredValidator" runat="server" ControlToValidate="PostalCodeTextBox"
				Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
			<asp:RegularExpressionValidator ID="PostalCodeRegexValidator" runat="server" ControlToValidate="PostalCodeTextBox"
				Display="Dynamic" ErrorMessage="*" ValidationExpression="^(\d{5}-\d{4}|\d{5}|\d{9})$|^([a-zA-Z]\d[a-zA-Z] \d[a-zA-Z]\d)$"></asp:RegularExpressionValidator>
			<ajaxtoolkitwcsfextensions:ContextSensitiveAutoCompleteExtender ID="PostalCodeAutoComplete"
				runat="server" TargetControlID="PostalCodeTextBox" CompletionSetCount="30" CompletionInterval="400"
				MinimumPrefixLength="1" ServiceMethod="GetZipCodes" ServicePath="../../PostalCodeAutoCompleteService.asmx"
				EnableCaching="true">
				<CompletionContextItems>
					<ajaxtoolkitwcsfextensions:CompletionContextItem Key="State" ControlId="StateDropDown" />
					<ajaxtoolkitwcsfextensions:CompletionContextItem Key="City" ControlId="CityTextBox" />
				</CompletionContextItems>
			</ajaxtoolkitwcsfextensions:ContextSensitiveAutoCompleteExtender>
		</td>
	</tr>
</table>
<table border="0">
	<tr>
		<td style="width: 125px">
		</td>
		<td style="width: 125px">
			<asp:ImageButton ID="NextButton" runat="server" OnClick="NextButton_Click" SkinID="NavButtonDefault"
				ImageUrl="~/Images/next_light.png" />
		</td>
		<td style="width: 125px">
			<asp:ImageButton ID="SaveButton" runat="server" OnClick="SaveButton_Click" SkinID="NavButton"
				ImageUrl="~/Images/save_light.png" />
		</td>
		<td style="width: 125px">
			<asp:ImageButton ID="CancelButton" runat="server" OnClick="CancelButton_Click" SkinID="NavButton"
				CausesValidation="False" ImageUrl="~/Images/cancel_light.png" OnClientClick="javascript:document.forms[0].reset();" />
		</td>
	</tr>
</table>

<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPages/Module.master" AutoEventWireup="True"
	CodeBehind="NewTransferView.aspx.cs" Inherits="EFT_NewTransferView"
	Title="Enter Transfer" %>

<%@ Register Assembly="Microsoft.Practices.Web.UI.WebControls" Namespace="Microsoft.Practices.Web.UI.WebControls"
	TagPrefix="pp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="moduleActionContent" runat="Server">
	<div id="contentTitle" class="sectionTitle">
		Enter Transfer</div>
	<div id="moduleContentContent">
		<pp:ObjectContainerDataSource DataObjectTypeName="GlobalBank.Commercial.EBanking.Modules.EFT.BusinessEntities.Account"
			ID="AccountsDataSource" runat="server" />
		<pp:ObjectContainerDataSource DataObjectTypeName="GlobalBank.Commercial.EBanking.Modules.EFT.BusinessEntities.Transfer"
			ID="TransferBatchDataSource" runat="server" OnDeleted="TransferBatchDataSource_Deleted"
			OnInserted="TransferBatchDataSource_Inserted" OnUpdated="TransferBatchDataSource_Updated" />
		<asp:DetailsView ID="AddTransferView" runat="server" AutoGenerateRows="False" DataSourceID="TransferBatchDataSource"
			DefaultMode="Insert" EnableViewState="true" HeaderStyle-CssClass="newTransferHeader">
			<Fields>
				<asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="True" SortExpression="Id"
					Visible="False" />
				<asp:TemplateField HeaderText="From:" SortExpression="SourceAccount">
					<InsertItemTemplate>
						<asp:DropDownList Width="300px" AppendDataBoundItems="true" ID="SourceDropDown" runat="server"
							DataSourceID="AccountsDataSource" DataTextField="Name" DataValueField="Number"
							SelectedValue='<%# Bind("SourceAccount") %>'>
							<asp:ListItem></asp:ListItem>
						</asp:DropDownList>
						<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="SourceDropDown"
							ErrorMessage="(*)" ValidationGroup="AddTransferViewValidationGroup"></asp:RequiredFieldValidator>
						<asp:CompareValidator ID="CompareSourceAccount" runat="server" ControlToCompare="DestinationDropDown"
							ControlToValidate="SourceDropDown" ErrorMessage="CompareValidator" Text="Can't transfer between same accounts"
							Operator="NotEqual" ValidationGroup="AddTransferViewValidationGroup" Display="Dynamic" />
					</InsertItemTemplate>
					<HeaderStyle CssClass="newTransferHeader" />
				</asp:TemplateField>
				<asp:TemplateField HeaderText="To:" SortExpression="DestinationAccount">
					<InsertItemTemplate>
						<asp:DropDownList Width="300px" AppendDataBoundItems="true" ID="DestinationDropDown" runat="server"
							DataSourceID="AccountsDataSource" DataTextField="Name" DataValueField="Number"
							SelectedValue='<%# Bind("DestinationAccount") %>'>
							<asp:ListItem></asp:ListItem>
						</asp:DropDownList>&nbsp;
						<asp:CompareValidator ID="CompareDestinationAccount" runat="server" ControlToCompare="SourceDropDown"
							ControlToValidate="DestinationDropDown" ErrorMessage="CompareValidator" Text="Can't transfer between same accounts"
							Operator="NotEqual" ValidationGroup="AddTransferViewValidationGroup" Display="Dynamic" />
						<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="DestinationDropDown"
							ErrorMessage="(*)" ValidationGroup="AddTransferViewValidationGroup"></asp:RequiredFieldValidator>
					</InsertItemTemplate>
					<HeaderStyle CssClass="newTransferHeader" />
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Amount:" SortExpression="Amount">
					<InsertItemTemplate>
						<asp:TextBox ID="AmountBox" runat="server" Text='<%# Bind("Amount") %>' MaxLength="07"></asp:TextBox>
						<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="AmountBox"
							ErrorMessage="(*)" ValidationGroup="AddTransferViewValidationGroup" Display="Dynamic"></asp:RequiredFieldValidator>
						<asp:CompareValidator ID="TypeValidator1" runat="server" ControlToValidate="AmountBox"
							Operator="DataTypeCheck" Type="Double" ErrorMessage="Only numbers." />
						<asp:RangeValidator ID="AmountRangeValidator" runat="server" ControlToValidate="AmountBox"
							MaximumValue="9999999" MinimumValue="0" ErrorMessage="The valid range is between 0 and 9,999,999"
							ValidationGroup="AddTransferViewValidationGroup" Display="Dynamic" Type="Double" />
					</InsertItemTemplate>
					<HeaderStyle CssClass="newTransferHeader" />
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Description:" SortExpression="Description">
					<InsertItemTemplate>
						<asp:TextBox ID="DescriptionBox" Rows="80" runat="server" Text='<%# Bind("Description") %>'
							Width="300px" MaxLength="50"></asp:TextBox>
						<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="DescriptionBox"
							ErrorMessage="(*)" ValidationGroup="AddTransferViewValidationGroup" Display="Dynamic"></asp:RequiredFieldValidator>
						<asp:RegularExpressionValidator ID="DescriptionRegExValidator" ValidationGroup="AddTransferViewValidationGroup"
							runat="server" ControlToValidate="DescriptionBox" ValidationExpression="^[a-zA-Z0-9\040]+$"
							ErrorMessage="The description contains invalid characters" Display="Dynamic" />
					</InsertItemTemplate>
					<HeaderStyle CssClass="newTransferHeader" />
				</asp:TemplateField>
				<asp:TemplateField ShowHeader="False">
					<InsertItemTemplate>
						<div id="newTransferButtons">
							<asp:ImageButton ID="AddButton" SkinID="addButton" CausesValidation="True" CommandName="Insert"
								ValidationGroup="AddTransferViewValidationGroup" runat="server" />
							<asp:ImageButton ID="CancelButton" SkinID="cancelButton" CausesValidation="False"
								CommandName="Cancel" runat="server" />
						</div>
					</InsertItemTemplate>
				</asp:TemplateField>
			</Fields>
			<HeaderStyle CssClass="newTransferHeader" />
		</asp:DetailsView>
		<p />
		<div class="sectionHeader">
			TRANSFERS</div>
		<hr />
		<asp:GridView ID="TransfersGridView" runat="server" AutoGenerateColumns="False" DataSourceID="TransferBatchDataSource"
			DataKeyNames="Id">
			<EmptyDataTemplate>
				(No transfers entered.)
			</EmptyDataTemplate>
			<Columns>
				<asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="True" Visible="False" />
				<asp:TemplateField HeaderText="From" SortExpression="SourceAccount">
					<EditItemTemplate>
						<asp:DropDownList AppendDataBoundItems="true" ID="SourceDropDownforEditTransfer"
							runat="server" DataSourceID="AccountsDataSource" DataTextField="Name" DataValueField="Number"
							SelectedValue='<%# Bind("SourceAccount") %>'>
							<asp:ListItem></asp:ListItem>
						</asp:DropDownList>
						<asp:RequiredFieldValidator ID="RequiredFieldValidatorSourceAccount" runat="server"
							ControlToValidate="SourceDropDownforEditTransfer" ErrorMessage="(*)" ValidationGroup="TransferGridViewValidationGroup"></asp:RequiredFieldValidator>
						<asp:CompareValidator ID="CompareSourceAccount" runat="server" ControlToCompare="DestinationDropDownforEditTransfer"
							ControlToValidate="SourceDropDownforEditTransfer" ErrorMessage="Can't transfer between same accounts"
							Text="(*)" Operator="NotEqual" ValidationGroup="TransferGridViewValidationGroup" />
					</EditItemTemplate>
					<ItemTemplate>
						<asp:Label ID="Label1" runat="server" Text='<%# this.Presenter.GetAccountName((string)Eval("SourceAccount")) %>'></asp:Label>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="To" SortExpression="DestinationAccount">
					<EditItemTemplate>
						<asp:DropDownList AppendDataBoundItems="true" ID="DestinationDropDownforEditTransfer"
							runat="server" DataSourceID="AccountsDataSource" DataTextField="Name" DataValueField="Number"
							SelectedValue='<%# Bind("DestinationAccount") %>'>
							<asp:ListItem />
						</asp:DropDownList>
						<asp:RequiredFieldValidator ID="RequiredFieldValidatorDestinationAccount" runat="server"
							ControlToValidate="DestinationDropDownforEditTransfer" ErrorMessage="(*)" ValidationGroup="TransferGridViewValidationGroup"></asp:RequiredFieldValidator>
						<asp:CompareValidator ID="CompareDestinationAccount" runat="server" ControlToCompare="SourceDropDownforEditTransfer"
							ControlToValidate="DestinationDropDownforEditTransfer" ErrorMessage="Can't transfer between same accounts"
							Text="(*)" Operator="NotEqual" ValidationGroup="TransferGridViewValidationGroup" />
					</EditItemTemplate>
					<ItemTemplate>
						<asp:Label ID="Label2" runat="server" Text='<%# this.Presenter.GetAccountName((string)Eval("DestinationAccount")) %>'></asp:Label>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Amount" SortExpression="Amount">
					<EditItemTemplate>
						<asp:TextBox ID="AmountBoxGrid" runat="server" Text='<%# Bind("Amount") %>' MaxLength="7"></asp:TextBox>
						<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="The amount is required"
							ControlToValidate="AmountBoxGrid" ValidationGroup="TransferGridViewValidationGroup">(*)</asp:RequiredFieldValidator>
						<asp:CompareValidator ID="TypeValidator2" runat="server" ControlToValidate="AmountBoxGrid"
							Operator="DataTypeCheck" Type="Double" ErrorMessage="Only numbers." />
						<asp:RangeValidator ID="AmountBoxGridRangeValidator" runat="server" ControlToValidate="AmountBoxGrid"
							MaximumValue="9999999" MinimumValue="0" ErrorMessage="The valid range is between 0 and 9,999,999"
							ValidationGroup="TransferGridViewValidationGroup" Display="Dynamic" Type="Double" />
					</EditItemTemplate>
					<ItemTemplate>
						<asp:Label ID="Label4" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Description" SortExpression="Description">
					<EditItemTemplate>
						<asp:TextBox ID="DescriptionBoxGrid" runat="server" Text='<%# Bind("Description") %>'
							MaxLength="50"></asp:TextBox>
						<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="DescriptionBoxGrid"
							ErrorMessage="Please enter a description for this transfer" ValidationGroup="TransferGridViewValidationGroup">(*)</asp:RequiredFieldValidator>
						<asp:RegularExpressionValidator ID="DescriptionBoxGridRegExValidator" runat="server"
							ControlToValidate="DescriptionBoxGrid" ValidationExpression="^[a-zA-Z0-9\040]+$"
							ErrorMessage="The description contains invalid characters" ValidationGroup="TransferGridViewValidationGroup"
							Display="Dynamic" />
					</EditItemTemplate>
					<ItemTemplate>
						<asp:Label ID="Label3" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Action" ShowHeader="False">
					<EditItemTemplate>
						<asp:LinkButton ID="UpdateLinkButton" runat="server" CausesValidation="True" CommandName="Update"
							Text="Update" ValidationGroup="TransferGridViewValidationGroup"></asp:LinkButton>
						<asp:LinkButton ID="CancelLinkButton" runat="server" CausesValidation="False" CommandName="Cancel"
							Text="Cancel"></asp:LinkButton>
					</EditItemTemplate>
					<ItemTemplate>
						<asp:LinkButton ID="EditLinkButton" runat="server" CausesValidation="False" CommandName="Edit"
							Text="Edit"></asp:LinkButton>
						<asp:LinkButton ID="DeleteLinkButton" runat="server" CausesValidation="False" CommandName="Delete"
							Text="Delete"></asp:LinkButton>
					</ItemTemplate>
				</asp:TemplateField>
			</Columns>
		</asp:GridView>
		<asp:Label ID="ErrorLabel" runat="server" ForeColor="Red"></asp:Label>&nbsp;
		<p />
		<asp:ImageButton ID="NextButton" SkinID="nextButton" OnClick="NextButton_Click" runat="server" />
	</div>
</asp:Content>

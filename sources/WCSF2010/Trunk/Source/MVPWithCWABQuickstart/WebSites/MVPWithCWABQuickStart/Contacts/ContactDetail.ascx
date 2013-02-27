<%@ Control Language="C#" AutoEventWireup="true" Codebehind="ContactDetail.ascx.cs"
    Inherits="MVPWithCWABQuickStart.Contacts.Views.ContactDetail" %>
<%@ Register Assembly="Microsoft.Practices.Web.UI.WebControls" Namespace="Microsoft.Practices.Web.UI.WebControls"
    TagPrefix="pp" %>
<table>
    <tr>
        <td colspan="2">
            <asp:DetailsView ID="CustomerDetailsView" runat="server" AutoGenerateRows="False"
                DataSourceID="CustomerDataSource" DataKeyNames="Id">
                <Fields>
                    <asp:TemplateField HeaderText="Name:" SortExpression="Name">
                        <EditItemTemplate>
                            <asp:TextBox ID="NameTextBox" runat="server" Text='<%# Bind("Name") %>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="NameRequiredFieldValidator" runat="server" ErrorMessage="Name is required"
                                ControlToValidate="NameTextBox" ValidationGroup="SaveDetails" Display="Dynamic" />
                            <asp:RegularExpressionValidator ID="NameRegexValidator" runat="server" ErrorMessage="Invalid Name"
                                ControlToValidate="NameTextBox" ValidationGroup="SaveDetails" Display="Dynamic"
                                ValidationExpression="^[\w',.\s\-]{1,40}$" />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Address:" SortExpression="Address">
                        <EditItemTemplate>
                            <asp:TextBox ID="AddressTextBox" runat="server" Text='<%# Bind("Address") %>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="AddressRequiredFieldValidator" runat="server" ErrorMessage="Address is required"
                                ControlToValidate="AddressTextBox" ValidationGroup="SaveDetails" Display="Dynamic" />
                            <asp:RegularExpressionValidator ID="AddressRegexValidator" runat="server" ControlToValidate="AddressTextBox"
                                ErrorMessage="Invalid Address" ValidationGroup="SaveDetails" Display="Dynamic"
                                ValidationExpression="^[\w',.\s\-]{1,60}$" />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("Address") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="City:" SortExpression="City">
                        <EditItemTemplate>
                            <asp:TextBox ID="CityTextBox" runat="server" Text='<%# Bind("City") %>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="CityRequiredFieldValidator" runat="server" ErrorMessage="City is required"
                                ControlToValidate="CityTextBox" ValidationGroup="SaveDetails" Display="Dynamic" />
                            <asp:RegularExpressionValidator ID="CityRegexValidator" runat="server" ControlToValidate="CityTextBox"
                                ErrorMessage="Invalid City" ValidationGroup="SaveDetails" Display="Dynamic" ValidationExpression="^[\w',.\s\-]{1,15}$" />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("City") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="State:" SortExpression="State">
                        <EditItemTemplate>
                            <asp:DropDownList ID="StateDropDownList" runat="server" DataSourceID="StatesDataSource"
                                DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("State") %>'
                                Enabled="true">
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:DropDownList ID="StateDropDownList" runat="server" DataSourceID="StatesDataSource"
                                DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("State") %>'
                                Enabled="false">
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Postal Code:" SortExpression="Zip">
                        <EditItemTemplate>
                            <asp:TextBox ID="PostalCodeTextBox" runat="server" Text='<%# Bind("Zip") %>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="PostalCodeRequiredFieldValidator" runat="server"
                                ErrorMessage="Postal Code is required" ControlToValidate="PostalCodeTextBox"
                                ValidationGroup="SaveDetails" Display="Dynamic" />
                            <asp:RegularExpressionValidator ID="PostalCodeRegexValidator" runat="server" ControlToValidate="PostalCodeTextBox"
                                ErrorMessage="Invalid Postal Code" ValidationGroup="SaveDetails" Display="Dynamic"
                                ValidationExpression="^[\w',.\s\-]{1,10}$" />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("Zip") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Fields>
            </asp:DetailsView>
            <pp:ObjectContainerDataSource ID="CustomerDataSource" runat="server" DataObjectTypeName="MVPWithCWABQuickStart.Contacts.BusinessEntities.Customer"
                OnUpdated="CustomerDataSource_Updated" />
            <pp:ObjectContainerDataSource ID="StatesDataSource" runat="server" DataObjectTypeName="MVPWithCWABQuickStart.Contacts.BusinessEntities.State" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Button ID="EditButton" runat="server" Text="Edit" OnClick="EditButton_Click" />
            <asp:Button ID="SaveButton" runat="server" Text="Save" OnClick="SaveButton_Click"
                ValidationGroup="SaveDetails" />
            <asp:Button ID="DiscardChangesButton" runat="server" Text="Discard Changes" OnClick="DiscardChangesButton_Click"
                OnClientClick="javascript:document.forms[0].reset();" />
        </td>
    </tr>
</table>

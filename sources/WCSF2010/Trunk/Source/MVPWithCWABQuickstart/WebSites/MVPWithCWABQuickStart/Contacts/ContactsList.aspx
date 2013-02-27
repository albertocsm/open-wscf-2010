<%@ Page Language="C#" AutoEventWireup="true" Codebehind="ContactsList.aspx.cs" Inherits="MVPWithCWABQuickStart.Contacts.Views.ContactsList"
    Title="ContactsList" MasterPageFile="~/Shared/QuickStarts.master" %>

<%@ Register Src="ContactDetail.ascx" TagName="ContactDetail" TagPrefix="contacts" %>

<asp:Content ID="content" ContentPlaceHolderID="mainContent" runat="Server">
    <h1>Your Contacts</h1>
    <p>
        <asp:GridView ID="CustomersGridView" runat="server" DataSourceID="CustomersDataSource" AutoGenerateColumns="False" OnSelectedIndexChanged="CustomersGridView_SelectedIndexChanged">
            <Columns>
                <asp:CommandField ShowSelectButton="True" />
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
            </Columns>
        </asp:GridView>
    </p>
    <asp:ObjectDataSource ID="CustomersDataSource" 
    TypeName="MVPWithCWABQuickStart.Contacts.ContactsController"
    SelectMethod="GetCustomers"
    runat="server" OnObjectCreating="CustomersDataSource_ObjectCreating"></asp:ObjectDataSource>
    <contacts:ContactDetail ID="ContactDetail1" runat="server" />
</asp:Content>

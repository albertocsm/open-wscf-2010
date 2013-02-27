<%@ Page Language="C#" AutoEventWireup="true" Codebehind="SearchCustomer.aspx.cs"
    Inherits="RealTimeSearchQuickstart.CustomerManager.Views.SearchCustomer" Title="Search Customer"
    MasterPageFile="~/Shared/Default.master" %>

<%@ Register Assembly="Microsoft.Practices.Web.UI.WebControls" Namespace="Microsoft.Practices.Web.UI.WebControls"
    TagPrefix="WebControls" %>
<%@ Register Assembly="RealTimeSearch" Namespace="RealTimeSearch" TagPrefix="rts" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <asp:ScriptManagerProxy ID="SearchCustomerScriptManagerProxy" runat="server">
        <Scripts>
            <asp:ScriptReference Path="~/CustomerManager/SearchCustomerBehavior.js" IgnoreScriptPath="True" />
        </Scripts>
    </asp:ScriptManagerProxy>
    <h1>
        Search Customer</h1>
    <table style="width:100%">
        <tr>
            <td>
                <asp:Label ID="CompanyNameLabel" runat="server" Text="Name:"></asp:Label>
            </td>
            <td style="width:155px">
                <asp:TextBox ID="CompanyNameTextBox" runat="server" OnTextChanged="CompanyNameTextBox_TextChanged" MaxLength="50" />
                <asp:RegularExpressionValidator ID="CompanyNameRegexValidator" runat="server" ControlToValidate="CompanyNameTextBox"
                    ErrorMessage="Invalid search query string" ValidationExpression="^[\w',.\s\-]+$"
                    Display="Dynamic"></asp:RegularExpressionValidator>
            </td>
            <td rowspan="4" colspan="100%" style="vertical-align: middle; text-align: center">
                <asp:UpdateProgress ID="SearchUpdateProgress" runat="server" DynamicLayout="False">
                    <ProgressTemplate>
                        <asp:Image ID="ProcessingImage" runat="server" ImageUrl="~/images/loading.gif" /> <strong>Searching...</strong>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="CityLabel" runat="server" Text="City:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="CityTextBox" runat="server" OnTextChanged="CityTextBox_TextChanged" MaxLength="50"></asp:TextBox>
                <asp:RegularExpressionValidator ID="CityRegexValidator" runat="server" ControlToValidate="CityTextBox"
                    ErrorMessage="Invalid search query string" ValidationExpression="^[\w',.\s\-]+$"
                    Display="Dynamic"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="StateLabel" runat="server" Text="State:"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="StateDropDown" runat="server" AppendDataBoundItems="True" DataTextField="Name"
                    DataValueField="Id" OnSelectedIndexChanged="StateDropDown_SelectedIndexChanged"
                    Width="155px">
                    <asp:ListItem Selected="True"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="ZipCodeLabel" runat="server" Text="Postal Code:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="ZipCodeTextBox" runat="server" OnTextChanged="ZipCodeTextBox_TextChanged" MaxLength="50"></asp:TextBox>
                <asp:RegularExpressionValidator ID="ZipCodeRegexValidator" runat="server" ControlToValidate="ZipCodeTextBox"
                    ErrorMessage="Invalid search query string" ValidationExpression="^[\w',.\s\-]+$"
                    Display="Dynamic"></asp:RegularExpressionValidator>
            </td>
        </tr>
    </table>
    <asp:Button ID="SearchButton" runat="server" Text="Search" OnClick="SearchButton_Click" />&nbsp;
    <rts:RealTimeSearchMonitor ID="CustomerRealTimeSearchMonitor" runat="server" Interval="700" AssociatedUpdatePanelID="ResultsUpdatePanel">
        <ControlsToMonitor>
            <rts:ControlMonitorParameter TargetId="CompanyNameTextBox" />
            <rts:ControlMonitorParameter TargetId="CityTextBox" />
            <rts:ControlMonitorParameter TargetId="StateDropDown" />
            <rts:ControlMonitorParameter TargetId="ZipCodeTextBox" />
        </ControlsToMonitor>
    </rts:RealTimeSearchMonitor>
    <asp:UpdatePanel ID="ResultsUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 100%; text-align: right;">
                <asp:Label ID="TotalResultsCountTitleLabel" runat="server" Text="Total Results: "></asp:Label>
                <asp:Label ID="TotalResultsCountLabel" runat="server" Text="N"></asp:Label><br />
            </div>
            <asp:GridView ID="CustomersGridView" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                DataSourceID="CustomersContainerDataSource" AllowPaging="True" PageSize="10"
                Width="690px">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="ID" SortExpression="Id" />
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                    <asp:BoundField DataField="City" HeaderText="City" SortExpression="City" />
                    <asp:BoundField DataField="State" HeaderText="State" SortExpression="State" />
                    <asp:BoundField DataField="Zip" HeaderText="Zip" SortExpression="Zip" />
                    <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
                </Columns>
                <EmptyDataTemplate>
                    <div style="width: 100%; text-align: center;">
                        <asp:Label ID="NoResultsLabel" runat="server" Text="No results found for criteria."></asp:Label>
                    </div>
                </EmptyDataTemplate>
                <PagerSettings Mode="NextPreviousFirstLast" Position="TopAndBottom" />
            </asp:GridView>
            <WebControls:ObjectContainerDataSource ID="CustomersContainerDataSource" runat="server"
                DataObjectTypeName="RealTimeSearchQuickstart.CustomerManager.BusinessEntities.Customer"
                UsingServerPaging="true" OnSelecting="CustomersContainerDataSource_Selecting"></WebControls:ObjectContainerDataSource>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="SearchButton" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

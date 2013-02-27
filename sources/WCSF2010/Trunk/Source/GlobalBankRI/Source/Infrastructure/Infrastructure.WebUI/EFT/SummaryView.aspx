<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPages/Module.master" AutoEventWireup="true" Inherits="EFT_SummaryView" Title="Summary" Codebehind="SummaryView.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="moduleActionContent" Runat="Server">
	<div id="contentTitle" class="sectionTitle">Summary</div>
    <div id="moduleContentContent">
    <asp:GridView ID="TransferGridView" runat="server" AutoGenerateColumns="False" EnableViewState="False" PageSize="5">
        <Columns>
            <asp:BoundField DataField="SourceAccountName" HeaderText="Source" SortExpression="SourceAccount" />
            <asp:BoundField DataField="DestinationAccountName" HeaderText="Destination" SortExpression="DestinationAccount" />
            <asp:BoundField DataField="Amount" HeaderText="Amount" />
            <asp:BoundField DataField="Description" HeaderText="Description" />
            <asp:BoundField DataField="Status" HeaderText="Result" />
            <asp:BoundField DataField="TransactionId" HeaderText="Transaction ID" />
        </Columns>
    </asp:GridView>
    </div>
</asp:Content>


<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPages/Module.master" AutoEventWireup="true" Inherits="Reports_AccountsSummaryView" Title="Accounts Summary" Codebehind="AccountsSummaryView.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="moduleActionContent" Runat="Server">
<asp:XmlDataSource ID="XmlDataSource1" 
     runat="server" DataFile="~/Reports/SimpleData/CashAccounts.xml"
     XPath="Accounts/Account">
    </asp:XmlDataSource>
<asp:XmlDataSource ID="XmlDataSource2" 
     runat="server" DataFile="~/Reports/SimpleData/InvestmentAccounts.xml"
     XPath="Accounts/Account">
    </asp:XmlDataSource>
<asp:XmlDataSource ID="XmlDataSource3" 
     runat="server" DataFile="~/Reports/SimpleData/CreditCardAccounts.xml"
     XPath="Accounts/Account">
    </asp:XmlDataSource>
    
<div id="contentTitle" class="sectionTitle">Accounts Summary</div>
  <div id="moduleContentContent">
  <div class="sectionHeader">CASH ACCOUNTS</div>
  <hr />
  <asp:GridView ID="GridView1" runat="server" BorderWidth="0px" BorderStyle="None" AutoGenerateColumns="False" DataSourceID="XmlDataSource1">
      <Columns>
        <asp:BoundField DataField="Type" 
         HeaderText="Account type" 
         SortExpression="Type" /> 
        <asp:BoundField DataField="Number" 
         HeaderText="Account number" 
         SortExpression="Number" /> 
        <asp:BoundField DataField="Balance" 
         HeaderText="Balance" 
         SortExpression="Balance" />          
      </Columns>
    </asp:GridView>
    <br />
  <div class="sectionHeader">INVESTMENT ACCOUNTS</div>
  <hr />
      <asp:GridView ID="GridView2" runat="server" BorderWidth="0px" BorderStyle="None" AutoGenerateColumns="False" DataSourceID="XmlDataSource2">
      <Columns>
        <asp:BoundField DataField="Type" 
         HeaderText="Account type" 
         SortExpression="Type" /> 
        <asp:BoundField DataField="Shares" 
         HeaderText="Shares" 
         SortExpression="Shares" /> 
        <asp:BoundField DataField="Quote" 
         HeaderText="Quote" 
         SortExpression="Quote" />          
      </Columns>
    </asp:GridView>
    <br />
    
    <div class="sectionHeader">CREDIT CARD ACCOUNTS</div>
  <hr />
      <asp:GridView ID="GridView3" runat="server" BorderWidth="0px" BorderStyle="None" AutoGenerateColumns="False" DataSourceID="XmlDataSource3">
      <Columns>
        <asp:BoundField DataField="Number" 
         HeaderText="Account number" 
         SortExpression="Number" />  
        <asp:BoundField DataField="Type" 
         HeaderText="Account type" 
         SortExpression="Type" /> 
        <asp:BoundField DataField="Balance" 
         HeaderText="Balance" 
         SortExpression="Balance" /> 
         <asp:BoundField DataField="Limit" 
         HeaderText="Limit" 
         SortExpression="Limit" />
         <asp:BoundField DataField="Payment" 
         HeaderText="Payment due" 
         SortExpression="Payment" />         
      </Columns>
    </asp:GridView>
  </div>
</asp:Content>


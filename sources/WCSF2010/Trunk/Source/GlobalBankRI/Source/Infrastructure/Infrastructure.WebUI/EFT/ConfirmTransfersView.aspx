<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPages/Module.master" AutoEventWireup="true" Inherits="EFT_ConfirmTransfersView" Title="Confirm Transaction" Codebehind="ConfirmTransfersView.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="moduleActionContent" Runat="Server">
    <div id="contentTitle" class="sectionTitle">Confirm Transaction</div>
    <div id="moduleContentContent">
    <asp:GridView ID="TransferGridView" runat="server" AutoGenerateColumns="False" EnableViewState="False" PageSize="5">
        <Columns>
            <asp:BoundField DataField="SourceAccountName" HeaderText="Source" SortExpression="SourceAccount" />
            <asp:BoundField DataField="DestinationAccountName" HeaderText="Destination" SortExpression="DestinationAccount" />
            <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" />
            <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Amount" />
        </Columns>
    </asp:GridView>
    <p />
    <asp:ImageButton ID="ChangeButton" SkinID="changeButton" OnClick="PreviousButton_Click" runat="server" />
    <asp:ImageButton ID="SubmitButton" SkinID="submitButton" OnClick="SubmitButton_Click" runat="server" />
    </div>
</asp:Content>


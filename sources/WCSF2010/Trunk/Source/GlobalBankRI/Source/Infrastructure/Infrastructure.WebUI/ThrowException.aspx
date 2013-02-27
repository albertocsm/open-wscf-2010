<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPages/PublicMaster.master" AutoEventWireup="true" Inherits="ThrowException" Title="GlobalBank" Codebehind="ThrowException.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PublicContent" Runat="Server">
    <div id="contentTitle" class="sectionTitle">
        Exception Logging & Shielding Test</div>
    <div id="publicContentContent">
	<asp:Button ID="Throw" runat="server" Text="Throw Exception" OnClick="Throw_Click" />
    </div>
</asp:Content>


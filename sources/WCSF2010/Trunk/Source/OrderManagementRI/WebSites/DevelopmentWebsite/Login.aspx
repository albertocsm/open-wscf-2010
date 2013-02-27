<%@ Page Language="C#" MasterPageFile="~/Shared/Default.master" AutoEventWireup="true" Title="Login" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" Runat="Server">
	<asp:Login ID="Login1" runat="server">
	</asp:Login>
	<br />
	User Name: any valid employee id (a-jdoe is an approver, jblack is a regular user.)<br />
	Password: P@ssw0rd
</asp:Content>

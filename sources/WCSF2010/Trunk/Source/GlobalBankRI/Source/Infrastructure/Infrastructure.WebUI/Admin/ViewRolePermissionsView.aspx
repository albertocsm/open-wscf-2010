<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPages/Module.master" AutoEventWireup="true" Inherits="Admin_ViewRolePermissionsView" Title="Permissions" Codebehind="ViewRolePermissionsView.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="moduleActionContent" Runat="Server">
<div id="contentTitle" class="sectionTitle">Permissions</div>
    <div id="moduleContentContent">
        <asp:Repeater ID="ViewsRepeater" runat="server">
        <ItemTemplate>
            <asp:GridView runat="server" ID="PermissionView" DataSource='<%# Container.DataItem %>' />
            <hr />
        </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>


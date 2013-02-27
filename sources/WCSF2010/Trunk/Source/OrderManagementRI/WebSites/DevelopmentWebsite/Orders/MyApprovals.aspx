<%@ Page Language="C#" MasterPageFile="~/Shared/Default.master" Title="My Approvals" %>

<%@ Register Src="Parts/Approvals.ascx" TagName="Approvals" TagPrefix="uc1" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" Runat="Server">
    <h1>My Approvals</h1>
    
    <uc1:Approvals ID="MyApprovals" runat="server" />
    
</asp:Content>

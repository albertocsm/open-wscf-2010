<%@ Page Language="C#" MasterPageFile="~/Shared/Default.master" Title="My Saved Drafts" %>

<%@ Register Src="Parts/SavedDrafts.ascx" TagName="SavedDrafts" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="DefaultContent" Runat="Server">
	<h1>My Saved Drafts</h1>
        
    <uc1:SavedDrafts ID="MySavedDrafts" runat="server" />
        
</asp:Content>


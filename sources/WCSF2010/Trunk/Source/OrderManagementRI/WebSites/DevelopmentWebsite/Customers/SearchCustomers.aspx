<%@ Page Language="C#" MasterPageFile="~/Shared/Default.master" Title="Search Customers" %>
<%@ Register Src="~/SharedUserControls/SearchCustomer.ascx" TagName="SearchCustomer" TagPrefix="uc1" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" Runat="Server">
		<h1>Search Customers</h1>
    
        <uc1:SearchCustomer id="SearchCustomer1" runat="server">
        </uc1:SearchCustomer>
		   
</asp:Content>
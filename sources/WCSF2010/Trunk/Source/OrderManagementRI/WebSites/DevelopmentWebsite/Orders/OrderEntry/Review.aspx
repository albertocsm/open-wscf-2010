<%@ Page Language="C#" MasterPageFile="~/Shared/Default.master" Title="Review Order" %>

<%@ Register Src="~/Orders/OrderEntry/OrderReview.ascx" TagName="OrderReview" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="DefaultContent" Runat="Server">
    <uc1:OrderReview ID="OrderReview1" runat="server" />
</asp:Content>


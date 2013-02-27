<%@ Page Language="C#" MasterPageFile="~/Shared/Default.master" Title="Confirmation" %>

<%@ Register Src="~/Orders/OrderEntry/OrderConfirmation.ascx" TagName="OrderConfirmation" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="DefaultContent" Runat="Server">
    <uc1:OrderConfirmation ID="OrderConfirmation1" runat="server" />
</asp:Content>


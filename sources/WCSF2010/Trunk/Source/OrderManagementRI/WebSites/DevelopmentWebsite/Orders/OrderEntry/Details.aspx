<%@ Page Language="C#" MasterPageFile="~/Shared/Default.master" Title="Order Details" ValidateRequest="false" %>
<%@ Register Src="~/Orders/OrderEntry/OrderDetails.ascx" TagName="OrderDetails" TagPrefix="uc1" %>
<%--
 Please note that in the @Page directive above, we have ValidateRequest="false". 
 Normally we would not recommend this, as it removes a useful automatic mechanism
 for protecting the server against malicious requests. In this case, however, we
 have a free-form text entry field (in the Search Products popup). Users are able
 to type arbitrary characters into this field, which will trip the request validation
 on valid input.

 Instead of relying on request validation, we have implemented validation in the server
 to mitigate these threats.
--%>

<asp:Content ID="Content1" ContentPlaceHolderID="DefaultContent" Runat="Server">
    <uc1:OrderDetails ID="OrderDetails1" runat="server" />
    Instructions<ol>
        <li>Open the help panel to see available functionality.</li>
        <li>Edit order items.</li>
        <li>Either save the order as draft or preview it for later submission.</li>
	</ol>
	Per order item instructions<ol>
		<li>Click the row to enter in edit mode.</li>
		<li>Type the Sku. Some of the existing Sku are "<strong>1234-56789</strong>", 
		"<strong>2222-33333</strong>" and "<strong>3333-44444</strong>".</li>
		<li>Optional use the search product functionality by clicking the magnifying glass icon.</li>
		<li>Type the quantity. The quantity must be an integer value.</li>
	</ol>
</asp:Content>


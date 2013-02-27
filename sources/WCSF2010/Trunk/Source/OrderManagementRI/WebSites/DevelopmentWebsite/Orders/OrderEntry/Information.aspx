<%@ Page Language="C#" MasterPageFile="~/Shared/Default.master" Title="Order Information" %>

<%@ Register Src="~/Orders/OrderEntry/OrderInformation.ascx" TagName="OrderInformation" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="DefaultContent" Runat="Server">
    <uc1:OrderInformation ID="OrderInformation1" runat="server" />
    Instructions<ol>
        <li>Type a valid order name. The order name field is required and it must be shorter
            than 50 characters. Valid characters are letters, dash (-), spaces, and apostrophes
            (').</li>
        <li>(Optional) Type a description. Same valid characters as order name field.</li>
        <li>Select a customer. Either start typing a valid customer name or use the search 
        customer functionality by clicking the magnifying glass icon. Existing customer prefix are
        "<strong>G</strong>", "<strong>H</strong>", "<strong>L</strong>" and "<strong>O</strong>".
        <li>Select an approver.</li>
        <li>Type the street. The street field is required and it must be shorter
            than 60 characters. Same valid characters as order name field.</li>
        <li>Select a state.</li>
        <li>Type the City. Autocomplete will suggest cities of the selected 
        state after first character is entered.</li>
		<li>Type the Zip. Autocomplete will suggest zip codes of the selected 
        state and city after first character is entered.</li>
        <li>Click <strong>Next</strong> to edit the order details.</li>
    </ol>
</asp:Content>


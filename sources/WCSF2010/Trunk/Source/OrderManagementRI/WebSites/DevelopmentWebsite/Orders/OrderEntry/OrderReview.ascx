<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderReview.ascx.cs" Inherits="Orders_OrderReview" %>

<%@ Register Src="../Parts/OrderPreview.ascx" TagName="OrderPreview" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>
<h1>
    Review</h1>
<p>
    <uc1:OrderPreview ID="OrderPreviewUserControl" runat="server" EnableViewState="false" />
</p>
<table border="0">
    <tr>
        <td style="width: 125px">
            <asp:ImageButton ID="PreviousButton" runat="server" OnClick="PreviousButton_Click" 
                SkinID="NavButton" ImageUrl="~/Images/previous_light.png" />
        </td>
        <td style="width: 125px">
            <asp:ImageButton ID="SubmitButton" runat="server" OnClick="SubmitButton_Click" 
                SkinID="NavButtonDefault" ImageUrl="~/Images/submit_light.png" />
            <ajaxtoolkit:ConfirmButtonExtender ID="SubmitButtonConfirmationExtender" runat="server" TargetControlID="SubmitButton"
                ConfirmText="Are you sure you want to submit this order?" />
        </td>
    </tr>
</table>
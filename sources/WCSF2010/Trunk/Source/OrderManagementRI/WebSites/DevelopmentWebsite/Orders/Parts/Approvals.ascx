<%@ Control Language="C#" AutoEventWireup="true" Codebehind="Approvals.ascx.cs" Inherits="Orders_Approvals" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="OrderPreview.ascx" TagName="OrderPreview" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:GridView ID="MyApprovalsGridView" runat="server" AutoGenerateColumns="False"
            OnRowCommand="MyApprovalsGridView_RowCommand" DataKeyNames="OrderId">
            <Columns>
                <asp:ButtonField CommandName="ShowOrderDetails" HeaderText="Details" Text="&lt;span style='text-decoration: none;'&gt;[+]&lt;/span&gt;" />
                <asp:BoundField DataField="OrderId" HeaderText="ID" />
                <asp:BoundField DataField="OrderName" HeaderText="Name" />
                <asp:TemplateField HeaderText="Amount">
                    <ItemTemplate>
                        <asp:Label ID="AmountLabel" runat="server" Text='<%# Eval("OrderTotal", "{0:C2}") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Description" HeaderText="Description" />
                <asp:ButtonField CommandName="ApproveOrder" Text="Approve" />
                <asp:ButtonField CommandName="RejectOrder" Text="Reject" />
            </Columns>
            <EmptyDataTemplate>
                No pending orders for approval.
            </EmptyDataTemplate>
        </asp:GridView>
        <asp:Label ID="NullControl" runat="server" /><%-- Non operational control required for the TargetControlID property of popup extender --%>
        <ajax:ModalPopupExtender ID="DetailsModalPopupExtender" runat="server" TargetControlID="NullControl"
            PopupControlID="DetailsPopupPanel" DropShadow="true" CancelControlID="CancelButton"
            BackgroundCssClass="modalBackground" />
        <asp:Panel ID="DetailsPopupPanel" runat="server" Width="570px" BorderStyle="Outset"
            BackColor="white">
            <table border="0">
                <tr>
                    <td>
                        <uc1:OrderPreview ID="OrderPreviewPart" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:ImageButton ID="ApproveOrderButton" runat="server" OnCommand="ApproveRejectOrder_Command"
                            CommandName="ApproveOrder" ImageUrl="~/Images/approve_light.png" />
                        <asp:ImageButton ID="RejectOrderButton" runat="server" OnCommand="ApproveRejectOrder_Command"
                            CommandName="RejectOrder" ImageUrl="~/Images/reject_light.png" />
                        <asp:ImageButton ID="CancelButton" runat="server" ImageUrl="~/Images/cancel_light.png" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>

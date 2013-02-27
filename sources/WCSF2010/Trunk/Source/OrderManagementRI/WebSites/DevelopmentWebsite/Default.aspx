<%@ Page Language="C#" AutoEventWireup="true"  Codebehind="Default.aspx.cs" Inherits="_Default" MasterPageFile="~/Shared/Default.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
	<h1>
		Welcome to Order Management site</h1>
	<p>
		Each user can create and edit draft orders. When user completes the creation of
		an order, it can be submitted for approval. Order's completion can be accomplished
		between different sessions.
	</p>
	<ul>
		<li>To start new orders go to <b>Create New Order</b>.</li>
		<li>Draft orders are shown in <b>My Saved Drafts</b>, edition can be resumed here.</li>
	</ul>
	<p>
		When the approver is logged in, the list of orders pending for approvals are shown
		in <b>My Approvals</b>.
	</p>
	<p>
		Using <b>Search Orders</b> users can find any orders by name or description.
	</p>
    <asp:LoginView ID="LoginView1" runat="server">
        <AnonymousTemplate>
            The menu items will not be displayed until you
            <asp:LoginStatus ID="LoginStatus1" runat="server" LoginText="Log in." />
        </AnonymousTemplate>
    </asp:LoginView>
	<hr style="width: 80%" />
	<p>
		Web Client Software Factory 2.0 - Reference Implementation
	</p>
	<asp:Panel ID="PanelDevInfo" runat="server">
		<asp:Panel ID="CollapsingDevPanelHeader" runat="server" BackColor="beige" BorderColor="Blue"
			BorderStyle="Ridge" BorderWidth="5" Height="22px" Width="292px">
			<div id="DevInfoCollapseButton">&nbsp;
				<asp:ImageButton ID="ImageExpandCollapse" runat="server" ImageUrl="~/images/expand.jpg"/>
				<asp:Label Font-Bold="true" Font-Size="Larger" ID="Label1" runat="server">&nbsp;Technical Information</asp:Label>
		</asp:Panel>
		<asp:Panel ID="CollapsingDevPanel" runat="server" BackColor="White" BorderColor="Blue"
			BorderStyle="Ridge" BorderWidth="5" Height="100%" Width="292px">
			<asp:Panel runat="server" ID="textHolder" Width="292px">
				<div id="DevInfoText">
					This application was developed using the Web Client Software Factory.&nbsp; It shows
					a number of proven practices and solutions to several challenges that web developers
					encounter regularly.
					<br />
					<strong>
						<br />
						Modular Design</strong><br />
					The main web site project uses the Composite Web Application Block (CWAB) to split
					the functionality into modules that seperate teams can work on in parallel.&nbsp;
					<br />
					<br />
					<strong>Consistent Look</strong><br />
					The sites utilizes a Master Page to ensure a consistent look and feel for the user,
					across modules.&nbsp;
					<br />
					<br />
					<strong>Authentication and Authorization</strong><br />
					Access to the modules, pages, and web services is controlled via the built-in ASP.NET
					authentication mechanism and a Role Provider, which CWAB uses out-of-the-box.
					<br />
					<br />
					<strong>Responsiveness<br />
					</strong>Different areas of the site use ASP.NET AJAX to provide a responsive user
					interface.<br />
				</div>
			</asp:Panel>
		</asp:Panel>
	</asp:Panel>
	<ajaxtoolkit:AlwaysVisibleControlExtender ID="avDev" TargetControlID="PanelDevInfo"
		HorizontalSide="Right" VerticalSide="Top" runat="server" />
	<ajaxtoolkit:CollapsiblePanelExtender ID="bob" runat="server" CollapseControlID="CollapsingDevPanelHeader"
		ExpandControlID="CollapsingDevPanelHeader" TargetControlID="CollapsingDevPanel"
		CollapsedImage="~/Images/expand.jpg" ExpandedImage="~/Images/collapse.jpg" ImageControlID="ImageExpandCollapse"
		SuppressPostBack="true" Collapsed="true" CollapsedSize="0" ExpandDirection="Vertical" />
</asp:Content>

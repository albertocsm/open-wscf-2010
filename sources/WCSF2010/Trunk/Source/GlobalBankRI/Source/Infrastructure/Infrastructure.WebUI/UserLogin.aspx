<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPages/PublicMaster.master" AutoEventWireup="True"
  Inherits="UserLogin" Title="Login" Codebehind="UserLogin.aspx.cs" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PublicContent" runat="Server">
    <div id="contentTitle" class="sectionTitle">
        Login</div>
    <div id="publicContentContent">
        To login to the application, please use one of the following usernames and passwords:<br />
        <table>
            <tr>
                <td style="width:100px">
                    <strong>Username</strong></td>
                <td>
                    <strong>Password</strong></td>
            </tr>
            <tr>
                <td>
                    admin</td>
                <td>
                    <span style="font-family: Courier New">p@ssw0rd</span></td>
            </tr>
            <tr>
                <td style="height: 18px">
                    oper01</td>
                <td style="height: 18px">
                    <span style="font-family: Courier New">p@ssw0rd</span></td>
            </tr>
        </table>
        <div id="loginBox">
            <asp:Login SkinID="login" ID="Login1" runat="server"  />
        </div>
    </div>
</asp:Content>

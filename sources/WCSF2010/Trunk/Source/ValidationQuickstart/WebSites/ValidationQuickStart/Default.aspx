<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Default.aspx.cs" Inherits="ValidationQuickStart._Default"
    MasterPageFile="~/Shared/QuickStarts.master" Title="Profile information - Validation quickstart" %>

<%@ Register Assembly="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet"
    Namespace="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet"
    TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit.WCSFExtensions" Namespace="AjaxControlToolkit.WCSFExtensions"
    TagPrefix="ajaxtoolkitwcsfextensions" %>
<asp:Content ID="content" ContentPlaceHolderID="mainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div>
        <h1>
            Profile Information</h1>
    </div>
    <table border="0" cellpadding="0" cellspacing="0" style="font-size: 10.2pt; color: #666666">
        <tr>
            <td>
                <span class="inputDescriptor">Account number:</span></td>
            <td>
                <table style="border-top-width: 0px; padding-right: 0px; padding-left: 0px; border-left-width: 0px;
                    border-bottom-width: 0px; padding-bottom: 0px; margin: 0px; padding-top: 0px;
                    border-right-width: 0px;">
                    <tr>
                        <td>
                            <asp:TextBox ID="AccountNumberTextBox" runat="server" Width="320px" MaxLength="50"></asp:TextBox>
                        </td>
                        <td>
                            <cc1:PropertyProxyValidator ID="AccountNumberPropertyProxyValidator" runat="server"
                                ControlToValidate="AccountNumberTextBox" Display="Dynamic" PropertyName="AccountNumber"
                                SourceTypeName="ValidationQuickStart.BusinessEntities.Profile"></cc1:PropertyProxyValidator>
                        </td>
                    </tr>
                </table>
                <ajaxtoolkitwcsfextensions:ServerSideValidationExtender ID="AccountNumberServerSideValidationExtender"
                    runat="server" TargetControlID="AccountNumberPropertyProxyValidator">
                </ajaxtoolkitwcsfextensions:ServerSideValidationExtender>
            </td>
        </tr>
        <tr>
            <td style="height: 32px">
                Email:</td>
            <td style="height: 32px">
                <table style="border-top-width: 0px; padding-right: 0px; padding-left: 0px; border-left-width: 0px;
                    border-bottom-width: 0px; padding-bottom: 0px; margin: 0px; padding-top: 0px;
                    border-right-width: 0px;">
                    <tr>
                        <td>
                            <asp:TextBox ID="EmailTextBox" runat="server" Width="320px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RegularExpressionValidator ID="EmailRegularExpressionValidator" runat="server"
                                ControlToValidate="EmailTextBox" Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">The e-mail address is not valid.</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="height: 73px">
                Name:</td>
            <td style="height: 73px">
                <table style="border-top-width: 0px; padding-right: 0px; padding-left: 0px; border-left-width: 0px;
                    border-bottom-width: 0px; padding-bottom: 0px; margin: 0px; padding-top: 0px;
                    border-right-width: 0px;">
                    <tr>
                        <td>
                            <asp:TextBox ID="NameTextBox" runat="server" MaxLength="50" Width="320px" Wrap="False"></asp:TextBox></td>
                        <td>
                            <asp:Panel ID="Panel1" runat="server" BorderStyle="None">
                                <asp:RequiredFieldValidator ID="NameRequiredFieldValidator" runat="server" Display="Dynamic"
                                    ControlToValidate="NameTextBox">* </asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="NameCharacterValidator" runat="server" ControlToValidate="NameTextBox"
                                    ValidationExpression="[a-zA-Z-` ]*" Display="Dynamic">* The name may only contain letters, dashes, apostrophes, and spaces</asp:RegularExpressionValidator>
                                <cc1:PropertyProxyValidator ID="NamePropertyProxyValidator" runat="server" ControlToValidate="NameTextBox"
                                    Display="Dynamic" PropertyName="Name" SourceTypeName="ValidationQuickStart.BusinessEntities.Profile"></cc1:PropertyProxyValidator></asp:Panel>
                        </td>
                    </tr>
                </table>
                <ajaxtoolkitwcsfextensions:ServerSideValidationExtender ID="NameServerSideValidationExtender"
                    runat="server" TargetControlID="NamePropertyProxyValidator">
                </ajaxtoolkitwcsfextensions:ServerSideValidationExtender>
            </td>
        </tr>
        <tr>
            <td>
                Age</td>
            <td style="height: 24px">
                <table style="border-top-width: 0px; padding-right: 0px; padding-left: 0px; border-left-width: 0px;
                    border-bottom-width: 0px; padding-bottom: 0px; margin: 0px; padding-top: 0px;
                    border-right-width: 0px;">
                    <tr>
                        <td>
                            <asp:TextBox ID="AgeTextBox" runat="server" Columns="3" MaxLength="3"></asp:TextBox>
                        </td>
                        <td>
                            <cc1:PropertyProxyValidator ID="AgePropertyProxyValidator" runat="server" ControlToValidate="AgeTextBox"
                                Display="Dynamic" PropertyName="Age" SourceTypeName="ValidationQuickStart.BusinessEntities.Profile"></cc1:PropertyProxyValidator>
                        </td>
                    </tr>
                </table>
                <ajaxtoolkitwcsfextensions:ServerSideValidationExtender ID="AgeServerSideValidationExtender"
                    runat="server" TargetControlID="AgePropertyProxyValidator">
                </ajaxtoolkitwcsfextensions:ServerSideValidationExtender>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <hr />
            </td>
        </tr>
        <tr>
            <td>
                <span class="inputDescriptor">Homepage:</span></td>
            <td style="height: 24px">
                <table style="border-top-width: 0px; padding-right: 0px; padding-left: 0px; border-left-width: 0px;
                    border-bottom-width: 0px; padding-bottom: 0px; margin: 0px; padding-top: 0px;
                    border-right-width: 0px;">
                    <tr>
                        <td>
                            <asp:TextBox ID="HomepageTextBox" runat="server" Width="320px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RegularExpressionValidator ID="HomepageRegularExpressionValidator" runat="server"
                                ControlToValidate="HomepageTextBox" Display="Dynamic" ErrorMessage="The syntax for the URL is incorrect"
                                ValidationExpression="http(s)?://([\w-]+\.)*[\w-]+(:\d+)?(/[\w- ./?%&=]*)?"></asp:RegularExpressionValidator>
                            <asp:CustomValidator ID="HompageCustomValidator" runat="server" ControlToValidate="HomepageTextBox"
                                ErrorMessage="The specified URL is not online or does not accept HTTP connections."
                                OnServerValidate="HompageCustomValidator_ServerValidate" Display="Dynamic"></asp:CustomValidator>
                        </td>
                    </tr>
                </table>
                <ajaxtoolkitwcsfextensions:ServerSideValidationExtender ID="HomepageServerSideValidationExtender"
                    runat="server" TargetControlID="HompageCustomValidator">
                </ajaxtoolkitwcsfextensions:ServerSideValidationExtender>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <hr />
            </td>
        </tr>
        <tr>
            <td>
                State:</td>
            <td>
                <table style="border-top-width: 0px; padding-right: 0px; padding-left: 0px; border-left-width: 0px;
                    border-bottom-width: 0px; padding-bottom: 0px; margin: 0px; padding-top: 0px;
                    border-right-width: 0px;">
                    <tr>
                        <td>
                            <asp:TextBox ID="StateTextBox" runat="server" Width="30px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="StateRequiredFieldValidator" runat="server" Display="Dynamic"
                                ControlToValidate="StateTextBox">* </asp:RequiredFieldValidator>
                            <cc1:PropertyProxyValidator ID="StatePropertyProxyValidator" runat="server" ControlToValidate="StateTextBox"
                                Display="Dynamic" PropertyName="State" SourceTypeName="ValidationQuickStart.BusinessEntities.Profile"></cc1:PropertyProxyValidator>
                        </td>
                    </tr>
                </table>
                <ajaxtoolkitwcsfextensions:ServerSideValidationExtender ID="StateServerSideValidationExtender"
                    runat="server" TargetControlID="StatePropertyProxyValidator">
                </ajaxtoolkitwcsfextensions:ServerSideValidationExtender>
            </td>
        </tr>
        <tr>
            <td>
                <span class="inputDescriptor">Zip:</span></td>
            <td style="height: 24px">
                <table style="border-top-width: 0px; padding-right: 0px; padding-left: 0px; border-left-width: 0px;
                    border-bottom-width: 0px; padding-bottom: 0px; margin: 0px; padding-top: 0px;
                    border-right-width: 0px;">
                    <tr>
                        <td>
                            <asp:TextBox ID="ZipTextBox" runat="server" Width="100px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="ZipRequiredFieldValidator" runat="server" Display="Dynamic"
                                ControlToValidate="ZipTextBox">* </asp:RequiredFieldValidator>
                            <cc1:PropertyProxyValidator ID="ZipPropertyProxyValidator" runat="server" ControlToValidate="ZipTextBox"
                                Display="Dynamic" PropertyName="Zip" SourceTypeName="ValidationQuickStart.BusinessEntities.Profile"></cc1:PropertyProxyValidator>
                            <asp:RegularExpressionValidator ID="ZipCharactersValidator" runat="server" ErrorMessage="* Zip code must be five digits followed by an optional - and four digits"
                                ControlToValidate="ZipTextBox" ValidationExpression="[0-9]{5}(-[0-9]{4})?">* Zip code must be five digits followed by an optional - and four digits</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                </table>
                <ajaxtoolkitwcsfextensions:ServerSideValidationExtender ID="ZipServerSideValidationExtender"
                    runat="server" TargetControlID="ZipPropertyProxyValidator">
                </ajaxtoolkitwcsfextensions:ServerSideValidationExtender>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td style="height: 24px">
                <asp:UpdatePanel ID="SaveUpdatePanel" runat="server">
                    <ContentTemplate>
                        <asp:CustomValidator ID="AddressCustomValidator" runat="server" OnServerValidate="AddressCustomValidator_ServerValidate"
                            Display="Dynamic"></asp:CustomValidator>
                        <asp:Label ID="ProfileSavedLabel" runat="server" ForeColor="Green" Text="The profile information was saved."
                            Visible="False"></asp:Label><br />
                        <asp:Button ID="SaveButton" runat="server" Text="Save" OnClick="SaveButton_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    Instructions<ol>
        <li>Type a valid account number. Account numbers are valid from "<strong>00001</strong>"
            to "<strong>00009</strong>".</li>
        <li>Type the email for the profile. The email string should have the correct syntax.</li>
        <li>Type the name for the profile. The name field is required and it must be shorter
            than 25 characters. Valid characters are letters, dash (-), spaces, and apostrophes
            (')</li>
        <li>Type the age. The age must be a value between 16 and 30.</li>
        <li>Type the homepage of the customer. The URL must exist and be currently online. You
            must include the protocol (e.g.: 'http://').</li>
        <li>Type the State. The state field should be a valid 2 characters state code. For example
            "WA".</li>
        <li>Type the Zip code.&nbsp; The Zip code field should be a valid Zip code string.</li>
        <li>Zip codes "<strong>98052</strong>" and "<strong>98053</strong>" are the only valid
            zip codes for "<strong>REDMOND</strong>" city and "<strong>WA</strong>" state.</li>
    </ol>
</asp:Content>

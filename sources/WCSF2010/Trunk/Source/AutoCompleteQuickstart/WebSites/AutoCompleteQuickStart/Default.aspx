<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Default.aspx.cs" Inherits="AutoCompleteQuickStart.DefaultPage"
    MasterPageFile="~/Shared/QuickStarts.master" Title="Auto Complete QuickStart" %>
<%@ Register Assembly="AjaxControlToolkit.WCSFExtensions" Namespace="AjaxControlToolkit.WCSFExtensions"
    TagPrefix="ajaxtoolkitwcsfextensions" %>
<asp:Content ID="content" ContentPlaceHolderID="mainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <h1>Input Form</h1>
    <div>
    <table border="0" cellpadding="0" cellspacing="0" style="font-size: 10.2pt; color: #666666">
        <tr>
            <td style="width: 106px; height: 22px;">
                <span class="inputDescriptor">State:</span>
            </td>
            <td style="height: 22px">
            <!-- The State drop down is a simple drop down list control.  There is no extra context, nor Ajax extenders -->
                <asp:DropDownList ID="StateDropDown" runat="server" AppendDataBoundItems="True" DataTextField="Name"
                    DataValueField="Id" Width="320px">
                    <asp:ListItem Selected="True"></asp:ListItem>
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td style="width: 106px">
                <span class="inputDescriptor">City:</span>
            </td>
            <td>
				<!-- 
					The City text box's AJAX extenders use the selected State to help filter the returned autocomplete list.
					The extender talks to the PostalCodeAutocompleteService, which is an XML/JSON web service.
				 -->
                <asp:TextBox ID="CityTextBox" runat="server" Width="320px"></asp:TextBox>
                <ajaxtoolkitwcsfextensions:ContextSensitiveAutoCompleteExtender
                    ID="CityAutoComplete"
                    runat="server"
                    TargetControlID="CityTextBox"
                    CompletionSetCount="30"
                    CompletionInterval="400" 
                    MinimumPrefixLength="1" 
                    ServiceMethod="GetCities" 
                    ServicePath="PostalCodeAutoCompleteService.asmx">
                    <CompletionContextItems>
                        <ajaxtoolkitwcsfextensions:CompletionContextItem Key="State" ControlId="StateDropDown" />
                    </CompletionContextItems>
                </ajaxtoolkitwcsfextensions:ContextSensitiveAutoCompleteExtender>
            </td>
        </tr>
        <tr>
            <td style="height: 24px; width: 106px;">
                <span class="inputDescriptor">Zip Code:</span>
            </td>
            <td style="height: 24px">
				<!-- 
					The Zip Code text box's AJAX extenders use the selected State and City to help filter the returned autocomplete list.
					The extender talks to the PostalCodeAutocompleteService, which is an XML/JSON web service.
				 -->
                <asp:TextBox ID="PostalCodeTextBox" runat="server" Width="320px"></asp:TextBox>
                <ajaxtoolkitwcsfextensions:ContextSensitiveAutoCompleteExtender
                    ID="PostalCodeAutoComplete"
                    runat="server" 
                    TargetControlID="PostalCodeTextBox"
                    CompletionSetCount="30" 
                    CompletionInterval="400" 
                    MinimumPrefixLength="1" 
                    ServiceMethod="GetZipCodes"
                    ServicePath="PostalCodeAutoCompleteService.asmx">
                    <CompletionContextItems>
                        <ajaxtoolkitwcsfextensions:CompletionContextItem Key="State" ControlId="StateDropDown" />
                        <ajaxtoolkitwcsfextensions:CompletionContextItem Key="City" ControlId="CityTextBox" />
                    </CompletionContextItems>
                </ajaxtoolkitwcsfextensions:ContextSensitiveAutoCompleteExtender>
            </td>
        </tr>
        <tr><td colspan="2"><asp:Button runat="server" ID="Submit" Text="Submit" OnClick="Submit_Click" /></td></tr>
    </table>
    <br />
    <h3>Instructions</h3>
            <ol><li>Select a state from the <b>State</b> drop-down list</li>
            <li>Type the first letters of a city in the <b>City</b> field. If cities that start with the prefix you entered exist within the selected state, a list of suggested cities will appear. Select a city from the list.</li>
            <li>Type the first numbers of a zip code in the <b>Zip Code</b> field. If zip codes that start with the prefix you entered exist within the selected city, a list of suggested zip codes will appear.</li></ol>
    </div>
</asp:Content>

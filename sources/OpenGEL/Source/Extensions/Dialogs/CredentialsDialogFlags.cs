//===============================================================================
// Microsoft patterns & practices
//  GAX Extension Library
//===============================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Practices.RecipeFramework.Extensions.Dialogs
{
	/// <summary>
	/// Specifies special behavior for this function. 
	/// This value can be a bitwise-OR combination of zero or more of the following values. 
	/// </summary>
	[Flags]
	public enum CredentialsDialogFlags
	{
		/// <summary>
		/// The default flags settings. These are the following values:
		/// <see cref="GenericCredentials"/>, <see cref="ShowSaveCheckbox"/>, 
		/// <see cref="AlwaysShowUI"/> and <see cref="ExpectConfirmation"/>  
		/// </summary>
		Default = GenericCredentials |
					ShowSaveCheckbox |
					AlwaysShowUI |
					ExpectConfirmation,

		/// <summary>
		/// No flags specified.
		/// </summary>
		None = 0x0,

		/// <summary>
		/// Notify the user of insufficient credentials by displaying the "Logon unsuccessful" balloon tip.
		/// </summary>
		IncorrectPassword = 0x1,

		/// <summary>
		/// Do not store credentials or display check boxes. 
		/// You can pass <see cref="CredentialsDialogFlags.ShowSaveCheckbox"/> with this flag to display 
		/// the Save check box only, and the result is returned in the pfSave output parameter.
		/// </summary>
		DoNotPersist = 0x2,

		/// <summary>
		/// Populate the combo box with local administrators only.
		/// Windows XP Home Edition:  This flag will filter out the well-known Administrator account.
		/// </summary>
		RequestAdministrator = 0x4,

		/// <summary>
		/// Populate the combo box with user name/password only. 
		/// Do not display certificates or smart cards in the combo box.
		/// </summary>
		ExcludesCertificates = 0x8,

		/// <summary>
		/// Populate the combo box with certificates and smart cards only. 
		/// Do not allow a user name to be entered.
		/// </summary>
		RequireCertificate = 0x10,

		/// <summary>
		/// If the check box is selected, show the Save check box and return true in the
		/// <see cref="CredentialsDialog.SaveChecked"/> property, otherwise, return false. 
		/// <see cref="CredentialsDialogFlags.DoNotPersist"/> must be specified to use this flag. 
		/// Check box uses the value in <see cref="CredentialsDialog.SaveChecked"/> by default.
		/// </summary>
		ShowSaveCheckbox = 0x40,

		/// <summary>
		/// Specifies that a user interface will be shown even if the credentials can be 
		/// returned from an existing credential in credential manager. 
		/// This flag is permitted only if CredentialsDialog.GenericCredentials is also specified.
		/// </summary>
		AlwaysShowUI = 0x80,

		/// <summary>
		/// Populate the combo box with certificates or smart cards only. 
		/// Do not allow a user name to be entered.
		/// </summary>
		RequireSmartCard = 0x100,

		/// <summary/> 
		PasswordOnlyOk = 0x200,

		/// <summary/>
		ValidateUsername = 0x400,

		/// <summary/>
		CompleteUserName = 0x800,

		/// <summary>
		/// Do not show the Save check box, but the credential is saved as though the box 
		/// were shown and selected.
		/// </summary>
		Persist = 0x1000,

		/// <summary>
		/// This flag is meaningful only in locating a matching credential to prefill the dialog box, 
		/// should authentication fail. When this flag is specified, wildcard credentials will not be matched. 
		/// It has no effect when writing a credential. CredUI does not create credentials that contain 
		/// wildcard characters. Any found were either created explicitly by the user or created 
		/// programmatically, as happens when a RAS connection is made.
		/// </summary>
		ServerCredential = 0x4000,

		/// <summary>
		/// Specifies that the caller will call CredentialsDialog.ShowDialog after checking to determine whether 
		/// the returned credentials are actually valid. This mechanism ensures that invalid credentials are 
		/// not saved to the credential manager. 
		/// Specify this flag in all cases unless <see cref="CredentialsDialogFlags.DoNotPersist"/> is specified.
		/// </summary>
		ExpectConfirmation = 0x20000,

		/// <summary>
		/// Consider the credentials entered by the user to be generic credentials.
		/// </summary>
		GenericCredentials = 0x40000,

		/// <summary>
		/// The credential is a "runas" credential. The <see cref="CredentialsDialog.Target"/> parameter specifies the name of the 
		/// command or program being run. It is used for prompting purposes only.
		/// </summary>
		UsernameTargetCredentials = 0x80000,

		/// <summary>
		/// Sets if the CredentialsDialog.Credentials.User displayed as the user name is read-only.
		/// </summary>
		KeepUsername = 0x100000
	}
}

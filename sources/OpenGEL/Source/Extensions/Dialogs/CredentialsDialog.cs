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
using System.Runtime.InteropServices;
using System.Text;
using System.Net;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using System.Globalization;

namespace Microsoft.Practices.RecipeFramework.Extensions.Dialogs
{
	/// <summary>
	/// Displays a dialog box and promts the user for login credentials.
	/// </summary>
	/// <remarks>
	/// <p>
	/// This dialog provide access to a feature provided by Windows XP and Windows Server 2003
	/// called "Stored User Names and Passwords" to associate a set of credentials with a single
	/// Windows user account, storing those credentials using the Data Protection API (DPAPI) (See ProtectedData class).
	/// </p>
	/// <p>
	/// If your application, is running on Windows XP or Windows .NET, 
	/// can use the Credential Management API functions to prompt the user for credentials.
	/// Using these APIs will provide you with a consistent user interface and
	/// will allow you to automatically support the caching of these credentials by the operating system.
	/// </p>
	/// </remarks>
	[ToolboxItem(true)]
	[DesignerCategory("Dialogs")]
	[DefaultProperty("Credentials")]
	[ToolboxItemFilter("Microsoft.Practices.Windows.Forms.CredentialsDialog")]
	public class CredentialsDialog : CommonDialog
	{
		#region Fields

		private SecureCredential credentials;
		private string target;
		private string message;
		private string caption;
		private Image banner;
		private bool saveChecked;
		private CredentialsDialogFlags flags;

		#endregion

		#region Constructors

		/// <summary>
		/// Default class initializer.
		/// </summary>
		public CredentialsDialog()
		{
			this.Reset();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CredentialsDialog"/> class
		/// with the specified target.
		/// </summary>
		/// <param name="target">Contains the name of the target for the credentials, 
		/// typically a server name. For distributed file system (DFS) connections, 
		/// this string is of the form "servername\sharename".
		/// This parameter is used to identify Target Information when storing and retrieving credentials. 
		/// </param>
		public CredentialsDialog(string target)
			: this(target, null, null, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CredentialsDialog"/> class
		/// with the specified target and caption.
		/// </summary>
		/// <param name="target">
		/// Contains the name of the target for the credentials, 
		/// typically a server name. For distributed file system (DFS) connections, 
		/// this string is of the form "servername\sharename".
		/// This parameter is used to identify Target Information when storing and retrieving credentials. 
		/// </param>
		/// <param name="caption">The caption of the dialog (null will cause a system default title to be used).</param>
		public CredentialsDialog(string target, string caption)
			: this(target, caption, null, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CredentialsDialog"/> class
		/// with the specified target, caption and message.
		/// </summary>
		/// <param name="target">
		/// Contains the name of the target for the credentials, 
		/// typically a server name. For distributed file system (DFS) connections, 
		/// this string is of the form "servername\sharename".
		/// This parameter is used to identify Target Information when storing and retrieving credentials. 
		/// </param>
		/// <param name="caption">The caption of the dialog (null will cause a system default title to be used).</param>
		/// <param name="message">The message of the dialog (null will cause a system default message to be used).</param>
		public CredentialsDialog(string target, string caption, string message)
			: this(target, caption, message, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CredentialsDialog"/> class
		/// with the specified target, caption, message and banner.
		/// </summary>
		/// <param name="target">
		/// Contains the name of the target for the credentials, 
		/// typically a server name. For distributed file system (DFS) connections, 
		/// this string is of the form "servername\sharename".
		/// This parameter is used to identify Target Information when storing and retrieving credentials. 
		/// </param>
		/// <param name="caption">The caption of the dialog (null will cause a system default title to be used).</param>
		/// <param name="message">The message of the dialog (null will cause a system default message to be used).</param>
		/// <param name="banner">The image to display on the dialog (null will cause a system default image to be used).</param>
		public CredentialsDialog(string target, string caption, string message, Image banner) : this()
		{
			this.Target = target;
			this.Caption = caption;
			this.Message = message;
			this.Banner = banner;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the target.
		/// </summary>
		/// <value>The target.</value>
		public string Target
		{
			get { return target; }
			set
			{
				if (value != null)
				{
					if (value.Length > UnsafeNativeMethods.CREDUI_MAX_GENERIC_TARGET_LENGTH)
					{
						throw new ArgumentException(
                            string.Format(CultureInfo.CurrentUICulture, Properties.Resources.InvalidTargetLength,
							UnsafeNativeMethods.CREDUI_MAX_GENERIC_TARGET_LENGTH), "Target");
					}
				}
				target = value; 
			}
		}

		/// <summary>
		/// Gets or sets the message.
		/// </summary>
		/// <value>The message.</value>
		public string Message
		{
			get { return message; }
			set
			{
				if (value != null)
				{
					if (value.Length > UnsafeNativeMethods.CREDUI_MAX_MESSAGE_LENGTH)
					{
						throw new ArgumentException(
                            string.Format(CultureInfo.CurrentUICulture, Properties.Resources.InvalidMessageLength,
							UnsafeNativeMethods.CREDUI_MAX_MESSAGE_LENGTH), "Message");
					}
				}
				message = value; 
			}
		}

		/// <summary>
		/// Gets or sets the caption.
		/// </summary>
		/// <value>The caption.</value>
		public string Caption
		{
			get { return caption; }
			set 
			{
				if (value != null)
				{
					if (value.Length > UnsafeNativeMethods.CREDUI_MAX_CAPTION_LENGTH)
					{
						throw new ArgumentException(
                            string.Format(CultureInfo.CurrentUICulture, Properties.Resources.InvalidCaptionLength,
							UnsafeNativeMethods.CREDUI_MAX_CAPTION_LENGTH), "Caption");
					}
				}
				caption = value; 
			}
		}

		/// <summary>
		/// Gets or sets the credentials.
		/// </summary>
		/// <value>The credentials.</value>
		public SecureCredential Credentials
		{
			get { return credentials; }
			set { credentials = value; }
		}

		/// <summary>
		/// Gets or sets the banner.
		/// </summary>
		/// <value>The banner.</value>
		public Image Banner
		{
			get { return banner; }
			set
			{
				if (value != null)
				{
					if (value.Width != UnsafeNativeMethods.CREDUI_BANNER_WIDTH)
					{
						throw new ArgumentException(
							string.Format(CultureInfo.CurrentUICulture, Properties.Resources.InvalidImageWidth, 
							UnsafeNativeMethods.CREDUI_BANNER_WIDTH), "Banner");
					}
					if (value.Height != UnsafeNativeMethods.CREDUI_BANNER_HEIGHT)
					{
						throw new ArgumentException(
                            string.Format(CultureInfo.CurrentUICulture, Properties.Resources.InvalidImageHeight,
							UnsafeNativeMethods.CREDUI_BANNER_HEIGHT), "Banner");
					}
				}
				banner = value; 
			}
		}

		/// <summary>
		/// Gets or sets if the save checkbox status.
		/// </summary>
		/// <remarks>
		/// If Persist is false, this value will always be false
		/// because the save checkbox will be hidden. 
		/// </remarks>
		public bool SaveChecked
		{
			get { return saveChecked; }
			set	{ saveChecked = value;	}
		}

		/// <summary>
		/// See <see cref="CredentialsDialogFlags"/>.
		/// </summary>
		public CredentialsDialogFlags Flags
		{
			get { return flags; }
			set { flags = value; }
		}

		#endregion

		#region Public methods

		/// <summary>
		/// The ConfirmCredentials method is called after ShowDialog,
		/// to confirm the validity of the credential harvested. 
		/// </summary>
		/// <remarks>
		/// After calling ShowDialog and before calling <see cref="ConfirmCredentials"/>, 
		/// the caller must determine whether or not the credentials are actually valid by 
		/// using the credentials to access the resource specified by targetName.
		/// The results of that validation test are passed to <see cref="ConfirmCredentials"/> in the
		/// confirm parameter.
		/// </remarks>
		/// <param name="confirm">Specifies whether the credentials returned from the prompt function are valid.
		/// If TRUE, the credentials are stored in the credential manager.
		/// If FALSE, the credentials are not stored and various pieces of memory are cleaned up.
		/// </param>
		/// <permission cref="UIPermission">Demand for <see cref="UIPermissionWindow.SafeSubWindows"/> permission.</permission>
		public void ConfirmCredentials(bool confirm)
		{
			new UIPermission(UIPermissionWindow.SafeSubWindows).Demand();

			UnsafeNativeMethods.CredUIReturnCodes result = UnsafeNativeMethods.CredUIConfirmCredentialsW(this.target, confirm);
				
			if(result != UnsafeNativeMethods.CredUIReturnCodes.NO_ERROR && 
				result != UnsafeNativeMethods.CredUIReturnCodes.ERROR_NOT_FOUND &&
				result != UnsafeNativeMethods.CredUIReturnCodes.ERROR_INVALID_PARAMETER)
			{
				throw new InvalidOperationException(TranslateReturnCode(result));
				//Trace.TraceError(TranslateReturnCode(result));
			}
		}

		/// <summary>
		/// Deletes a credential from the user's credential set. 
		/// The credential set used is the one associated with the <see cref="Target"/> value.
		/// </summary>
		/// <permission cref="UIPermission">Demand for <see cref="UIPermissionWindow.SafeSubWindows"/> permission.</permission>
		public void DeleteSavedCredentials()
		{
			new UIPermission(UIPermissionWindow.SafeSubWindows).Demand();

			if (!UnsafeNativeMethods.DeleteCredential(this.target, UnsafeNativeMethods.CRED_TYPE_GENERIC, 0))
			{
				throw new Win32Exception();
			}
		}

		#endregion

		#region CommonDialog overrides

        /// <summary>
        /// When overridden in a derived class, specifies a common dialog box.
        /// </summary>
        /// <param name="hwndOwner">A value that represents the window handle of the owner window for the common dialog box.</param>
        /// <returns>
        /// true if the dialog box was successfully run; otherwise, false.
        /// </returns>
		protected override bool RunDialog(IntPtr hwndOwner)
		{
			if (Environment.OSVersion.Version.Major < 5)
			{
				throw new PlatformNotSupportedException(Properties.Resources.FunctionNotSupported);
			}

			UnsafeNativeMethods.CredUIInfo credInfo = new UnsafeNativeMethods.CredUIInfo(hwndOwner,
				this.caption, this.message, this.banner);
			StringBuilder usr = new StringBuilder(UnsafeNativeMethods.CREDUI_MAX_USERNAME_LENGTH);
			StringBuilder pwd = new StringBuilder(UnsafeNativeMethods.CREDUI_MAX_PASSWORD_LENGTH);

			if (this.credentials != null)
			{
				usr.Append(this.credentials.PrincipalName);
				pwd.Append(this.credentials.PasswordToString());
			}

			try
			{
				//For more info see:
				//http://msdn.microsoft.com/library/default.asp?url=/library/en-us/secauthn/security/creduipromptforcredentials.asp
				//http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnnetsec/html/dpapiusercredentials.asp?frame=true

				UnsafeNativeMethods.CredUIReturnCodes result = UnsafeNativeMethods.CredUIPromptForCredentials(
														ref credInfo, this.target,
														IntPtr.Zero, 0,
														usr, UnsafeNativeMethods.CREDUI_MAX_USERNAME_LENGTH,
														pwd, UnsafeNativeMethods.CREDUI_MAX_PASSWORD_LENGTH,
														ref this.saveChecked, this.flags);
				switch (result)
				{
					case UnsafeNativeMethods.CredUIReturnCodes.NO_ERROR:
						this.Credentials = new SecureCredential(usr.ToString(), pwd);
						return true;
					case UnsafeNativeMethods.CredUIReturnCodes.ERROR_CANCELLED:
						this.Credentials = null;
						return false;
					default:
						throw new InvalidOperationException(TranslateReturnCode(result));
				}
			}
			finally
			{
				usr.Remove(0, usr.Length);
				pwd.Remove(0, pwd.Length);
				if (this.banner != null)
				{
					UnsafeNativeMethods.DeleteObject(credInfo.hbmBanner);
				}
			}
		}

		/// <summary>
		/// Set all properties to it's default values.
		/// </summary>
		public override void Reset()
		{
			this.target = Application.ProductName ?? AppDomain.CurrentDomain.FriendlyName;
			this.credentials = null;
			this.caption = null;// target as caption;
			this.message = null; 
			this.banner = null;
			this.saveChecked = false;
			this.flags = CredentialsDialogFlags.Default;
		}

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.ComponentModel.Component"></see> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (this.credentials != null)
			{
				this.credentials.Dispose();
				this.credentials = null;
			}
		}

		#endregion

		#region Private methods

		private static string TranslateReturnCode(UnsafeNativeMethods.CredUIReturnCodes result)
		{
            return string.Format(CultureInfo.CurrentUICulture, Properties.Resources.CredUIReturn, result.ToString());
		}

		#endregion
	}
}

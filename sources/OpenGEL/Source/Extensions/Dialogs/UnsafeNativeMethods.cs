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
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices.ComTypes;

namespace Microsoft.Practices.RecipeFramework.Extensions.Dialogs
{
	/// <summary>
	/// Class for handling platform invoke declarations.
	/// </summary>
	[PermissionSet(SecurityAction.Demand, Name="FullTrust")]
	internal static class UnsafeNativeMethods
	{
		#region Common functions

		[DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
		internal static extern bool DeleteObject(IntPtr hObject);

		//CloseHandle parameters. When you are finished, 
		//free the memory allocated for the handle.
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern bool CloseHandle(IntPtr handle);

		#endregion

		#region CredUI declares

		internal const int CRED_TYPE_GENERIC = 1;

		[DllImport("advapi32.dll", EntryPoint = "CredDeleteW", SetLastError = true, CharSet = CharSet.Unicode)]
		internal static extern bool DeleteCredential(string targetName, int credType, int flags);

		internal struct CredUIInfo
		{
	        [PermissionSet(SecurityAction.Demand, Name="FullTrust")]
			internal CredUIInfo(IntPtr owner, string caption, string message, Image banner)
			{
				this.cbSize = Marshal.SizeOf(typeof(CredUIInfo));

				//Specifies the handle to the parent window of the dialog box. 
				//If this member is NULL, the desktop will be the parent window of the dialog box.
				this.hwndParent = owner;
				this.pszCaptionText = caption;
				this.pszMessageText = message;

				if (banner != null)
				{
					this.hbmBanner = new Bitmap(banner,
						UnsafeNativeMethods.CREDUI_BANNER_WIDTH, UnsafeNativeMethods.CREDUI_BANNER_HEIGHT).GetHbitmap();
				}
				else
				{
					this.hbmBanner = IntPtr.Zero;
				}
			}

			internal int cbSize;
			internal IntPtr hwndParent;
			[MarshalAs(UnmanagedType.LPWStr)]
			internal string pszMessageText;
			[MarshalAs(UnmanagedType.LPWStr)]
			internal string pszCaptionText;
			internal IntPtr hbmBanner;
		}

		[DllImport("credui.dll", EntryPoint="CredUIPromptForCredentialsW", SetLastError=true, CharSet=CharSet.Unicode)]
		internal extern static CredUIReturnCodes CredUIPromptForCredentials(
			ref CredUIInfo creditUR,
			string targetName,
			IntPtr reserved1,
			int iError,
			StringBuilder userName,
			int maxUserName,
			StringBuilder password,
			int maxPassword,
			ref bool iSave,
			CredentialsDialogFlags flags);

		[DllImport("credui.dll", SetLastError=true, CharSet=CharSet.Unicode)]
		internal extern static CredUIReturnCodes CredUIParseUserNameW(
			string userName,
			StringBuilder user,
			int userMaxChars,
			StringBuilder domain,
			int domainMaxChars);

		[DllImport("credui.dll",SetLastError=true, CharSet=CharSet.Unicode)]
		internal extern static CredUIReturnCodes CredUIConfirmCredentialsW(string targetName, bool confirm);

		internal enum CredUIReturnCodes
		{
			NO_ERROR = 0,
			ERROR_CANCELLED = 1223,
			ERROR_NO_SUCH_LOGON_SESSION = 1312,
			ERROR_NOT_FOUND = 1168,
			ERROR_INVALID_ACCOUNT_NAME = 1315,
			ERROR_INSUFFICIENT_BUFFER = 122,
			ERROR_INVALID_PARAMETER = 87,
			ERROR_INVALID_FLAGS = 1004
		}

		/// <summary>
		/// Maximum number of characters for the pszMessageText member of a CREDUI_INFO structure.
		/// </summary>
		internal const int CREDUI_MAX_MESSAGE_LENGTH = 100;

		/// <summary>
		/// Maximum number of characters for the pszCaptionText member of a CREDUI_INFO structure.
		/// </summary>
		internal const int CREDUI_MAX_CAPTION_LENGTH = 100;

		/// <summary>
		/// Maximum number of characters in a string that specifies a target name.
		/// </summary>
		internal const int CREDUI_MAX_GENERIC_TARGET_LENGTH = 100;

		/// <summary>
		/// Maximum number of characters in a string that specifies a domain name.
		/// </summary>
		internal const int CREDUI_MAX_DOMAIN_TARGET_LENGTH = 100;

		/// <summary>
		/// Maximum number of characters in a string that specifies a user account name.
		/// </summary>
		internal const int CREDUI_MAX_USERNAME_LENGTH = 100;

		/// <summary>
		/// Maximum number of characters in a string that specifies a password.
		/// </summary>
		internal const int CREDUI_MAX_PASSWORD_LENGTH = 100;

		/// <summary>
		/// Valid bitmap height (in pixels) of a user-defined banner.
		/// </summary>
		internal const int CREDUI_BANNER_HEIGHT = 60;

		/// <summary>
		/// Valid bitmap width (in pixels) of a user-defined banner.
		/// </summary>
		internal const int CREDUI_BANNER_WIDTH = 320;

		#endregion

		#region DsObjectPicker

		/// <summary>
		/// The GlobalLock function locks a global memory object and returns a pointer to 
        /// the first byte of the object's memory block.
		/// GlobalLock function increments the lock count by one.
		/// Needed for the clipboard functions when getting the data from IDataObject
		/// </summary>
		/// <param name="hMem"></param>
		/// <returns></returns>
		[DllImport("Kernel32.dll")]
        internal static extern IntPtr GlobalLock(IntPtr hMem);

		/// <summary>
		/// The GlobalUnlock function decrements the lock count associated with a memory object.
		/// </summary>
		/// <param name="hMem"></param>
		/// <returns></returns>
		[DllImport("Kernel32.dll")]
        internal static extern bool GlobalUnlock(IntPtr hMem);

        /// <summary>
        /// The DSOP_INIT_INFO structure contains data required to initialize an object picker dialog box. 
        /// </summary>
        //[CLSCompliant(false)]
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal unsafe struct DsopInitInfo
        {
            /// <summary/>
            public int cbSize;
            /// <summary/>
            public string pwzTargetComputer;
            /// <summary/>
            public int cDsScopeInfos;
            /// <summary/>
            public DsopScopeInitInfo* aDsScopeInfos;
            /// <summary/>
            public int flOptions;
            /// <summary/>
            public int cAttributesToFetch;
            /// <summary/>
            public short* apwzAttributeNames;
        }

		/// <summary>
        /// The DsopScopeInitInfo structure describes one or more scope types that 
        /// have the same attributes. A scope type is a type of location, for example 
        /// a domain, computer, or Global Catalog, from which the user can select objects.
		/// A scope type is a type of location, for example a domain, computer, or 
        /// Global Catalog, from which the user can select objects. 
		/// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal unsafe struct DsopScopeInitInfo
		{
            public int cbSize;
            public int flType;
            public int flScope;
			public DsopFilterFlags FilterFlags;
            public short* pwzDcName;
            public short* pwzADZPath;
            public void* hr;
        }

		/// <summary>
        /// The DsopUplevelFilterFlags structure contains flags that indicate the 
        /// filters to use for an up-level scope. An up-level scope is a scope that 
        /// supports the ADSI LDAP provider.
		/// </summary>
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct DsopUplevelFilterFlags
		{
			public int flBothModes;
			public int flMixedModeOnly;
			public int flNativeModeOnly;
		}

		/// <summary>
        /// The DsopFilterFlags structure contains flags that indicate the types of 
        /// objects presented to the user for a specified scope or scopes.
		/// </summary>
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct DsopFilterFlags
		{
			public DsopUplevelFilterFlags Uplevel;
			public int flDownlevel;
		}

		/// <summary>
        /// The DsSelection structure contains data about an object the user selected 
        /// from an object picker dialog box. 
        /// The DsSelectionList structure contains an array of DS_SELECTION structures.
		/// </summary>
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct DsSelection
		{
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pwzName;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pwzADsPath;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pwzClass;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pwzUPN;
			public IntPtr pvarFetchedAttributes;
			public uint flScopeType;
		}

		/// <summary>
        /// The DsSelectionList structure contains data about the objects the user 
        /// selected from an object picker dialog box.
		/// This structure is supplied by the IDataObject interface supplied by the 
        /// IDsObjectPicker::InvokeDialog method in the CFSTR_DSOP_DS_SELECTION_LIST data format.
		/// </summary>
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct DsSelectionList
		{
			public uint cItems;
			public uint cFetchedAttributes;
			public DsSelection[] aDsSelection;
		}

        [DllImport("ole32.dll", EntryPoint = "ReleaseStgMedium", PreserveSig = false)]
        internal static extern void ReleaseStgMedium(ref STGMEDIUM medium);

		#endregion
	}
}

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
using System.Text;
using System.Globalization;
using System.Security.Permissions;

namespace Microsoft.Practices.RecipeFramework.Extensions.Dialogs
{
	/// <summary>
	/// Summary description for SecureCredential.
	/// </summary>
	[PermissionSet(SecurityAction.Demand, Name="FullTrust")]
	public class SecureCredential : IDisposable
	{
		#region Private Fields

		private string user;
		private SecureString password;
		private string domain;
		private string principalName;

		private bool disposed;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		public SecureCredential(string principalName, char[] password) :
			this(principalName, ToSecureString(password))
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public SecureCredential(string principalName, StringBuilder password) :
			this(principalName, ToSecureString(password))
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public SecureCredential(string principalName, SecureString password)
		{
			// validate inputs
			if (principalName.Length >
				UnsafeNativeMethods.CREDUI_MAX_USERNAME_LENGTH + UnsafeNativeMethods.CREDUI_MAX_DOMAIN_TARGET_LENGTH)
			{
                throw new ArgumentException(string.Format(CultureInfo.CurrentUICulture, Properties.Resources.InvalidPrincipalLength, 
					UnsafeNativeMethods.CREDUI_MAX_USERNAME_LENGTH + UnsafeNativeMethods.CREDUI_MAX_DOMAIN_TARGET_LENGTH), 
					"principalName");
			}

			if (password.Length > UnsafeNativeMethods.CREDUI_MAX_PASSWORD_LENGTH)
			{
                throw new ArgumentException(string.Format(CultureInfo.CurrentUICulture, Properties.Resources.InvalidPasswordLength,
					UnsafeNativeMethods.CREDUI_MAX_PASSWORD_LENGTH), "password");
			}

			this.principalName = principalName;
			this.password = password.Copy();
			this.password.MakeReadOnly();
			LoadUserDomainValues();
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// User Id
		/// </summary>
		public string User
		{
			get { return this.user; }
		}

		/// <summary>
		/// Password
		/// </summary>
		public SecureString Password
		{
			get { return this.password; }
		}

		/// <summary>
		/// Domain name
		/// </summary>
		public string Domain
		{
			get { return this.domain; }
		}

		/// <summary>
		/// The principal name (domain\user);
		/// </summary>
		public string PrincipalName
		{
			get { return this.principalName; }
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// This method is for backward compatibility with APIs that does
		/// not provide the SecureString type.
		/// </summary>
		/// <returns></returns>
		public string PasswordToString()
		{
			IntPtr ptr = Marshal.SecureStringToGlobalAllocUnicode(this.password);
			try
			{
				// Unsecure managed string
				return Marshal.PtrToStringUni(ptr);
			}
			finally
			{
				Marshal.ZeroFreeGlobalAllocUnicode(ptr);
			}
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// 
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this); 
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="disposing"></param>
		public void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				this.password.Dispose();
				this.disposed = true; 
			} 
		}

		#endregion

		#region Private Methods

		private static SecureString ToSecureString(char[] password)
		{
			SecureString securePassword = new SecureString();
			foreach (char c in password)
			{
				securePassword.AppendChar(c);
			}
			securePassword.MakeReadOnly();
			return securePassword;
		}

		private static SecureString ToSecureString(StringBuilder password)
		{
			char[] pwd = new char[password.Length];
			try
			{
				password.CopyTo(0, pwd, 0, pwd.Length);
				return ToSecureString(pwd);
			}
			finally
			{
				// discard the char array
				Array.Clear(pwd, 0, pwd.Length);
			}
		}

		private void LoadUserDomainValues()
		{
            StringBuilder user = new StringBuilder(UnsafeNativeMethods.CREDUI_MAX_USERNAME_LENGTH);
            StringBuilder domain = new StringBuilder(UnsafeNativeMethods.CREDUI_MAX_DOMAIN_TARGET_LENGTH);
			UnsafeNativeMethods.CredUIReturnCodes result = UnsafeNativeMethods.CredUIParseUserNameW(this.principalName,
				user, UnsafeNativeMethods.CREDUI_MAX_USERNAME_LENGTH, domain, UnsafeNativeMethods.CREDUI_MAX_DOMAIN_TARGET_LENGTH);
			
			if (result == UnsafeNativeMethods.CredUIReturnCodes.NO_ERROR)
			{
				this.user = user.ToString();
				this.domain = domain.ToString();
			}
			else
			{
				this.user = this.principalName;
				this.domain = Environment.MachineName;
			}
		}

		#endregion
	}
}

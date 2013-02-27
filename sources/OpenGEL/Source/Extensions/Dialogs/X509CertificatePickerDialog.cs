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
using System.Windows.Forms;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Practices.RecipeFramework.Extensions.Dialogs
{
	/// <summary>
	/// X.509 Certificate picker dialog.
	/// </summary>
	[ToolboxItem(true)]
	[DesignerCategory("Dialogs")]
	[ToolboxItemFilter("Microsoft.Practices.Windows.Forms.X509CertificatePickerDialog")]
	[DefaultProperty("Certificate")]
	public class X509CertificatePickerDialog : CommonDialog
	{
		#region Fields

		private StoreName storeName;
		private StoreLocation storeLocation;
		private X509Certificate2 certificate;
		private IWin32Window owner;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="T:X509CertificatePickerDialog"/> class.
		/// </summary>
		public X509CertificatePickerDialog() : base()
		{
			this.Reset();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the name of the store.
		/// </summary>
		/// <value>The name of the store.</value>
		public StoreName StoreName
		{
			get { return this.storeName; }
			set { this.storeName = value; }
		}

		/// <summary>
		/// Gets or sets the store location.
		/// </summary>
		/// <value>The store location.</value>
		public StoreLocation StoreLocation
		{
			get { return this.storeLocation; }
			set { this.storeLocation = value; }
		}

		/// <summary>
		/// Gets or sets the certificate.
		/// </summary>
		/// <value>The certificate.</value>
		public X509Certificate2 Certificate
		{
			get { return this.certificate; }
			set { this.certificate = value; }
		}

		#endregion

		#region CommonDialog overrides

        /// <summary>
        /// Runs a common dialog box with the specified owner.
        /// </summary>
        /// <param name="owner">Any object that implements <see cref="T:System.Windows.Forms.IWin32Window"></see> that represents the top-level window that will own the modal dialog box.</param>
        /// <returns>
        /// 	<see cref="F:System.Windows.Forms.DialogResult.OK"></see> if the user clicks OK in the dialog box; otherwise, <see cref="F:System.Windows.Forms.DialogResult.Cancel"></see>.
        /// </returns>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		public new DialogResult ShowDialog(IWin32Window owner)
		{
			this.owner = owner;
			return base.ShowDialog(owner);
		}

		/// <summary>
		/// When overridden in a derived class, specifies a common dialog box.
		/// </summary>
		/// <param name="hwndOwner">A value that represents the window handle of the owner window for the common dialog box.</param>
		/// <returns>
		/// true if the dialog box was successfully run; otherwise, false.
		/// </returns>
		protected override bool RunDialog(IntPtr hwndOwner)
		{
			using (X509CertificatePickerView view = new X509CertificatePickerView(this.storeLocation, this.storeName))
			{
				DialogResult result = view.ShowDialog(this.owner);
				if (result == DialogResult.OK)
				{
					this.storeName = view.StoreName;
					this.storeLocation = view.StoreLocation;
					this.Certificate = new X509Certificate2(view.Certificate.RawData);
				}
				return result == DialogResult.OK;
			}
		}

		/// <summary>
		/// When overridden in a derived class, resets the properties of a common dialog box to their default values.
		/// </summary>
		public override void Reset()
		{
			this.StoreLocation = StoreLocation.LocalMachine;
			this.StoreName = StoreName.My;
			this.certificate = null;
		}

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="T:System.ComponentModel.Component"></see> and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (this.certificate != null)
			{
				//this.certificate.Dispose();
				this.certificate = null;
			}
		}
		#endregion
	}
}

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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Practices.RecipeFramework.Extensions.Dialogs
{
	public partial class X509CertificatePickerView : Form
	{
		private StoreName storeName;
		private StoreLocation storeLocation;
		private X509Certificate2 certificate;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:X509CertificatePickerView"/> class.
		/// </summary>
		public X509CertificatePickerView()
		{
			InitializeComponent();

			cbLocation.DataSource = Enum.GetNames(typeof(StoreLocation));
			cbStore.DataSource = Enum.GetNames(typeof(StoreName));
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:X509CertificatePickerView"/> class.
		/// </summary>
		/// <param name="storeLocation">The store location.</param>
		/// <param name="storeName">Name of the store.</param>
		public X509CertificatePickerView(StoreLocation storeLocation, StoreName storeName)
			: this()
		{
			this.storeLocation = storeLocation;
			this.storeName = storeName;
			cbLocation.SelectedItem = storeLocation.ToString();
			cbStore.SelectedItem = storeName.ToString();
		}

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

		private void bCertSelection_Click(object sender, EventArgs e)
		{
			using (X509CertificatePicker dialog = new X509CertificatePicker(this.storeName, this.storeLocation))
			{
				this.Cursor = Cursors.WaitCursor;
				this.certificate = dialog.PickCertificate(this.Handle);
				this.Cursor = Cursors.Default;
				if(this.certificate != null)
				{
					txCertificate.Text = this.certificate.SubjectName.Name;
				}
			}
		}

		private void bAccept_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void cbLocation_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.storeLocation = (StoreLocation)Enum.Parse(typeof(StoreLocation), cbLocation.SelectedItem.ToString());
		}

		private void cbStore_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.storeName = (StoreName)Enum.Parse(typeof(StoreName), cbStore.SelectedItem.ToString());
		}

		private void txCertificate_OnTextChanged(object sender, EventArgs e)
		{
			bAccept.Enabled = txCertificate.Text.Length > 0;
		}
	}
}
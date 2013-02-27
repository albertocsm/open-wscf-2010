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
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Practices.Common;
using System.ComponentModel.Design;
using Microsoft.Practices.RecipeFramework.Extensions.Dialogs;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers;

namespace Microsoft.Practices.RecipeFramework.Extensions.Editors
{
    /// <summary>
    /// The X509 certificate editor class.
    /// </summary>
    [ServiceDependency(typeof(IUIService))]
    [ServiceDependency(typeof(IDictionaryService))]
    public class X509CertificateEditor : UITypeEditor, IAttributesConfigurable
    {
        /// <summary>
        /// The <see cref="StoreName"/> argument name.
        /// </summary>
        public const string StoreNameArgumentName = "StoreNameArgument";

        /// <summary>
        /// The <see cref="StoreLocation"/> argument name.
        /// </summary>
        public const string StoreLocationArgumentName = "StoreLocationArgument";

        /// <summary>
        /// The <see cref="X509FindType"/> argument name.
        /// </summary>
        public const string FindTypeArgumentName = "FindTypeArgument";

		string storeNameArgument;
		string storeLocationArgument;
        string findTypeArgument;

        /// <summary>
        /// Gets the editor style used by the <see cref="M:System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object)"></see> method.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that can be used to gain additional context information.</param>
        /// <returns>
        /// A <see cref="T:System.Drawing.Design.UITypeEditorEditStyle"></see> value that indicates the style of editor used by the <see cref="M:System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object)"></see> method. If the <see cref="T:System.Drawing.Design.UITypeEditor"></see> does not support this method, then <see cref="M:System.Drawing.Design.UITypeEditor.GetEditStyle"></see> will return <see cref="F:System.Drawing.Design.UITypeEditorEditStyle.None"></see>.
        /// </returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        /// <summary>
        /// Edits the specified object's value using the editor style indicated by the <see cref="M:System.Drawing.Design.UITypeEditor.GetEditStyle"></see> method.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that can be used to gain additional context information.</param>
        /// <param name="provider">An <see cref="T:System.IServiceProvider"></see> that this editor can use to obtain services.</param>
        /// <param name="value">The object to edit.</param>
        /// <returns>
        /// The new value of the object. If the value of the object has not changed, this should return the same object it was passed.
        /// </returns>
        public override object EditValue(ITypeDescriptorContext context, 
            IServiceProvider provider, object value)
        {
			IUIService svc = (IUIService)provider.GetService(typeof(IUIService));
			IDictionaryService dictService = (IDictionaryService) provider.GetService(typeof(IDictionaryService));

			StoreName storeName = (StoreName) dictService.GetValue(storeNameArgument);
			StoreLocation storeLocation = (StoreLocation)dictService.GetValue(storeLocationArgument);
            X509FindType findType = (X509FindType)dictService.GetValue(findTypeArgument);

			using (X509CertificatePicker certPicker = new
			X509CertificatePicker(storeName, storeLocation))
			{
				X509Certificate2 selectedCert = certPicker.PickCertificate(
                    svc != null ? svc.GetDialogOwnerWindow().Handle : IntPtr.Zero);
				if (selectedCert != null)
				{
                    return X509CertificateHelper.GetFindValue(findType, selectedCert);
				}
				else
				{
					return null;
				}
			}
        }

		#region IAttributesConfigurable Members

        /// <summary>
        /// Configures the component using the dictionary of attributes specified
        /// in the configuration file.
        /// </summary>
        /// <param name="attributes">The attributes in the configuration element.</param>
		public void Configure(System.Collections.Specialized.StringDictionary attributes)
		{
            if (attributes.ContainsKey(StoreNameArgumentName))
			{
                storeNameArgument = attributes[StoreNameArgumentName];
			}

			if(attributes.ContainsKey(StoreLocationArgumentName))
			{
				storeLocationArgument = attributes[StoreLocationArgumentName];
			}

            if (attributes.ContainsKey(FindTypeArgumentName))
            {
                findTypeArgument = attributes[FindTypeArgumentName];
            }
		}

		#endregion
    }
}

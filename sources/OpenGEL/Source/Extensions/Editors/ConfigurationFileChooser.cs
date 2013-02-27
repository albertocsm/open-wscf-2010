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
#region Using directives
using System;
using System.Drawing.Design;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using Microsoft.Practices.Common;
using System.ComponentModel.Design;
using Microsoft.Practices.ComponentModel;
using System.Collections;
using EnvDTE;
using System.IO;
using System.Globalization;
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.Editors
{
    /// <summary>
    /// Editor that will choose a file when it is used
    /// It has properties that can be overrided 
    /// to use different filter, title and initialdirectory
    /// </summary>
    [ServiceDependency(typeof(IDictionaryService))]
    public class ConfigurationFileChooser : UITypeEditor
    {
		private IServiceProvider provider;
		private OpenFileDialog openFileDialog;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ConfigurationFileChooser"/> class.
		/// </summary>
        public ConfigurationFileChooser()
        {
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
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            this.provider = provider;

            if(provider != null)
            {
                IWindowsFormsEditorService service = 
                    (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if(service == null)
                {
                    return value;
                }

                this.openFileDialog = new OpenFileDialog();
                this.openFileDialog.Multiselect = true;
                this.InitializeDialog(this.openFileDialog);

                if(value is string)
                {
                    String filename = (String)value;
                    if (!filename.Contains("\""))
                    {
                        this.openFileDialog.FileName = filename;
                    }
                    else
                    {
                        this.openFileDialog.FileName = String.Empty;
                    }
                }

                if(this.openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (this.openFileDialog.FileNames != null && this.openFileDialog.FileNames.Length > 1)
                    {
                        String files = String.Empty;
                        foreach (String str in this.openFileDialog.FileNames)
                        {
                            files = files + "\"" + str + "\" ";
                        }
                        StringInfo si = new StringInfo(files);
                        value = si.SubstringByTextElements(0, si.LengthInTextElements - 1);
                    }
                    else
                    {
                        value = this.openFileDialog.FileName;
                    }
                }
            }
            return value;
        }

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

        private void InitializeDialog(OpenFileDialog openFileDialog)
        {
			DTE vs = (DTE)this.provider.GetService(typeof(DTE));

			openFileDialog.InitialDirectory = Path.GetDirectoryName(vs.Solution.FullName);
            openFileDialog.Title = Properties.Resources.ConfigurationFileChooserTitle;
            openFileDialog.Filter = "All Files (*.config) |*.config";
        }
    }
}

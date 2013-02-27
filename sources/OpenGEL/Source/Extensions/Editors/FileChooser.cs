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
#region Using Statements
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
    public class FileChooser : UITypeEditor, IAttributesConfigurable
    {
        private string title;
        private string filter;
        private string initialDirectory;
        private IServiceProvider provider;
        private OpenFileDialog openFileDialog;
        private const string DEFAULT_START_LOCATION = @"C:\";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:FileChooser"/> class.
        /// </summary>
        public FileChooser()
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
                IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
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

        /// <summary>
        /// Initializes the dialog.
        /// </summary>
        /// <param name="openFileDialog">The open file dialog.</param>
        private void InitializeDialog(OpenFileDialog openFileDialog)
        {
            if(String.IsNullOrEmpty(title))
            {
                this.title = Properties.Resources.DefaultFileChooserTitle;
            }

            if(String.IsNullOrEmpty(filter))
            {
                this.filter = Properties.Resources.DefaultFilterExpression;
            }

            if(String.IsNullOrEmpty(initialDirectory))
            {
                this.initialDirectory = DEFAULT_START_LOCATION;
            }

            openFileDialog.InitialDirectory = this.initialDirectory;
            openFileDialog.Title = this.title;
            openFileDialog.Filter = this.filter;
        }

        #region IAttributesConfigurable Members

        /// <summary>
        /// Configures the component using the dictionary of attributes specified
        /// in the configuration file.
        /// </summary>
        /// <param name="attributes">The attributes in the configuration element.</param>
        public void Configure(System.Collections.Specialized.StringDictionary attributes)
        {
            if(attributes.ContainsKey("Title"))
            {
                title = attributes["Title"];
            }

            if(attributes.ContainsKey("Filter"))
            {
                filter = attributes["Filter"];
            }

            if(attributes.ContainsKey("InitialDirectory"))
            {
                initialDirectory = attributes["InitialDirectory"];
            }
        }
        #endregion
    }
}

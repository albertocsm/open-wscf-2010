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
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Practices.ComponentModel;
using System.Windows.Forms.Design;
using Microsoft.Practices.Common;
using System.Collections.Specialized;

namespace Microsoft.Practices.RecipeFramework.Extensions.Editors
{
    /// <summary>
    /// Editor for selecting a folder.
    /// </summary>
    [ServiceDependency(typeof(IUIService))]
    public class FolderChooser : UITypeEditor, IAttributesConfigurable
    {
        private string description; 
        private Environment.SpecialFolder rootFolder;

        /// <summary>
        /// The root folder where the browsing starts from.
        /// Should be a value from <see cref="Environment.SpecialFolder"/> enum.
        /// </summary>
        public const string InitialDirectoryAttributeName = "InitialDirectory";

        /// <summary>
        /// The descriptive text displayed above the tree view control in the dialog box.
        /// </summary>
        public const string TitleAttributeName = "Title";

        /// <summary>
        /// The default root folder where the browsing starts from.
        /// </summary>
        public static readonly Environment.SpecialFolder DefaultInitialDirectory = Environment.SpecialFolder.Desktop;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:FolderChooser"/> class.
        /// </summary>
        public FolderChooser()
            : this(null, DefaultInitialDirectory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:FolderChooser"/> class.
        /// </summary>
        /// <param name="description">The descriptive text displayed above the tree view control in the dialog box.</param>
        public FolderChooser(string description)
            : this(description, DefaultInitialDirectory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:FolderChooser"/> class.
        /// </summary>
        /// <param name="description">The descriptive text displayed above the tree view control in the dialog box.</param>
        /// <param name="rootFolder">The root folder where the browsing starts from.</param>
        public FolderChooser(string description, Environment.SpecialFolder rootFolder)
        {
            this.description = description;
            this.rootFolder = rootFolder;
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
            IUIService svc = (IUIService)provider.GetService(typeof(IUIService));
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = this.description;
                dialog.RootFolder = this.rootFolder;
                DialogResult result = dialog.ShowDialog(svc != null ? svc.GetDialogOwnerWindow() : null);
                if (result == DialogResult.OK)
                {
                    return dialog.SelectedPath;
                }
                return value;
            }
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

        #region IAttributesConfigurable Members

        /// <summary>
        /// Configures the component using the dictionary of attributes specified
        /// in the configuration file.
        /// </summary>
        /// <param name="attributes">The attributes in the configuration element.</param>
        public void Configure(StringDictionary attributes)
        {
            if (attributes.ContainsKey(TitleAttributeName))
            {
                this.description = attributes[TitleAttributeName];
            }

            if (attributes.ContainsKey(InitialDirectoryAttributeName) &&
                Enum.IsDefined(typeof(Environment.SpecialFolder), attributes[InitialDirectoryAttributeName]))
            {
                this.rootFolder = (Environment.SpecialFolder)Enum.Parse(
                    typeof(Environment.SpecialFolder), attributes[InitialDirectoryAttributeName]);
            }
        }

        #endregion
    }
}

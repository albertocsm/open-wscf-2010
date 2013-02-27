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
using Microsoft.Practices.RecipeFramework;
using EnvDTE;
using System.IO;
using Microsoft.Practices.Common.Services;
using System.ComponentModel.Design;

namespace Microsoft.Practices.RecipeFramework.Extensions.Actions.VisualStudio
{
    /// <summary>
    /// OpenFileInBrowserAction class.
    /// </summary>
    public class OpenFileInBrowserAction : ConfigurableAction
    {
        /// <summary>
        /// Name of html documentation file
        /// </summary>
        [Input(Required = true)]
        public string DocumentationFileName
        {
            get { return documentationFileName; }
            set { documentationFileName = value; }
        } private string documentationFileName;

        /// <summary>
        /// Relative Path to the documentation file
        /// </summary>
        [Input(Required = false)]
        public string RelativeDocumentationFilePath
        {
            get { return relativeDocumentationFilePath; }
            set { relativeDocumentationFilePath = value; }
        } private string relativeDocumentationFilePath = string.Empty;


        #region ConfigurableAction Implementation

        /// <summary>
        /// See <see cref="M:Microsoft.Practices.RecipeFramework.IAction.Execute"/>.
        /// </summary>
        public override void Execute()
        {
            string path;

            if (!Path.IsPathRooted(this.documentationFileName))
            {
                TypeResolutionService service = this.GetService(typeof(ITypeResolutionService)) as TypeResolutionService;
                path = string.Concat(service.BasePath,
                    this.relativeDocumentationFilePath ?? string.Empty,
                    this.documentationFileName);
            }
            else
            {
                path = this.documentationFileName;
            }

            if (File.Exists(path))
            {
                DTE vs = (DTE)GetService(typeof(DTE));
                vs.ItemOperations.Navigate(path, vsNavigateOptions.vsNavigateOptionsNewWindow);
            }
        }

        /// <summary>
        /// See <see cref="M:Microsoft.Practices.RecipeFramework.IAction.Undo"/>.
        /// </summary>
        public override void Undo()
        {
            //Not Implemented
        }

        #endregion
    }
}

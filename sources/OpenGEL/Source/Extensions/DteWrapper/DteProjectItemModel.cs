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
using EnvDTE;
using Microsoft.Practices.RecipeFramework.Library;
using Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell.Design;
using Microsoft.VisualStudio.Shell.Interop;
using System.Diagnostics;
using System.IO;

namespace Microsoft.Practices.RecipeFramework.Extensions.DteWrapper
{
    /// <summary>
    /// Implementation of the IProjectItemModel interface that talks to the
    /// visual studio DTE object model.
    /// </summary>
    public class DteProjectItemModel : IProjectItemModel
    {
        private ProjectItem projectItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DteFolderModel"/> class.
        /// </summary>
        /// <param name="projectItem">The current projectitem.</param>
        public DteProjectItemModel(ProjectItem projectItem)
        {
            this.projectItem = projectItem;
        }

        /// <summary>
        /// Returns the actual DTE project
        /// </summary>
        public object ProjectItem
        {
            get { return this.projectItem; }
        }

        /// <summary>
        /// Returns the name of the folder
        /// </summary>        
        public string Name
        {
            get { return this.projectItem.Name; }
        }

        /// <summary>
        /// Returns the name of the folder
        /// </summary>        
        public string ItemPath
        {
            get { return Path.GetDirectoryName( this.projectItem.Properties.Item("FullPath").Value.ToString() ); }
        }

        /// <summary>
        /// Returns the project name
        /// </summary>
        /// <returns>Project name</returns>
        public override string ToString()
        {
            return projectItem.Name;
        }       
    }
}

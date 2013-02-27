//===============================================================================
// Microsoft patterns & practices
// Web Client Software Factory 2010
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
//===============================================================================
// Microsoft patterns & practices
// Web Client Software Factory
//-------------------------------------------------------------------------------
// Copyright (C) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//-------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================
using System.Collections.Generic;
using System.Text;
using EnvDTE80;
using Microsoft.Practices.RecipeFramework.VisualStudio;
using Microsoft.Practices.ComponentModel;
using EnvDTE;
using Microsoft.Practices.RecipeFramework.Library;
using System.IO;
using Microsoft.Practices.RecipeFramework;

namespace Microsoft.Practices.WebClientFactory.Actions
{
    /// <summary>
    /// Action that deletes a folder
    /// </summary>
    public class DeleteFolderAction : Action
    {
        #region Input Properties        
        private string path;
        /// <summary>
        /// Gets or sets the path of the folder to delete
        /// </summary>
        /// <value>The folder.</value>
        [Input(Required = true)]
        public string Path
        {
            get { return path; }
            set { path = value; }
        }        
        #endregion

        #region ConfigurableAction Implementation
        /// <summary>
        /// Delete Project
        /// </summary>
        public override void Execute()
        {
            if (Directory.Exists(this.path))
            {
                Directory.Delete(this.path, true);
            }           
        }       

        /// <summary>
        /// See <see cref="M:Microsoft.Practices.RecipeFramework.IAction.Undo"/>.
        /// </summary>
        public override void Undo()
        {
            // No undo supported as no Remove method exists on the VSProject.References property.
        }
        #endregion
    }
   
}

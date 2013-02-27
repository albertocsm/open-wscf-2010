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
using Microsoft.Practices.RecipeFramework.Library.Actions;
using EnvDTE;
using System.IO;
using System.Collections;
using Microsoft.Practices.RecipeFramework.Library;
using Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers;

namespace Microsoft.Practices.RecipeFramework.Extensions.Actions.VisualStudio
{
    /// <summary>
    /// Deletes an item from a project
    /// </summary>
    public class DeleteItemIfExistsFromProjectAction : Action
    {
        #region Input Properties

        private string itemName;

        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        /// <value>The name of the item.</value>
        [Input(Required = true)]
        public string ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }

        private Project project;

        /// <summary>
        /// Gets or sets the project.
        /// </summary>
        /// <value>The project.</value>
        [Input(Required = true)]
        public Project Project
        {
            get { return project; }
            set { project = value; }
        }


        #endregion


        /// <summary>
        /// Delete an item from a project
        /// </summary>
        public override void Execute()
        {
            DTE dte1 = (DTE)base.GetService(typeof(DTE));

            // The search will be recursive to explore every sub filder inside the specified project.
            ProjectItem item = DteHelperEx.FindItemByName(this.Project.ProjectItems, this.itemName, true);

            if (item != null)
            {
                item.Delete();
            }
        }

        /// <summary>
        /// This method do nothing
        /// </summary>
        public override void Undo()
        {
            //Do Nothing
        }
    }
}

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
using System.Collections.Generic;
using System.Text;
using EnvDTE80;
using Microsoft.Practices.RecipeFramework.VisualStudio;
using Microsoft.Practices.ComponentModel;
using EnvDTE;
using VsWebSite;
using Microsoft.Practices.RecipeFramework.Library; 
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.Actions.VisualStudio
{
    /// <summary>
    /// Action that deletes ProjectItems content filtering by the input kind guid
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class DeleteProjectItemsAction : ConfigurableAction 
    {
        #region Input Properties

        private ProjectItems projectItems;
        /// <summary>
        /// Gets or sets the project items.
        /// </summary>
        /// <value>The project items.</value>
        [Input(Required = false)]
        public ProjectItems ProjectItems
        {
            get { return projectItems; }
            set { projectItems = value; }
        }

        private string kind;
        /// <summary>
        /// Gets or sets the kind.
        /// </summary>
        /// <value>The kind.</value>
        [Input(Required = false)]
        public string Kind
        {
            get { return kind; }
            set { kind = value; }
        }

        private ProjectItem projectItem;
        /// <summary>
        /// Gets or sets the project item.
        /// </summary>
        /// <value>The project item.</value>
        [Input(Required = false)]
        public ProjectItem ProjectItem
        {
            get { return projectItem; }
            set { projectItem = value; }
        }

        private Project project;
        /// <summary>
        /// Gets or sets the project.
        /// </summary>
        /// <value>The project.</value>
        [Input(Required = false)]
        public Project Project
        {
            get { return project; }
            set { project = value; }
        }

        private string itemName;
        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        /// <value>The name of the item.</value>
        [Input(Required = false)]
        public string ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }
        #endregion

        #region ConfigurableAction Implementation
        /// <summary>
        /// Delete ProjectItems content filtering by the input kind guid
        /// </summary>
        public override void Execute()
        {
            if (projectItems != null)
            {
                foreach (ProjectItem projectItem in projectItems)
                {
                    if (string.IsNullOrEmpty(kind) || 
                        projectItem.Kind.Equals(kind, StringComparison.InvariantCultureIgnoreCase))
                    {
                        DTE vs = (DTE)GetService(typeof(DTE));
                        if (string.Compare(projectItem.Name, "properties", StringComparison.InvariantCultureIgnoreCase) == 0)
                        {
                            DeleteItem(projectItem, vs);
                        }
                    }
                }
            }
            else if (this.projectItem != null)
            {
                DTE vs = (DTE)GetService(typeof(DTE));
                DeleteItem(projectItem, vs);
            }
            else if (project != null && !string.IsNullOrEmpty(itemName))
            {
                DTE vs = (DTE)GetService(typeof(DTE));

                this.projectItem = DteHelper.FindItemByName(project.ProjectItems, itemName, true);

                if (this.projectItem != null)
                {
                    DeleteItem(projectItem, vs);
                }
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

        #region Private Implementation
        private void DeleteItem(ProjectItem projectItem, DTE vs)
        {
            string itemPath = DteHelper.BuildPath(projectItem);
            itemPath = DteHelper.GetPathFull(vs, itemPath);

            projectItem.Remove();
            projectItem.Delete();
        }
        #endregion
    }
}

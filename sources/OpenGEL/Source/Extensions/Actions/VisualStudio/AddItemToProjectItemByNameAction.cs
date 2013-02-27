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
using System.Text;
using Microsoft.Practices.ComponentModel;
using EnvDTE;
using System.Diagnostics;
using System.IO;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.RecipeFramework.Library;
using Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers;

namespace Microsoft.Practices.RecipeFramework.Extensions.Actions.VisualStudio
{
    /// <summary>
    /// The action creates a folder on the parent prject item on a given project
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class AddItemToProjectItemByNameAction : ConfigurableAction
    {
        private const string SolutionFolderKind = "{6BB5F8EF-4483-11D3-8BCF-00C04F8EC28C}";
        private const string DtePropertiesFullPath = "FullPath";

        #region Input Properties       

        private string targetFolderName;
        /// <summary>
        /// Name of the folder
        /// </summary>
        [Input(Required = true)]
        public string TargetFolderName
        {
            get { return targetFolderName; }
            set { targetFolderName = value; }
        }

        private Project project;
        /// <summary>
        /// Project where the item it is going to be inserted.
        /// </summary>
        [Input(Required = true)]
        public Project Project
        {
            get { return project; }
            set { project = value; }
        }

        private string parentItem;
        /// <summary>
        /// Item Name where the item it is going to be inserted.
        /// </summary>
        [Input(Required = false)]
        public string ParentItem
        {
            get { return parentItem; }
            set { parentItem = value; }
        }
        #endregion Input Properties

        #region Output Properties

        private ProjectItem projectItem;
        /// <summary>
        /// A property that can be used to pass the creted item to
        /// a following action.
        /// </summary>
        [Output]
        public ProjectItem ProjectItem
        {
            get { return projectItem; }
            set { projectItem = value; }
        }

        #endregion Output Properties

        /// <summary>
        /// The method that creates a new item on the parent item
        /// </summary>
        public override void Execute()
        {
            DTE vs = (DTE)GetService(typeof(DTE));

            if (this.parentItem != null && this.parentItem.Length > 0)
            {
                ProjectItem parentProjectItem = GetOrCreateItem(this.project.ProjectItems, this.parentItem);
                this.projectItem = GetOrCreateItem(parentProjectItem.ProjectItems, this.targetFolderName);
            }
            else
            {
                ProjectItem parentProjectItem = GetOrCreateItem(this.project.ProjectItems, this.targetFolderName);                
                this.projectItem = parentProjectItem;
            }            
        }

        private ProjectItem GetOrCreateItem(ProjectItems items, string item)
        {
            ProjectItem parentProjectItem = DteHelperEx.FindItemByName(items, item, true);
            if (parentProjectItem == null)
            {
                parentProjectItem = items.AddFolder(item, SolutionFolderKind);
            }
            return parentProjectItem;
        }

        /// <summary>
        /// Undoes the creation of the item, then deletes the item
        /// </summary>
        public override void Undo()
        {
            if (projectItem != null)
            {
                projectItem.Delete();
            }
        }                
    }
}

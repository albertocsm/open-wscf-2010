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
using System.Text;
using Microsoft.Practices.ComponentModel;
using EnvDTE;
using System.Diagnostics;
using System.IO;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.RecipeFramework.VisualStudio;
using EnvDTE80;
using Microsoft.Practices.RecipeFramework.Library; 
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.Actions.VisualStudio
{
    /// <summary>
    /// The action adds a file to the project item or solution item passed 
    /// to the action in the Content input property. 
    /// The other input properties of the action are 
    /// (a) SourceFileName - provides the name of the file 
    /// (b) SourceFilePath - provides the path name of the file
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class AddProjectItemFromFileAction : ConfigurableAction
    {
        #region Input Properties

        /// <summary>
        /// Name of the source file path
        /// </summary>
        [Input(Required = false)]
        public string SourceFilePath
        {
            get { return sourceFilePath; }
            set { sourceFilePath = value; }
        } private string sourceFilePath;

        /// <summary>
        /// TargetProject where the item is going to be inserted
        /// </summary>
        [Input(Required = false)]
        public Project TargetProject
        {
            get { return targetProject; }
            set { targetProject = value; }
        } private Project targetProject;

        /// <summary>
        /// A flag to indicate if the newly created item should be shown
        /// in a window.
        /// </summary>
        [Input(Required = false)]
        public bool Open
        {
            get { return open; }
            set { open = value; }
        } private bool open = false;

        private string itemName;
        /// <summary>
        /// ItemName where the item is going to be inserted.
        /// </summary>
        [Input(Required = false)]
        public string ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }

        private ProjectItem projectItem;
        /// <summary>
        /// ProjectItem where the item is going to be inserted.
        /// </summary>
        [Input(Required = false)]
        public ProjectItem ProjectItem
        {
            get { return projectItem; }
            set { projectItem = value; }
        }

        private bool overwriteItemIfExists = false;
        /// <summary>
        /// Gets or sets a value indicating whether [overwrite item if exists].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [overwrite item if exists]; otherwise, <c>false</c>.
        /// </value>
        [Input(Required = false)]
        public bool OverwriteItemIfExists
        {
            get { return overwriteItemIfExists; }
            set { overwriteItemIfExists = value; }
        }
        #endregion Input Properties

        #region Output Properties

        /// <summary>
        /// A property that can be used to pass the creted item to
        /// a following action.
        /// </summary>
        [Output]
        public ProjectItem OutputProjectItem
        {
            get { return outputProjectItem; }
            set { outputProjectItem = value; }
        } private ProjectItem outputProjectItem;

        #endregion Output Properties

        #region ConfigurableAction Implementation
        /// <summary>
        /// The method that creates a new item from the intput string.
        /// </summary>
        public override void Execute()
        {
            if (!string.IsNullOrEmpty(itemName))
            {
                ProjectItem item = DteHelper.FindItemByName(targetProject.ProjectItems, itemName, true);

                if (item != null)
                {
                    AddItem(item, item.ProjectItems);
                }
            }
            else if (projectItem != null)
            {
                AddItem(projectItem, projectItem.ProjectItems);
            }
            else if (targetProject != null)
            {
                AddItem(targetProject, targetProject.ProjectItems);
            }

            if (open && outputProjectItem != null)
            {
                Window wnd = outputProjectItem.Open(Constants.vsViewKindPrimary);
                wnd.Visible = true;
                wnd.Activate();
            }
        }

        /// <summary>
        /// Undoes the creation of the item, then deletes the item
        /// </summary>
        public override void Undo()
        {
            if (outputProjectItem != null)
            {
                outputProjectItem.Delete();
            }
        } 
        #endregion

        #region Private Implementation
        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <param name="projectItem">The project item.</param>
        /// <param name="projectItems">The project items.</param>
        private void AddItem(ProjectItem projectItem, ProjectItems projectItems)
        {
            if(!overwriteItemIfExists)
            {
                outputProjectItem = DteHelper.FindItemByName(projectItems, Path.GetFileName(sourceFilePath), false);
                if (outputProjectItem == null)
                {
                    outputProjectItem = projectItems.AddFromFileCopy(sourceFilePath);
                }
            }
            else
            {
                File.Copy(sourceFilePath,                    
                    Path.Combine(projectItem.Properties.Item("FullPath").Value.ToString(),
                                 Path.GetFileName(sourceFilePath)),
                    true);
            }
        }

        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="projectItems">The project items.</param>
        private void AddItem(Project project, ProjectItems projectItems)
        {
            if(!overwriteItemIfExists)
            {
                outputProjectItem = DteHelper.FindItemByName(projectItems, Path.GetFileName(sourceFilePath), false);
                if (outputProjectItem == null)
                {
                    outputProjectItem = projectItems.AddFromFileCopy(sourceFilePath);
                }
            }
            else
            {
                File.Copy(sourceFilePath,
                    Path.Combine(projectItem.Properties.Item("FullPath").Value.ToString(),
                                 Path.GetFileName(sourceFilePath)),
                    true);
            }
        }
        #endregion
    }
}

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
using System.Collections.Generic; 
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
    public class AddProjectItemsFromDirectoryAction : ConfigurableAction
    {
        #region Input Properties

        /// <summary>
        /// Name of the source file path
        /// </summary>
        [Input]
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

        private string searchPattern = "*.*";
        /// <summary>
        /// The searchPattern used to filter the files from the directory.
        /// </summary>
        [Input(Required = false)]
        public string SearchPattern
        {
            get { return searchPattern; }
            set { searchPattern = value; }
        }

        private bool open = false;
        /// <summary>
        /// A flag to indicate if the newly created item should be shown
        /// in a window.
        /// </summary>
        [Input]
        public bool Open
        {
            get { return open; }
            set { open = value; }
        }

        private bool copy = false;
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:AddProjectItemsFromDirectoryAction"/> is copy.
        /// </summary>
        /// <value><c>true</c> if copy; otherwise, <c>false</c>.</value>
        [Input(Required=false)]
        public bool Copy
        {
            get { return copy; }
            set { copy = value; }
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
        #endregion Input Properties

        #region Output Properties

        private ProjectItem[] projectItems;
        /// <summary>
        /// Returns the ProjectItem collection added.
        /// </summary>
        [Output]
        public ProjectItem[] ProjectItems
        {
            get { return projectItems; }
            set { projectItems = value; }
        } 

        #endregion Output Properties

        #region ConfigurableAction Implementation
        /// <summary>
        /// The method that creates a new item from the intput string.
        /// </summary>
        public override void Execute()
        {
            List<ProjectItem> items = new List<ProjectItem>();

            ProjectItems tempProjectItems = null;

            if (projectItem != null)
            {
                tempProjectItems = projectItem.ProjectItems;
            }
            else
            {
                tempProjectItems = targetProject.ProjectItems;
            }

            foreach (string fileName in Directory.GetFiles(sourceFilePath, searchPattern))
            {
                ProjectItem outputProjectItem;
                if (this.copy)
                {
                    outputProjectItem = tempProjectItems.AddFromFileCopy(fileName);
                }
                else
                {
                    outputProjectItem = tempProjectItems.AddFromFile(fileName);
                }

                items.Add(outputProjectItem);

                if (open && outputProjectItem != null)
                {
                    Window wnd = outputProjectItem.Open(Constants.vsViewKindPrimary);
                    wnd.Visible = true;
                    wnd.Activate();
                }
            }

            projectItems = items.ToArray();
        }

        /// <summary>
        /// Deletes the created items
        /// </summary>
        public override void Undo()
        {
            foreach (ProjectItem projectItem in projectItems)
            {
                if (projectItem != null)
                {
                    projectItem.Delete();
                }
            }
        } 
        #endregion
    }
}

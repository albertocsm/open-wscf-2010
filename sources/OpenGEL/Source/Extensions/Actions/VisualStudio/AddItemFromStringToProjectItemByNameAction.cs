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
using Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers;
using Microsoft.Practices.RecipeFramework.Library;

namespace Microsoft.Practices.RecipeFramework.Extensions.Actions.VisualStudio
{
    /// <summary>
    /// The action creates a project item from a string passed to the action
    /// in the Content input property. The other two input properties of the
    /// action are (a) targetFileName - provides the name of the item file 
    /// and (b) ProjectItem - identifies the project item where the item is created. 
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class AddItemFromStringToProjectItemByNameAction : ConfigurableAction
    {
        private const string SolutionFolderKind = "{6BB5F8EF-4483-11D3-8BCF-00C04F8EC28C}";
        private const string DtePropertiesFullPath = "FullPath";

        #region Input Properties

        private string content;
        /// <summary>
        /// The string with the content of the item. In most cases it is a class
        /// generated using the T4 engine.
        /// </summary>
        [Input(Required = true)]
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        private string targetFileName;
        /// <summary>
        /// Name of the file where the item is to be stored.
        /// </summary>
        [Input(Required = true)]
        public string TargetFileName
        {
            get { return targetFileName; }
            set { targetFileName = value; }
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

        private string itemName;
        /// <summary>
        /// Item Name where the item it is going to be inserted.
        /// </summary>
        [Input(Required = false)]
        public string ItemName
        {
            get { return itemName; }
            set { itemName = value; }
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
        /// The method that creates a new item from the intput string.
        /// </summary>
        public override void Execute()
        {
            DTE vs = (DTE)GetService(typeof(DTE));
            string tempfile = Path.GetTempFileName();

            try
            {
                using (StreamWriter sw = new StreamWriter(tempfile, false, new UTF8Encoding(true, true)))
                {
                    sw.WriteLine(content);
                }
                if (!string.IsNullOrEmpty(itemName))
                {
                    ProjectItem projectItem = DteHelperEx.FindItemByName(this.project.ProjectItems, this.itemName, true);
                    SecureAddItem(projectItem, tempfile);
                }
                else
                {
                    SecureAddItem(tempfile);
                }
            }
            finally
            {
                if (File.Exists(tempfile))
                {
                    File.Delete(tempfile);
                }
            }
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

        private void SecureAddItem(ProjectItem projectItem, string tempfile)
        {
            if (projectItem == null)
            {
                projectItem = this.project.ProjectItems.AddFolder(this.itemName, SolutionFolderKind);
            }

            string filePath = Path.Combine(projectItem.Properties.Item(DtePropertiesFullPath).Value.ToString(), targetFileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            projectItem.ProjectItems.AddFromTemplate(tempfile, targetFileName);
        }

        private void SecureAddItem(string tempfile)
        {
            string filePath = Path.Combine(this.project.Properties.Item(DtePropertiesFullPath).Value.ToString(), this.targetFileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            projectItem = this.project.ProjectItems.AddFromTemplate(tempfile, targetFileName);
        }

    }
}

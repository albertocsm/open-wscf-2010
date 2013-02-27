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
#region Using Directives

using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using EnvDTE;
using EnvDTE80;
using Microsoft.Practices.Common.Services;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers;
using Microsoft.Practices.RecipeFramework.Extensions.Properties;
using Microsoft.Practices.RecipeFramework.Library;

#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.Actions.Templates
{
    /// <summary>
    /// Unfolds a Visual Studio Template
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    [ServiceDependency(typeof(ITypeResolutionService))]
    public class UnfoldTemplateAction: ConfigurableAction
    {
        #region Input Properties

        /// <summary>
        /// The root object where the template will be unfolded.
        /// This object could be a <see cref="ProjectItem"/>, a <see cref="Project"/> or null
        /// </summary>
        [Input]
        public object Root
        {
            get { return root;  }
            set { root = value; }
        } object root;

        /// <summary>
        /// The .vstemplate file to unfold
        /// </summary>
        [Input(Required = true)]
        public string Template
        {
            get 
            {
                if (!File.Exists(template))
                {
                    TypeResolutionService typeResService =
                        (TypeResolutionService)GetService(typeof(ITypeResolutionService));
                    if (typeResService != null)
                    {
                        template = new FileInfo(
                            System.IO.Path.Combine(
                              System.IO.Path.Combine(typeResService.BasePath, @"Templates\"), 
                              template)).FullName;
                    }
                }
                return template;  
            }
            set { template = value; }
        } string template;

        /// <summary>
        /// The name of the new item after unfold
        /// </summary>
        [Input(Required = true)]
        public string ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        } string itemName;

        /// <summary>
        /// The physical locatiuon of the a project template been unfolded
        /// </summary>
        [Input]
        public string DestinationFolder
        {
            get { return destFolder; }
            set { destFolder = value; }
        } string destFolder;

        /// <summary>
        /// The path whitin a <see cref="Project"/> where the item template will be unfolded 
        /// </summary>
        [Input]
        public string Path
        {
            get { return path; }
            set { path = value; }
        } string path = string.Empty;

        #endregion

        #region Output Properties

        /// <summary>
        /// The new item just created, this object can be a <see cref="Project"/> or a <see cref="ProjectItem"/>
        /// </summary>
        [Output]
        public object NewItem
        {
            get { return newItem; }
            set { newItem = value; }
        } object newItem;

        #endregion

        #region IAction members

        /// <summary>
        /// Unfolds the template
        /// </summary>
        public override void Execute()
        {
            DTE dte = (DTE)GetService(typeof(DTE));
            Project project = DteHelperEx.FindProjectByName(dte, this.ItemName, false);

            if (project != null)
            {
                this.NewItem = project;
            }
            else
            {
                if (string.IsNullOrEmpty(this.DestinationFolder))
                {
                    this.DestinationFolder = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(dte.Solution.FileName), this.ItemName);
                    if (Directory.Exists(this.DestinationFolder))
                    {
                        Directory.Delete(this.DestinationFolder, true);
                    }
                }
                InternalExecute();
            }
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        public override void Undo()
        {
        }

        #endregion

        #region Private Implementation

        private void InternalExecute()
        {
            if (Root == null || Root is Solution)
            {
                AddProjectTemplate(null);
            }
            else if (Root is Project && ((Project)Root).Object is SolutionFolder)
            {
                AddProjectTemplate((Project)Root);
            }
            else if (Root is SolutionFolder)
            {
                AddProjectTemplate(((SolutionFolder)Root).Parent);
            }
            else if (Root is Project)
            {
                AddItemTemplate((Project)Root);
            }
        }

        private void AddItemTemplate(Project rootproject)
        {
            DTE dte = (DTE)GetService(typeof(DTE));
            string projectPath = DteHelper.BuildPath(rootproject);
            string folderPath = projectPath;
            if (!string.IsNullOrEmpty(this.Path))
            {
                folderPath = System.IO.Path.Combine(folderPath, this.Path);
            }
            ProjectItem prItem = DteHelper.FindItemByPath(dte.Solution, folderPath);
            if (prItem != null)
            {
                this.NewItem = prItem.ProjectItems.AddFromTemplate(this.Template, this.ItemName);
            }
            else
            {
                Project project = DteHelper.FindProjectByPath(dte.Solution, folderPath);
                if (project != null)
                {
                    project.ProjectItems.AddFromTemplate(this.Template, this.ItemName);
                }
                else
                {
                    throw new InvalidOperationException(
                        String.Format(CultureInfo.CurrentUICulture, Resources.InsertionPointException, folderPath));
                }
            }
        }

        private void AddProjectTemplate(Project project)
        {
            DTE dte = (DTE)GetService(typeof(DTE));
            SolutionFolder slnFolder = null;

            if (project == null)
            {
                if (string.IsNullOrEmpty(this.Path))
                {
                    this.NewItem = dte.Solution.AddFromTemplate(this.Template, this.DestinationFolder, this.ItemName, false);
                }
                else
                {
                    Project subProject = DteHelper.FindProjectByPath(dte.Solution, this.Path);
                    slnFolder = (SolutionFolder)subProject.Object;
                    this.NewItem = slnFolder.AddFromTemplate(this.Template, this.DestinationFolder, this.ItemName);
                }
            }
            else
            {
                slnFolder = (SolutionFolder)project.Object;
                this.NewItem = slnFolder.AddFromTemplate(this.Template, this.DestinationFolder, this.ItemName);
            }

            if (this.newItem == null)
            {
                //Return the project already added if the AddFromTemplate method returns null
                ProjectItems childItems;

                if (slnFolder != null)
                {
                    childItems = slnFolder.Parent.ProjectItems;
                }
                else
                {
                    childItems = dte.Solution.Projects as ProjectItems;
                }

                if (childItems != null)
                {
                    foreach (ProjectItem item in childItems)
                    {
                        if (item.Name.Contains(this.ItemName))
                        {
                            this.NewItem = item.Object as Project;
                            break;
                        }
                    }
                }
                else
                {
                    this.NewItem = DteHelperEx.FindProjectByName(dte, this.ItemName, false);
                }
            }
        }

        #endregion
    }
}

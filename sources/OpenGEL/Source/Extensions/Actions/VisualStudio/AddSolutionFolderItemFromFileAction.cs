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
using EnvDTE;
using Microsoft.Practices.RecipeFramework.Library;
using EnvDTE80;
using System.IO; 
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.Actions.VisualStudio
{
    /// <summary>
    /// Action that adds an item to a Solution Folder
    /// </summary>
    public class AddSolutionFolderItemFromFileAction : ConfigurableAction
    {
        #region Input Properties

        /// <summary>
        /// Gets or sets the source file path.
        /// </summary>
        /// <value>The source file path.</value>
        [Input(Required = true)]
        public string SourceFilePath
        {
            get { return sourceFilePath; }
            set { sourceFilePath = value; }
        } private string sourceFilePath;

        /// <summary>
        /// Gets or sets the solution folder.
        /// </summary>
        /// <value>The solution folder.</value>
        [Input(Required = true)]
        public SolutionFolder SolutionFolder
        {
            get { return solutionFolder; }
            set { solutionFolder = value; }
        }
        private SolutionFolder solutionFolder;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:AddSolutionFolderItemFromFileAction"/> is open.
        /// </summary>
        /// <value><c>true</c> if open; otherwise, <c>false</c>.</value>
        [Input(Required = false)]
        public bool Open
        {
            get { return open; }
            set { open = value; }
        } 
        private bool open = false;

        /// <summary>
        /// Gets or sets the relative path.
        /// </summary>
        /// <value>The relative path.</value>
        [Input(Required = false)]
        public string RelativePath
        {
            get { return relativePath; }
            set { relativePath = value; }
        } private string relativePath;

        /// <summary>
        /// Gets or sets the solution.
        /// </summary>
        /// <value>The solution.</value>
        [Input(Required = false)]
        public Solution Solution
        {
            get { return solution; }
            set { solution = value; }
        } private Solution solution;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:AddSolutionFolderItemFromFileAction"/> is copy.
        /// </summary>
        /// <value><c>true</c> if copy; otherwise, <c>false</c>.</value>
        [Input(Required = false)]
        public bool Copy
        {
            get { return copy; }
            set { copy = value; }
        } private bool copy = true;
        #endregion

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
        /// See <see cref="M:Microsoft.Practices.RecipeFramework.IAction.Execute"/>.
        /// </summary>
        public override void Execute()
        {
            if(this.copy)
            {
                if(!string.IsNullOrEmpty(relativePath))
                {
                    string destinationPath = string.Concat(
                                    Path.GetDirectoryName(Solution.Properties.Item("Path").Value.ToString()),
                                    relativePath,
                                    Path.GetFileName(sourceFilePath));

                    File.Copy(sourceFilePath, destinationPath, true);

                    outputProjectItem = solutionFolder.Parent.ProjectItems.AddFromFile(destinationPath);
                }
                else
                {
                    outputProjectItem = solutionFolder.Parent.ProjectItems.AddFromFileCopy(sourceFilePath);
                }
            }
            else
            {
                if(!string.IsNullOrEmpty(relativePath))
                {
                    string destinationPath = string.Concat(
                                    Path.GetDirectoryName(Solution.Properties.Item("Path").Value.ToString()),
                                    relativePath,
                                    Path.GetFileName(sourceFilePath));

                    outputProjectItem = solutionFolder.Parent.ProjectItems.AddFromFile(destinationPath);
                }
                else
                {
                    outputProjectItem = solutionFolder.Parent.ProjectItems.AddFromFile(sourceFilePath);
                }            
            }

            if(open && outputProjectItem != null)
            {
                Window wnd = outputProjectItem.Open(Constants.vsViewKindPrimary);
                wnd.Visible = true;
                wnd.Activate();
            }
        }

        /// <summary>
        /// See <see cref="M:Microsoft.Practices.RecipeFramework.IAction.Undo"/>.
        /// </summary>
        public override void Undo()
        {
            if(outputProjectItem != null)
            {
                outputProjectItem.Delete();
            }
        } 
        #endregion
    }
}

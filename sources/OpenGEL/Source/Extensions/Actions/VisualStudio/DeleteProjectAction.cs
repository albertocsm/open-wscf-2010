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
using System.IO; 
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.Actions.VisualStudio
{
    /// <summary>
    /// Action that deletes a Project
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class DeleteProjectAction : ConfigurableAction 
    {
        #region Input Properties

        private Project project;
        /// <summary>
        /// Gets or sets the project to delete.
        /// </summary>
        /// <value>The project.</value>
        [Input(Required = true)]
        public Project Project
        {
            get { return project; }
            set { project = value; }
        }

        private bool deleteProjectFiles = false;
        /// <summary>
        /// Gets or sets if you want to delete the project file (csproj) and bin obj dirs 
        /// </summary>
        /// <value>The project.</value>
        [Input(Required = false)]
        public bool DeleteProjectFiles
        {
            get { return deleteProjectFiles; }
            set { deleteProjectFiles = value; }
        }      
        #endregion

        #region ConfigurableAction Implementation
        /// <summary>
        /// Delete Project
        /// </summary>
        public override void Execute()
        {            
            DTE vs = (DTE)GetService(typeof(DTE));
            string projectFilePath = this.project.FullName;
            vs.Solution.Remove(this.project);
            if (this.deleteProjectFiles)
            {
                string projectPath = Path.GetDirectoryName(projectFilePath);
                if (File.Exists(projectFilePath))
                {
                    File.Delete(projectFilePath);
                    Directory.Delete(Path.Combine(projectPath, "bin"), true);
                    Directory.Delete(Path.Combine(projectPath, "obj"), true);                    
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
    }
}

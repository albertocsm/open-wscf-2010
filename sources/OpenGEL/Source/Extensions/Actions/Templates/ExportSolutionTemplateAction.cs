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
using Microsoft.Practices.RecipeFramework.Extensions.Common.Templates;
using System.IO;
using Microsoft.Practices.ComponentModel; 
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.Actions.Templates
{
    /// <summary>
    /// Export a Visual Studio Solution Template including all projects inside solution
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class ExportSolutionTemplateAction : ConfigurableAction
    {
        #region Fields

        private string destinationPath; 

        #endregion

        #region Input Properties
        /// <summary>
        /// Gets or sets the destination path.
        /// </summary>
        /// <value>The destination path.</value>
        [Input(Required = true)]
        public string DestinationPath
        {
            get { return destinationPath; }
            set { destinationPath = value; }
        }

        private string projectsBaseDirectory;

        /// <summary>
        /// Gets or sets the projects base directory.
        /// </summary>
        /// <value>The projects base directory.</value>
        [Input(Required = true)]
        public string ProjectsBaseDirectory
        {
            get { return projectsBaseDirectory; }
            set { projectsBaseDirectory = value; }
        }

        private string solutionTemplateIconFile;

        /// <summary>
        /// Gets or sets the solution template icon file.
        /// </summary>
        /// <value>The solution template icon file.</value>
        [Input(Required = true)]
        public string SolutionTemplateIconFile
        {
            get { return solutionTemplateIconFile; }
            set { solutionTemplateIconFile = value; }
        }

        private string projectsTemplateIconFile;

        /// <summary>
        /// Gets or sets the projects template icon file.
        /// </summary>
        /// <value>The projects template icon file.</value>
        [Input(Required = true)]
        public string ProjectsTemplateIconFile
        {
            get { return projectsTemplateIconFile; }
            set { projectsTemplateIconFile = value; }
        }

        #endregion

        #region ConfigurableAction Implementation
        /// <summary>
        /// See <see cref="M:Microsoft.Practices.RecipeFramework.IAction.Execute"/>.
        /// </summary>
        public override void Execute()
        {
            DTE dte = GetService<DTE>(true);
            Solution solution = dte.Solution;

            VSSolutionTemplateExporter solutionExporter =
                new VSSolutionTemplateExporter(
                    solution,
                    this.DestinationPath,
                    this.ProjectsBaseDirectory,
                    this.SolutionTemplateIconFile,
                    this.ProjectsTemplateIconFile);

            solutionExporter.Export();
        }


        /// <summary>
        /// See <see cref="M:Microsoft.Practices.RecipeFramework.IAction.Undo"/>.
        /// </summary>
        public override void Undo()
        {
            if (Directory.Exists(this.DestinationPath))
            {
                Directory.Delete(this.DestinationPath);
            }
        } 

        #endregion
    }
}

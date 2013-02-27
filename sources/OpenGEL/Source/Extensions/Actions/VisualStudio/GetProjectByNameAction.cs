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
#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.RecipeFramework.Library.Actions;
using EnvDTE;
using System.IO;
using Microsoft.Practices.RecipeFramework.VisualStudio;
using System.Collections;
using Microsoft.Practices.RecipeFramework.Library;
using Microsoft.Practices.Common.Services;
using System.ComponentModel.Design; 
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.Actions.VisualStudio
{
    /// <summary>
    /// Gets a proyect by name.
    /// </summary>
	public class GetProjectByNameAction : ConfigurableAction
    {
        #region Input Properties

		private string projectName;

        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        /// <value>The name of the project.</value>
        [Input(Required = true)]
		public string ProjectName
        {
			get { return projectName; }
			set { projectName = value; }
        }

		private Project project;

        /// <summary>
        /// Gets or sets the project.
        /// </summary>
        /// <value>The project.</value>
		[Output]
		public Project Project
		{
			get { return project; }
			set { project = value; }
		}
        #endregion

		#region ConfigurableAction implementation

		/// <summary>
		/// Delete an item from a project
		/// </summary>
		public override void Execute()
		{
			DTE dte = GetService<DTE>();

			this.project = DteHelper.FindProjectByPath(dte.Solution, this.projectName);
		}

		/// <summary>
		/// This method do nothing
		/// </summary>
		public override void Undo()
		{
			//Do Nothing
		} 
		#endregion
    }
}

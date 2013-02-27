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
using Microsoft.Practices.ComponentModel; 
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.Actions.VisualStudio
{
	/// <summary>
	/// Adds a solution folder item.
	/// </summary>
	[ServiceDependency(typeof(DTE))]
	public class AddSolutionFolderAction : ConfigurableAction
    {
        #region Input Properties

		/// <summary>
		/// Gets or sets the name of the solution folder.
		/// </summary>
		/// <value>The name of the solution folder.</value>
        [Input(Required = true)]
		public string SolutionFolderName
        {
			get { return solutionFolderName; }
			set { solutionFolderName = value; }
		} private string solutionFolderName;

        #endregion

		#region ConfigurableAction implementation

		/// <summary>
		/// Delete an item from a project
		/// </summary>
		public override void Execute()
		{
			DTE vs = GetService<DTE>(true);
			
			EnvDTE80.Solution2 sln = (EnvDTE80.Solution2)vs.Solution;
			if (DteHelper.FindSolutionFolderByPath(vs.Solution, solutionFolderName) == null)
			{
				sln.AddSolutionFolder(solutionFolderName);
			}
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

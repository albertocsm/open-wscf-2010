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
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.Actions.VisualStudio
{
    /// <summary>
    /// Deletes an item from a project
    /// </summary>
    public class DeleteSolutionItemIfExistsFromProjectAction : ConfigurableAction
    {
        #region Input Properties

        private string itemName;

        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        /// <value>The name of the item.</value>
        [Input(Required = true)]
        public string ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }

        private Project project;

        /// <summary>
        /// Gets or sets the project.
        /// </summary>
        /// <value>The project.</value>
        [Input(Required = true)]
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
			foreach(ProjectItem item in Project.ProjectItems)
			{
				DTE vs = GetService<DTE>(true);

				if(item.Name.Equals(itemName, StringComparison.InvariantCultureIgnoreCase))
				{
					string itemPath = Path.Combine(
						Path.GetDirectoryName(vs.Solution.FileName),
						item.Name);
					item.Delete();
					File.Delete(itemPath);
					break;
				}
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

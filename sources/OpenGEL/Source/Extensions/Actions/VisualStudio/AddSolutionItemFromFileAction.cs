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
using System.IO;
using EnvDTE; 
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.Actions.VisualStudio
{
    /// <summary>
    /// Action that adds an item to the solution
    /// </summary>
	public class AddSolutionItemFromFileAction : ConfigurableAction
	{
		#region Input Properties

		private string sourceFile;

        /// <summary>
        /// Gets or sets the source file.
        /// </summary>
        /// <value>The source file.</value>
		[Input()]
		public string SourceFile
		{
			get { return sourceFile; }
			set { sourceFile = value; }
		}

		#endregion

		#region Configurable action implementation
        /// <summary>
        /// See <see cref="M:Microsoft.Practices.RecipeFramework.IAction.Execute"/>.
        /// </summary>
		public override void Execute()
		{
			DTE vs = (DTE)GetService(typeof(DTE));

			if(File.Exists(this.sourceFile))
			{
				string destinationFile = Path.GetDirectoryName(vs.Solution.Properties.Item("Path").Value.ToString()) +
					"\\" +
					Path.GetFileName(this.sourceFile);

				File.Copy(this.sourceFile, destinationFile, true);
			}
		}

        /// <summary>
        /// See <see cref="M:Microsoft.Practices.RecipeFramework.IAction.Undo"/>.
        /// </summary>
		public override void Undo()
		{
			//Not Implemented
		} 
		#endregion
	}
}

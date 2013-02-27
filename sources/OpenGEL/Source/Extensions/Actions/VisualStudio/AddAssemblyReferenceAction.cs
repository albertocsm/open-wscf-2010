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
using System.Collections;
using System.ComponentModel;
using System.IO;
using EnvDTE;
using VSLangProj;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework.Library;
using VsWebSite;
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.Actions.VisualStudio
{
	/// <summary>
	/// Adds a reference to a project pointing to another 
	/// project in the same solution. 
	/// </summary>
	[ServiceDependency(typeof(DTE))]
	public class AddAssemblyReferenceAction : ConfigurableAction
	{
		#region Inputs

        /// <summary>
        /// The project where the reference is been added
        /// </summary>
		[Input(Required = true)]
		public Project ReferringProject
		{
			get { return referringProject; }
			set { referringProject = value; }
		} Project referringProject;

        /// <summary>
        /// The file name reference
        /// </summary>
		[Input(Required = true)]
		public string AssemblyFilePath
		{
            get { return assemblyFilePath; }
            set { assemblyFilePath = value; }
        } string assemblyFilePath;

		#endregion

        /// <summary>
        /// Adds the reference to the project
        /// </summary>
		public override void Execute()
		{
            if (DteHelper.IsWebProject(referringProject))
            {
                // This is a web project
                VSWebSite webProject = referringProject.Object as VSWebSite;
                if (webProject != null)
                {
                    if (File.Exists(this.assemblyFilePath))
                    {
                        webProject.References.AddFromFile(this.assemblyFilePath);
                    }
                    else
                    {
                        webProject.References.AddFromGAC(this.assemblyFilePath);
                    }
                }
            }
            else
            {
                // This is a standard project
                VSProject vsProject = referringProject.Object as VSProject;
                if (vsProject != null)
                {
                    vsProject.References.Add(this.assemblyFilePath);
                }
            }
		}

        /// <summary>
        /// Not implemented
        /// </summary>
		public override void Undo()
		{
			// No undo supported as no Remove method exists on the VSProject.References property.
		}
    }
}


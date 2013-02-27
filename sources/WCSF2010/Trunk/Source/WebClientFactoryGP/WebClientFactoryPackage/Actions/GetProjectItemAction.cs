//===============================================================================
// Microsoft patterns & practices
// Web Client Software Factory 2010
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
//===============================================================================
// Microsoft patterns & practices
// Web Client Software Factory
//-------------------------------------------------------------------------------
// Copyright (C) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//-------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================
using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE;
using Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers;
using Microsoft.Practices.RecipeFramework;

namespace Microsoft.Practices.WebClientFactory.Actions
{
	public class GetProjectItemAction : ConfigurableAction
	{
		// Fields
		private string itemName;
		private Project project;
		private ProjectItem projectItem;

		// Methods
		public override void Execute()
		{
			this.ProjectItem = DteHelperEx.FindItemByName(this.Project.ProjectItems, this.ItemName, true);
		}

		public override void Undo()
		{
			this.ProjectItem = null;
		}

		// Properties
		[Input(Required = true)]
		public string ItemName
		{
			get
			{
				return this.itemName;
			}
			set
			{
				this.itemName = value;
			}
		}

		[Input(Required = true)]
		public Project Project
		{
			get
			{
				return this.project;
			}
			set
			{
				this.project = value;
			}
		}

		[Output]
		public ProjectItem ProjectItem
		{
			get
			{
				return this.projectItem;
			}
			set
			{
				this.projectItem = value;
			}
		}
	}
}

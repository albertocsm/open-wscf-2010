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
using System.Collections.Generic;
using Microsoft.Practices.CompositeWeb.Utility;

namespace Microsoft.Practices.CompositeWeb.Authorization
{
	/// <summary>
	/// Defines a set of actions available in a module.
	/// </summary>
	public class ModuleActionSet
	{
		private IList<Action> _actions;
		private string _moduleName;

		/// <summary>
		/// Initializes a new instance of <see cref="ModuleActionSet"/>.
		/// </summary>
		/// <param name="moduleName">The name of the module this set is for.</param>
		/// <param name="actions">A <see cref="List{Action}"/> with the actions available in the module.</param>
		public ModuleActionSet(string moduleName, IList<Action> actions)
		{
			Guard.ArgumentNotNull(moduleName, "moduleName");

			ModuleName = moduleName;
			_actions = actions ?? new List<Action>();
		}

		/// <summary>
		/// Gets or sets the module name.
		/// </summary>
		public string ModuleName
		{
			get { return _moduleName; }
			set { _moduleName = value; }
		}

		/// <summary>
		/// Gets the list of actions available in the module.
		/// </summary>
		public IList<Action> Actions
		{
			get { return _actions; }
		}
	}
}
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
using Microsoft.Practices.CompositeWeb.Interfaces;

namespace Microsoft.Practices.CompositeWeb.Authorization
{
	/// <summary>
	/// Implements a catalog of <see cref="ModuleActionSet"/> objects.
	/// </summary>
	public class PermissionsCatalog : IPermissionsCatalog
	{
		private Dictionary<string, ModuleActionSet> _modulePermissionSet;

		/// <summary>
		/// Initializes a new instance of <see cref="PermissionsCatalog"/>.
		/// </summary>
		public PermissionsCatalog()
		{
			_modulePermissionSet = new Dictionary<string, ModuleActionSet>();
		}

		#region IPermissionsCatalog Members

		/// <summary>
		/// Registers a <see cref="ModuleActionSet"/> with the catalog.
		/// </summary>
		/// <param name="set">A <see cref="ModuleActionSet"/>.</param>
		public void RegisterPermissionSet(ModuleActionSet set)
		{
			_modulePermissionSet.Add(set.ModuleName, set);
		}

		/// <summary>
		/// Gets a <see cref="Dictionary{TKey,TValue}"/> with the registered modules.
		/// </summary>
		public Dictionary<string, ModuleActionSet> RegisteredPermissions
		{
			get { return _modulePermissionSet; }
		}

		#endregion
	}
}
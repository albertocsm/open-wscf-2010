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
using System.Web.Security;
using Microsoft.Practices.CompositeWeb.Interfaces;

namespace Microsoft.Practices.CompositeWeb.Providers
{
	/// <summary>
	/// Extends the <see cref="SqlRoleProvider"/> by using the <see cref="IRolesCatalog"/>.
	/// </summary>
	public class RoleProvider : SqlRoleProvider
	{
		private IRolesCatalog _rolesCatalog;

		/// <summary>
		/// Gets or sets the <see cref="IRolesCatalog"/>.
		/// </summary>
		[ServiceDependency]
		public IRolesCatalog RolesCatalog
		{
			get { return _rolesCatalog; }
			set { _rolesCatalog = value; }
		}

		/// <summary>
		/// Loads the roles from the provider into the catalog.
		/// </summary>
		public void LoadRoles()
		{
			string[] roles = GetAllRoles();

			RolesCatalog.LoadRoles(roles);
		}
	}
}
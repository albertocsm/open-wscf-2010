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
using Microsoft.Practices.CompositeWeb.Authorization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests.Services
{
	/// <summary>
	/// Summary description for PermissionsCatalogFixture
	/// </summary>
	[TestClass]
	public class PermissionsCatalogFixture
	{
		[TestMethod]
		public void TestInitialization()
		{
			PermissionsCatalog catalog = new PermissionsCatalog();

			Assert.IsNotNull(catalog.RegisteredPermissions);
			Assert.AreEqual(0, catalog.RegisteredPermissions.Count);
		}

		[TestMethod]
		public void TestRegisterPermissionSet()
		{
			PermissionsCatalog catalog = new PermissionsCatalog();
			ModuleActionSet set = new ModuleActionSet("module", null);
			catalog.RegisterPermissionSet(set);

			Assert.AreEqual(set, catalog.RegisteredPermissions[set.ModuleName]);
		}
	}
}
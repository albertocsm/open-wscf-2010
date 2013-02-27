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
using System.Diagnostics;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Authorization;
using Microsoft.Practices.CompositeWeb.EnterpriseLibrary.Services;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Services;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb.ObjectBuilder;
using GlobalBank.Commercial.EBanking.Modules.Shell.Tests.Mocks;

namespace GlobalBank.Commercial.EBanking.Modules.Shell.Tests
{
	[TestClass]
	public class ShellModuleInitializerFixture
	{
		//private TestContext _testContext;

		//public TestContext TestContext
		//{
		//    get { return _testContext; }
		//    set { _testContext = value; }
		//}

		[TestMethod]
		public void ShellModuleInitializerIsIModuleInitializer()
		{
			Assert.IsTrue(typeof(IModuleInitializer).IsAssignableFrom(typeof(ShellModuleInitializer)));
		}

		[TestMethod]
		public void LoadRegistersServicesInRootContainer()
		{
			MockRootCompositionContainer rootContainer = new MockRootCompositionContainer();
			CompositionContainer moduleContainer = rootContainer.Containers.AddNew<CompositionContainer>();
			ShellModuleInitializer moduleInitializer = new ShellModuleInitializer();

			moduleInitializer.Load(moduleContainer);

			IAuthorizationService authorizationService = rootContainer.Services.Get<IAuthorizationService>(true);
			Assert.IsTrue(authorizationService is EnterpriseLibraryAuthorizationService);

			IPermissionsCatalog permissionsCatalog = rootContainer.Services.Get<IPermissionsCatalog>(true);
			Assert.IsTrue(permissionsCatalog is PermissionsCatalog);

			IRolesCatalog rolesCatalog = rootContainer.Services.Get<IRolesCatalog>();
			Assert.IsTrue(rolesCatalog is RolesCatalog);

			ISiteMapBuilderService siteMapBuilderService = rootContainer.Services.Get<ISiteMapBuilderService>();
			Assert.IsTrue(siteMapBuilderService is SiteMapBuilderService);
		}
	}

	namespace Mocks
	{
		class MockRootCompositionContainer : CompositionContainer
		{
			public MockRootCompositionContainer()
			{
				InitializeRootContainer(CreateBuilder());
			}

			private WCSFBuilder CreateBuilder()
			{
				WCSFBuilder builder = new WCSFBuilder();
				builder.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));
				return builder;
			}
		}
	}
}

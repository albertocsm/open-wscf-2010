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


using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Authorization;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Services;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb.ObjectBuilder;
using GlobalBank.Commercial.EBanking.Modules.Reports.Tests.Mocks;

namespace GlobalBank.Commercial.EBanking.Modules.Reports.Tests
{
	[TestClass]
	public class ReportsModuleInitializerFixture
	{
		[TestMethod]
		public void ReportsModuleInitializerIsIModuleInitializer()
		{
			Assert.IsTrue(typeof(IModuleInitializer).IsAssignableFrom(typeof(ReportsModuleInitializer)));
		}

		[TestMethod]
		public void RegisterSiteMapInformationRegistersOneNodeWithTwoChilds()
		{
			TestableModule module = new TestableModule();
			SiteMapBuilderService siteMapBuilderService = new SiteMapBuilderService();

			module.TestRegisterSiteMapInformation(siteMapBuilderService);

			ReadOnlyCollection<SiteMapNodeInfo> nodes = siteMapBuilderService.GetChildren(siteMapBuilderService.RootNode.Key);
			Assert.AreEqual(1, nodes.Count);
			Assert.AreEqual(2, siteMapBuilderService.GetChildren(nodes[0].Key).Count);
		}

		[TestMethod]
		public void RegisterRequiredPermissionsRegistersOnePermissionSetWithTwoActions()
		{
			TestableModule module = new TestableModule();
			MockPermissionCatalog permissionCatalog = new MockPermissionCatalog();

			module.TestRegisterRequiredPermissions(permissionCatalog);

			Assert.AreEqual(1, permissionCatalog.RegisteredPermissions.Count);
			Assert.AreEqual("Reports", permissionCatalog.RegisteredSet.ModuleName);
			Assert.AreEqual(2, permissionCatalog.RegisteredSet.Actions.Count);
			List<Action> actions = new List<Action>(permissionCatalog.RegisteredSet.Actions);
			Assert.IsTrue(actions.Exists(delegate(Action action) { return action.RuleName == Constants.Permissions.AllowViewPaymentHistory; }));
			Assert.IsTrue(actions.Exists(delegate(Action action) { return action.RuleName == Constants.Permissions.AllowViewAccountsSummary; }));
		}

		[TestMethod]
		public void OnLoadRegistersRequiredPermissionsAndSiteMapNodesUsingServicesInModuleContainer()
		{
			MockRootCompositionContainer rootContainer = new MockRootCompositionContainer();
			MockPermissionCatalog permissionCatalog = rootContainer.Services.AddNew<MockPermissionCatalog, IPermissionsCatalog>();
			SiteMapBuilderService siteMapBuilderService = rootContainer.Services.AddNew<SiteMapBuilderService, ISiteMapBuilderService>();
			CompositionContainer moduleContainer = rootContainer.Containers.AddNew<CompositionContainer>();
			TestableModule module = new TestableModule();

			module.Load(moduleContainer);

			Assert.IsTrue(module.RegisterRequiredPermissionsCalled);
			Assert.IsTrue(module.RegisterSiteMapInformationCalled);
			Assert.AreSame(siteMapBuilderService, module.SiteMapBuilderService);
			Assert.AreSame(permissionCatalog, module.PermissionCatalog);
		}
	}

	class TestableModule : ReportsModuleInitializer
	{
		public bool RegisterRequiredPermissionsCalled = false;
		public bool RegisterSiteMapInformationCalled = false;
		public ISiteMapBuilderService SiteMapBuilderService = null;
		public IPermissionsCatalog PermissionCatalog = null;

		internal void TestRegisterRequiredPermissions(IPermissionsCatalog permissionsCatalog)
		{
			base.RegisterRequiredPermissions(permissionsCatalog);
		}

		internal void TestRegisterSiteMapInformation(ISiteMapBuilderService siteMapBuilderService)
		{
			base.RegisterSiteMapInformation(siteMapBuilderService);
		}

		protected override void RegisterRequiredPermissions(IPermissionsCatalog permissionCatalog)
		{
			RegisterRequiredPermissionsCalled = true;
			PermissionCatalog = permissionCatalog;
		}

		protected override void RegisterSiteMapInformation(ISiteMapBuilderService siteMapBuilderService)
		{
			RegisterSiteMapInformationCalled = true;
			SiteMapBuilderService = siteMapBuilderService;
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

		class MockPermissionCatalog : IPermissionsCatalog
		{
			public ModuleActionSet RegisteredSet = null;
			private Dictionary<string, ModuleActionSet> _registeredPermissions = new Dictionary<string, ModuleActionSet>();

			#region IPermissionsCatalog Members

			public void RegisterPermissionSet(ModuleActionSet set)
			{
				RegisteredSet = set;
				_registeredPermissions.Add(set.ModuleName, set);
			}

			public Dictionary<string, ModuleActionSet> RegisteredPermissions
			{
				get { return _registeredPermissions; }
			}

			#endregion
		}
	}
}

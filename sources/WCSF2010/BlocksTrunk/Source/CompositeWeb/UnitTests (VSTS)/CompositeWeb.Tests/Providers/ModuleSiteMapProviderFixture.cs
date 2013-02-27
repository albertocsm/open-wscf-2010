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
using System.Web;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Providers;
using Microsoft.Practices.CompositeWeb.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests.Providers
{
	/// <summary>
	/// Summary description for ModuleSiteMapProviderFixture
	/// </summary>
	[TestClass]
	public class ModuleSiteMapProviderFixture
	{
		[TestMethod]
		public void ModuleSiteMapProviderIsStaticSiteMapProvider()
		{
			ModuleSiteMapProvider provider = new ModuleSiteMapProvider();

			Assert.IsTrue(provider is StaticSiteMapProvider);
		}

		[TestMethod]
		public void TestGetRootNodeCoreReturnsProviderManagedRootNode()
		{
			ModuleSiteMapProvider provider = new ModuleSiteMapProvider();
			provider.AuthorizationService = new MockAuthorizationService();
			SiteMapBuilderService siteMapBuilder = new SiteMapBuilderService();
			provider.SiteMapBuilder = siteMapBuilder;

			Assert.IsNotNull(provider.RootNode);
			Assert.AreSame(provider, provider.RootNode.Provider);
		}

		[TestMethod]
		public void RootNodeContainsAddedChild()
		{
			ModuleSiteMapProvider provider = new ModuleSiteMapProvider();
			provider.AuthorizationService = new MockAuthorizationService();
			SiteMapBuilderService siteMapBuilder = new SiteMapBuilderService();
			provider.SiteMapBuilder = siteMapBuilder;

			SiteMapNodeInfo child = new SiteMapNodeInfo("child");
			siteMapBuilder.AddNode(child);

			Assert.AreEqual(1, provider.RootNode.ChildNodes.Count);
			Assert.AreEqual("child", provider.RootNode.ChildNodes[0].Key);
		}

		[TestMethod]
		public void CanBuildSimpleTree()
		{
			ModuleSiteMapProvider provider = new ModuleSiteMapProvider();
			provider.AuthorizationService = new MockAuthorizationService();
			SiteMapBuilderService siteMapBuilder = new SiteMapBuilderService();
			provider.SiteMapBuilder = siteMapBuilder;

			SiteMapNodeInfo child = new SiteMapNodeInfo("child");
			SiteMapNodeInfo grandChild = new SiteMapNodeInfo("grandChild");
			SiteMapNodeInfo greatGrandChild = new SiteMapNodeInfo("greatGrandChild");
			siteMapBuilder.AddNode(child);
			siteMapBuilder.AddNode(grandChild, child);
			siteMapBuilder.AddNode(greatGrandChild, grandChild);

			Assert.AreEqual(1, provider.RootNode.ChildNodes.Count);
			Assert.AreEqual("child", provider.RootNode.ChildNodes[0].Key);
			Assert.AreEqual("grandChild", provider.RootNode.ChildNodes[0].ChildNodes[0].Key);
			Assert.AreEqual("greatGrandChild", provider.RootNode.ChildNodes[0].ChildNodes[0].ChildNodes[0].Key);
		}

		[TestMethod]
		public void UserDeniedAccessToNodeWhenLackingRequiredRole()
		{
			TestableModuleSiteMapProvider provider = new TestableModuleSiteMapProvider();
			SiteMapBuilderService siteMapBuilder = new SiteMapBuilderService();
			MockAuthorizationService authorizationService = new MockAuthorizationService();
			authorizationService.ShouldAuthorize = false;
			provider.SiteMapBuilder = siteMapBuilder;
			provider.AuthorizationService = authorizationService;

			SiteMapNodeInfo child = new SiteMapNodeInfo("node");
			siteMapBuilder.AddNode(child, "TestRule");

			SiteMapNode node = provider.RootNode.ChildNodes[0];
			Assert.AreEqual(1, provider.RootNode.ChildNodes.Count);
			Assert.AreEqual("node", node.Key);

			Assert.IsFalse(provider.TestIsAccessibleToUser(node));
		}

		[TestMethod]
		public void UserGrantedAccessToNodeWhenNoAuthServiceDefined()
		{
			TestableModuleSiteMapProvider provider = new TestableModuleSiteMapProvider();
			provider.AuthorizationService = new MockAuthorizationService();
			SiteMapBuilderService siteMapBuilder = new SiteMapBuilderService();
			provider.SiteMapBuilder = siteMapBuilder;

			SiteMapNodeInfo child = new SiteMapNodeInfo("node");
			siteMapBuilder.AddNode(child, "TestRule");

			SiteMapNode node = provider.RootNode.ChildNodes[0];
			Assert.AreEqual(1, provider.RootNode.ChildNodes.Count);
			Assert.AreEqual("node", node.Key);

			Assert.IsTrue(provider.TestIsAccessibleToUser(node));
		}
	}


	internal class TestableModuleSiteMapProvider : ModuleSiteMapProvider
	{
		public bool TestIsAccessibleToUser(SiteMapNode node)
		{
			return base.IsAccessibleToUser(node);
		}
	}

	internal class MockAuthorizationService : IAuthorizationService
	{
		public bool ShouldAuthorize = true;

		#region IAuthorizationService Members

		public bool IsAuthorized(string context)
		{
			if (String.IsNullOrEmpty(context))
			{
				return true;
			}
			return ShouldAuthorize;
		}

		public bool IsAuthorized(string role, string context)
		{
			if (String.IsNullOrEmpty(context))
			{
				return true;
			}
			return ShouldAuthorize;
		}

		#endregion
	}
}
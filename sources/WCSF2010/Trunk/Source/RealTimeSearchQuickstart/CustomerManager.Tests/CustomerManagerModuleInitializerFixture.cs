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
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Services;
using Microsoft.Practices.CompositeWeb.Interfaces;

namespace RealTimeSearchQuickstart.CustomerManager.Tests
{
	/// <summary>
	/// Summary description for CustomerManagerModuleInitializerFixture
	/// </summary>
	[TestClass]
	public class CustomerManagerModuleInitializerFixture
	{
		public CustomerManagerModuleInitializerFixture()
		{
		}

		[TestMethod]
		public void CustomerManagerGetsRegisteredOnSiteMap()
		{
			TestableModuleInitializer moduleInitializer = new TestableModuleInitializer();
			SiteMapBuilderService siteMapBuilder = new SiteMapBuilderService();

			moduleInitializer.RegisterSiteMapInformation(siteMapBuilder);

			SiteMapNodeInfo node = siteMapBuilder.GetChildren(siteMapBuilder.RootNode.Key)[0];
			Assert.AreEqual("CustomerManager", node.Key);
		}

	}

	class TestableModuleInitializer : CustomerManagerModuleInitializer
	{
		public new void RegisterSiteMapInformation(ISiteMapBuilderService siteMapBuilder)
		{
			base.RegisterSiteMapInformation(siteMapBuilder);
		}
	}
}

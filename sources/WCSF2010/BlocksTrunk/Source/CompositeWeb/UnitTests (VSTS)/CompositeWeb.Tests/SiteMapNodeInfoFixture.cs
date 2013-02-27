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
using System.Collections.Specialized;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests
{
	[TestClass]
	public class SiteMapNodeInfoFixture
	{
		[TestMethod]
		[ExpectedException(typeof (ArgumentNullException))]
		public void TestCannotInitializeWithNullKey()
		{
			SiteMapNodeInfo node = new SiteMapNodeInfo(null);
		}


		[TestMethod]
		public void TestCannotInitializeWithEmptyKeyString()
		{
			SiteMapNodeInfo node = new SiteMapNodeInfo(String.Empty);
		}

		[TestMethod]
		public void TestCanInitializationWithKey()
		{
			SiteMapNodeInfo node = new SiteMapNodeInfo("Key");

			Assert.AreEqual("Key", node.Key);
			Assert.IsNull(node.Url);
			Assert.IsNull(node.Title);
			Assert.IsNull(node.Description);
			Assert.IsNull(node.Roles);
			Assert.IsNull(node.Attributes);
			Assert.IsNull(node.Attributes);
			Assert.IsNull(node.ExplicitResourcesKey);
			Assert.IsNull(node.ImplicitResourceKey);
		}


		[TestMethod]
		public void TestCanInitializeWithKeyAndUrl()
		{
			SiteMapNodeInfo node = new SiteMapNodeInfo("Key", "Url");

			Assert.AreEqual("Key", node.Key);
			Assert.AreEqual("Url", node.Url);
			Assert.IsNull(node.Title);
			Assert.IsNull(node.Description);
			Assert.IsNull(node.Roles);
			Assert.IsNull(node.Attributes);
			Assert.IsNull(node.ExplicitResourcesKey);
			Assert.IsNull(node.ImplicitResourceKey);
		}

		[TestMethod]
		public void TestCanInitializeWithKeyUrlAndTitle()
		{
			SiteMapNodeInfo node = new SiteMapNodeInfo("Key", "Url", "Title");

			Assert.AreEqual("Key", node.Key);
			Assert.AreEqual("Url", node.Url);
			Assert.AreEqual("Title", node.Title);
			Assert.IsNull(node.Description);
			Assert.IsNull(node.Roles);
			Assert.IsNull(node.Attributes);
			Assert.IsNull(node.ExplicitResourcesKey);
			Assert.IsNull(node.ImplicitResourceKey);
		}

		[TestMethod]
		public void TestCanInitializeWithKeyUrlTitleAndDescription()
		{
			SiteMapNodeInfo node = new SiteMapNodeInfo("Key", "Url", "Title", "Description");

			Assert.AreEqual("Key", node.Key);
			Assert.AreEqual("Url", node.Url);
			Assert.AreEqual("Title", node.Title);
			Assert.AreEqual("Description", node.Description);
			Assert.IsNull(node.Roles);
			Assert.IsNull(node.Attributes);
			Assert.IsNull(node.ExplicitResourcesKey);
			Assert.IsNull(node.ImplicitResourceKey);
		}

		[TestMethod]
		public void TestCanInitializeWithAllMembers()
		{
			List<object> roles = new List<object>();
			NameValueCollection attributes = new NameValueCollection();
			NameValueCollection explicitResourcesKey = new NameValueCollection();
			SiteMapNodeInfo node =
				new SiteMapNodeInfo("Key", "Url", "Title", "Description", roles, attributes, explicitResourcesKey,
				                    "ImplicitResourceKey");

			Assert.AreEqual("Key", node.Key);
			Assert.AreEqual("Url", node.Url);
			Assert.AreEqual("Title", node.Title);
			Assert.AreEqual("Description", node.Description);
			Assert.AreSame(roles, node.Roles);
			Assert.AreSame(attributes, node.Attributes);
			Assert.AreSame(explicitResourcesKey, node.ExplicitResourcesKey);
			Assert.AreEqual("ImplicitResourceKey", node.ImplicitResourceKey);
		}

		[TestMethod]
		public void CanChangeSiteMapNodeInfoValues()
		{
			List<object> roles = new List<object>();

			SiteMapNodeInfo node = new SiteMapNodeInfo("Key", "Url", "Title", "Description");
			node.Key = "NewKey";
			node.Url = "NewUrl";
			node.Title = "NewTitle";
			node.Description = "NewDescription";
			node.ImplicitResourceKey = "ImplicitResourceKey";

			Assert.AreEqual("NewKey", node.Key);
			Assert.AreEqual("NewUrl", node.Url);
			Assert.AreEqual("NewTitle", node.Title);
			Assert.AreEqual("NewDescription", node.Description);
			Assert.AreEqual("ImplicitResourceKey", node.ImplicitResourceKey);
		}
	}
}
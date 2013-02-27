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
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests.Services
{
	/// <summary>
	/// Summary description for SiteMapServiceFixture
	/// </summary>
	[TestClass]
	public class SiteMapServiceFixture
	{
		[TestMethod]
		public void TestRootNodeIsNotNull()
		{
			SiteMapBuilderService service = new SiteMapBuilderService();
			Assert.IsNotNull(service.RootNode);
		}

		[TestMethod]
		public void CanSetPropertiesToRootNode()
		{
			SiteMapBuilderService service = new SiteMapBuilderService();

			service.RootNode.Title = "Title";

			Assert.AreEqual("Title", service.RootNode.Title);
		}

		[TestMethod]
		public void InitializedRootHasNoChildren()
		{
			SiteMapBuilderService service = new SiteMapBuilderService();

			Assert.AreEqual(0, service.GetChildren(service.RootNode.Key).Count);
		}

		[TestMethod]
		public void VerifyThatAddSingleNodeAddsANodeToTheRoot()
		{
			SiteMapBuilderService service = new SiteMapBuilderService();
			SiteMapNodeInfo node = new SiteMapNodeInfo("node");

			service.AddNode(node);

			Assert.AreEqual(1, service.GetChildren(service.RootNode.Key).Count);
			Assert.IsTrue(service.GetChildren(service.RootNode.Key).Contains(node));
		}

		[TestMethod]
		public void AddTwoNodesWorks()
		{
			SiteMapBuilderService service = new SiteMapBuilderService();
			SiteMapNodeInfo node1 = new SiteMapNodeInfo("node1");
			SiteMapNodeInfo node2 = new SiteMapNodeInfo("node2");

			service.AddNode(node1);
			service.AddNode(node2);

			Assert.AreEqual(2, service.GetChildren(service.RootNode.Key).Count);
			Assert.IsTrue(service.GetChildren(service.RootNode.Key).Contains(node1));
			Assert.IsTrue(service.GetChildren(service.RootNode.Key).Contains(node2));
		}

		[TestMethod]
		[ExpectedException(typeof (ServiceException))]
		public void CannotAddNodesWithSameKey()
		{
			SiteMapBuilderService service = new SiteMapBuilderService();
			SiteMapNodeInfo node1 = new SiteMapNodeInfo("node");
			SiteMapNodeInfo node2 = new SiteMapNodeInfo("node");

			service.AddNode(node1);
			service.AddNode(node2);
		}

		[TestMethod]
		public void CanBuildSimpleTree()
		{
			SiteMapBuilderService service = new SiteMapBuilderService();
			SiteMapNodeInfo child = new SiteMapNodeInfo("child");
			SiteMapNodeInfo grandChild = new SiteMapNodeInfo("grandChild");

			service.AddNode(child);
			service.AddNode(grandChild, child);

			Assert.IsTrue(service.GetChildren(service.RootNode.Key).Contains(child));
			Assert.IsTrue(service.GetChildren(child.Key).Contains(grandChild));
		}

		[TestMethod]
		[ExpectedException(typeof (ArgumentNullException))]
		public void CannotAddNullNode()
		{
			SiteMapBuilderService service = new SiteMapBuilderService();
			service.AddNode(null);
		}

		[TestMethod]
		public void AssociateAuthorizationRuleWithNode()
		{
			SiteMapBuilderService service = new SiteMapBuilderService();
			SiteMapNodeInfo node = new SiteMapNodeInfo("node");

			service.AddNode(node, "rule");

			Assert.AreEqual("rule", service.GetAuthorizationRule(node.Key));
		}

		[TestMethod]
		public void NodeThatDoesNotRequireAuthorizationReturnsNullAuthorizationRule()
		{
			SiteMapBuilderService service = new SiteMapBuilderService();
			SiteMapNodeInfo node = new SiteMapNodeInfo("node");

			service.AddNode(node);

			Assert.IsNull(service.GetAuthorizationRule(node.Key));
		}

		[TestMethod]
		public void CanSpecifyAuthorizationRuleWhenAddingChildNode()
		{
			SiteMapBuilderService service = new SiteMapBuilderService();
			SiteMapNodeInfo parent = new SiteMapNodeInfo("parent");
			SiteMapNodeInfo child = new SiteMapNodeInfo("child");

			service.AddNode(parent);
			service.AddNode(child, parent, "rule");

			Assert.IsNull(service.GetAuthorizationRule(parent.Key));
			Assert.AreEqual("rule", service.GetAuthorizationRule(child.Key));
		}

		[TestMethod]
		[ExpectedException(typeof (ArgumentNullException))]
		public void ThrowsWhenSpecifyingNullAuthorizationRule()
		{
			SiteMapBuilderService service = new SiteMapBuilderService();
			SiteMapNodeInfo parent = new SiteMapNodeInfo("parent");
			string rule = null;

			service.AddNode(parent, rule);
		}

		[TestMethod]
		[ExpectedException(typeof (ArgumentException))]
		public void ThrowsWhenSpecifyingEmptyAuthorizationRule()
		{
			SiteMapBuilderService service = new SiteMapBuilderService();
			SiteMapNodeInfo parent = new SiteMapNodeInfo("parent");
			string rule = String.Empty;

			service.AddNode(parent, rule);
		}

		[TestMethod]
		public void CanAddNodeWithUIOrderSet()
		{
			ISiteMapBuilderService service = new SiteMapBuilderService();
			SiteMapNodeInfo node = new SiteMapNodeInfo("parent");

			service.AddNode(node, 1);

			Assert.IsTrue(service.GetChildren(service.RootNode.Key).Contains(node));
		}

		[TestMethod]
		public void GetChildrenReturnsTwoTopLevelNodesInDisplayOrder()
		{
			ISiteMapBuilderService service = new SiteMapBuilderService();
			SiteMapNodeInfo node1 = new SiteMapNodeInfo("node1");
			SiteMapNodeInfo node2 = new SiteMapNodeInfo("node2");

			service.AddNode(node2, 2);
			service.AddNode(node1, 1);

			Assert.IsTrue(service.GetChildren(service.RootNode.Key).Contains(node1));
			Assert.IsTrue(service.GetChildren(service.RootNode.Key).Contains(node2));

			Assert.AreSame(node1, service.GetChildren(service.RootNode.Key)[0]);
			Assert.AreSame(node2, service.GetChildren(service.RootNode.Key)[1]);
		}


		[TestMethod]
		public void GetChildrenReturnsThreeTopLevelNodesInDisplayOrderWhenOneDoesNotSpecifyPreference()
		{
			ISiteMapBuilderService service = new SiteMapBuilderService();
			SiteMapNodeInfo node1 = new SiteMapNodeInfo("node1");
			SiteMapNodeInfo node2 = new SiteMapNodeInfo("node2");
			SiteMapNodeInfo node3 = new SiteMapNodeInfo("node3");

			service.AddNode(node2, 2);
			service.AddNode(node3);
			service.AddNode(node1, 1);


			Assert.IsTrue(service.GetChildren(service.RootNode.Key).Contains(node1));
			Assert.IsTrue(service.GetChildren(service.RootNode.Key).Contains(node2));
			Assert.IsTrue(service.GetChildren(service.RootNode.Key).Contains(node3));

			Assert.AreSame(node1, service.GetChildren(service.RootNode.Key)[0]);
			Assert.AreSame(node2, service.GetChildren(service.RootNode.Key)[1]);
			Assert.AreSame(node3, service.GetChildren(service.RootNode.Key)[2]);
		}

		[TestMethod]
		public void GetChildrenReturnsProperDisplayOrderWhenSeveralDoNotSpecifyPreference()
		{
			ISiteMapBuilderService service = new SiteMapBuilderService();
			SiteMapNodeInfo node1 = new SiteMapNodeInfo("node1");
			SiteMapNodeInfo node2 = new SiteMapNodeInfo("node2");
			SiteMapNodeInfo node3 = new SiteMapNodeInfo("node3");
			SiteMapNodeInfo node4 = new SiteMapNodeInfo("node4");

			service.AddNode(node2, 2);
			service.AddNode(node3);
			service.AddNode(node1, 1);
			service.AddNode(node4);


			Assert.IsTrue(service.GetChildren(service.RootNode.Key).Contains(node1));
			Assert.IsTrue(service.GetChildren(service.RootNode.Key).Contains(node2));
			Assert.IsTrue(service.GetChildren(service.RootNode.Key).Contains(node3));
			Assert.IsTrue(service.GetChildren(service.RootNode.Key).Contains(node4));

			Assert.AreSame(node1, service.GetChildren(service.RootNode.Key)[0]);
			Assert.AreSame(node2, service.GetChildren(service.RootNode.Key)[1]);
			Assert.AreSame(node3, service.GetChildren(service.RootNode.Key)[2]);
			Assert.AreSame(node4, service.GetChildren(service.RootNode.Key)[3]);
		}


		[TestMethod]
		public void GetChildrenReturnsProperDisplayOrderWithLotsOfNodes()
		{
			ISiteMapBuilderService service = new SiteMapBuilderService();
			SiteMapNodeInfo node1 = new SiteMapNodeInfo("node1");
			SiteMapNodeInfo node2 = new SiteMapNodeInfo("node2");
			SiteMapNodeInfo node3 = new SiteMapNodeInfo("node3");
			SiteMapNodeInfo node4 = new SiteMapNodeInfo("node4");
			SiteMapNodeInfo node5 = new SiteMapNodeInfo("node5");
			SiteMapNodeInfo node6 = new SiteMapNodeInfo("node6");
			SiteMapNodeInfo node7 = new SiteMapNodeInfo("node7");
			SiteMapNodeInfo node8 = new SiteMapNodeInfo("node8");

			service.AddNode(node2, 2);
			service.AddNode(node3);
			service.AddNode(node1, 1);
			service.AddNode(node4, 4);
			service.AddNode(node5);
			service.AddNode(node6, 10);
			service.AddNode(node7);
			service.AddNode(node8, 1000);

			Assert.AreSame(node1, service.GetChildren(service.RootNode.Key)[0]);
			Assert.AreSame(node2, service.GetChildren(service.RootNode.Key)[1]);
			Assert.AreSame(node4, service.GetChildren(service.RootNode.Key)[2]);
			Assert.AreSame(node6, service.GetChildren(service.RootNode.Key)[3]);
			Assert.AreSame(node8, service.GetChildren(service.RootNode.Key)[4]);
			Assert.AreSame(node3, service.GetChildren(service.RootNode.Key)[5]);
			Assert.AreSame(node5, service.GetChildren(service.RootNode.Key)[6]);
			Assert.AreSame(node7, service.GetChildren(service.RootNode.Key)[7]);
		}

		[TestMethod]
		public void GetChildrenReturnsChildNodesInDisplayOrder()
		{
			ISiteMapBuilderService service = new SiteMapBuilderService();
			SiteMapNodeInfo parent = new SiteMapNodeInfo("parent");
			SiteMapNodeInfo toplevel = new SiteMapNodeInfo("toplevel");
			SiteMapNodeInfo child1 = new SiteMapNodeInfo("child1");
			SiteMapNodeInfo child2 = new SiteMapNodeInfo("child2");

			service.AddNode(toplevel);
			service.AddNode(parent, 1);
			service.AddNode(child2, parent);
			service.AddNode(child1, parent, 100);

			Assert.AreSame(parent, service.GetChildren(service.RootNode.Key)[0]);
			Assert.AreSame(toplevel, service.GetChildren(service.RootNode.Key)[1]);

			Assert.AreSame(child1, service.GetChildren(parent.Key)[0]);
			Assert.AreSame(child2, service.GetChildren(parent.Key)[1]);
		}

		[TestMethod]
		public void CanSpecifyAuthorizationRuleWhenAddingNodesWithPreferredDisplayOrder()
		{
			ISiteMapBuilderService service = new SiteMapBuilderService();
			SiteMapNodeInfo child2 = new SiteMapNodeInfo("child1");
			SiteMapNodeInfo child1 = new SiteMapNodeInfo("child2");

			service.AddNode(child2, "rule");
			service.AddNode(child1, "rule", 1000);

			Assert.AreSame(child1, service.GetChildren(service.RootNode.Key)[0]);
			Assert.AreSame(child2, service.GetChildren(service.RootNode.Key)[1]);
		}


		[TestMethod]
		public void CanSpecifyAuthorizationRuleWhenAddingChildNodesWithPreferredDisplayOrder()
		{
			ISiteMapBuilderService service = new SiteMapBuilderService();
			SiteMapNodeInfo parent = new SiteMapNodeInfo("parent");
			SiteMapNodeInfo child2 = new SiteMapNodeInfo("child1");
			SiteMapNodeInfo child1 = new SiteMapNodeInfo("child2");

			service.AddNode(parent);
			service.AddNode(child2, parent, "rule");
			service.AddNode(child1, parent, "rule", 1000);

			Assert.AreSame(child1, service.GetChildren(parent.Key)[0]);
			Assert.AreSame(child2, service.GetChildren(parent.Key)[1]);
		}
	}
}
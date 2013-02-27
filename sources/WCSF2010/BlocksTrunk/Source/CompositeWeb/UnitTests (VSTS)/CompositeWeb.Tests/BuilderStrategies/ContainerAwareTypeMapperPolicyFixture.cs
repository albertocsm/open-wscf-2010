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
using Microsoft.Practices.CompositeWeb.BuilderStrategies;
using Microsoft.Practices.CompositeWeb.Tests.Mocks;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests.BuilderStrategies
{
	[TestClass]
	public class ContainerAwareTypeMapperPolicyFixture
	{
		[TestMethod]
		public void PolicyShouldRequestTypeMappingFromContainer()
		{
			TestableRootCompositionContainer container = new TestableRootCompositionContainer();
			container.RegisterTypeMapping<IFoo, Foo>();

			ContainerAwareTypeMappingPolicy policy = new ContainerAwareTypeMappingPolicy();

			string requestId = "My mapped object";
			DependencyResolutionLocatorKey requestKey =
				new DependencyResolutionLocatorKey(typeof (IFoo), requestId);
			DependencyResolutionLocatorKey result = policy.Map(container.Locator, requestKey);

			Assert.AreEqual(typeof (Foo), result.Type);
			Assert.AreEqual(requestId, result.ID);
		}

		[TestMethod]
		public void PolicyCallsUpToParentContainerToGetMapping()
		{
			TestableRootCompositionContainer container = new TestableRootCompositionContainer();
			container.RegisterTypeMapping<IFoo, Foo>();
			CompositionContainer child = container.Containers.AddNew<CompositionContainer>();

			ContainerAwareTypeMappingPolicy policy = new ContainerAwareTypeMappingPolicy();

			string requestId = "My mapped object";
			DependencyResolutionLocatorKey requestKey =
				new DependencyResolutionLocatorKey(typeof (IFoo), requestId);
			DependencyResolutionLocatorKey result = policy.Map(child.Locator, requestKey);

			Assert.AreEqual(typeof (Foo), result.Type);
			Assert.AreEqual(requestId, result.ID);
		}
	}
}
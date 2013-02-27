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
using Microsoft.Practices.CompositeWeb.BuilderStrategies;
using Microsoft.Practices.CompositeWeb.Tests.Mocks;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests.BuilderStrategies
{
	[TestClass]
	public class ContainerAwareTypeMappingStrategyFixture
	{
		[TestMethod]
		public void StrategyShouldDoTypeMapping()
		{
			TestableRootCompositionContainer root = new TestableRootCompositionContainer();
			root.RegisterTypeMapping<IFoo, Foo>();

			ContainerAwareTypeMappingStrategy strategy = new ContainerAwareTypeMappingStrategy();
			MockBuilderContext builderContext =
				new MockBuilderContext(strategy, new ReturnRequestedTypeStrategy());
			builderContext.Policies.SetDefault<IContainerAwareTypeMappingPolicy>(
				new ContainerAwareTypeMappingPolicy());
			builderContext.Locator = root.Locator;

			object result = strategy.BuildUp(builderContext, typeof (IFoo), null, "Test object");
			DependencyResolutionLocatorKey key = result as DependencyResolutionLocatorKey;
			Assert.IsNotNull(key);
			Assert.AreEqual(typeof (Foo), key.Type);
		}

		#region Nested type: ReturnRequestedTypeStrategy

		/// <summary>
		/// A dumb little class that returns the type requested/key pair.
		/// </summary>
		private class ReturnRequestedTypeStrategy : BuilderStrategy
		{
			public override object BuildUp(
				IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
			{
				return new DependencyResolutionLocatorKey(typeToBuild, idToBuild);
			}
		}

		#endregion
	}
}
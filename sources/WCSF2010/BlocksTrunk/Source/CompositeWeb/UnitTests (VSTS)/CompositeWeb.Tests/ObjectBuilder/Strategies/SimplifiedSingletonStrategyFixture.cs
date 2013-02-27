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
using Microsoft.Practices.CompositeWeb.ObjectBuilder.Strategies;
using Microsoft.Practices.CompositeWeb.Tests.Mocks;
using Microsoft.Practices.CompositeWeb.Tests.Mocks.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests.ObjectBuilder.Strategies
{
	[TestClass]
	public class SimplifiedSingletonStrategyFixture
	{
		public SimplifiedSingletonStrategyFixture()
		{
		}

		[TestMethod]
		public void CreatingASingletonTwiceReturnsSameInstance()
		{
			MockBuilderContext ctx = BuildContext();
			ctx.Policies.Set<ISingletonPolicy>(new SingletonPolicy(true), typeof (MockObject), null);

			MockObject i1 = (MockObject) ctx.HeadOfChain.BuildUp(ctx, typeof (MockObject), null, null);
			MockObject i2 = (MockObject) ctx.HeadOfChain.BuildUp(ctx, typeof (MockObject), null, null);

			Assert.AreSame(i1, i2);
		}

		[TestMethod]
		public void SingletonsCanBeBasedOnTypeAndID()
		{
			MockBuilderContext ctx = BuildContext();
			ctx.Policies.Set<ISingletonPolicy>(new SingletonPolicy(true), typeof (MockObject), "magickey");

			MockObject i1a = (MockObject) ctx.HeadOfChain.BuildUp(ctx, typeof (MockObject), null, "magickey");
			MockObject i1b = (MockObject) ctx.HeadOfChain.BuildUp(ctx, typeof (MockObject), null, "magickey");
			MockObject i2 = (MockObject) ctx.HeadOfChain.BuildUp(ctx, typeof (MockObject), null, null);
			MockObject i3 = (MockObject) ctx.HeadOfChain.BuildUp(ctx, typeof (MockObject), null, null);

			Assert.AreSame(i1a, i1b);
			Assert.IsTrue(i1a != i2);
			Assert.IsTrue(i2 != i3);
		}

		private static MockBuilderContext BuildContext()
		{
			IBuilderStrategy[] strategies = {
			                                	new SimplifiedSingletonStrategy(),
			                                	new MockCreationStrategy()
			                                };
			MockBuilderContext ctx = new MockBuilderContext(strategies);
			LifetimeContainer lifetimeContainer = new LifetimeContainer();

			if (!ctx.Locator.Contains(typeof (ILifetimeContainer)))
				ctx.Locator.Add(typeof (ILifetimeContainer), lifetimeContainer);

			ctx.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());

			return ctx;
		}

		#region Nested type: MockObject

		private class MockObject
		{
		}

		#endregion
	}
}
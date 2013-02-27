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
using Microsoft.Practices.CompositeWeb.ObjectBuilder.BuildPlan;
using Microsoft.Practices.CompositeWeb.ObjectBuilder.BuildPlan.DynamicMethodPlan;
using Microsoft.Practices.CompositeWeb.Tests.Mocks;
using Microsoft.Practices.CompositeWeb.Tests.Mocks.ObjectBuilder.BuildPlan.DynamicMethodPlan;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests.ObjectBuilder.Strategies
{
	[TestClass]
	public class DynamicMethodPlanBuilderPolicyFixture
	{
		public DynamicMethodPlanBuilderPolicyFixture()
		{
		}

		[TestMethod]
		public void CreateInstanceBuildUpIsCalled()
		{
			MockReturnExistingPlanBuilderStrategy mockPlanBuilderStrategy = new MockReturnExistingPlanBuilderStrategy();
			MockBuilderContext ctx = BuildContext(mockPlanBuilderStrategy);

			MockObject i1 = (MockObject) ctx.HeadOfChain.BuildUp(ctx, typeof (MockObject), null, null);

			Assert.IsTrue(mockPlanBuilderStrategy.BuildUpCalled);
		}

		[TestMethod]
		public void BuildUpExistingInstanceBuildUpIsCalled()
		{
			MockReturnExistingPlanBuilderStrategy mockPlanBuilderStrategy = new MockReturnExistingPlanBuilderStrategy();
			MockBuilderContext ctx = BuildContext(mockPlanBuilderStrategy);
			MockObject expectedObject = new MockObject();

			MockObject i1 = (MockObject) ctx.HeadOfChain.BuildUp(ctx, typeof (MockObject), expectedObject, null);

			Assert.IsTrue(mockPlanBuilderStrategy.BuildUpCalled);
			Assert.AreSame(expectedObject, i1);
		}

		private static MockBuilderContext BuildContext(MockReturnExistingPlanBuilderStrategy mockPlanBuilderStrategy)
		{
			MockBuilderContext ctx = new MockBuilderContext(new BuildPlanStrategy());

			ctx.Policies.SetDefault<IPlanBuilderPolicy>(CreatePlanBuilder(mockPlanBuilderStrategy));

			return ctx;
		}

		private static IPlanBuilderPolicy CreatePlanBuilder(MockReturnExistingPlanBuilderStrategy mockPlanBuilderStrategy)
		{
			BuilderStrategyChain chain = new BuilderStrategyChain();
			chain.Add(mockPlanBuilderStrategy);

			PolicyList policies = new PolicyList();

			return new DynamicMethodPlanBuilderPolicy(chain, policies);
		}

		#region Nested type: MockObject

		private class MockObject
		{
		}

		#endregion
	}
}
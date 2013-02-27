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
using System;
using Microsoft.Practices.CompositeWeb.ObjectBuilder.BuildPlan;
using Microsoft.Practices.CompositeWeb.ObjectBuilder.BuildPlan.DynamicMethodPlan;
using Microsoft.Practices.CompositeWeb.ObjectBuilder.BuildPlan.DynamicMethodPlan.Creation;
using Microsoft.Practices.CompositeWeb.ObjectBuilder.BuildPlan.DynamicMethodPlan.Method;
using Microsoft.Practices.CompositeWeb.Tests.Mocks;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests.ObjectBuilder.BuildPlan.DynamicMethodPlan.Method
{
	[TestClass]
	public class CallMethodsStrategyFixture
	{
		public CallMethodsStrategyFixture()
		{
		}

		[TestMethod]
		[ExpectedException(typeof (InvalidOperationException))]
		public void BuildUpWithNoMethodChooserRaisesNullException()
		{
			MockBuilderContext ctx = BuildContext();

			MockObject mockInstance = (MockObject) ctx.HeadOfChain.BuildUp(ctx, typeof (MockObject), null, null);
		}

		[TestMethod]
		public void BuildUpExecutesTwoParameterlessMethods()
		{
			PolicyList policies = new PolicyList();
			policies.SetDefault<IMethodChooserPolicy>(new AttributeBasedMethodChooser());
			MockBuilderContext ctx = BuildContext(policies);

			MockObject mockInstance = (MockObject) ctx.HeadOfChain.BuildUp(ctx, typeof (MockObject), null, null);

			Assert.IsTrue(mockInstance.InjectionMethod1Called);
			Assert.IsTrue(mockInstance.InjectionMethod2Called);
			Assert.IsFalse(mockInstance.Method3Called);
		}

		private static MockBuilderContext BuildContext()
		{
			return BuildContext(new PolicyList());
		}

		private static MockBuilderContext BuildContext(PolicyList innerPolicies)
		{
			MockBuilderContext ctx = new MockBuilderContext(new BuildPlanStrategy());

			ctx.Policies.SetDefault<IPlanBuilderPolicy>(CreatePlanBuilder(innerPolicies));

			return ctx;
		}

		private static IPlanBuilderPolicy CreatePlanBuilder(PolicyList innerPolicies)
		{
			BuilderStrategyChain chain = new BuilderStrategyChain();
			chain.Add(new CallConstructorStrategy());
			chain.Add(new CallMethodsStrategy());

			return new DynamicMethodPlanBuilderPolicy(chain, innerPolicies);
		}

		#region Nested type: IMockDependentObject

		private interface IMockDependentObject
		{
		}

		#endregion

		#region Nested type: MockCreateNewDependingObject

		private class MockCreateNewDependingObject
		{
			public object DependentObject;

			public MockCreateNewDependingObject([CreateNew] MockDependentObject dependentObject)
			{
				DependentObject = dependentObject;
			}
		}

		#endregion

		#region Nested type: MockDependentObject

		private class MockDependentObject : IMockDependentObject
		{
		}

		#endregion

		#region Nested type: MockObject

		public class MockObject
		{
			public bool InjectionMethod1Called;
			public bool InjectionMethod2Called;
			public bool Method3Called;

			[InjectionMethod]
			public void InjectionMethod1()
			{
				InjectionMethod1Called = true;
			}

			[InjectionMethod]
			public void InjectionMethod2()
			{
				InjectionMethod2Called = true;
			}

			public void Method3()
			{
				Method3Called = true;
			}
		}

		#endregion
	}
}
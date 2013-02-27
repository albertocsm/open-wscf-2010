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
using Microsoft.Practices.CompositeWeb.ObjectBuilder.BuildPlan.DynamicMethodPlan.Creation;
using Microsoft.Practices.CompositeWeb.ObjectBuilder.BuildPlan.DynamicMethodPlan.Properties;
using Microsoft.Practices.CompositeWeb.Tests.Mocks;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests.ObjectBuilder.BuildPlan.DynamicMethodPlan.Properties
{
	[TestClass]
	public class SetPropertiesStrategyFixture
	{
		public SetPropertiesStrategyFixture()
		{
		}

		[TestMethod]
		public void BuildUpDefaultPropertyChooserSetPublicProperties()
		{
			MockBuilderContext ctx = BuildContext();
			TestableRootCompositionContainer compositionContainer = new TestableRootCompositionContainer();
			ctx.Locator.Add(new DependencyResolutionLocatorKey(typeof (CompositionContainer), null), compositionContainer);
			MockDependentObject serviceDependency =
				compositionContainer.Services.AddNew<MockDependentObject, IMockDependentObject>();

			MockObject mockInstance = new MockObject();
			mockInstance = (MockObject) ctx.HeadOfChain.BuildUp(ctx, typeof (MockObject), null, null);

			Assert.IsNotNull(mockInstance.CreateNewProperty);
			Assert.AreSame(serviceDependency, mockInstance.ServiceDependencyProperty);
			Assert.IsNull(mockInstance.InternalProperty);
		}

		[TestMethod]
		public void BuildUpAttributePropChooserSetPropertiesWithInheritedParameterAttributes()
		{
			PolicyList policies = new PolicyList();
			policies.SetDefault<IPropertyChooserPolicy>(new AttributeBasedPropertyChooser());

			MockBuilderContext ctx = BuildContext(policies);
			TestableRootCompositionContainer compositionContainer = new TestableRootCompositionContainer();
			ctx.Locator.Add(new DependencyResolutionLocatorKey(typeof (CompositionContainer), null), compositionContainer);
			MockDependentObject serviceDependency =
				compositionContainer.Services.AddNew<MockDependentObject, IMockDependentObject>();

			MockObject mockInstance = new MockObject();
			mockInstance = (MockObject) ctx.HeadOfChain.BuildUp(ctx, typeof (MockObject), null, null);

			Assert.IsNotNull(mockInstance.CreateNewProperty);
			Assert.AreSame(serviceDependency, mockInstance.ServiceDependencyProperty);
			Assert.IsNull(mockInstance.InternalProperty);
		}

		[TestMethod]
		public void BuildUpDefaultPropertyChooserSetPublicPropertiesOnSimpleBaseClass()
		{
			MockBuilderContext ctx = BuildContext();
			TestableRootCompositionContainer compositionContainer = new TestableRootCompositionContainer();
			ctx.Locator.Add(new DependencyResolutionLocatorKey(typeof(CompositionContainer), null), compositionContainer);

			SimpleChildMockObject mockInstance;
			mockInstance = (SimpleChildMockObject)ctx.HeadOfChain.BuildUp(ctx, typeof(SimpleChildMockObject), null, null);

			Assert.IsNotNull(mockInstance.CreateNewProperty);
		}

		// BUG: Added a few unit tests to exhibit a bug that a customer saw using VS2005 and .NET2.0 without SP1.  
		// .Net 2.0 SP1 fixes the issue.
		[TestMethod]
		public void BuildUpDefaultPropertyChooserSetPublicPropertiesOnGenericBaseClass()
		{
			MockBuilderContext ctx = BuildContext();
			TestableRootCompositionContainer compositionContainer = new TestableRootCompositionContainer();
			ctx.Locator.Add(new DependencyResolutionLocatorKey(typeof(CompositionContainer), null), compositionContainer);

			GenericChildMockObject mockInstance;
			mockInstance = (GenericChildMockObject)ctx.HeadOfChain.BuildUp(ctx, typeof(GenericChildMockObject), null, null);

			Assert.IsNotNull(mockInstance.CreateNewProperty);
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
			chain.Add(new SetPropertiesStrategy());

			return new DynamicMethodPlanBuilderPolicy(chain, innerPolicies);
		}

		public class GenericBaseMockObject<T>
		{
			T field;

			[CreateNew]
			public T CreateNewProperty
			{
				set { field = value; }
				get { return field; }
			}
		}
		public class SimpleBaseMockObject
		{
			MockDependentObject field;

			[CreateNew]
			public MockDependentObject CreateNewProperty
			{
				set { field = value; }
				get { return field; }
			}
		}
		public class SimpleChildMockObject : SimpleBaseMockObject { }
		public class GenericChildMockObject : GenericBaseMockObject<MockDependentObject> { }

		#region Nested type: IMockDependentObject

		public interface IMockDependentObject
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

		public class MockDependentObject : IMockDependentObject
		{
		}

		#endregion

		#region Nested type: MockObject

		public class MockObject
		{
			public object CreateNewProperty;
			public object InternalProperty;
			public object ServiceDependencyProperty;

			[CreateNew]
			public MockDependentObject CreateNew
			{
				set { CreateNewProperty = value; }
			}

			[ServiceDependency(Type = typeof (IMockDependentObject))]
			public IMockDependentObject ServiceDependency
			{
				set { ServiceDependencyProperty = value; }
			}

			internal MockDependentObject Internal
			{
				set { InternalProperty = value; }
			}
		}

		#endregion
	}
}
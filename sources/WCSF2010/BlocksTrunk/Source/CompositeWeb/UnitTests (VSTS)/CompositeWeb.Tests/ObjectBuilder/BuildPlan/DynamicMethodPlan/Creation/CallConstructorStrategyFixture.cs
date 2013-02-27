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
using Microsoft.Practices.CompositeWeb.Tests.Mocks;
using Microsoft.Practices.CompositeWeb.Tests.Mocks.ObjectBuilder.BuildPlan.DynamicMethodPlan.Creation;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests.ObjectBuilder.BuildPlan.DynamicMethodPlan.Creation
{
	[TestClass]
	public class CallConstructorStrategyFixture
	{
		public CallConstructorStrategyFixture()
		{
		}

		// This unit test is obsolete since DefaultConstructorChooser is used.
		//[TestMethod]
		//[ExpectedException(typeof(NullReferenceException))]
		//[Ignore]
		//public void CreateInstanceWithNoContructorChooserRaisesNullException()
		//{
		//    MockBuilderContext ctx = BuildContext();

		//    MockObject mockInstance = (MockObject)ctx.HeadOfChain.BuildUp(ctx, typeof(MockObject), null, null);
		//}

		[TestMethod]
		public void WhenNoConstructorChooserPolicyDefaultChooserIsUsed()
		{
			PolicyList policies = new PolicyList();

			MockBuilderContext ctx = BuildContext(policies);

			MockObject mockInstance = (MockObject) ctx.HeadOfChain.BuildUp(ctx, typeof (MockObject), null, null);

			Assert.IsTrue(mockInstance.DefaultConstructorCalled);
		}

		[TestMethod]
		public void CreateInstanceCallDefaultExplicitConstructor()
		{
			PolicyList policies = new PolicyList();
			policies.SetDefault<IConstructorChooserPolicy>(new DefaultConstructorChooserPolicy());

			MockBuilderContext ctx = BuildContext(policies);

			MockObject mockInstance = (MockObject) ctx.HeadOfChain.BuildUp(ctx, typeof (MockObject), null, null);

			Assert.IsTrue(mockInstance.DefaultConstructorCalled);
		}

		[TestMethod]
		public void CreateInstanceCallAttributedCtor()
		{
			PolicyList policies = new PolicyList();
			policies.SetDefault<IConstructorChooserPolicy>(new AttributeBasedConstructorChooser());

			MockBuilderContext ctx = BuildContext(policies);

			MockObjectWithAttributedCtor mockInstance =
				(MockObjectWithAttributedCtor) ctx.HeadOfChain.BuildUp(ctx, typeof (MockObjectWithAttributedCtor), null, null);

			Assert.IsTrue(mockInstance.AttributedConstructorCalled);
		}

		[TestMethod]
		public void CreateInstanceWithNoConstructorAtAll()
		{
			PolicyList policies = new PolicyList();
			policies.SetDefault<IConstructorChooserPolicy>(new DefaultConstructorChooserPolicy());

			MockBuilderContext ctx = BuildContext(policies);

			int mockInstance = (int) ctx.HeadOfChain.BuildUp(ctx, typeof (int), null, null);

			//Int instances have zero as initialization value
			Assert.AreEqual((int) 0, mockInstance);
		}

		[TestMethod]
		public void CreateInstanceCallCreateInstance()
		{
			PolicyList policies = new PolicyList();
			policies.SetDefault<IConstructorChooserPolicy>(new MockReturnNullConstructor());

			MockBuilderContext ctx = BuildContext(policies);

			MockObject mockInstance = (MockObject) ctx.HeadOfChain.BuildUp(ctx, typeof (MockObject), null, null);

			Assert.IsTrue(mockInstance.DefaultConstructorCalled);
		}

		[TestMethod]
		public void CreateInstanceCreateNewDependingObject()
		{
			PolicyList policies = new PolicyList();
			MockBuilderContext ctx = BuildContext(policies);

			MockCreateNewDependingObject mockInstance =
				(MockCreateNewDependingObject) ctx.HeadOfChain.BuildUp(ctx, typeof (MockCreateNewDependingObject), null, null);

			Assert.IsNotNull(mockInstance);
			Assert.IsNotNull(mockInstance.DependentObject);
		}

		[TestMethod]
		public void CreateInstanceCreateNewDependingObjectInTwoLevels()
		{
			PolicyList policies = new PolicyList();
			MockBuilderContext ctx = BuildContext(policies);

			MockCreateNewTwoLevelDependingObject mockInstance =
				(MockCreateNewTwoLevelDependingObject)
				ctx.HeadOfChain.BuildUp(ctx, typeof (MockCreateNewTwoLevelDependingObject), null, null);

			Assert.IsNotNull(mockInstance);
			Assert.IsNotNull(mockInstance.DependentObject);
			Assert.IsNotNull(mockInstance.DependentObject.DependentObject);
		}

		[TestMethod]
		public void CreateInstanceInjectServiceDependencyDependingObject()
		{
			PolicyList policies = new PolicyList();
			MockBuilderContext ctx = BuildContext(policies);
			TestableRootCompositionContainer compositionContainer = new TestableRootCompositionContainer();
			ctx.Locator.Add(new DependencyResolutionLocatorKey(typeof (CompositionContainer), null), compositionContainer);
			MockDependentObject mockDependency = compositionContainer.Services.AddNew<MockDependentObject, MockDependentObject>();

			MockDependencyDependingObject mockInstance =
				(MockDependencyDependingObject) ctx.HeadOfChain.BuildUp(ctx, typeof (MockDependencyDependingObject), null, null);

			Assert.IsNotNull(mockInstance);
			Assert.IsNotNull(mockInstance.DependentObject);
			Assert.AreSame(mockDependency, mockInstance.DependentObject);
		}

		[TestMethod]
		public void CreateInstancenotInjectsRegisteredInterfaceService()
		{
			PolicyList policies = new PolicyList();
			MockBuilderContext ctx = BuildContext(policies);
			TestableRootCompositionContainer compositionContainer = new TestableRootCompositionContainer();
			ctx.Locator.Add(new DependencyResolutionLocatorKey(typeof (CompositionContainer), null), compositionContainer);
			MockDependentObject mockDependency =
				compositionContainer.Services.AddNew<MockDependentObject, IMockDependentObject>();

			MockDependencyDependingInterface mockInstance =
				(MockDependencyDependingInterface)
				ctx.HeadOfChain.BuildUp(ctx, typeof (MockDependencyDependingInterface), null, null);

			Assert.IsNotNull(mockInstance);
			Assert.IsNotNull(mockInstance.DependentObject);
			Assert.AreSame(mockDependency, mockInstance.DependentObject);
		}

		[TestMethod]
		public void CreateInstancenotInjectNonRequiredNotPresentServiceDependency()
		{
			PolicyList policies = new PolicyList();
			MockBuilderContext ctx = BuildContext(policies);
			TestableRootCompositionContainer compositionContainer = new TestableRootCompositionContainer();
			ctx.Locator.Add(new DependencyResolutionLocatorKey(typeof (CompositionContainer), null), compositionContainer);

			MockDependencyDependingObjectNotRequired mockInstance =
				(MockDependencyDependingObjectNotRequired)
				ctx.HeadOfChain.BuildUp(ctx, typeof (MockDependencyDependingObjectNotRequired), null, null);

			Assert.IsNotNull(mockInstance);
			Assert.IsNull(mockInstance.DependentObject);
		}

		[TestMethod]
		public void CreateInstanceUseDependencyResolverAndInjectsNamedDependency()
		{
			PolicyList policies = new PolicyList();
			MockBuilderContext ctx = BuildContext(policies);

			object dependency = new object();
			ctx.Locator.Add(new DependencyResolutionLocatorKey(typeof (object), "Foo"), dependency);

			NamedNullMockObject mockInstance =
				(NamedNullMockObject) ctx.HeadOfChain.BuildUp(ctx, typeof (NamedNullMockObject), null, null);

			Assert.IsNotNull(mockInstance);
			Assert.IsNotNull(mockInstance.DependentObject);
			Assert.AreSame(dependency, mockInstance.DependentObject);
		}

		[TestMethod]
		public void CreateInstanceValueTypedParameterIsUnboxed()
		{
			PolicyList policies = new PolicyList();
			MockBuilderContext ctx = BuildContext(policies);

			int dependency = new int();
			ctx.Locator.Add(new DependencyResolutionLocatorKey(typeof (int), "Foo"), dependency);

			ValueTypeDependentMockObject mockInstance =
				(ValueTypeDependentMockObject) ctx.HeadOfChain.BuildUp(ctx, typeof (ValueTypeDependentMockObject), null, null);

			Assert.IsNotNull(mockInstance);
			Assert.AreEqual((int) 0, mockInstance.DependentObject);
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

		#region Nested type: MockCreateNewTwoLevelDependingObject

		private class MockCreateNewTwoLevelDependingObject
		{
			public MockCreateNewDependingObject DependentObject;

			public MockCreateNewTwoLevelDependingObject([CreateNew] MockCreateNewDependingObject dependentObject)
			{
				DependentObject = dependentObject;
			}
		}

		#endregion

		#region Nested type: MockDependencyDependingInterface

		private class MockDependencyDependingInterface
		{
			public object DependentObject;

			public MockDependencyDependingInterface(
				[ServiceDependency(Type = typeof (IMockDependentObject))] object dependentObject)
			{
				DependentObject = dependentObject;
			}
		}

		#endregion

		#region Nested type: MockDependencyDependingObject

		private class MockDependencyDependingObject
		{
			public object DependentObject;

			public MockDependencyDependingObject([ServiceDependency] MockDependentObject dependentObject)
			{
				DependentObject = dependentObject;
			}
		}

		#endregion

		#region Nested type: MockDependencyDependingObjectNotRequired

		private class MockDependencyDependingObjectNotRequired
		{
			public object DependentObject;

			public MockDependencyDependingObjectNotRequired(
				[ServiceDependency(Required = false)] MockDependentObject dependentObject)
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

		private class MockObject
		{
			public bool DefaultConstructorCalled;

			public MockObject()
			{
				DefaultConstructorCalled = true;
			}
		}

		#endregion

		#region Nested type: MockObjectWithAttributedCtor

		private class MockObjectWithAttributedCtor
		{
			public bool AttributedConstructorCalled;

			public MockObjectWithAttributedCtor()
			{
			}

			[InjectionConstructor]
			public MockObjectWithAttributedCtor([CreateNew] MockDependentObject dependentObject)
			{
				AttributedConstructorCalled = true;
			}
		}

		#endregion

		#region Nested type: NamedNullMockObject

		private class NamedNullMockObject
		{
			public object DependentObject;

			public NamedNullMockObject(
				[Dependency(Name = "Foo", SearchMode=SearchMode.Up, NotPresentBehavior = NotPresentBehavior.ReturnNull)] object
					dependentObject)
			{
				DependentObject = dependentObject;
			}
		}

		#endregion

		#region Nested type: ValueTypeDependentMockObject

		private class ValueTypeDependentMockObject
		{
			public int DependentObject;

			public ValueTypeDependentMockObject(
				[Dependency(Name = "Foo", SearchMode = SearchMode.Up, NotPresentBehavior = NotPresentBehavior.ReturnNull)] int
					dependentObject)
			{
				DependentObject = dependentObject;
			}
		}

		#endregion
	}
}
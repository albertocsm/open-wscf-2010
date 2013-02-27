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
using System.Reflection;
using Microsoft.Practices.CompositeWeb.Tests.Mocks;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CreationStrategy=Microsoft.Practices.CompositeWeb.ObjectBuilder.Strategies.CreationStrategy;
using MethodReflectionStrategy=Microsoft.Practices.CompositeWeb.ObjectBuilder.Strategies.MethodReflectionStrategy;

namespace Microsoft.Practices.CompositeWeb.Tests.ObjectBuilder.Strategies
{
	[TestClass]
	public class MethodReflectionStrategyFixture
	{
		// Invalid attribute combination

		[TestMethod]
		[ExpectedException(typeof (InvalidAttributeException))]
		public void SpecifyingCreateNewAndDependencyThrows()
		{
			MockBuilderContext context = CreateContext();

			context.HeadOfChain.BuildUp(context, typeof (MockInvalidDualAttributes), null, null);
		}

		// Attribute Inheritance

		[TestMethod]
		public void CanInheritDependencyAttribute()
		{
			MockBuilderContext context = CreateContext();

			MockDependingObjectDerived depending =
				(MockDependingObjectDerived) context.HeadOfChain.BuildUp(context, typeof (MockDependingObjectDerived), null, null);

			Assert.IsNotNull(depending);
			Assert.IsNotNull(depending.InjectedObject);
		}

		[TestMethod]
		public void CanInheritCreateNewAttribute()
		{
			MockBuilderContext context = CreateContext();

			MockRequiresNewObjectDerived depending =
				(MockRequiresNewObjectDerived)
				context.HeadOfChain.BuildUp(context, typeof (MockRequiresNewObjectDerived), null, null);

			Assert.IsNotNull(depending);
			Assert.IsNotNull(depending.Foo);
		}

		// Non creatable stuff

		[TestMethod]
		[ExpectedException(typeof (ArgumentException))]
		public void ThrowsIfConcreteTypeToCreateCannotBeCreated()
		{
			MockBuilderContext context = CreateContext();
			context.HeadOfChain.BuildUp(context, typeof (MockDependsOnInterface), null, null);
		}

		// Mode 1

		[TestMethod]
		public void CreateNewAttributeAlwaysCreatesNewObject()
		{
			MockBuilderContext context;

			context = CreateContext();
			MockRequiresNewObject depending1 =
				(MockRequiresNewObject) context.HeadOfChain.BuildUp(context, typeof (MockRequiresNewObject), null, null);

			context = CreateContext();
			MockRequiresNewObject depending2 =
				(MockRequiresNewObject) context.HeadOfChain.BuildUp(context, typeof (MockRequiresNewObject), null, null);

			Assert.IsNotNull(depending1);
			Assert.IsNotNull(depending2);
			Assert.IsNotNull(depending1.Foo);
			Assert.IsNotNull(depending2.Foo);
			Assert.IsFalse(depending1.Foo == depending2.Foo);
		}

		[TestMethod]
		public void NamedAndUnnamedObjectsInLocatorDontGetUsedForCreateNew()
		{
			MockBuilderContext context;
			object unnamed = new object();
			object named = new object();

			context = CreateContext();
			context.Locator.Add(new DependencyResolutionLocatorKey(typeof (object), null), unnamed);
			context.Locator.Add(new DependencyResolutionLocatorKey(typeof (object), "Foo"), named);
			MockRequiresNewObject depending1 =
				(MockRequiresNewObject) context.HeadOfChain.BuildUp(context, typeof (MockRequiresNewObject), null, null);

			context = CreateContext();
			context.Locator.Add(new DependencyResolutionLocatorKey(typeof (object), null), unnamed);
			context.Locator.Add(new DependencyResolutionLocatorKey(typeof (object), "Foo"), named);
			MockRequiresNewObject depending2 =
				(MockRequiresNewObject) context.HeadOfChain.BuildUp(context, typeof (MockRequiresNewObject), null, null);

			Assert.IsFalse(depending1.Foo == unnamed);
			Assert.IsFalse(depending1.Foo == unnamed);
			Assert.IsFalse(depending2.Foo == named);
			Assert.IsFalse(depending2.Foo == named);
		}

		// Mode 2

		[TestMethod]
		public void CanInjectExistingUnnamedObjectIntoProperty()
		{
			// Mode 2, with an existing object
			MockBuilderContext context = CreateContext();
			object dependent = new object();
			context.Locator.Add(new DependencyResolutionLocatorKey(typeof (object), null), dependent);

			object depending = context.HeadOfChain.BuildUp(context, typeof (MockDependingObject), null, null);

			Assert.IsNotNull(depending);
			Assert.IsTrue(depending is MockDependingObject);
			Assert.AreSame(dependent, ((MockDependingObject) depending).InjectedObject);
		}

		[TestMethod]
		public void InjectionCreatingNewUnnamedObjectWillOnlyCreateOnce()
		{
			// Mode 2, both flavors
			MockBuilderContext context;

			context = CreateContext();
			MockDependingObject depending1 =
				(MockDependingObject) context.HeadOfChain.BuildUp(context, typeof (MockDependingObject), null, null);

			context = CreateContext(context.Locator);
			MockDependingObject depending2 =
				(MockDependingObject) context.HeadOfChain.BuildUp(context, typeof (MockDependingObject), null, null);

			Assert.AreSame(depending1.InjectedObject, depending2.InjectedObject);
		}

		[TestMethod]
		public void InjectionCreatesNewObjectIfNotExisting()
		{
			// Mode 2, no existing object
			MockBuilderContext context = CreateContext();

			object depending = context.HeadOfChain.BuildUp(context, typeof (MockDependingObject), null, null);

			Assert.IsNotNull(depending);
			Assert.IsTrue(depending is MockDependingObject);
			Assert.IsNotNull(((MockDependingObject) depending).InjectedObject);
		}

		[TestMethod]
		public void CanInjectNewInstanceWithExplicitTypeIfNotExisting()
		{
			// Mode 2, explicit type
			MockBuilderContext context = CreateContext();

			MockDependsOnIFoo depending = (MockDependsOnIFoo) context.HeadOfChain.BuildUp(
			                                                  	context, typeof (MockDependsOnIFoo), null, null);

			Assert.IsNotNull(depending);
			Assert.IsNotNull(depending.Foo);
		}

		// Mode 3

		[TestMethod]
		public void CanInjectExistingNamedObjectIntoProperty()
		{
			// Mode 3, with an existing object
			MockBuilderContext context = CreateContext();
			object dependent = new object();
			context.Locator.Add(new DependencyResolutionLocatorKey(typeof (object), "Foo"), dependent);

			object depending = context.HeadOfChain.BuildUp(context, typeof (MockDependingNamedObject), null, null);

			Assert.IsNotNull(depending);
			Assert.IsTrue(depending is MockDependingNamedObject);
			Assert.AreSame(dependent, ((MockDependingNamedObject) depending).InjectedObject);
		}

		[TestMethod]
		public void InjectionCreatingNewNamedObjectWillOnlyCreateOnce()
		{
			// Mode 3, both flavors
			MockBuilderContext context;

			context = CreateContext();
			MockDependingNamedObject depending1 =
				(MockDependingNamedObject) context.HeadOfChain.BuildUp(context, typeof (MockDependingNamedObject), null, null);

			context = CreateContext(context.Locator);
			MockDependingNamedObject depending2 =
				(MockDependingNamedObject) context.HeadOfChain.BuildUp(context, typeof (MockDependingNamedObject), null, null);

			Assert.AreSame(depending1.InjectedObject, depending2.InjectedObject);
		}

		[TestMethod]
		public void InjectionCreatesNewNamedObjectIfNotExisting()
		{
			// Mode 3, no existing object
			MockBuilderContext context = CreateContext();

			MockDependingNamedObject depending =
				(MockDependingNamedObject) context.HeadOfChain.BuildUp(context, typeof (MockDependingNamedObject), null, null);

			Assert.IsNotNull(depending);
			Assert.IsNotNull(depending.InjectedObject);
		}

		[TestMethod]
		public void CanInjectNewNamedInstanceWithExplicitTypeIfNotExisting()
		{
			// Mode 3, explicit type
			MockBuilderContext context = CreateContext();

			MockDependsOnNamedIFoo depending =
				(MockDependsOnNamedIFoo) context.HeadOfChain.BuildUp(context, typeof (MockDependsOnNamedIFoo), null, null);

			Assert.IsNotNull(depending);
			Assert.IsNotNull(depending.Foo);
		}

		// Mode 2 & 3 together

		[TestMethod]
		public void NamedAndUnnamedObjectsDontCollide()
		{
			MockBuilderContext context = CreateContext();
			object dependent = new object();
			context.Locator.Add(new DependencyResolutionLocatorKey(typeof (object), null), dependent);

			MockDependingNamedObject depending =
				(MockDependingNamedObject) context.HeadOfChain.BuildUp(context, typeof (MockDependingNamedObject), null, null);

			Assert.IsFalse(ReferenceEquals(dependent, depending.InjectedObject));
		}

		// Mode 4

		[TestMethod]
		public void PropertyIsNullIfUnnamedNotExists()
		{
			// Mode 4, no object provided
			MockBuilderContext context = CreateContext();

			MockOptionalDependingObject depending =
				(MockOptionalDependingObject) context.HeadOfChain.BuildUp(context, typeof (MockOptionalDependingObject), null, null);

			Assert.IsNotNull(depending);
			Assert.IsNull(depending.InjectedObject);
		}

		[TestMethod]
		public void CanInjectExistingUnnamedObjectIntoOptionalDependentProperty()
		{
			// Mode 4, with an existing object
			MockBuilderContext context = CreateContext();
			object dependent = new object();
			context.Locator.Add(new DependencyResolutionLocatorKey(typeof (object), null), dependent);

			object depending = context.HeadOfChain.BuildUp(context, typeof (MockOptionalDependingObject), null, null);

			Assert.IsNotNull(depending);
			Assert.IsTrue(depending is MockOptionalDependingObject);
			Assert.AreSame(dependent, ((MockOptionalDependingObject) depending).InjectedObject);
		}

		// Mode 5

		[TestMethod]
		public void PropertyIsNullIfNamedNotExists()
		{
			// Mode 5, no object provided
			MockBuilderContext context = CreateContext();

			MockOptionalDependingObjectWithName depending =
				(MockOptionalDependingObjectWithName)
				context.HeadOfChain.BuildUp(context, typeof (MockOptionalDependingObjectWithName), null, null);

			Assert.IsNotNull(depending);
			Assert.IsNull(depending.InjectedObject);
		}

		[TestMethod]
		public void CanInjectExistingNamedObjectIntoOptionalDependentProperty()
		{
			// Mode 5, with an existing object
			MockBuilderContext context = CreateContext();
			object dependent = new object();
			context.Locator.Add(new DependencyResolutionLocatorKey(typeof (object), "Foo"), dependent);

			object depending = context.HeadOfChain.BuildUp(context, typeof (MockOptionalDependingObjectWithName), null, null);

			Assert.IsNotNull(depending);
			Assert.IsTrue(depending is MockOptionalDependingObjectWithName);
			Assert.AreSame(dependent, ((MockOptionalDependingObjectWithName) depending).InjectedObject);
		}

		// NotPresentBehavior.Throw Tests

		[TestMethod]
		[ExpectedException(typeof (DependencyMissingException))]
		public void StrategyThrowsIfObjectNotPresent()
		{
			MockBuilderContext context = CreateContext();

			context.HeadOfChain.BuildUp(context, typeof (ThrowingMockObject), null, null);
		}

		[TestMethod]
		[ExpectedException(typeof (DependencyMissingException))]
		public void StrategyThrowsIfNamedObjectNotPresent()
		{
			MockBuilderContext context = CreateContext();

			context.HeadOfChain.BuildUp(context, typeof (NamedThrowingMockObject), null, null);
		}

		// SearchMode Tests

		[TestMethod]
		public void CanSearchDependencyUp()
		{
			Locator parent = new Locator();
			// We're having a problem with this test intermittently failing.
			// Since the locator is a weak referencing container, our current
			// theory is that the GC is collecting this dependency before
			// the buildup call runs. By boxing it explicitly, we can keep
			// the object alive until after the test completes.
			object intValue = 25;
			parent.Add(new DependencyResolutionLocatorKey(typeof (int), null), intValue);
			Locator child = new Locator(parent);
			MockBuilderContext context = CreateContext(child);

			GC.Collect();
			context.HeadOfChain.BuildUp(context, typeof (SearchUpMockObject), null, null);
			GC.KeepAlive(intValue);
		}

		[TestMethod]
		[ExpectedException(typeof (DependencyMissingException))]
		public void LocalSearchFailsIfDependencyIsOnlyUpstream()
		{
			Locator parent = new Locator();
			object parentValue = 25;
			parent.Add(new DependencyResolutionLocatorKey(typeof (int), null), parentValue);
			Locator child = new Locator(parent);
			MockBuilderContext context = CreateContext(child);

			context.HeadOfChain.BuildUp(context, typeof (SearchLocalMockObject), null, null);
		}


		[TestMethod]
		public void LocalSearchGetsLocalIfDependencyIsAlsoUpstream()
		{
			Locator parent = new Locator();
			object parentValue = 25;
			object childValue = 15;
			parent.Add(new DependencyResolutionLocatorKey(typeof (int), null), parentValue);
			Locator child = new Locator(parent);
			child.Add(new DependencyResolutionLocatorKey(typeof (int), null), childValue);
			MockBuilderContext context = CreateContext(child);

			SearchLocalMockObject obj =
				(SearchLocalMockObject) context.HeadOfChain.BuildUp(context, typeof (SearchLocalMockObject), null, null);

			Assert.AreEqual(15, obj.Value);
		}

		// Caching

		[TestMethod]
		public void RelectionStrategyCacheMethodReflectionInfo()
		{
			MockMethodReflectionStrategy mockReflectionStrategy = new MockMethodReflectionStrategy();

			MockBuilderContext context1 = CreateContext(mockReflectionStrategy);
			MockBuilderContext context2 = CreateContext(mockReflectionStrategy);

			int methodsCount = typeof (MockDecoratedMethodObject).GetMethods().Length;

			// building the object twice should call once to GetMembers and MemberRequiresProcessing
			context1.HeadOfChain.BuildUp(context1, typeof (MockDecoratedMethodObject), null, null);
			context2.HeadOfChain.BuildUp(context2, typeof (MockDecoratedMethodObject), null, null);

			Assert.AreEqual(1, mockReflectionStrategy.GetMembersCalledCount);
			Assert.AreEqual(methodsCount, mockReflectionStrategy.MemberRequiresProcessingCalledCount);
		}

		// Helpers

		private MockBuilderContext CreateContext()
		{
			return CreateContext(new Locator());
		}

		private MockBuilderContext CreateContext(IReadWriteLocator locator)
		{
			IBuilderStrategy[] strategies = {
			                                	new SingletonStrategy(),
			                                	new MethodReflectionStrategy(),
			                                	new CreationStrategy(),
			                                	new MethodExecutionStrategy()
			                                };
			MockBuilderContext result = new MockBuilderContext(strategies);
			result.Locator = locator;

			LifetimeContainer lifetimeContainer = new LifetimeContainer();

			if (!result.Locator.Contains(typeof (ILifetimeContainer)))
				result.Locator.Add(typeof (ILifetimeContainer), lifetimeContainer);

			result.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
			result.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));
			return result;
		}

		private MockBuilderContext CreateContext(MockMethodReflectionStrategy mockReflectionStrategy)
		{
			IBuilderStrategy[] strategies = {
			                                	new SingletonStrategy(),
			                                	mockReflectionStrategy,
			                                	new CreationStrategy()
			                                };
			MockBuilderContext context = new MockBuilderContext(strategies);

			context.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
			context.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));
			return context;
		}

		#region Nested type: MockMethodReflectionStrategy

		public class MockMethodReflectionStrategy : MethodReflectionStrategy
		{
			public int GetMembersCalledCount = 0;
			public int MemberRequiresProcessingCalledCount = 0;

			protected override IEnumerable<IReflectionMemberInfo<MethodInfo>> GetMembers(IBuilderContext context,
			                                                                             Type typeToBuild, object existing,
			                                                                             string idToBuild)
			{
				GetMembersCalledCount++;
				return base.GetMembers(context, typeToBuild, existing, idToBuild);
			}

			protected override bool MemberRequiresProcessing(IReflectionMemberInfo<MethodInfo> member)
			{
				MemberRequiresProcessingCalledCount++;
				return base.MemberRequiresProcessing(member);
			}
		}

		#region Mock Classes

		#region Nested type: Foo

		public class Foo : IFoo
		{
		}

		#endregion

		#region Nested type: IFoo

		public interface IFoo
		{
		}

		#endregion

		#region Nested type: MockDecoratedMethodObject

		public class MockDecoratedMethodObject
		{
			[InjectionMethod]
			public void InjectionMethod([CreateNew] object foo)
			{
			}

			public void NonInjectionMethod()
			{
			}
		}

		#endregion

		#region Nested type: MockDependingNamedObject

		public class MockDependingNamedObject
		{
			public object InjectedObject;

			[InjectionMethod]
			public void SetObject([Dependency(Name = "Foo")] object foo)
			{
				InjectedObject = foo;
			}
		}

		#endregion

		#region Nested type: MockDependingObject

		public class MockDependingObject
		{
			public object InjectedObject;

			[InjectionMethod]
			public virtual void DoSomething([Dependency] object injectedObject)
			{
				InjectedObject = injectedObject;
			}
		}

		#endregion

		#region Nested type: MockDependingObjectDerived

		public class MockDependingObjectDerived : MockDependingObject
		{
			public override void DoSomething(object injectedObject)
			{
				base.DoSomething(injectedObject);
			}
		}

		#endregion

		#region Nested type: MockDependsOnIFoo

		public class MockDependsOnIFoo
		{
			public IFoo Foo;

			[InjectionMethod]
			public void SetFoo([Dependency(CreateType = typeof (Foo))] IFoo foo)
			{
				Foo = foo;
			}
		}

		#endregion

		#region Nested type: MockDependsOnInterface

		public class MockDependsOnInterface
		{
			[InjectionMethod]
			public void DoSomething(IFoo foo)
			{
			}
		}

		#endregion

		#region Nested type: MockDependsOnNamedIFoo

		public class MockDependsOnNamedIFoo
		{
			public IFoo Foo;

			[InjectionMethod]
			public void SetFoo([Dependency(Name = "Foo", CreateType = typeof (Foo))] IFoo foo)
			{
				Foo = foo;
			}
		}

		#endregion

		#region Nested type: MockInvalidDualAttributes

		public class MockInvalidDualAttributes
		{
			[InjectionMethod]
			public void SetValue([CreateNew] [Dependency] int dummy)
			{
			}
		}

		#endregion

		#region Nested type: MockOptionalDependingObject

		public class MockOptionalDependingObject
		{
			public object InjectedObject;

			[InjectionMethod]
			public void SetObject([Dependency(NotPresentBehavior = NotPresentBehavior.ReturnNull)] object foo)
			{
				InjectedObject = foo;
			}
		}

		#endregion

		#region Nested type: MockOptionalDependingObjectWithName

		public class MockOptionalDependingObjectWithName
		{
			public object InjectedObject;

			[InjectionMethod]
			public void SetObject([Dependency(Name = "Foo", NotPresentBehavior = NotPresentBehavior.ReturnNull)] object foo)
			{
				InjectedObject = foo;
			}
		}

		#endregion

		#region Nested type: MockRequiresNewObject

		public class MockRequiresNewObject
		{
			public object Foo;

			[InjectionMethod]
			public virtual void SetFoo([CreateNew] object foo)
			{
				Foo = foo;
			}
		}

		#endregion

		#region Nested type: MockRequiresNewObjectDerived

		public class MockRequiresNewObjectDerived : MockRequiresNewObject
		{
			public override void SetFoo(object foo)
			{
				base.SetFoo(foo);
			}
		}

		#endregion

		#region Nested type: NamedThrowingMockObject

		public class NamedThrowingMockObject
		{
			[InjectionMethod]
			public void SetValue([Dependency(Name = "Foo", NotPresentBehavior = NotPresentBehavior.Throw)] object obj)
			{
			}
		}

		#endregion

		#region Nested type: SearchLocalMockObject

		public class SearchLocalMockObject
		{
			public int Value;

			[InjectionMethod]
			public void SetValue(
				[Dependency(SearchMode = SearchMode.Local, NotPresentBehavior = NotPresentBehavior.Throw)] int value)
			{
				Value = value;
			}
		}

		#endregion

		#region Nested type: SearchUpMockObject

		public class SearchUpMockObject
		{
			public int Value;

			[InjectionMethod]
			public void SetValue(
				[Dependency(SearchMode = SearchMode.Up, NotPresentBehavior = NotPresentBehavior.Throw)] int value)
			{
				Value = value;
			}
		}

		#endregion

		#region Nested type: ThrowingMockObject

		public class ThrowingMockObject
		{
			[InjectionMethod]
			public void SetValue([Dependency(NotPresentBehavior = NotPresentBehavior.Throw)] object obj)
			{
			}
		}

		#endregion

		#endregion

		#endregion
	}
}
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
using ConstructorReflectionStrategy=
	Microsoft.Practices.CompositeWeb.ObjectBuilder.Strategies.ConstructorReflectionStrategy;
using CreationStrategy=Microsoft.Practices.CompositeWeb.ObjectBuilder.Strategies.CreationStrategy;

namespace Microsoft.Practices.CompositeWeb.Tests.ObjectBuilder.Strategies
{
	// These "modes" describe the classed of behavior provided by DI.
	// 1. I need a new X. Don'typeToBuild reuse any existing ones.
	// 2. I need the unnamed X. Create it if it doesn'typeToBuild exist, else return the existing one.
	// 3. I need the X named Y. Create it if it doesn'typeToBuild exist, else return the existing one.
	// 4. I want the unnamed X. Return null if it doesn'typeToBuild exist.
	// 5. I want the X named Y. Return null if it doesn'typeToBuild exist.

	[TestClass]
	public class ConstructorReflectionStrategyFixture
	{
		// Value type creation

		[TestMethod]
		public void CanCreateValueTypesWithConstructorInjectionStrategyInPlace()
		{
			MockBuilderContext context = CreateContext();

			Assert.AreEqual(0, context.HeadOfChain.BuildUp(context, typeof (int), null, null));
		}

		// Invalid attribute combination

		[TestMethod]
		[ExpectedException(typeof (InvalidAttributeException))]
		public void SpecifyingMultipleConstructorsThrows()
		{
			MockBuilderContext context = CreateContext();

			context.HeadOfChain.BuildUp(context, typeof (MockInvalidDualConstructorAttributes), null, null);
		}

		[TestMethod]
		[ExpectedException(typeof (InvalidAttributeException))]
		public void SpecifyingCreateNewAndDependencyThrows()
		{
			MockBuilderContext context = CreateContext();

			context.HeadOfChain.BuildUp(context, typeof (MockInvalidDualParameterAttributes), null, null);
		}

		// Default behavior

		[TestMethod]
		public void DefaultBehaviorIsMode2ForUndecoratedParameter()
		{
			MockBuilderContext context = CreateContext();

			MockUndecoratedObject obj1 =
				(MockUndecoratedObject) context.HeadOfChain.BuildUp(context, typeof (MockUndecoratedObject), null, null);
			MockUndecoratedObject obj2 =
				(MockUndecoratedObject) context.HeadOfChain.BuildUp(context, typeof (MockUndecoratedObject), null, null);

			Assert.AreSame(obj1.Foo, obj2.Foo);
		}

		[TestMethod]
		public void WhenSingleConstructorIsPresentDecorationIsntRequired()
		{
			MockBuilderContext context = CreateContext();

			MockUndecoratedConstructorObject obj1 =
				(MockUndecoratedConstructorObject)
				context.HeadOfChain.BuildUp(context, typeof (MockUndecoratedConstructorObject), null, null);
			MockUndecoratedConstructorObject obj2 =
				(MockUndecoratedConstructorObject)
				context.HeadOfChain.BuildUp(context, typeof (MockUndecoratedConstructorObject), null, null);

			Assert.IsNotNull(obj1.Foo);
			Assert.AreSame(obj1.Foo, obj2.Foo);
		}

		// Mode 1

		[TestMethod]
		public void CreateNewAttributeAlwaysCreatesNewObject()
		{
			MockBuilderContext context = CreateContext();

			MockRequiresNewObject depending1 =
				(MockRequiresNewObject) context.HeadOfChain.BuildUp(context, typeof (MockRequiresNewObject), null, "Foo");
			MockRequiresNewObject depending2 =
				(MockRequiresNewObject) context.HeadOfChain.BuildUp(context, typeof (MockRequiresNewObject), null, "Bar");

			Assert.IsNotNull(depending1);
			Assert.IsNotNull(depending2);
			Assert.IsNotNull(depending1.Foo);
			Assert.IsNotNull(depending2.Foo);
			Assert.IsFalse(ReferenceEquals(depending1.Foo, depending2.Foo));
		}

		[TestMethod]
		public void NamedAndUnnamedObjectsInLocatorDontGetUsedForCreateNew()
		{
			MockBuilderContext context = CreateContext();
			object unnamed = new object();
			object named = new object();
			context.Locator.Add(new DependencyResolutionLocatorKey(typeof (object), null), unnamed);
			context.Locator.Add(new DependencyResolutionLocatorKey(typeof (object), "Foo"), named);

			MockRequiresNewObject depending1 =
				(MockRequiresNewObject) context.HeadOfChain.BuildUp(context, typeof (MockRequiresNewObject), null, null);
			MockRequiresNewObject depending2 =
				(MockRequiresNewObject) context.HeadOfChain.BuildUp(context, typeof (MockRequiresNewObject), null, null);

			Assert.IsFalse(depending1.Foo == unnamed);
			Assert.IsFalse(depending2.Foo == unnamed);
			Assert.IsFalse(depending1.Foo == named);
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
			MockBuilderContext context = CreateContext();

			MockDependingObject depending1 =
				(MockDependingObject) context.HeadOfChain.BuildUp(context, typeof (MockDependingObject), null, null);
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
			MockBuilderContext context = CreateContext();

			MockDependingNamedObject depending1 =
				(MockDependingNamedObject) context.HeadOfChain.BuildUp(context, typeof (MockDependingNamedObject), null, null);
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
			object intValue = 25;
			parent.Add(new DependencyResolutionLocatorKey(typeof (int), null), intValue);
			Locator child = new Locator(parent);
			MockBuilderContext context = CreateContext(child);

			context.HeadOfChain.BuildUp(context, typeof (SearchUpMockObject), null, null);
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
		public void RelectionStrategyCacheConstructorReflectionInfo()
		{
			MockConstructorReflectionStrategy mockReflectionStrategy = new MockConstructorReflectionStrategy();

			MockBuilderContext context1 = CreateContext(mockReflectionStrategy);
			MockBuilderContext context2 = CreateContext(mockReflectionStrategy);

			int ctorCount = typeof (MockDecoratedCtorObject).GetConstructors().Length;

			// building the object twice should call once to GetMembers and MemberRequiresProcessing
			context1.HeadOfChain.BuildUp(context1, typeof (MockDecoratedCtorObject), null, null);
			context2.HeadOfChain.BuildUp(context2, typeof (MockDecoratedCtorObject), null, null);

			Assert.AreEqual(1, mockReflectionStrategy.GetMembersCalledCount);
			Assert.AreEqual(ctorCount, mockReflectionStrategy.MemberRequiresProcessingCalledCount);
		}


		// Helpers        

		private MockBuilderContext CreateContext()
		{
			return CreateContext(new Locator());
		}

		private MockBuilderContext CreateContext(Locator locator)
		{
			IBuilderStrategy[] strategies = {
			                                	new SingletonStrategy(),
			                                	new ConstructorReflectionStrategy(),
			                                	new CreationStrategy()
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

		private MockBuilderContext CreateContext(MockConstructorReflectionStrategy mockReflectionStrategy)
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

		#region Nested type: MockConstructorReflectionStrategy

		public class MockConstructorReflectionStrategy : ConstructorReflectionStrategy
		{
			public int GetMembersCalledCount = 0;
			public int MemberRequiresProcessingCalledCount = 0;

			protected override IEnumerable<IReflectionMemberInfo<ConstructorInfo>> GetMembers(IBuilderContext context,
			                                                                                  Type typeToBuild, object existing,
			                                                                                  string idToBuild)
			{
				GetMembersCalledCount++;
				return base.GetMembers(context, typeToBuild, existing, idToBuild);
			}

			protected override bool MemberRequiresProcessing(IReflectionMemberInfo<ConstructorInfo> member)
			{
				MemberRequiresProcessingCalledCount++;
				return base.MemberRequiresProcessing(member);
			}
		}

		#endregion

		#region Nested type: MockDecoratedCtorObject

		private class MockDecoratedCtorObject
		{
			[InjectionConstructor]
			public MockDecoratedCtorObject()
			{
			}
		}

		#endregion

		#region Nested type: MockDependingNamedObject

		public class MockDependingNamedObject
		{
			private object injectedObject;

			public MockDependingNamedObject([Dependency(Name = "Foo")] object injectedObject)
			{
				this.injectedObject = injectedObject;
			}

			public object InjectedObject
			{
				get { return injectedObject; }
				set { injectedObject = value; }
			}
		}

		#endregion

		#region Nested type: MockDependingObject

		public class MockDependingObject
		{
			private object injectedObject;

			public MockDependingObject([Dependency] object injectedObject)
			{
				this.injectedObject = injectedObject;
			}

			public virtual object InjectedObject
			{
				get { return injectedObject; }
				set { injectedObject = value; }
			}
		}

		#endregion

		#region Nested type: MockDependsOnIFoo

		public class MockDependsOnIFoo
		{
			private IFoo foo;

			public MockDependsOnIFoo([Dependency(CreateType = typeof (Foo))] IFoo foo)
			{
				this.foo = foo;
			}

			public IFoo Foo
			{
				get { return foo; }
				set { foo = value; }
			}
		}

		#endregion

		#region Nested type: MockDependsOnNamedIFoo

		public class MockDependsOnNamedIFoo
		{
			private IFoo foo;

			public MockDependsOnNamedIFoo([Dependency(Name = "Foo", CreateType = typeof (Foo))] IFoo foo)
			{
				this.foo = foo;
			}

			public IFoo Foo
			{
				get { return foo; }
				set { foo = value; }
			}
		}

		#endregion

		#region Nested type: MockInvalidDualConstructorAttributes

		private class MockInvalidDualConstructorAttributes
		{
			[InjectionConstructor]
			public MockInvalidDualConstructorAttributes(object obj)
			{
			}

			[InjectionConstructor]
			public MockInvalidDualConstructorAttributes(int i)
			{
			}
		}

		#endregion

		#region Nested type: MockInvalidDualParameterAttributes

		private class MockInvalidDualParameterAttributes
		{
			[InjectionConstructor]
			public MockInvalidDualParameterAttributes([CreateNew] [Dependency] object obj)
			{
			}
		}

		#endregion

		#region Nested type: MockOptionalDependingObject

		public class MockOptionalDependingObject
		{
			private object injectedObject;

			public MockOptionalDependingObject
				(
				[Dependency(NotPresentBehavior = NotPresentBehavior.ReturnNull)] object injectedObject
				)
			{
				this.injectedObject = injectedObject;
			}

			public object InjectedObject
			{
				get { return injectedObject; }
				set { injectedObject = value; }
			}
		}

		#endregion

		#region Nested type: MockOptionalDependingObjectWithName

		public class MockOptionalDependingObjectWithName
		{
			private object injectedObject;

			public MockOptionalDependingObjectWithName
				(
				[Dependency(Name = "Foo", NotPresentBehavior = NotPresentBehavior.ReturnNull)] object injectedObject
				)
			{
				this.injectedObject = injectedObject;
			}

			public object InjectedObject
			{
				get { return injectedObject; }
				set { injectedObject = value; }
			}
		}

		#endregion

		#region Nested type: MockRequiresNewObject

		public class MockRequiresNewObject
		{
			private object foo;

			public MockRequiresNewObject([CreateNew] object foo)
			{
				this.foo = foo;
			}

			public virtual object Foo
			{
				get { return foo; }
				set { foo = value; }
			}
		}

		#endregion

		#region Nested type: MockUndecoratedConstructorObject

		private class MockUndecoratedConstructorObject
		{
			public object Foo;

			public MockUndecoratedConstructorObject(object foo)
			{
				Foo = foo;
			}
		}

		#endregion

		#region Nested type: MockUndecoratedObject

		private class MockUndecoratedObject
		{
			public object Foo;

			[InjectionConstructor]
			public MockUndecoratedObject(object foo)
			{
				Foo = foo;
			}
		}

		#endregion

		#region Nested type: NamedThrowingMockObject

		public class NamedThrowingMockObject
		{
			[InjectionConstructor]
			public NamedThrowingMockObject([Dependency(Name = "Foo", NotPresentBehavior = NotPresentBehavior.Throw)] object foo)
			{
			}
		}

		#endregion

		#region Nested type: SearchLocalMockObject

		public class SearchLocalMockObject
		{
			public int Value;

			public SearchLocalMockObject(
				[Dependency(SearchMode = SearchMode.Local, NotPresentBehavior = NotPresentBehavior.Throw)] int value
				)
			{
				Value = value;
			}
		}

		#endregion

		#region Nested type: SearchUpMockObject

		public class SearchUpMockObject
		{
			public int Value;

			public SearchUpMockObject(
				[Dependency(SearchMode = SearchMode.Up, NotPresentBehavior = NotPresentBehavior.Throw)] int value)
			{
				Value = value;
			}
		}

		#endregion

		#region Nested type: ThrowingMockObject

		public class ThrowingMockObject
		{
			[InjectionConstructor]
			public ThrowingMockObject([Dependency(NotPresentBehavior = NotPresentBehavior.Throw)] object foo)
			{
			}
		}

		#endregion
	}
}
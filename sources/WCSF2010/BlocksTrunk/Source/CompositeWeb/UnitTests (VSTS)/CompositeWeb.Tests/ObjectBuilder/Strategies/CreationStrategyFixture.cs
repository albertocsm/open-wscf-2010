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
using System.Reflection;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests.ObjectBuilder.Strategies
{
	[TestClass]
	public class CreationStrategyFixture
	{
		[TestMethod]
		[ExpectedException(typeof (ArgumentException))]
		public void CreationStrategyWithNoPoliciesFails()
		{
			MockBuilderContext ctx = CreateContext();

			ctx.HeadOfChain.BuildUp(ctx, typeof (object), null, null);
		}

		[TestMethod]
		public void CreationStrategyUsesSingletonPolicyToLocateCreatedItems()
		{
			MockBuilderContext ctx = CreateContext();
			ILifetimeContainer container = ctx.Locator.Get<ILifetimeContainer>();
			ctx.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
			ctx.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));

			object obj = ctx.HeadOfChain.BuildUp(ctx, typeof (object), null, null);

			Assert.AreEqual(1, container.Count);
			Assert.AreSame(obj, ctx.Locator.Get(new DependencyResolutionLocatorKey(typeof (object), null)));
		}

		[TestMethod]
		public void CreationStrategyOnlyLocatesItemIfSingletonPolicySetForThatType()
		{
			MockBuilderContext ctx = CreateContext();
			ILifetimeContainer container = ctx.Locator.Get<ILifetimeContainer>();
			ctx.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
			ctx.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));
			ctx.Policies.Set<ISingletonPolicy>(new SingletonPolicy(false), typeof (object), null);

			object obj = ctx.HeadOfChain.BuildUp(ctx, typeof (object), null, null);

			Assert.AreEqual(0, container.Count);
			Assert.IsNull(ctx.Locator.Get(new DependencyResolutionLocatorKey(typeof (object), null)));
		}

		[TestMethod]
		public void AllCreatedDependenciesArePlacedIntoLocatorAndLifetimeContainer()
		{
			MockBuilderContext ctx = CreateContext();
			ILifetimeContainer container = ctx.Locator.Get<ILifetimeContainer>();
			ctx.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
			ctx.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));

			MockDependingObject obj =
				(MockDependingObject) ctx.HeadOfChain.BuildUp(ctx, typeof (MockDependingObject), null, null);

			Assert.AreEqual(2, container.Count);
			Assert.AreSame(obj, ctx.Locator.Get(new DependencyResolutionLocatorKey(typeof (MockDependingObject), null)));
			Assert.AreSame(obj.DependentObject,
			               ctx.Locator.Get(new DependencyResolutionLocatorKey(typeof (MockDependentObject), null)));
		}

		[TestMethod]
		public void InjectedDependencyIsReusedWhenDependingObjectIsCreatedTwice()
		{
			MockBuilderContext ctx = CreateContext();
			ILifetimeContainer container = ctx.Locator.Get<ILifetimeContainer>();
			ctx.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
			ctx.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));

			MockDependingObject obj1 =
				(MockDependingObject) ctx.HeadOfChain.BuildUp(ctx, typeof (MockDependingObject), null, null);
			MockDependingObject obj2 =
				(MockDependingObject) ctx.HeadOfChain.BuildUp(ctx, typeof (MockDependingObject), null, null);

			Assert.AreSame(obj1.DependentObject, obj2.DependentObject);
		}

		[TestMethod]
		public void NamedObjectsOfSameTypeAreUnique()
		{
			MockBuilderContext ctx = CreateContext();
			ILifetimeContainer container = ctx.Locator.Get<ILifetimeContainer>();
			ctx.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
			ctx.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));

			object obj1 = ctx.HeadOfChain.BuildUp(ctx, typeof (object), null, "Foo");
			object obj2 = ctx.HeadOfChain.BuildUp(ctx, typeof (object), null, "Bar");

			Assert.AreEqual(2, container.Count);
			Assert.IsFalse(ReferenceEquals(obj1, obj2));
		}

		[TestMethod]
		public void CircularDependenciesCanBeResolved()
		{
			MockBuilderContext ctx = CreateContext();
			ILifetimeContainer container = ctx.Locator.Get<ILifetimeContainer>();
			ctx.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
			ctx.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));

			CircularDependency1 d1 = (CircularDependency1) ctx.HeadOfChain.BuildUp(ctx, typeof (CircularDependency1), null, null);

			Assert.IsNotNull(d1);
			Assert.IsNotNull(d1.Depends2);
			Assert.IsNotNull(d1.Depends2.Depends1);
			Assert.AreSame(d1, d1.Depends2.Depends1);
			Assert.AreEqual(2, container.Count);
		}

		[TestMethod]
		[ExpectedException(typeof (ArgumentException))]
		public void CreatingAbstractTypeThrows()
		{
			MockBuilderContext ctx = CreateContext();
			ILifetimeContainer container = ctx.Locator.Get<ILifetimeContainer>();
			ctx.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
			ctx.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));

			ctx.HeadOfChain.BuildUp(ctx, typeof (AbstractClass), null, null);
		}

		[TestMethod]
		public void CanCreateValueTypes()
		{
			MockBuilderContext ctx = CreateContext();
			ctx.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());

			Assert.AreEqual(0, (int) ctx.HeadOfChain.BuildUp(ctx, typeof (int), null, null));
		}

		[TestMethod]
		[ExpectedException(typeof (ArgumentException))]
		public void CannotCreateStrings()
		{
			MockBuilderContext ctx = CreateContext();
			ctx.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());

			ctx.HeadOfChain.BuildUp(ctx, typeof (string), null, null);
		}

		[TestMethod]
		[ExpectedException(typeof (ArgumentException))]
		public void NotFindingAMatchingConstructorThrows()
		{
			MockBuilderContext ctx = CreateContext();
			FailingCreationPolicy policy = new FailingCreationPolicy();
			ctx.Policies.SetDefault<ICreationPolicy>(policy);

			ctx.HeadOfChain.BuildUp(ctx, typeof (object), null, null);
		}

		[TestMethod]
		public void CreationStrategyWillLocateExistingObjects()
		{
			MockBuilderContext ctx = CreateContext();
			ILifetimeContainer container = ctx.Locator.Get<ILifetimeContainer>();
			ctx.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
			ctx.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));
			object obj = new object();

			ctx.HeadOfChain.BuildUp(ctx, typeof (object), obj, null);

			Assert.AreEqual(1, container.Count);
			Assert.AreSame(obj, ctx.Locator.Get(new DependencyResolutionLocatorKey(typeof (object), null)));
		}

		[TestMethod]
		[ExpectedException(typeof (IncompatibleTypesException))]
		public void IncompatibleTypesThrows()
		{
			MockBuilderContext ctx = CreateContext();
			ILifetimeContainer container = ctx.Locator.Get<ILifetimeContainer>();
			ConstructorInfo ci = typeof (MockObject).GetConstructor(new Type[] {typeof (int)});
			ICreationPolicy policy = new ConstructorPolicy(ci, new ValueParameter<string>(String.Empty));
			ctx.Policies.Set<ICreationPolicy>(policy, typeof (MockObject), null);

			object obj = ctx.HeadOfChain.BuildUp(ctx, typeof (MockObject), null, null);
		}

		[TestMethod]
		public void CreationPolicyWillRecordSingletonsUsingLocalLifetimeContainerOnly()
		{
			BuilderStrategyChain chain = new BuilderStrategyChain();
			chain.Add(new CreationStrategy());

			Locator parentLocator = new Locator();
			LifetimeContainer container = new LifetimeContainer();
			parentLocator.Add(typeof (ILifetimeContainer), container);

			Locator childLocator = new Locator(parentLocator);

			PolicyList policies = new PolicyList();
			policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
			policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));

			BuilderContext ctx = new BuilderContext(chain, childLocator, policies);

			object obj = ctx.HeadOfChain.BuildUp(ctx, typeof (object), null, null);

			Assert.IsNotNull(obj);
			Assert.IsNull(childLocator.Get(new DependencyResolutionLocatorKey(typeof (object), null)));
		}

		#region Helpers

		private MockBuilderContext CreateContext()
		{
			MockBuilderContext result = new MockBuilderContext();
			result.InnerChain.Add(new SingletonStrategy());
			result.InnerChain.Add(new CreationStrategy());
			return result;
		}

		#region Nested type: AbstractClass

		private abstract class AbstractClass
		{
			public AbstractClass()
			{
			}
		}

		#endregion

		#region Nested type: CircularDependency1

		private class CircularDependency1
		{
			public CircularDependency2 Depends2;

			public CircularDependency1(CircularDependency2 depends2)
			{
				Depends2 = depends2;
			}
		}

		#endregion

		#region Nested type: CircularDependency2

		private class CircularDependency2
		{
			public CircularDependency1 Depends1;

			public CircularDependency2(CircularDependency1 depends1)
			{
				Depends1 = depends1;
			}
		}

		#endregion

		#region Nested type: FailingCreationPolicy

		internal class FailingCreationPolicy : ICreationPolicy
		{
			#region ICreationPolicy Members

			public ConstructorInfo SelectConstructor(IBuilderContext context, Type type, string id)
			{
				return null;
			}

			public object[] GetParameters(IBuilderContext context, Type type, string id, ConstructorInfo ci)
			{
				return new object[] {};
			}

			#endregion
		}

		#endregion

		#region Nested type: MockBuilderContext

		private class MockBuilderContext : BuilderContext
		{
			public BuilderStrategyChain InnerChain = new BuilderStrategyChain();
			public IReadWriteLocator InnerLocator;
			public PolicyList InnerPolicies = new PolicyList();
			public LifetimeContainer lifetimeContainer = new LifetimeContainer();

			public MockBuilderContext()
				: this(new Locator())
			{
			}

			public MockBuilderContext(IReadWriteLocator locator)
			{
				InnerLocator = locator;
				SetLocator(InnerLocator);
				StrategyChain = InnerChain;
				SetPolicies(InnerPolicies);

				if (!Locator.Contains(typeof (ILifetimeContainer)))
					Locator.Add(typeof (ILifetimeContainer), lifetimeContainer);
			}
		}

		#endregion

		#region Nested type: MockDependentObject

		private class MockDependentObject
		{
		}

		#endregion

		#region Nested type: MockDependingObject

		private class MockDependingObject
		{
			public object DependentObject;

			public MockDependingObject(MockDependentObject obj)
			{
				DependentObject = obj;
			}
		}

		#endregion

		#region Nested type: MockObject

		private class MockObject
		{
			private int _foo;

			public MockObject(int foo)
			{
				_foo = foo;
			}
		}

		#endregion

		#endregion
	}
}
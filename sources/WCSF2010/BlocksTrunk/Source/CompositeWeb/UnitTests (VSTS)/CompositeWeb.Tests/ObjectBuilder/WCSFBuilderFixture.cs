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
using System.Collections;
using System.Collections.Generic;
using Microsoft.Practices.CompositeWeb.ObjectBuilder;
using Microsoft.Practices.CompositeWeb.ObjectBuilder.BuildPlan;
using Microsoft.Practices.CompositeWeb.ObjectBuilder.Strategies;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests.ObjectBuilder
{
	/// <summary>
	/// Summary description for WCSFBuilderFixture
	/// </summary>
	[TestClass]
	public class WCSFBuilderFixture
	{
		public WCSFBuilderFixture()
		{
		}

		[TestMethod]
		public void EmptyBuilderWillCreateAnyValueTypeWithDefaultValue()
		{
			WCSFBuilder builder = new WCSFBuilder();
			Locator locator = CreateLocator();

			int actual = builder.BuildUp<int>(locator, null, null);
			Assert.AreEqual(default(int), actual);
		}

		[TestMethod]
		public void EmptyBuilderWillCreateSimpleInstances()
		{
			WCSFBuilder builder = new WCSFBuilder();
			Locator locator = CreateLocator();

			SimpleObject o = builder.BuildUp<SimpleObject>(locator, null, null);

			Assert.IsNotNull(o);
			Assert.AreEqual(0, o.IntParam);
		}

		[TestMethod]
		public void EmptyBuilderWillCreateComplexInstances()
		{
			WCSFBuilder builder = new WCSFBuilder();
			Locator locator = CreateLocator();

			ComplexObject o = builder.BuildUp<ComplexObject>(locator, null, null);

			Assert.IsNotNull(o);
			Assert.IsNotNull(o.SimpleObject);
			Assert.AreEqual(default(int), o.SimpleObject.IntParam);
		}

		[TestMethod]
		public void CanCreateSingletonObjectWithDefaultObjectBuilder()
		{
			WCSFBuilder builder = new WCSFBuilder();
			Locator locator = new Locator();
			LifetimeContainer lifetime = new LifetimeContainer();
			locator.Add(typeof (ILifetimeContainer), lifetime);

			builder.Policies.Set<ISingletonPolicy>(new SingletonPolicy(true), typeof (MockObject), "foo");

			MockObject obj1 = builder.BuildUp<MockObject>(locator, "foo", null);
			MockObject obj2 = builder.BuildUp<MockObject>(locator, "foo", null);

			Assert.AreSame(obj1, obj2);
		}

		[TestMethod]
		public void CanMapTypesWithDefaultObjectBuilder()
		{
			WCSFBuilder builder = new WCSFBuilder();
			Locator locator = CreateLocator();

			TypeMappingPolicy policy = new TypeMappingPolicy(typeof (MockObject), null);
			builder.Policies.Set<ITypeMappingPolicy>(policy, typeof (IMockObject), null);

			IMockObject obj = builder.BuildUp<IMockObject>(locator, null, null);

			Assert.IsTrue(obj is MockObject);
		}

		[TestMethod]
		public void SingletonPolicyBasedOnConcreteTypeRatherThanRequestedType()
		{
			WCSFBuilder builder = new WCSFBuilder();
			LifetimeContainer lifetimeContainer = new LifetimeContainer();
			Locator locator = CreateLocator(lifetimeContainer);
			builder.Policies.Set<ITypeMappingPolicy>(new TypeMappingPolicy(typeof (Foo), null), typeof (IFoo), null);
			builder.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));
			GC.Collect();
			object obj1 = builder.BuildUp(locator, typeof (IFoo), null, null);
			object obj2 = builder.BuildUp(locator, typeof (IFoo), null, null);

			Assert.AreSame(obj1, obj2);
			GC.KeepAlive(lifetimeContainer);
		}


		[TestMethod]
		public void StrategiesContainsOBTypeMappingStrategy()
		{
			WCSFBuilder builder = new WCSFBuilder();
			IBuilderStrategyChain chain = builder.Strategies.MakeStrategyChain();
			IBuilderStrategy head = chain.Head;
			Assert.IsTrue(head is TypeMappingStrategy);
		}

		[TestMethod]
		public void StrategiesContainsOBBuilderAwareStrategy()
		{
			WCSFBuilder builder = new WCSFBuilder();
			IBuilderStrategyChain chain = builder.Strategies.MakeStrategyChain();
			bool foundCorrectStrategyType = false;

			IBuilderStrategy current = chain.Head;
			do
			{
				if (current is BuilderAwareStrategy)
				{
					foundCorrectStrategyType = true;
				}
				current = chain.GetNext(current);
			} while ((current != null) && (foundCorrectStrategyType == false));
			Assert.IsTrue(foundCorrectStrategyType);
		}

		[TestMethod]
		public void StrategiesContainsCustomSimplifiedSingletonStrategy()
		{
			WCSFBuilder builder = new WCSFBuilder();
			IBuilderStrategyChain chain = builder.Strategies.MakeStrategyChain();
			bool foundCorrectStrategyType = false;

			IBuilderStrategy current = chain.Head;
			do
			{
				if (current is SimplifiedSingletonStrategy)
				{
					foundCorrectStrategyType = true;
				}
				current = chain.GetNext(current);
			} while ((current != null) && (foundCorrectStrategyType == false));
			Assert.IsTrue(foundCorrectStrategyType);
		}

		[TestMethod]
		public void StrategiesContainsCustomBuildPlanStrategy()
		{
			WCSFBuilder builder = new WCSFBuilder();
			IBuilderStrategyChain chain = builder.Strategies.MakeStrategyChain();
			bool foundCorrectStrategyType = false;

			IBuilderStrategy current = chain.Head;
			do
			{
				if (current is BuildPlanStrategy)
				{
					foundCorrectStrategyType = true;
				}
				current = chain.GetNext(current);
			} while ((current != null) && (foundCorrectStrategyType == false));
			Assert.IsTrue(foundCorrectStrategyType);
		}

		#region Helpers

		private Locator CreateLocator()
		{
			return CreateLocator(new LifetimeContainer());
		}

		private Locator CreateLocator(ILifetimeContainer lifetimeContainer)
		{
			Locator locator = new Locator();
			locator.Add(typeof (ILifetimeContainer), lifetimeContainer);
			return locator;
		}

		#region Nested type: ComplexObject

		private class ComplexObject
		{
			public SimpleObject SimpleObject;

			public ComplexObject(SimpleObject monk)
			{
				SimpleObject = monk;
			}
		}

		#endregion

		#region Nested type: Foo

		private class Foo : IFoo
		{
		}

		#endregion

		#region Nested type: IFoo

		private interface IFoo
		{
		}

		#endregion

		#region Nested type: IMockObject

		private interface IMockObject
		{
		}

		#endregion

		#region Nested type: MockLifetimeContainer

		private class MockLifetimeContainer : ILifetimeContainer
		{
			public bool WasDisposed = false;

			#region ILifetimeContainer Members

			public void Add(object item)
			{
				throw new NotImplementedException();
			}

			public bool Contains(object item)
			{
				throw new NotImplementedException();
			}

			public void Dispose()
			{
				WasDisposed = true;
			}

			public void Remove(object item)
			{
				throw new NotImplementedException();
			}

			public IEnumerator<object> GetEnumerator()
			{
				throw new NotImplementedException();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				throw new NotImplementedException();
			}

			public int Count
			{
				get { throw new NotImplementedException(); }
			}

			#endregion

			public event EventHandler<LifetimeEventArgs> Added;
			public event EventHandler<LifetimeEventArgs> Removed;

			private void UnusedEventsWarningRemoval()
			{
				Added(null, null);
				Removed(null, null);
			}
		}

		#endregion

		#region Nested type: MockLocator

		private class MockLocator : ReadWriteLocator
		{
			public object AddedKey;
			public object AddedValue;

			public override int Count
			{
				get { throw new NotImplementedException(); }
			}

			public override void Add(object key, object value)
			{
				AddedKey = key;
				AddedValue = value;
			}

			public override bool Contains(object key, SearchMode options)
			{
				throw new NotImplementedException();
			}

			public override object Get(object key, SearchMode options)
			{
				throw new NotImplementedException();
			}

			public override IEnumerator<KeyValuePair<object, object>> GetEnumerator()
			{
				throw new NotImplementedException();
			}

			public override bool Remove(object key)
			{
				throw new NotImplementedException();
			}
		}

		#endregion

		#region Nested type: MockObject

		private class MockObject : IMockObject
		{
			public int IntValue;

			public MockObject(int val)
			{
				IntValue = val;
			}
		}

		#endregion

		#region Nested type: MockStrategy

		private class MockStrategy : BuilderStrategy
		{
			public bool BuildWasRun = false;
			public string StringValue;
			public bool UnbuildWasRun = false;

			public MockStrategy()
				: this("")
			{
			}

			public MockStrategy(string value)
			{
				StringValue = value;
			}

			public override object BuildUp(IBuilderContext context, Type t, object existing, string id)
			{
				BuildWasRun = true;
				return base.BuildUp(context, t, AppendString(existing), id);
			}

			public override object TearDown(IBuilderContext context, object item)
			{
				UnbuildWasRun = true;
				return base.TearDown(context, AppendString(item));
			}

			private string AppendString(object item)
			{
				string result;

				if (item == null)
					result = StringValue;
				else
					result = ((string) item) + StringValue;

				return result;
			}
		}

		#endregion

		#region Nested type: PropertyObject

		private class PropertyObject
		{
			private int intProp;

			public PropertyObject()
			{
			}

			public int IntProp
			{
				get { return intProp; }
				set { intProp = value; }
			}
		}

		#endregion

		#region Nested type: SimpleObject

		private class SimpleObject
		{
			public int IntParam;

			public SimpleObject(int foo)
			{
				IntParam = foo;
			}
		}

		#endregion

		#endregion

		/*		
		 * Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
		* */
	}
}
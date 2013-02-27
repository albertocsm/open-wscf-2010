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
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.ObjectBuilder;
using Microsoft.Practices.CompositeWeb.Tests.Mocks;
using Microsoft.Practices.CompositeWeb.Web;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests
{
	[TestClass]
	public class StateDependencyAttributeFixture
	{
		[TestMethod]
		public void BuildUpReturnsNewInstanceOfStateValue()
		{
			Locator locator = new Locator();
			LifetimeContainer container = new LifetimeContainer();
			locator.Add(typeof (ILifetimeContainer), container);
			WCSFBuilder builder = new WCSFBuilder();
			builder.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));
			MockSessionStateLocatorService stateLocator = new MockSessionStateLocatorService();
			locator.Add(new DependencyResolutionLocatorKey(typeof (ISessionStateLocatorService), null), stateLocator);

			SampleClass builtObject =
				builder.BuildUp(locator, typeof (SampleClass), Guid.NewGuid().ToString(), null) as SampleClass;

			Assert.IsNotNull(builtObject.MyStringValue);
		}

		[TestMethod]
		public void BuildUpReturnsInstanceOfStateValueWithCorrespondantValueFromSessionWhenInjectingConstructor()
		{
			Locator locator = new Locator();
			LifetimeContainer container = new LifetimeContainer();
			locator.Add(typeof (ILifetimeContainer), container);
			WCSFBuilder builder = new WCSFBuilder();
			builder.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));
			MockSessionStateLocatorService stateLocator = new MockSessionStateLocatorService();
			locator.Add(new DependencyResolutionLocatorKey(typeof (ISessionStateLocatorService), null), stateLocator);

			stateLocator.SessionState["stringKey"] = "value";
			SampleClass builtObject =
				builder.BuildUp(locator, typeof (SampleClass), Guid.NewGuid().ToString(), null) as SampleClass;

			Assert.IsNotNull(builtObject.MyStringValue);
			Assert.IsNotNull(builtObject.MyStringValue.Value);
			Assert.AreEqual("value", builtObject.MyStringValue.Value);
		}


		[TestMethod]
		public void BuildUpReturnsInstanceOfStateValueWithCorrespondantValueFromSessionWhenInjectingProperties()
		{
			Locator locator = new Locator();
			LifetimeContainer container = new LifetimeContainer();
			locator.Add(typeof (ILifetimeContainer), container);
			WCSFBuilder builder = new WCSFBuilder();
			builder.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));
			MockSessionStateLocatorService stateLocator = new MockSessionStateLocatorService();
			locator.Add(new DependencyResolutionLocatorKey(typeof (ISessionStateLocatorService), null), stateLocator);

			stateLocator.SessionState["intKey"] = 5;
			SampleClass builtObject =
				builder.BuildUp(locator, typeof (SampleClass), Guid.NewGuid().ToString(), null) as SampleClass;

			Assert.IsNotNull(builtObject.MyIntValue);
			Assert.AreEqual(5, builtObject.MyIntValue.Value);
		}

		[TestMethod]
		[ExpectedException(typeof (ArgumentException))]
		public void BuildUpThrowsWhenTryingToInjectNonIStateValueParameters()
		{
			Locator locator = new Locator();
			LifetimeContainer container = new LifetimeContainer();
			locator.Add(typeof (ILifetimeContainer), container);
			WCSFBuilder builder = new WCSFBuilder();
			builder.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));
			locator.Add(new DependencyResolutionLocatorKey(typeof (ISessionStateLocatorService), null),
			            new MockSessionStateLocatorService());

			builder.BuildUp(locator, typeof (InvalidClass), Guid.NewGuid().ToString(), null);
		}

		#region Nested type: InvalidClass

		private class InvalidClass
		{
			[InjectionConstructor]
			public InvalidClass([StateDependency("key")] string myValue)
			{
			}
		}

		#endregion

		#region Nested type: SampleClass

		private class SampleClass
		{
			private StateValue<int> _myIntValue;
			private StateValue<string> _myStringValue;

			public SampleClass([StateDependency("stringKey")] StateValue<string> myStringValue)
			{
				_myStringValue = myStringValue;
			}

			public StateValue<string> MyStringValue
			{
				get { return _myStringValue; }
			}

			[StateDependency("intKey")]
			public StateValue<int> MyIntValue
			{
				get { return _myIntValue; }
				set { _myIntValue = value; }
			}
		}

		#endregion
	}
}
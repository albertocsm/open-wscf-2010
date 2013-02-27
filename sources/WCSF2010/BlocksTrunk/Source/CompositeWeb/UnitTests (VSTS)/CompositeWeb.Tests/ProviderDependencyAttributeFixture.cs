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
using System.Configuration.Provider;
using Microsoft.Practices.CompositeWeb.ObjectBuilder;
using Microsoft.Practices.CompositeWeb.Tests.Mocks;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests
{
	/// <summary>
	/// Summary description for ProviderDependencyAttributeFixture
	/// </summary>
	[TestClass]
	public class ProviderDependencyAttributeFixture
	{
		[TestMethod]
		public void CanCreateSpecifyingTheHostType()
		{
			ProviderDependencyAttribute attribute = new ProviderDependencyAttribute(typeof (TestProviderHost));

			Assert.AreEqual(typeof (TestProviderHost), attribute.ProviderHostType);
			Assert.AreEqual("Provider", attribute.ProviderGetterProperty);
		}

		[TestMethod]
		public void CanCreateSpecifyingHostTypeAndAccessor()
		{
			ProviderDependencyAttribute attribute = new ProviderDependencyAttribute(typeof (TestProviderHost), "AnotherProvider");

			Assert.AreEqual("AnotherProvider", attribute.ProviderGetterProperty);
		}

		[TestMethod]
		[ExpectedException(typeof (ArgumentException))]
		public void DeafultFailsIfHostHasNoProviderGetter()
		{
			ProviderDependencyAttribute attribute = new ProviderDependencyAttribute(typeof (EmptyHost));
		}

		[TestMethod]
		[ExpectedException(typeof (ArgumentException))]
		public void FailsWhenNamedGetterIsNotPresent()
		{
			ProviderDependencyAttribute attribute = new ProviderDependencyAttribute(typeof (EmptyHost), "TestProperty");
		}

		[TestMethod]
		public void TestParameterDependency()
		{
			Locator locator = new Locator();
			LifetimeContainer container = new LifetimeContainer();
			locator.Add(typeof (ILifetimeContainer), container);
			WCSFBuilder builder = new WCSFBuilder();

			TestProviderHost.Provider = new MockProvider();

			TestParameterDependency obj = builder.BuildUp<TestParameterDependency>(locator, "foo", null);

			Assert.IsNotNull(obj.Provider);
			Assert.AreSame(TestProviderHost.Provider, obj.Provider);
		}

		[TestMethod]
		public void TestPareameteDependencyWithNamedGetter()
		{
			Locator locator = new Locator();
			LifetimeContainer container = new LifetimeContainer();
			locator.Add(typeof (ILifetimeContainer), container);
			WCSFBuilder builder = new WCSFBuilder();

			TestProviderHost.AnotherProvider = new MockProvider();

			TestNamedParameterDependency obj = builder.BuildUp<TestNamedParameterDependency>(locator, "foo", null);

			Assert.IsNotNull(obj.Provider);
			Assert.AreSame(TestProviderHost.AnotherProvider, obj.Provider);
		}

		[TestMethod]
		public void TestSimpleDependency()
		{
			Locator locator = new Locator();
			LifetimeContainer container = new LifetimeContainer();
			locator.Add(typeof (ILifetimeContainer), container);
			WCSFBuilder builder = new WCSFBuilder();

			TestProviderHost.Provider = new MockProvider();

			TestSimpleDependency obj = builder.BuildUp<TestSimpleDependency>(locator, "foo", null);

			Assert.IsNotNull(obj.Provider);
			Assert.AreSame(TestProviderHost.Provider, obj.Provider);
		}

		[TestMethod]
		public void TestDependantProviderDependency()
		{
			Locator locator = new Locator();
			LifetimeContainer container = new LifetimeContainer();
			locator.Add(typeof (ILifetimeContainer), container);
			TestableRootCompositionContainer compositionContainer = new TestableRootCompositionContainer();
			locator.Add(new DependencyResolutionLocatorKey(typeof (CompositionContainer), null), compositionContainer);
			MockDependencyObject mockDependency =
				compositionContainer.Services.AddNew<MockDependencyObject, MockDependencyObject>();
			WCSFBuilder builder = new WCSFBuilder();

			TestDependentProviderHost.Provider = new MockDependentProvider();

			TestDependentParameterDependency obj = builder.BuildUp<TestDependentParameterDependency>(locator, "foo", null);

			Assert.IsNotNull(obj.Provider);
			Assert.AreSame(TestDependentProviderHost.Provider, obj.Provider);
			Assert.IsNotNull(TestDependentProviderHost.Provider.DependencyObject);
			Assert.AreSame(mockDependency, TestDependentProviderHost.Provider.DependencyObject);
		}
	}

	public class MockProvider : ProviderBase
	{
	}

	internal class EmptyHost
	{
	}

	public class TestProviderHost
	{
		private static MockProvider _anotherProvider;
		private static MockProvider _provider;

		public static MockProvider Provider
		{
			get { return _provider; }
			set { _provider = value; }
		}

		public static MockProvider AnotherProvider
		{
			get { return _anotherProvider; }
			set { _anotherProvider = value; }
		}
	}

	internal class TestParameterDependency
	{
		public MockProvider Provider = null;

		public TestParameterDependency([ProviderDependency(typeof (TestProviderHost))] MockProvider provider)
		{
			Provider = provider;
		}
	}

	public class TestNamedParameterDependency
	{
		public MockProvider Provider;

		public TestNamedParameterDependency(
			[ProviderDependency(typeof (TestProviderHost), "AnotherProvider")] MockProvider provider)
		{
			Provider = provider;
		}
	}

	internal class TestSimpleDependency
	{
		private MockProvider _provider;

		[ProviderDependency(typeof (TestProviderHost))]
		public MockProvider Provider
		{
			get { return _provider; }
			set { _provider = value; }
		}
	}

	internal class MockDependencyObject
	{
	}

	internal class MockDependentProvider : ProviderBase
	{
		public object DependencyObject;

		[ServiceDependency]
		public MockDependencyObject ServiceDependency
		{
			set { DependencyObject = value; }
		}
	}

	internal class TestDependentProviderHost
	{
		private static MockDependentProvider _provider;

		public static MockDependentProvider Provider
		{
			get { return _provider; }
			set { _provider = value; }
		}
	}

	internal class TestDependentParameterDependency
	{
		public MockDependentProvider Provider = null;

		public TestDependentParameterDependency(
			[ProviderDependency(typeof (TestDependentProviderHost))] MockDependentProvider provider)
		{
			Provider = provider;
		}
	}
}
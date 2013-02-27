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
using Microsoft.Practices.CompositeWeb.ObjectBuilder;
using Microsoft.Practices.CompositeWeb.Tests.Mocks;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests
{
	[TestClass]
	public class ServiceDependencyAttributeFixture
	{
		[TestMethod]
		public void CanCreateSpecifyingTheServiceType()
		{
			ServiceDependencyAttribute attribute = new ServiceDependencyAttribute();
			attribute.Type = typeof (TestService);

			Assert.AreEqual(typeof (TestService), attribute.Type);
		}

		[TestMethod]
		public void TestBuildWithParameterDependency()
		{
			Locator locator = new Locator();
			LifetimeContainer container = new LifetimeContainer();
			locator.Add(typeof (ILifetimeContainer), container);
			WCSFBuilder builder = new WCSFBuilder();
			builder.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));

			TestableRootCompositionContainer compositionContainer = new TestableRootCompositionContainer(builder);
			locator.Add(new DependencyResolutionLocatorKey(typeof (CompositionContainer), null), compositionContainer);
			ITestService service = compositionContainer.Services.AddNew<TestService, ITestService>();

			TestParameterDependency builtObject =
				builder.BuildUp(locator, typeof (TestParameterDependency), Guid.NewGuid().ToString(), null) as
				TestParameterDependency;

			Assert.IsNotNull(builtObject);
			Assert.IsNotNull(builtObject.TestService);
			Assert.AreEqual(service, builtObject.TestService);
		}

		[TestMethod]
		public void TestBuildWithSimpleDependency()
		{
			Locator locator = new Locator();
			WCSFBuilder builder = new WCSFBuilder();
			builder.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));

			TestableRootCompositionContainer compositionContainer = new TestableRootCompositionContainer(builder);
			locator.Add(new DependencyResolutionLocatorKey(typeof (CompositionContainer), null), compositionContainer);
			ITestService service = compositionContainer.Services.AddNew<TestService, ITestService>();

			TestSimpleDependency builtObject =
				builder.BuildUp(locator, typeof (TestSimpleDependency), Guid.NewGuid().ToString(), null) as TestSimpleDependency;

			Assert.IsNotNull(builtObject);
			Assert.IsNotNull(builtObject.TestService);
			Assert.AreEqual(service, builtObject.TestService);
		}

		[TestMethod]
		public void CanSetRequiredValue()
		{
			ServiceDependencyAttribute attribute = new ServiceDependencyAttribute();

			Assert.IsTrue(attribute.Required);

			attribute.Required = false;

			Assert.IsFalse(attribute.Required);
		}

		#region Nested type: ITestService

		private interface ITestService
		{
		}

		#endregion

		#region Nested type: TestParameterDependency

		private class TestParameterDependency
		{
			private ITestService testService;

			public TestParameterDependency([ServiceDependency] ITestService testService)
			{
				this.testService = testService;
			}

			public ITestService TestService
			{
				get { return testService; }
				set { testService = value; }
			}
		}

		#endregion

		#region Nested type: TestService

		private class TestService : ITestService
		{
		}

		#endregion

		#region Nested type: TestSimpleDependency

		private class TestSimpleDependency
		{
			private ITestService testService;

			[ServiceDependency]
			public ITestService TestService
			{
				get { return testService; }
				set { testService = value; }
			}
		}

		#endregion
	}
}
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
using Microsoft.Practices.CompositeWeb.BuilderStrategies;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Tests.Mocks;
using Microsoft.Practices.CompositeWeb.Web;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests.BuilderStrategies
{
	/// <summary>
	/// Summary description for SessionStateBindingStrategyFixture
	/// </summary>
	[TestClass]
	public class SessionStateBindingStrategyFixture
	{
		[TestMethod]
		public void TestValuesAreCreated()
		{
			SessionStateBindingStrategy strategy = new SessionStateBindingStrategy();
			MockBuilderContext builderContext = new MockBuilderContext(strategy);
			MockSessionStateLocatorService sessionLocator = new MockSessionStateLocatorService();
			builderContext.Locator.Add(new DependencyResolutionLocatorKey(typeof (ISessionStateLocatorService), null),
			                           sessionLocator);

			SampleClass sample = new SampleClass();
			sample.String0 = "test";
			strategy.BuildUp(builderContext, typeof (SampleClass), sample, null);

			Assert.AreEqual("test", sample.String0);
			Assert.IsNotNull(sample.String1);
			Assert.IsNotNull(sample.String2);
		}

		[TestMethod]
		public void TestKeyNameAssigned()
		{
			SessionStateBindingStrategy strategy = new SessionStateBindingStrategy();
			MockBuilderContext builderContext = new MockBuilderContext(strategy);
			MockSessionStateLocatorService sessionLocator = new MockSessionStateLocatorService();
			builderContext.Locator.Add(new DependencyResolutionLocatorKey(typeof (ISessionStateLocatorService), null),
			                           sessionLocator);
			SampleClass sample = new SampleClass();
			strategy.BuildUp(builderContext, typeof (SampleClass), sample, null);

			Assert.IsNotNull(sample.String1);
			Assert.IsNotNull(sample.String2);

			Assert.AreEqual("SampleClass;String1", sample.String1.KeyName);
			Assert.AreEqual("key", sample.String2.KeyName);
		}

		[TestMethod]
		public void ValuesArePulledFromSession()
		{
			SessionStateBindingStrategy strategy = new SessionStateBindingStrategy();
			MockBuilderContext builderContext = new MockBuilderContext(strategy);
			MockSessionStateLocatorService sessionLocator = new MockSessionStateLocatorService();
			sessionLocator.SessionState["key"] = "value";
			builderContext.Locator.Add(new DependencyResolutionLocatorKey(typeof (ISessionStateLocatorService), null),
			                           sessionLocator);

			SampleClass sample = new SampleClass();
			strategy.BuildUp(builderContext, typeof (SampleClass), sample, null);

			Assert.AreEqual("value", sample.String2.Value);
		}

		#region Nested type: SampleClass

		public class SampleClass
		{
			public String String0;

			public StateValue<string> String1;

			[SessionStateKey("key")] public StateValue<string> String2;
		}

		#endregion
	}
}
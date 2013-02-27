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
using Microsoft.Practices.CompositeWeb.ObjectBuilder;
using Microsoft.Practices.CompositeWeb.Tests.Mocks;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests
{
	/// <summary>
	/// This fixture tests the expected order of operations in the WebClientApplication class
	/// </summary>
	[TestClass]
	public class WebClientApplicationLifecycleFixture
	{
		[TestMethod]
		public void TestApplicationLifecyleIsExecuted()
		{
			TestableWebClientApplication application = new TestableWebClientApplication();

			application.TestApplicationStart();


			Assert.IsTrue(application.CalledMethods.IndexOf("CreateBuilder") <
			              application.CalledMethods.IndexOf("AddBuilderStrategies"));

			Assert.IsTrue(application.CalledMethods.IndexOf("AddBuilderStrategies") <
			              application.CalledMethods.IndexOf("CreateRootContainer"));

			Assert.IsTrue(application.CalledMethods.IndexOf("CreateRootContainer") <
			              application.CalledMethods.IndexOf("AddRequiredServices"));

			Assert.IsTrue(application.CalledMethods.IndexOf("AddRequiredServices") <
			              application.CalledMethods.IndexOf("LoadModules"));

			Assert.IsTrue(application.CalledMethods.IndexOf("LoadModules") <
			              application.CalledMethods.IndexOf("Start"));
		}

		#region Nested type: TestableWebClientApplication

		private class TestableWebClientApplication : WebClientApplication
		{
			public List<string> CalledMethods = new List<string>();

			public override CompositionContainer RootContainer
			{
				get { return new TestableRootCompositionContainer(); }
			}

			public void TestApplicationStart()
			{
				base.Application_Start(this, EventArgs.Empty);
			}

			protected override void CreateApplicationBuilder()
			{
				CalledMethods.Add("CreateBuilder");
			}

			protected override void AddBuilderStrategies(IBuilder<WCSFBuilderStage> builder)
			{
				CalledMethods.Add("AddBuilderStrategies");
			}

			protected override void AddRequiredServices()
			{
				CalledMethods.Add("AddRequiredServices");
			}

			protected override void LoadModules()
			{
				CalledMethods.Add("LoadModules");
			}

			protected override void Start()
			{
				CalledMethods.Add("Start");
			}

			protected override void CreateRootContainer()
			{
				CalledMethods.Add("CreateRootContainer");
			}
		}

		#endregion
	}
}
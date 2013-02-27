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

using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderManagement.Customers.Tests;
using OrderManagement.Customers.Tests.Mocks;
using OrderManagement.SharedUserControls.Services;
using OrderManagement.SharedUserControls;
using OrdersRepository.Interfaces.Services;

namespace SharedUserControls.Tests
{
	/// <summary>
	/// Summary description for SharedUserControlsModuleInitializerFixture
	/// </summary>
	[TestClass]
	public class SharedUserControlsModuleInitializerFixture
	{
		public SharedUserControlsModuleInitializerFixture()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		[TestMethod]
		public void LoadCallsAddGlobalServices()
		{
			TestableRootCompositionContainer parentContainer = new TestableRootCompositionContainer();
			TestableRootCompositionContainer container = parentContainer.Containers.AddNew<TestableRootCompositionContainer>("TEST");

			TestableModuleInitializer moduleInitializer = new TestableModuleInitializer();
			ISiteMapBuilderService siteMapBuilder = new MockSiteMapBuilderService();
			IHttpContextLocatorService contextLocator = new MockHttpContextLocatorService();
			container.Services.Add<IHttpContextLocatorService>(contextLocator);
			container.Services.Add<ISiteMapBuilderService>(siteMapBuilder);


			moduleInitializer.Load(container);

			Assert.IsTrue(moduleInitializer.AddGlobalServicesWasCalled);
		}

		[TestMethod]
		public void LoadCallsAddModuleServices()
		{
			TestableRootCompositionContainer parentContainer = new TestableRootCompositionContainer();
			TestableRootCompositionContainer container = parentContainer.Containers.AddNew<TestableRootCompositionContainer>("TEST");
			TestableModuleInitializer moduleInitializer = new TestableModuleInitializer();

			IHttpContextLocatorService contextLocator = new MockHttpContextLocatorService();
			container.Services.Add<IHttpContextLocatorService>(contextLocator);

			moduleInitializer.Load(container);

			Assert.IsTrue(moduleInitializer.AddModuleServicesWasCalled);
		}

		[TestMethod]
		public void RegisterGlobalServicesRegistersIFindCustomerService()
		{
			TestableModuleInitializer module = new TestableModuleInitializer();
			MockServiceCollection serviceCollection = new MockServiceCollection();

			module.TestAddGlobalServices(serviceCollection);

			Assert.IsTrue(serviceCollection.RegistedServices.ContainsKey(typeof(IFindCustomerService)));
		}

		/*
		// This service is registered via config.
		// If you wanted to register it via code, you should uncomment this unit test.
		 * 
		[TestMethod]
		public void RegisterModuleServicesRegistersICustomerService()
		{
			TestableModuleInitializer module = new TestableModuleInitializer();
			MockServiceCollection serviceCollection = new MockServiceCollection();

			module.TestAddModuleServices(serviceCollection);

			Assert.IsTrue(serviceCollection.RegistedServices.ContainsKey(typeof(ICustomerService)));
		}
		*/

		class TestableModuleInitializer : SharedUserControlsModuleInitializer
		{
			public Boolean AddModuleServicesWasCalled = false;
			public Boolean AddGlobalServicesWasCalled = false;
			public Boolean RegisterSiteMapWasCalled = false;

			public TestableModuleInitializer()
			{

			}

			protected override void AddModuleServices(IServiceCollection moduleServices)
			{
				AddModuleServicesWasCalled = true;
			}

			protected override void AddGlobalServices(IServiceCollection globalServices)
			{
				AddGlobalServicesWasCalled = true;
			}

			public void TestAddModuleServices(IServiceCollection moduleServices)
			{
				base.AddModuleServices(moduleServices);
			}

			public void TestAddGlobalServices(IServiceCollection globalServices)
			{
				base.AddGlobalServices(globalServices);
			}
		}


	}
}

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
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Services;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.ObjectBuilder;
using OrderManagement.Orders.Tests.Mocks;
using System.Configuration;
using OrdersRepository.Interfaces.Services;
using OrdersRepository.BusinessEntities;
using Orders.PresentationEntities;
using OrderManagement.Orders.Converters;

namespace OrderManagement.Orders.Tests
{
    /// <summary>
    /// Summary description for OrdersModuleInitializerFixture
    /// </summary>
    [TestClass]
    public class OrdersModuleInitializerFixture
    {
        [TestMethod]
        public void OrdersModuleInitializerIsIModuleInitializer()
        {
            Assert.IsTrue(new OrdersModuleInitializer() is ModuleInitializer);
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

            ISiteMapBuilderService siteMapBuilder = new MockSiteMapBuilderService();
            IHttpContextLocatorService contextLocator = new MockHttpContextLocatorService();
            container.Services.Add<IHttpContextLocatorService>(contextLocator);
            container.Services.Add<ISiteMapBuilderService>(siteMapBuilder);

            moduleInitializer.Load(container);


            Assert.IsTrue(moduleInitializer.AddModuleServicesWasCalled);
        }

        [TestMethod]
        public void LoadCallsRegisterSiteMapInformation()
        {
            TestableRootCompositionContainer parentContainer = new TestableRootCompositionContainer();
            TestableRootCompositionContainer container = parentContainer.Containers.AddNew<TestableRootCompositionContainer>("TEST");

            TestableModuleInitializer moduleInitializer = new TestableModuleInitializer();

            ISiteMapBuilderService siteMapBuilder = new MockSiteMapBuilderService();
            IHttpContextLocatorService contextLocator = new MockHttpContextLocatorService();
            container.Services.Add<IHttpContextLocatorService>(contextLocator);
            container.Services.Add<ISiteMapBuilderService>(siteMapBuilder);


            moduleInitializer.Load(container);

            Assert.IsTrue(moduleInitializer.RegisterSiteMapWasCalled);
        }

        [TestMethod]
        public void OrdersGetsRegisteredOnSiteMap()
        {
            TestableModuleInitializer moduleInitializer = new TestableModuleInitializer();
            SiteMapBuilderService siteMapBuilder = new SiteMapBuilderService();

            moduleInitializer.TestRegisterSiteMapInformation(siteMapBuilder);

            SiteMapNodeInfo node = siteMapBuilder.GetChildren(siteMapBuilder.RootNode.Key)[0];
            Assert.AreEqual("Orders", node.Key);
        }

        [TestMethod]
        public void OrdersActionsGetsRegisteredOnSiteMap()
        {
            TestableModuleInitializer moduleInitializer = new TestableModuleInitializer();
            SiteMapBuilderService siteMapBuilder = new SiteMapBuilderService();

            moduleInitializer.TestRegisterSiteMapInformation(siteMapBuilder);

            SiteMapNodeInfo nodeCreateNewOrder = siteMapBuilder.GetChildren("Orders")[0];
            Assert.AreEqual("CreateNewOrder", nodeCreateNewOrder.Key);

            SiteMapNodeInfo nodeMySavedDrafts = siteMapBuilder.GetChildren("Orders")[1];
            Assert.AreEqual("MySavedDrafts", nodeMySavedDrafts.Key);

            SiteMapNodeInfo nodeMyApprovals = siteMapBuilder.GetChildren("Orders")[2];
            Assert.AreEqual("MyApprovals", nodeMyApprovals.Key);

            SiteMapNodeInfo nodeSearchOrder = siteMapBuilder.GetChildren("Orders")[3];
            Assert.AreEqual("SearchOrder", nodeSearchOrder.Key);
        }

        [TestMethod]
        public void ConfigureShouldRegisterAuthorizationRules()
        {
            MockServiceCollection collection = new MockServiceCollection();
            collection.Add(typeof(IAuthorizationRulesService), new MockAuthorizationRulesService());

            TestableModuleInitializer module = new TestableModuleInitializer();
            module.Configure(collection, ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None));

            MockAuthorizationRulesService authRulesServices =
                (MockAuthorizationRulesService)collection.Get<IAuthorizationRulesService>();

            Assert.AreEqual(1, authRulesServices.RegisteredAuthorizationRules.Count);
            Assert.IsTrue(authRulesServices.RegisteredAuthorizationRules.ContainsKey("Default.aspx"));
            Assert.AreEqual("MockRule01", authRulesServices.RegisteredAuthorizationRules["Default.aspx"]);
        }

        [TestMethod]
        public void ConfigureShouldNotThrowExceptionIfAuthorizationServicesIsNotLoaded()
        {
            MockServiceCollection collection = new MockServiceCollection();

            TestableModuleInitializer module = new TestableModuleInitializer();
            module.Configure(collection, ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None));

            MockAuthorizationRulesService authRulesServices =
                (MockAuthorizationRulesService)collection.Get<IAuthorizationRulesService>();

            Assert.IsNull(authRulesServices);
        }

        [TestMethod]
        public void RegisterModuleServicesRegistersIEmployeeService()
        {
            TestableModuleInitializer module = new TestableModuleInitializer();
            MockServiceCollection serviceCollection = new MockServiceCollection();

            module.TestAddModuleServices(serviceCollection);

            Assert.IsTrue(serviceCollection.RegistedServices.ContainsKey(typeof(IEmployeeService)));
        }

        [TestMethod]
        public void RegisterModuleServicesRegistersIOrdersService()
        {
            TestableModuleInitializer module = new TestableModuleInitializer();
            MockServiceCollection serviceCollection = new MockServiceCollection();

            module.TestAddModuleServices(serviceCollection);

            Assert.IsTrue(serviceCollection.RegistedServices.ContainsKey(typeof(IOrdersService)));
        }

        // This test removed intentionally.  The service is added via config.
        // If the code is added back, this test should be uncommented
        /*
        [TestMethod]
        public void RegisterModuleServicesRegistersIOrderEntryFlowService()
        {
            TestableModuleInitializer module = new TestableModuleInitializer();
            MockServiceCollection serviceCollection = new MockServiceCollection();

            module.TestAddModuleServices(serviceCollection);

            Assert.IsTrue(serviceCollection.RegistedServices.ContainsKey(typeof(IOrderEntryFlowService)));
        }
        */

        [TestMethod]
        public void RegisterModuleServicesRegistersIBusinessPresentationConverterForEmployees()
        {
            TestableModuleInitializer module = new TestableModuleInitializer();
            MockServiceCollection serviceCollection = new MockServiceCollection();

            module.TestAddModuleServices(serviceCollection);

            Assert.IsTrue(serviceCollection.RegistedServices.ContainsKey(typeof(IBusinessPresentationConverter<Employee, EmployeeDisplay>)));
        }

        [TestMethod]
        public void RegisterModuleServicesRegistersIBusinessPresentationConverterForOrders()
        {
            TestableModuleInitializer module = new TestableModuleInitializer();
            MockServiceCollection serviceCollection = new MockServiceCollection();

            module.TestAddModuleServices(serviceCollection);

            Assert.IsTrue(serviceCollection.RegistedServices.ContainsKey(typeof(IBusinessPresentationConverter<Order, OrderInfo>)));
        }

        [TestMethod]
        public void RegisterModuleServicesRegistersIProductService()
        {
            TestableModuleInitializer module = new TestableModuleInitializer();
            MockServiceCollection serviceCollection = new MockServiceCollection();

            module.TestAddModuleServices(serviceCollection);

            Assert.IsTrue(serviceCollection.RegistedServices.ContainsKey(typeof(IProductService)));
        }

        // This test removed intentionally.  The service is added vian config.
        // If the code is added back, this test should be uncommented
        /*
        [TestMethod]
        public void RegisterModuleServicesRegistersIPageFlowOrderEntryFlowService()
        {
            TestableModuleInitializer module = new TestableModuleInitializer();
            MockServiceCollection serviceCollection = new MockServiceCollection();

            module.TestAddModuleServices(serviceCollection);

            Assert.IsTrue(serviceCollection.RegistedServices.ContainsKey(typeof(IOrderEntryFlowService)));
        }
        */

        [TestMethod]
        public void RegisterGlobalServicesRegistersNoServices()
        {
            TestableModuleInitializer module = new TestableModuleInitializer();
            MockServiceCollection serviceCollection = new MockServiceCollection();

            module.TestAddGlobalServices(serviceCollection);

            Assert.AreEqual(0, serviceCollection.RegistedServices.Count);
        }

        [TestMethod]
        public void LoadRegistersTypeMappingForController()
        {
            TestableRootCompositionContainer parentContainer = new TestableRootCompositionContainer();
            TestableRootCompositionContainer container = parentContainer.Containers.AddNew<TestableRootCompositionContainer>("TEST");
            TestableModuleInitializer moduleInitializer = new TestableModuleInitializer();
            container.Services.Add<IHttpContextLocatorService>(new MockHttpContextLocatorService());
            container.Services.Add<ISiteMapBuilderService>(new MockSiteMapBuilderService());

            moduleInitializer.Load(container);
            
            Assert.AreEqual(typeof(OrdersController), container.GetMappedType(typeof(IOrdersController)));
        }

        internal class TestableRootCompositionContainer : CompositionContainer
        {
            private WCSFBuilder _builder;

            public TestableRootCompositionContainer()
            {
                _builder = new WCSFBuilder();
                //			_builder.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));

                InitializeRootContainer(_builder);
            }

            public new WCSFBuilder Builder
            {
                get { return _builder; }
            }

        }

        internal class MockSiteMapBuilderService : ISiteMapBuilderService
        {
            #region ISiteMapBuilderService Members

            public SiteMapNodeInfo RootNode
            {
                get { throw new Exception("The method or operation is not implemented."); }
            }

            public void AddNode(SiteMapNodeInfo node)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public void AddNode(SiteMapNodeInfo node, int preferredDisplayOrder)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public void AddNode(SiteMapNodeInfo node, SiteMapNodeInfo parent)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public void AddNode(SiteMapNodeInfo node, SiteMapNodeInfo parent, int preferredDisplayOrder)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public void AddNode(SiteMapNodeInfo node, string authorizationRule)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public void AddNode(SiteMapNodeInfo node, string authorizationRule, int preferredDisplayOrder)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public void AddNode(SiteMapNodeInfo node, SiteMapNodeInfo parent, string authorizationRule)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public void AddNode(SiteMapNodeInfo node, SiteMapNodeInfo parent, string authorizationRule, int preferredDisplayOrder)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public System.Collections.ObjectModel.ReadOnlyCollection<SiteMapNodeInfo> GetChildren(string nodeKey)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public string GetAuthorizationRule(string nodeKey)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            #endregion
        }

    }

    class TestableModuleInitializer : OrdersModuleInitializer
    {
       public Boolean AddModuleServicesWasCalled = false;
            public Boolean AddGlobalServicesWasCalled = false;
            public Boolean RegisterSiteMapWasCalled = false;

            public TestableModuleInitializer()
            {
                
            }

            protected override void RegisterSiteMapInformation(ISiteMapBuilderService siteMapBuilder)
            {
                RegisterSiteMapWasCalled = true;
            }

            protected override void AddModuleServices(IServiceCollection moduleServices)
            {
                AddModuleServicesWasCalled = true;
            }

            protected override void AddGlobalServices(IServiceCollection globalServices)
            {
                AddGlobalServicesWasCalled = true;
            }

            public void TestRegisterSiteMapInformation(ISiteMapBuilderService siteMapBuilder)
            {
                base.RegisterSiteMapInformation(siteMapBuilder);
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

        internal class MockAuthorizationRulesService : IAuthorizationRulesService
        {
            public Dictionary<string, string> RegisteredAuthorizationRules = new Dictionary<string, string>();

            #region IAuthorizationRulesService Members

            public void RegisterAuthorizationRule(string urlPath, string rule)
            {
                RegisteredAuthorizationRules[urlPath] = rule;
            }

            public string[] GetAuthorizationRules(string urlPath)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            #endregion
        }
}

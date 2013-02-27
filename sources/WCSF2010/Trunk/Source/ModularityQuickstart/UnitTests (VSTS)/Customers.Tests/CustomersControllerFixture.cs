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
using System.Collections.Generic;
using Microsoft.Practices.CompositeWeb.Web;
using ModularityQuickstart.Customers.BusinessEntities;
using ModularityQuickstart.Customers.Constants;
using ModularityQuickstart.Customers.Tests.Mocks;
using ModularityQuickstart.Navigation.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ModularityQuickstart.Customers.Tests
{
    /// <summary>
    /// Summary description for CustomersControllerFixture
    /// </summary>
    [TestClass]
    public class CustomersControllerFixture
    {
        [TestMethod]
        public void ApproveCurrentCustomerApprovesCustomerAndUpdatesDataStoreAndRedirectsToSummary()
        {
            MockNavigationService navigationService = new MockNavigationService();
            List<Customer> customers = new List<Customer>();
            CustomersController controller = GetCustomersControllerInitialized(navigationService, customers);
            Customer customerInDataStore = new Customer(1000, "Enrique", "Gil", false);
            customers.Add(customerInDataStore);
            Customer customerToApprove = new Customer(1000, "Enrique", "Gil", false);
            controller.CurrentCustomer = customerToApprove;

            controller.ApproveCurrentCustomer();

            Assert.IsTrue(customerToApprove.Approved);
            Assert.IsTrue(customerInDataStore.Approved);
            Assert.IsTrue(navigationService.NavigateCalled);
            Assert.AreEqual(ViewNames.SummaryView, navigationService.View);
        }

        [TestMethod]
        public void ApproveAnotherCustomerClearsCurrentCustomerAndRedirectsToApproveCustomerView()
        {
            MockNavigationService navigationService = new MockNavigationService();
            CustomersController controller = GetCustomersControllerInitialized(navigationService);
            controller.CurrentCustomer = new Customer();
            
            controller.ApproveAnotherCustomer();
            
            Assert.IsNull(controller.CurrentCustomer);
            Assert.IsTrue(navigationService.NavigateCalled);
            Assert.AreEqual(ViewNames.ApproveCustomerView, navigationService.View);
        }

        [TestMethod]
        public void CurrentCustomerReturnsCurrentCustomer()
        {
            CustomersController controller = GetCustomersControllerInitialized();
            Customer currentCustomer = new Customer();
            controller.CurrentCustomer = currentCustomer;

            Customer result = controller.CurrentCustomer;
            
            Assert.AreSame(currentCustomer, result);
        }

        [TestMethod]
        public void CurrentCustomerReturnsNextCustomerIfCurrentCustomerIsNull()
        {
            List<Customer> customers = new List<Customer>();
            CustomersController controller = GetCustomersControllerInitialized(customers);
            Customer nonApprovedCustomer = new Customer(1000, "Enrique", "Gil", false);
            customers.Add(nonApprovedCustomer);

            Customer result = controller.CurrentCustomer;

            Assert.AreSame(nonApprovedCustomer, result);
        }

        private CustomersController GetCustomersControllerInitialized(INavigationService navigationService, List<Customer> customers)
        {
            CustomersController controller = new CustomersController(navigationService);
            StateValue<Customer>  currentCustomerState = new StateValue<Customer>();
            StateValue<List<Customer>> customersState = new StateValue<List<Customer>>(customers);

            controller.InitializeState(currentCustomerState, customersState);

            return controller;
        }

        private CustomersController GetCustomersControllerInitialized(INavigationService navigationService)
        {
            return GetCustomersControllerInitialized(navigationService, new List<Customer>());
        }

        private CustomersController GetCustomersControllerInitialized(List<Customer> customers)
        { 
            return GetCustomersControllerInitialized(new MockNavigationService(),customers);
        }

        private CustomersController GetCustomersControllerInitialized()
        {
            return GetCustomersControllerInitialized(new MockNavigationService());
        }
    }
}

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
// The following code was generated by Microsoft Visual Studio 2005.
// The test owner should check each test for validity.
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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Collections.Generic;
using MVPWithCWABQuickStart.Contacts.Tests.Mocks;
using MVPWithCWABQuickStart.Contacts.BusinessEntities;

namespace MVPWithCWABQuickStart.Contacts.Tests
{
    /// <summary>
    /// Summary description for ContactsControllerFixture
    /// </summary>
    [TestClass]
    public class ContactsControllerFixture
    {
        MockCustomersDataSource dataSource;
        ContactsController controller;

        [TestInitialize]
        public void MyTestInitialize()
        {
            dataSource = new MockCustomersDataSource();
            controller = new ContactsController(dataSource);
        }

        [TestMethod]
        public void GetCustomersShouldReturnDataSourceCustomers()
        {
            IList<Customer> dataSourceCustomers=new List<Customer>();
            dataSource.GetCustomersReturnValue = dataSourceCustomers;
            dataSource.GetCustomersCalled = false;

            IList<Customer> retrievedCustomers = controller.GetCustomers();

            Assert.IsTrue(dataSource.GetCustomersCalled);
            Assert.AreSame(dataSourceCustomers, retrievedCustomers);
        }

        [TestMethod]
        public void UpdateCurrentCustomerUpdatesDataSource()
        {
            Customer updatedCustomer = new Customer();
            dataSource.UpdatedCustomerValue = null;
            dataSource.UpdateCustomerCalled = false;

            controller.UpdateCurrentCustomer(updatedCustomer);

            Assert.IsTrue(dataSource.UpdateCustomerCalled);
            Assert.AreSame(updatedCustomer, dataSource.UpdatedCustomerValue);
        }

        [TestMethod]
        public void SetSelectedIndexRaisesCurrentCustomerChangedEvent()
        {
            bool onCurrentCustomerChangedCalled = false;
            controller.CurrentCustomerChanged += new EventHandler(delegate { onCurrentCustomerChangedCalled = true; });
            controller.SetSelectedContactIndex(1);

            Assert.IsTrue(onCurrentCustomerChangedCalled);
        }

        [TestMethod]
        public void SetSelectedIndexChangedCurrentCustomer()
        {
            dataSource.GetCustomersReturnValue = new List<Customer>();
            dataSource.GetCustomersReturnValue.Add(new Customer("0", "Customer 0", "Address 0", "City 0", "State 0", "Zip 0"));
            dataSource.GetCustomersReturnValue.Add(new Customer("1", "Customer 1", "Address 1", "City 1", "State 1", "Zip 1"));
            dataSource.GetCustomersReturnValue.Add(new Customer("2", "Customer 2", "Address 2", "City 2", "State 2", "Zip 2"));
            Customer selectedCustomer3 = new Customer("3", "Customer 3", "Address 3", "City 3", "State 3", "Zip 3");
            dataSource.GetCustomersReturnValue.Add(selectedCustomer3);
            Customer selectedCustomer4 = new Customer("4", "Customer 4", "Address 4", "City 4", "State 4", "Zip 4");
            dataSource.GetCustomersReturnValue.Add(selectedCustomer4);

            controller.SetSelectedContactIndex(3);

            Assert.AreSame(selectedCustomer3, controller.CurrentCustomer);

            controller.SetSelectedContactIndex(4);

            Assert.AreSame(selectedCustomer4, controller.CurrentCustomer);
        }

        [TestMethod]
        public void RestoreSelectedIndexRaisesCurrentCustomerChangedEvent()
        {
            bool onCurrentCustomerChangedCalled = false;
            controller.CurrentCustomerChanged += new EventHandler(delegate { onCurrentCustomerChangedCalled = true; });
            controller.RestoreFromPersistedState(1);

            Assert.IsTrue(onCurrentCustomerChangedCalled);
        }
    }
}

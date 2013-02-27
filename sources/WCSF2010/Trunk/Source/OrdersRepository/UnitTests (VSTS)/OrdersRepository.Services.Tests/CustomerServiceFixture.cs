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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrdersRepository.BusinessEntities;

namespace OrdersRepository.Services.Tests
{
    /// <summary>
    /// Summary description for CustomerServiceFixture
    /// </summary>
    [TestClass]
    public class CustomerServiceFixture
    {
        [TestMethod]
        public void CanCreateInstanceWithoutPassingRepositoryToConstructor()
        {
            CustomerService service = new CustomerService();
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void GetCustomerByNameMatchesExactName()
        {
            OrdersManagementDataSet ds = new OrdersManagementDataSet();
			AddCustomer(ds, "21", "The Store");
			AddCustomer(ds, "22", "These Cheeses");
            AddCustomer(ds, "23", "These Cheeses International");
            CustomerService customerService = new CustomerService(ds);

            Customer customer = customerService.GetCustomerByName("These Cheeses");

            Assert.IsNotNull(customer);
            Assert.AreEqual("22", customer.CustomerId);
        }

        [TestMethod]
        public void GetCustomerByNameReturnsNullWithNoMatch()
        {
            OrdersManagementDataSet ds = new OrdersManagementDataSet();
            CustomerService customerService = new CustomerService(ds);

            Customer customer = customerService.GetCustomerByName("No Match");

            Assert.IsNull(customer);
        }

        [TestMethod]
        public void GetCustomersByNamePrefixMatchesStartingName()
        {
            OrdersManagementDataSet ds = new OrdersManagementDataSet();
			AddCustomer(ds, "21", "The Store");
			AddCustomer(ds, "22", "These Cheeses");
			AddCustomer(ds, "23", "These Cheeses International");
            CustomerService customerService = new CustomerService(ds);

            ICollection<Customer> customers = customerService.GetCustomersByNamePrefix("THES");

            Assert.IsNotNull(customers);
            Assert.AreEqual(2, customers.Count);
            List<Customer> searchableList = new List<Customer>(customers);
            Assert.IsNotNull(searchableList.Exists(delegate(Customer customer) { return customer.CustomerId == "22"; }));
            Assert.IsNotNull(searchableList.Exists(delegate(Customer customer) { return customer.CustomerId == "23"; }));
        }

        [TestMethod]
        public void GetCustomersByNamePrefixReturnsEmptyListWhenNoMatchesFound()
        {
            OrdersManagementDataSet ds = new OrdersManagementDataSet();
            CustomerService customerService = new CustomerService(ds);

            ICollection<Customer> customers = customerService.GetCustomersByNamePrefix("No Match");

            Assert.IsNotNull(customers);
            Assert.AreEqual(0, customers.Count);
        }

        [TestMethod]
        public void GetCustomersByIDReturnsCustomer()
        {
            OrdersManagementDataSet ds = new OrdersManagementDataSet();
			AddCustomer(ds, "21", "The Store");
			AddCustomer(ds, "22", "These Cheeses");
            CustomerService customerService = new CustomerService(ds);

            Customer customer = customerService.GetCustomerById("22");

            Assert.IsNotNull(customer);
            Assert.AreEqual("These Cheeses", customer.CompanyName);
        }

        [TestMethod]
        public void GetCustomerByIdReturnsNullWithNoMatch()
        {
            OrdersManagementDataSet ds = new OrdersManagementDataSet();
            CustomerService customerService = new CustomerService(ds);

            Customer customer = customerService.GetCustomerById("12345");

            Assert.IsNull(customer);
        }

		[TestMethod]
		public void CanReadAllCustomerProperties()
		{
			OrdersManagementDataSet ds = new OrdersManagementDataSet();
			OrdersManagementDataSet.CustomersRow row = ds.Customers.NewCustomersRow();
			row.CustomerId = "42";
			row.Address = "My Address";
			row.City = "My City";
			row.CompanyName = "My Company";
			row.PostalCode = "12345";
			row.Region = "My Region";
			ds.Customers.AddCustomersRow(row);
			ds.Customers.AcceptChanges();
			CustomerService customerService = new CustomerService(ds);

			Customer customer = customerService.GetCustomerById("42");

			Assert.IsNotNull(customer);
			Assert.AreEqual("My Address", customer.Address);
			Assert.AreEqual("My City", customer.City);
			Assert.AreEqual("My Company", customer.CompanyName);
			Assert.AreEqual("12345", customer.PostalCode);
			Assert.AreEqual("My Region", customer.Region);
		}

		[TestMethod]
		public void GetCustomersByNamePrefixWithQuotesSearchesCorrectlyAndPreventsInjection()
		{
			OrdersManagementDataSet ds = new OrdersManagementDataSet();
			AddCustomer(ds, "21", "The Store");
			AddCustomer(ds, "22", "The'se Cheeses");
			AddCustomer(ds, "23", "The'se Cheeses International");
			CustomerService customerService = new CustomerService(ds);

			ICollection<Customer> customers = customerService.GetCustomersByNamePrefix("THE'S");

			Assert.IsNotNull(customers);
			Assert.AreEqual(2, customers.Count);
			List<Customer> searchableList = new List<Customer>(customers);
			Assert.IsNotNull(searchableList.Exists(delegate(Customer customer) { return customer.CustomerId == "22"; }));
			Assert.IsNotNull(searchableList.Exists(delegate(Customer customer) { return customer.CustomerId == "23"; }));
		}

		[TestMethod]
		public void GetCustomerByNameWithQuotesSearchesCorrectlyAndPreventsInjection()
		{
			OrdersManagementDataSet ds = new OrdersManagementDataSet();
			AddCustomer(ds, "21", "The Store");
			AddCustomer(ds, "22", "These' Cheeses");
			AddCustomer(ds, "23", "These Cheeses International");
			CustomerService customerService = new CustomerService(ds);

			Customer customer = customerService.GetCustomerByName("These' Cheeses");

			Assert.IsNotNull(customer);
			Assert.AreEqual("22", customer.CustomerId);
		}

		[TestMethod]
		public void SearchCustomersWithQuotesSearchesCorrectlyAndPreventsInjection()
		{
			OrdersManagementDataSet ds = new OrdersManagementDataSet();
			AddCustomer(ds, "10", "Company' A", "City' A", "State' A", "ZipCode' A", "Address' A");
			AddCustomer(ds, "11", "Company B", "City B", "State B", "ZipCode B", "Address B");
			AddCustomer(ds, "12", "Company C", "City C", "State C", "ZipCode C", "Address C");

			CustomerService customerService = new CustomerService(ds);

			IList<Customer> customers = customerService.SearchCustomers("Company' A", "City' A", "State' A", "ZipCode' A", "Address' A");

			Assert.IsNotNull(customers);
			Assert.AreEqual(1, customers.Count);
			Assert.AreEqual("10", customers[0].CustomerId);
		}

		[TestMethod]
		public void GetCustomersByIDWithQuotesSearchesCorrectlyAndPreventsInjection()
		{
			OrdersManagementDataSet ds = new OrdersManagementDataSet();
			AddCustomer(ds, "21", "The Store");
			AddCustomer(ds, "2'2", "These Cheeses");
			CustomerService customerService = new CustomerService(ds);

			Customer customer = customerService.GetCustomerById("2'2");

			Assert.IsNotNull(customer);
			Assert.AreEqual("These Cheeses", customer.CompanyName);
		}

        #region SearchCustomers Tests

        [TestMethod]
        public void SearchCustomersMatchesExactCompanyName()
        {
            OrdersManagementDataSet ds = new OrdersManagementDataSet();
            AddCustomer(ds, "10", "Company A");
            AddCustomer(ds, "11", "Company B");
            AddCustomer(ds, "12", "Company C");

            CustomerService customerService = new CustomerService(ds);

            IList<Customer> customers = customerService.SearchCustomers("Company B", string.Empty, string.Empty, string.Empty, string.Empty);

            Assert.IsNotNull(customers);
            Assert.AreEqual(1, customers.Count);
            Assert.AreEqual("11", customers[0].CustomerId);
            Assert.AreEqual("Company B", customers[0].CompanyName);
        }

        [TestMethod]
        public void SearchCustomersMatchesPartialCompanyName()
        {
            OrdersManagementDataSet ds = new OrdersManagementDataSet();
            AddCustomer(ds, "10", "Company A");
            AddCustomer(ds, "11", "Company B");
            AddCustomer(ds, "12", "Company C");

            CustomerService customerService = new CustomerService(ds);

            IList<Customer> customers = customerService.SearchCustomers("mp", string.Empty, string.Empty, string.Empty, string.Empty);

            Assert.IsNotNull(customers);
            Assert.AreEqual(3, customers.Count);
        }

        [TestMethod]
        public void SearchCustomersReturnsEmptyListWhenNoMatches()
        {
            OrdersManagementDataSet ds = new OrdersManagementDataSet();
            AddCustomer(ds, "10", "Company A");
            AddCustomer(ds, "11", "Company B");
            AddCustomer(ds, "12", "Company C");

            CustomerService customerService = new CustomerService(ds);

            IList<Customer> customers = customerService.SearchCustomers("None", string.Empty, string.Empty, string.Empty, string.Empty);

            Assert.IsNotNull(customers);
            Assert.AreEqual(0, customers.Count);
        }

        [TestMethod]
        public void SearchCustomersMatchesExactCity()
        {
            OrdersManagementDataSet ds = new OrdersManagementDataSet();
            AddCustomer(ds, "10", "Company A", "City A", string.Empty, string.Empty, string.Empty);
            AddCustomer(ds, "11", "Company B", "City B", string.Empty, string.Empty, string.Empty);
            AddCustomer(ds, "12", "Company C", "City B", string.Empty, string.Empty, string.Empty);

            CustomerService customerService = new CustomerService(ds);

            IList<Customer> customers = customerService.SearchCustomers(string.Empty, "City B", string.Empty, string.Empty, string.Empty);

            Assert.IsNotNull(customers);
            Assert.AreEqual(2, customers.Count);
            Assert.AreEqual("11", customers[0].CustomerId);
            Assert.AreEqual("12", customers[1].CustomerId);
        }

        [TestMethod]
        public void SearchCustomersMatchesExactState()
        {
            OrdersManagementDataSet ds = new OrdersManagementDataSet();
            AddCustomer(ds, "10", "Company A", string.Empty, "State B", string.Empty, string.Empty);
            AddCustomer(ds, "11", "Company B", string.Empty, "State A", string.Empty, string.Empty);
            AddCustomer(ds, "12", "Company C", string.Empty, "State A", string.Empty, string.Empty);

            CustomerService customerService = new CustomerService(ds);

            IList<Customer> customers = customerService.SearchCustomers(string.Empty, string.Empty, "State A", string.Empty, string.Empty);

            Assert.IsNotNull(customers);
            Assert.AreEqual(2, customers.Count);
            Assert.AreEqual("11", customers[0].CustomerId);
            Assert.AreEqual("12", customers[1].CustomerId);
        }

        [TestMethod]
        public void SearchCustomersMatchesExactZip()
        {
            OrdersManagementDataSet ds = new OrdersManagementDataSet();
            AddCustomer(ds, "10", "Company A", string.Empty, string.Empty, "ZipCode A", string.Empty);
            AddCustomer(ds, "11", "Company B", string.Empty, string.Empty, "ZipCode B", string.Empty);
            AddCustomer(ds, "12", "Company C", string.Empty, string.Empty, "ZipCode A", string.Empty);

            CustomerService customerService = new CustomerService(ds);

            IList<Customer> customers = customerService.SearchCustomers(string.Empty, string.Empty, string.Empty, "ZipCode B", string.Empty);

            Assert.IsNotNull(customers);
            Assert.AreEqual(1, customers.Count);
            Assert.AreEqual("11", customers[0].CustomerId);
        }

        [TestMethod]
        public void SearchCustomersMarchesExactAddress()
        {
            OrdersManagementDataSet ds = new OrdersManagementDataSet();
            AddCustomer(ds, "10", "Company A", string.Empty, string.Empty, string.Empty, "Address B");
            AddCustomer(ds, "11", "Company B", string.Empty, string.Empty, string.Empty, "Address A");
            AddCustomer(ds, "12", "Company C", string.Empty, string.Empty, string.Empty, "Address A");

            CustomerService customerService = new CustomerService(ds);

            IList<Customer> customers = customerService.SearchCustomers(string.Empty, string.Empty, string.Empty, string.Empty, "Address A");

            Assert.IsNotNull(customers);
            Assert.AreEqual(2, customers.Count);
            Assert.AreEqual("11", customers[0].CustomerId);
            Assert.AreEqual("12", customers[1].CustomerId);
        }

        [TestMethod]
        public void SearchCustomersMarchesExactStateAndPartialCompanyName()
        {
            OrdersManagementDataSet ds = new OrdersManagementDataSet();
            AddCustomer(ds, "10", "Corp A", string.Empty, "State A", string.Empty, string.Empty);
            AddCustomer(ds, "11", "Company B", string.Empty, "State A", string.Empty, string.Empty);
            AddCustomer(ds, "12", "Corp B", string.Empty, "State A", string.Empty, string.Empty);

            CustomerService customerService = new CustomerService(ds);

            IList<Customer> customers = customerService.SearchCustomers("Corp", string.Empty, "State A", string.Empty, string.Empty);

            Assert.IsNotNull(customers);
            Assert.AreEqual(2, customers.Count);
            Assert.AreEqual("10", customers[0].CustomerId);
            Assert.AreEqual("12", customers[1].CustomerId);
        }

        [TestMethod]
        public void SearchCustomersMatchesAllParameters()
        {
            OrdersManagementDataSet ds = new OrdersManagementDataSet();
            AddCustomer(ds, "10", "Company A", "City A", "State A", "ZipCode A", "Address A");
            AddCustomer(ds, "11", "Company B", "City B", "State B", "ZipCode B", "Address B");
            AddCustomer(ds, "12", "Company C", "City C", "State C", "ZipCode C", "Address C");

            CustomerService customerService = new CustomerService(ds);

            IList<Customer> customers = customerService.SearchCustomers("Company A", "City A", "State A", "ZipCode A", "Address A");

            Assert.IsNotNull(customers);
            Assert.AreEqual(1, customers.Count);
            Assert.AreEqual("10", customers[0].CustomerId);
        }

        #endregion

        private void AddCustomer(OrdersManagementDataSet ds, string customerId, string companyName)
		{
			OrdersManagementDataSet.CustomersRow row = ds.Customers.NewCustomersRow();
			row.CustomerId = customerId;
			row.CompanyName = companyName;
            row.City = string.Empty;
            row.Region = string.Empty;
            row.PostalCode = string.Empty;
            row.Address = string.Empty;
			ds.Customers.AddCustomersRow(row);
			ds.AcceptChanges();
		}

        private void AddCustomer(OrdersManagementDataSet ds, string customerId, string companyName, string city, string state, string zipCode, string address)
        {
            OrdersManagementDataSet.CustomersRow row = ds.Customers.NewCustomersRow();
            row.CustomerId = customerId;
            row.CompanyName = companyName;
            row.City = city;
            row.Region = state;
            row.PostalCode = zipCode;
            row.Address = address;
            ds.Customers.AddCustomersRow(row);
            ds.AcceptChanges();
        }
    }
}

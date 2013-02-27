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
using OrderManagement.Customers.Tests.Mocks;
using OrderManagement.SharedUserControls.Parts;
using OrdersRepository.BusinessEntities;

namespace OrderManagement.Customers.Tests
{
    /// <summary>
    /// Summary description for SearchCustomerPresenterTestFixture
    /// </summary>
    [TestClass]
    public class SearchCustomerPresenterTestFixture
    {
      SearchCustomerPresenter presenter;
        MockSearchCustomerPart view;
        MockFindCustomerService findCustomerService;
        MockPostalInfoLookupService postalInfoLookupService;

        [TestInitialize]
        public void TestInitialize() 
        {
            findCustomerService = new MockFindCustomerService();
            postalInfoLookupService = new MockPostalInfoLookupService();
            presenter = new SearchCustomerPresenter(findCustomerService, postalInfoLookupService);
            view = new MockSearchCustomerPart();
            presenter.View = view;
        }

        [TestMethod]
        public void OnViewInitializedSetStates()
        {
            List<State> states = new List<State>();
            states.Add(new State("id", "name"));
            postalInfoLookupService.AllStates = states;

            presenter.OnViewInitialized();

            Assert.IsTrue(view.StatesSet);
            Assert.AreEqual(postalInfoLookupService.AllStates, view.States);
        }
        
        [TestMethod]
        public void OnSearchCustomersRequestValues()
        {
            presenter.OnSearchCustomers();

            Assert.IsTrue(view.CompanyNameRequested);
            Assert.IsTrue(view.CityRequested);
            Assert.IsTrue(view.StateRequested);
            Assert.IsTrue(view.ZipCodeRequested);
            Assert.IsTrue(view.AddressRequested);
        }

        [TestMethod]
        public void OnSearchCustomersSelectedCustomerIsReset()
        {
            presenter.OnSearchCustomers();

            Assert.IsTrue(view.ResetSelectedCustomerCalled);
        }

        [TestMethod]
        public void OnSearchCustomersSearchCustomerServiceIsCalled()
        {
            findCustomerService.CustomersList = new List<Customer>();

            presenter.OnSearchCustomers();

            Assert.IsTrue(findCustomerService.SearchCustomersCalled);
            Assert.AreEqual(findCustomerService.CompanyName, view.CompanyName);
            Assert.AreEqual(findCustomerService.City, view.City);
            Assert.AreEqual(findCustomerService.State, view.State);
            Assert.AreEqual(findCustomerService.ZipCode, view.ZipCode);
            Assert.AreEqual(findCustomerService.Address, view.Address);
        }

        [TestMethod]
        public void OnSearchCustomersServiceInformationIsSetToView()
        {
            List<Customer> customerList = new List<Customer>();
            customerList.Add(new Customer());
            findCustomerService.CustomersList = customerList;

            presenter.OnSearchCustomers();

            Assert.IsTrue(view.ShowCustomersResultsCalled);
            Assert.AreEqual(1, view.CustomersResults.Count);
        }

		[TestMethod]
		public void ShouldShowMessageWhenNoResultsFound()
		{
			List<Customer> customerList = new List<Customer>();
			findCustomerService.CustomersList = customerList;

			presenter.OnSearchCustomers();

			Assert.IsTrue(findCustomerService.SearchCustomersCalled);
			Assert.AreEqual("No results were found.", view.NoResultsMessage);
		}
    }

    class MockSearchCustomerPart : ISearchCustomer
    {
        public bool CompanyNameRequested;
        public bool CityRequested;
        public bool StateRequested;
        public bool ZipCodeRequested;
        public bool AddressRequested;
        public bool ShowCustomersResultsCalled;
        public bool ResetSelectedCustomerCalled;
        public bool StatesSet;
		public string NoResultsMessage;

        public string CompanyName
        {
            get
            {
                CompanyNameRequested = true;
                return string.Empty;
            }
        }

        public string City
        {
            get
            {
                CityRequested = true;
                return string.Empty;
            }
        }

        public string State
        {
            get
            {
                StateRequested = true;
                return string.Empty;
            }
        }

        public string ZipCode
        {
            get
            {
                ZipCodeRequested = true;
                return string.Empty;
            }
        }

        public string Address
        {
            get
            {
                AddressRequested = true;
                return string.Empty;
            }
        }

        public IList<Customer> CustomersResults;

        public void ShowCustomersResults(IList<Customer> customersResults)
        {
            ShowCustomersResultsCalled = true;
            CustomersResults = customersResults;
        }

        private IEnumerable<State> _states;

        public IEnumerable<State> States
        {
            set
            {
                _states = value;
                StatesSet = true;
            }

            get
            {
                return _states;
            }
        }

        public void ResetSelectedCustomer()
        {
            ResetSelectedCustomerCalled = true;
        }

		#region ISearchCustomer Members


		public void ShowNoResultsMessage(string message)
		{
			NoResultsMessage = message;
		}

		#endregion
	}
}


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
using RealTimeSearchQuickstart.CustomerManager.Views;
using RealTimeSearchQuickstart.CustomerManager.Tests.Mocks;
using RealTimeSearchQuickstart.CustomerManager.BusinessEntities;
using System.Collections.ObjectModel;

namespace RealTimeSearchQuickstart.CustomerManager.Tests
{
	/// <summary>
	/// Summary description for SearchCustomerPresenterTestFixture
	/// </summary>
	[TestClass]
	public class SearchCustomerPresenterTestFixture
	{
		public SearchCustomerPresenterTestFixture()
		{
		}

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//

		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

        SearchCustomerPresenter presenter;
        MockSearchCustomerView view;
        MockCustomerService customerService;
         //Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void MyTestInitialize()
        {
            customerService = new MockCustomerService();
            presenter = new SearchCustomerPresenter(customerService);
            view = new MockSearchCustomerView();
            presenter.View = view;
        }

        [TestMethod]
        public void OnViewInitializedLoadStates()
        {
            presenter.OnViewInitialized();

            Assert.IsTrue(view.LoadStatesCalled);
        }

        [TestMethod]
        public void OnSearchCriteriaChangedCallGetCustomers()
        {
            view.CompanyName = "Any";

            presenter.OnSearchCriteriaChanged();

            Assert.IsTrue(customerService.GetCustomersCalled);
        }

        [TestMethod]
        public void OnSearchCriteriaShowCustomers()
        {
            view.CompanyName = "Any";
            ICollection<Customer> previosCustomers=new Collection<Customer>();
            view.CustomersShown=previosCustomers;
            presenter.OnSearchCriteriaChanged();

            Assert.IsTrue(view.ShowCustomersCalled);
            Assert.AreNotSame(previosCustomers, view.CustomersShown);
        }

		[TestMethod]
		public void SelectedStateIsUsedInSearchCriteria()
		{
			view.StateId = "WA";
			presenter.OnSearchCriteriaChanged();

			Assert.IsTrue(view.ShowCustomersCalled);
			Assert.AreEqual("WA", customerService.GetCustomers_StateId);
		}

		[TestMethod]
		public void SelectedPostalCodeIsUsedInSearchCriteria()
		{
			view.PostalCode = "95";
			presenter.OnSearchCriteriaChanged();

			Assert.IsTrue(view.ShowCustomersCalled);
			Assert.AreEqual("95", customerService.GetCustomers_PostalCode);
		}

        [TestMethod]
        public void SelectedCityNameIsUsedInSearchCriteria()
        {
            view.City = "REDMOND";
            presenter.OnSearchCriteriaChanged();

            Assert.IsTrue(view.ShowCustomersCalled);
            Assert.AreEqual("REDMOND", customerService.GetCustomers_City);
        }

		[TestMethod]
		public void OnSearchCriteriaUptadesViewTotalResultsIsUpdated()
		{
            view.CompanyName = "Any";
			customerService.GetCustomers_ReturnedTotalCount = 998;

			presenter.OnSearchCriteriaChanged();

			Assert.IsTrue(view.TotalResultsCountCalled);
			Assert.AreEqual(998, view.TotalResultsCount);
		}

        [TestMethod]
        public void OnEmptySearchCriteriaClearResults()
        {
            view.CompanyName = string.Empty;
            view.City = string.Empty;
            view.StateId = string.Empty;
            view.PostalCode = string.Empty;

            presenter.OnSearchCriteriaChanged();

            Assert.IsTrue(view.ClearResultsCalled);
        }

        [TestMethod]
        public void OnSearchCriteriaWithSpacesClearResults() {
            view.CompanyName = "    ";
            view.City = "      ";
            view.StateId = string.Empty;
            view.PostalCode = "  ";

            presenter.OnSearchCriteriaChanged();

            Assert.IsTrue(view.ClearResultsCalled);
        }
	}

	class MockSearchCustomerView : ISearchCustomerView
	{
        public bool LoadStatesCalled;
        public bool ShowCustomersCalled;
		public bool TotalResultsCountCalled;
        public bool PageResetCalled;
        public bool ClearResultsCalled;
        public ICollection<Customer> CustomersShown;

        public void LoadStates(RealTimeSearchQuickstart.CustomerManager.Repository.CustomersDataSet.StatesDataTable states)
		{
            LoadStatesCalled = true;
		}

        private string companyName;
        public string CompanyName
        {
            get { return companyName; }
            set { companyName = value; }
        }

        private string city;
        public string City
        {
            get { return city; }
            set { city = value; }
        }

		private string stateId;
		public string StateId
		{
			get { return stateId; }
			set { stateId = value; }
		}

		private string postalCode;
		public string PostalCode
		{
			get { return postalCode; }
			set { postalCode = value; }
		}

		private int totalResultsCount;

        public void SetTotalResultsCount(int totalResultsCount)
        {
            TotalResultsCountCalled = true;
            this.totalResultsCount = totalResultsCount;
        }
 
		internal int TotalResultsCount
		{
			get { return totalResultsCount; }
		}

        public void ShowCustomers(ICollection<Customer> customers)
        {
            ShowCustomersCalled = true;
            CustomersShown = customers;
        }

        public void ResetPage()
        {
            PageResetCalled = true;
        }

        public void ClearResults()
        {
            ClearResultsCalled = true;
        }
    }
}


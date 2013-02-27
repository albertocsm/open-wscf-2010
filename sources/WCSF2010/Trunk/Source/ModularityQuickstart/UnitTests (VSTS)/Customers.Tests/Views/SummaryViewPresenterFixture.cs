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
using ModularityQuickstart.Customers.BusinessEntities;
using ModularityQuickstart.Customers.Tests.Mocks;
using ModularityQuickstart.Customers.Views;
using ModularityQuickstart.Customers.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModularityQuickstart.Customers.Tests.Views.Mocks;

namespace ModularityQuickstart.Customers.Tests.Views
{
    [TestClass]
    public class SummaryViewPresenterFixture
    {
        [TestMethod]
        public void OnViewLoadedCallsControllerCurrentCustomerAndViewSetCustomer()
        {
            MockCustomersController controller = new MockCustomersController();
            Customer customer = controller.InnerCurrentCustomer = new Customer();
            SummaryViewPresenter presenter = new SummaryViewPresenter(controller);
            MockSummaryView view = new MockSummaryView();
            presenter.View = view;

            presenter.OnViewLoaded();

            Assert.IsTrue(controller.CurrentCustomerRetrieved);
            Assert.IsTrue(view.CustomerSet);
            Assert.AreSame(customer, view.Customer);
        }

        [TestMethod]
        public void OnApproveAnotherCustomerCallsControllerApproveAnotherCustomer()
        {
            MockCustomersController controller = new MockCustomersController();
            SummaryViewPresenter presenter = new SummaryViewPresenter(controller);
            MockSummaryView view = new MockSummaryView();
            presenter.View = view;

            presenter.OnApproveAnotherCustomer();

            Assert.IsTrue(controller.ApproveAnotherCustomerCalled);
        }
    }

    namespace Mocks
    {
        internal class MockSummaryView : ISummaryView
        {
            private Customer _customer;
            public bool CustomerSet = false;

            #region ISummaryView Members

            public Customer Customer
            {
                get { return _customer; }
                set
                {
                    _customer = value;
                    CustomerSet = true;
                }
            }

            #endregion
        }
    }
}

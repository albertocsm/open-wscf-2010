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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModularityQuickstart.Customers.Tests.Views.Mocks;

namespace ModularityQuickstart.Customers.Tests.Views
{
    [TestClass]
    public class ApproveCustomerViewPresenterFixture
    {
        [TestMethod]
        public void OnViewLoadedCallsControllerCurrentCustomerAndViewSetCustomer()
        {
            MockCustomersController controller = new MockCustomersController();
            Customer customer = controller.InnerCurrentCustomer = new Customer();
            ApproveCustomerViewPresenter presenter = new ApproveCustomerViewPresenter(controller);
            MockApproveCustomerView view = new MockApproveCustomerView();
            presenter.View = view;
            
            presenter.OnViewLoaded();

            Assert.IsTrue(controller.CurrentCustomerRetrieved);
            Assert.IsTrue(view.CustomerSet);
            Assert.AreSame(customer, view.Customer);
        }

        [TestMethod]
        public void OnViewLoadedDisablesShowingAndApprovingCustomerIfControllerCurrentCustomerIsNull()
        {
            MockCustomersController controller = new MockCustomersController();
            controller.InnerCurrentCustomer = null;
            ApproveCustomerViewPresenter presenter = new ApproveCustomerViewPresenter(controller);
            MockApproveCustomerView view = new MockApproveCustomerView();
            presenter.View = view;

            presenter.OnViewLoaded();

            Assert.IsTrue(controller.CurrentCustomerRetrieved);
            Assert.IsNull(view.Customer);
            Assert.IsFalse(view.AllowApproveCustomer);
            Assert.IsFalse(view.ShowCustomerDetails);
        }

        [TestMethod]
        public void OnApproveCustomerCallsControllerApproveCurrentCustomer()
        {
            MockCustomersController controller = new MockCustomersController();
            ApproveCustomerViewPresenter presenter = new ApproveCustomerViewPresenter(controller);
            MockApproveCustomerView view = new MockApproveCustomerView();
            presenter.View = view;

            presenter.OnApproveCustomer();

            Assert.IsTrue(controller.ApproveCurrentCustomerCalled);
        }
    }

    namespace Mocks
    {
        internal class MockApproveCustomerView : IApproveCustomerView
        {
            private Customer _customer = null;
            private bool _allowApproveCustomer = true;
            private bool _showCustomerDetails = true;
            public bool CustomerSet = false;

            #region IApproveCustomerView Members

            public bool AllowApproveCustomer
            {
                set { _allowApproveCustomer = value; }
                get { return _allowApproveCustomer; }
            }

            public Customer Customer
            {
                set 
                {
                    _customer = value;
                    CustomerSet = true; 
                }
                get { return _customer; }
            }

            public bool ShowCustomerDetails
            {
                set { _showCustomerDetails = value; }
                get { return _showCustomerDetails; }
            }

            #endregion
        }
    }
}

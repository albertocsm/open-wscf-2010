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
using System.Text;
using MVPWithCWABQuickStart.Contacts;
using MVPWithCWABQuickStart.Contacts.BusinessEntities;

namespace MVPWithCWABQuickStart.Contacts.Tests.Mocks
{
    public class MockContactsController : IContactsController
    {
        public event EventHandler CurrentCustomerChanged;
        public bool GetCustomersCalled=false;
        public bool UpdateCustomerCalled;
        public bool RestoreFromPersistedStateCalled;
        public Customer UpdatedCustomer;
        public int SelectedContactIndex;

        Customer _currentCustomer=null;
        public Customer CurrentCustomer
        {
            get
            {
                return _currentCustomer;
            }
            set { _currentCustomer = value; }
        }
        private List<Customer> _customers;

        internal IList<Customer> Customers
        {
            get
            {
                GetCustomersCalled = true;
                return _customers;
            }
            set { _customers = new List<Customer>(value); }
        }

        public IList<Customer> GetCustomers()
        {
            return _customers;
        }

        public void SetSelectedContactIndex(int selectedContactIndex)
        {
            SelectedContactIndex = selectedContactIndex;
        }

        public void FireCurrentCustomerChanged()
        {
            if (CurrentCustomerChanged != null)
            {
                CurrentCustomerChanged(this, EventArgs.Empty);
            }
        }

        public void RestoreFromPersistedState(int selectedIndex)
        {
            RestoreFromPersistedStateCalled = true;
            SetSelectedContactIndex(selectedIndex);
        }

        public void UpdateCurrentCustomer(Customer customer)
        {
            UpdateCustomerCalled = true;
            UpdatedCustomer = customer;
        }
    }
}

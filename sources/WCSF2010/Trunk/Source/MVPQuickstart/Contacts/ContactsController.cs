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
using System.Web;
using System.Collections.Generic;
using MVPQuickstart.Contacts.BusinessEntities;
using MVPQuickstart.Contacts.Services;

namespace MVPQuickstart.Contacts
{
    public class ContactsController : IContactsController
    {
        public event EventHandler CurrentCustomerChanged;

        private int _selectedContactIndex = -1;
        private ICustomersDataSource _datasource;
        private IList<Customer> _customers;

        public ContactsController(ICustomersDataSource datasource)
        {
            _datasource = datasource;
        }

        public void SetSelectedContactIndex(int selectedContactIndex)
        {
            _selectedContactIndex = selectedContactIndex;
            OnCurrentCustomerChanged(new EventArgs());
        }

        public void RestoreFromPersistedState(int selectedIndex)
        {
            SetSelectedContactIndex(selectedIndex);
        }

        public IList<Customer> GetCustomers()
        {
            return Customers;
        }

        private IList<Customer> Customers
        {
            get
            {
                if (_customers == null)
                {
                    _customers = _datasource.Customers;
                }
                return _customers;
            }
        }

        public Customer CurrentCustomer
        {
            get
            {
                Customer currentCustomer = Customers[_selectedContactIndex];

                return currentCustomer;
            }
        }

        protected void OnCurrentCustomerChanged(EventArgs e)
        {
            if (CurrentCustomerChanged != null)
            {
                CurrentCustomerChanged(this, e);
            }
        }

        public void UpdateCurrentCustomer(Customer customer)
        {
            _datasource.UpdateCustomer(customer);
            customer = null;
        }
    }
}

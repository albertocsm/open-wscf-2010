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
using MVPQuickstart.Contacts.Services;
using MVPQuickstart.Contacts.BusinessEntities;

namespace MVPQuickstart.Contacts.Tests.Mocks
{
    class MockCustomersDataSource : ICustomersDataSource
    {
        public IList<Customer> GetCustomersReturnValue = new List<Customer>();
        public bool GetCustomersCalled = false;
        public List<State> GetStatesReturnValue = new List<State>();
        public bool GetStatesCalled = false;
        public Customer UpdatedCustomerValue;
        public bool UpdateCustomerCalled = false;

        public IList<Customer> Customers
        {
            get
            {
                GetCustomersCalled = true;
                return GetCustomersReturnValue;
            }

        }
        public ICollection<State> States
        {
            get
            {
                GetStatesCalled = true;
                return GetStatesReturnValue;
            }
        }

        public void UpdateCustomer(Customer customer)
        {
            UpdateCustomerCalled = true;
            UpdatedCustomerValue = customer;
        }
    }
}

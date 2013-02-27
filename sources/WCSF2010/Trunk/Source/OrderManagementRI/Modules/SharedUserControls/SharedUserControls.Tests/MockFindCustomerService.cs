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
using OrderManagement.SharedUserControls.Services;
using OrdersRepository.BusinessEntities;

namespace OrderManagement.Customers.Tests.Mocks
{
    class MockFindCustomerService : IFindCustomerService
    {

        public bool SearchCustomersCalled;
        public IList<Customer> CustomersList;
        public string CompanyName;
        public string City;
        public string State;
        public string ZipCode;
        public string Address;

        #region IFindCustomerService Members

        public IList<Customer> SearchCustomers(string companyName, string city, string state, string zipCode, string address)
        {
            CompanyName = companyName;
            State = state;
            City = city;
            ZipCode = zipCode;
            Address = address;

            SearchCustomersCalled = true;
            return CustomersList;
        }

        #endregion
    }
}

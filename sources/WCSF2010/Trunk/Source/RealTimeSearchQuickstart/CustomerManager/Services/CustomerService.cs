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
using RealTimeSearchQuickstart.CustomerManager.BusinessEntities;
using System.Collections.ObjectModel;
using RealTimeSearchQuickstart.CustomerManager.Repository;

namespace RealTimeSearchQuickstart.CustomerManager.Services
{
    public class CustomerService : ICustomerService
    {
        CustomersDataSource _customersDataSource;
        public CustomerService()
        {
            _customersDataSource = new CustomersDataSource();
        }
		public ICollection<Customer> GetCustomers(string companyName, string stateId, string postalCode, string city,int startRowIndex, out int totalCount)
        {
            ICollection<Customer> col;
            col = _customersDataSource.SearchCustomers(companyName, city, stateId, postalCode, startRowIndex, 10);
            totalCount = _customersDataSource.GetRowCount(companyName, city, stateId, postalCode, 0, 0);
            return col;
        }
    }
}

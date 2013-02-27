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
using RealTimeSearchQuickstart.CustomerManager.Services;
using RealTimeSearchQuickstart.CustomerManager.BusinessEntities;

namespace RealTimeSearchQuickstart.CustomerManager.Tests.Mocks
{
    class MockCustomerService:ICustomerService
    {
		public string GetCustomers_StateId;
        public string GetCustomers_PostalCode;
        public string GetCustomers_City;
        public bool GetCustomersCalled;
        public int GetCustomers_StartRowIndex;
        public int GetCustomers_ReturnedTotalCount;

		public ICollection<Customer> GetCustomers(string CompanyName, string stateId, string postalCode, string city,int startRowIndex, out int totalCount)
        {
            GetCustomersCalled = true;
			GetCustomers_StateId = stateId;
			GetCustomers_PostalCode = postalCode;
            GetCustomers_City = city;
            GetCustomers_StartRowIndex = startRowIndex;
			totalCount = GetCustomers_ReturnedTotalCount;
            return null;
        }
    }
}

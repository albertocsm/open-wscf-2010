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
using OrdersRepository.BusinessEntities;
using OrdersRepository.Interfaces.Services;

namespace OrderManagement.Orders.Tests.Mocks
{
	public class MockCustomerService : ICustomerService
	{
        public bool GetCustomersByNameCalled;
        public bool GetCustomersByIdCalled;
        public Customer ReturnedCustomer = new Customer();

		public Customer GetCustomerByName(string name)
		{
			GetCustomersByNameCalled = true;
            if (ReturnedCustomer != null)
            {
                ReturnedCustomer.CompanyName = name;
                ReturnedCustomer.CustomerId = "ID" + name;
            }
            return ReturnedCustomer;
		}


        public Customer GetCustomerById(string id)
        {
            GetCustomersByIdCalled = true;
            return ReturnedCustomer;
        }

		public IList<Customer> GetCustomersByNamePrefix(string namePrefix)
		{
			throw new Exception("The method or operation is not implemented.");
		}

        public IList<Customer> SearchCustomers(string companyName, string city)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IList<Customer> SearchCustomers(string companyName, string city, string state, string zipCode, string address)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}

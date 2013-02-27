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
using System.Data;
using System.Globalization;
using OrdersRepository.BusinessEntities;
using OrdersRepository.Interfaces.Services;
using OrdersRepository.Services.Utility;

namespace OrdersRepository.Services
{
	public class CustomerService : ICustomerService
	{
       OrdersManagementDataSet repository;

        public CustomerService() : this(OrdersManagementRepository.Instance)
        {  }

        public CustomerService(OrdersManagementDataSet repository)
        {
            this.repository = repository;
        }


        public IList<Customer> GetCustomersByNamePrefix(string namePrefix)
        {
			namePrefix = InputValidator.EncodeQueryStringParameter(namePrefix);
            DataRow[] rows = repository.Customers.Select(String.Format(CultureInfo.CurrentCulture, "CompanyName LIKE '{0}*'", namePrefix));
            IList<Customer> customers = new List<Customer>(rows.Length);
            foreach (OrdersManagementDataSet.CustomersRow customersRow in rows)
            {
                customers.Add(TranslateFromCustomerRowToCustomerEntity(customersRow));
            }
            return customers;
        }

        public Customer GetCustomerByName(string name)
        {
			name = InputValidator.EncodeQueryStringParameter(name);
            DataRow[] rows = repository.Customers.Select(String.Format(CultureInfo.CurrentCulture, "CompanyName='{0}'", name));
            if (rows != null && rows.Length > 0)
                return TranslateFromCustomerRowToCustomerEntity(rows[0] as OrdersManagementDataSet.CustomersRow);

            return null;
        }


        public Customer GetCustomerById(string id)
        {
            OrdersManagementDataSet.CustomersRow customerRow = repository.Customers.FindByCustomerId(id);
            if (customerRow == null)
                return null;

            return TranslateFromCustomerRowToCustomerEntity(customerRow);
        }

        private static Customer TranslateFromCustomerRowToCustomerEntity(OrdersManagementDataSet.CustomersRow customersRow)
        {
			Customer customer = new Customer();
			customer.Address = customersRow.IsAddressNull() ? null : customersRow.Address;
			customer.City = customersRow.IsCityNull() ? null : customersRow.City;
			customer.CompanyName = customersRow.CompanyName;
			customer.CustomerId = customersRow.CustomerId;
			customer.PostalCode = customersRow.IsPostalCodeNull() ? null : customersRow.PostalCode;
			customer.Region = customersRow.IsRegionNull() ? null : customersRow.Region;
			return customer;
        }

        public IList<Customer> SearchCustomers(string companyName, string city, string state, string zipCode, string address)
        {
			companyName = InputValidator.EncodeQueryStringParameter(companyName);
			city = InputValidator.EncodeQueryStringParameter(city);
			state = InputValidator.EncodeQueryStringParameter(state);
			zipCode = InputValidator.EncodeQueryStringParameter(zipCode);
			address = InputValidator.EncodeQueryStringParameter(address);

            // LIKE comparisons are done under the premise that there are not any NULL values in the DataStore.
            DataRow[] rows = repository.Customers.Select(String.Format(CultureInfo.CurrentCulture, "CompanyName LIKE '*{0}*' AND (City LIKE '*{1}*') AND (Region LIKE '*{2}*') AND (PostalCode LIKE '*{3}*') AND (Address LIKE '*{4}*')", companyName, city, state, zipCode, address));
            IList<Customer> customers = new List<Customer>(rows.Length);
            foreach (OrdersManagementDataSet.CustomersRow customersRow in rows)
            {
                customers.Add(TranslateFromCustomerRowToCustomerEntity(customersRow));
            }
            return customers;
        }
    }
}

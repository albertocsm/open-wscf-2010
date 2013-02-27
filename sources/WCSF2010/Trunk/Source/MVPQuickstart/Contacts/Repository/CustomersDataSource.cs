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
using System.Data;
using System.Configuration;
using System.Web;
using System.Globalization;
using System.Collections.Generic;
using System.Collections;
using MVPQuickstart.Contacts.BusinessEntities;
using MVPQuickstart.Contacts.Services;
using System.Collections.ObjectModel;

namespace MVPQuickstart.Contacts.Repository
{
    public class CustomersDataSource : ICustomersDataSource
    {
        private IList<Customer> _customers;

        public IList<Customer> Customers
        {
            get
            {
                DataView view = new DataView(CustomersDataSet.Instance.Customers);
                _customers = new List<Customer>();
                foreach (DataRowView row in view)
                {
                    CustomersDataSet.CustomersRow customerRow = row.Row as CustomersDataSet.CustomersRow;
                    _customers.Add(new Customer(customerRow.Id, customerRow.Name, customerRow.Address, customerRow.City, customerRow.State, customerRow.Zip));
                }

                return _customers;
            }
        }

        public ICollection<State> States
        {
            get
            {
                DataView view = new DataView(CustomersDataSet.Instance.States);
                ICollection<State> _states = new Collection<State>();
                foreach (DataRowView row in view)
                {
                    CustomersDataSet.StatesRow stateRow = row.Row as CustomersDataSet.StatesRow;
                    _states.Add(new State(stateRow.Id, stateRow.Name));
                }
                return _states;
            }
        }

        public void UpdateCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            CustomersDataSet.CustomersDataTable customers = CustomersDataSet.Instance.Customers;
            CustomersDataSet.CustomersRow row = customers.FindById(customer.Id);
            if (row != null)
            {
                row.Name = customer.Name;
                row.Address = customer.Address;
                row.City = customer.City;
                row.State = customer.State;
                row.Zip = customer.Zip;
                row.AcceptChanges();
            }
        }
    }
}

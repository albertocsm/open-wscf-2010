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
using RealTimeSearchQuickstart.CustomerManager.BusinessEntities;

namespace RealTimeSearchQuickstart.CustomerManager.Repository
{
    public class CustomersDataSource
    {
        private const string SearchCustomersQueryFormat = @"(Name LIKE '%{0}%' OR '{0}' = '')
                                                        AND (City LIKE '%{1}%' OR '{1}' = '')
                                                        AND (State = '{2}' OR '{2}' = '')
                                                        AND (Zip LIKE '%{3}%' OR '{3}' = '')";
        private string _lastName;
        private string _lastCity;
        private string _lastState;
        private string _lastZip;
        private int _lastStartrowIndex = -1;
        private int _lastMaximunRows = -1;
		private List<Customer> _customers;

		public ICollection<Customer> SearchCustomers(string name, string city, string state, string zip, int startRowIndex, int maximumRows)
        {
            List<Customer> customers = getCustomers(name, city, state, zip, startRowIndex, maximumRows);
            return customers;
        }

        public int GetRowCount(string name, string city, string state, string zip, int startRowIndex, int maximumRows)
        {
            List<Customer> customers = getCustomers(name, city, state, zip, startRowIndex, maximumRows);
            return customers.Count;
        }

        private List<Customer> getCustomers(string name, string city, string state, string zip, int startRowIndex, int maximumRows)
        {
            if (ParametersHasChanged(name, city, state, zip, startRowIndex, maximumRows))
            {
                string encodedName = InputValidator.EncodeQueryStringParameter(name);
                string encodedCity = InputValidator.EncodeQueryStringParameter(city);
                string encodedState = InputValidator.EncodeQueryStringParameter(state);
                string encodedZip = InputValidator.EncodeQueryStringParameter(zip);

                string query = string.Format(CultureInfo.CurrentCulture, SearchCustomersQueryFormat, encodedName, encodedCity, encodedState, encodedZip);
                DataView view = new DataView(CustomersDataSet.Instance.Customers);
                view.RowFilter = query;
                int _rowCount = view.Count;

                int rowsCount = 0;
                int index = startRowIndex;
                _customers = new List<Customer>(maximumRows);
                while ((rowsCount < maximumRows || maximumRows==0) && index < _rowCount)
                {
                    CustomersDataSet.CustomersRow row = view[index].Row as CustomersDataSet.CustomersRow;
                    _customers.Add(new Customer(row.Id, row.Name, row.Address, row.City, row.State, row.Zip));
                    rowsCount++;
                    index++;
                }

                StoreLastUsedParameters(name, city, state, zip, startRowIndex, maximumRows);
            }
            return _customers;
        }

        private bool ParametersHasChanged(string name, string city, string state, string zip, int startRowIndex, int maximumRows)
        {
            return _lastName != name || _lastCity != city || _lastState != state || _lastZip != zip || _lastStartrowIndex != startRowIndex || _lastMaximunRows != maximumRows;
        }

        private void StoreLastUsedParameters(string name, string city, string state, string zip, int startRowIndex, int maximumRows)
        {
            _lastName = name;
            _lastCity = city;
            _lastState = state;
            _lastZip = zip;
            _lastStartrowIndex = startRowIndex;
            _lastMaximunRows = maximumRows;
        }
    }

}

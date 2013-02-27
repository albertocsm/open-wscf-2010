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
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Web;
using ModularityQuickstart.Navigation.Services;
using ModularityQuickstart.Customers.BusinessEntities;
using ModularityQuickstart.Customers.Constants;
using Microsoft.Practices.ObjectBuilder;

namespace ModularityQuickstart.Customers
{
    public class CustomersController : ICustomersController
    {
        private StateValue<Customer> _currentCustomer;
        private StateValue<List<Customer>> _customers;
        private INavigationService _navigationService;

        public CustomersController([ServiceDependency] INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [InjectionMethod]
        public void InitializeState([StateDependency("CurrentCustomer")] StateValue<Customer> currentCustomerState,
                                [StateDependency("Customers")] StateValue<List<Customer>> customersState)
        {
            _currentCustomer = currentCustomerState;
            _customers = customersState;
        }

        public Customer CurrentCustomer
        {
            get
            {
                if (_currentCustomer.Value == null)
                {
                    _currentCustomer.Value = GetNextCustomer();
                }
                return _currentCustomer.Value;
            }
            set
            {
                _currentCustomer.Value = value;
            }
        }

        public void ApproveAnotherCustomer()
        {
            CurrentCustomer = null;
            Navigate(ViewNames.ApproveCustomerView);
        }

        public void ApproveCurrentCustomer()
        {
            CurrentCustomer.Approved = true;
            UpdateCustomer(CurrentCustomer);
            Navigate(ViewNames.SummaryView);
        }

        public void UpdateCustomer(Customer customer)
        {
            Customer customerFound = FindCustomer(customer);
            customerFound.Approved = customer.Approved;
            customerFound.LastName = customer.LastName;
            customerFound.Name = customer.Name;
        }

        private List<Customer> Customers
        {
            get
            {
                if (_customers.Value == null)
                {
                    // Set up a mock customers data store
                    _customers.Value = new List<Customer>();
                    _customers.Value.Add(new Customer(1000, "Enrique", "Gil", false));
                    _customers.Value.Add(new Customer(1001, "Paul", "West", false));
                    _customers.Value.Add(new Customer(1002, "Nuria", "Gonzalez", false));
                }
                return _customers.Value;
            }
        }

        private Customer FindCustomer(Customer customer)
        {
            return Customers.Find(delegate(Customer match)
            {
                return match.Id == customer.Id;
            });
        }

        private Customer GetNextCustomer()
        {
            return Customers.Find(delegate(Customer customer)
            {
                return !customer.Approved;
            });
        }


        private void Navigate(string to)
        {
            _navigationService.Navigate(to);
        }

    }
}

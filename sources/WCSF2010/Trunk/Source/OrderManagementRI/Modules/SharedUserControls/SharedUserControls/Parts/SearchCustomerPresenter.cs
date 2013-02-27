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
using OrdersRepository.Interfaces.Services;
using OrderManagement.SharedUserControls.Services;
using OrdersRepository.BusinessEntities;
using Microsoft.Practices.CompositeWeb;
using System.Collections.Generic;

namespace OrderManagement.SharedUserControls.Parts
{
    public class SearchCustomerPresenter : Presenter<ISearchCustomer>
    {
        private IFindCustomerService _findCustomerService;
        private IPostalInfoLookupService _postalInfoLookupService;

        public SearchCustomerPresenter(
            [ServiceDependency] IFindCustomerService findCustomerService, 
            [ServiceDependency] IPostalInfoLookupService postalInfoLookupService)
        {
            _findCustomerService = findCustomerService;
            _postalInfoLookupService = postalInfoLookupService;
        }

        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {
            View.States = _postalInfoLookupService.AllStates;
        }

		public void OnSearchCustomers()
		{
			View.ResetSelectedCustomer();

			string companyName = View.CompanyName;
			string city = View.City;
			string state = View.State;
			string zipCode = View.ZipCode;
			string address = View.Address;

			IList<Customer> customers = _findCustomerService.SearchCustomers(companyName, city, state, zipCode, address);
			if ((customers == null)||(customers.Count == 0))
			{
				View.ShowNoResultsMessage("No results were found.");
			}

			View.ShowCustomersResults(customers);
		}
    }
}





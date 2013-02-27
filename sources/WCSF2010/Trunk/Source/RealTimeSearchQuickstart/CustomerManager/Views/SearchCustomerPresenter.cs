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
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using RealTimeSearchQuickstart.CustomerManager.Repository;
using RealTimeSearchQuickstart.CustomerManager.Services;
using RealTimeSearchQuickstart.CustomerManager.BusinessEntities;

namespace RealTimeSearchQuickstart.CustomerManager.Views
{
	public class SearchCustomerPresenter : Presenter<ISearchCustomerView>
	{
		private ICustomerService _customerService;
		public SearchCustomerPresenter([ServiceDependency] ICustomerService customerService)
		{
			_customerService = customerService;
		}

		public override void OnViewLoaded()
		{
			// TODO: Implement code that will be executed every time the view loads
		}

		public override void OnViewInitialized()
		{
			View.LoadStates(CustomersDataSet.Instance.States);
			View.ClearResults();
		}

		public void OnSearchCriteriaChanged()
		{
			View.ResetPage();

			OnPageChange(0);
		}

		public void OnPageChange(int startRowIndex)
		{
			int totalCount = RetrieveAndShowCustomers(startRowIndex);
			View.SetTotalResultsCount(totalCount);
		}

		private int RetrieveAndShowCustomers(int startRowIndex)
		{
			int totalCount = 0;
			ICollection<Customer> customers = null;

			if (EmptyInput())
			{
				View.ClearResults();
			}
			else
			{
				customers = _customerService.GetCustomers(TrimIfNotNull(View.CompanyName), TrimIfNotNull(View.StateId), TrimIfNotNull(View.PostalCode), TrimIfNotNull(View.City), startRowIndex, out totalCount);
				View.ShowCustomers(customers);
			}
			return totalCount;
		}

		private bool EmptyInput()
		{
			return string.IsNullOrEmpty(TrimIfNotNull(View.CompanyName))
				&& string.IsNullOrEmpty(TrimIfNotNull(View.StateId))
				&& string.IsNullOrEmpty(TrimIfNotNull(View.PostalCode))
				&& string.IsNullOrEmpty(TrimIfNotNull(View.City));
		}

		private static string TrimIfNotNull(string s)
		{
			if (s == null)
			{
				return s;
			}
			else
			{
				return s.Trim();
			}
		}
	}
}





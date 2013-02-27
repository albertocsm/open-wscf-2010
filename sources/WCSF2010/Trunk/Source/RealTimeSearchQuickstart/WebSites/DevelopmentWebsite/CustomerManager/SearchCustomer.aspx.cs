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
using Microsoft.Practices.ObjectBuilder;
using RealTimeSearchQuickstart.CustomerManager.Repository;
using RealTimeSearchQuickstart.CustomerManager.BusinessEntities;
using System.Collections.Generic;
using System.Globalization;

namespace RealTimeSearchQuickstart.CustomerManager.Views
{
    public partial class SearchCustomer : Microsoft.Practices.CompositeWeb.Web.UI.Page, ISearchCustomerView
    {
        private SearchCustomerPresenter _presenter;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
            }
            this._presenter.OnViewLoaded();

        }

        [CreateNew]
        public SearchCustomerPresenter Presenter
        {
            set
            {
                if (value != null)
                {
                    this._presenter = value;
                    this._presenter.View = this;
                }
            }
            get
            {
                return _presenter;
            }
        }

        public void ResetPage()
        {
            CustomersGridView.PageIndex = 0;
        }

        protected void CompanyNameTextBox_TextChanged(object sender, EventArgs e)
        {
            // If you do not see the progress indicator, uncomment the line below 
            // to see it. You will see the progress indicator when entering a value in the 
            // 'Name' input field.
            //
            // System.Threading.Thread.Sleep(2000);

            _presenter.OnSearchCriteriaChanged();
        }

		protected void StateDropDown_SelectedIndexChanged(object sender, EventArgs e)
		{
            _presenter.OnSearchCriteriaChanged();
        }

		protected void ZipCodeTextBox_TextChanged(object sender, EventArgs e)
		{
            _presenter.OnSearchCriteriaChanged();
        }

        protected void CityTextBox_TextChanged(object sender, EventArgs e)
        {
            _presenter.OnSearchCriteriaChanged();
        }

        public void ShowCustomers(ICollection<Customer> customers)
        {
            CustomersContainerDataSource.DataSource = customers;
            CustomersGridView.Visible = true;
        }

        public void ClearResults()
        {
            CustomersContainerDataSource.DataSource = null;
            CustomersGridView.Visible = false;
            TotalResultsCountLabel.Visible = false;
            TotalResultsCountTitleLabel.Visible = false;
        }

        public void SetTotalResultsCount(int totalResultsCount)
        {
            bool displayTotalResultsCount = totalResultsCount != 0;
            TotalResultsCountLabel.Visible = displayTotalResultsCount;
            TotalResultsCountTitleLabel.Visible = displayTotalResultsCount;
            TotalResultsCountLabel.Text = totalResultsCount.ToString(CultureInfo.CurrentCulture);
            CustomersContainerDataSource.TotalRowCount = totalResultsCount;
        }

        public void LoadStates(CustomersDataSet.StatesDataTable states)
        {
            StateDropDown.DataSource = states;
            StateDropDown.DataBind();
        }

        public string CompanyName
        {
            get
            { return this.CompanyNameTextBox.Text; }
        }

        public string City
        {
            get
            { return this.CityTextBox.Text; }
        }

        public string StateId
        {
            get { return this.StateDropDown.SelectedValue; }
        }

		public string PostalCode
		{
			get { return this.ZipCodeTextBox.Text; }
		}

        protected void CustomersContainerDataSource_Selecting(object sender, Microsoft.Practices.Web.UI.WebControls.ObjectContainerDataSourceSelectingEventArgs e)
        {
            if (e != null)
            {
                _presenter.OnPageChange(e.Arguments.StartRowIndex);
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            _presenter.OnSearchCriteriaChanged();
        }
    }
}


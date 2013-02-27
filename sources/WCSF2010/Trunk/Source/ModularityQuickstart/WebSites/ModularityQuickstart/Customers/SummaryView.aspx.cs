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
using System.Web.UI;
using Microsoft.Practices.ObjectBuilder;
using ModularityQuickstart.Customers.BusinessEntities;
using ModularityQuickstart.Customers.Views;
using System.Globalization;

public partial class CustomersSummaryView : Microsoft.Practices.CompositeWeb.Web.UI.Page, ISummaryView
{
    SummaryViewPresenter _presenter;
    private Customer _customer;

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        ShowCustomer();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            _presenter.OnViewInitialized();
        }
        _presenter.OnViewLoaded();
    }

    [CreateNew]
    public SummaryViewPresenter Presenter
    {
		get
		{
			return _presenter;
		}
        set
        {
            _presenter = value;
            if (value != null)
            {
                _presenter.View = this;
            }
        }
    }

    public Customer Customer
    {
        get { return _customer; }
        set { _customer = value; }
    }

    private void ShowCustomer()
    {
        if (_customer == null)
            return;

        LastNameLabel.Text = _customer.LastName;
        NameLabel.Text = _customer.Name;
        IdLabel.Text = _customer.Id.ToString(CultureInfo.CurrentUICulture);
    }

    protected void ApproveAnotherCustomerButton_Click(object sender, EventArgs e)
    {
        _presenter.OnApproveAnotherCustomer();
    }
}

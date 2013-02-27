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
using ModularityQuickstart.Customers.BusinessEntities;
using ModularityQuickstart.Customers.Views;
using System.Globalization;

public partial class CustomersApproveCustomerView : Microsoft.Practices.CompositeWeb.Web.UI.Page, IApproveCustomerView
{
    private ApproveCustomerViewPresenter _presenter;
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
    public ApproveCustomerViewPresenter Presenter
    {
		get
		{
			return _presenter;
		}
        set
        {
            _presenter = value;
            if (_presenter != null)
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

    public bool AllowApproveCustomer
    {
        get { return ApproveCustomerButton.Visible; }
        set { ApproveCustomerButton.Visible = value; }
    }

    public bool ShowCustomerDetails
    {
        get { return CustomerDetailsPanel.Visible; }
        set
        {
            CustomerDetailsPanel.Visible = value;
            NoCustomersLabel.Visible = !value;
        }
    }

    private void ShowCustomer()
    {
        if (_customer == null)
            return;

        LastNameLabel.Text = _customer.LastName;
        NameLabel.Text = _customer.Name;
        IdLabel.Text = _customer.Id.ToString(CultureInfo.CurrentUICulture);
        ApprovedLabel.Text = _customer.Approved ? "Yes" : "No";
    }

    protected void ApproveCustomerButton_Click(object sender, EventArgs e)
    {
        _presenter.OnApproveCustomer();
    }
}

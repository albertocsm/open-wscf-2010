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
using System;
using Microsoft.Practices.ObjectBuilder;
using ObjectContainerDataSourceQuickstart.Modules.Customers.BusinessEntities;
using System.Collections.Generic;
using Microsoft.Practices.Web.UI.WebControls;
using ObjectContainerDataSourceQuickstart.Modules.Customers.Views.CustomersAdvancedView;
using Microsoft.Practices.CompositeWeb.Web.UI;

public partial class Customers_CustomersAdvancedView : Page, ICustomersAdvancedView
{
    private CustomersAdvancedViewPresenter _presenter;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            _presenter.OnViewInitialized();
        }
        _presenter.OnViewLoaded();
    }

    [CreateNew]
    public CustomersAdvancedViewPresenter Presenter
    {
        set
        {
            _presenter = value;
            _presenter.View = this;
        }
    }

    #region ICustomersAdvancedView Members

    public IList<Customer> Customers
    {
        set { CustomersDataSource.DataSource = value; }
    }

    // <snippet id="TotalCustomersCount">
    public int TotalCustomersCount
    {
        set { CustomersDataSource.TotalRowCount = value; }
    }
    // </snippet>

    #endregion

    protected void CustomersDataSource_Inserted(object sender, ObjectContainerDataSourceStatusEventArgs e)
    {
        _presenter.OnCustomerInserted((Customer)e.Instance);
    }

    protected void CustomersDataSource_Deleted(object sender, ObjectContainerDataSourceStatusEventArgs e)
    {
        _presenter.OnCustomerDeleted((Customer)e.Instance);
    }

    protected void CustomersDataSource_Updated(object sender, ObjectContainerDataSourceStatusEventArgs e)
    {
        _presenter.OnCustomerUpdated((Customer)e.Instance);
    }

    // <snippet id="Selecting">
    protected void CustomersDataSource_Selecting(object sender, ObjectContainerDataSourceSelectingEventArgs e)
    {
        _presenter.OnSelecting(e.Arguments.StartRowIndex, e.Arguments.MaximumRows, e.Arguments.SortExpression);
    }
    // </snippet>
}

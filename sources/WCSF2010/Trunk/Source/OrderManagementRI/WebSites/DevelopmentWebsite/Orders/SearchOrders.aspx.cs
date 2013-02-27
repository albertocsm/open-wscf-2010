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
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.ObjectBuilder;
using OrderManagement.Orders.Views;
using Orders.PresentationEntities;
using System.Collections.Generic;
using OrdersRepository.BusinessEntities;
using Microsoft.Practices.Web.UI.WebControls;
using System.Globalization;
using Microsoft.Security.Application;

public partial class Orders_SearchOrders : Microsoft.Practices.CompositeWeb.Web.UI.Page, ISearchOrders
{
    private SearchOrdersPresenter _presenter;
    private const string ShowOrderDetailsCommand = "ShowOrderDetails";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            this._presenter.OnViewInitialized();
        }
        this._presenter.OnViewLoaded();
    }

    [CreateNew]
    public SearchOrdersPresenter Presenter
    {
        set
        {
            this._presenter = value;
            this._presenter.View = this;
        }
    }

    public void ShowOrders(ICollection<OrderInfo> orders)
    {
        OrdersContainerDataSource.DataSource = orders;
    }

    public void HidePagesPanel()
    {
        PagesPanel.Visible = false;
    }

    public void ShowPagesCounters()
    {
        PagesPanel.Visible = true;
        int actualPage = OrdersGridView.PageIndex + 1;
        ActualPage.Text = actualPage.ToString();
        TotalPages.Text = PageCount.ToString(CultureInfo.CurrentCulture);
    }

    public void ShowSearchText(string searchText)
    {
        SearchedTextLabel.Text = AntiXss.HtmlEncode(searchText);
    }

    protected void OrdersGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == ShowOrderDetailsCommand)
        {
            int rowIndex = int.Parse((string)e.CommandArgument);
            string orderID = (string)((GridView)e.CommandSource).DataKeys[rowIndex].Value;
            _presenter.OnOrderDetailsRequested(orderID);
        }
    }

    public void ShowOrderDetails(Order order)
    {
        OrderPreviewPart.ShowOrder(order);

        //Programmatically show the popup. A non operational control was needed to put as the TargetControlID of the 
        //extender in order to show the popup programmatically (this could not be done client-side, 
        //because we need to populate de details of the selected order)
        DetailsModalPopupExtender.Show();
    }

    protected void OrdersContainerDataSource_Selecting(object sender, ObjectContainerDataSourceSelectingEventArgs e)
    {
        Page.Validate();
        if (Page.IsValid)
        {
            string searchText = SearchTextBox.Text;
            _presenter.OnOrdersSelecting(searchText, e.Arguments.StartRowIndex, e.Arguments.MaximumRows);
        }
    }

    public int TotalOrdersCount
    {
        set
        {
            OrdersContainerDataSource.TotalRowCount = value;

            // The GridView controls uses the ViewState to store its PageCount.
            // As the OrdersGridView ViewState has been disabled, the TotalOrdersCount
            // is stored to calculate the PageCount.
            ViewState["TotalOrdersCount"] = value;
        }
        get
        {
            return (int)(ViewState["TotalOrdersCount"] ?? 0);
        }
    }

    public int MaximumRowsPerPage
    {
        get { return OrdersGridView.PageSize; }
    }

    protected void GoToPageButton_Click(object sender, EventArgs e)
    {
        Page.Validate("Paging");
        if (Page.IsValid)
        {
            int pageIndex = int.Parse(GoToPageTextBox.Text) - 1;
            _presenter.OnGoToPage(pageIndex);
        }
    }

    public void ShowCurrentPage(int currentPage)
    {
        OrdersGridView.PageIndex = currentPage;
    }

    public int PageCount
    {
        get
        {
            if (TotalOrdersCount == 0)
                return 0;

            return (TotalOrdersCount / MaximumRowsPerPage) + 1;
        }
    }

    protected void SearchButton_Click(object sender, EventArgs e)
    {
        string searchText = SearchTextBox.Text;
        _presenter.OnSearchTextChanged(searchText);
    }
}



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
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.ObjectBuilder;
using OrderManagement.Orders.Views.Parts;
using OrdersRepository.BusinessEntities;
using UserControl=Microsoft.Practices.CompositeWeb.Web.UI.UserControl;

public partial class Orders_SearchProduct : UserControl, ISearchProduct
{
    private SearchProductPresenter _presenter;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            this._presenter.OnViewInitialized();
        }
        this._presenter.OnViewLoaded();

		NoResultsLabel.Visible = false;

        WireUpEvents();
    }

    [CreateNew]
    public SearchProductPresenter Presenter
    {
        set
        {
            this._presenter = value;
            this._presenter.View = this;
        }
    }

    private void WireUpEvents()
    {
        // Javascript event wired to trigger ProductUpdatePanel when search text changes (using a buffer to prevent flickering when the user types fast). 
        string script = String.Format(CultureInfo.InvariantCulture, "var callbackMethod = \"{0}\"; var searchProductTextBoxId = '{1}'; setTimeout('requestProductsLoop();', 4000);"
            , Page.ClientScript.GetPostBackClientHyperlink(ProductTextBox, "")
            , ProductTextBox.ClientID);
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "InitializeTimer", script, true);
    }

    public string SelectedProductSku
    {
        get
        {
            if (this.ProductsGridView.SelectedIndex >= 0)
                return this.ProductsGridView.SelectedDataKey.Value.ToString();

            return null;
        }
    }

    public void ShowProductsResults(IList<Product> products)
    {
        ProductContainerDataSource.DataSource = products;
        ProductsGridView.SelectedIndex = -1;
    }

    public void ResetSelectedProduct()
    {
        ProductsGridView.SelectedIndex = 0;
    }

	public void ShowNoResultsMessage(string message)
	{
		NoResultsLabel.Visible = true;
		NoResultsLabel.Text = message;
	}

    protected void ProductTextBox_TextChanged(object sender, EventArgs e)
    {
        _presenter.OnSearchTextChanged(ProductTextBox.Text);
    }
    protected void ProductsGridView_Sorting(object sender, GridViewSortEventArgs e)
    {
        _presenter.OnSorting(e.SortExpression, !OrderDirectionAscending, ProductTextBox.Text);
        e.Cancel = true;
    }

    public bool OrderDirectionAscending
    {
        get
        {
            return (bool)(ViewState["AscendingOrder"] ?? true);
        }
        set
        {
            ViewState["AscendingOrder"] = value;
        }
    }

    protected void ProductsGridView_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Javascript event wired over Grid row to select the row without needing to click the Select link.
            string rowPostbackArguments = string.Format("Select${0}", e.Row.RowIndex);
            string postbackCall = this.Page.ClientScript.GetPostBackClientHyperlink(ProductsGridView, rowPostbackArguments);
            e.Row.Attributes.Add("onclick", postbackCall);
        }
    }

    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow row in ProductsGridView.Rows)
        {
            string rowPostbackArguments = string.Format("Select${0}", row.RowIndex);
            this.Page.ClientScript.RegisterForEventValidation(ProductsGridView.UniqueID, rowPostbackArguments);
        }

        base.Render(writer);
    }

    // This method let reuse the MVP pair when used inside a ModalPopupExtender
    public void ClearView()
    {
        ProductTextBox.TextChanged -= ProductTextBox_TextChanged;
        ProductTextBox.Text = "";
        ShowProductsResults(new List<Product>());
        ProductTextBox.TextChanged += ProductTextBox_TextChanged;
    }
}



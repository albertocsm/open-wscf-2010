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
using OrderManagement.Orders.Views.Parts;
using Orders.PresentationEntities;
using System.Collections.Generic;
using OrdersRepository.BusinessEntities;

public partial class Orders_SavedDrafts : Microsoft.Practices.CompositeWeb.Web.UI.UserControl, ISavedDrafts
{
    private SavedDraftsPresenter _presenter;
    const string EditOrderCommand = "EditOrder";
    const string DeleteOrderCommand = "DeleteOrder";
    const string ShowOrderDetailsCommand = "ShowOrderDetails";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            this._presenter.OnViewInitialized();
        }
        this._presenter.OnViewLoaded();
    }

    [CreateNew]
    public SavedDraftsPresenter Presenter
    {
        set
        {
            this._presenter = value;
            this._presenter.View = this;
        }
    }

    protected void MySavedDraftsGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == EditOrderCommand)
        {
            int rowIndex = int.Parse((string)e.CommandArgument);
            string orderID = (string)((GridView)e.CommandSource).DataKeys[rowIndex].Value;
            this._presenter.OnEditOrder(orderID);
        }
        else if (e.CommandName == DeleteOrderCommand)
        {
            int rowIndex = int.Parse((string)e.CommandArgument);
            string orderID = (string)((GridView)e.CommandSource).DataKeys[rowIndex].Value;
            this._presenter.OnDeleteOrder(orderID);
        }
        else if (e.CommandName == ShowOrderDetailsCommand)
        {
            int rowIndex = int.Parse((string)e.CommandArgument);
            string orderID = (string)((GridView)e.CommandSource).DataKeys[rowIndex].Value;
            _presenter.OnOrderDetailsRequested(orderID);
        }
    }

    protected void EditDeleteOrder_Command(object sender, CommandEventArgs e)
    {
        string orderID = (string)e.CommandArgument;
        if (e.CommandName == EditOrderCommand)
        {
            this._presenter.OnEditOrder(orderID);
        }
        else if (e.CommandName == DeleteOrderCommand)
        {
            this._presenter.OnDeleteOrder(orderID);
        }
        EditOrderButton.CommandArgument = String.Empty;
        DeleteOrderButton.CommandArgument = String.Empty;

        DetailsModalPopupExtender.Hide();
    }

    public void ShowOrders(ICollection<OrderInfo> orders)
    {
        MySavedDraftsGridView.DataSource = orders;
        MySavedDraftsGridView.DataBind();
    }

    public void ShowOrderDetails(Order order)
    {
        //Tell the button which order to approve or reject when clicking on them
        EditOrderButton.CommandArgument = order.OrderId.ToString();
        DeleteOrderButton.CommandArgument = order.OrderId.ToString();

        OrderPreviewPart.ShowOrder(order);

        //Programmatically show the popup. A non operational control was needed to put as the TargetControlID of the 
        //extender in order to show the popup programmatically (this could not be done client-side, 
        //because we need to populate de details of the selected order)
        DetailsModalPopupExtender.Show();
    }
}



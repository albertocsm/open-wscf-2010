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
using System.Collections.Generic;
using Orders.PresentationEntities;
using OrdersRepository.BusinessEntities;

public partial class Orders_Approvals : Microsoft.Practices.CompositeWeb.Web.UI.UserControl, IApprovals
{
    private ApprovalsPresenter _presenter;
    const string ApproveOrderCommand = "ApproveOrder";
    const string RejectOrderCommand = "RejectOrder";
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
    public ApprovalsPresenter Presenter
    {
        set
        {
            this._presenter = value;
            this._presenter.View = this;
        }
    }

    public void ShowOrders(ICollection<OrderInfo> orders)
    {
        MyApprovalsGridView.DataSource = orders;
        MyApprovalsGridView.DataBind();
    }

    protected void MyApprovalsGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == ApproveOrderCommand)
        {
            int rowIndex = int.Parse((string)e.CommandArgument);
            string orderID = (string)((GridView)e.CommandSource).DataKeys[rowIndex].Value;
            this._presenter.OnApproveOrder(orderID);
        }
        else if (e.CommandName == RejectOrderCommand)
        {
            int rowIndex = int.Parse((string)e.CommandArgument);
            string orderID = (string)((GridView)e.CommandSource).DataKeys[rowIndex].Value;
            this._presenter.OnRejectOrder(orderID);
        }
        else if (e.CommandName == ShowOrderDetailsCommand)
        {
            int rowIndex = int.Parse((string)e.CommandArgument);
            string orderID = (string)((GridView)e.CommandSource).DataKeys[rowIndex].Value;
            _presenter.OnOrderDetailsRequested(orderID);

        }
    }

    public void ShowOrderDetails(Order order)
    {
        //Tell the button which order to approve or reject when clicking on them
        ApproveOrderButton.CommandArgument = order.OrderId.ToString();
        RejectOrderButton.CommandArgument = order.OrderId.ToString();

        OrderPreviewPart.ShowOrder(order);

        //Programmatically show the popup. A non operational control was needed to put as the TargetControlID of the 
        //extender in order to show the popup programmatically (this could not be done client-side, 
        //because we need to populate de details of the selected order)
        DetailsModalPopupExtender.Show();
    }

    protected void ApproveRejectOrder_Command(object sender, CommandEventArgs e)
    {
        string orderID = (string)e.CommandArgument;
        if (e.CommandName == ApproveOrderCommand)
        {
            this._presenter.OnApproveOrder(orderID);
        }
        else if (e.CommandName == RejectOrderCommand)
        {
            this._presenter.OnRejectOrder(orderID);
        }
        ApproveOrderButton.CommandArgument = String.Empty;
        RejectOrderButton.CommandArgument = String.Empty;

        DetailsModalPopupExtender.Hide();
    }
}



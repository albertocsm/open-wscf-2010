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
using System.Web.UI.WebControls;
using Microsoft.Practices.ObjectBuilder;
using OrderManagement.Orders.Views.Parts;
using Orders.PresentationEntities;
using System.Collections.Generic;
using OrdersRepository.BusinessEntities;
using Microsoft.Security.Application;

public partial class Orders_OrderPreview : Microsoft.Practices.CompositeWeb.Web.UI.UserControl, IOrderPreview
{
    private OrderPreviewPresenter _presenter;
    private decimal _total;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            this._presenter.OnViewInitialized();
        }
        this._presenter.OnViewLoaded();
    }

    [CreateNew]
    public OrderPreviewPresenter Presenter
    {
        set
        {
            this._presenter = value;
            this._presenter.View = this;
        }
    }


    public void ShowOrderInfo(OrderInfo order, IList<OrderItemLine> orderItemLines)
    {
        //The order information data will be shown in Label controls, which are not automatically encoded by ASP.NET.
        //It's a good practice to always encode data that comes from an external/untrusted source (in this case, the database).
        //The data is explicitly encoded using Microsoft Anti-Cross Site Scripting Library which can be downloaded from:
        //http://www.microsoft.com/downloads/details.aspx?familyid=efb9c819-53ff-4f82-bfaf-e11625130c25
        OrderNo.Text = order.OrderId;
        OrderName.Text = AntiXss.HtmlEncode(order.OrderName);
        OrderStatus.Text = AntiXss.HtmlEncode(order.OrderStatus);
        Description.Text = AntiXss.HtmlEncode(order.Description);
        Customer.Text = AntiXss.HtmlEncode(order.CustomerName);
        ApprovedBy.Text = AntiXss.HtmlEncode(order.Approver);
        Street.Text = AntiXss.HtmlEncode(order.Address);
        State.Text = AntiXss.HtmlEncode(order.State);
        City.Text = AntiXss.HtmlEncode(order.City);
        PostalCode.Text = AntiXss.HtmlEncode(order.PostalCode);

        _total = order.OrderTotal;

        OrderDetailsGridView.DataSource = orderItemLines;
        OrderDetailsGridView.DataBind();
    }

    protected void OrderDetailsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label totalLabel = e.Row.FindControl("TotalLabel") as Label;
            if (totalLabel != null)
            {
                totalLabel.Text = _total.ToString("C2");
            }
        }
    }

    public void ShowOrder(Order order)
    {
        _presenter.OnShowOrder(order);
    }
}



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
using System.Globalization;
using OrderManagement.Orders.Properties;

namespace OrderManagement.Orders.Views
{
    public class OrderEntryPresenter : Presenter<IOrderEntry>
    {
        IOrdersController _ordersController;

        public OrderEntryPresenter(IOrdersController ordersController)
        {
            _ordersController = ordersController;
        }

        public override void OnViewInitialized()
        {
            string editOrderId = View.EditOrderId;
            if (String.IsNullOrEmpty(editOrderId))
            {
                _ordersController.StartCreateOrderFlow();
            }
            else
            {
                int orderId;
                if (!Int32.TryParse(editOrderId, NumberStyles.Integer, CultureInfo.CurrentCulture, out orderId))
                    throw new ArgumentException(Resources.InvalidQueryString);

                _ordersController.StartLoadOrderFlow(orderId);
            }
        }
    }
}





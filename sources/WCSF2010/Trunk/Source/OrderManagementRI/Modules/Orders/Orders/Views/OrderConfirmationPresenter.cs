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
    public class OrderConfirmationPresenter : Presenter<IOrderConfirmation>
    {
        private IOrdersController _controller;

        public OrderConfirmationPresenter([CreateNew] IOrdersController controller)
        {
            if (controller == null)
                throw new ArgumentNullException("controller");

            _controller = controller;
        }

        public override void OnViewInitialized()
        {
            int orderId = _controller.CurrentOrder.OrderId;
            View.ShowConfirmationMessage(String.Format(CultureInfo.CurrentCulture, Resources.SubmitConfirmationMessage, orderId));
            _controller.CreateOrderComplete();
        }

        public void CreateNewOrder()
        {
            _controller.StartCreateOrderFlow();
        }
    }
}





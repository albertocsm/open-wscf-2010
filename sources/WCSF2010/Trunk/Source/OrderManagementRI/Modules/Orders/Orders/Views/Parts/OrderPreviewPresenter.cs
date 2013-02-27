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
using Orders.PresentationEntities;
using OrdersRepository.BusinessEntities;
using OrderManagement.Orders.Converters;

namespace OrderManagement.Orders.Views.Parts
{
    public class OrderPreviewPresenter : Presenter<IOrderPreview>
    {
        private IBusinessPresentationConverter<Order, OrderInfo> _ordersConverter;
        private IBusinessPresentationConverter<OrderDetail, OrderItemLine> _orderDetailsConverter;

        [InjectionConstructor]
        public OrderPreviewPresenter(
           [ServiceDependency] IBusinessPresentationConverter<Order, OrderInfo> ordersConverter,
           [ServiceDependency] IBusinessPresentationConverter<OrderDetail, OrderItemLine> orderDetailsConverter
            )
        {
            _ordersConverter = ordersConverter;
            _orderDetailsConverter = orderDetailsConverter;
        }


        public void OnShowOrder(Order order)
        {
            OrderInfo orderInfo = _ordersConverter.ConvertBusinessToPresentation(order);
            List<OrderItemLine> lines = new List<OrderItemLine>();

            foreach (OrderDetail detail in order.Details)
            {
                OrderItemLine orderItemLine = _orderDetailsConverter.ConvertBusinessToPresentation(detail);
                lines.Add(orderItemLine);
            }
            View.ShowOrderInfo(orderInfo, lines);
        }
    }
}





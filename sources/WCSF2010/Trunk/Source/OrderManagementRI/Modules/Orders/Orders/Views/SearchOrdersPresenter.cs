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
using OrdersRepository.Interfaces.Services;
using OrderManagement.Orders.Converters;
using OrdersRepository.BusinessEntities;
using Orders.PresentationEntities;
using System.Globalization;
using System.Collections.ObjectModel;

namespace OrderManagement.Orders.Views
{
    public class SearchOrdersPresenter : Presenter<ISearchOrders>
    {
        private IOrdersService _ordersService;
        private IBusinessPresentationConverter<Order, OrderInfo> _ordersConverter;

        public SearchOrdersPresenter([ServiceDependency] IOrdersService ordersService,
                                    [ServiceDependency] IBusinessPresentationConverter<Order, OrderInfo> ordersConverter)
        {
            _ordersService = ordersService;
            _ordersConverter = ordersConverter;
        }

        public override void OnViewLoaded()
        {
            View.HidePagesPanel();
        }

        public void OnSearchTextChanged(string searchText)
        {
            View.ShowCurrentPage(0);
            View.ShowSearchText(searchText);
        }

        public void OnOrderDetailsRequested(string orderId)
        {
            int _orderId = int.Parse(orderId,CultureInfo.CurrentCulture);
            Order selectedOrder = _ordersService.GetOrderWithDetails(_orderId);
            View.ShowOrderDetails(selectedOrder);
        }

        public void OnOrdersSelecting(string searchText, int startRowIndex, int maximumRows)
        {
            SelectOrders(searchText, startRowIndex, maximumRows);
        }

        private void SelectOrders(string searchText, int startRowIndex, int maximumRows)
        {
            if (!string.IsNullOrEmpty(searchText))
            {
                int ordersTotalCount;
                ICollection<Order> orders = _ordersService.SearchOrders(searchText, startRowIndex, maximumRows, out ordersTotalCount);

                ICollection<OrderInfo> ordersInfo = new Collection<OrderInfo>();
                foreach (Order order in orders)
                {
                    OrderInfo info = _ordersConverter.ConvertBusinessToPresentation(order);
                    ordersInfo.Add(info);
                }

                View.ShowOrders(ordersInfo);
                View.TotalOrdersCount=ordersTotalCount;
                View.ShowPagesCounters();
                View.ShowSearchText(searchText);
            }
            else
            {
                View.ShowOrders(new Collection<OrderInfo>());
                View.TotalOrdersCount=0;
                View.HidePagesPanel();
            }
        }

        public void OnGoToPage(int pageIndex)
        {
            if (pageIndex >= 0 && pageIndex < View.PageCount)
            {
                View.ShowCurrentPage(pageIndex);
            }
            // TODO: Show error, or find a better way to validate the pageIndex value.

        }
    }
}





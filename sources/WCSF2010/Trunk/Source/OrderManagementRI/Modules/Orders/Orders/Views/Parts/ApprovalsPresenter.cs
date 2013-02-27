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
using OrderManagement.Orders.Converters;
using OrdersRepository.BusinessEntities;
using Orders.PresentationEntities;
using Microsoft.Practices.CompositeWeb.Interfaces;
using OrdersRepository.Interfaces.Services;
using System.Globalization;
using System.Collections.ObjectModel;

namespace OrderManagement.Orders.Views.Parts
{
    public class ApprovalsPresenter : Presenter<IApprovals>
    {
        private IOrdersService _ordersService;
        private IBusinessPresentationConverter<Order, OrderInfo> _ordersConverter;
        private IHttpContextLocatorService _httpContextLocatorService;

        public ApprovalsPresenter([ServiceDependency] IOrdersService ordersService, 
            [ServiceDependency] IBusinessPresentationConverter<Order, OrderInfo> ordersConverter,
           [ServiceDependency] IHttpContextLocatorService httpContextLocatorService)
        {
            _ordersService = ordersService;
            _ordersConverter = ordersConverter;
            _httpContextLocatorService = httpContextLocatorService;
        }

        public override void OnViewInitialized()
        {
			LoadOrdersForCurrentApprover();
        }

		private void LoadOrdersForCurrentApprover()
		{
			string employeeId = this.CurrentEmployee;
            ICollection<OrderInfo> orderInfos = new Collection<OrderInfo>();
			foreach (Order order in _ordersService.GetOrdersForApprover(employeeId))
			{
				OrderInfo info = _ordersConverter.ConvertBusinessToPresentation(order);
				orderInfos.Add(info);
			}

			View.ShowOrders(orderInfos);
		}

        protected virtual string CurrentEmployee
        {
            get
            {
                return _httpContextLocatorService.GetCurrentContext().User.Identity.Name;
            }
        }

		public void OnApproveOrder(string orderId)
		{
			int _orderId = int.Parse(orderId,CultureInfo.CurrentCulture);
			_ordersService.ApproveOrder(_orderId);
			LoadOrdersForCurrentApprover();
		}

		public void OnRejectOrder(string orderId)
		{
			int _orderId = int.Parse(orderId,CultureInfo.CurrentCulture);
			_ordersService.RejectOrder(_orderId);
			LoadOrdersForCurrentApprover();
		}

        public void OnOrderDetailsRequested(string orderId)
        {
			int _orderId = int.Parse(orderId, CultureInfo.CurrentCulture);
            Order selectedOrder = _ordersService.GetOrderWithDetails(_orderId);
            View.ShowOrderDetails(selectedOrder);
        }
    }
}





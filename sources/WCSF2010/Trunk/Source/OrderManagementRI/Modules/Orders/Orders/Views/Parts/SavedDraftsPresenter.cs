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
using Microsoft.Practices.CompositeWeb.Interfaces;
using OrdersRepository.BusinessEntities;
using Orders.PresentationEntities;
using System.Globalization;
using System.Collections.ObjectModel;

namespace OrderManagement.Orders.Views.Parts
{
    public class SavedDraftsPresenter : Presenter<ISavedDrafts>
    {
	    IOrdersService _ordersService;
		IOrdersController _ordersController;
		IBusinessPresentationConverter<Order, OrderInfo> _ordersConverter;
		IHttpContextLocatorService _httpContextLocatorService;

        public SavedDraftsPresenter(
			[ServiceDependency] IOrdersService ordersService,
			[CreateNew] IOrdersController ordersController,
		    [ServiceDependency] IBusinessPresentationConverter<Order, OrderInfo> ordersConverter,
		    [ServiceDependency] IHttpContextLocatorService httpContextLocatorService)
		{
			_ordersService = ordersService;
			_ordersController = ordersController;
			_ordersConverter = ordersConverter;
			_httpContextLocatorService = httpContextLocatorService;
		}

		public override void OnViewInitialized()
		{
			LoadOrdersForCurrentEmployee();
		}

		private void LoadOrdersForCurrentEmployee()
		{
			string employeeId = GetCurrentEmployee();
			ICollection<OrderInfo> orderInfos = new Collection<OrderInfo>();
			_ordersService.GetSavedDraftOrders(employeeId);

			foreach (Order order in _ordersService.GetSavedDraftOrders(employeeId))
			{
				OrderInfo info = _ordersConverter.ConvertBusinessToPresentation(order);
				orderInfos.Add(info);
			}

			View.ShowOrders(orderInfos);
		}

		private string GetCurrentEmployee()
		{
			return _httpContextLocatorService.GetCurrentContext().User.Identity.Name;
		}

		public void OnOrderDetailsRequested(string orderId)
		{
			int _orderId = int.Parse(orderId, CultureInfo.CurrentCulture);
			Order selectedOrder = _ordersService.GetOrderWithDetails(_orderId);
            View.ShowOrderDetails(selectedOrder);
		}

		public void OnEditOrder(string orderId)
		{
			int _orderId = int.Parse(orderId, CultureInfo.CurrentCulture);
			_ordersController.StartLoadOrderFlow(_orderId);
		}

		public void OnDeleteOrder(string orderId)
		{
			int _orderId = int.Parse(orderId,CultureInfo.CurrentCulture);
			_ordersService.DeleteOrder(_orderId);
			LoadOrdersForCurrentEmployee();
		}
    }
}





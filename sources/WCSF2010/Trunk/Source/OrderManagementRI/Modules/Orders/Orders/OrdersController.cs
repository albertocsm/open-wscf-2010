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
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Interfaces;
using OrdersRepository.Interfaces.Services;
using OrderManagement.Orders.Services;
using Microsoft.Practices.ObjectBuilder;
using OrdersRepository.BusinessEntities;
using OrderStatusEnum = OrdersRepository.BusinessEntities.Enums.OrderStatus;
using OrderManagement.Orders.Properties;
using Microsoft.Practices.CompositeWeb.Web;

namespace OrderManagement.Orders
{
    public class OrdersController : IOrdersController
    {
        private IOrdersService _ordersService;
        private IHttpContextLocatorService _httpContextLocatorService;
        private IOrderEntryFlowService _orderEntryFlowService;
        private StateValue<Order> _currentOrder;

        [InjectionConstructor]
        public OrdersController(
            [ServiceDependency] IOrdersService ordersService,
            [ServiceDependency] IHttpContextLocatorService httpContextLocatorService,
            [ServiceDependency] IOrderEntryFlowService orderEntryFlowService)
        {
            _ordersService = ordersService;
            _httpContextLocatorService = httpContextLocatorService;
            _orderEntryFlowService = orderEntryFlowService;
        }

        [InjectionMethod]
        public void InitializeState([StateDependency("CurrentOrder")]StateValue<Order> orderState)
        {
            _currentOrder = orderState;
        }

        protected IOrderEntryFlowService OrderEntryFlowService
        {
            get { return _orderEntryFlowService; }
        }

        protected IHttpContextLocatorService HttpContextLocatorService
        {
            get { return _httpContextLocatorService; }
        }

        protected IOrdersService OrdersService
        {
            get { return _ordersService; }
        }

        public void SaveCurrentOrderAsDraft()
        {
            Order order = CurrentOrder;
            _ordersService.SaveAsDraft(order);
            CurrentOrder = order;
        }

        public void SubmitOrder()
        {
            Order order = CurrentOrder;
            _ordersService.SubmitOrder(order);
            CurrentOrder = order;
        }

        public virtual void CreateNewOrder()
        {
            Order currentOrder = new Order();
            currentOrder.Creator = this.CurrentEmployee;
            currentOrder.OrderStatus = (int)OrderStatusEnum.Draft;
            CurrentOrder = currentOrder;
        }

        protected string CurrentEmployee
        {
            get
            {
                return _httpContextLocatorService.GetCurrentContext().User.Identity.Name;
            }
        }

        public Order CurrentOrder
        {
            get
            {
                return _currentOrder.Value;
            }
            set
            {
                _currentOrder.Value = value;
            }
        }

        public void StartCreateOrderFlow()
        {
            CreateNewOrder();
            NavigateToCurrentView();
        }

        public void StartLoadOrderFlow(int orderId)
        {
            Order order = _ordersService.GetOrderWithDetails(orderId);
            if (order == null
                || order.Creator != CurrentEmployee
                || order.OrderStatus != (int)OrderStatusEnum.Draft
                )
                throw new ArgumentException(Resources.InvalidQueryString);

            CurrentOrder = order;
            NavigateToCurrentView();
        }

        public void NavigateNextFromGeneralView()
        {
            _orderEntryFlowService.NavigateNextFromGeneralView();
        }

        public void NavigatePreviousFromDetailsView()
        {
            _orderEntryFlowService.NavigatePreviousFromDetailsView();
        }

        public void NavigateNextFromDetailsView()
        {
            _orderEntryFlowService.NavigateNextFromDetailsView();
        }

        public void NavigatePreviousFromPreviewView()
        {
            _orderEntryFlowService.NavigatePreviousFromReviewView();
        }

        protected void NavigateToCurrentView()
        {
            _orderEntryFlowService.NavigateToCurrentView();
        }

        public void NavigateToConfirmationView()
        {
            _orderEntryFlowService.NavigateNextFromReviewView();
        }

        public void CancelChanges()
        {
            CurrentOrder = null;
            _orderEntryFlowService.NavigateToDefaultView();
        }

        public void CreateOrderComplete()
        {
            CurrentOrder = null;
        }

        /// <summary>
        /// Verifies that the user has started a flow. If the flow is not started, the user gets redirected to the default page.
        /// </summary>
        /// <remarks>
        /// The user could be writing the URL manually, and should not be allowed to access the pages in the flow.
        /// The user cannot use the browser's back button when the flow is completed or aborted.
        /// </remarks>
        public void VerifyOrderEntryFlowIsStarted()
        {
            if (CurrentOrder == null)
            {
                _orderEntryFlowService.NavigateToDefaultView();
            }
        }
    }
}

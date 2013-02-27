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
using System.Collections.Generic;
using OrdersRepository.BusinessEntities;
using OrdersRepository.Interfaces.Services;

namespace OrderManagement.Orders.Tests.Mocks
{
    public class MockOrdersService : IOrdersService
    {
        public Order Order;
        public bool GetOrderWithDetailsCalled;
        public bool SaveAsDraftCalled;
        public bool SubmitOrderCalled;
        public bool GetOrdersForApproverCalled;
        public string GetOrdersForApproverArgumentValue;
        public List<Order> GetOrdersForApproverList;
		public bool GetSavedDraftOrdersCalled;
		public string GetSavedDraftOrdersArgumentValue;
		public List<Order> GetSavedDraftOrdersList;
		public bool ApproveOrderCalled;
		public int ApproveOrderArgumentValue;
		public bool RejectOrderCalled;
		public int RejectOrderArgumentValue;
		public bool DeleteOrderCalled;
		public int DeleteOrderArgumentValue;
        public bool SearchOrdersCalled;
        public bool SearchOrdersOverloadCalled;

        public ICollection<Order> OrdersForSearchOrders;

        public Order GetOrderWithDetails(int orderId)
        {
            GetOrderWithDetailsCalled = true;
            if (Order != null)
                Order.OrderId = orderId;
            return Order;
        }


        public void SaveAsDraft(Order order)
        {
            SaveAsDraftCalled = true;
        }

        public void SubmitOrder(Order order)
        {
            SubmitOrderCalled = true;
        }

        public IList<Order> GetOrdersForApprover(string employeeId)
        {
            GetOrdersForApproverCalled = true;
            GetOrdersForApproverArgumentValue = employeeId;
            return GetOrdersForApproverList;
        }

		public IList<Order> GetSavedDraftOrders(string employeeId)
		{
			GetSavedDraftOrdersCalled = true;
			GetSavedDraftOrdersArgumentValue = employeeId;
			return GetSavedDraftOrdersList;
		}

		public void ApproveOrder(int orderId)
		{
			ApproveOrderCalled = true;
			ApproveOrderArgumentValue = orderId;
		}

		public void RejectOrder(int orderId)
		{
			RejectOrderCalled = true;
			RejectOrderArgumentValue = orderId;
		}

		public void DeleteOrder(int orderId)
		{
			DeleteOrderCalled = true;
			DeleteOrderArgumentValue = orderId;
		}

        public ICollection<Order> SearchOrders(string searchText)
        {
            SearchOrdersCalled = true;
            return OrdersForSearchOrders;
        }

        public ICollection<Order> SearchOrders(string searchText, int startOrderIndex, int maximumOrdersCount, out int ordersTotalCount)
        {
            SearchOrdersOverloadCalled = true;
            ordersTotalCount = OrdersForSearchOrders.Count;
            return OrdersForSearchOrders;
        }
    }
}

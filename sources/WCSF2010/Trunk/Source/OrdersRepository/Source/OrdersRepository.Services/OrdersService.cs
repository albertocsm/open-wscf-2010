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
using System.Data;
using System.Globalization;
using OrdersRepository.BusinessEntities;
using OrdersRepository.Interfaces.Services;
using OrdersRepository.Services.Properties;
using OrderStatusEnum = OrdersRepository.BusinessEntities.Enums.OrderStatus;
using OrdersRepository.Services.Utility;
using System.Collections.ObjectModel;
using Microsoft.Practices.ObjectBuilder;

namespace OrdersRepository.Services
{
	public class OrdersService : IOrdersService
	{
        OrdersManagementDataSet repository;
		IProductService _productService;

        [InjectionConstructor]
		public OrdersService(IProductService productService)
			: this(OrdersManagementRepository.Instance, productService)
        {  }

		public OrdersService(OrdersManagementDataSet repository, IProductService productService)
		{
			this.repository = repository;
			_productService = productService;
		}

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validation done by Guard class.")]
        public void SaveAsDraft(Order order)
        {
            Guard.ArgumentNotNull(order, "order");

            if (order.OrderStatus > (int)OrderStatusEnum.Draft)
                throw new InvalidOperationException(Resources.OrderStatusInvalidForSaveAsDraft);

            int newStatus = (int)OrderStatusEnum.Draft;
            order.OrderId = PersistOrder(order, newStatus);
            order.OrderStatus = newStatus;
        }

        public void SubmitOrder(Order order)
        {
            if (order.OrderStatus > (int)OrderStatusEnum.Draft)
				throw new InvalidOperationException(Resources.OrderStatusInvalidForSubmitOrder);

            int newStatus = (int)OrderStatusEnum.Submitted;
            order.OrderDate = DateTime.Now;
            order.OrderId = PersistOrder(order, newStatus);
            order.OrderStatus = newStatus;
        }

        private int PersistOrder(Order order, int newStatus)
        {
            OrdersManagementDataSet.OrdersRow ordersRow;
            if (order.OrderId <= 0)
            {
                ordersRow = repository.Orders.NewOrdersRow();
            }
            else
            {
                ordersRow = repository.Orders.FindByOrderId(order.OrderId);
                foreach (OrdersManagementDataSet.OrderDetailsRow detailRow in ordersRow.GetOrderDetailsRows())
                {
                    detailRow.Delete();
                }
            }
            TranslateFromOrderEntityToOrdersRow(order, ordersRow);
            ordersRow.OrderStatus = newStatus;

            if (order.Details != null)
            {
                foreach (OrderDetail detail in order.Details)
                {
                    OrdersManagementDataSet.OrderDetailsRow detailRow = repository.OrderDetails.NewOrderDetailsRow();
                    detailRow.OrdersRow = ordersRow;
                    detailRow.ProductId = detail.ProductId;
                    detailRow.Quantity = detail.Quantity;
                    repository.OrderDetails.AddOrderDetailsRow(detailRow);
                }
            }

            if (order.OrderId <= 0)
                repository.Orders.AddOrdersRow(ordersRow);

            int newOrderId = ordersRow.OrderId;
            return newOrderId;
        }


        public Order GetOrderWithDetails(int orderId)
        {
            OrdersManagementDataSet.OrdersRow ordersRow = repository.Orders.FindByOrderId(orderId);
            if (ordersRow == null)
                return null;

			Order order = TranslateFromOrdersRowToOrderEntityWithDetails(ordersRow);
            return order;

        }

		private Order TranslateFromOrdersRowToOrderEntityWithDetails(OrdersManagementDataSet.OrdersRow ordersRow)
		{
			OrdersManagementDataSet.OrderDetailsRow[] detailRows = ordersRow.GetOrderDetailsRows();
			Order order = new Order();
			TranslateFromOrdersRowToOrderEntity(ordersRow, order);

			order.Details = new List<OrderDetail>(detailRows.Length);
			foreach (OrdersManagementDataSet.OrderDetailsRow detailRow in detailRows)
			{
				Product product = _productService.GetProductById(detailRow.ProductId);
				OrderDetail detail = new OrderDetail(detailRow.OrderId, detailRow.ProductId, detailRow.Quantity, product.UnitPrice.Value);
				order.Details.Add(detail);
			}
			return order;
		}

        public IList<Order> GetOrdersForApprover(string employeeId)
        {
            DataRow[] rows = repository.Orders.Select(String.Format(CultureInfo.CurrentCulture, "OrderStatus = {0} AND Approver = '{1}'", (int)OrderStatusEnum.Submitted, employeeId));
            List<Order> orders = new List<Order>(rows.Length);
            foreach (OrdersManagementDataSet.OrdersRow ordersRow in rows)
            {
				Order order = TranslateFromOrdersRowToOrderEntityWithDetails(ordersRow);
                orders.Add(order);
            }
            return orders;
        }

		public IList<Order> GetSavedDraftOrders(string employeeId)
		{
            Guard.ArgumentNotNullOrEmptyString(employeeId, "employeeId");

			System.Data.DataRow[] rows = repository.Orders.Select(String.Format(CultureInfo.CurrentCulture, "OrderStatus = {0} AND Creator = '{1}'", (int)OrderStatusEnum.Draft, employeeId));
			List<Order> orders = new List<Order>(rows.Length);
			foreach (OrdersManagementDataSet.OrdersRow ordersRow in rows)
			{
				Order order = TranslateFromOrdersRowToOrderEntityWithDetails(ordersRow);
				orders.Add(order);
			}
			return orders;
		}

        public void ApproveOrder(int orderId)
        {
            OrdersManagementDataSet.OrdersRow ordersRow = repository.Orders.FindByOrderId(orderId);
            if (ordersRow == null)
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.OrderIdNotExistsInRepository, orderId));

            if (ordersRow.OrderStatus != (int)OrderStatusEnum.Submitted)
                throw new InvalidOperationException(Resources.OrderStatusInvalidForApproveOrder);

            ordersRow.OrderStatus = (int)OrderStatusEnum.Approved;
        }

		public void RejectOrder(int orderId)
		{
			OrdersManagementDataSet.OrdersRow ordersRow = repository.Orders.FindByOrderId(orderId);
			if (ordersRow == null)
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.OrderIdNotExistsInRepository, orderId));

			if (ordersRow.OrderStatus != (int)OrderStatusEnum.Submitted)
				throw new InvalidOperationException(Resources.OrderStatusInvalidForRejectOrder);

			ordersRow.OrderStatus = (int)OrderStatusEnum.Rejected;
		}

		public void DeleteOrder(int orderId)
		{
			OrdersManagementDataSet.OrdersRow ordersRow = repository.Orders.FindByOrderId(orderId);
			if (ordersRow == null)
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.OrderIdNotExistsInRepository, orderId));

			if (ordersRow.OrderStatus != (int)OrderStatusEnum.Draft)
				throw new InvalidOperationException(Resources.OrderStatusInvalidForDeleteOrder);
			
			OrdersRepository.Services.OrdersManagementDataSet.OrderDetailsRow[] details = ordersRow.GetOrderDetailsRows();
			foreach (OrdersRepository.Services.OrdersManagementDataSet.OrderDetailsRow detail in details)
			{
				repository.OrderDetails.RemoveOrderDetailsRow(detail);
			}

			repository.Orders.RemoveOrdersRow(ordersRow);
		}

        private static void TranslateFromOrderEntityToOrdersRow(Order order, OrdersManagementDataSet.OrdersRow ordersRow)
        {
            ordersRow.CustomerId = order.CustomerId;
            ordersRow.Description = order.Description;
            ordersRow.Approver = order.Approver;
            ordersRow.Creator = order.Creator;
            ordersRow.OrderName = order.OrderName;
            ordersRow.OrderStatus = order.OrderStatus;
            ordersRow.ShipAddress = order.ShipAddress;
            ordersRow.ShipCity = order.ShipCity;
            ordersRow.ShipPostalCode = order.ShipPostalCode;
            ordersRow.ShipRegion = order.ShipRegion;
        }

        public ICollection<Order> SearchOrders(string searchText)
        {
            searchText = InputValidator.EncodeQueryStringParameter(searchText);
            string query = string.Format(CultureInfo.InvariantCulture, "OrderName like '*{0}*' OR Description like '*{0}*'", searchText);
            OrdersManagementDataSet.OrdersRow[] ordersRows = repository.Orders.Select(query) as OrdersManagementDataSet.OrdersRow[];
            ICollection<Order> orderList = new Collection<Order>();
            foreach (OrdersManagementDataSet.OrdersRow row in ordersRows)
            {
                Order order = TranslateFromOrdersRowToOrderEntityWithDetails(row);
                orderList.Add(order);
            }

            return orderList;
        }

        public ICollection<Order> SearchOrders(string searchText, int startOrderIndex, int maximumOrdersCount, out int ordersTotalCount)
        {
            List<Order> orderList = new List<Order>(SearchOrders(searchText));
            ordersTotalCount = orderList.Count;

            if (startOrderIndex < orderList.Count - maximumOrdersCount)
            {
                return orderList.GetRange(startOrderIndex, maximumOrdersCount);
            }
            else
            {
                return orderList.GetRange(startOrderIndex, orderList.Count - startOrderIndex);
            }
        }

        private static void TranslateFromOrdersRowToOrderEntity(OrdersManagementDataSet.OrdersRow ordersRow, Order order)
        {
            order.OrderId = ordersRow.OrderId;
            order.CustomerId = ordersRow.IsCustomerIdNull() ? null : ordersRow.CustomerId;
            order.Description = ordersRow.IsDescriptionNull() ? null : ordersRow.Description;
            order.Approver = ordersRow.Approver;
            order.Creator = ordersRow.Creator;
            order.OrderName = ordersRow.IsOrderNameNull() ? null : ordersRow.OrderName;
            order.OrderStatus = ordersRow.OrderStatus;
            order.ShipAddress = ordersRow.IsShipAddressNull() ? null : ordersRow.ShipAddress;
            order.ShipCity = ordersRow.IsShipCityNull() ? null : ordersRow.ShipCity;
            order.ShipPostalCode = ordersRow.IsShipPostalCodeNull() ? null : ordersRow.ShipPostalCode;
            order.ShipRegion = ordersRow.IsShipRegionNull() ? null : ordersRow.ShipRegion;
        }
	}
}

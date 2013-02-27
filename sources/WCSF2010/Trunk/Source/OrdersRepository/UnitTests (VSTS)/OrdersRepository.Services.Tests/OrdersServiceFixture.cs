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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrdersRepository.BusinessEntities;
using OrderStatusEnum = OrdersRepository.BusinessEntities.Enums.OrderStatus;
using OrdersRepository.Services.Tests.Fakes;

namespace OrdersRepository.Services.Tests
{
    /// <summary>
    /// Summary description for OrdersServiceFixture
    /// </summary>
    [TestClass]
    public class OrdersServiceFixture
    {
        [TestMethod]
        public void CanCreateInstanceWithoutPassingRepositoryToConstructor()
        {
            OrdersService service = new OrdersService(new FakeProductService());
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void SaveAsDraftSetsOrdersStatusToDraft()
        {
            Order order = GetPopulatedOrder();
            order.OrderStatus = 0;
            OrdersService ordersService = new OrdersService(new OrdersManagementDataSet(), new FakeProductService());

            ordersService.SaveAsDraft(order);

            Assert.AreEqual((int) OrderStatusEnum.Draft, order.OrderStatus);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OrderStatusDifferentThanDraftOrNotAssignedThrowsOnSaveAsDraft()
        {
            Order order = GetPopulatedOrder();
            order.OrderStatus = (int)OrderStatusEnum.Submitted;
			OrdersService ordersService = new OrdersService(new OrdersManagementDataSet(), new FakeProductService());

            ordersService.SaveAsDraft(order);
        }

        [TestMethod]
        public void SaveAsDraftAssignsIDWhenNotAssignded()
        {
            Order order = GetPopulatedOrder();
            order.OrderId = 0;
            OrdersManagementDataSet ds = new OrdersManagementDataSet();
			OrdersService ordersService = new OrdersService(ds, new FakeProductService());

            ordersService.SaveAsDraft(order);

            Assert.AreNotEqual(0, order.OrderId);
        }

        [TestMethod]
        public void SaveAsDraftStoresOrderInRepository()
        {
            Order order = GetPopulatedOrder();
            order.OrderId = 0;
            OrdersManagementDataSet ds = new OrdersManagementDataSet();
			OrdersService ordersService = new OrdersService(ds, GetPopulatedProductService());

            ordersService.SaveAsDraft(order);

            OrdersManagementDataSet.OrdersRow row = ds.Orders.FindByOrderId(order.OrderId);
            Assert.IsNotNull(row);
            Assert.AreEqual(1, ds.Orders.Count);
            Assert.AreEqual(order.OrderName, row.OrderName);
            Assert.AreEqual(order.CustomerId, row.CustomerId);
            Assert.AreEqual(order.Description, row.Description);
            Assert.AreEqual(order.Approver, row.Approver);
            Assert.AreEqual(order.OrderStatus, row.OrderStatus);
            Assert.AreEqual(order.ShipAddress, row.ShipAddress);
            Assert.AreEqual(order.ShipCity, row.ShipCity);
            Assert.AreEqual(order.ShipPostalCode, row.ShipPostalCode);
            Assert.AreEqual(order.ShipRegion, row.ShipRegion);
        }

        [TestMethod]
        public void SaveAsDraftCanStoreOrderWithoutDetailsInRepository()
        {
            Order order = new Order();
            order.OrderId = 0;
            order.Approver = "11";
            order.Creator = "11";
            order.Details = null;

            OrdersManagementDataSet ds = new OrdersManagementDataSet();
			OrdersService ordersService = new OrdersService(ds, new FakeProductService());

            ordersService.SaveAsDraft(order);

            OrdersManagementDataSet.OrdersRow row = ds.Orders.FindByOrderId(order.OrderId);
            Assert.IsNotNull(row);
            Assert.AreEqual(1, ds.Orders.Count);
        }

        [TestMethod]
        public void SaveAsDraftWithExistingOrderUpdatesRepository()
        {
            Order order = GetPopulatedOrder();
            order.OrderId = 0;
            OrdersManagementDataSet ds = new OrdersManagementDataSet();
			OrdersService ordersService = new OrdersService(ds, GetPopulatedProductService());
            ordersService.SaveAsDraft(order);
            int orderID = order.OrderId;

            order = GetPopulatedOrder();
            order.OrderId = orderID;
            order.OrderName = "MyNewName";

            ordersService.SaveAsDraft(order);

            OrdersManagementDataSet.OrdersRow row = ds.Orders.FindByOrderId(orderID);
            Assert.IsNotNull(row);
            Assert.AreEqual("MyNewName", row.OrderName);
            Assert.AreEqual(1, ds.Orders.Count);
        }

        [TestMethod]
        public void SaveAsDraftWithNewOrderAddsOrderDetails()
        {
            Order order = GetPopulatedOrder();
            order.OrderId = 0;
            order.Details.Add(new OrderDetail(0, 10, 1, 1.99m));
            int detailsCount = order.Details.Count;

			OrdersManagementDataSet ds = new OrdersManagementDataSet();
			OrdersService ordersService = new OrdersService(ds, new FakeProductService());

            ordersService.SaveAsDraft(order);

            Assert.AreEqual(detailsCount, ds.OrderDetails.Count);
            OrdersManagementDataSet.OrderDetailsRow detailRow = ds.OrderDetails.FindByOrderIdProductId(order.OrderId, 10);
            Assert.IsNotNull(detailRow);
            Assert.AreEqual((short)1, detailRow.Quantity);
        }

        [TestMethod]
        public void SaveAsDraftWithExistingOrderUpdatesDetailsInRepository()
        {
            Order order = GetPopulatedOrder();
            order.OrderId = 0;
            order.Details.Add(new OrderDetail(0, 10, 1, 1.99m));

            OrdersManagementDataSet ds = new OrdersManagementDataSet();
			OrdersService ordersService = new OrdersService(ds, GetPopulatedProductService());
            ordersService.SaveAsDraft(order);
            int orderID = order.OrderId;

            order = GetPopulatedOrder();
            order.OrderId = orderID;
            order.Details.Add(new OrderDetail(0, 20, 3, 2.50m));
            int detailsCount = order.Details.Count;

            ordersService.SaveAsDraft(order);

            Assert.AreEqual(detailsCount, ds.OrderDetails.Count);
            OrdersManagementDataSet.OrderDetailsRow detailRow = ds.OrderDetails.FindByOrderIdProductId(order.OrderId, 20);
            Assert.IsNotNull(detailRow);
            Assert.AreEqual((short)3, detailRow.Quantity);

            OrdersManagementDataSet.OrderDetailsRow deletedDetailRow = ds.OrderDetails.FindByOrderIdProductId(order.OrderId, 10);
            Assert.IsNull(deletedDetailRow);
        }

        [TestMethod]
        public void GetOrderWithDetailsGetsBothOrderAndDetails()
        {
            OrdersManagementDataSet ds = new OrdersManagementDataSet();
            OrdersManagementDataSet.OrdersRow ordersRow = ds.Orders.NewOrdersRow();
            ordersRow.CustomerId = "11";
            ordersRow.Description = "MyDescription";
            ordersRow.Approver = "MyEmployeeId";
            ordersRow.Creator = "MyEmployeeId";
            ordersRow.OrderName = "MyOrderName";
            ordersRow.OrderStatus = (int)OrderStatusEnum.Draft;
            ordersRow.ShipAddress = "MyShipAddress";
            ordersRow.ShipCity = "MyShipCity";
            ordersRow.ShipPostalCode = "MyZip";
            ordersRow.ShipRegion = "MyShipRegion";
            ds.Orders.AddOrdersRow(ordersRow);
            OrdersManagementDataSet.OrderDetailsRow detailRow = ds.OrderDetails.NewOrderDetailsRow();
            detailRow.OrdersRow = ordersRow;
            detailRow.ProductId = 11;
            detailRow.Quantity = 3;
            ds.OrderDetails.AddOrderDetailsRow(detailRow);
            ds.AcceptChanges();

			FakeProductService productService = new FakeProductService();
			productService.Products.Add(new Product(11, null, null, 2.99m, null));

            OrdersService ordersService = new OrdersService(ds, productService);

            Order order = ordersService.GetOrderWithDetails(ordersRow.OrderId);

            Assert.IsNotNull(order);
            Assert.AreEqual(ordersRow.OrderId, order.OrderId);
            Assert.AreEqual("MyDescription", order.Description);
            Assert.IsNotNull(order.Details);
            Assert.AreEqual(1, order.Details.Count);
            Assert.AreEqual((short)3, order.Details[0].Quantity);
            Assert.AreEqual(2.99m, order.Details[0].UnitPrice);
        }

        [TestMethod]
        public void GetOrderWithDetailsReturnsNullWhenOrderNotExists()
        {
            OrdersManagementDataSet ds = new OrdersManagementDataSet();
			OrdersService ordersService = new OrdersService(ds, new FakeProductService());

            Order order = ordersService.GetOrderWithDetails(0);

            Assert.IsNull(order);
        }

        [TestMethod]
        public void SubmitOrderSetsOrdersStatusToSubmitted()
        {
            Order order = GetPopulatedOrder();
            order.OrderStatus = 0;
			OrdersService ordersService = new OrdersService(new OrdersManagementDataSet(), new FakeProductService());

            ordersService.SubmitOrder(order);

            Assert.AreEqual((int)OrderStatusEnum.Submitted, order.OrderStatus);
        }

        [TestMethod]
        public void SubmitOrderSetsOrdersDate()
        {
            Order order = GetPopulatedOrder();
            order.OrderStatus = 0;
			OrdersService ordersService = new OrdersService(new OrdersManagementDataSet(), new FakeProductService());

            ordersService.SubmitOrder(order);

            Assert.IsNotNull(order.OrderDate);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OrderStatusDifferentThanDraftOrNotAssignedThrowsOnSubmitOrder()
        {
            Order order = GetPopulatedOrder();
            order.OrderStatus = (int)OrderStatusEnum.Submitted;
			OrdersService ordersService = new OrdersService(new OrdersManagementDataSet(), new FakeProductService());

            ordersService.SubmitOrder(order);
        }

        [TestMethod]
        public void SubmitOrderAssignsIDWhenNotAssignded()
        {
            Order order = GetPopulatedOrder();
            order.OrderId = 0;
            OrdersManagementDataSet ds = new OrdersManagementDataSet();
			OrdersService ordersService = new OrdersService(ds, new FakeProductService());

            ordersService.SubmitOrder(order);

            Assert.AreNotEqual(0, order.OrderId);
        }

        [TestMethod]
        public void SubmitOrderAddsOrderDetails()
        {
            Order order = GetPopulatedOrder();
            order.OrderId = 0;
            OrdersManagementDataSet ds = new OrdersManagementDataSet();
			OrdersService ordersService = new OrdersService(ds, new FakeProductService());
            order.Details.Add(new OrderDetail(0, 10, 1, 1.99m));
            int detailsCount = order.Details.Count;

            ordersService.SaveAsDraft(order);

            Assert.AreEqual(detailsCount, ds.OrderDetails.Count);
            OrdersManagementDataSet.OrderDetailsRow detailRow = ds.OrderDetails.FindByOrderIdProductId(order.OrderId, 10);
            Assert.IsNotNull(detailRow);
            Assert.AreEqual((short)1, detailRow.Quantity);
        }

        [TestMethod]
        public void SubmitOrderStoresOrderInRepository()
        {
            Order order = GetPopulatedOrder();
            order.OrderId = 0;
            OrdersManagementDataSet ds = new OrdersManagementDataSet();
			OrdersService ordersService = new OrdersService(ds, new FakeProductService());

            ordersService.SubmitOrder(order);

            OrdersManagementDataSet.OrdersRow row = ds.Orders.FindByOrderId(order.OrderId);
            Assert.IsNotNull(row);
            Assert.AreEqual(1, ds.Orders.Count);
            Assert.AreEqual(order.OrderName, row.OrderName);
            Assert.AreEqual(order.CustomerId, row.CustomerId);
            Assert.AreEqual(order.Description, row.Description);
            Assert.AreEqual(order.Approver, row.Approver);
            Assert.AreEqual(order.OrderStatus, row.OrderStatus);
            Assert.AreEqual(order.ShipAddress, row.ShipAddress);
            Assert.AreEqual(order.ShipCity, row.ShipCity);
            Assert.AreEqual(order.ShipPostalCode, row.ShipPostalCode);
            Assert.AreEqual(order.ShipRegion, row.ShipRegion);
        }

        [TestMethod]
        public void GetOrdersForApproverRetrievesOrdersWithSubmittedStatusAndForCurrentUser()
        {
            OrdersManagementDataSet ds = new OrdersManagementDataSet();
            OrdersManagementDataSet.OrdersRow ordersRow = ds.Orders.NewOrdersRow();
            ordersRow.Approver = "CurrentUser";
            ordersRow.Creator = "CurrentUser";
            ordersRow.OrderName = "Test Order";
            ordersRow.OrderStatus = (int)OrderStatusEnum.Submitted;
            ds.Orders.AddOrdersRow(ordersRow);
            OrdersManagementDataSet.OrderDetailsRow detailRow = ds.OrderDetails.NewOrderDetailsRow();
            detailRow.OrdersRow = ordersRow;
            detailRow.ProductId = 11;
            detailRow.Quantity = 3;
            ds.OrderDetails.AddOrderDetailsRow(detailRow);
            ds.AcceptChanges();

			FakeProductService productService = new FakeProductService();
			productService.Products.Add(new Product(11, null, null, 2.99m, null));
            OrdersService ordersService = new OrdersService(ds, productService);

            IList<Order> orders = ordersService.GetOrdersForApprover("CurrentUser");

            Assert.IsNotNull(orders);
            Assert.AreEqual(1, orders.Count);
            Order order = orders[0];
            Assert.AreEqual("Test Order", order.OrderName);
            Assert.IsNotNull(order);
            Assert.AreEqual(1, order.Details.Count);
            Assert.AreEqual((short)3, order.Details[0].Quantity);
            Assert.AreEqual(2.99m, order.Details[0].UnitPrice);
        }

        [TestMethod]
        public void GetOrdersForApproverDoesNotRetrieveOrdersWithoutSubmittedStatus()
        {
            OrdersManagementDataSet ds = new OrdersManagementDataSet();
            OrdersManagementDataSet.OrdersRow ordersRow = ds.Orders.NewOrdersRow();
            ordersRow.Approver = "CurrentUser";
            ordersRow.Creator = "CurrentUser";
            ordersRow.OrderStatus = (int)OrderStatusEnum.Draft;
            ds.Orders.AddOrdersRow(ordersRow);
            ordersRow = ds.Orders.NewOrdersRow();
            ordersRow.Approver = "CurrentUser";
            ordersRow.Creator = "CurrentUser";
            ordersRow.OrderStatus = (int)OrderStatusEnum.Approved;
            ds.Orders.AddOrdersRow(ordersRow);
            ds.AcceptChanges();
			OrdersService ordersService = new OrdersService(ds, new FakeProductService());

            IList<Order> orders = ordersService.GetOrdersForApprover("CurrentUser");

            Assert.IsNotNull(orders);
            Assert.AreEqual(0, orders.Count);
        }

        [TestMethod]
        public void GetOrdersForCurrentApproverDoesNotRetrieveOrderForApproversWhoAreNotTheCurrentLoggedInOne()
        {
            OrdersManagementDataSet ds = new OrdersManagementDataSet();
            OrdersManagementDataSet.OrdersRow ordersRow = ds.Orders.NewOrdersRow();
            ordersRow.Approver = "DifferentUser";
            ordersRow.Creator = "DifferentUser";
            ordersRow.OrderStatus = (int)OrderStatusEnum.Submitted;
            ds.Orders.AddOrdersRow(ordersRow);
            ds.AcceptChanges();
			OrdersService ordersService = new OrdersService(ds, new FakeProductService());

            IList<Order> orders = ordersService.GetOrdersForApprover("CurrentUser");

            Assert.IsNotNull(orders);
            Assert.AreEqual(0, orders.Count);
        }

		[TestMethod]
		public void GetSavedDraftOrdersRetrievesOrdersWithDraftStatusAndForCurrentUser()
		{
			OrdersManagementDataSet ds = new OrdersManagementDataSet();
			OrdersManagementDataSet.OrdersRow ordersRow = ds.Orders.NewOrdersRow();
			ordersRow.Approver = "WhoCares";
			ordersRow.Creator = "CurrentUser";
			ordersRow.OrderName = "Test Order";
			ordersRow.OrderStatus = (int)OrderStatusEnum.Draft;
			ds.Orders.AddOrdersRow(ordersRow);
			OrdersManagementDataSet.OrderDetailsRow detailRow = ds.OrderDetails.NewOrderDetailsRow();
			detailRow.OrdersRow = ordersRow;
			detailRow.ProductId = 11;
			detailRow.Quantity = 3;
			ds.OrderDetails.AddOrderDetailsRow(detailRow);
			ds.AcceptChanges();

			FakeProductService productService = new FakeProductService();
			productService.Products.Add(new Product(11, null, null, 2.99m, null));
			OrdersService ordersService = new OrdersService(ds, productService);

			IList<Order> orders = ordersService.GetSavedDraftOrders("CurrentUser");

			Assert.IsNotNull(orders);
			Assert.AreEqual(1, orders.Count);
			Order order = orders[0];
			Assert.AreEqual("Test Order", order.OrderName);
			Assert.IsNotNull(order);
			Assert.AreEqual(1, order.Details.Count);
			Assert.AreEqual((short)3, order.Details[0].Quantity);
			Assert.AreEqual(2.99m, order.Details[0].UnitPrice);
		}

		[TestMethod]
		public void GetSavedDraftOrdersDoesNotRetrieveOrdersWithoutDraftStatus()
		{
			OrdersManagementDataSet ds = new OrdersManagementDataSet();
			OrdersManagementDataSet.OrdersRow ordersRow = ds.Orders.NewOrdersRow();
			ordersRow.Approver = "WhoCares";
			ordersRow.Creator = "CurrentUser";
			ordersRow.OrderStatus = (int)OrderStatusEnum.Submitted;
			ds.Orders.AddOrdersRow(ordersRow);
			ordersRow = ds.Orders.NewOrdersRow();
			ordersRow.Approver = "WhoCares";
			ordersRow.Creator = "CurrentUser";
			ordersRow.OrderStatus = (int)OrderStatusEnum.Approved;
			ds.Orders.AddOrdersRow(ordersRow);
			ds.AcceptChanges();
			OrdersService ordersService = new OrdersService(ds, new FakeProductService());

			IList<Order> orders = ordersService.GetSavedDraftOrders("CurrentUser");

			Assert.IsNotNull(orders);
			Assert.AreEqual(0, orders.Count);
		}

		[TestMethod]
		public void GetSavedDraftOrdersDoesNotRetrieveOrderForApproversWhoAreNotTheCurrentLoggedInOne()
		{
			OrdersManagementDataSet ds = new OrdersManagementDataSet();
			OrdersManagementDataSet.OrdersRow ordersRow = ds.Orders.NewOrdersRow();
			ordersRow.Approver = "DifferentWhoCares";
			ordersRow.Creator = "DifferentUser";
			ordersRow.OrderStatus = (int)OrderStatusEnum.Draft;
			ds.Orders.AddOrdersRow(ordersRow);
			ds.AcceptChanges();
			OrdersService ordersService = new OrdersService(ds, new FakeProductService());

			IList<Order> orders = ordersService.GetOrdersForApprover("CurrentUser");

			Assert.IsNotNull(orders);
			Assert.AreEqual(0, orders.Count);
		}

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ApproveUnexistingOrderThrowsException()
        {
			OrdersService ordersService = new OrdersService(new OrdersManagementDataSet(), new FakeProductService());

            ordersService.ApproveOrder(1234);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RejectUnexistingOrderThrowsException()
        {
			OrdersService ordersService = new OrdersService(new OrdersManagementDataSet(), new FakeProductService());

            ordersService.RejectOrder(1234);
        }

		[TestMethod]
		public void ApproveOrderSetsOrdersStatusToApproved()
		{
			Order order = GetPopulatedOrder();
			order.OrderStatus = 0;
			OrdersService ordersService = new OrdersService(new OrdersManagementDataSet(), GetPopulatedProductService());

			ordersService.SubmitOrder(order);

			ordersService.ApproveOrder(order.OrderId);

			order = ordersService.GetOrderWithDetails(order.OrderId);

			Assert.AreEqual((int)OrderStatusEnum.Approved, order.OrderStatus);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void OrderStatusDifferentThanSubmittedThrowsOnApproveOrder()
		{
			Order order = GetPopulatedOrder();
			OrdersService ordersService = new OrdersService(new OrdersManagementDataSet(), new FakeProductService());
			ordersService.SaveAsDraft(order);

			ordersService.ApproveOrder(order.OrderId);
		}

		[TestMethod]
		public void RejectOrderSetsOrdersStatusToRejected()
		{
			Order order = GetPopulatedOrder();
			order.OrderStatus = 0;
			OrdersService ordersService = new OrdersService(new OrdersManagementDataSet(), GetPopulatedProductService());

			ordersService.SubmitOrder(order);

			ordersService.RejectOrder(order.OrderId);

			order = ordersService.GetOrderWithDetails(order.OrderId);

			Assert.AreEqual((int)OrderStatusEnum.Rejected, order.OrderStatus);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void OrderStatusDifferentThanSubmittedThrowsOnRejectOrder()
		{
			Order order = GetPopulatedOrder();
			OrdersService ordersService = new OrdersService(new OrdersManagementDataSet(), new FakeProductService());
			ordersService.SaveAsDraft(order);

			ordersService.RejectOrder(order.OrderId);
		}

        [TestMethod]
        public void SearchOrdersRetrievesOrdersByName()
        {
            OrdersManagementDataSet ds = new OrdersManagementDataSet();
            OrdersManagementDataSet.OrdersRow ordersRowA = ds.Orders.NewOrdersRow();
            ordersRowA.OrderId = 1;
            ordersRowA.OrderName = "OrderA";
            ordersRowA.Description = string.Empty;
            ordersRowA.Approver = "User";
            ordersRowA.Creator = "User";
            ordersRowA.OrderStatus = (int)OrderStatusEnum.Submitted;

            ds.Orders.AddOrdersRow(ordersRowA);

            OrdersManagementDataSet.OrdersRow ordersRowB = ds.Orders.NewOrdersRow();
            ordersRowB.OrderId = 2;
            ordersRowB.OrderName = "OrderB";
            ordersRowB.Description = string.Empty;
            ordersRowB.Approver = "User";
            ordersRowB.Creator = "User";
            ordersRowB.OrderStatus = (int)OrderStatusEnum.Submitted;

            ds.Orders.AddOrdersRow(ordersRowB);
            ds.AcceptChanges();


			OrdersService ordersService = new OrdersService(ds, new FakeProductService());

            ICollection<Order> foundOrders = ordersService.SearchOrders("OrderA");

            Assert.AreEqual(1, foundOrders.Count);
            IEnumerator<Order> enumerator = foundOrders.GetEnumerator();
            enumerator.Reset();
            enumerator.MoveNext();
            Assert.AreEqual(1, enumerator.Current.OrderId);
        }

        [TestMethod]
        public void SearchOrdersRetrievesOrdersByDescription()
        {
            OrdersManagementDataSet ds = new OrdersManagementDataSet();
            OrdersManagementDataSet.OrdersRow ordersRowA = ds.Orders.NewOrdersRow();
            ordersRowA.OrderId = 1;
            ordersRowA.OrderName = string.Empty;
            ordersRowA.Description = "Description A";
            ordersRowA.Approver = "User";
            ordersRowA.Creator = "User";
            ordersRowA.OrderStatus = (int)OrderStatusEnum.Submitted;

            ds.Orders.AddOrdersRow(ordersRowA);

            OrdersManagementDataSet.OrdersRow ordersRowB = ds.Orders.NewOrdersRow();
            ordersRowB.OrderId = 2;
            ordersRowB.OrderName = string.Empty;
            ordersRowB.Description = "Description B";
            ordersRowB.Approver = "User";
            ordersRowB.Creator = "User";
            ordersRowB.OrderStatus = (int)OrderStatusEnum.Submitted;

            ds.Orders.AddOrdersRow(ordersRowB);
            ds.AcceptChanges();

			OrdersService ordersService = new OrdersService(ds, new FakeProductService());

            ICollection<Order> foundOrders = ordersService.SearchOrders("Description B");

            Assert.AreEqual(1, foundOrders.Count);
            IEnumerator<Order> enumerator = foundOrders.GetEnumerator();
            enumerator.Reset();
            enumerator.MoveNext();
            Assert.AreEqual(2, enumerator.Current.OrderId);
        }

        [TestMethod]
        public void SearchOrdersDoesALikeMatch()
        {
            OrdersManagementDataSet ds = InitOrdersManagementDataSet();

			OrdersService ordersService = new OrdersService(ds, new FakeProductService());

            ICollection<Order> foundOrders = ordersService.SearchOrders("Description");

            List<Order> searchableList = new List<Order>(foundOrders);

            Assert.AreEqual(2, foundOrders.Count);
            Assert.IsTrue(searchableList.Exists(delegate(Order order) { return order.OrderId == 1; }));
            Assert.IsTrue(searchableList.Exists(delegate(Order order) { return order.OrderId == 2; }));
            Assert.IsFalse(searchableList.Exists(delegate(Order order) { return order.OrderId == 3; }));
        }

        [TestMethod]
        public void SearchOrderRetrievesOrderFromStartOrderIndex()
        {
            OrdersManagementDataSet ds = InitOrdersManagementDataSet();
            OrdersService ordersService = new OrdersService(ds, new FakeProductService());

            int ordersTotalCount;
            ICollection<Order> foundOrders = ordersService.SearchOrders("Order", 2, int.MaxValue, out ordersTotalCount);

            Assert.AreEqual(3, ordersTotalCount);
            Assert.AreEqual(1, foundOrders.Count);
            List<Order> searchableList = new List<Order>(foundOrders);
            Assert.IsFalse(searchableList.Exists(delegate(Order order) { return order.OrderId == 1; }));
            Assert.IsFalse(searchableList.Exists(delegate(Order order) { return order.OrderId == 2; }));
            Assert.IsTrue(searchableList.Exists(delegate(Order order) { return order.OrderId == 3; }));
        }

		[TestMethod]
		public void SearchOrderNotExceedMaximumOrdersCount()
		{
			OrdersManagementDataSet ds = InitOrdersManagementDataSet();
			OrdersService ordersService = new OrdersService(ds, new FakeProductService());

			int ordersTotalCount;
			ICollection<Order> foundOrders = ordersService.SearchOrders("Order", 0, 2, out ordersTotalCount);

			Assert.AreEqual(3, ordersTotalCount);
			Assert.AreEqual(2, foundOrders.Count);
            List<Order> searchableList = new List<Order>(foundOrders);
            Assert.IsTrue(searchableList.Exists(delegate(Order order) { return order.OrderId == 1; }));
            Assert.IsTrue(searchableList.Exists(delegate(Order order) { return order.OrderId == 2; }));
            Assert.IsFalse(searchableList.Exists(delegate(Order order) { return order.OrderId == 3; }));
		}

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SearchOrderThrowsArgumentExceptionWhenStartOrderIndexIsTooLarge()
        {
            OrdersManagementDataSet ds = InitOrdersManagementDataSet();
            OrdersService ordersService = new OrdersService(ds, new FakeProductService());

            int ordersTotalCount;
            ICollection<Order> foundOrders = ordersService.SearchOrders("Order", 4, int.MaxValue, out ordersTotalCount);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SearchOrderThrowsArgumentExceptionWhenStartOrderIndexIsNegative()
        {
            OrdersManagementDataSet ds = InitOrdersManagementDataSet();

            OrdersService ordersService = new OrdersService(ds, new FakeProductService());

            int ordersTotalCount;
            ICollection<Order> foundOrders = ordersService.SearchOrders("Order", -1, int.MaxValue, out ordersTotalCount);
        }

        private static OrdersManagementDataSet InitOrdersManagementDataSet()
        {
            OrdersManagementDataSet ds = new OrdersManagementDataSet();
            OrdersManagementDataSet.OrdersRow ordersRowA = ds.Orders.NewOrdersRow();
            ordersRowA.OrderId = 1;
            ordersRowA.OrderName = "Order A";
            ordersRowA.Description = "Description A";
            ordersRowA.Approver = "User";
            ordersRowA.Creator = "User";
            ordersRowA.OrderStatus = (int)OrderStatusEnum.Submitted;

            ds.Orders.AddOrdersRow(ordersRowA);

            OrdersManagementDataSet.OrdersRow ordersRowB = ds.Orders.NewOrdersRow();
            ordersRowB.OrderId = 2;
            ordersRowB.OrderName = "Order B";
            ordersRowB.Description = "Description B";
            ordersRowB.Approver = "User";
            ordersRowB.Creator = "User";
            ordersRowB.OrderStatus = (int)OrderStatusEnum.Submitted;

            ds.Orders.AddOrdersRow(ordersRowB);

            OrdersManagementDataSet.OrdersRow ordersRowC = ds.Orders.NewOrdersRow();
            ordersRowC.OrderId = 3;
            ordersRowC.OrderName = "Order C";
            ordersRowC.Description = "C";
            ordersRowC.Approver = "User";
            ordersRowC.Creator = "User";
            ordersRowC.OrderStatus = (int)OrderStatusEnum.Submitted;

            ds.Orders.AddOrdersRow(ordersRowC);
            ds.AcceptChanges();
            return ds;
        }

		[TestMethod]
		public void DeleteRemovesOrderFromRepository()
		{
			OrdersManagementDataSet ds = new OrdersManagementDataSet();
			OrdersManagementDataSet.OrdersRow ordersRowA = ds.Orders.NewOrdersRow();
			ordersRowA.OrderId = 1;
			ordersRowA.OrderName = "Order A";
			ordersRowA.Description = "Description A";
			ordersRowA.Approver = "User";
			ordersRowA.Creator = "User";
			ordersRowA.OrderStatus = (int)OrderStatusEnum.Draft;
			ds.Orders.AddOrdersRow(ordersRowA);

			ds.AcceptChanges();

			OrdersService ordersService = new OrdersService(ds, new FakeProductService());

			ordersService.DeleteOrder(1);

			Assert.AreEqual(0, ds.Orders.Count);
		}

		[TestMethod]
		public void DeleteRemovesOnlyExpectedOrderFromRepository()
		{
			OrdersManagementDataSet ds = new OrdersManagementDataSet();
			OrdersManagementDataSet.OrdersRow ordersRowA = ds.Orders.NewOrdersRow();
			ordersRowA.OrderId = 3;
			ordersRowA.OrderName = "Order A";
			ordersRowA.Description = "Description A";
			ordersRowA.Approver = "User";
			ordersRowA.Creator = "User";
			ordersRowA.OrderStatus = (int)OrderStatusEnum.Draft;
			ds.Orders.AddOrdersRow(ordersRowA);

			OrdersManagementDataSet.OrdersRow ordersRowB = ds.Orders.NewOrdersRow();
			ordersRowB.OrderId = 2;
			ordersRowB.OrderName = "Order B";
			ordersRowB.Description = "Description B";
			ordersRowB.Approver = "User";
			ordersRowB.Creator = "User";
			ordersRowB.OrderStatus = (int)OrderStatusEnum.Submitted;
			ds.Orders.AddOrdersRow(ordersRowB);

			ds.AcceptChanges();

			OrdersService ordersService = new OrdersService(ds, new FakeProductService());

			ordersService.DeleteOrder(3);

			Assert.AreEqual(1, ds.Orders.Count);
			Assert.AreEqual(2, ds.Orders[0].OrderId);
		}

		[TestMethod]
		public void DeleteRemovesOrderDetailsFromRepository()
		{
			OrdersManagementDataSet ds = new OrdersManagementDataSet();
			OrdersManagementDataSet.OrdersRow ordersRow = ds.Orders.NewOrdersRow();
			ordersRow.OrderId = 2;
			ordersRow.Approver = "CurrentUser";
			ordersRow.Creator = "CurrentUser";
			ordersRow.OrderName = "Test Order";
			ordersRow.OrderStatus = (int)OrderStatusEnum.Draft;
			ds.Orders.AddOrdersRow(ordersRow);
			OrdersManagementDataSet.OrderDetailsRow detailRow = ds.OrderDetails.NewOrderDetailsRow();
			detailRow.OrdersRow = ordersRow;
			detailRow.ProductId = 11;
			detailRow.Quantity = 3;
			ds.OrderDetails.AddOrderDetailsRow(detailRow);
			ds.AcceptChanges();
			OrdersService ordersService = new OrdersService(ds, new FakeProductService());

			ordersService.DeleteOrder(2);

			Assert.AreEqual(0, ds.Orders.Count);
			Assert.AreEqual(0, ds.OrderDetails.Count);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void DeleteUnexistingOrderThrowsException()
		{
			OrdersService ordersService = new OrdersService(new OrdersManagementDataSet(), new FakeProductService());

			ordersService.DeleteOrder(1234);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void DeleteNonDraftOrderThrowsException()
		{
			OrdersManagementDataSet ds = new OrdersManagementDataSet();
			OrdersManagementDataSet.OrdersRow ordersRowA = ds.Orders.NewOrdersRow();
			ordersRowA.OrderId = 1;
			ordersRowA.OrderName = "Order A";
			ordersRowA.Description = "Description A";
			ordersRowA.Approver = "User";
			ordersRowA.Creator = "User";
			ordersRowA.OrderStatus = (int)OrderStatusEnum.Submitted;
			ds.Orders.AddOrdersRow(ordersRowA);

			ds.AcceptChanges();

			OrdersService ordersService = new OrdersService(ds, new FakeProductService());

			ordersService.DeleteOrder(1);
		}

        [TestMethod]
        public void SearchOrdersWithQuotesSearchesCorrectlyAndPreventsInjection()
        {
            OrdersManagementDataSet ds = new OrdersManagementDataSet();
            OrdersManagementDataSet.OrdersRow ordersRowA = ds.Orders.NewOrdersRow();
            ordersRowA.OrderId = 1;
            ordersRowA.OrderName = "Order A's";
            ordersRowA.Description = "Description A";
            ordersRowA.Approver = "User";
            ordersRowA.Creator = "User";
            ordersRowA.OrderStatus = (int)OrderStatusEnum.Draft;
            ds.Orders.AddOrdersRow(ordersRowA);

            OrdersManagementDataSet.OrdersRow ordersRowB = ds.Orders.NewOrdersRow();
            ordersRowB.OrderId = 2;
            ordersRowB.OrderName = "Order B";
            ordersRowB.Description = "Description B";
            ordersRowB.Approver = "User";
            ordersRowB.Creator = "User";
            ordersRowB.OrderStatus = (int)OrderStatusEnum.Draft;
            ds.Orders.AddOrdersRow(ordersRowB);
            ds.AcceptChanges();

            OrdersService ordersService = new OrdersService(ds, new FakeProductService());

            ICollection<Order> orders = ordersService.SearchOrders("'");

            Assert.AreEqual(1, orders.Count);
            IEnumerator<Order> enumerator = orders.GetEnumerator();
            enumerator.Reset();
            enumerator.MoveNext();
            Assert.AreEqual(1, enumerator.Current.OrderId);
        }
		

        public static Order GetPopulatedOrder()
        {
            Order order = new Order();
            order.Approver = "11";
            order.Creator = "11";
            order.CustomerId = "100";
            order.ShipAddress = "225 112th NE Street";
            order.ShipCity = "Bellevue";
            order.ShipRegion = "Washington";
            order.ShipPostalCode = "98005";
            order.OrderName = "ORDER1";
            order.Description = "Needs it quick";

            order.Details = new List<OrderDetail>();
            order.Details.Add(new OrderDetail(0, 1, 2, 5.95m));
            order.Details.Add(new OrderDetail(0, 2, 4, 1.25m));
            return order;
        }

		public static FakeProductService GetPopulatedProductService()
		{
			FakeProductService productService = new FakeProductService();
			productService.Products.Add(new Product(1, null, null, 5.95m, null));
			productService.Products.Add(new Product(2, null, null, 1.25m, null));
			return productService;
		}

        
    }
}

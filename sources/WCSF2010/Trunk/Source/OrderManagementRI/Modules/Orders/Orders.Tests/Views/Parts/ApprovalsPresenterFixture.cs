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
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderManagement.Orders.Views.Parts;
using OrderManagement.Orders.Tests.Mocks;
using OrderManagement.Orders.Converters;
using OrdersRepository.BusinessEntities;
using Orders.PresentationEntities;
using System.Security.Principal;
using OrderStatusEnum = OrdersRepository.BusinessEntities.Enums.OrderStatus;

namespace OrderManagement.Orders.Tests
{
    /// <summary>
    /// Summary description for ApprovalsPresenterTestFixture
    /// </summary>
    [TestClass]
    public class ApprovalsPresenterTestFixture
    {
        ApprovalsPresenter presenter;
        MockApprovals view;
        MockOrdersService ordersService;
        IBusinessPresentationConverter<Order, OrderInfo> ordersConverter;
        MockHttpContextLocatorService httpContextLocatorService;

        [TestInitialize]
        public void InitMVP()
        {
            view = new MockApprovals();
            ordersService = new MockOrdersService();
            ordersConverter = new OrdersConverter(new MockCustomerService(), new MockEmployeeService(), new MockGenericConverter<Employee,EmployeeDisplay>());
            httpContextLocatorService = new MockHttpContextLocatorService();
            presenter = new ApprovalsPresenter(ordersService, ordersConverter, httpContextLocatorService);

            presenter.View = view;
        }

        [TestMethod]
        public void OnInitializedGetsPendingApprovalOrdersForCurrentLoggedApprover()
        {
            httpContextLocatorService.MockHttpContext.User = new GenericPrincipal(new GenericIdentity("CurrentUser"), null);
            ordersService.GetOrdersForApproverList = new List<Order>();

            presenter.OnViewInitialized();

            Assert.IsTrue(ordersService.GetOrdersForApproverCalled);
            Assert.AreEqual("CurrentUser", ordersService.GetOrdersForApproverArgumentValue);
        }

        [TestMethod]
        public void OnInitializedSetsOrdersInView()
        {
            httpContextLocatorService.MockHttpContext.User = new GenericPrincipal(new GenericIdentity("empl1"), null);
            ordersService.GetOrdersForApproverList = new List<Order>();
            ordersService.GetOrdersForApproverList.Add(new Order(1, "name1", "cust1", "empl1", "desc1", (int)OrderStatusEnum.Submitted, null, null, null, null, null, null, null));
            ordersService.GetOrdersForApproverList.Add(new Order(2, "name2", "cust2", "empl1", "desc2", (int)OrderStatusEnum.Submitted, null, null, null, null, null, null, null));
            Order orderWithDetails = new Order(3, "name3", "cust3", "empl1", "desc3", (int)OrderStatusEnum.Submitted, null, null, null, null, null, null, null);
            orderWithDetails.Details.Add(new OrderDetail(3, 1, 2, 2.25m));
            ordersService.GetOrdersForApproverList.Add(orderWithDetails);

            presenter.OnViewInitialized();

            Assert.IsNotNull(view.Orders);
            Assert.AreEqual(3, view.Orders.Count);

            IEnumerator<OrderInfo> enumerator = view.Orders.GetEnumerator();
            enumerator.Reset();
            enumerator.MoveNext();
            Assert.AreEqual("1", enumerator.Current.OrderId);
            Assert.AreEqual("name1", enumerator.Current.OrderName);
            enumerator.MoveNext();
            Assert.AreEqual("desc2", enumerator.Current.Description);
            enumerator.MoveNext();
            Assert.AreEqual(4.50m, enumerator.Current.OrderTotal);
        }

		[TestMethod]
		public void OnApproveOrderUseOrdersServiceAndReloadOrders()
		{
			httpContextLocatorService.MockHttpContext.User = new GenericPrincipal(new GenericIdentity("empl1"), null);
			ordersService.GetOrdersForApproverList = new List<Order>();
            ordersService.GetOrdersForApproverList.Add(new Order(1, "name1", "cust1", "empl1", "desc1", (int)OrderStatusEnum.Submitted, null, null, null, null, null, null, null));
            ordersService.GetOrdersForApproverList.Add(new Order(2, "name2", "cust2", "empl1", "desc2", (int)OrderStatusEnum.Submitted, null, null, null, null, null, null, null));
            Order orderWithDetails = new Order(3, "name3", "cust3", "empl1", "desc3", (int)OrderStatusEnum.Submitted, null, null, null, null, null, null, null);
			orderWithDetails.Details.Add(new OrderDetail(3, 1, 2, 2.25m));
			ordersService.GetOrdersForApproverList.Add(orderWithDetails);

			presenter.OnViewInitialized();

			// Set expected orders before calling ApproveOrder
			ordersService.GetOrdersForApproverList.RemoveAt(1);
			ordersService.GetOrdersForApproverCalled = false;
			view.OrdersAssigned = false;

			presenter.OnApproveOrder("2");

			Assert.IsTrue(ordersService.ApproveOrderCalled);
			Assert.AreEqual(2, ordersService.ApproveOrderArgumentValue);
			Assert.IsTrue(ordersService.GetOrdersForApproverCalled);
			Assert.IsTrue(view.OrdersAssigned);
			Assert.AreEqual(2, view.Orders.Count);

            IEnumerator<OrderInfo> enumerator = view.Orders.GetEnumerator();
            enumerator.Reset();
            enumerator.MoveNext();
            Assert.AreEqual("1", enumerator.Current.OrderId);
            enumerator.MoveNext();
            Assert.AreEqual("3", enumerator.Current.OrderId);
		}

		[TestMethod]
		public void OnRejectOrderUseOrdersServiceAndReloadOrders()
		{
			httpContextLocatorService.MockHttpContext.User = new GenericPrincipal(new GenericIdentity("empl1"), null);
			ordersService.GetOrdersForApproverList = new List<Order>();
            ordersService.GetOrdersForApproverList.Add(new Order(1, "name1", "cust1", "empl1", "desc1", (int)OrderStatusEnum.Submitted, null, null, null, null, null, null, null));
            ordersService.GetOrdersForApproverList.Add(new Order(2, "name2", "cust2", "empl1", "desc2", (int)OrderStatusEnum.Submitted, null, null, null, null, null, null, null));
            Order orderWithDetails = new Order(3, "name3", "cust3", "empl1", "desc3", (int)OrderStatusEnum.Submitted, null, null, null, null, null, null, null);
			orderWithDetails.Details.Add(new OrderDetail(3, 1, 2, 2.25m));
			ordersService.GetOrdersForApproverList.Add(orderWithDetails);

			presenter.OnViewInitialized();

			// Set expected orders before calling RejectOrder
			ordersService.GetOrdersForApproverList.RemoveAt(1);
			ordersService.GetOrdersForApproverCalled = false;
			view.OrdersAssigned = false;

			presenter.OnRejectOrder("2");

			Assert.IsTrue(ordersService.RejectOrderCalled);
			Assert.AreEqual(2, ordersService.RejectOrderArgumentValue);
			Assert.IsTrue(ordersService.GetOrdersForApproverCalled);
			Assert.IsTrue(view.OrdersAssigned);
			Assert.AreEqual(2, view.Orders.Count);

            IEnumerator<OrderInfo> enumerator = view.Orders.GetEnumerator();
            enumerator.Reset();
            enumerator.MoveNext();
            Assert.AreEqual("1", enumerator.Current.OrderId);
            enumerator.MoveNext();
            Assert.AreEqual("3", enumerator.Current.OrderId);
		}

        [TestMethod]
        public void ShouldShowOrderDetailsOnDetailsRequested()
        {
            ordersService.Order = new Order(1, "OrderName", "CustomerId", "EmployeeId", "Description", (int)OrderStatusEnum.Draft, null, null, null, null, null, null, null);

            presenter.OnOrderDetailsRequested("1");

            Assert.IsTrue(view.ShowOrderDetailsCalled);
            Assert.IsNotNull(view.OrderShown);
            Assert.AreEqual(1, view.OrderShown.OrderId);
        }
    }

    class MockApprovals : IApprovals
    {
        private ICollection<OrderInfo> _orders;
        public bool OrdersAssigned;
        public bool ShowOrderDetailsCalled;
        public Order OrderShown;

        public void ShowOrders(ICollection<OrderInfo> orders)
        {
            OrdersAssigned = true;
            _orders = orders;
        }

        public ICollection<OrderInfo> Orders
        {
            get { return _orders; }
        }

        public void ShowOrderDetails(Order order)
        {
            ShowOrderDetailsCalled = true;
            OrderShown = order;
        }
    }
}


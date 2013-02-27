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
using OrderManagement.Orders.Views;
using OrdersRepository.BusinessEntities;
using System.Collections.ObjectModel;
using OrderManagement.Orders.Tests.Mocks;
using OrderManagement.Orders.Converters;
using Orders.PresentationEntities;

namespace OrderManagement.Orders.Tests
{
    /// <summary>
    /// Summary description for SearchOrdersPresenterTestFixture
    /// </summary>
    [TestClass]
    public class SearchOrdersPresenterTestFixture
    {
        private SearchOrdersPresenter presenter;
        private MockOrdersService ordersService;
        private MockSearchOrders view;
        private IBusinessPresentationConverter<Order, OrderInfo> orderInfoConverter;

        [TestInitialize]
        public void InitMVP()
        {
            view = new MockSearchOrders();
            ordersService = new MockOrdersService();

            orderInfoConverter = new OrdersConverter(new MockCustomerService(), new MockEmployeeService(), new MockGenericConverter<Employee, EmployeeDisplay>());

            presenter = new SearchOrdersPresenter(ordersService, orderInfoConverter);
            presenter.View = view;
        }

        [TestMethod]
        public void ShouldHidePagesPanelOnViewLoaded()
        {
            presenter.OnViewLoaded();

            Assert.IsTrue(view.HidePagesPanelCalled);
        }

        [TestMethod]
        public void ShouldResetCurrentPageAndShowSearchTextOnSearchTextChanged()
        {
            string searchText = "Order";
            InitializeOrdersForSearchOrdersMethod();
            view.ShowCurrentPage(4);

            presenter.OnSearchTextChanged(searchText);

            Assert.AreEqual(0, view.CurrentPage);
            Assert.IsTrue(view.ShowSearchTextCalled);
        }

        [TestMethod]
        public void ShouldShowOrdersAndSetTotalCountOnOrdersSelecting()
        {
            string searchText = "Order";
            InitializeOrdersForSearchOrdersMethod();

            presenter.OnOrdersSelecting(searchText, 2, 5);

            Assert.IsTrue(ordersService.SearchOrdersOverloadCalled);
            Assert.IsTrue(view.ShowOrdersCalled);
            Assert.IsTrue(view.TotalOrdersCountSet);
            Assert.AreEqual(ordersService.OrdersForSearchOrders.Count, view.TotalOrdersCount);
            Assert.IsTrue(view.ShowPagesCountersCalled);
            Assert.IsTrue(view.ShowSearchTextCalled);
        }

        [TestMethod]
        public void ShouldEmptyOrderResultsOnEmptySearchText()
        {
            presenter.OnOrdersSelecting(string.Empty, 0, int.MaxValue);

            Assert.IsTrue(view.ShowOrdersCalled);
            Assert.IsTrue(view.TotalOrdersCountSet);
            Assert.IsTrue(view.HidePagesPanelCalled);
        }

        [TestMethod]
        public void ShouldShowOrderDetailsOnDetailsRequested()
        {
            ordersService.Order = new Order(1, "OrderName", "CustomerId", "EmployeeId", "Description", 1, null, null, null, null, null, null, null);

            presenter.OnOrderDetailsRequested("1");

            Assert.IsTrue(view.ShowOrderDetailsCalled);
            Assert.IsNotNull(view.OrderShown);
            Assert.AreEqual(1, view.OrderShown.OrderId);
        }

        [TestMethod]
        public void SetViewCurrentPageOnGoToPage()
        {
            view.PageCount = 3;

            presenter.OnGoToPage(1);

            Assert.AreEqual(1, view.CurrentPage);
        }

        [TestMethod]
        public void DoesNotSetViewCurrentPageOnInvalidGoToPageValue()
        {
            view.ShowCurrentPage(0);
            view.PageCount = 2;

            // pageIndex too large.
            presenter.OnGoToPage(3); 
            Assert.AreEqual(0, view.CurrentPage);

            // negative pageIndex.
            presenter.OnGoToPage(-1); 
            Assert.AreEqual(0, view.CurrentPage);

            presenter.OnGoToPage(1);
            Assert.AreEqual(1, view.CurrentPage);
        }

        private void InitializeOrdersForSearchOrdersMethod()
        {
            ordersService.OrdersForSearchOrders = new Collection<Order>();
            ordersService.OrdersForSearchOrders.Add(new Order());
            ordersService.OrdersForSearchOrders.Add(new Order());
            ordersService.OrdersForSearchOrders.Add(new Order());
            ordersService.OrdersForSearchOrders.Add(new Order());

            ordersService.OrdersForSearchOrders.Add(new Order());
            ordersService.OrdersForSearchOrders.Add(new Order());
            ordersService.OrdersForSearchOrders.Add(new Order());
            ordersService.OrdersForSearchOrders.Add(new Order());

            ordersService.OrdersForSearchOrders.Add(new Order());
            ordersService.OrdersForSearchOrders.Add(new Order());
            ordersService.OrdersForSearchOrders.Add(new Order());
            ordersService.OrdersForSearchOrders.Add(new Order());
        }
    }

    class MockSearchOrders : ISearchOrders
    {
        private int _totalOrdersCount;
        private int _pageCount;
        private int _currentPage;

        public bool ShowOrdersCalled;
        public bool HidePagesPanelCalled;
        public bool ShowPagesCountersCalled;
        public bool ShowSearchTextCalled;
        public bool ShowOrderDetailsCalled;
        public bool TotalOrdersCountSet;
        public bool MaximumRowsPerPageRequested;

        public ICollection<OrderInfo> Orders;
        public Order OrderShown;
        public void ShowOrders(ICollection<OrderInfo> orders)
        {
            ShowOrdersCalled = true;
            Orders = orders;
        }

        public void HidePagesPanel()
        {
            HidePagesPanelCalled = true;
        }

        public void ShowPagesCounters()
        {
            ShowPagesCountersCalled = true;
        }

        public void ShowSearchText(string searchText)
        {
            ShowSearchTextCalled = true;
        }

        public void ShowOrderDetails(Order order)
        {
            ShowOrderDetailsCalled = true;
            OrderShown = order;
        }

        public int TotalOrdersCount
        {
            set
            {
                TotalOrdersCountSet = true;
                _totalOrdersCount = value;
            }
            get
            {
                return _totalOrdersCount;
            }
        }

        public int MaximumRowsPerPage
        {
            get
            {
                MaximumRowsPerPageRequested = true;
                return 10;
            }
        }

        public int PageCount
        {
            get { return _pageCount; }
            set { _pageCount = value; }
        }

        public void ShowCurrentPage(int currentPage)
        {
            _currentPage = currentPage;
        }

        public int CurrentPage
        {
            get { return _currentPage; }
        }

    }
}


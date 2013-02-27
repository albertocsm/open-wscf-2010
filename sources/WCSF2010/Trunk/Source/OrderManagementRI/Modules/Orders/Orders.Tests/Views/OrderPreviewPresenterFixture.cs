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
using Orders.PresentationEntities;
using OrderManagement.Orders.Tests.Mocks;
using OrderManagement.Orders.Converters;
using OrdersRepository.BusinessEntities;
using OrderStatusEnum = OrdersRepository.BusinessEntities.Enums.OrderStatus;

namespace OrderManagement.Orders.Tests
{
    /// <summary>
    /// Summary description for OrderPreviewPresenterTestFixture
    /// </summary>
    [TestClass]
    public class OrderPreviewPresenterTestFixture
    {
        MockOrderPreviewPart view;
        OrderPreviewPresenter presenter;
        MockProductService productService;
        MockCustomerService customerService;
        MockEmployeeService employeeService;
        IBusinessPresentationConverter<Order, OrderInfo> ordersConverter;
        IBusinessPresentationConverter<Employee, EmployeeDisplay> employeeConverter;
        private IBusinessPresentationConverter<OrderDetail, OrderItemLine> orderDetailsConverter;

        [TestInitialize]
        public void InitMVP()
        {
            productService = new MockProductService();
            customerService = new MockCustomerService();
            employeeService = new MockEmployeeService();
            employeeConverter = new MockGenericConverter<Employee, EmployeeDisplay>();
            ordersConverter = new MockGenericConverter<Order, OrderInfo>();
            orderDetailsConverter = new OrderDetailsConverter(productService);
            view = new MockOrderPreviewPart();
            presenter = new OrderPreviewPresenter(ordersConverter, orderDetailsConverter);
            presenter.View = view;
        }

        [TestMethod]
        public void ShouldShowOrderOnInitialize()
        {
            Order order = new Order(1, "OrderName", "CustomerId", "EmployeeId", "Description", (int)OrderStatusEnum.Draft, null, null, null, null, null, null, null);
            order.Details.Add(new OrderDetail(1, 1, 10, 15.0m));
            order.Details.Add(new OrderDetail(1, 2, 20, 30.5m));
            productService.AddProduct(new Product(1, "1111-11111", "Product A", 15.0m, null));
            productService.AddProduct(new Product(2, "2222-22222", "Product B", 30.5m, null));

            presenter.OnShowOrder(order);

            Assert.IsTrue(view.ShowOrderInfoCalled);
            Assert.IsNotNull(view.OrderInfoShown);
            Assert.IsNotNull(view.OrderItemLinesShown);
            Assert.AreEqual(2, view.OrderItemLinesShown.Count);
        }
    }


    class MockOrderPreviewPart : IOrderPreview
    {
        public bool ShowOrderInfoCalled;
        public OrderInfo OrderInfoShown;
        public IList<OrderItemLine> OrderItemLinesShown;

        public void ShowOrder(Order order)
        {
            ShowOrderInfoCalled = true;
        }

        public void ShowOrderInfo(OrderInfo order, IList<OrderItemLine> orderItemLines)
        {
            ShowOrderInfoCalled = true;
            OrderInfoShown = order;
            OrderItemLinesShown = orderItemLines;
        }

    }
}


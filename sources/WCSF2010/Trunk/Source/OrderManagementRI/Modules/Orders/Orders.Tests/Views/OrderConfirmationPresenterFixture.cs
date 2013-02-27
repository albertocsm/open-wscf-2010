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
using OrderManagement.Orders.Tests.Mocks;
using OrdersRepository.BusinessEntities;

namespace OrderManagement.Orders.Tests
{
    /// <summary>
    /// Summary description for ConfirmationPresenterTestFixture
    /// </summary>
    [TestClass]
    public class ConfirmationPresenterTestFixture
    {
        OrderConfirmationPresenter presenter;
        MockConfirmationView view;
        MockOrdersController controller;

        [TestInitialize]
        public void InitMVP()
        {
            view = new MockConfirmationView();
            controller = new MockOrdersController();
            presenter = new OrderConfirmationPresenter(controller);

            presenter.View = view;
        }

        [TestMethod]
        public void OnViewInitializedCompletesFlow()
        {
            presenter.OnViewInitialized();

            Assert.IsTrue(controller.CreateOrderCompleteCalled);
        }

        [TestMethod]
        public void OnViewInitializedDisplaysConfirmationMessage()
        {
            controller.CurrentOrder = new Order(1234, "Name", null, null, null, 1, null, null, null, null, null, null, null);

            presenter.OnViewInitialized();

            Assert.IsNotNull(view.ConfirmationMessage);
            Assert.IsTrue(view.ConfirmationMessage.Contains("1234"));
        }

        [TestMethod]
        public void ShouldCallStartCreateOrderFlowOnCreateNewOrder()
        {
            presenter.CreateNewOrder();

            Assert.IsTrue(controller.StartCreateOrderFlowCalled);
        }

    }

    class MockConfirmationView : IOrderConfirmation
    {
        public string ConfirmationMessage = null;
        public void ShowConfirmationMessage(string message)
        {
            ConfirmationMessage = message;
        }

    }
}


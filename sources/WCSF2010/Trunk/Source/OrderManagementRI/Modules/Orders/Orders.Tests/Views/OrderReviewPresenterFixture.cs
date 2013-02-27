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
using OrderManagement.Orders.Tests.Mocks;
using OrderStatusEnum = OrdersRepository.BusinessEntities.Enums.OrderStatus;

namespace OrderManagement.Orders.Tests
{
    /// <summary>
    /// Summary description for ReviewPresenterTestFixture
    /// </summary>
    [TestClass]
    public class ReviewPresenterTestFixture
    {
        MockPreview view;
        MockOrdersController controller;
        OrderReviewPresenter presenter;

        [TestInitialize]
        public void InitMVP()
        {
            controller = new MockOrdersController();
            view = new MockPreview();
            presenter = new OrderReviewPresenter(controller);
            presenter.View = view;
        }

        [TestMethod]
        public void ShouldShowOrderOnInitialize()
        {
            controller.CurrentOrder = new Order(1, "OrderName", "CustomerId", "EmployeeId", "Description", (int)OrderStatusEnum.Draft, null, null, null, null, null, null, null);

            presenter.OnViewInitialized();

            Assert.IsTrue(view.ShowOrderCalled);
            Assert.IsNotNull(view.OrderShown);
            Assert.AreEqual(controller.CurrentOrder, view.OrderShown);
        }

        [TestMethod]
        public void ShouldSetConfirmationTextOnInitialize()
        {
            view.SetConfirmationText(null);

            presenter.OnViewInitialized();

            Assert.IsNotNull(view.ConfirmationTextShown);
        }

        [TestMethod]
        public void OnPreviousCallsNavigatePreviousFromPreviewView()
        {
            Assert.IsFalse(controller.NavigatePreviousFromPreviewViewCalled);

            presenter.OnPrevious();

            Assert.IsTrue(controller.NavigatePreviousFromPreviewViewCalled);
        }

        [TestMethod]
        public void OnSubmitCallsSubmitOrderOnController()
        {
            presenter.OnSubmit();

            Assert.IsTrue(controller.SubmitOrderCalled);
        }

        [TestMethod]
        public void OnSubmitCallsNavigatesToConfirmation()
        {
            presenter.OnSubmit();

            Assert.IsTrue(controller.NavigateToConfirmationViewCalled);
        }

        [TestMethod]
        public void ConstructorCallsVerifyOrderEntryIsStarted()
        {
            Assert.IsTrue(controller.VerifyOrderEntryFlowIsStartedCalled);
        }
    }

    class MockPreview : IOrderReview
    {
        public bool ShowOrderCalled;
        public Order OrderShown;
        public string ConfirmationTextShown;

        public void ShowOrder(Order order)
        {
            ShowOrderCalled = true;
            OrderShown = order;
        }

        public void SetConfirmationText(string confirmationText)
        {
            ConfirmationTextShown = confirmationText;
        }
    }
}


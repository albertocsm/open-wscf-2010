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
using System.Security.Principal;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderManagement.Orders.Services;
using OrderManagement.Orders.Tests.Mocks;
using OrdersRepository.BusinessEntities;
using OrderStatusEnum = OrdersRepository.BusinessEntities.Enums.OrderStatus;

namespace OrderManagement.Orders.Tests
{
    [TestClass]
    public class OrdersModuleControllerFixture
    {
        public OrdersModuleControllerFixture()
        {
        }

        [TestMethod]
        public void OnStartLoadOrderFlowStoresOrderInCurrentOrder()
        {
            TestableOrdersController controller = GetOrderControllerInitialized();
            controller.MockOrdersService.Order = GetPopulatedOrder();
            controller.MockHttpContextLocatorService.GetCurrentContext().User = new GenericPrincipal(new GenericIdentity("CurrentUser"), null);

            controller.StartLoadOrderFlow(controller.MockOrdersService.Order.OrderId);

            Assert.IsTrue(controller.MockOrdersService.GetOrderWithDetailsCalled);
            Assert.IsNotNull(controller.CurrentOrder);
            Assert.AreEqual(controller.MockOrdersService.Order, controller.CurrentOrder);
        }

        [TestMethod]
        public void SaveOrderCallsRepository()
        {
            TestableOrdersController controller = GetOrderControllerInitialized();
            Order order = GetPopulatedOrder();

            controller.CurrentOrder = order;
            controller.SaveCurrentOrderAsDraft();

            Assert.IsTrue(controller.MockOrdersService.SaveAsDraftCalled);
        }

        [TestMethod]
        public void SubmitOrderCallsRepository()
        {
            TestableOrdersController controller = GetOrderControllerInitialized();
            Order order = GetPopulatedOrder();

            controller.CurrentOrder = order;
            controller.SubmitOrder();

            Assert.IsTrue(controller.MockOrdersService.SubmitOrderCalled);
        }


        [TestMethod]
        public void CancelChangesClearCurrentOrder()
        {
            TestableOrdersController controller = GetOrderControllerInitialized();
            controller.MockOrdersService.Order = GetPopulatedOrder();
            controller.MockHttpContextLocatorService.GetCurrentContext().User = new GenericPrincipal(new GenericIdentity("CurrentUser"), null);

            Assert.IsNull(controller.CurrentOrder);

            controller.StartLoadOrderFlow(controller.MockOrdersService.Order.OrderId);

            Assert.IsNotNull(controller.CurrentOrder);

            controller.CancelChanges();

            Assert.IsNull(controller.CurrentOrder);
        }

        [TestMethod]
        public void CancelChangesClearCurrentOrderAndDonNotCallRepository()
        {
            TestableOrdersController controller = GetOrderControllerInitialized();
            controller.MockOrdersService.Order = GetPopulatedOrder();
            controller.MockHttpContextLocatorService.GetCurrentContext().User = new GenericPrincipal(new GenericIdentity("CurrentUser"), null);

            Assert.IsNull(controller.CurrentOrder);

            controller.StartLoadOrderFlow(controller.MockOrdersService.Order.OrderId);
            Assert.IsNotNull(controller.CurrentOrder);
            controller.CancelChanges();
            Assert.IsNull(controller.CurrentOrder);

            Assert.IsFalse(controller.MockOrdersService.SaveAsDraftCalled);
        }

        [TestMethod]
        public void CreateOrderCompleteClearCurrentOrderAndCompletesFlow()
        {
            TestableOrdersController controller = GetOrderControllerInitialized();
            controller.MockHttpContextLocatorService.GetCurrentContext().User = new GenericPrincipal(new GenericIdentity("CurrentUser"), null);
            controller.CreateNewOrder();
            Assert.IsNotNull(controller.CurrentOrder);

            controller.CreateOrderComplete();

            Assert.IsNull(controller.CurrentOrder);
        }

        [TestMethod]
        public void OrderControllerCallsOrderEntryFlowServiceForNavigation()
        {
            TestableOrdersController controller = GetOrderControllerInitialized();
            MockOrderEntryFlowService flowService = controller.MockOrderEntryFlowService;

            Assert.IsFalse(flowService.NavigateNextFromDetailsViewCalled);
            controller.NavigateNextFromDetailsView();
            Assert.IsTrue(flowService.NavigateNextFromDetailsViewCalled);

            Assert.IsFalse(flowService.NavigateNextFromGeneralViewCalled);
            controller.NavigateNextFromGeneralView();
            Assert.IsTrue(flowService.NavigateNextFromGeneralViewCalled);

            Assert.IsFalse(flowService.NavigatePreviousFromDetailsViewCalled);
            controller.NavigatePreviousFromDetailsView();
            Assert.IsTrue(flowService.NavigatePreviousFromDetailsViewCalled);

            Assert.IsFalse(flowService.NavigatePreviousFromReviewViewCalled);
            controller.NavigatePreviousFromPreviewView();
            Assert.IsTrue(flowService.NavigatePreviousFromReviewViewCalled);

            Assert.IsFalse(flowService.NavigateNextFromReviewViewCalled);
            controller.NavigateToConfirmationView();
            Assert.IsTrue(flowService.NavigateNextFromReviewViewCalled);

            Assert.IsFalse(flowService.NavigateToCurrentViewCalled);
            controller.CallNavigateToCurrentView();
            Assert.IsTrue(flowService.NavigateToCurrentViewCalled);

        }

        [TestMethod]
        public void CreateOrderSetCreatorProperty()
        {
            TestableOrdersController controller = GetOrderControllerInitialized();
            controller.MockHttpContextLocatorService.GetCurrentContext().User = new GenericPrincipal(new GenericIdentity("CurrentUser"), null);
            controller.CreateNewOrder();

            Assert.IsNotNull(controller.CurrentOrder.Creator);
            Assert.AreEqual("CurrentUser", controller.CurrentOrder.Creator);
        }

        [TestMethod]
        public void StartCreateOrderFlowShouldNavigateToStartView()
        {
            TestableOrdersController controller = GetOrderControllerInitialized();
            controller.MockHttpContextLocatorService.GetCurrentContext().User = new GenericPrincipal(new GenericIdentity("CurrentUser"), null);

            controller.StartCreateOrderFlow();

            Assert.IsTrue(controller.MockOrderEntryFlowService.NavigateToCurrentViewCalled);
        }

        [TestMethod]
        public void StartLoadOrderFlowShouldNavigateToStartView()
        {
            TestableOrdersController controller = GetOrderControllerInitialized();
            controller.MockOrdersService.Order = GetPopulatedOrder();
            controller.MockHttpContextLocatorService.GetCurrentContext().User = new GenericPrincipal(new GenericIdentity("CurrentUser"), null);

            controller.StartLoadOrderFlow(controller.MockOrdersService.Order.OrderId);


            Assert.IsTrue(controller.MockOrderEntryFlowService.NavigateToCurrentViewCalled);
        }

        [TestMethod]
        public void StartCreateOrderFlowCallsCreateNewOrderIfNeeded()
        {

            TestableOrdersController controller = GetOrderControllerInitialized();
            controller.MockHttpContextLocatorService.GetCurrentContext().User = new GenericPrincipal(new GenericIdentity("CurrentUser"), null);

            controller.StartCreateOrderFlow();

            Assert.IsTrue(controller.CreateNewOrderCalled);
        }

        [TestMethod]
        public void StartLoadOrderFlowCallsOrdersService()
        {
            TestableOrdersController controller = GetOrderControllerInitialized();
            controller.MockOrdersService.Order = GetPopulatedOrder();
            controller.MockHttpContextLocatorService.GetCurrentContext().User = new GenericPrincipal(new GenericIdentity("CurrentUser"), null);

            controller.StartLoadOrderFlow(controller.MockOrdersService.Order.OrderId);

            Assert.IsTrue(controller.MockOrdersService.GetOrderWithDetailsCalled);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StartLoadOrderFlowThrowsWithOrderNotFromCurrentUser()
        {
            TestableOrdersController controller = GetOrderControllerInitialized();
            controller.MockHttpContextLocatorService.GetCurrentContext().User = new GenericPrincipal(new GenericIdentity("CurrentUser"), null);
            controller.MockOrdersService.Order = GetPopulatedOrder();
            controller.MockOrdersService.Order.Creator = "AnotherUser";

            controller.StartLoadOrderFlow(controller.MockOrdersService.Order.OrderId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StartLoadOrderFlowThrowsWithOrderThatIsNotDraft()
        {
            TestableOrdersController controller = GetOrderControllerInitialized();
            controller.MockHttpContextLocatorService.GetCurrentContext().User = new GenericPrincipal(new GenericIdentity("CurrentUser"), null);
            controller.MockOrdersService.Order = GetPopulatedOrder();
            controller.MockOrdersService.Order.OrderStatus = (int)OrderStatusEnum.Submitted;

            controller.StartLoadOrderFlow(controller.MockOrdersService.Order.OrderId);
        }

        [TestMethod]
        public void CheckOrderEntryFlowIsStartedDoesNotRedirectWithRunningFlow()
        {
            TestableOrdersController controller = GetOrderControllerInitialized();
            controller.CurrentOrder = new Order();

            controller.VerifyOrderEntryFlowIsStarted();

            Assert.IsFalse(controller.MockOrderEntryFlowService.NavigateToDefaultViewCalled);
        }

        [TestMethod]
        public void CheckOrderEntryFlowIsStartedRedirectsToDefaultWithNotStartedFlow()
        {
            TestableOrdersController controller = GetOrderControllerInitialized();
            controller.CurrentOrder = null;

            controller.VerifyOrderEntryFlowIsStarted();

            Assert.IsTrue(controller.MockOrderEntryFlowService.NavigateToDefaultViewCalled);

        }

        private TestableOrdersController GetOrderControllerInitialized()
        {
            TestableOrdersController controller = new TestableOrdersController(
                new MockHttpContextLocatorService(),
                new MockOrderEntryFlowService()
            );
            controller.InitializeState(new StateValue<Order>());
            return controller;
        }

        private Order GetPopulatedOrder()
        {
            Order order = new Order();
            order.OrderId = 1;
            order.Approver = "11";
            order.CustomerId = "100";
            order.ShipAddress = "225 112th NE Street";
            order.ShipCity = "Bellevue";
            order.ShipRegion = "Washington";
            order.ShipPostalCode = "98005";
            order.Creator = "CurrentUser";
            order.OrderStatus = (int)OrderStatusEnum.Draft;
            return order;
        }


    }

    class TestableOrdersController : OrdersController
    {
        public bool CreateNewOrderCalled;

        public TestableOrdersController(IHttpContextLocatorService httpContextLocatorService, IOrderEntryFlowService orderEntryFlowService)
            : base(new MockOrdersService(), httpContextLocatorService, orderEntryFlowService)
        {
        }

        public MockOrdersService MockOrdersService
        {
            get { return this.OrdersService as MockOrdersService; }
        }

        public MockOrderEntryFlowService MockOrderEntryFlowService
        {
            get { return this.OrderEntryFlowService as MockOrderEntryFlowService; }
        }

        public MockHttpContextLocatorService MockHttpContextLocatorService
        {
            get { return this.HttpContextLocatorService as MockHttpContextLocatorService; }
        }

        public void CallNavigateToCurrentView()
        {
            base.NavigateToCurrentView();
        }
        public override void CreateNewOrder()
        {
            CreateNewOrderCalled = true;
            base.CreateNewOrder();
        }
    }
}

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
using OrdersRepository.BusinessEntities;
using OrdersRepository.Interfaces.Services;

namespace OrderManagement.Orders.Tests.Mocks
{

    public class MockOrdersController : IOrdersController
    {
        public bool SaveCurrentOrderAsDraftCalled;
        public bool SubmitOrderCalled;
        public bool GetCurrentOrderCalled;
        public bool SetCurrentOrderCalled;
        public bool NavigateNextFromGeneralViewCalled;
        public bool NavigatePreviousFromDetailsViewCalled;
        public bool NavigateNextFromDetailsViewCalled;
        public bool NavigatePreviousFromPreviewViewCalled;
        public bool NavigateToConfirmationViewCalled;
        public bool CancelChangesCalled;
        public bool ThrowExceptionWhenSaving = false;
        public bool StartCreateOrderFlowCalled;
        public bool StartLoadOrderFlowCalled;
        public bool CreateOrderCompleteCalled;
        public int LoadedOrderId;
        public bool VerifyOrderEntryFlowIsStartedCalled;

        public Order OrderToLoad;

        private Order _currentOrder;

        public MockOrdersController()
        {
            _currentOrder = new Order();
        }

        //public MockOrdersController()
        //    : this(new MockOrdersService(), new MockHttpContextLocatorService())
        //{ }

        //public MockOrdersController(IOrdersService ordersService, IHttpContextLocatorService httpContextLocatorService)
        //    : base(ordersService, httpContextLocatorService, new MockOrderEntryFlowService())
        //{
        //    _currentOrder = new Order();

        //    httpContextLocatorService.GetCurrentContext().User = new GenericPrincipal(new GenericIdentity("CurrentUser"), null);
        //}

        public void SaveCurrentOrderAsDraft()
        {
            SaveCurrentOrderAsDraftCalled = true;
            if (ThrowExceptionWhenSaving)
                throw new Exception("Mock has ThrowExceptionWhenSaving = true");
            //throw new Exceptions.DuplicateOrderNumberException("Mock has ThrowExceptionWhenSaving = true");
        }

        public void CancelChanges()
        {
            CancelChangesCalled = true;
        }

        public Order CurrentOrder
        {
            get
            {
                GetCurrentOrderCalled = true;
                return _currentOrder;
            }
            set
            {
                SetCurrentOrderCalled = true;
                _currentOrder = value;
            }
        }

        public void NavigateNextFromGeneralView()
        {
            NavigateNextFromGeneralViewCalled = true;
        }

        public void NavigatePreviousFromDetailsView()
        {
            NavigatePreviousFromDetailsViewCalled = true;
        }

        public void NavigateNextFromDetailsView()
        {
            NavigateNextFromDetailsViewCalled = true;
        }

        public void NavigatePreviousFromPreviewView()
        {
            NavigatePreviousFromPreviewViewCalled = true;
        }

        public void StartCreateOrderFlow()
        {
            StartCreateOrderFlowCalled = true;
        }

        public void StartLoadOrderFlow(int orderId)
        {
            StartLoadOrderFlowCalled = true;
            CurrentOrder = OrderToLoad;
            LoadedOrderId = orderId;
        }

        public void CreateOrderComplete()
        {
            CreateOrderCompleteCalled = true;
        }

        public void SubmitOrder()
        {
            SubmitOrderCalled = true;
        }

        public void NavigateToConfirmationView()
        {
            NavigateToConfirmationViewCalled = true;
        }

        public void VerifyOrderEntryFlowIsStarted()
        {
            VerifyOrderEntryFlowIsStartedCalled = true;
        }

        public void CreateNewOrder()
        {
            throw new Exception("The method or operation is not implemented.");
        }

    }
}

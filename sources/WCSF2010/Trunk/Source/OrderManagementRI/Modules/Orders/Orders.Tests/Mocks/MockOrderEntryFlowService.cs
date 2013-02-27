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
using OrderManagement.Orders.Services;

namespace OrderManagement.Orders.Tests.Mocks
{
    class MockOrderEntryFlowService : IOrderEntryFlowService
    {
        public bool NavigateNextFromGeneralViewCalled;
        public bool NavigatePreviousFromDetailsViewCalled;
        public bool NavigateNextFromDetailsViewCalled;
        public bool NavigatePreviousFromReviewViewCalled;
        public bool NavigateNextFromReviewViewCalled;
        public bool NavigateToCurrentViewCalled;
        public bool NavigateToDefaultViewCalled;

        #region IOrderEntryFlow Members

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

        public void NavigatePreviousFromReviewView()
        {
            NavigatePreviousFromReviewViewCalled = true;
        }

        public void NavigateToCurrentView()
        {
            NavigateToCurrentViewCalled = true;
        }

        public void NavigateNextFromReviewView()
        {
            NavigateNextFromReviewViewCalled = true;
        }

        public void NavigateToDefaultView()
        {
            NavigateToDefaultViewCalled = true;
        }

        #endregion
    }
}

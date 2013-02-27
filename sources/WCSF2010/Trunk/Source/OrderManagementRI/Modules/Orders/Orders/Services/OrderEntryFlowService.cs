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
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Interfaces;

namespace OrderManagement.Orders.Services
{
    public class OrderEntryFlowService : IOrderEntryFlowService
    {
        private IHttpContextLocatorService httpContextLocatorService;
        public OrderEntryFlowService([ServiceDependency] IHttpContextLocatorService httpContextLocatorService)
        {
            this.httpContextLocatorService = httpContextLocatorService;
        }

        public void NavigateNextFromGeneralView()
        {
            GoToDetails();
        }

        public void NavigatePreviousFromDetailsView()
        {
            GoToInformation();
        }

        public void NavigateNextFromDetailsView()
        {
            GoToReview();
        }

        public void NavigatePreviousFromReviewView()
        {
            GoToDetails();
        }

        public void NavigateNextFromReviewView()
        {
            GoToConfirmation();
        }

        public void NavigateToDefaultView()
        {
            GoToDefault();
        }

        public void NavigateToCurrentView()
        {
            GoToInformation();
        }

        #region private methods

        private const string BasePath = "~/Orders/OrderEntry/";

        protected IHttpContext HttpContext
        {
            get { return httpContextLocatorService.GetCurrentContext(); }
        }

        private void GoToInformation()
        {
            HttpContext.Response.Redirect(BasePath + "Information.aspx", true);
        }

        private void GoToDetails()
        {
            HttpContext.Response.Redirect(BasePath + "Details.aspx", true);
        }

        private void GoToReview()
        {
            HttpContext.Response.Redirect(BasePath + "Review.aspx", true);
        }

        private void GoToConfirmation()
        {
            HttpContext.Response.Redirect(BasePath + "Confirmation.aspx", true);
        }

        private void GoToDefault()
        {
            HttpContext.Response.Redirect("~/Orders/Default.aspx", true);
        }

        #endregion private methods

    }
}

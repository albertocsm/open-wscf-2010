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
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using System.Globalization;
using OrderManagement.Orders.Properties;

namespace OrderManagement.Orders.Views
{
    public class OrderReviewPresenter : Presenter<IOrderReview>
    {

      private IOrdersController _controller;

        [InjectionConstructor]
        public OrderReviewPresenter([CreateNew] IOrdersController controller)
        {
            if (controller == null)
                throw new ArgumentNullException("controller");

            controller.VerifyOrderEntryFlowIsStarted();

            _controller = controller;
        }

        public override void OnViewInitialized()
        {
            View.ShowOrder(_controller.CurrentOrder);
            View.SetConfirmationText(string.Format(CultureInfo.CurrentCulture, Resources.SubmittingOrderConfirmationMessage));
        }

        public void OnPrevious()
        {
            _controller.NavigatePreviousFromPreviewView();
        }

        public void OnSubmit()
        {
            _controller.SubmitOrder();
            _controller.NavigateToConfirmationView();
        }
    }
}





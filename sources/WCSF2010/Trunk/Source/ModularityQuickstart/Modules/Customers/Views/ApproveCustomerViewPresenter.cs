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
using ModularityQuickstart.Customers.BusinessEntities;
using Microsoft.Practices.ObjectBuilder;

namespace ModularityQuickstart.Customers.Views
{
    public class ApproveCustomerViewPresenter : Presenter<IApproveCustomerView>
    {
        private ICustomersController _controller;

        public ApproveCustomerViewPresenter([CreateNew] ICustomersController controller)
        {
            _controller = controller;
        }

        public void OnApproveCustomer()
        {
            _controller.ApproveCurrentCustomer();
        }
        
        public override void OnViewLoaded()
        {
            Customer customer = _controller.CurrentCustomer;
            if (customer == null)
            {
                View.ShowCustomerDetails = false;
                View.AllowApproveCustomer = false;
            }
            View.Customer = customer;
        }
    }
}

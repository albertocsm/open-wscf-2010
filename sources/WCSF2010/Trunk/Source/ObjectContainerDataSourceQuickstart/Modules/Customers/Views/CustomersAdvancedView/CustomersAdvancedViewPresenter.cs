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
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.ObjectBuilder;
using ObjectContainerDataSourceQuickstart.Modules.Customers.BusinessEntities;

namespace ObjectContainerDataSourceQuickstart.Modules.Customers.Views.CustomersAdvancedView
{
    public class CustomersAdvancedViewPresenter : Presenter<ICustomersAdvancedView>
    {
        private CustomersController _controller;

        public CustomersAdvancedViewPresenter([CreateNew] CustomersController controller)
        {
            _controller = controller;
        }

        public void OnCustomerInserted(Customer customer)
        {
            _controller.AddCustomer(customer);
        }

        public void OnCustomerDeleted(Customer customer)
        {
            _controller.DeleteCustomer(customer);
        }

        public void OnCustomerUpdated(Customer customer)
        {
            _controller.UpdateCustomer(customer);
        }

        // <snippet id="OnSelecting">
        public void OnSelecting(int startRowIndex, int maximumRows, string sortExpression)
        {
            View.Customers = _controller.GetCustomers(startRowIndex, maximumRows, sortExpression);
            View.TotalCustomersCount = _controller.CustomersTotalCount;
        }
        // </snippet>
    }
}

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
using OrderManagement.Customers.Views;
using OrderManagement.Customers.Tests.Mocks;

namespace OrderManagement.Customers.Tests
{
    /// <summary>
    /// Summary description for DefaultViewPresenterTestFixture
    /// </summary>
    [TestClass]
    public class DefaultViewPresenterTestFixture
    {
        [TestMethod]
        public void ShouldNavigateToSearchPageOnViewInitialized()
        {
            MockDefaultView view = new MockDefaultView();
            DefaultViewPresenter presenter = new DefaultViewPresenter();

            presenter.View = view;

            presenter.OnViewInitialized();

            Assert.IsTrue(view.NavigateToSearchCustomersPageCalled);
        }
    }
    namespace Mocks
    {
        class MockDefaultView : IDefaultView
        {
            public bool NavigateToSearchCustomersPageCalled;

            public void NavigateToSearchCustomersPage()
            {
                NavigateToSearchCustomersPageCalled = true;
            }
        }
    }
}

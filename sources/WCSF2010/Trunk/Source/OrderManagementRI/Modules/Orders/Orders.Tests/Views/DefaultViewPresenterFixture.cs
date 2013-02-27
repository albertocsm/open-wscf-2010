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

namespace OrderManagement.Orders.Tests
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

            Assert.IsTrue(view.NavigateToSearchOrdersPageCalled);
        }
    }
    namespace Mocks
    {
        class MockDefaultView : IDefaultView
        {
            public bool NavigateToSearchOrdersPageCalled;

            public void NavigateToSearchOrdersPage()
            {
                NavigateToSearchOrdersPageCalled = true;
            }
        }
    }
}

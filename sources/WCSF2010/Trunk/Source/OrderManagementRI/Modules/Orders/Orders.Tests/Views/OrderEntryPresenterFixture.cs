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
    /// Summary description for OrderEntryPresenterTestFixture
    /// </summary>
    [TestClass]
    public class OrderEntryPresenterTestFixture
    {
        private MockOrderEntry view;
        private OrderEntryPresenter presenter;
        private MockOrdersController controller;

        [TestInitialize]
        public void InitMVP()
        {
            view = new MockOrderEntry();
            controller = new MockOrdersController();
            presenter = new OrderEntryPresenter(controller);

            presenter.View = view;
        }

        [TestMethod]
        public void OnViewInitializedStartCreateOrderFlowIfEditIDNotSet()
        {
            view.EditOrderId = null;

            presenter.OnViewInitialized();

            Assert.IsTrue(controller.StartCreateOrderFlowCalled);
        }

        [TestMethod]
        public void OnViewInitializedStartLoadOrderFlowIfEditIDSet()
        {
            view.EditOrderId = "1";

            presenter.OnViewInitialized();

            Assert.IsTrue(controller.StartLoadOrderFlowCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EditIdWithLettersThrows()
        {
            view.EditOrderId = "A";

            presenter.OnViewInitialized();
        }

    }

    class MockOrderEntry : IOrderEntry
    {
        #region IOrderEntryView Members

        private string editOrderId;

        public string EditOrderId
        {
            get { return editOrderId; }
            set { editOrderId = value; }
        }

        #endregion
    }
}


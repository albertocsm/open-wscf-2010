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
using MVPQuickstart.Contacts.Views;
using MVPQuickstart.Contacts.BusinessEntities;
using MVPQuickstart.Contacts.Tests.Mocks;
using MVPQuickstart.Contacts.Services;
using System.Web.UI.WebControls;

namespace MVPQuickstart.Contacts.Tests
{
    /// <summary>
    /// Summary description for ContactsListPresenterTestFixture
    /// </summary>
    [TestClass]
    public class ContactsListPresenterTestFixture
    {
        public ContactsListPresenterTestFixture()
        {
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        ContactsListPresenter presenter;
        MockContactsListView view;
        MockContactsController controller;

        //Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void MyTestInitialize()
        {
            controller = new MockContactsController();
            view = new MockContactsListView();
            presenter = new ContactsListPresenter(view, controller);
        }

        [TestMethod]
        public void ControllerPropertyReturnsInjectedController()
        {
            Assert.AreSame(controller, presenter.Controller);
        }

        [TestMethod]
        public void OnViewLoadedRestoreFromPersisntenceState()
        {
            controller.SelectedContactIndex = 0;
            controller.RestoreFromPersistedStateCalled = false;
            view.SelectedIndex = 1;

            presenter.OnViewLoaded();

            Assert.IsTrue(controller.RestoreFromPersistedStateCalled);
            Assert.AreEqual(1, controller.SelectedContactIndex);
        }

        [TestMethod]
        public void OnSelectedIndexChangedUpdateModel()
        {
            view.SelectedIndex = 1;
            controller.SetSelectedContactIndex(0);

            presenter.OnSelectedIndexChanged();

            Assert.AreEqual(1, controller.SelectedContactIndex);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OnFireCreatingDataSourceWithNullArgumentsFails()
        {
            presenter.OnCreatingDataSource(null);
        }

        [TestMethod]
        public void OnFireCreatingDataSourceControllerIsSet()
        {
            ObjectDataSourceEventArgs e = new ObjectDataSourceEventArgs(null);

            presenter.OnCreatingDataSource(e);

            Assert.IsNotNull(e.ObjectInstance);
            Assert.AreSame(controller, e.ObjectInstance);
        }
    }

    class MockContactsListView : IContactsListView
    {
        private int _selectedIndex;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { _selectedIndex = value; }
        }
    }
    
}


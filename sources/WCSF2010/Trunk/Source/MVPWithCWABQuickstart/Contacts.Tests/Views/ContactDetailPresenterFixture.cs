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
using MVPWithCWABQuickStart.Contacts.Views;
using MVPWithCWABQuickStart.Contacts.Tests.Mocks;
using MVPWithCWABQuickStart.Contacts.BusinessEntities;
using System.Collections.ObjectModel;
using Microsoft.Practices.CompositeWeb.Utility;

namespace MVPWithCWABQuickStart.Contacts.Tests
{
    /// <summary>
    /// Summary description for ContactDetailPresenterTestFixture
    /// </summary>
    [TestClass]
    public class ContactDetailPresenterTestFixture
    {
        MockCustomersDataSource dataSource;
        MockContactsController controller;
        ContactDetailPresenter presenter;
        MockContactDetailView view;

        public ContactDetailPresenterTestFixture()
        {
        }

        [TestInitialize()]
        public void MyTestInitialize()
        {
            dataSource = new MockCustomersDataSource();
            controller = new MockContactsController();
            
            presenter = new ContactDetailPresenter(dataSource);
            presenter.Controller = controller;
            view = new MockContactDetailView();
            presenter.View = view;
        }

        [TestMethod]
        public void OnViewLoadedLoadStates()
        {
            dataSource.GetStatesReturnValue.Add(new State("WA", "Washington"));

            presenter.OnViewLoaded();

            Assert.IsTrue(dataSource.GetStatesCalled);
            Assert.IsTrue(view.LoadStatesCalled);
            Assert.IsTrue(view.LoadStatesArgumentValue.Count > 0);
        }

        [TestMethod]
        public void WhenCurrentContactChangeLoadInfo()
        {
            controller.CurrentCustomer = new Customer("1", "Customer 1", "Address 1", "City 1", "WA", "11111");

            presenter.OnViewLoaded();
            controller.FireCurrentCustomerChanged();

            Assert.AreSame(controller.CurrentCustomer, view.CustomerLoaded);
        }

        [TestMethod]
        public void OnViewInitializeHideEditControls()
        {
            view.SetViewControlsVisibleMode = true;
            view.SetViewControlsVisibleCalled = false;

            presenter.OnViewInitialized();

            Assert.IsTrue(view.SetViewControlsVisibleCalled);
            Assert.IsFalse(view.SetViewControlsVisibleMode);
        }

        [TestMethod]
        public void WhenCurrentContactChangeSwitchToReadOnlyView()
        {
            presenter.OnViewLoaded();

            controller.CurrentCustomer = new Customer("1", "Customer 1", "Address 1", "City 1", "WA", "11111");
            controller.FireCurrentCustomerChanged();

            Assert.IsTrue(view.SetViewModeCalled);
            Assert.IsTrue(view.SetViewModeReadOnly);
        }

        [TestMethod]
        public void WhenCurrentContactChangeShowEditControls()
        {
            view.SetViewControlsVisibleCalled = false;
            view.SetViewControlsVisibleMode = false;
            presenter.OnViewLoaded();

            controller.CurrentCustomer = new Customer("1", "Customer 1", "Address 1", "City 1", "WA", "11111");
            controller.FireCurrentCustomerChanged();

            Assert.IsTrue(view.SetViewControlsVisibleCalled);
            Assert.IsTrue(view.SetViewControlsVisibleMode);
        }

        [TestMethod]
        public void WhenEditIsClickedViewModeIsNotReadOnly()
        {
            view.SetViewModeCalled = false;
            view.SetViewModeReadOnly = true;

            presenter.OnViewLoaded();
            view.FireEditClicked();

            Assert.IsTrue(view.SetViewModeCalled);
            Assert.IsFalse(view.SetViewModeReadOnly);
        }

        [TestMethod]
        public void WhenDiscardChangesIsClickedViewModeIsReadOnlyAndCustomerInfoIsRestored()
        {
            presenter.OnViewLoaded();
            controller.CurrentCustomer = new Customer("1", "Customer 1", "Address 1", "City 1", "WA", "11111");
            view.CustomerLoaded = new Customer("1", "Customer 1 Modified", "Address 1 Modified", "City 2", "WA", "22222");
            view.SetViewModeCalled = false;
            view.SetViewModeReadOnly = false;

            view.FireDiscardChangesClicked();

            Assert.IsTrue(view.SetViewModeCalled);
            Assert.IsTrue(view.SetViewModeReadOnly);

            Assert.AreSame(controller.CurrentCustomer, view.CustomerLoaded);
        }

        [TestMethod]
        public void WhenSaveIsClickedViewModeIsReadOnlyAndCustomerInfoIsUpdatedInController()
        {
            presenter.OnViewLoaded();
            controller.CurrentCustomer = new Customer("1", "Customer 1", "Address 1", "City 1", "WA", "11111");
            Customer editedCustomer = new Customer("1", "Customer 1 Modified", "Address 1 Modified", "City 2", "OR", "22222");

            view.SetViewModeCalled = false;
            view.SetViewModeReadOnly = false;
            view.FireSaveClicked(editedCustomer);

            Assert.IsTrue(controller.UpdateCustomerCalled);

            Assert.IsTrue(view.SetViewModeCalled);
            Assert.IsTrue(view.SetViewModeReadOnly);

            Assert.AreNotSame(editedCustomer, controller.CurrentCustomer);
            Assert.AreSame(editedCustomer, controller.UpdatedCustomer);




            Assert.AreEqual("Customer 1 Modified", controller.UpdatedCustomer.Name);
            Assert.AreEqual("Address 1 Modified", controller.UpdatedCustomer.Address);
            Assert.AreEqual("City 2", controller.UpdatedCustomer.City);
            Assert.AreEqual("OR", controller.UpdatedCustomer.State);
            Assert.AreEqual("22222", controller.UpdatedCustomer.Zip);
        }
    }

    class MockContactDetailView : IContactDetailView
    {
        public bool SetViewModeCalled;
        public bool SetViewModeReadOnly;
        public bool SetViewControlsVisibleMode;
        public bool SetViewControlsVisibleCalled;
        public bool LoadStatesCalled;
        public ICollection<State> LoadStatesArgumentValue;
        public Customer CustomerLoaded;

        public event EventHandler EditClicked;
        public event EventHandler<DataEventArgs<Customer>> SaveClicked;
        public event EventHandler DiscardChangesClicked;
        public event EventHandler UserControlLoaded;

        public void ShowCustomer(Customer customer)
        {
            CustomerLoaded = customer;
        }

        public void SetViewReadOnlyMode(bool readOnly)
        {
            SetViewModeCalled = true;
            SetViewModeReadOnly = readOnly;
        }

        public void SetViewControlsVisible(bool visible)
        {
            SetViewControlsVisibleCalled = true;
            SetViewControlsVisibleMode = visible;
        }

        public void LoadStates(ICollection<State> states)
        {
            LoadStatesCalled = true;
            LoadStatesArgumentValue = states;
        }

        public void FireEditClicked()
        {
            if (EditClicked != null)
            {
                EditClicked(this, EventArgs.Empty);
            }
        }

        public void FireSaveClicked(Customer customer)
        {
            if (SaveClicked != null)
            {
                SaveClicked(this, new DataEventArgs<Customer>(customer));
            }
        }

        public void FireDiscardChangesClicked()
        {
            if (DiscardChangesClicked != null)
            {
                DiscardChangesClicked(this, EventArgs.Empty);
            }
        }

        public void FireUserControlLoaded()
        {
            if (UserControlLoaded != null)
            {
                UserControlLoaded(this, EventArgs.Empty);
            }
        }

        public ContactDetailPresenter Presenter
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }
    }
}


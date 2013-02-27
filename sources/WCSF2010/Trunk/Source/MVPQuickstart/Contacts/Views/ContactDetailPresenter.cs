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
using MVPQuickstart.Contacts.Services;
using MVPQuickstart.Contacts.BusinessEntities;

namespace MVPQuickstart.Contacts.Views
{
    public class ContactDetailPresenter
    {
        private IContactDetailView _view;
        private ICustomersDataSource _dataSource;
        private IContactsController _controller;

        public ContactDetailPresenter(IContactDetailView view,
                                    ICustomersDataSource dataSource)
        {
            _view = view;
            _dataSource = dataSource;
        }

        public IContactsController Controller
        {
            set
            {
                _controller = value;
            }
            get
            {
                return _controller;
            }
        }

        // When the contact details view is loaded, the presenter adds event handlers for the view�s events
        public virtual void OnViewLoaded()
        {
            _view.LoadStates(_dataSource.States);
            Controller.CurrentCustomerChanged += new EventHandler(Controller_CurrentCustomerChanged);
            _view.EditClicked += new EventHandler(View_EditClicked);
            _view.DiscardChangesClicked += new EventHandler(View_DiscardChangesClicked);
            _view.SaveClicked += new EventHandler<DataEventArgs<Customer>>(View_SaveClicked);
        }

        public virtual void OnViewInitialized()
        {
            _view.SetViewControlsVisible(false);
        }

        void Controller_CurrentCustomerChanged(object sender, EventArgs e)
        {
            LoadCurrentCustomerOnView();
            _view.SetViewControlsVisible(true);
            _view.SetViewReadOnlyMode(true);
        }

        void View_EditClicked(object sender,EventArgs e)
        {
            LoadCurrentCustomerOnView();
            _view.SetViewReadOnlyMode(false);            
        }

        void View_DiscardChangesClicked(object sender, EventArgs e)
        {
            LoadCurrentCustomerOnView();
            _view.SetViewReadOnlyMode(true);
        }

        void View_SaveClicked(object sender, DataEventArgs<Customer> e)
        {
            Controller.UpdateCurrentCustomer(e.Data);

            LoadCurrentCustomerOnView();
            _view.SetViewReadOnlyMode(true);
        }

        private void LoadCurrentCustomerOnView()
        {
            _view.ShowCustomer(Controller.CurrentCustomer);
        }
    }
}





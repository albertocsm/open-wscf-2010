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
using MVPWithCWABQuickStart.Contacts.Services;
using Microsoft.Practices.CompositeWeb.Utility;
using MVPWithCWABQuickStart.Contacts.BusinessEntities;

namespace MVPWithCWABQuickStart.Contacts.Views
{
    public class ContactDetailPresenter : Presenter<IContactDetailView>
    {
        private ICustomersDataSource _dataSource;
        private IContactsController _controller;

        public ContactDetailPresenter([ServiceDependency] ICustomersDataSource dataSource)
        {
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
        public override void OnViewLoaded()
        {
            View.LoadStates(_dataSource.States); 
            Controller.CurrentCustomerChanged += new EventHandler(Controller_CurrentCustomerChanged);
            View.EditClicked += new EventHandler(View_EditClicked);
            View.DiscardChangesClicked += new EventHandler(View_DiscardChangesClicked);
            View.SaveClicked += new EventHandler<DataEventArgs<Customer>>(View_SaveClicked);
        }

        public override void OnViewInitialized()
        {
            View.SetViewControlsVisible(false);
        }

        void Controller_CurrentCustomerChanged(object sender, EventArgs e)
        {
            LoadCurrentCustomerOnView();
            View.SetViewControlsVisible(true);
            View.SetViewReadOnlyMode(true);
        }

        void View_EditClicked(object sender, EventArgs e)
        {
            LoadCurrentCustomerOnView();
            View.SetViewReadOnlyMode(false);            
        }

        void View_DiscardChangesClicked(object sender, EventArgs e)
        {
            LoadCurrentCustomerOnView();
            View.SetViewReadOnlyMode(true);
        }

        void View_SaveClicked(object sender, DataEventArgs<Customer> e)
        {
            Controller.UpdateCurrentCustomer(e.Data);

            LoadCurrentCustomerOnView();
            View.SetViewReadOnlyMode(true);
        }

        private void LoadCurrentCustomerOnView()
        {
            View.ShowCustomer(Controller.CurrentCustomer);
        }
    }
}





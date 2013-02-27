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
using Microsoft.Practices.ObjectBuilder;
using System.Collections.ObjectModel;
using MVPWithCWABQuickStart.Contacts.BusinessEntities;
using System.Collections.Generic;
using Microsoft.Practices.CompositeWeb.Utility;

namespace MVPWithCWABQuickStart.Contacts.Views
{
    public partial class ContactDetail : Microsoft.Practices.CompositeWeb.Web.UI.UserControl, IContactDetailView
    {
        private ContactDetailPresenter _presenter;

        public event EventHandler EditClicked;
        public event EventHandler<DataEventArgs<Customer>> SaveClicked;
        public event EventHandler DiscardChangesClicked;
        public event EventHandler UserControlLoaded;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
            }
            OnUserControlLoaded();
            this._presenter.OnViewLoaded();
        }

        private void OnUserControlLoaded()
        {
            if (UserControlLoaded != null)
                UserControlLoaded(this, EventArgs.Empty);
        }
        
        [CreateNew]
        public ContactDetailPresenter Presenter
        {
            set
            {
                this._presenter = value;
                if (value != null)
                {
                    this._presenter.View = this;
                }
            }
            get
            {
                return this._presenter;
            }
        }

        public void ShowCustomer(Customer customer)
        {
            CustomerDataSource.DataSource = customer;
        }

        public void LoadStates(ICollection<State> states)
        {
            StatesDataSource.DataSource = states;
        }

        public void SetViewReadOnlyMode(bool readOnly)
        {
            if (readOnly)
            { CustomerDetailsView.ChangeMode(System.Web.UI.WebControls.DetailsViewMode.ReadOnly); }
            else
            { CustomerDetailsView.ChangeMode(System.Web.UI.WebControls.DetailsViewMode.Edit); }

            EditButton.Visible = readOnly;
            SaveButton.Visible = !readOnly;
            DiscardChangesButton.Visible = !readOnly;
        }

        // Views do not contain code to handle user interface events; instead, views notify 
        // their presenters through events or direct method calls to the presenter. 
        // In this case, the view notify their presenters through events
        protected void OnEditClicked(EventArgs e)
        {
            if (EditClicked != null)
            {
                EditClicked(this, e);
            }
        }

        protected void OnSaveClicked(DataEventArgs<Customer> e)
        {
            if (SaveClicked != null)
            {
                SaveClicked(this, e);
            }
        }

        protected void OnDiscardChangesClicked(EventArgs e)
        {
            if (DiscardChangesClicked != null)
            {
                DiscardChangesClicked(this, e);
            }
        }

        protected void EditButton_Click(object sender, EventArgs e)
        {
            OnEditClicked(e);
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            CustomerDetailsView.UpdateItem(true);
        }

        protected void DiscardChangesButton_Click(object sender, EventArgs e)
        {
            OnDiscardChangesClicked(e);
        }

        protected void CustomerDataSource_Updated(object sender, Microsoft.Practices.Web.UI.WebControls.ObjectContainerDataSourceStatusEventArgs e)
        {
            OnSaveClicked(new DataEventArgs<Customer>((Customer)e.Instance));
        }

        public void SetViewControlsVisible(bool visible)
        {
            EditButton.Visible = visible;
            SaveButton.Visible = visible;
            DiscardChangesButton.Visible = visible;
        }
    }
}


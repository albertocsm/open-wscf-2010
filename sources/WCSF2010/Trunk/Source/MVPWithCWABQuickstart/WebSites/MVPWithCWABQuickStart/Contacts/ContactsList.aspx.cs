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
using MVPWithCWABQuickStart.Contacts.BusinessEntities;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Microsoft.Practices.CompositeWeb.Utility;

namespace MVPWithCWABQuickStart.Contacts.Views
{
    public partial class ContactsList : Microsoft.Practices.CompositeWeb.Web.UI.Page, IContactsListView
    {
        private ContactsListPresenter _presenter;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.ContactDetail1.UserControlLoaded += new EventHandler(ContactDetail1_UserControlLoaded);

            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
            }
            this._presenter.OnViewLoaded();
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            CustomersGridView.DataBind();
        }

        void ContactDetail1_UserControlLoaded(object sender, EventArgs e)
        {
            this.ContactDetail1.Presenter.Controller = _presenter.Controller;
        }
        
        [CreateNew]
        public ContactsListPresenter Presenter
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

        public int SelectedIndex
        {
            get { return CustomersGridView.SelectedIndex; }
            set { CustomersGridView.SelectedIndex = value; }
        }

        // Views do not contain code to handle user interface events; instead, views notify 
        // their presenters through events or direct method calls to the presenter. 
        // In this case, the view notify their presenters through direct method calls to the presenter
        protected void CustomersGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            _presenter.OnSelectedIndexChanged();
        }

        protected void CustomersDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            _presenter.OnCreatingDataSource(e);
        }
    }
}


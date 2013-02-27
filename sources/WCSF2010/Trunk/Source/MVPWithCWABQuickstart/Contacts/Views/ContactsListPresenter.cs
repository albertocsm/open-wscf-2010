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
using MVPWithCWABQuickStart.Contacts.Repository;
using MVPWithCWABQuickStart.Contacts.Services;
using Microsoft.Practices.CompositeWeb.Utility;
using System.Web.UI.WebControls;

namespace MVPWithCWABQuickStart.Contacts.Views
{
    public class ContactsListPresenter : Presenter<IContactsListView>
    {
        IContactsController _controller;
        public ContactsListPresenter([CreateNew] IContactsController controller)
        {
            _controller = controller;
        }

        public IContactsController Controller
        {
            get { return _controller; }
        }
        
        public override void OnViewLoaded()
        {
            _controller.RestoreFromPersistedState(View.SelectedIndex);
        }

        public void OnSelectedIndexChanged()
        {
            _controller.SetSelectedContactIndex(View.SelectedIndex);
        }

        public void OnCreatingDataSource(ObjectDataSourceEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            e.ObjectInstance = _controller;
        }
    }
}





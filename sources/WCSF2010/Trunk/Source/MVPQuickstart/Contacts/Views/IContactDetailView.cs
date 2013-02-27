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
using MVPQuickstart.Contacts.BusinessEntities;
using System.Collections.ObjectModel;

namespace MVPQuickstart.Contacts.Views
{
    public interface IContactDetailView
    {
        // A different approach consists of using properties instead of methods, as shown below:
        // ICollection<State> States { get; set;}
        void LoadStates(ICollection<State> states);
        void SetViewReadOnlyMode(bool readOnly);
        void SetViewControlsVisible(bool visible);
        void ShowCustomer(Customer customer);

        ContactDetailPresenter Presenter { get;}
        event EventHandler EditClicked;
        event EventHandler<DataEventArgs<Customer>> SaveClicked;
        event EventHandler DiscardChangesClicked;
        event EventHandler UserControlLoaded;
    }
}





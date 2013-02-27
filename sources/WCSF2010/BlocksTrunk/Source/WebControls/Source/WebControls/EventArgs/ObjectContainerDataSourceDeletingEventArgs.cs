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
using System.Collections;
using System.ComponentModel;

namespace Microsoft.Practices.Web.UI.WebControls
{
	/// <summary>
	/// Provides data for the <see cref="ObjectContainerDataSource.Deleting"/> event.
	/// </summary>
    public class ObjectContainerDataSourceDeletingEventArgs : CancelEventArgs
    {
        private IDictionary _keys;
        private IDictionary _oldValues;

		/// <summary>
		/// Initializes a new instance of <see cref="ObjectContainerDataSourceDeletingEventArgs"/>.
		/// </summary>
		/// <param name="keys">An <see cref="IDictionary"/> object with the affected keys.</param>
		/// <param name="oldValues">An <see cref="IDictionary"/> object with the old values.</param>
        public ObjectContainerDataSourceDeletingEventArgs(IDictionary keys, IDictionary oldValues)
        {
            _keys = keys;
            _oldValues = oldValues;
        }

		/// <summary>
		/// Gets the affected keys <see cref="IDictionary"/>.
		/// </summary>
        public IDictionary Keys
        {
            get { return _keys; }
        }

		/// <summary>
		/// Gets the old values <see cref="IDictionary"/>.
		/// </summary>
        public IDictionary OldValues
        {
            get { return _oldValues; }
        }
    }
}

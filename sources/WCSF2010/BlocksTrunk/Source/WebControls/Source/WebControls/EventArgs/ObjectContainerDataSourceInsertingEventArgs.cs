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
	/// Provides data for the <see cref="ObjectContainerDataSource.Inserting"/> event.
	/// </summary>
    public class ObjectContainerDataSourceInsertingEventArgs : CancelEventArgs
    {
        private IDictionary _newValues;

		/// <summary>
		/// Initializes a new instance of <see cref="ObjectContainerDataSourceInsertingEventArgs"/>.
		/// </summary>
		/// <param name="newValues">An <see cref="IDictionary"/> object with the values being inserted.</param>
        public ObjectContainerDataSourceInsertingEventArgs(IDictionary newValues)
        {
            _newValues = newValues;
        }

		/// <summary>
		/// Gets an <see cref="IDictionary"/> object with the values being inserted.
		/// </summary>
        public IDictionary NewValues
        {
            get { return _newValues; }
        }

    }
}

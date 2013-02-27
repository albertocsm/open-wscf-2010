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
	/// Provides data fr the <see cref="ObjectContainerDataSource.Updating"/> event..
	/// </summary>
    public class ObjectContainerDataSourceUpdatingEventArgs : CancelEventArgs
    {
        private IDictionary _keys;
        private IDictionary _oldvalues;
        private IDictionary _newValues;
        
		/// <summary>
		/// Initializes a new instance of <see cref="ObjectContainerDataSourceUpdatingEventArgs"/>.
		/// </summary>
		/// <param name="keys">An <see cref="IDictionary"/> with the keys affected by the data operation.</param>
		/// <param name="newValues">An <see cref="IDictionary"/> with the values previous to the data operation.</param>
		/// <param name="oldValues">An <see cref="IDictionary"/> with the values after the data operation.</param>
        public ObjectContainerDataSourceUpdatingEventArgs(IDictionary keys, IDictionary newValues, IDictionary oldValues)
        {
            _keys = keys;
            _oldvalues = oldValues;
            _newValues = newValues;
        }

		/// <summary>
		/// Gets an <see cref="IDictionary"/> with the keys affected by the data operation
		/// </summary>
        public IDictionary Keys
        {
            get { return _keys; }
        }

		/// <summary>
		/// Gets an <see cref="IDictionary"/> with the values previous to the data operation.
		/// </summary>
        public IDictionary NewValues
        {
            get { return _newValues; }
        }

		/// <summary>
		/// Gets an <see cref="IDictionary"/> with the values after the data operation.
		/// </summary>
        public IDictionary OldValues
        {
            get { return _oldvalues; }
        }
    }
}

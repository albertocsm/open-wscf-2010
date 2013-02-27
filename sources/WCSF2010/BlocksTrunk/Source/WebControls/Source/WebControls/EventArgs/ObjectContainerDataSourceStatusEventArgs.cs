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

namespace Microsoft.Practices.Web.UI.WebControls
{
	/// <summary>
	/// Provides data for <see cref="ObjectContainerDataSource.Updated"/>, <see cref="ObjectContainerDataSource.Deleted"/>
	/// and <see cref="ObjectContainerDataSource.Inserted"/> events of the the <see cref="ObjectContainerDataSource"/> control.
	/// </summary>
    public class ObjectContainerDataSourceStatusEventArgs : EventArgs
    {
        private int _affectedRows;
        private object _instance;

		/// <summary>
		/// Initializes a new instance of <see cref="ObjectContainerDataSourceStatusEventArgs"/>.
		/// </summary>
		/// <param name="instance">The object instance affected by the data operation.</param>
		/// <param name="affectedRows">The number of rows that are affected by the data operation.</param>
        public ObjectContainerDataSourceStatusEventArgs(object instance, int affectedRows)
        {
            _affectedRows = affectedRows;
            _instance = instance;
        }
		/// <summary>
		/// Gets the number of rows that are affected by the data operation.
		/// </summary>
        public int AffectedRows
        {
            get { return _affectedRows; }
        }

		/// <summary>
		/// Gets the object instance affected by the data operation.
		/// </summary>
        public object Instance
        {
            get { return _instance; }
        } 
    }
}

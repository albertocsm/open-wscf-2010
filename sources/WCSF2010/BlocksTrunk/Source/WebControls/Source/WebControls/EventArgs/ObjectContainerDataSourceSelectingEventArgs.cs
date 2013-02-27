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
using System.ComponentModel;
using System.Web.UI;

namespace Microsoft.Practices.Web.UI.WebControls
{
	/// <summary>
	/// Provides data for the <see cref="ObjectContainerDataSource.Selecting"/> event.
	/// </summary>
    public class ObjectContainerDataSourceSelectingEventArgs : CancelEventArgs
    {
        private DataSourceSelectArguments _arguments;
        
		/// <summary>
		/// Initializes a new instance of <see cref="ObjectContainerDataSourceSelectingEventArgs"/>.
		/// </summary>
		/// <param name="arguments">The <see cref="DataSourceSelectArguments"/> with the select arguments.</param>
        public ObjectContainerDataSourceSelectingEventArgs(DataSourceSelectArguments arguments)
        {
            _arguments = arguments;
        }
        
		/// <summary>
		/// Gets the <see cref="DataSourceSelectArguments"/> with the select arguments.
		/// </summary>
        public DataSourceSelectArguments  Arguments
        {
            get { return _arguments; }
            set { _arguments = value; }
        }
    }
}

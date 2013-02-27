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
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Web.UI;
using Microsoft.Practices.Web.UI.WebControls.Design;
using Microsoft.Practices.Web.UI.WebControls.Properties;
[assembly: TagPrefix("Microsoft.Practices.Web.UI.WebControls", "pp")]
namespace Microsoft.Practices.Web.UI.WebControls
{
	/// <summary>
	/// Implements data binding in a way that easily integrates with the View-Presenter pattern.
	/// </summary>
    [Designer(typeof(ObjectContainerDataSourceDesigner)),
    DefaultProperty("DataObjectTypeName"),
    ParseChildren(true),
    PersistChildren(false)]
    public class ObjectContainerDataSource : DataSourceControl
    {
        #region Private Fields

        private ObjectContainerDataSourceView _view;
        private string[] _viewNames;

        #endregion

        #region Events

		/// <summary>
		/// This event occurs after an object is deleted. Handle this event to update your data store.
		/// </summary>
        [Category("Data"),
        Description("Occurs after an object is deleted. Handle this event to update your data store.")]
        public event EventHandler<ObjectContainerDataSourceStatusEventArgs> Deleted
        {
            add { View.Deleted += value; }
            remove { View.Deleted -= value; }
        }

		/// <summary>
		/// This event occurs after an object is deleted. Handle this event to update your data store.
		/// </summary>
        [Category("Data"),
       Description("Occurs after an object is deleted. Handle this event to update your data store.")]
        public event EventHandler<ObjectContainerDataSourceStatusEventArgs> Updated
        {
            add { View.Updated += value; }
            remove { View.Updated -= value; }
        }

		/// <summary>
		/// This event occurs after an object is inserted. Handle this event to update your data store.
		/// </summary>
        [Category("Data"),
        Description("Occurs after an object is inserted. Handle this event to update your data store.")]
        public event EventHandler<ObjectContainerDataSourceStatusEventArgs> Inserted
        {
            add { View.Inserted += value; }
            remove { View.Inserted -= value; }
        }

		/// <summary>
		/// This event occurs before an object is deleted. Handle this event to validate and/or modify the input parameters.
		/// </summary>
        [Category("Data"),
        Description("Occurs before an object is deleted. Handle this event to validate and/or modify the input parameters.")]
        public event EventHandler<ObjectContainerDataSourceDeletingEventArgs> Deleting
        {
            add { View.Deleting += value; }
            remove { View.Deleting -= value; }
        }

		/// <summary>
		/// This event occurs before an object is updated. Handle this event to validate and/or modify the input parameters.
		/// </summary>
        [Category("Data"),
        Description("Occurs before an object is updated. Handle this event to validate and/or modify the input parameters.")]
        public event EventHandler<ObjectContainerDataSourceUpdatingEventArgs> Updating
        {
            add { View.Updating += value; }
            remove { View.Updating -= value; }
        }

		/// <summary>
		/// This event occurs before an object is inserted. Handle this event to validate and/or modify the input parameters.
		/// </summary>
        [Category("Data"),
        Description("Occurs before an object is inserted. Handle this event to validate and/or modify the input parameters.")]
        public event EventHandler<ObjectContainerDataSourceInsertingEventArgs> Inserting
        {
            add { View.Inserting += value; }
            remove { View.Inserting -= value; }
        }

		/// <summary>
		/// This event occurs before performing a select operation. Handle this event to set the DataSource property when using server paging.
		/// </summary>
        [Category("Data"),
        Description("Occurs before performing a select operation. Handle this event to set the DataSource property when using server paging.")]
        public event EventHandler<ObjectContainerDataSourceSelectingEventArgs> Selecting
        {
            add { View.Selecting += value; }
            remove { View.Selecting -= value; }
        }

        #endregion

        #region Properties

		/// <summary>
		/// Gets a <see cref="ReadOnlyCollection{T}"/> of the items in the Data Source View.
		/// </summary>
        [Browsable(false)]
        public ReadOnlyCollection<object> Items
        {
            get { return View.Items; }
        }

		/// <summary>
		/// Sets the View Data Source.
		/// </summary>

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly")]
        [Browsable(false)]
        public object DataSource
        {
            set { View.DataSource = value; }
        }

		/// <summary>
		/// Gets or sets the full name of the type of objects that will be stored in the control.
		/// </summary>
        [Browsable(true),
        Category("Data"),
        Description("Full name of the type of objects that will be stored in the control.")]
        public string DataObjectTypeName
        {
            get { return View.DataObjectTypeName; }
            set { View.DataObjectTypeName = value; }
        }

		/// <summary>
		/// Gets or sets whether to use server sorting. If enabled, you must perform sorting manually.
		/// </summary>
        [Browsable(true),
        Category("Data"),
        DefaultValue(false),
        Description("Whether to use server sorting. If enabled, you must perform sorting manually.")]
        public bool UsingServerSorting
        {
            get { return View.UsingServerSorting; }
            set { View.UsingServerSorting = value; }
        }

		/// <summary>
		/// Gets or sets whether to use server paging. If enabled, you must perform paging manually.
		/// </summary>
        [Browsable(true),
        Category("Paging"),
        DefaultValue(false),
        Description("Whether to use server paging. If enabled, you must perform paging manually.")]
        public bool UsingServerPaging
        {
            get { return View.UsingServerPaging; }
            set { View.UsingServerPaging = value; }
        }

		/// <summary>
		/// Sets the number of rows retrieved during a data retrieval operation
		/// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly")]
        [Browsable(false)]
        public int TotalRowCount
        {
            set { View.TotalRowCount = value; }
        }
            
        #endregion

        #region Methods

		/// <summary>
		/// Gets a collection of names, representing the list of <see cref="DataSourceView"/> objects associated with the <see cref="DataSourceControl"/> control. 
		/// </summary>
		/// <returns>An ICollection that contains the names of the <see cref="DataSourceView"/> objects associated with the <see cref="DataSourceControl"/>.</returns>
        protected override ICollection GetViewNames()
        {
            if (_viewNames == null)
            {
                _viewNames = new string[] { Resources.DefaultViewName };
            }
            return _viewNames;
        }

		/// <summary>
		/// Gets the named data source view associated with the data source control. 
		/// </summary>
		/// <param name="viewName">The name of the <see cref="DataSourceView"/> to retrieve</param>
		/// <returns>Returns the named <see cref="DataSourceView"/> associated with the <see cref="DataSourceControl"/>.</returns>
        protected override DataSourceView GetView(string viewName)
        {
            if (!string.Equals(viewName, Resources.DefaultViewName, StringComparison.OrdinalIgnoreCase) && !String.IsNullOrEmpty(viewName))
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, Resources.InvalidViewName, "viewName"));

             return View;
        }

		/// <summary>
		/// Gets the default <see cref="ObjectContainerDataSourceView"/> associated with the data source control.
		/// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        protected virtual ObjectContainerDataSourceView View
        {
            get
            {
                if (_view == null)
                {
                    _view = new ObjectContainerDataSourceView(this, Resources.DefaultViewName);
                }
                return _view;
            }
        }

        #endregion
    }
}

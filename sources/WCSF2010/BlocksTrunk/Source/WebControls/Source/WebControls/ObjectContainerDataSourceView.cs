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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Web.Compilation;
using System.Web.UI;
using Microsoft.Practices.Web.UI.WebControls.Properties;
using Microsoft.Practices.Web.UI.WebControls.Utility;

namespace Microsoft.Practices.Web.UI.WebControls
{
	/// <summary>
	/// Defnies a data source view to be used with the <see cref="ObjectContainerDataSource"/> control.
	/// </summary>
	public class ObjectContainerDataSourceView : DataSourceView
	{
		#region Private Fields

		private List<object> _data = new List<object>();
		private bool _usingServerSorting;
		private string _dataObjectTypeName;
		private Type _dataObjectType;
		private int _totalRowCount = -1;
		private bool _usingServerPaging;
        private ObjectContainerDataSource _owner;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of <see cref="ObjectContainerDataSourceView"/>.
		/// </summary>
		/// <param name="owner">The <see cref="ObjectContainerDataSource"/> owner of this view.</param>
		/// <param name="name">The name of this view.</param>
		public ObjectContainerDataSourceView(ObjectContainerDataSource owner, string name)
			: base(owner, name)
		{
            this._owner = owner;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets a <see cref="ReadOnlyCollection{T}"/> with the data maanged by this view.
		/// </summary>
		public virtual ReadOnlyCollection<object> Items
		{
			get { return Data.AsReadOnly(); }
		}

		/// <summary>
		/// Sets the data source for this view.
		/// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly")]
        public virtual object DataSource
		{
			set
			{
				Data.Clear();

				if (value == null)
					return;

				IEnumerable values = value as IEnumerable;
				if (values != null)
				{
					SafeAddRange(values);
				}
				else
				{
					SafeAdd(value);
				}
				OnDataSourceViewChanged(EventArgs.Empty);
			}
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="DataSourceView"/> object associated
		/// with the current <see cref="DataSourceControl"/> object supports the <see cref="DataSourceView.ExecuteDelete(System.Collections.IDictionary,System.Collections.IDictionary)"/>
		/// operation.
		/// </summary>
		public override bool CanDelete
		{
			get { return true; }
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="DataSourceView"/> object associated
		/// with the current <see cref="DataSourceControl"/> object supports the <see cref="DataSourceView.ExecuteUpdate(System.Collections.IDictionary,System.Collections.IDictionary,System.Collections.IDictionary)"/>
		/// operation.
		/// </summary>
		public override bool CanUpdate
		{
			get { return true; }
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="DataSourceView"/> object associated
		/// with the current <see cref="DataSourceControl"/> object supports the <see cref="DataSourceView.ExecuteInsert(System.Collections.IDictionary)"/>
		/// operation.
		/// </summary>
		public override bool CanInsert
		{
			get { return true; }
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="DataSourceView"/> object associated
		/// with the current <see cref="DataSourceControl"/> object supports retrieving
		/// the total number of data rows, instead of the data.
		/// </summary>
		public override bool CanRetrieveTotalRowCount
		{
			get { return true; }
		}

		/// <summary>
		///  Gets a value indicating whether the <see cref="DataSourceView"/> object associated
		/// with the current <see cref="DataSourceControl"/> object supports a sorted
		/// view on the underlying data source.
		/// </summary>

		public override bool CanSort
		{
			get { return true; }
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="DataSourceView"/> object associated
		/// with the current <see cref="DataSourceControl"/> object supports paging through
		/// the data retrieved by the <see cref="DataSourceView.ExecuteSelect(System.Web.UI.DataSourceSelectArguments)"/>
		/// method.
		/// </summary>
		public override bool CanPage
		{
			get { return true; }
		}

		/// <summary>
		/// Gets or sets a value indicating if the <see cref="DataSourceView"/> object associated
		/// with the current <see cref="DataSourceControl"/> object uses sorting on the server.
		/// </summary>
		public virtual bool UsingServerSorting
		{
			get { return _usingServerSorting; }
			set
			{
				if (UsingServerSorting != value)
				{
					_usingServerSorting = value;
					OnDataSourceViewChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating if the <see cref="DataSourceView"/> object associated
		/// with the current <see cref="DataSourceControl"/> object uses paging on the server.
		/// </summary>
		public virtual bool UsingServerPaging
		{
			get { return _usingServerPaging; }
			set
			{
				if (UsingServerPaging != value)
				{
					_usingServerPaging = value;
					OnDataSourceViewChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Gets or sets the total number of rows retrieved in a data retrieval operation.
		/// </summary>
		public virtual int TotalRowCount
		{
			protected get { return _totalRowCount; }
			set
			{
				Guard.ArgumentNonNegative(value, "value");

				if (TotalRowCount != value)
				{
					_totalRowCount = value;
					OnDataSourceViewChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Gets or sets the data type name of the data objects.
		/// </summary>
		public virtual string DataObjectTypeName
		{
			get
			{
				return _dataObjectTypeName ?? String.Empty;
			}
			set
			{
				if (_dataObjectTypeName != value)
				{
					_dataObjectTypeName = value;
					OnDataSourceViewChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Gets the list of data objects managed by this view.
		/// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        protected virtual List<object> Data
		{
			get { return _data; }
		}

        /// <summary>
        /// Gets the <see cref="ObjectContainerDataSource"/> owner of this view.
        /// </summary>
        public virtual ObjectContainerDataSource Owner
        {
            get { return _owner; }
        }


		#endregion

		#region Events

		private readonly static object DeletedEventKey = new object();
		private readonly static object InsertedEventKey = new object();
		private readonly static object UpdatedEventKey = new object();
		private readonly static object DeletingEventKey = new object();
		private readonly static object InsertingEventKey = new object();
		private readonly static object UpdatingEventKey = new object();
		private readonly static object SelectingEventKey = new object();

		/// <summary>
		/// Occurs after a delete operation.
		/// </summary>
		public event EventHandler<ObjectContainerDataSourceStatusEventArgs> Deleted
		{
			add { base.Events.AddHandler(DeletedEventKey, value); }
			remove { base.Events.RemoveHandler(DeletedEventKey, value); }
		}

		/// <summary>
		/// Occurs after an update operation.
		/// </summary>
		public event EventHandler<ObjectContainerDataSourceStatusEventArgs> Updated
		{
			add { base.Events.AddHandler(UpdatedEventKey, value); }
			remove { base.Events.RemoveHandler(UpdatedEventKey, value); }
		}

		/// <summary>
		/// Occurs after an insert operation.
		/// </summary>
		public event EventHandler<ObjectContainerDataSourceStatusEventArgs> Inserted
		{
			add { base.Events.AddHandler(InsertedEventKey, value); }
			remove { base.Events.RemoveHandler(InsertedEventKey, value); }
		}

		/// <summary>
		/// Cancelable event that occurs before a delete operation.
		/// </summary>
		public event EventHandler<ObjectContainerDataSourceDeletingEventArgs> Deleting
		{
			add { base.Events.AddHandler(DeletingEventKey, value); }
			remove { base.Events.RemoveHandler(DeletingEventKey, value); }
		}

		/// <summary>
		/// Cancelable event that occurs before an update operation.
		/// </summary>
		public event EventHandler<ObjectContainerDataSourceUpdatingEventArgs> Updating
		{
			add { base.Events.AddHandler(UpdatingEventKey, value); }
			remove { base.Events.RemoveHandler(UpdatingEventKey, value); }
		}

		/// <summary>
		/// Cancelable event that occurs before an insert operation.
		/// </summary>
		public event EventHandler<ObjectContainerDataSourceInsertingEventArgs> Inserting
		{
			add { base.Events.AddHandler(InsertingEventKey, value); }
			remove { base.Events.RemoveHandler(InsertingEventKey, value); }
		}

		/// <summary>
		/// Cancelabe event that occurs before a select operation.
		/// </summary>
		public event EventHandler<ObjectContainerDataSourceSelectingEventArgs> Selecting
		{
			add { base.Events.AddHandler(SelectingEventKey, value); }
			remove { base.Events.RemoveHandler(SelectingEventKey, value); }
		}

		/// <summary>
		/// Fires the <see cref="ObjectContainerDataSourceView.Deleted"/> event.
		/// </summary>
		/// <param name="e">The event associated data.</param>
		protected virtual void OnDeleted(ObjectContainerDataSourceStatusEventArgs e)
		{
			EventHandler<ObjectContainerDataSourceStatusEventArgs> handler =
				base.Events[DeletedEventKey] as EventHandler<ObjectContainerDataSourceStatusEventArgs>;

			if (handler != null)
			{
				handler(this, e);
			}
		}

		/// <summary>
		/// Fires the <see cref="ObjectContainerDataSourceView.Updated"/> event.
		/// </summary>
		/// <param name="e">The event associated data.</param>
		protected virtual void OnUpdated(ObjectContainerDataSourceStatusEventArgs e)
		{
			EventHandler<ObjectContainerDataSourceStatusEventArgs> handler =
				base.Events[UpdatedEventKey] as EventHandler<ObjectContainerDataSourceStatusEventArgs>;

			if (handler != null)
			{
				handler(this, e);
			}
		}

		/// <summary>
		/// Fires the <see cref="ObjectContainerDataSourceView.Inserted"/> event.
		/// </summary>
		/// <param name="e">The event associated data.</param>
		protected virtual void OnInserted(ObjectContainerDataSourceStatusEventArgs e)
		{
			EventHandler<ObjectContainerDataSourceStatusEventArgs> handler =
				base.Events[InsertedEventKey] as EventHandler<ObjectContainerDataSourceStatusEventArgs>;

			if (handler != null)
			{
				handler(this, e);
			}
		}

		/// <summary>
		/// Fires the <see cref="ObjectContainerDataSourceView.Deleting"/> event.
		/// </summary>
		/// <param name="e">The event associated data.</param>
		protected virtual void OnDeleting(ObjectContainerDataSourceDeletingEventArgs e)
		{
			EventHandler<ObjectContainerDataSourceDeletingEventArgs> handler =
				base.Events[DeletingEventKey] as EventHandler<ObjectContainerDataSourceDeletingEventArgs>;

			if (handler != null)
			{
				handler(this, e);
			}
		}

		/// <summary>
		/// Fires the <see cref="ObjectContainerDataSourceView.Inserting"/> event.
		/// </summary>
		/// <param name="e">The event associated data.</param>
		protected virtual void OnInserting(ObjectContainerDataSourceInsertingEventArgs e)
		{
			EventHandler<ObjectContainerDataSourceInsertingEventArgs> handler =
				base.Events[InsertingEventKey] as EventHandler<ObjectContainerDataSourceInsertingEventArgs>;

			if (handler != null)
			{
				handler(this, e);
			}
		}

		/// <summary>
		/// Fires the <see cref="ObjectContainerDataSourceView.Updating"/> event.
		/// </summary>
		/// <param name="e">The event associated data.</param>
		protected virtual void OnUpdating(ObjectContainerDataSourceUpdatingEventArgs e)
		{
			EventHandler<ObjectContainerDataSourceUpdatingEventArgs> handler =
				base.Events[UpdatingEventKey] as EventHandler<ObjectContainerDataSourceUpdatingEventArgs>;

			if (handler != null)
			{
				handler(this, e);
			}
		}

		/// <summary>
		/// Fires the <see cref="ObjectContainerDataSourceView.Selecting"/> event.
		/// </summary>
		/// <param name="e">The event associated data.</param>
		protected virtual bool OnSelecting(ObjectContainerDataSourceSelectingEventArgs e)
		{
			EventHandler<ObjectContainerDataSourceSelectingEventArgs> handler =
				base.Events[SelectingEventKey] as EventHandler<ObjectContainerDataSourceSelectingEventArgs>;

			if (handler != null)
			{
				handler(this, e);
				return true;
			}
			else
			{
				return false;
			}
		}

		#endregion

		#region Methods

		private void SafeAdd(object item)
		{
			TypeDescriptionHelper.ThrowIfInvalidType(item, GetDataObjectType());

			Data.Add(item);
		}

		private void SafeAddRange(IEnumerable items)
		{
			foreach (object item in items)
			{
				SafeAdd(item);
			}
		}

		private void Add(object item)
		{
			Data.Add(item);
		}

		/// <summary>
		/// Performs an asynchronous insert operation on the list of data that the <see	cref="DataSourceView"/> object represents.
		/// </summary>
		/// <param name="values">An <see cref="IDictionary"/> of name/value pairs used during an insert operation.</param>
		/// <param name="callback"></param>
		public override void Insert(IDictionary values, DataSourceViewOperationCallback callback)
		{
			TypeDescriptionHelper.ThrowIfNoDefaultConstructor(GetDataObjectType());
			base.Insert(values, callback);
		}

		/// <summary>
		/// Gets a list of data from the underlying data storage.
		/// </summary>
		/// <param name="arguments">A <see cref="DataSourceSelectArguments"/> that is used to request operations on the data 
		/// beyond basic data retrieval.</param>
		/// <returns>An <see cref="IEnumerable"/> list of data from the underlying data storage.</returns>
		protected override IEnumerable ExecuteSelect(DataSourceSelectArguments arguments)
		{
            AddSupportedCapabilities(arguments);
			
			ObjectContainerDataSourceSelectingEventArgs selectingEventArgs = new ObjectContainerDataSourceSelectingEventArgs(arguments);
            FireSelectingEvent(selectingEventArgs);
			if (selectingEventArgs.Cancel)
				return null;

            SortDataForSelectIfNeeded(arguments);

			if (arguments.RetrieveTotalRowCount)
			{
				arguments.TotalRowCount = _totalRowCount < 0 ? Data.Count : _totalRowCount;
			}
			if (arguments.MaximumRows > 0)
			{
				if (UsingServerPaging) 
				{
                    GuardTotalRowCountNotNegative(TotalRowCount);
				}
				else
				{
                    GuardStartRowIndexNotNegative(arguments);

                    if ((Data.Count == 0 && arguments.StartRowIndex > 0) || (Data.Count > 0 && (arguments.StartRowIndex >= Data.Count)))
                        throw new ArgumentOutOfRangeException(String.Format(CultureInfo.CurrentCulture, Resources.StartRowIndexOutOfRange, arguments.StartRowIndex, Data.Count));

					int rowsLeft = Data.Count - arguments.StartRowIndex;
					int rowCount = Math.Min(rowsLeft,arguments.MaximumRows);
					return Data.GetRange(arguments.StartRowIndex, rowCount);
				}
			}

			return Data;
		}

        private static void GuardTotalRowCountNotNegative(int TotalRowCount)
        {
            if (TotalRowCount < 0)
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, Resources.InvalidServerPagingSettings));
        }

        private static void GuardStartRowIndexNotNegative(DataSourceSelectArguments arguments)
        {
            if (arguments.StartRowIndex < 0)
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, Resources.InvalidStartRowIndex, arguments.StartRowIndex));
        }

        private void FireSelectingEvent(ObjectContainerDataSourceSelectingEventArgs selectingEventArgs)
        {
            bool handled = OnSelecting(selectingEventArgs);
            if (!handled && UsingServerPaging)
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, Resources.UsingServerPagingAndSelectingEventNotHandled));
        }

        private void SortDataForSelectIfNeeded(DataSourceSelectArguments arguments)
        {
            if (!(UsingServerSorting || String.IsNullOrEmpty(arguments.SortExpression)))
            {
                Data.Sort(new ObjectComparer(arguments.SortExpression, GetDataObjectType()));
            }
        }

        private void AddSupportedCapabilities(DataSourceSelectArguments arguments)
        {
            if (CanSort)
            {
                arguments.AddSupportedCapabilities(DataSourceCapabilities.Sort);
            }
            if (CanRetrieveTotalRowCount)
            {
                arguments.AddSupportedCapabilities(DataSourceCapabilities.RetrieveTotalRowCount);
            }
            if (CanPage)
            {
                arguments.AddSupportedCapabilities(DataSourceCapabilities.Page);
            }

            arguments.RaiseUnsupportedCapabilitiesError(this);
        }

		/// <summary>
		/// Performs an insert operation on the list of data that the <see cref="DataSourceView"/> object represents.
		/// </summary>
		/// <param name="values">An <see cref="IDictionary"/> of name/value pairs used during an insert operation.</param>
		/// <returns>The number of items that were inserted into the underlying data storage.</returns>
		protected override int ExecuteInsert(IDictionary values)
		{
			Guard.ArgumentNotNull(values, "values");

			ObjectContainerDataSourceInsertingEventArgs insertingEventArgs = new ObjectContainerDataSourceInsertingEventArgs(values);
			OnInserting(insertingEventArgs);
			if (insertingEventArgs.Cancel)
				return 0;

			object instance = CreateInstance();
			TypeDescriptionHelper.BuildInstance(values, instance);
			Add(instance);
			OnDataSourceViewChanged(EventArgs.Empty);

			int rowsAffected = 1;
			ObjectContainerDataSourceStatusEventArgs insertedEventArgs = new ObjectContainerDataSourceStatusEventArgs(instance, rowsAffected);
			OnInserted(insertedEventArgs);

			return rowsAffected;
		}

		/// <summary>
		/// Performs a delete operation on the list of data that the <see cref="DataSourceView"/> object represents.
		/// </summary>
		/// <param name="keys">An <see cref="IDictionary"/> of object or row keys to be deleted by
		/// the <see cref="DataSourceView.ExecuteDelete(System.Collections.IDictionary,System.Collections.IDictionary)"/>
		/// operation.</param>
		/// <param name="oldValues">An <see cref="System.Collections.IDictionary"/> of name/value pairs that represent data elements and their original values.</param>
		/// <returns>The number of items that were deleted from the underlying data storage.</returns>
		protected override int ExecuteDelete(IDictionary keys, IDictionary oldValues)
		{
			Guard.CollectionNotNullNorEmpty(keys, String.Format(CultureInfo.CurrentCulture, Resources.NoKeysSpecified), "keys");

			ObjectContainerDataSourceDeletingEventArgs deletingEventArgs =
				new ObjectContainerDataSourceDeletingEventArgs(DictionaryHelper.GetReadOnlyDictionary(keys), oldValues);
			OnDeleting(deletingEventArgs);
			if (deletingEventArgs.Cancel)
				return 0;

			int rowsAffected;
			object instance = FindInstance(keys);
			if (instance == null)
			{
				rowsAffected = 0;
			}
			else
			{
				Data.Remove(instance);
				rowsAffected = 1;
			}
			instance = CreateInstance();
			TypeDescriptionHelper.BuildInstance(oldValues, instance);
			TypeDescriptionHelper.BuildInstance(keys, instance);
			OnDataSourceViewChanged(EventArgs.Empty);

			ObjectContainerDataSourceStatusEventArgs deletedEventArgs = new ObjectContainerDataSourceStatusEventArgs(instance, rowsAffected);
			OnDeleted(deletedEventArgs);

			return rowsAffected;
		}

		/// <summary>
		/// Performs an update operation on the list of data that the <see cref="DataSourceView"/> object represents.
		/// </summary>
		/// <param name="keys">An <see cref="System.Collections.IDictionary"/> of object or row keys to be updated by the update operation.</param>
		/// <param name="values">An <see cref="System.Collections.IDictionary"/> of name/value pairs that represent data elements and their new values.</param>
		/// <param name="oldValues">An <see cref="System.Collections.IDictionary"/> of name/value pairs that represent data elements and their original values.</param>
		/// <returns>The number of items that were updated in the underlying data storage.</returns>
		protected override int ExecuteUpdate(IDictionary keys, IDictionary values, IDictionary oldValues)
		{
			Guard.CollectionNotNullNorEmpty(keys, String.Format(CultureInfo.CurrentCulture, Resources.NoKeysSpecified), "keys");
			Guard.ArgumentNotNull(values, "values");

			ObjectContainerDataSourceUpdatingEventArgs updatingEventArgs =
				new ObjectContainerDataSourceUpdatingEventArgs(DictionaryHelper.GetReadOnlyDictionary(keys), values, oldValues);
			OnUpdating(updatingEventArgs);
			if (updatingEventArgs.Cancel)
				return 0;

			object newInstance = CreateInstance();
			TypeDescriptionHelper.BuildInstance(keys, newInstance);
			TypeDescriptionHelper.BuildInstance(values, newInstance);
			int rowsAffected;
			object oldInstance = FindInstance(keys);
			if (oldInstance != null)
			{
				int index = Data.IndexOf(oldInstance);
				Data[index] = newInstance;
				rowsAffected = 1;
			}
			else
			{
				rowsAffected = 0;
			}
			OnDataSourceViewChanged(EventArgs.Empty);

			ObjectContainerDataSourceStatusEventArgs updatedEventArgs = new ObjectContainerDataSourceStatusEventArgs(newInstance, rowsAffected);
			OnUpdated(updatedEventArgs);

			return rowsAffected;
		}

		private Type GetDataObjectType()
		{
			if (_dataObjectType == null)
			{
				if (String.IsNullOrEmpty(DataObjectTypeName))
					throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, Resources.DataObjectTypeNameNotSet));

				Type type = BuildManager.GetType(DataObjectTypeName, false, true);
				if (type == null)
					throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, Resources.DataObjectTypeNotFound, DataObjectTypeName));

				_dataObjectType = type;
			}
			return _dataObjectType;
		}

		private object CreateInstance()
		{
			Type type = GetDataObjectType();
			return Activator.CreateInstance(type);
		}

		private object FindInstance(IDictionary keys)
		{
			return _data.Find(delegate(object obj)
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(obj);
				foreach (string keyName in keys.Keys)
				{
					PropertyDescriptor property = TypeDescriptionHelper.GetValidProperty(properties, keyName);
					if (!property.GetValue(obj).Equals(keys[keyName]))
						return false;
				}
				return true;
			});
		}

		#endregion

		#region Nested Types

		private class ObjectComparer : IComparer<object>
		{
			PropertyDescriptor _property;
			bool _desc;

			public ObjectComparer(string sortExpression, Type type)
			{
				string[] sortExpressionParts = sortExpression.Split(' ');

				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(type);
				_property = properties.Find(sortExpressionParts[0], true);
				if (_property == null)
					throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, Resources.InvalidPropertyInSortingExpression, sortExpressionParts[0]));

				if (!typeof(IComparable).IsAssignableFrom(_property.PropertyType))
					throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, Resources.PropertyDoesNotImplementIComparable, sortExpressionParts[0]));

				if (sortExpressionParts.Length > 1)
				{
					string order = sortExpressionParts[1];
					if (order.Equals("DESC"))
					{
						_desc = true;
					}
					else if (!order.Equals("ASC"))
						throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, Resources.InvalidSortingExpression, sortExpressionParts[1]));
				}
			}

			#region IComparer<object> Members

			public int Compare(object x, object y)
			{
				IComparable valueX = _property.GetValue(x) as IComparable;
				IComparable valueY = _property.GetValue(y) as IComparable;
				int comparisonResult = valueX.CompareTo(valueY);
				if (_desc)
				{
					comparisonResult *= -1;
				}
				return comparisonResult;
			}

			#endregion
		}

		#endregion
	}
}

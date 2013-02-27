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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.ObjectBuilder;
using Microsoft.Practices.CompositeWeb.Properties;
using Microsoft.Practices.CompositeWeb.Utility;
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.CompositeWeb.Collections
{
	/// <summary>
	/// Represents a filtered collection of objects, based on the locator used by the <see cref="CompositionContainer"/>.
	/// </summary>
	[SuppressMessage("Microsoft.Design", "CA1035:ICollectionImplementationsHaveStronglyTypedMembers")]
	public class ManagedObjectCollection<TItem> : IManagedObjectCollection<TItem>
	{
		#region Delegates

		/// <summary>
		/// The delegate used for auto-creation of an item when not present, but indexed.
		/// </summary>
		/// <param name="t">The type being asked for.</param>
		/// <param name="id">The ID being asked for.</param>
		/// <returns></returns>
		public delegate TItem IndexerCreationDelegate(Type t, string id);

		#endregion

		private IBuilder<WCSFBuilderStage> _builder;
		private CompositionContainer _container;
		private Predicate<TItem> _filter;
		private IndexerCreationDelegate _indexerCreationDelegate;
		private ILifetimeContainer _lifetimeContainer;
		private IReadWriteLocator _locator;
		private IManagedObjectCollection<TItem> _parentCollection;
		private SearchMode _searchMode;

		/// <summary>
		/// Initializes an instance of the <see cref="ManagedObjectCollection{TItem}"/> class.
		/// </summary>
		public ManagedObjectCollection(
			ILifetimeContainer container,
			IReadWriteLocator locator,
			IBuilder<WCSFBuilderStage> builder,
			SearchMode searchMode,
			IndexerCreationDelegate indexerCreationDelegate,
			Predicate<TItem> filter)
			: this(container, locator, builder, searchMode, indexerCreationDelegate, filter, null)
		{
		}

		/// <summary>
		/// Initializes an instance of the <see cref="ManagedObjectCollection{TItem}"/> class.
		/// </summary>
		public ManagedObjectCollection(
			ILifetimeContainer container,
			IReadWriteLocator locator,
			IBuilder<WCSFBuilderStage> builder,
			SearchMode searchMode,
			IndexerCreationDelegate indexerCreationDelegate,
			Predicate<TItem> filter,
			ManagedObjectCollection<TItem> parentCollection)
		{
			_lifetimeContainer = container;
			_locator = locator;
			_builder = builder;
			_searchMode = searchMode;
			_indexerCreationDelegate = indexerCreationDelegate;
			_filter = filter;
			_parentCollection = parentCollection;
			_container =
				locator.Get<CompositionContainer>(new DependencyResolutionLocatorKey(typeof (CompositionContainer), null));

			if (_container != null)
			{
				_container.ObjectAdded += new EventHandler<DataEventArgs<object>>(Container_ItemAdded);
				_container.ObjectRemoved += new EventHandler<DataEventArgs<object>>(Container_ItemRemoved);
			}
		}

		/// <summary>
		/// Gets the number of items in the collection.
		/// </summary>
		public int Count
		{
			get
			{
				int result = 0;

				foreach (KeyValuePair<object, object> pair in _locator)
				{
					DependencyResolutionLocatorKey key = pair.Key as DependencyResolutionLocatorKey;

					if (key != null && key.ID != null && pair.Value is TItem && PassesFilter(pair.Value))
						result++;
				}

				if (_searchMode == SearchMode.Up && _parentCollection != null)
					result += _parentCollection.Count;

				return result;
			}
		}

		#region IManagedObjectCollection<TItem> Members

		/// <summary>
		/// Gets an item by ID.
		/// </summary>
		/// <param name="index">The ID of the item to get.</param>
		/// <returns>The item, if present; null otherwise. If the collection has been given an
		/// <see cref="IndexerCreationDelegate"/> and the item is not found, it will be created
		/// using the delegate.</returns>
		public TItem this[string index]
		{
			get
			{
				TItem result = Get(index);

				if (result == null && _indexerCreationDelegate != null && !Contains(index, _searchMode, false))
				{
					result = _indexerCreationDelegate(typeof (TItem), index);
					Add(result, index);
				}

				return result;
			}
		}

		/// <summary>
		/// Creates a new item and adds it to the collection, without an ID.
		/// </summary>
		/// <typeparam name="TTypeToBuild">The type of item to be built.</typeparam>
		/// <returns>The newly created item.</returns>
		public TTypeToBuild AddNew<TTypeToBuild>()
			where TTypeToBuild : TItem
		{
			return (TTypeToBuild) AddNew(typeof (TTypeToBuild), null);
		}

		/// <summary>
		/// Creates a new item and adds it to the collection, with an ID.
		/// </summary>
		/// <typeparam name="TTypeToBuild">The type of item to be built.</typeparam>
		/// <param name="id">The ID of the item.</param>
		/// <returns>The newly created item.</returns>
		public TTypeToBuild AddNew<TTypeToBuild>(string id)
			where TTypeToBuild : TItem
		{
			return (TTypeToBuild) AddNew(typeof (TTypeToBuild), id);
		}

		/// <summary>
		/// Finds all objects that are type-compatible with the given type.
		/// </summary>
		/// <typeparam name="TSearchType">The type of item to find.</typeparam>
		/// <returns>A collection of the found items.</returns>
		public ICollection<TSearchType> FindByType<TSearchType>()
			where TSearchType : TItem
		{
			List<TSearchType> result = new List<TSearchType>();

			foreach (object obj in _lifetimeContainer)
				if (typeof (TSearchType).IsAssignableFrom(obj.GetType()))
					result.Add((TSearchType) obj);

			if (_searchMode == SearchMode.Up && _parentCollection != null)
				result.AddRange(_parentCollection.FindByType<TSearchType>());

			return result;
		}

		/// <summary>
		/// Finds all objects that are type-compatible with the given type.
		/// </summary>
		/// <param name="searchType">The type of item to find.</param>
		/// <returns>A collection of the found items.</returns>
		public ICollection<TItem> FindByType(Type searchType)
		{
			List<TItem> result = new List<TItem>();

			foreach (object obj in _lifetimeContainer)
				if (searchType.IsAssignableFrom(obj.GetType()))
					result.Add((TItem) obj);

			if (_searchMode == SearchMode.Up && _parentCollection != null)
				result.AddRange(_parentCollection.FindByType(searchType));

			return result;
		}

		/// <summary>
		/// Gets the object with the given ID.
		/// </summary>
		/// <param name="id">The ID to get.</param>
		/// <returns>The object, if present; null otherwise.</returns>
		[SuppressMessage("Microsoft.Usage", "CA2223:MembersShouldDifferByMoreThanReturnType")]
		public TItem Get(string id)
		{
			return Get(id, _searchMode, true);
		}

		/// <summary>
		/// Gets an enumerator which returns all the objects in the container, along with their
		/// names.
		/// </summary>
		/// <returns>The enumerator.</returns>
		public IEnumerator<KeyValuePair<string, TItem>> GetEnumerator()
		{
			IReadableLocator currentLocator = _locator;
			Dictionary<string, object> seenNames = new Dictionary<string, object>();

			do
			{
				foreach (KeyValuePair<object, object> pair in currentLocator)
					if (pair.Key is DependencyResolutionLocatorKey)
					{
						DependencyResolutionLocatorKey depKey = (DependencyResolutionLocatorKey) pair.Key;

						if (depKey.ID != null && pair.Value is TItem)
						{
							string id = depKey.ID;
							TItem value = (TItem) pair.Value;

							if (!seenNames.ContainsKey(id) && (_filter == null || _filter(value)))
							{
								seenNames[id] = String.Empty;
								yield return new KeyValuePair<string, TItem>(id, value);
							}
						}
					}
			} while (_searchMode == SearchMode.Up && (currentLocator = currentLocator.ParentLocator) != null);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		void ICollection.CopyTo(Array array, int index)
		{
			throw new NotImplementedException();
		}

		int ICollection.Count
		{
			get { return Count; }
		}

		[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		bool ICollection.IsSynchronized
		{
			get { return false; }
		}

		[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		object ICollection.SyncRoot
		{
			get { throw new NotImplementedException(); }
		}

		#endregion

		/// <summary>
		/// Event that's fired when an object is added to the collection.
		/// </summary>
		public event EventHandler<DataEventArgs<TItem>> Added;

		/// <summary>
		/// Event that's fired when an object is removed from the collection, or disposed when the
		/// collection is cleaning up.
		/// </summary>
		public event EventHandler<DataEventArgs<TItem>> Removed;

		/// <summary>
		/// Adds an item to the collection, without an ID.
		/// </summary>
		/// <param name="item">The item to be added.</param>
		public void Add(TItem item)
		{
			Add(item, null);
		}

		/// <summary>
		/// Adds an item to the collection, with an ID.
		/// </summary>
		/// <param name="item">The item to be added.</param>
		/// <param name="id">The ID of the item.</param>
		public void Add(TItem item, string id)
		{
			Guard.ArgumentNotNull(item, "item");

			Build(item.GetType(), id, item);
		}

		/// <summary>
		/// Creates a new item and adds it to the collection, without an ID.
		/// </summary>
		/// <param name="typeToBuild">The type of item to be built.</param>
		/// <returns>The newly created item.</returns>
		public TItem AddNew(Type typeToBuild)
		{
			return AddNew(typeToBuild, null);
		}

		/// <summary>
		/// Creates a new item and adds it to the collection, with an ID.
		/// </summary>
		/// <param name="typeToBuild">The type of item to be built.</param>
		/// <param name="id">The ID of the item.</param>
		/// <returns>The newly created item.</returns>
		public TItem AddNew(Type typeToBuild, string id)
		{
			return Build(typeToBuild, id, null);
		}

		/// <summary>
		/// Determines if the collection contains an object with the given ID.
		/// </summary>
		/// <param name="id">The ID of the object.</param>
		/// <returns>Returns true if the collection contains the object; false otherwise.</returns>
		public bool Contains(string id)
		{
			return Get(id) != null;
		}

		private bool Contains(string id, SearchMode searchMode, bool filtered)
		{
			return Get(id, searchMode, filtered) != null;
		}

		/// <summary>
		/// Determines if the collection contains an object.
		/// </summary>
		/// <param name="item">The object.</param>
		/// <returns>Returns true if the collection contains the object; false otherwise.</returns>
		public bool ContainsObject(TItem item)
		{
			return _lifetimeContainer.Contains(item);
		}

		private TItem Get(string id, SearchMode searchMode, bool filtered)
		{
			Guard.ArgumentNotNull(id, "id");

			foreach (KeyValuePair<object, object> pair in _locator)
			{
				if (pair.Key is DependencyResolutionLocatorKey)
				{
					DependencyResolutionLocatorKey depKey = (DependencyResolutionLocatorKey) pair.Key;

					if (Equals(depKey.ID, id))
					{
						TItem result = (TItem) pair.Value;
						if (!filtered || _filter == null || _filter(result))
							return result;
					}
				}
			}

			if (searchMode == SearchMode.Up && _parentCollection != null)
				return _parentCollection.Get(id);

			return default(TItem);
		}

		/// <summary>
		/// Gets the object with the given ID.
		/// </summary>
		/// <typeparam name="TTypeToGet">The type of the object to get.</typeparam>
		/// <param name="id">The ID to get.</param>
		/// <returns>The object, if present; null otherwise.</returns>
		[SuppressMessage("Microsoft.Usage", "CA2223:MembersShouldDifferByMoreThanReturnType")]
		public TTypeToGet Get<TTypeToGet>(string id)
			where TTypeToGet : TItem
		{
			return (TTypeToGet) Get(id);
		}

		/// <summary>
		/// Removes an object from the container.
		/// </summary>
		/// <param name="item">The item to be removed.</param>
		public void Remove(TItem item)
		{
			if (_lifetimeContainer.Contains(item))
			{
				ThrowIfItemRemovalIsNotPermitted(item);

				_builder.TearDown(_locator, item);
				_lifetimeContainer.Remove(item);

				List<object> keysToRemove = new List<object>();

				foreach (KeyValuePair<object, object> pair in _locator)
					if (pair.Value.Equals(item))
						keysToRemove.Add(pair.Key);

				foreach (object key in keysToRemove)
					_locator.Remove(key);
			}
		}

		private static void ThrowIfItemRemovalIsNotPermitted(TItem item)
		{
			if (item is CompositionContainer)
				throw new ArgumentException(Resources.NoRemoveContainerFromManagedObjectCollection, "item");
		}

		private TItem Build(Type typeToBuild, string idToBuild, object item)
		{
			if (idToBuild != null && Contains(idToBuild, SearchMode.Local, true))
				throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, Resources.DuplicateID, idToBuild));

			if (item != null &&
			    ReferenceEquals(item, _locator.Get(new DependencyResolutionLocatorKey(typeof (CompositionContainer), null))))
				throw new ArgumentException(Resources.CannotAddContainerToItself, "item");

			if (item == null)
				item = BuildFirstTimeItem(typeToBuild, idToBuild, null);
			else if (!_lifetimeContainer.Contains(item))
				item = BuildFirstTimeItem(typeToBuild, idToBuild, item);
			else
				BuildRepeatedItem(typeToBuild, idToBuild, item);

			return (TItem) item;
		}

		/// <summary>
		/// Used to build a first time item (either an existing one or a new one). The Builder will
		/// end up locating it, and we add it to the container.
		/// </summary>
		private object BuildFirstTimeItem(Type typeToBuild, string idToBuild, object item)
		{
			return _builder.BuildUp(_locator, typeToBuild, NormalizeID(idToBuild), item);
		}

		/// <summary>
		/// Used to "build" an item we've already seen once. We don't use the builder, because that
		/// would do double-injection. Since it's already in the lifetime container, all we need to
		/// do is add a second locator registration for it for the right name.
		/// </summary>
		private void BuildRepeatedItem(Type typeToBuild, string idToBuild, object item)
		{
			_locator.Add(new DependencyResolutionLocatorKey(typeToBuild, NormalizeID(idToBuild)), item);
		}

		private bool PassesFilter(object item)
		{
			if (_filter != null)
				return _filter((TItem) item);
			else
				return true;
		}

		private string NormalizeID(string idToBuild)
		{
			return idToBuild ?? Guid.NewGuid().ToString();
		}

		private void Container_ItemAdded(object sender, DataEventArgs<object> e)
		{
			if (Added != null && e.Data is TItem)
			{
				TItem value = (TItem) e.Data;
				DependencyResolutionLocatorKey key = FindObjectInLocator(value);

				if (key != null && key.ID != null && PassesFilter(value))
					Added(this, new DataEventArgs<TItem>(value));
			}
		}

		private void Container_ItemRemoved(object sender, DataEventArgs<object> e)
		{
			if (Removed != null && e.Data is TItem)
			{
				TItem value = (TItem) e.Data;
				DependencyResolutionLocatorKey key = FindObjectInLocator(value);

				if (key != null && key.ID != null && PassesFilter(value))
					Removed(this, new DataEventArgs<TItem>(value));
			}
		}

		private DependencyResolutionLocatorKey FindObjectInLocator(object value)
		{
			foreach (KeyValuePair<object, object> pair in _locator)
				if (pair.Key is DependencyResolutionLocatorKey && pair.Value == value)
					return ((DependencyResolutionLocatorKey) pair.Key);

			return null;
		}
	}
}
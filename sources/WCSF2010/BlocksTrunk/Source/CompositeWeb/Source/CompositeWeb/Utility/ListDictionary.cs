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

namespace Microsoft.Practices.CompositeWeb.Utility
{
	/// <summary>
	/// A dictionary of lists.
	/// </summary>
	/// <typeparam name="TKey">The key to use for lists.</typeparam>
	/// <typeparam name="TValue">The type of the value held by lists.</typeparam>
	public class ListDictionary<TKey, TValue> : IDictionary<TKey, List<TValue>>, IEnumerable
	{
		private Dictionary<TKey, List<TValue>> _innerValues = new Dictionary<TKey, List<TValue>>();

		#region IDictionary<TKey,List<TValue>> Members

		/// <summary>
		/// Gets or sets the list associated with the given key. The 
		/// access always succeeds, eventually returning an empty list.
		/// </summary>
		/// <param name="key">The key of the list to access.</param>
		/// <returns>The list associated with the key.</returns>
		[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
		public List<TValue> this[TKey key]
		{
			get
			{
				if (_innerValues.ContainsKey(key) == false)
				{
					_innerValues.Add(key, new List<TValue>());
				}
				return _innerValues[key];
			}
			set
			{
				Guard.ArgumentNotNull(value, "value");
				_innerValues[key] = value;
			}
		}

		/// <summary>
		/// See <see cref="IDictionary{TKey,TValue}.Add"/> for more information.
		/// </summary>
		void IDictionary<TKey, List<TValue>>.Add(TKey key, List<TValue> value)
		{
			Guard.ArgumentNotNull(key, "key");
			Guard.ArgumentNotNull(value, "value");
			_innerValues.Add(key, value);
		}

		/// <summary>
		/// See <see cref="IDictionary{TKey,TValue}.TryGetValue"/> for more information.
		/// </summary>
		bool IDictionary<TKey, List<TValue>>.TryGetValue(TKey key, out List<TValue> value)
		{
			value = this[key];
			return true;
		}

		/// <summary>
		/// See <see cref="IDictionary{TKey,TValue}.Values"/> for more information.
		/// </summary>
		ICollection<List<TValue>> IDictionary<TKey, List<TValue>>.Values
		{
			get { return _innerValues.Values; }
		}

		/// <summary>
		/// See <see cref="ICollection{TValue}.Add"/> for more information.
		/// </summary>
		void ICollection<KeyValuePair<TKey, List<TValue>>>.Add(KeyValuePair<TKey, List<TValue>> item)
		{
			((ICollection<KeyValuePair<TKey, List<TValue>>>) _innerValues).Add(item);
		}

		/// <summary>
		/// See <see cref="ICollection{TValue}.Contains"/> for more information.
		/// </summary>
		bool ICollection<KeyValuePair<TKey, List<TValue>>>.Contains(KeyValuePair<TKey, List<TValue>> item)
		{
			return ((ICollection<KeyValuePair<TKey, List<TValue>>>) _innerValues).Contains(item);
		}

		/// <summary>
		/// See <see cref="ICollection{TValue}.CopyTo"/> for more information.
		/// </summary>
		void ICollection<KeyValuePair<TKey, List<TValue>>>.CopyTo(KeyValuePair<TKey, List<TValue>>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<TKey, List<TValue>>>) _innerValues).CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// See <see cref="ICollection{TValue}.IsReadOnly"/> for more information.
		/// </summary>
		bool ICollection<KeyValuePair<TKey, List<TValue>>>.IsReadOnly
		{
			get { return ((ICollection<KeyValuePair<TKey, List<TValue>>>) _innerValues).IsReadOnly; }
		}

		/// <summary>
		/// See <see cref="ICollection{TValue}.Remove"/> for more information.
		/// </summary>
		bool ICollection<KeyValuePair<TKey, List<TValue>>>.Remove(KeyValuePair<TKey, List<TValue>> item)
		{
			return ((ICollection<KeyValuePair<TKey, List<TValue>>>) _innerValues).Remove(item);
		}

		/// <summary>
		/// See <see cref="IEnumerable{TValue}.GetEnumerator"/> for more information.
		/// </summary>
		IEnumerator<KeyValuePair<TKey, List<TValue>>> IEnumerable<KeyValuePair<TKey, List<TValue>>>.GetEnumerator()
		{
			return _innerValues.GetEnumerator();
		}

		/// <summary>
		/// See <see cref="System.Collections.IEnumerable.GetEnumerator"/> for more information.
		/// </summary>
		public IEnumerator GetEnumerator()
		{
			return _innerValues.GetEnumerator();
		}

		#region Public Methods

		/// <summary>
		/// Removes all entries in the dictionary.
		/// </summary>
		public void Clear()
		{
			_innerValues.Clear();
		}

		/// <summary>
		/// Determines whether the dictionary contains the given key.
		/// </summary>
		/// <param name="key">The key to locate.</param>
		/// <returns>true if the dictionary contains the given key; otherwise, false.</returns>
		public bool ContainsKey(TKey key)
		{
			Guard.ArgumentNotNull(key, "key");
			return _innerValues.ContainsKey(key);
		}

		/// <summary>
		/// Removes a list by key.
		/// </summary>
		/// <param name="key">The key of the list to remove.</param>
		/// <returns></returns>
		public bool Remove(TKey key)
		{
			Guard.ArgumentNotNull(key, "key");
			return _innerValues.Remove(key);
		}

		/// <summary>
		/// If a list does not already exist, it will be created automatically.
		/// </summary>
		/// <param name="key">The key of the list that will hold the value.</param>
		public void Add(TKey key)
		{
			Guard.ArgumentNotNull(key, "key");

			CreateNewList(key);
		}

		/// <summary>
		/// Adds a value to a list with the given key. If a list does not already exist, 
		/// it will be created automatically.
		/// </summary>
		/// <param name="key">The key of the list that will hold the value.</param>
		/// <param name="value">The value to add to the list under the given key.</param>
		public void Add(TKey key, TValue value)
		{
			Guard.ArgumentNotNull(key, "key");

			if (_innerValues.ContainsKey(key))
			{
				_innerValues[key].Add(value);
			}
			else
			{
				List<TValue> values = CreateNewList(key);
				values.Add(value);
			}
		}

		private List<TValue> CreateNewList(TKey key)
		{
			List<TValue> values = new List<TValue>();
			_innerValues.Add(key, values);

			return values;
		}

		/// <summary>
		/// Determines whether the dictionary contains the specified value.
		/// </summary>
		/// <param name="value">The value to locate.</param>
		/// <returns>true if the dictionary contains the value in any list; otherwise, false.</returns>
		public bool ContainsValue(TValue value)
		{
			foreach (KeyValuePair<TKey, List<TValue>> pair in _innerValues)
			{
				if (pair.Value.Contains(value))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Retrieves the all the elements from the list which have a key that matches the condition 
		/// defined by the specified predicate.
		/// </summary>
		/// <param name="keyFilter">The filter with the condition to use to filter lists by their key.</param>
		/// <returns></returns>
		public IEnumerable<TValue> FindAllValuesByKey(Predicate<TKey> keyFilter)
		{
			foreach (KeyValuePair<TKey, List<TValue>> pair in this)
			{
				if (keyFilter(pair.Key))
				{
					foreach (TValue value in pair.Value)
					{
						yield return value;
					}
				}
			}
		}

		/// <summary>
		/// Retrieves all the elements that match the condition defined by the specified predicate.
		/// </summary>
		/// <param name="valueFilter">The filter with the condition to use to filter values.</param>
		/// <returns></returns>
		public IEnumerable<TValue> FindAllValues(Predicate<TValue> valueFilter)
		{
			foreach (KeyValuePair<TKey, List<TValue>> pair in this)
			{
				foreach (TValue value in pair.Value)
				{
					if (valueFilter(value))
					{
						yield return value;
					}
				}
			}
		}

		/// <summary>
		/// Removes a value from the list with the given key.
		/// </summary>
		/// <param name="key">The key of the list where the value exists.</param>
		/// <param name="value">The value to remove.</param>
		public void Remove(TKey key, TValue value)
		{
			Guard.ArgumentNotNull(key, "key");
			Guard.ArgumentNotNull(value, "value");

			if (_innerValues.ContainsKey(key))
			{
				_innerValues[key].RemoveAll(delegate(TValue item) { return value.Equals(item); });
			}
		}

		/// <summary>
		/// Removes a value from all lists where it may be found.
		/// </summary>
		/// <param name="value">The value to remove.</param>
		public void Remove(TValue value)
		{
			foreach (KeyValuePair<TKey, List<TValue>> pair in _innerValues)
			{
				Remove(pair.Key, value);
			}
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets a shallow copy of all values in all lists.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
		public List<TValue> Values
		{
			get
			{
				List<TValue> values = new List<TValue>();
				foreach (IEnumerable<TValue> list in _innerValues.Values)
				{
					values.AddRange(list);
				}

				return values;
			}
		}

		/// <summary>
		/// Gets the list of keys in the dictionary.
		/// </summary>
		public ICollection<TKey> Keys
		{
			get { return _innerValues.Keys; }
		}

		/// <summary>
		/// Gets the number of lists in the dictionary.
		/// </summary>
		public int Count
		{
			get { return _innerValues.Count; }
		}

		#endregion

		#endregion
	}
}
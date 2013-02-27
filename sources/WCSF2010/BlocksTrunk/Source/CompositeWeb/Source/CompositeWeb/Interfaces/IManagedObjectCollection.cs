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

namespace Microsoft.Practices.CompositeWeb.Interfaces
{
	/// <summary>
	/// Defines the contract for a managed collection of items of a specified type.
	/// </summary>
	/// <typeparam name="TItem">The type of the items the collection manages.</typeparam>
	public interface IManagedObjectCollection<TItem> : ICollection, IEnumerable<KeyValuePair<string, TItem>>
	{
		/// <summary>
		/// Gets the item indexed by the specified index.
		/// </summary>
		/// <param name="index">The index of the item.</param>
		/// <returns>The located item.</returns>
		TItem this[string index] { get; }

		/// <summary>
		/// Creates and adds a new item to the collection.
		/// </summary>
		/// <typeparam name="TTypeToBuild">The type of the item to create.</typeparam>
		/// <returns>The instance of the created item.</returns>
		TTypeToBuild AddNew<TTypeToBuild>()
			where TTypeToBuild : TItem;

		/// <summary>
		/// Created and adds a new item to the collection with the specified id.
		/// </summary>
		/// <typeparam name="TTypeToBuild">The type of the item to create.</typeparam>
		/// <param name="id">The id for the item.</param>
		/// <returns>The instance of the created item.</returns>
		TTypeToBuild AddNew<TTypeToBuild>(string id)
			where TTypeToBuild : TItem;

		/// <summary>
		/// Searches the collection for item of the specified type.
		/// </summary>
		/// <typeparam name="TSearchType">The type of item to search for.</typeparam>
		/// <returns>A <see cref="ICollection{T}"/>.</returns>
		ICollection<TSearchType> FindByType<TSearchType>()
			where TSearchType : TItem;

		/// <summary>
		/// Searches the collection for item of the specified type.
		/// </summary>
		/// <param name="searchType">The type of item to search for.</param>
		/// <returns>A <see cref="ICollection{T}"/>.</returns>
		ICollection<TItem> FindByType(Type searchType);

		/// <summary>
		/// Gets the item indexed by the specified id.
		/// </summary>
		/// <param name="id">The id of the item.</param>
		/// <returns>The located item.</returns>
		TItem Get(string id);
	}
}
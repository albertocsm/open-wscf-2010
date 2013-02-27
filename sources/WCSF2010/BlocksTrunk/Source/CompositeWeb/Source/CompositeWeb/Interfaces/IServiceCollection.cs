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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Practices.CompositeWeb.Interfaces
{
	/// <summary>
	/// Defines a managed collection of services.
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
	public interface IServiceCollection
	{
		/// <summary>
		/// Adds a service to the collection.
		/// </summary>
		/// <typeparam name="TService">The type of the service.</typeparam>
		/// <param name="serviceInstance">The service instance to add.</param>
		void Add<TService>(TService serviceInstance);

		/// <summary>
		/// Adds a service to the collection.
		/// </summary>
		/// <param name="serviceType">The type of the service.</param>
		/// <param name="serviceInstance">The service instance to add.</param>
		void Add(Type serviceType, object serviceInstance);

		/// <summary>
		/// Creates and adds a service to the collection.
		/// </summary>
		/// <typeparam name="TService">The type of the service.</typeparam>
		/// <typeparam name="TRegisterAs">The type to register as service.</typeparam>
		/// <returns>The created service instance.</returns>
		/// <remarks>This method is used to register a service under a different type than the service type.</remarks>
		[SuppressMessage("Microsoft.Usage", "CA2223:MembersShouldDifferByMoreThanReturnType")]
		TService AddNew<TService, TRegisterAs>() where TService : TRegisterAs;

		/// <summary>
		/// Creates and adds a service to the collection.
		/// </summary>
		/// <typeparam name="TService">The type of the service to create.</typeparam>
		/// <returns>The created service instance.</returns>
		[SuppressMessage("Microsoft.Usage", "CA2223:MembersShouldDifferByMoreThanReturnType")]
		TService AddNew<TService>();

		/// <summary>
		/// Creates and adds a service to the collection.
		/// </summary>
		/// <param name="serviceType">The type of the service.</param>
		/// <param name="registerAs">The type to register as service.</param>
		/// <returns>The created service instance.</returns>
		/// <remarks>This method is used to register a service under a different type than the service type.</remarks>
		object AddNew(Type serviceType, Type registerAs);

		/// <summary>
		/// Creates and adds a service to the collection.
		/// </summary>
		/// <param name="serviceType">The type of the service to create.</param>
		/// <returns>The created service instance.</returns>
		object AddNew(Type serviceType);

		/// <summary>
		/// Adds a service that will not be created until the first time it is requested.
		/// </summary>
		/// <typeparam name="TService">The type of service</typeparam>
		/// <typeparam name="TRegisterAs">The type to register the service as</typeparam>
		[SuppressMessage("Microsoft.Usage", "CA2223:MembersShouldDifferByMoreThanReturnType")]
		[SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		void AddOnDemand<TService, TRegisterAs>();

		/// <summary>
		/// Adds a service that will not be created until the first time it is requested.
		/// </summary>
		/// <typeparam name="TService">The type of service</typeparam>
		[SuppressMessage("Microsoft.Usage", "CA2223:MembersShouldDifferByMoreThanReturnType")]
		[SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		void AddOnDemand<TService>();

		/// <summary>
		/// Adds a service that will not be created until the first time it is requested.
		/// </summary>
		/// <param name="serviceType">The type of service</param>
		/// <param name="registerAs">The type to register the service as</param>
		[SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		void AddOnDemand(Type serviceType, Type registerAs);

		/// <summary>
		/// Adds a service that will not be created until the first time it is requested.
		/// </summary>
		/// <param name="serviceType">The type of service</param>
		[SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		void AddOnDemand(Type serviceType);

		/// <summary>
		/// Determines whether the given service type exists in the collection.
		/// </summary>
		/// <param name="serviceType">Type of service to search for.</param>
		/// <returns>
		/// <see langword="true"/>if the serviceType exists; <see langword="false"/>.
		/// </returns>
		bool Contains(Type serviceType);

		/// <summary>
		/// Determines whether the given service type exists in the collection.
		/// </summary>
		/// <typeparam name="TService">Type of service to search for.</typeparam>
		/// <returns>
		/// <see langword="true"/>if the serviceType exists; <see langword="false"/>.
		/// </returns>
		bool Contains<TService>();

		/// <summary>
		/// Determines whether the given service type exists directly in the local collection. The parent
		/// collections are not consulted.
		/// </summary>
		/// <param name="serviceType">Type of service to search for.</param>
		/// <returns>
		/// <see langword="true"/>if the serviceType exists; <see langword="false"/>.
		/// </returns>
		bool ContainsLocal(Type serviceType);

		/// <summary>
		/// Gets a service.
		/// </summary>
		/// <param name="serviceType">The type of the service to be found.</param>
		/// <returns>The service instance, if present; null otherwise.</returns>
		object Get(Type serviceType);

		/// <summary>
		/// Gets a service.
		/// </summary>
		/// <param name="serviceType">The type of the service to be found.</param>
		/// <param name="ensureExists">If true, will throw an exception if the service is not found.</param>
		/// <returns>The service instance, if present; null if not (and ensureExists is false).</returns>
		/// <exception cref="ServiceMissingException">Thrown if ensureExists is true and the service is not found.</exception>
		object Get(Type serviceType, bool ensureExists);

		/// <summary>
		/// Gets a service.
		/// </summary>
		/// <typeparam name="TService">The type of the service to be found.</typeparam>
		/// <param name="ensureExists">If true, will throw an exception if the service is not found.</param>
		/// <returns>The service instance, if present; null if not (and ensureExists is false).</returns>
		/// <exception cref="ServiceMissingException">Thrown if ensureExists is true and the service is not found.</exception>
		TService Get<TService>(bool ensureExists);

		/// <summary>
		/// Gets a service.
		/// </summary>
		/// <typeparam name="TService">The type of the service to be found.</typeparam>
		/// <returns>The service instance, if present; null otherwise.</returns>
		TService Get<TService>();

		/// <summary>
		/// Enumerates through all seen types and retrieves a KeyValuePair for each. 
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
		IEnumerator<KeyValuePair<Type, object>> GetEnumerator();

		/// <summary>
		/// Removes a service registration from the <see cref="CompositionContainer"/>.
		/// </summary>
		/// <param name="serviceType">The service type to remove.</param>
		void Remove(Type serviceType);

		/// <summary>
		/// Removes a service registration from the <see cref="CompositionContainer"/>.
		/// </summary>
		/// <typeparam name="TService">The service type to remove.</typeparam>
		void Remove<TService>();
	}
}
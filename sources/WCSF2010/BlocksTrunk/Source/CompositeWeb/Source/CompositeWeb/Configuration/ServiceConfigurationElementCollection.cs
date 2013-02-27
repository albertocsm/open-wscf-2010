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
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.Practices.CompositeWeb.Utility;

namespace Microsoft.Practices.CompositeWeb.Configuration
{
	/// <summary>
	/// A collection of <see cref="ServiceConfigurationElement"/>.
	/// </summary>
	[SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface")]
	public class ServiceConfigurationElementCollection : ConfigurationElementCollection
	{
		/// <summary>
		/// Initializes a new instance of <see cref="ServiceConfigurationElementCollection"/>.
		/// </summary>
		public ServiceConfigurationElementCollection()
		{
		}

		/// <summary>
		/// Initializes a new <see cref="ServiceConfigurationElementCollection"/>.
		/// </summary>
		/// <param name="services">The initial set of <see cref="ServiceConfigurationElement"/>.</param>
		[SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		[SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
		public ServiceConfigurationElementCollection(ServiceConfigurationElement[] services)
		{
			Guard.ArgumentNotNull(services, "services");

			foreach (ServiceConfigurationElement service in services)
			{
				BaseAdd(service);
			}
		}


		/// <summary>
		/// Indicates that an exception should be raised if a duplicate element is found.
		/// </summary>
		protected override bool ThrowOnDuplicate
		{
			get { return true; }
		}

		/// <summary>
		/// Gets the type of <see cref="ConfigurationElementCollectionType"/>.
		/// </summary>
		public override ConfigurationElementCollectionType CollectionType
		{
			get { return ConfigurationElementCollectionType.BasicMap; }
		}

		/// <summary>
		/// Gets the name to identify this collection in the configuration.
		/// </summary>
		protected override string ElementName
		{
			get { return "service"; }
		}

		/// <summary>
		/// Gets the <see cref="ServiceConfigurationElement"/> located at the specified index in the collection.
		/// </summary>
		/// <param name="index">The index of the element in the collection.</param>
		/// <returns>A <see cref="ModuleConfigurationElement"/>.</returns>
		public ServiceConfigurationElement this[int index]
		{
			get { return (ServiceConfigurationElement) base.BaseGet(index); }
		}

		/// <summary>
		/// Adds a <see cref="ServiceConfigurationElement"/> to the collection.
		/// </summary>
		/// <param name="service">A <see cref="ServiceConfigurationElement"/> instance.</param>
		public void Add(ServiceConfigurationElement service)
		{
			BaseAdd(service);
		}

		/// <summary>
		/// Tests if the collection contains the configuration for the specified type.
		/// </summary>
		/// <param name="type">The type to search the configuration for.</param>
		/// <returns><see langword="true"/> if a configuration for the type is present; otherwise <see langword="false"/>.</returns>
		public bool Contains(Type type)
		{
			return base.BaseGet(type) != null;
		}

		/// <summary>
		/// Searches the collection for all the <see cref="ServiceConfigurationElement"/> that match the specified predicate.
		/// </summary>
		/// <param name="match">A <see cref="Predicate{T}"/> that implements the match test.</param>
		/// <returns>A <see cref="List{T}"/> with the successful matches.</returns>
		public IList<ServiceConfigurationElement> FindAll(Predicate<ServiceConfigurationElement> match)
		{
			IList<ServiceConfigurationElement> found = new List<ServiceConfigurationElement>();
			foreach (ServiceConfigurationElement serviceElement in this)
			{
				if (match(serviceElement))
				{
					found.Add(serviceElement);
				}
			}
			return found;
		}

		/// <summary>
		/// Creates a new <see cref="ServiceConfigurationElement"/>.
		/// </summary>
		/// <returns>A <see cref="ServiceConfigurationElement"/>.</returns>
		protected override ConfigurationElement CreateNewElement()
		{
			return new ServiceConfigurationElement();
		}

		/// <summary>
		/// Gets the element key for specified element.
		/// </summary>
		/// <param name="element">The <see cref="ConfigurationElement"/> to get the key for.</param>
		/// <returns>An <see langword="object"/>.</returns>
		protected override object GetElementKey(ConfigurationElement element)
		{
			string key = string.Format(CultureInfo.InvariantCulture, "{0}|{1}",
			                           ((ServiceConfigurationElement) element).RegisterAs.GetHashCode(),
			                           ((ServiceConfigurationElement) element).Type.GetHashCode());
			return key;
		}
	}
}
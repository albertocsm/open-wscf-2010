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
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Practices.CompositeWeb.Utility;

namespace Microsoft.Practices.CompositeWeb.Configuration
{
	/// <summary>
	/// A collection of <see cref="ModuleDependencyConfigurationElement"/>.
	/// </summary>
	[SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface")]
	public class ModuleDependencyCollection : ConfigurationElementCollection
	{
		/// <summary>
		/// Initializes a new instance of <see cref="ModuleDependencyCollection"/>.
		/// </summary>
		public ModuleDependencyCollection()
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="ModuleDependencyCollection"/>.
		/// </summary>
		/// <param name="dependencies">An array of <see cref="ModuleDependencyConfigurationElement"/> with initial list of dependencies.</param>
		[SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		[SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods",
			Justification = "Validation done by Guard class.")]
		public ModuleDependencyCollection(ModuleDependencyConfigurationElement[] dependencies)
		{
			Guard.ArgumentNotNull(dependencies, "dependencies");
			foreach (ModuleDependencyConfigurationElement dependency in dependencies)
			{
				BaseAdd(dependency);
			}
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
			get { return "dependency"; }
		}

		/// <summary>
		/// Gets the <see cref="ModuleDependencyConfigurationElement"/> located at the specified index in the collection.
		/// </summary>
		/// <param name="index">The index of the element in the collection.</param>
		/// <returns>A <see cref="ModuleDependencyConfigurationElement"/>.</returns>
		public ModuleDependencyConfigurationElement this[int index]
		{
			get { return (ModuleDependencyConfigurationElement) base.BaseGet(index); }
		}

		/// <summary>
		/// Creates a new <see cref="ModuleDependencyConfigurationElement"/>.
		/// </summary>
		/// <returns>A <see cref="ModuleDependencyConfigurationElement"/>.</returns>
		protected override ConfigurationElement CreateNewElement()
		{
			return new ModuleDependencyConfigurationElement();
		}

		/// <summary>
		/// Gets the element key for specified element.
		/// </summary>
		/// <param name="element">The <see cref="ConfigurationElement"/> to get the key for.</param>
		/// <returns>An <see langword="object"/>.</returns>
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((ModuleDependencyConfigurationElement) element).Module;
		}
	}
}
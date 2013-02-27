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

namespace Microsoft.Practices.CompositeWeb.Configuration
{
	/// <summary>
	/// A collection of <see cref="AuthorizationRuleElement"/>.
	/// </summary>
	[SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface")]
	[ConfigurationCollection(typeof (AuthorizationRuleElement), AddItemName = "rule",
		CollectionType = ConfigurationElementCollectionType.BasicMap)]
	public class AuthorizationRuleCollection : ConfigurationElementCollection
	{
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
			get { return "rule"; }
		}

		/// <summary>
		/// Gets the <see cref="AuthorizationRuleElement"/> located at the specified index in the collection.
		/// </summary>
		/// <param name="index">The index of the element in the collection.</param>
		/// <returns>A <see cref="AuthorizationRuleElement"/>.</returns>
		public AuthorizationRuleElement this[int index]
		{
			get { return (AuthorizationRuleElement) base.BaseGet(index); }
		}

		/// <summary>
		/// Gets the <see cref="AuthorizationRuleElement"/> indexed by the specified key in the collection.
		/// </summary>
		/// <param name="key">The key to the element.</param>
		/// <returns>A <see cref="AuthorizationRuleElement"/>.</returns>
		public new AuthorizationRuleElement this[string key]
		{
			get { return (AuthorizationRuleElement) base.BaseGet(key); }
		}

		/// <summary>
		/// Creates a new <see cref="AuthorizationRuleElement"/>.
		/// </summary>
		/// <returns>A <see cref="AuthorizationRuleElement"/>.</returns>
		protected override ConfigurationElement CreateNewElement()
		{
			return new AuthorizationRuleElement();
		}

		/// <summary>
		/// Gets the element key for specified element.
		/// </summary>
		/// <param name="element">The <see cref="ConfigurationElement"/> to get the key for.</param>
		/// <returns>An <see langword="object"/>.</returns>
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((AuthorizationRuleElement) element).AbsolutePath;
		}
	}
}
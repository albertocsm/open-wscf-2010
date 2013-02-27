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
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Practices.CompositeWeb.Configuration
{
	/// <summary>
	/// A configuration element to declare service metadata.
	/// </summary>
	public class ServiceConfigurationElement : ConfigurationElement
	{
		/// <summary>
		/// Initializes a new instance of <see cref="ServiceConfigurationElement"/>.
		/// </summary>
		public ServiceConfigurationElement()
			: base()
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="ServiceConfigurationElement"/>.
		/// </summary>
		/// <param name="registerAs">The service contract</param>
		/// <param name="type">The type of the service</param>
		/// <param name="scope">The scope of the service (Global or Module)</param>
		[SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public ServiceConfigurationElement(Type registerAs, Type type, string scope)
		{
			base["registerAs"] = registerAs;
			base["type"] = type;
			base["scope"] = scope;
		}

		/// <summary>
		/// Gets or sets the service contract.
		/// </summary>
		[ConfigurationProperty("registerAs", IsRequired = true)]
		[TypeConverter(typeof (TypeNameConverter))]
		public Type RegisterAs
		{
			get { return (Type) base["registerAs"]; }
			set { base["registerAs"] = value; }
		}

		/// <summary>
		/// Gets or sets the service type.
		/// </summary>
		[ConfigurationProperty("type", IsRequired = true)]
		[TypeConverter(typeof (TypeNameConverter))]
		public Type Type
		{
			get { return (Type) base["type"]; }
			set { base["type"] = value; }
		}

		/// <summary>
		/// Gets or sets the scope of the service
		/// </summary>
		[ConfigurationProperty("scope", IsRequired = false, DefaultValue = "Module")]
		public string Scope
		{
			get { return (string) base["scope"]; }
			set { base["scope"] = value; }
		}
	}
}
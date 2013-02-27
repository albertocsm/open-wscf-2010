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
	/// A <see cref="ConfigurationElement"/> for module dependencies.
	/// </summary>
	public class ModuleDependencyConfigurationElement : ConfigurationElement
	{
		/// <summary>
		/// Initializes a new instance of <see cref="ModuleDependencyConfigurationElement"/>.
		/// </summary>
		public ModuleDependencyConfigurationElement()
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="ModuleDependencyConfigurationElement"/>.
		/// </summary>
		/// <param name="moduleName">A module name.</param>
		[SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public ModuleDependencyConfigurationElement(string moduleName)
		{
			base["module"] = moduleName;
		}

		/// <summary>
		/// Gets or sets the name of a module antoher module depends on.
		/// </summary>
		[ConfigurationProperty("module", IsRequired = true, IsKey = true)]
		public string Module
		{
			get { return (string) base["module"]; }
			set { base["module"] = value; }
		}
	}
}
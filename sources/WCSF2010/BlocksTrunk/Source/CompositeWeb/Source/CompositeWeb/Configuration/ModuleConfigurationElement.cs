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
	/// A configuration element to declare module metadata.
	/// </summary>
	public class ModuleConfigurationElement : ConfigurationElement
	{
		/// <summary>
		/// Initializes a new instance of <see cref="ModuleConfigurationElement"/>.
		/// </summary>
		public ModuleConfigurationElement()
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="ModuleConfigurationElement"/>.
		/// </summary>
		/// <param name="name">The name of the module.</param>
		/// <param name="assemblyName">The module assembly.</param>
		/// <param name="virtualPath">The module location in the web site.</param>
		[SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public ModuleConfigurationElement(string name, string assemblyName, string virtualPath)
		{
			base["name"] = name;
			base["assemblyName"] = assemblyName;
			base["virtualPath"] = virtualPath;
		}

		/// <summary>
		/// Gets or sets the module name.
		/// </summary>
		[ConfigurationProperty("name", IsRequired = true)]
		public string Name
		{
			get { return (string) base["name"]; }
			set { base["name"] = value; }
		}

		/// <summary>
		/// Gets or sets the module assembly name.
		/// </summary>
		[ConfigurationProperty("assemblyName", IsRequired = true)]
		public string AssemblyName
		{
			get { return (string) base["assemblyName"]; }
			set { base["assemblyName"] = value; }
		}

		/// <summary>
		/// Gets or sets the module location.
		/// </summary>
		[ConfigurationProperty("virtualPath")]
		public string VirtualPath
		{
			get { return (string) base["virtualPath"]; }
			set { base["virtualPath"] = value; }
		}

		/// <summary>
		/// Gets or sets the modules this module depends on.
		/// </summary>
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		[ConfigurationProperty("dependencies", IsDefaultCollection = true, IsKey = false)]
		public ModuleDependencyCollection Dependencies
		{
			get { return (ModuleDependencyCollection) base["dependencies"]; }
			set { base["dependencies"] = value; }
		}

		/// <summary>
		/// Gets or sets the services this module will register.
		/// </summary>
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		[ConfigurationProperty("services", IsDefaultCollection = true, IsKey = false)]
		public ServiceConfigurationElementCollection Services
		{
			get { return (ServiceConfigurationElementCollection) base["services"]; }
			set { base["services"] = value; }
		}
	}
}
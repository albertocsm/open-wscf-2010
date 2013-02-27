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
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Practices.CompositeWeb.Configuration
{
	/// <summary>
	/// Extends the <see cref="ModuleInfo"/> metadata to support module dependencies.
	/// </summary>
	public class DependantModuleInfo : ModuleInfo
	{
		private ModuleDependency[] _dependencies;
		private ServiceInfo[] _services;

		/// <summary>
		/// Initializes a new instance of <see cref="DependantModuleInfo"/>
		/// </summary>
		public DependantModuleInfo()
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="DependantModuleInfo"/>
		/// </summary>
		/// <param name="name">The name of the module.</param>
		/// <param name="assemblyName">The module assembly.</param>
		/// <param name="virtualPath">The module location in the web site.</param>
		public DependantModuleInfo(string name, string assemblyName, string virtualPath)
			: base(name, assemblyName, virtualPath)
		{
		}

		/// <summary>
		/// Gets or sets the list of modules this module depends on.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		public ModuleDependency[] Dependencies
		{
			get { return _dependencies; }
			set { _dependencies = value; }
		}

		/// <summary>
		/// Gets or sets the list of modules this module will register.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		public ServiceInfo[] Services
		{
			get { return _services; }
			set { _services = value; }
		}
	}
}
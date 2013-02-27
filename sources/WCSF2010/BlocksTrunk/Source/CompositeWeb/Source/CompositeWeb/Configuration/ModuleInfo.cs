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
using Microsoft.Practices.CompositeWeb.Interfaces;

namespace Microsoft.Practices.CompositeWeb.Configuration
{
	/// <summary>
	/// Defines the basic metadata needed to describe a module.
	/// </summary>
	public class ModuleInfo : IModuleInfo
	{
		private string _assemblyName;
		private string _name;
		private string _virtualPath;

		/// <summary>
		/// Initializes a new instance of a <see cref="ModuleInfo"/>.
		/// </summary>
		public ModuleInfo()
		{
		}

		/// <summary>
		/// Initializes a new instance of a <see cref="ModuleInfo"/> with the given values.
		/// </summary>
		/// <param name="name">The name of the module.</param>
		/// <param name="assemblyName">The assembly implementing the module.</param>
		/// <param name="virtualPath">The virtual path to the module location.</param>
		public ModuleInfo(string name, string assemblyName, string virtualPath)
		{
			_name = name;
			_assemblyName = assemblyName;
			_virtualPath = virtualPath;
		}

		#region IModuleInfo Members

		/// <summary>
		/// Gets or sets the module name.
		/// </summary>
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		/// <summary>
		/// Gets or sets the module assembly.
		/// </summary>
		public string AssemblyName
		{
			get { return _assemblyName; }
			set { _assemblyName = value; }
		}

		/// <summary>
		/// Gets or sets the module location virtual path.
		/// </summary>
		public string VirtualPath
		{
			get { return _virtualPath; }
			set { _virtualPath = value; }
		}

		#endregion
	}
}
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
using System.Web.Configuration;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.CompositeWeb.Services
{
	/// <summary>
	/// Implements the <see cref="IModuleConfigurationLocatorService"/>.
	/// </summary>
	public class ModuleConfigurationLocatorService : IModuleConfigurationLocatorService
	{
		private CompositionContainer _container;

		/// <summary>
		/// Initializes a new instance of <see cref="ModuleConfigurationLocatorService"/>.
		/// </summary>
		/// <param name="container">The container for this service.</param>
		[InjectionConstructor]
		public ModuleConfigurationLocatorService([Dependency] CompositionContainer container)
		{
			_container = container;
		}

		/// <summary>
		/// Gets the <see cref="IModuleEnumerator"/>.
		/// </summary>
		protected IModuleEnumerator ModuleEnumerator
		{
			get { return _container.Services.Get<IModuleEnumerator>(true); }
		}

		/// <summary>
		/// Gets the <see cref="IModuleLoaderService"/>.
		/// </summary>
		protected IModuleLoaderService ModuleLoaderService
		{
			get { return _container.Services.Get<IModuleLoaderService>(true); }
		}

		#region IModuleConfigurationLocatorService Members

		/// <summary>
		/// Gets the <see cref="System.Configuration.Configuration"/> for the specified module name.
		/// </summary>
		/// <param name="moduleName">The name of the module to lookup the configuration for.</param>
		/// <returns>A <see cref="System.Configuration.Configuration"/> instance.</returns>
		public System.Configuration.Configuration FindModuleConfiguration(string moduleName)
		{
			foreach (IModuleInfo moduleInfo in ModuleEnumerator.EnumerateModules())
			{
				if (String.Equals(moduleInfo.Name, moduleName, StringComparison.CurrentCultureIgnoreCase))
				{
					if (!String.IsNullOrEmpty(moduleInfo.VirtualPath))
					{
						return WebConfigurationManager.OpenWebConfiguration(moduleInfo.VirtualPath);
					}
				}
			}
			return null;
		}

		#endregion
	}
}
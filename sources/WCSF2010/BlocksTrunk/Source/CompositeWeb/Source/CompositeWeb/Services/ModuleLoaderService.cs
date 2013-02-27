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
using System.Globalization;
using System.Reflection;
using Microsoft.Practices.CompositeWeb.Configuration;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Properties;
using Microsoft.Practices.CompositeWeb.Utility;

namespace Microsoft.Practices.CompositeWeb.Services
{
	/// <summary>
	/// Implements the <see cref="IModuleLoaderService"/> interface.
	/// </summary>
	public class ModuleLoaderService : IModuleLoaderService
	{
		private Dictionary<string, IModuleInitializer> _modules = new Dictionary<string, IModuleInitializer>();

		#region IModuleLoaderService Members

		/// <summary>
		/// Loads the specified modules into the given container.
		/// </summary>
		/// <param name="rootContainer">The container into wich create and add the module published services.</param>
		/// <param name="modulesInfo">The list of modules to load.</param>
		/// <remarks>A <see cref="CompositionContainer"/> is created for every module in the list and added to the 
		/// <paramref name="rootContainer"/>. The Load() method is called on every module that
		/// exposes a <see cref="IModuleInitializer"/>.</remarks>
		public void Load(CompositionContainer rootContainer, params IModuleInfo[] modulesInfo)
		{
			Guard.ArgumentNotNull(rootContainer, "compositionContainer");
			Guard.ArgumentNotNull(modulesInfo, "modules");

			foreach (IModuleInfo moduleInfo in modulesInfo)
			{
				Assembly moduleAssembly = Assembly.Load(moduleInfo.AssemblyName);

				CompositionContainer container = String.IsNullOrEmpty(moduleInfo.VirtualPath)
				                                 	? rootContainer
				                                 	:
				                                 		rootContainer.Containers.AddNew<CompositionContainer>(moduleInfo.Name);

				LoadServices(container, moduleInfo);

				foreach (Type t in moduleAssembly.GetTypes())
				{
					if (typeof (IModuleInitializer).IsAssignableFrom(t))
					{
						IModuleInitializer init = (IModuleInitializer) container.BuildNewItem(t);
						_modules.Add(moduleInfo.Name, init);
						try
						{
							init.Load(container);
						}
						catch (Exception ex)
						{
							ThrowModuleLoadException(ex, moduleAssembly);
						}
					}
				}
			}
		}

		/// <summary>
		/// Gets the <see cref="IModuleInitializer"/> exposed by the requested module.
		/// </summary>
		/// <param name="moduleName">The name of the module to get its initializer.</param>
		/// <returns>The exposed <see cref="IModuleInitializer"/> if found; otherwise, <see langword="null"/>.</returns>
		public IModuleInitializer FindInitializer(string moduleName)
		{
			Guard.ArgumentNotNullOrEmptyString(moduleName, "moduleName");

			if (_modules.ContainsKey(moduleName)) return _modules[moduleName];
			return null;
		}

		#endregion

		private static void LoadServices(CompositionContainer container, IModuleInfo moduleInfo)
		{
			IServiceLoaderService serviceLoader = container.Services.Get<IServiceLoaderService>();

			if (serviceLoader == null)
				return;

			DependantModuleInfo dependantModuleInfo = moduleInfo as DependantModuleInfo;

			if (dependantModuleInfo != null && dependantModuleInfo.Services != null)
			{
				serviceLoader.Load(container, dependantModuleInfo.Services);
			}
		}

		private static void ThrowModuleLoadException(Exception innerException, Assembly assembly)
		{
			throw new ModuleLoadException(assembly.FullName,
			                              String.Format(CultureInfo.CurrentCulture,
			                                            Resources.FailedToLoadModule,
			                                            assembly.FullName, innerException.Message),
			                              innerException);
		}
	}
}
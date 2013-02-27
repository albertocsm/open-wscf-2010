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
using System.Globalization;
using Microsoft.Practices.CompositeWeb.Configuration;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Properties;
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.CompositeWeb.Services
{
	/// <summary>
	/// Implements a <see cref="IModuleEnumerator"/> that gets the module metadata from
	/// the module configuration files.
	/// </summary>
	public class WebModuleEnumerator : IModuleEnumerator
	{
		private IModuleInfo[] _modules;
		private IModuleInfoStore _store;

		/// <summary>
		/// Initializes a new instance of <see cref="WebModuleEnumerator"/>.
		/// </summary>
		/// <param name="store">The <see cref="IModuleInfoStore"/> to use to get the module configuration.</param>
		[InjectionConstructor]
		public WebModuleEnumerator([ServiceDependency] IModuleInfoStore store)
		{
			_store = store;
		}

		#region IModuleEnumerator Members

		/// <summary>
		/// Gets the list of modules needed by the application.
		/// </summary>
		/// <returns>An array of <see cref="IModuleInfo"/>.</returns>
		public IModuleInfo[] EnumerateModules()
		{
			if (_modules != null) return _modules;

			List<DependantModuleInfo> moduleInfos = new List<DependantModuleInfo>();
			ModulesConfigurationSection section = _store.GetModuleConfigurationSection();
			ValidateDuplicates(section);

			foreach (ModuleConfigurationElement data in section.Modules)
			{
				DependantModuleInfo moduleInfo = new DependantModuleInfo(data.Name, data.AssemblyName, data.VirtualPath);
				if (data.Dependencies.Count > 0)
				{
					moduleInfo.Dependencies = new ModuleDependency[data.Dependencies.Count];
					for (int i = 0; i < data.Dependencies.Count; i++)
					{
						moduleInfo.Dependencies[i] = new ModuleDependency();
						moduleInfo.Dependencies[i].Name = data.Dependencies[i].Module;
					}
				}

				if (data.Services.Count > 0)
				{
					moduleInfo.Services = new ServiceInfo[data.Services.Count];

					for (int i = 0; i < data.Services.Count; i++)
					{
						moduleInfo.Services[i] =
							new ServiceInfo(data.Services[i].RegisterAs, data.Services[i].Type, data.Services[i].Scope);
					}
				}

				moduleInfos.Add(moduleInfo);
			}

			if (moduleInfos.Count > 0)
			{
				_modules = SortModules(moduleInfos);
			}
			else
			{
				_modules = moduleInfos.ToArray();
			}
			return _modules;
		}

		#endregion

		private static void ValidateDuplicates(ModulesConfigurationSection section)
		{
			foreach (ModuleConfigurationElement data in section.Modules)
			{
				ICollection<ModuleConfigurationElement> found = section.Modules.FindAll(delegate(ModuleConfigurationElement match)
				                                                                        	{
				                                                                        		return
				                                                                        			!string.IsNullOrEmpty(match.VirtualPath) &&
				                                                                        			(String.Equals(match.VirtualPath,
				                                                                        			               data.VirtualPath,
				                                                                        			               StringComparison.
				                                                                        			               	InvariantCultureIgnoreCase)
				                                                                        			 ||
				                                                                        			 String.Equals(match.AssemblyName,
				                                                                        			               data.AssemblyName,
				                                                                        			               StringComparison.
				                                                                        			               	InvariantCultureIgnoreCase));
				                                                                        	});
				if (found.Count > 1)
					ThrowConfigurationErrorsException(found);
			}
		}

		private static void ThrowConfigurationErrorsException(ICollection<ModuleConfigurationElement> found)
		{
			string moduleNames = String.Empty;
			foreach (ModuleConfigurationElement duplicatedModule in found)
			{
				if (moduleNames.Length > 0)
				{
					moduleNames = string.Format(CultureInfo.CurrentCulture, "{0}, {1}", moduleNames, duplicatedModule.Name);
				}
				else
				{
					moduleNames += duplicatedModule.Name;
				}
			}
			throw new ConfigurationErrorsException(
				String.Format(CultureInfo.CurrentCulture, Resources.DuplicatedModuleLocationOrAssemblyName, moduleNames));
		}

		private static IModuleInfo[] SortModules(List<DependantModuleInfo> modules)
		{
			List<DependantModuleInfo> result = new List<DependantModuleInfo>();
			Dictionary<string, DependantModuleInfo> index = new Dictionary<string, DependantModuleInfo>();
			ModuleDependencySolver solver = new ModuleDependencySolver();
			foreach (DependantModuleInfo moduleInfo in modules)
			{
				index.Add(moduleInfo.Name, moduleInfo);
				solver.AddModule(moduleInfo.Name);
				if (moduleInfo.Dependencies != null)
				{
					foreach (ModuleDependency dependency in moduleInfo.Dependencies)
					{
						solver.AddDependency(moduleInfo.Name, dependency.Name);
					}
				}
			}
			string[] sortedIndex = solver.Solve();
			for (int i = 0; i < sortedIndex.Length; i++)
			{
				string moduleName = sortedIndex[i];
				result.Add(index[moduleName]);
			}
			return result.ToArray();
		}
	}
}
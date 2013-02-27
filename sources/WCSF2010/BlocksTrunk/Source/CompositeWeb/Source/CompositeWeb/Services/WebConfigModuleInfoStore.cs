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
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Configuration;
using Microsoft.Practices.CompositeWeb.Configuration;
using Microsoft.Practices.CompositeWeb.Interfaces;

namespace Microsoft.Practices.CompositeWeb.Services
{
	/// <summary>
	/// Implements the <see cref="IModuleInfoStore"/> interface.
	/// </summary>
	public class WebConfigModuleInfoStore : IModuleInfoStore
	{
		private string _baseDirectory;

		/// <summary>
		/// Initializes a new instance of <see cref="WebConfigModuleInfoStore"/>.
		/// </summary>
		public WebConfigModuleInfoStore()
			: this(String.Empty)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="WebConfigModuleInfoStore"/>.
		/// </summary>
		/// <param name="baseDirectory">The directory from which to start searching for the 
		/// configuration files.</param>
		public WebConfigModuleInfoStore(string baseDirectory)
		{
			_baseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, baseDirectory);
		}

		#region IModuleInfoStore Members

		/// <summary>
		/// Gets the module configuration data.
		/// </summary>
		/// <returns>A <see cref="ModulesConfigurationSection"/> instance.</returns>
		public ModulesConfigurationSection GetModuleConfigurationSection()
		{
			ModulesConfigurationSection globalSection = new ModulesConfigurationSection();
			PopulateSection(globalSection, _baseDirectory);
			return globalSection;
		}

		#endregion

		private void PopulateSection(ModulesConfigurationSection section, string rootDirectory)
		{
			foreach (string fileName in Directory.GetFiles(rootDirectory, "Web.Config", SearchOption.TopDirectoryOnly))
			{
				System.Configuration.Configuration configuration = GetConfiguration(Path.Combine(rootDirectory, fileName));
				ModulesConfigurationSection localSection =
					(ModulesConfigurationSection) configuration.GetSection("compositeWeb/modules");
				if (localSection != null)
				{
					foreach (ModuleConfigurationElement module in localSection.Modules)
					{
						if (!section.Modules.Contains(module.Name))
						{
							section.Modules.Add(module);
						}
					}
				}
			}
			foreach (string childDirectory in Directory.GetDirectories(rootDirectory))
			{
				PopulateSection(section, childDirectory);
			}
		}

		private static System.Configuration.Configuration GetConfiguration(string configFilePath)
		{
            System.Configuration.Configuration configuration;
            HttpContext context = HttpContext.Current;
            if (context == null)
            {
                configuration = GetConfigurationForCustomFile(configFilePath);
            }
            else
            {
                configuration =
                WebConfigurationManager.OpenWebConfiguration(HttpRuntime.AppDomainAppVirtualPath + "/" +
                                                 configFilePath.Substring(HttpRuntime.AppDomainAppPath.Length));
            }
            return configuration;
        }

		private static System.Configuration.Configuration GetConfigurationForCustomFile(string fileName)
		{
			ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
			fileMap.ExeConfigFilename = fileName;
			File.SetAttributes(fileMap.ExeConfigFilename, FileAttributes.Normal);
			return ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
		}
	}
}
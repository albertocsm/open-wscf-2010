//===============================================================================
// Microsoft patterns & practices
//  GAX Extension Library
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
#region Using Statements
using System;
using System.Configuration;
using Microsoft.Practices.RecipeFramework.VisualStudio;
using Microsoft.Practices.RecipeFramework.Library;
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.Converters
{
	/// <summary>
	/// Provides helper methods for managing connection string settings.
	/// </summary>
	public sealed class ConnectionSettingsInfo
	{
		/// <summary>
		/// Retrieves the connection strings section from the selected project in VS.
		/// </summary>
		public static ConnectionStringsSection GetSection(EnvDTE.DTE vs)
		{
			if (vs == null)
			{
				return null;
			}
			return GetSection(DteHelper.GetSelectedProject(vs));
		}

		/// <summary>
		/// Retrieves the connection strings section from the given project.
		/// </summary>
		public static ConnectionStringsSection GetSection(EnvDTE.Project project)
		{
			if (project == null)
			{
				return null;
			}

			EnvDTE.ProjectItem configItem = null;
			
            
            //string configName;

            //if (DteHelper.IsWebProject(project))
            //{
            //    configName = "Web.config";
            //}
            //else
            //{
            //    configName = "App.config";
            //}
            //configItem = DteHelper.FindItemByName(project.ProjectItems, configName, false);
            //if (configItem == null)
            //{
            //    configItem = DteHelper.FindItemByName(project.ProjectItems, configName.ToLowerInvariant(), false);
            //}

            if(DteHelper.IsWebProject(project))
            {
                try
                {
                    configItem = FindItemByName(project.ProjectItems, "web.config", true);
                }
                catch
                {
                    //With Web projects without any subfolder this method throws an exception
                    configItem = FindItemByName(project.ProjectItems, "web.config", false);
                }
            }
            else
            {
                configItem = FindItemByName(project.ProjectItems, "app.config", true);
            }

			if (configItem == null)
			{
				return null;
			}

			string configFile = configItem.get_FileNames(1);
			return GetSection(configFile);
		}

		/// <summary>
		/// Retrieves the connection strings section from the given file.
		/// </summary>
		public static ConnectionStringsSection GetSection(string fileName)
		{
            System.Configuration.Configuration configurationRoot;
			return GetSection(fileName, out configurationRoot);
		}

		/// <summary>
		/// Retrieves the connection strings section from the given file.
		/// </summary>
		public static ConnectionStringsSection GetSection(string fileName, out System.Configuration.Configuration configurationRoot)
		{
			ExeConfigurationFileMap map = new ExeConfigurationFileMap();
			map.RoamingUserConfigFilename = fileName;
			map.ExeConfigFilename = fileName;
			configurationRoot = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
			return (ConnectionStringsSection)configurationRoot.GetSection("connectionStrings");
		}

        private static EnvDTE.ProjectItem FindItemByName(EnvDTE.ProjectItems collection, string name, bool recursive)
        {
            Guard.ArgumentNotNull(collection, "collection");
            Guard.ArgumentNotNull(name, "name");

            foreach(EnvDTE.ProjectItem item1 in collection)
            {
                if(string.Compare(item1.Name, name, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    return item1;
                }
                if(recursive)
                {
                    EnvDTE.ProjectItem item2 = FindItemByName(item1.ProjectItems, name, recursive);
                    if(item2 != null)
                    {
                        return item2;
                    }
                }
            }
            return null;
        }
	}
}

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
using System.ComponentModel;
using Microsoft.Practices.RecipeFramework.VisualStudio;
using System.Configuration;
using System.IO;
using System.Collections.Generic;
using Microsoft.Practices.Common;
using System.ComponentModel.Design;
using Microsoft.Practices.RecipeFramework.Library; 
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.Converters
{
	/// <summary>
	/// Provides a converter that can show a list of connection strings defined in the current 
	/// application configuration file.
	/// </summary>
	public class ConnectionStringConverter : StringConverter, IAttributesConfigurable
	{
		/// <summary>
		/// Optional configuration attribute that specifies an argument that holds the 
		/// file name to use to retrieve the settings from.
		/// </summary>
		public const string ConfigurationItemArgument = "ConfigurationItemArgument";

		string configurationArgument;

		/// <summary>
		/// Determines if the value received is valid according to the current application 
		/// configuration file.
		/// </summary>
		public override bool IsValid(ITypeDescriptorContext context, object value)
		{
			if (value is ConnectionStringSettings)
			{
				return true;
			}
			if (!(value is string))
			{
				return false;
			}

			ConnectionStringsSection section = ConnectionSettingsInfo.GetSection((EnvDTE.DTE)context.GetService(typeof(EnvDTE.DTE)));
			if (section == null)
			{
				return false;
			}
			else
			{
				return section.ConnectionStrings[(string)value] != null;
			}
		}

		/// <summary>
		/// This converter only supports conversions to string and to <see cref="ConnectionStringSettings"/> 
		/// <paramref name="destinationType"/> types.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="destinationType"></param>
		/// <returns></returns>
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return (destinationType == typeof(string) ||
				destinationType == typeof(ConnectionStringSettings));
		}

		/// <summary>
		/// Converts a value to the target type.
		/// </summary>
		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				if (value is ConnectionStringSettings)
				{
					return ((ConnectionStringSettings)value).Name;
				}
				else
				{
					return value.ToString();
				}
			}
			else
			{
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}

		/// <summary>
		/// Converts a value to a <see cref="ConnectionStringSettings"/> object.
		/// </summary>
		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			if (value is string)
			{
				ConnectionStringsSection section = ConnectionSettingsInfo.GetSection((EnvDTE.DTE)context.GetService(typeof(EnvDTE.DTE)));
				if (section != null)
				{
					return section.ConnectionStrings[(string)value];
				}
			}
			return null;
		}

		/// <summary>
		/// Returns <see langword="true"/> as the converter supports providing a list of values.
		/// </summary>
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		/// <summary>
		/// Returns <see langword="true"/> because the user must pick a value provided in the list.
		/// </summary>
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}

		/// <summary>
		/// Provides the list of valid values to choose from for the connection string.
		/// </summary>
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			ConnectionStringsSection section = null;

			if (String.IsNullOrEmpty(configurationArgument))
			{
				EnvDTE.DTE vs = (EnvDTE.DTE)context.GetService(typeof(EnvDTE.DTE));
				if (vs == null)
				{
					return new StandardValuesCollection(null);
				}
				EnvDTE.Project project = DteHelper.GetSelectedProject(vs);
				if (project == null)
				{
					return new StandardValuesCollection(null);
				}
				section = ConnectionSettingsInfo.GetSection(vs);
			}
			else
			{
				IDictionaryService dictionary = (IDictionaryService)context.GetService(typeof(IDictionaryService));
                EnvDTE.ProjectItem configurationItem = dictionary.GetValue(configurationArgument) as EnvDTE.ProjectItem;
                if (configurationItem != null)
                {
                    section = ConnectionSettingsInfo.GetSection(configurationItem.get_FileNames(1));
                }
			}

			if (section == null)
			{
				return new StandardValuesCollection(null);
			}

			List<ConnectionStringSettings> connectionslist = new List<ConnectionStringSettings>(section.ConnectionStrings.Count);
			foreach (ConnectionStringSettings setting in section.ConnectionStrings)
			{
				connectionslist.Add(setting);
			}
			connectionslist.Sort(CompareConnectionStringSettings);
			return new StandardValuesCollection(connectionslist);
		}

        /// <summary>
        /// Compares the connection string settings.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
		private int CompareConnectionStringSettings(ConnectionStringSettings x, ConnectionStringSettings y)
		{
			return string.Compare(x.Name, y.Name, StringComparison.InvariantCulture);
		}

		#region IAttributesConfigurable Members

        /// <summary>
        /// Configures the component using the dictionary of attributes specified
        /// in the configuration file.
        /// </summary>
        /// <param name="attributes">The attributes in the configuration element.</param>
		public void Configure(System.Collections.Specialized.StringDictionary attributes)
		{
			if (attributes.ContainsKey(ConfigurationItemArgument))
			{
				configurationArgument = attributes[ConfigurationItemArgument];
			}
		}

		#endregion
}
}

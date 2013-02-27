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
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Globalization;
using Microsoft.Win32;

namespace Microsoft.Practices.RecipeFramework.Extensions.Extenders
{
	/// <summary>
	/// Helper class for handling reflection stuff for extenders.
	/// </summary>
    public static class ExtenderUtility
    {
        private const string RegistryKey = @"SOFTWARE\Microsoft\VisualStudio\8.0\Extenders\{0}\{1}";

        /// <summary>
        /// Registers the provider.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="extenderCategoryGuid">The extender category GUID.</param>
        /// <param name="extenderName">Name of the extender.</param>
        public static void ComRegister(Type type, string extenderCategoryGuid, string extenderName)
        {
            Guid providerGuid = new Guid(ExtenderUtility.GetCustomAttribute<GuidAttribute>(type).Value);
            /*
             * [HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\VisualStudio\8.0\Extenders\[key1, key2]\[ExtenderName]]
             * @="[GUID]"
             */
            WriteKey(RegistryKey, extenderCategoryGuid, providerGuid, extenderName);
        }

        /// <summary>
        /// Un Registers the provider.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="extenderCategoryGuid">The extender category GUID.</param>
        /// <param name="extenderName">Name of the extender.</param>
        public static void ComUnRegister(Type type, string extenderCategoryGuid, string extenderName)
        {
            /*
             * [HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\VisualStudio\8.0\Extenders\[key1, key2]\[ExtenderName]]
             * @="[GUID]"
             */

            string keyPath = String.Format(CultureInfo.InvariantCulture, RegistryKey, extenderCategoryGuid, extenderName);
            Registry.LocalMachine.DeleteSubKey(keyPath, false);
        }

		/// <summary>
		/// Gets the custom attribute.
		/// </summary>
		/// <param name="reflectionElement">The reflection element.</param>
		/// <returns></returns>
        public static TAttribute GetCustomAttribute<TAttribute>(ICustomAttributeProvider reflectionElement)
			where TAttribute : Attribute
        {
            object[] attrs = reflectionElement.GetCustomAttributes(typeof(TAttribute), true);

            if(attrs.Length == 0)
            {
                return null;
            }

            return (TAttribute)attrs[0];
        }

        private static void WriteKey(string keyFormat, string extenderCategoryGuid, Guid providerGuid, string extenderName)
        {
            string keyPath = String.Format(CultureInfo.InvariantCulture, keyFormat, extenderCategoryGuid, extenderName);

            using(RegistryKey key = Registry.LocalMachine.CreateSubKey(keyPath))
            {
                key.SetValue("", providerGuid.ToString("B"));
            }
        }
    }
}

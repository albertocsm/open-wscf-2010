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
using System.CodeDom.Compiler;
using System.Text.RegularExpressions;

namespace Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers
{
    /// <summary>
    /// Provides utility methods for working with CodeDom
    /// </summary>
    public static class CodeDomHelper
    {
        private static CodeDomProvider codeDomProvider;
        private static Regex identifierEx;

        static CodeDomHelper()
        {
            codeDomProvider = new CSharp.CSharpCodeProvider();
            identifierEx = new Regex(@"(^(\d)+)|([.]+)|((\s)+)", 
                RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Converts to a valid identifier, or throws an exception if
        /// that is not possible
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <returns>A valid C# identifier</returns>
        /// <exception cref="ArgumentException">The value cannot be converted to a valid identifier</exception>
        public static string ConvertToValidIdentifier(string value)
        {
            string identifier = codeDomProvider.CreateEscapedIdentifier((string)value);
            if (!codeDomProvider.IsValidIdentifier(identifier))
            {
                throw new ArgumentException(Properties.Resources.InvalidIdentifiedArgumentException, "value");
            }

            return identifier;
        }

        /// <summary>
        /// Determines whether [is valid identifier] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// 	<c>true</c> if [is valid identifier] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidIdentifier(string value)
        {
            string identifier = codeDomProvider.CreateEscapedIdentifier((string)value);
            return codeDomProvider.IsValidIdentifier(identifier);
        }

        /// <summary>
        /// Gets a valid identifier.
        /// </summary>
        /// <param name="value">The value to create a valid indetifier from.</param>
        /// <returns>A valid C# identifier</returns>
        public static string GetValidIdentifier(string value)
        {
            string identifier = string.Empty;

            if(!string.IsNullOrEmpty(value))
            {
                string replacedValue = identifierEx.Replace(value, "_");
                identifier = ConvertToValidIdentifier(replacedValue);
            }

            return identifier;
        }

        /// <summary>
        /// Determines if the string is a valid namespace
        /// </summary>
        /// <param name="value">The string to validate</param>
        /// <returns>True if it's valid for a namespace</returns>
        public static bool IsValidNamespace(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            // Split the string by the dots, and check
            // each part is a valid identifier
            string[] identifiers = value.Split('.');
            foreach (string id in identifiers)
            {
                if (!codeDomProvider.IsValidIdentifier(id))
                {
                    return false;
                }
            }

            return true;
        }
    }
}

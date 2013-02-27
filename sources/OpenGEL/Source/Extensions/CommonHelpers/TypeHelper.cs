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
using System.Text.RegularExpressions;

namespace Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers
{
    /// <summary>
    /// Helper methods for working with types
    /// </summary>
    public static class TypeHelper
    {
        private static Regex genericRegex;

        static TypeHelper()
        {
            genericRegex = new Regex("^([^<]*)<(.*)>$", RegexOptions.Compiled);
        }

        /// <summary>
        /// Converts a generic type name with angle brackets to the CLR notation.
        /// If the type is not a generic type name, returns the same value unchanged
        /// </summary>
        public static string ParseGenericType(string type)
        {
            Match match = genericRegex.Match(type);

            // If it's not a generic type name, return int unchanged
            if (!match.Success)
                return type;

            // Convert the arguments recursively
            string[] arguments = match.Groups[2].Value.Split(',');
            for (int i = 0; i < arguments.Length; i++)
            {
                arguments[i] = ParseGenericType(arguments[i]);
            }

            // Format the CLR type name
            string realTypeName = string.Format("{0}`{1}[{2}]", 
                match.Groups[1].Value,
                arguments.Length,
                string.Join(",", arguments));
            return realTypeName;
        }
    }
}

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
using System.IO;

namespace Microsoft.Practices.RecipeFramework.Extensions.Common.Templates.Helpers
{
    internal static class ReplaceableExtension
    {
        #region Fields
        private static List<string> repeceableExtensions = new List<string>();
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the <see cref="T:ReplaceableExtension"/> class.
        /// </summary>
        static ReplaceableExtension()
        {
            repeceableExtensions.Add(".cs");
            repeceableExtensions.Add(".vb");
            repeceableExtensions.Add(".config");
            repeceableExtensions.Add(".cd");
            repeceableExtensions.Add(".htm");
            repeceableExtensions.Add(".resx");
            repeceableExtensions.Add(".settings");
            repeceableExtensions.Add(".css");
            repeceableExtensions.Add(".xsd");
            repeceableExtensions.Add(".txt");
            repeceableExtensions.Add(".aspx");
            repeceableExtensions.Add(".asmx");
        }
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Determines whether the specified item file name is replaceable.
        /// </summary>
        /// <param name="itemFileName">Name of the item file.</param>
        /// <returns>
        /// 	<c>true</c> if the specified item file name is replaceable; otherwise, <c>false</c>.
        /// </returns>
        internal static bool IsReplaceable(string itemFileName)
        {
            return repeceableExtensions.Contains((Path.GetExtension(itemFileName)));
        }

        #endregion
    }
}

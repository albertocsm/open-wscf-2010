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
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;

namespace Microsoft.Practices.RecipeFramework.Extensions.Converters
{
    /// <summary>
    /// Converter class that validates the name of a file or folder.
    /// Files and Folders cannot:
    ///    -be empty strings
    ///    - contain any of the following characters: / ? : * "  > | # %
    ///    - contain Unicode control characters
    ///    - contain surrogate characters
    ///    - be system reserved names, including 'CON', 'AUX', 'PRN', 'COM1' or 'LPT2'
    ///    - be '.' or '..'
    ///    - Have lenghts greater than 245 characters.
    /// </summary>
    public class ValidFileNameConverter : StringConverter
    {
        private Regex validNames = new Regex(@"^(?!^(PRN|AUX|CLOCK\$|NUL|CON|COM\d|LPT\d|\..*)(\..+)?$)[^\x00-\x1f\\?*:""><|/]+$", 
                                             RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        /// <summary>
        /// Returns whether the given value object is valid for this type and for the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
        /// <param name="value">The <see cref="T:System.Object"></see> to test for validity.</param>
        /// <returns>
        /// true if the specified value is valid for this object; otherwise, false.
        /// </returns>
        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            bool response = false;
            string file = value as string;
            
            if (file != null)
            {
                // Max length reduced to 110 because we were getting 'max path' errors
                // when users typed in 245 character identifiers. This converter is used
                // primarily as a check on identifier names, not full paths, so it should
                // be ok.
                response = (validNames.IsMatch(file) &&
                            file.Length <= 110);
            }

            return response;
        }
    }
}

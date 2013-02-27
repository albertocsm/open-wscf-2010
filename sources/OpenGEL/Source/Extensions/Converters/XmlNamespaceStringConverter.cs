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

namespace Microsoft.Practices.RecipeFramework.Extensions.Converters
{
    /// <summary>
    /// A converter that validates that each of the parts of an XML namespace name (in <see cref="Uri"/> format) is
    /// a valid Xml namespace.
    /// </summary>
    public class XmlNamespaceStringConverter : StringConverter
    {
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
            string uri = value as string;

            if (uri != null)
            {
                response = Uri.IsWellFormedUriString(uri, UriKind.Absolute);
            }

            return response;
        }
    }
}

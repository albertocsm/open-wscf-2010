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
using Microsoft.Practices.Common;
using Microsoft.Practices.RecipeFramework.Library.Converters;
using System.Collections.Specialized;

namespace Microsoft.Practices.RecipeFramework.Extensions.Converters
{
    /// <summary>
    /// Converter class that groups several converters that validate a code file
    /// to be added in a VS project.
    /// </summary>
    /// <remarks>
    /// The converters used are:
    /// - <see cref="CodeIdentifierStringConverter"/>
    /// - <see cref="ValidFileNameConverter"/>
    /// - <see cref="OverwriteItemConverter"/>
    /// </remarks>
    public class VsAddCodeFileStringOverwritableConverter : AgregatorStringConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:VsAddCodeFileStringOverwritableConverter"/> class.
        /// </summary>
        public VsAddCodeFileStringOverwritableConverter()
            : base()
        {
        }

        /// <summary>
        /// Called when <see cref="IAttributesConfigurable.Configure"/> method is called.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        /// <remarks>
        /// Override this method if you inherit from this class and want to add some converters to
        /// the attributes collection.
        /// </remarks>
        public override void OnConfigure(StringDictionary attributes)
        {
            string prefix = AgregatorStringConverter.AddConverterPrefixAttribute;

            attributes.Add(prefix + "ValidLanguageIndependentIdentifierConverter", 
                ReflectionHelper.GetSimpleTypeName(typeof(ValidLanguageIndependentIdentifierConverter)));
            attributes.Add(prefix + "ValidFileNameConverter", 
                ReflectionHelper.GetSimpleTypeName(typeof(ValidFileNameConverter)));

            base.OnConfigure(attributes);
        }
    }
}

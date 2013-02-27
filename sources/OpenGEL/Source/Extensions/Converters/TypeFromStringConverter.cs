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
using Microsoft.VisualStudio.Shell.Design;
using System.Diagnostics;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Practices.RecipeFramework.Library;
using System.ComponentModel.Design;

namespace Microsoft.Practices.RecipeFramework.Extensions.Converters
{
    /// <summary>
    /// Converter that converts a string into an type
    /// </summary>
    public class TypeFromStringConverter : StringConverter
    {
        /// <summary>
        /// Converts the specified value object to a <see cref="T:System.String"></see> object.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
        /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo"></see> to use.</param>
        /// <param name="value">The <see cref="T:System.Object"></see> to convert.</param>
        /// <returns>
        /// An <see cref="T:System.Object"></see> that represents the converted value.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The conversion could not be performed. </exception>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if(value is string)
            {
                DynamicTypeService typeService = (DynamicTypeService)context.GetService(typeof(DynamicTypeService));
                Debug.Assert(typeService != null, "No dynamic type service registered.");

                IVsHierarchy hier = DteHelper.GetCurrentSelection(context);
                Debug.Assert(hier != null, "No active hierarchy is selected.");

                ITypeResolutionService resolution = typeService.GetTypeResolutionService(hier);
                Type type = resolution.GetType(value.ToString());
                return type;
            }
            else
            {
                return base.ConvertFrom(context, culture, value);
            }
        }

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
            if (value is Type)
            {
                return true;
            }

            if (value is string)
            {
                Type type = (Type)ConvertFromInvariantString(context, value.ToString());
                return IsValid(context, type);
            }

            return false;
        }
    }
}

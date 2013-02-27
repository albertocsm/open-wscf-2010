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
using Microsoft.Practices.RecipeFramework.Library.Converters;
using Microsoft.Practices.Common;
using System.ComponentModel;
using EnvDTE;
using Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers;

namespace Microsoft.Practices.RecipeFramework.Extensions.Converters
{
    /// <summary/>
    public class ProjectWithResponsibilityConverter : ProjectConverter, IAttributesConfigurable
    {
        private const string ResponsibilityArgument = "ResponsibilityArgument";
        private string responsibility = string.Empty;

        /// <summary>
        /// Returns a collection of standard values for the data type this type converter is designed for when provided with a format context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context that can be used to extract additional information about the environment from which this converter is invoked. This parameter or properties of this parameter can be null.</param>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection"></see> that holds a standard set of valid values, or null if the data type does not support a standard set of values.
        /// </returns>
        public override StandardValuesCollection GetStandardValues(System.ComponentModel.ITypeDescriptorContext context)
        {
            DTE dte = (DTE)context.GetService(typeof(DTE));
            ResponsibleProjectFinder finder = new ResponsibleProjectFinder(dte);
            List<Project> projects = finder.FindProjectsWithResponsibility(responsibility);

            return new StandardValuesCollection(projects);
        }

        /// <summary>
        /// Returns whether the collection of standard values returned from <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues"></see> is an exclusive list of possible values, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
        /// <returns>
        /// true if the <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection"></see> returned from <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues"></see> is an exhaustive list of possible values; false if other values are possible.
        /// </returns>
        public override bool GetStandardValuesExclusive(System.ComponentModel.ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// Returns whether this object supports a standard set of values that can be picked from a list, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
        /// <returns>
        /// true if <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues"></see> should be called to find a common set of values the object supports; otherwise, false.
        /// </returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        #region IAttributesConfigurable Members

        /// <summary>
        /// Configures the component using the dictionary of attributes specified
        /// in the configuration file.
        /// </summary>
        /// <param name="attributes">The attributes in the configuration element.</param>
        public void Configure(System.Collections.Specialized.StringDictionary attributes)
        {
            if (attributes == null)
                throw new ArgumentNullException("attributes");

            if (attributes.ContainsKey(ResponsibilityArgument))
            {
                responsibility = attributes["ResponsibilityArgument"];
            }
        }

        #endregion
    }
}

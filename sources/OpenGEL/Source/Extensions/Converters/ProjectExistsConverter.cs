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
using EnvDTE;
using Microsoft.Practices.Common;
using Microsoft.Practices.RecipeFramework.Extensions.Actions.CodeDom.Helpers;
using Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers;

namespace Microsoft.Practices.RecipeFramework.Extensions.Converters
{
    /// <summary>
    /// Converter that verifies if a project already exists on the file system
    /// </summary>
    public class ProjectExistsConverter : StringConverter, IAttributesConfigurable
    {
        #region Constants
        private const string ProjectTypeArgument = "ProjectType"; 
        #endregion

        #region Fields
        private LanguageType language; 
        #endregion

        #region StringConverter Implementation
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
            bool response = true;

            if(value is string)
            {
                DTE vs = (DTE)context.GetService(typeof(DTE));
                response = !DteHelperEx.ProjectExists(vs.Solution, value.ToString(), this.language);
            }

            return response;
        } 
        #endregion

        #region IAttributesConfigurable Members

        /// <summary>
        /// Configures the component using the dictionary of attributes specified
        /// in the configuration file.
        /// </summary>
        /// <param name="attributes">The attributes in the configuration element.</param>
        public void Configure(System.Collections.Specialized.StringDictionary attributes)
        {
            if(attributes.ContainsKey(ProjectTypeArgument))
            {
                language = (LanguageType)Enum.Parse(typeof(LanguageType), attributes[ProjectTypeArgument]);
            }
        }

        #endregion
    }
}

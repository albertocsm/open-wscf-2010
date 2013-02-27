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
using System.ComponentModel;
using EnvDTE;
using Microsoft.Practices.RecipeFramework.Library;
using Microsoft.Practices.RecipeFramework.Extensions.Actions.CodeDom.Helpers;
using System.ComponentModel.Design;
using System.Globalization;

namespace Microsoft.Practices.RecipeFramework.Extensions.Converters
{
    /// <summary/>
    public class ProjectItemExistsConverter : StringConverter, IAttributesConfigurable
    {
        #region Constants
        private const string ProjectArgument = "ProjectArgument";
        private const string ProjectTypeArgument = "ProjectType";
        #endregion

        #region Fields
        private string projectArgument;
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
            bool itemFound = true;

            if(value is string)
            {
                IDictionaryService dictionary = (IDictionaryService)context.GetService(typeof(IDictionaryService));

                Project project = (Project)dictionary.GetValue(projectArgument);

                string itemName = string.Concat(value.ToString(), ".", language.ToString());

                itemFound = (FindItemByName(project.ProjectItems, itemName.ToLowerInvariant(), true) != null);
            }

            return !itemFound;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Finds the name of the item by.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="name">The name.</param>
        /// <param name="recursive">if set to <c>true</c> [recursive].</param>
        /// <returns></returns>
        public static ProjectItem FindItemByName(ProjectItems collection, string name, bool recursive)
        {
            foreach (ProjectItem item1 in collection)
            {
                if (string.Compare(item1.Name, name, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    return item1;
                }

                if (recursive)
                {
                    ProjectItem item2 = DteHelper.FindItemByName(item1.ProjectItems, name, recursive);
                    if (item2 != null)
                    {
                        return item2;
                    }
                }
            }
            return null;
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
                projectArgument = attributes[ProjectArgument].ToString(CultureInfo.InvariantCulture);
            }

            if(attributes.ContainsKey(ProjectTypeArgument))
            {
                language = (LanguageType)Enum.Parse(typeof(LanguageType), attributes[ProjectTypeArgument]);
            }
        }

        #endregion
    }
}
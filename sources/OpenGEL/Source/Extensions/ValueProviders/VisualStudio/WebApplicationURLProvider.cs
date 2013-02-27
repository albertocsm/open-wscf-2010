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
#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.Common;
using EnvDTE;
using System.ComponentModel.Design;
using Microsoft.Practices.ComponentModel;
using System.Globalization; 
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.ValueProviders.VisualStudio
{
    /// <summary>
    /// Returns the URL for particular resource on a Web Project
    /// </summary>
    public class WebApplicationURLProvider : ValueProvider, IAttributesConfigurable
    {
        string webProjectArgumentName;
        string resource;

        #region ValueProvider Implementation
        /// <summary>
        /// Contains code that will be called when recipe execution begins. This is the first method in the lifecycle.
        /// </summary>
        /// <param name="currentValue">An <see cref="T:System.Object"/> that contains the current value of the argument.</param>
        /// <param name="newValue">When this method returns, contains an <see cref="T:System.Object"/> that contains
        /// the new value of the argument, if the returned value
        /// of the method is <see langword="true"/>. Otherwise, it is ignored.</param>
        /// <returns>
        /// 	<see langword="true"/> if the argument value should be replaced with
        /// the value in <paramref name="newValue"/>; otherwise, <see langword="false"/>.
        /// </returns>
        /// <remarks>By default, always returns <see langword="false"/>, unless overriden by a derived class.</remarks>
        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            IDictionaryService dictservice = (IDictionaryService)
                ServiceHelper.GetService(this, typeof(IDictionaryService));

            Project webProject = dictservice.GetValue(this.webProjectArgumentName) as Project;

            if(webProject != null)
            {
                if(!String.IsNullOrEmpty(resource))
                {
                    newValue = string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", webProject.Properties.Item("BrowseURL").Value.ToString(), this.resource);
                }
                else
                {
                    newValue = webProject.Properties.Item("BrowseURL").Value.ToString();
                }

                return true;
            }

            newValue = null;
            return false;
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
            if(attributes.ContainsKey("WebProjectArgumentName"))
            {
                webProjectArgumentName = attributes["WebProjectArgumentName"];
            }

            if(attributes.ContainsKey("Resource"))
            {
                resource = attributes["Resource"];
            }
        }
        #endregion
    }
}

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
using Microsoft.Practices.ComponentModel;
using System.ComponentModel.Design;
using EnvDTE;
using Microsoft.Practices.Common;

namespace Microsoft.Practices.RecipeFramework.Extensions.ValueProviders.General
{
    /// <summary>
    /// Placeholder
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    [ServiceDependency(typeof(IDictionaryService))]
    public class IsNotNullProvider : ValueProvider, IAttributesConfigurable
    {
        #region Constants & Fields
        /// <summary>
        /// Placeholder
        /// </summary>
        public const string ArgumentAttribute = "Argument";
        private string argument;
        #endregion

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

            if(dictservice.GetValue(this.argument) != null)
            {
                newValue = true;
            }
            else
            {
                newValue = false;
            }

            return true;
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
            if(attributes.ContainsKey(ArgumentAttribute))
            {
                this.argument = attributes[ArgumentAttribute];
            }
        }

        #endregion
    }
}

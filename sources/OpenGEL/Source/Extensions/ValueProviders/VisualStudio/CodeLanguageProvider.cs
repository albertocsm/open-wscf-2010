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
using EnvDTE;
using Microsoft.Practices.RecipeFramework.Library;
using Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers;

namespace Microsoft.Practices.RecipeFramework.Extensions.ValueProviders.VisualStudio
{     
    /// <summary>
    /// Value provider that returns the CodeDomProvider for the selected project
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class CodeLanguageProvider : ValueProvider
    {
        #region Overrides

        /// <summary>
        /// Use the DTE.SelectedItems collection to get the current selected project
        /// </summary>
        /// <param name="currentValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        /// <seealso cref="ValueProvider.OnBeforeActions"/>
        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            return SetValue(currentValue, out newValue);
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="currentValue">The current value.</param>
        /// <param name="newValue">The new value.</param>
        /// <returns></returns>
        private bool SetValue(object currentValue, out object newValue)
        {
            if (currentValue == null)
            {
                DTE dte = (DTE)GetService(typeof(DTE));
                newValue = DteHelperEx.GetCodeDomProvider(DteHelper.GetSelectedProject(dte));
                return true;
            }
            newValue = null;
            return false;
        }

        #endregion
    }
}

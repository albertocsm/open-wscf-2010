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
using System.ComponentModel.Design;
using System.IO;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework;

namespace Microsoft.Practices.RecipeFramework.Extensions.ValueProviders.VisualStudio
{
    /// <summary>
    /// Provides the name of an item been unfolded
    /// </summary>
    [ServiceDependency(typeof(IDictionaryService))]
    public class ItemNameProvider : ValueProvider
    {
        #region Overrides

        /// <summary>Provides the name of an item been unfolded</summary>
        /// <param name="currentValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        /// <seealso cref="ValueProvider.OnBeginRecipe"/>
        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            if (currentValue == null)
            {
                IDictionaryService dictionaryService = (IDictionaryService)GetService(typeof(IDictionaryService));
                string name = dictionaryService.GetValue("rootname") as string;
                if (name != null)
                {
                    name = Path.GetFileNameWithoutExtension(name);
                }
                else
                {
                    name = dictionaryService.GetValue("projectname") as string;
                }
                if (name != null)
                {
                    newValue = name;
                    return true;
                }
                else
                {
                    newValue = currentValue;
                    return false;
                }
            }
            newValue = currentValue;
            return false;
        }

        #endregion
    }
}

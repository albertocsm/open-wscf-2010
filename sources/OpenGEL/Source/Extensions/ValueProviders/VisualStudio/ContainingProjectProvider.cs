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

using EnvDTE;
using Microsoft.Practices.ComponentModel;

#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.ValueProviders.VisualStudio
{
    /// <summary>
    /// Returns the containing project of the selected item
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class ContainingProjectProvider : ValueProvider
    {
        #region Overrides

        /// <summary>
        /// Use the DTE.SelectedItems collection to get the containing project of the selected item
        /// </summary>
        /// <param name="currentValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        /// <seealso cref="ValueProvider.OnBeginRecipe"/>
        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            if(currentValue == null)
            {
                DTE dte = (DTE)GetService(typeof(DTE));
                if(dte.SelectedItems.Count == 1)
                {
                    SelectedItem selection = dte.SelectedItems.Item(1);
                    if (selection.ProjectItem == null)
                    {
                        newValue = null;
                        return false;
                    }
                    if(selection.ProjectItem.ContainingProject != null)
                    {
                        newValue = selection.ProjectItem.ContainingProject;
                        return true;
                    }
                }
            }
            newValue = null;
            return false;
        }
        #endregion

    }
}


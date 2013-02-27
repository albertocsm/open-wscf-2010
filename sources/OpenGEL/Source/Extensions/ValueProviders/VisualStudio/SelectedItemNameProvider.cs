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
using System.Text;
using System.Diagnostics;
using EnvDTE;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework;
using System.IO; 
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.ValueProviders.VisualStudio
{
    /// <summary>
    /// ValueProvider that returns the Active Document selected
    /// in the selected Project
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class SelectedItemNameProvider : ValueProvider
    {
        /// <summary>
        /// Sets the newValue to the selected Document
        /// </summary>
        /// <param name="currentValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            DTE vs = (DTE)GetService(typeof(DTE));
            newValue = Path.GetFileNameWithoutExtension(vs.SelectedItems.Item(1).ProjectItem.get_FileNames(1));
            return true;
        }
    }
}
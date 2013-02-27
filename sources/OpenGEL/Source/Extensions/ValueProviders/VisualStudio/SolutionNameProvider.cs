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
	/// ValueProvider that returns the Solution Name
	/// </summary>
	[ServiceDependency(typeof(DTE))]
    public class SolutionNameProvider : ValueProvider
	{
        #region ValueProvider Implementation
        /// <summary>
        /// Sets the newValue to the Solution Name
        /// </summary>
        /// <param name="currentValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            return GetSolutionName(out newValue);
        } 
        #endregion

        #region Private Implementation
        private bool GetSolutionName(out object newValue)
        {
            newValue = null;
            // Get the vs DTE.
            DTE vs = (DTE)GetService(typeof(DTE));
            //Get Solution physical path name
            Property property = vs.Solution.Properties.Item("Name");
            newValue = property.Value.ToString();

            return true;
        } 
        #endregion
	}
}
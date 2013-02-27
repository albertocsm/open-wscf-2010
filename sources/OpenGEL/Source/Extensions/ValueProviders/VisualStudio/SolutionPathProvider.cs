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
    /// Returns the Solution Path
    /// </summary>
    [ServiceDependency(typeof(DTE))]
	public class SolutionPathProvider : ValueProvider
	{
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
            return GetSolutionPath(out newValue);
        } 
        #endregion

        #region Private Implementation
        /// <summary>
        /// Gets the solution path.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        /// <returns></returns>
        private bool GetSolutionPath(out object newValue)
        {
            newValue = null;
            // Get the vs DTE.
            DTE vs = (DTE)GetService(typeof(DTE));
            //Get Solution physical path name
            Property property = vs.Solution.Properties.Item("Path");
            newValue = Path.GetDirectoryName(property.Value.ToString());

            return true;
        } 
        #endregion
	}
}
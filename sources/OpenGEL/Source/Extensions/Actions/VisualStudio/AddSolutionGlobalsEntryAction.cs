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
using EnvDTE;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.ComponentModel;
using System.IO;
using Microsoft.Practices.RecipeFramework.Library; 
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.Actions.VisualStudio
{
    /// <summary>
    /// Action that saves an entry into the solution globals section
    /// </summary>
    public class AddSolutionGlobalsEntryAction : BaseGlobalsEntryAction
    {
        #region Input Properties

        /// <summary>
        /// Gets or sets the solution.
        /// </summary>
        /// <value>The solution.</value>
        [Input(Required = true)]
        public Solution Solution
        {
            get { return solution; }
            set { solution = value; }
        } private Solution solution;

        #endregion

        /// <summary>
        /// Called when [execute].
        /// </summary>
        /// <param name="value">The value.</param>
        public override void OnExecute(object value)
        {
            this.solution.Globals[this.PropertyName] = value;
            this.solution.Globals.set_VariablePersists(this.PropertyName, true);
        }
    }
}

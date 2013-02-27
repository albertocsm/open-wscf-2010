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
using EnvDTE; 
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.Actions.VisualStudio
{
    /// <summary>
    /// Action that saves an entry into the project globals section
    /// </summary>
    public class AddProjectGlobalsEntryAction : BaseGlobalsEntryAction
    {
        #region Input Properties
        
        /// <summary>
        /// Gets or sets the project.
        /// </summary>
        /// <value>The project.</value>
        [Input(Required = true)]
        public Project Project
        {
            get { return project; }
            set { project = value; }
        } private Project project;

        #endregion

        /// <summary>
        /// Called when [execute].
        /// </summary>
        /// <param name="value">The value.</param>
        public override void OnExecute(object value)
        {
            this.project.Globals[this.PropertyName] = value;
            this.project.Globals.set_VariablePersists(this.PropertyName, true);
        }
    }
}

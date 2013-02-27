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
using Microsoft.Practices.RecipeFramework.Services;

namespace Microsoft.Practices.RecipeFramework.Extensions.Actions.GAX
{
    /// <summary/>
    public class EnableGuidancePackageAction : ConfigurableAction
    {
        #region Input Properties
        private string packageName;

        /// <summary>
        /// Gets or sets the name of the package.
        /// </summary>
        /// <value>The name of the package.</value>
        [Input(Required=true)]
        public string PackageName
        {
            get { return packageName; }
            set { packageName = value; }
        }

        #endregion

        #region ConfigurableAction Implementation

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public override void Execute()
        {
            IRecipeManagerService manager = GetService<IRecipeManagerService>();

            try
            {
                manager.EnablePackage(this.packageName);
            }
            catch
            {
                //Do nothing if the package is not loaded
            }
        }

        /// <summary>
        /// Undoes this instance.
        /// </summary>
        public override void Undo()
        {
            //Not Implemented
        }
        #endregion
    }
}

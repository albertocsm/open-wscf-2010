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
using Microsoft.Win32; 
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.Actions.OS
{
    /// <summary>
    /// Base Action that reads a registry entry 
    /// </summary>
	public abstract class ReadRegistryEntryAction : ConfigurableAction
    {
        #region Input Properties

        /// <summary>
        /// The registry path.
        /// </summary>
        protected string registryPath;

        /// <summary>
        /// Gets or sets the registry path.
        /// </summary>
        /// <value>The registry path.</value>
        [Input(Required = true)]
        public string RegistryPath
        {
            get { return registryPath; }
            set { registryPath = value; }
        }

        /// <summary>
        /// The registry entry
        /// </summary>
		protected string registryEntry;

        /// <summary>
        /// Gets or sets the registry entry.
        /// </summary>
        /// <value>The registry entry.</value>
        [Input(Required = true)]
        public string RegistryEntry
        {
            get { return registryEntry; }
            set { registryEntry = value; }
        }
        #endregion

        #region Output Properties

        /// <summary>
        /// The output value
        /// </summary>
		protected string _value;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [Output]
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
        #endregion

        #region ConfigurableAction Implementation
        /// <summary>
        /// See <see cref="M:Microsoft.Practices.RecipeFramework.IAction.Execute"/>.
        /// </summary>
		public override void Execute()
        {
        }

        /// <summary>
        /// See <see cref="M:Microsoft.Practices.RecipeFramework.IAction.Undo"/>.
        /// </summary>
        public override void Undo()
        {
        }
        #endregion
    }
}

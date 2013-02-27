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
    /// Action that reads a Current User registry entry
    /// </summary>
	public class ReadCurrentUserEntryAction : ReadRegistryEntryAction
	{
		#region ReadRegistryEntryAction Implementation
        /// <summary>
        /// Executes this instance.
        /// </summary>
		public override void Execute()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(this.registryPath) as RegistryKey;

            if(key != null)
            {
                if(key.GetValue(this.registryEntry) != null)
                {
                    this._value = key.GetValue(this.registryEntry).ToString();
                }
            }
        }

        /// <summary>
        /// Undoes this instance.
        /// </summary>
        public override void Undo()
        {
            //Do Nothing
        }
        #endregion
    }
}

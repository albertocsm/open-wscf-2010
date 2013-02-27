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
using Microsoft.Practices.RecipeFramework.Library.Actions;
using EnvDTE;
using System.IO;
using System.Collections;
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.Actions.VisualStudio
{
    /// <summary>
    /// Action that deletes an item from a project item
    /// </summary>
    public class DeleteItemIfExistsFromProjectItemAction : ConfigurableAction
    {
        #region Input Properties

        private string itemName;

        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        /// <value>The name of the item.</value>
        [Input(Required=true)]
        public string ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }

        private ProjectItem inputProjectItem;

        /// <summary>
        /// Gets or sets the input project item.
        /// </summary>
        /// <value>The input project item.</value>
        [Input(Required=true)]
        public ProjectItem InputProjectItem
        {
            get { return inputProjectItem; }
            set { inputProjectItem = value; }
        }


        #endregion

        #region ConfigurableAction Implementation

        /// <summary>
        /// See <see cref="M:Microsoft.Practices.RecipeFramework.IAction.Execute"/>.
        /// </summary>
        public override void Execute()
        {
            DTE dte1 = (DTE)base.GetService(typeof(DTE));

            foreach (ProjectItem item in this.InputProjectItem.ProjectItems)
            {
                if (item.Name.Equals(itemName, StringComparison.InvariantCultureIgnoreCase))
                {
                    item.Delete();
                    break;
                }
            }
        }

        /// <summary>
        /// See <see cref="M:Microsoft.Practices.RecipeFramework.IAction.Undo"/>.
        /// </summary>
        public override void Undo()
        {
            //Not Implemented
        } 
        #endregion
    }
}

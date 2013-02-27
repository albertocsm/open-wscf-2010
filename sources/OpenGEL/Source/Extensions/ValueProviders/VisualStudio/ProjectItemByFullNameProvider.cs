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
using Microsoft.Practices.Common;
using EnvDTE;
using Microsoft.Practices.RecipeFramework.Library;
using System.ComponentModel.Design;
using Microsoft.Practices.ComponentModel; 
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.ValueProviders.VisualStudio
{
    /// <summary>
    /// Returns a project performing a search based on its fullname
    /// </summary>
    public class ProjectItemByFullNameProvider : ValueProvider, IAttributesConfigurable
    {
        private string itemFullName;
        private string projectArgumentName;

        #region Overrides

        /// <summary>
        /// Uses <see cref="DteHelper.FindItemByName"/> to search for the item specified by the "Name" attributte
        /// </summary>
        /// <param name="currentValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        /// <seealso cref="ValueProvider.OnBeginRecipe"/>
        /// <seealso cref="DteHelper.FindItemByName"/>
        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            if(currentValue == null)
            {
                IDictionaryService dictservice = (IDictionaryService)
                    ServiceHelper.GetService(this, typeof(IDictionaryService));

                Project project = dictservice.GetValue(projectArgumentName) as Project;

                if(project != null)
                {
                    try
                    {
                        newValue = DteHelper.FindItemByName(project.ProjectItems, itemFullName, true);
                    }
                    catch
                    {
                        //With Web projects without any subfolder this method throws an exception
                        newValue = DteHelper.FindItemByName(project.ProjectItems, itemFullName, false);
                    }

                    if(newValue != null)
                    {
                        return true;
                    }
                }
            }

            newValue = currentValue;
            return false;
        }

        /// <param name="currentValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        /// <seealso cref="ValueProvider.OnBeforeActions"/>
        /// <seealso cref="OnBeginRecipe"/>
        public override bool OnBeforeActions(object currentValue, out object newValue)
        {
            return OnBeginRecipe(currentValue, out newValue);
        }

        #endregion

        #region IAttributesConfigurable Members

        /// <summary>
        /// Configures the component using the dictionary of attributes specified
        /// in the configuration file.
        /// </summary>
        /// <param name="attributes">The attributes in the configuration element.</param>
        public void Configure(System.Collections.Specialized.StringDictionary attributes)
        {
            itemFullName = attributes["ItemFullName"];
            projectArgumentName = attributes["ProjectArgumentName"];
        }

        #endregion
    }
}

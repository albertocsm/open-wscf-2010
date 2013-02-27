//===============================================================================
// Microsoft patterns & practices
// Web Client Software Factory 2010
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
//===============================================================================
// Microsoft patterns & practices
// Web Client Software Factory
//-------------------------------------------------------------------------------
// Copyright (C) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//-------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================
#region Using Statements
using System;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.Common;
using Microsoft.Practices.RecipeFramework.Services;
using EnvDTE;
using Microsoft.Practices.RecipeFramework.VisualStudio;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework.Library;
using System.ComponentModel.Design;
using EnvDTE80;
using Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers;
using System.Collections.Generic;
#endregion

namespace Microsoft.Practices.WebClientFactory.ValueProviders
{
    /// <summary>
    /// Search for the project specified by the "AssemblyExpression" and the Responsibility attributte in the XML configuration file
    /// </summary>    
    [ServiceDependency(typeof(DTE))]
    public class ProjectByAssemblyNameAndResponsibilityProvider : ValueProvider, IAttributesConfigurable
    {
        private string _assemblyExpression;
        private string _responsibility;

        #region Overrides

        /// <summary>
        /// Uses <see cref="DteHelper.FindProjectByName"/> to search for the project specified by the "Name" attributte
        /// </summary>
        /// <param name="currentValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        /// <seealso cref="ValueProvider.OnBeginRecipe"/>
        /// <seealso cref="DteHelper.FindProjectByName"/>
        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            if (currentValue == null)
            {
                return Evaluate(out newValue);         
            }
            newValue = currentValue;
            return false;
        }

        public override bool OnArgumentChanged(string changedArgumentName, object changedArgumentValue, object currentValue, out object newValue)
        {
            Evaluate(out newValue);
            return true;
        }

        private bool Evaluate(out object newValue)
        {
            newValue = null; 
            ExpressionEvaluationService evaluator = new ExpressionEvaluationService();
            IDictionaryService dictservice = (IDictionaryService)GetService(typeof(IDictionaryService));

            string assembly = null;
            try
            {
                assembly = evaluator.Evaluate(
                                                 this._assemblyExpression,
                                                 new ServiceAdapterDictionary(dictservice)).ToString();
            }
            catch { }

            if (assembly == null)
            {
                // evaluation failed                
                return false;
            }

            ResponsibleProjectFinder finder = new ResponsibleProjectFinder((DTE)(Site.GetService(typeof(DTE))));
            List<Project> moduleProjects = finder.FindProjectsWithResponsibility(_responsibility);
            if (moduleProjects.Count > 0)
            {
                foreach (Project module in moduleProjects)
                {
                    string projectAssembly = module.Properties.Item("AssemblyName").Value.ToString();
                    if (String.Compare(projectAssembly, assembly, StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        newValue = module;
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region IAttributesConfigurable Members

        public void Configure(System.Collections.Specialized.StringDictionary attributes)
        {
            if (attributes != null)
            {
                _assemblyExpression = attributes["AssemblyExpression"];
                _responsibility = attributes["Responsibility"];
            }
        }

        #endregion
    }
}


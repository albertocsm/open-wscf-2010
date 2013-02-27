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
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.Common;
using Microsoft.Practices.RecipeFramework.Services;
using EnvDTE;
using Microsoft.Practices.RecipeFramework.VisualStudio;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.Common.Services;
using System.ComponentModel.Design;
using Microsoft.Practices.RecipeFramework.Library;
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.ValueProviders.VisualStudio
{
    /// <summary>
    /// Search for the project specified by the "Path" attributte in the XML configuration file
    /// </summary>
    /// <example>&lt;ValueProvider Type="ProjectByNameProvider" PathExpression="$(BusinessLayer)\SubProject"/></example>
    [ServiceDependency(typeof(DTE))]
    public class ProjectByPathExpressionProvider : ValueProvider, IAttributesConfigurable
    {
        private string pathExpression;

        #region Overrides

        /// <summary>
        /// Uses <see cref="DteHelper.FindProjectByPath"/> to search for the project specified by the "PathExpression" attributte
        /// </summary>
        /// <param name="currentValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        /// <seealso cref="ValueProvider.OnBeforeActions"/>
        /// <seealso cref="DteHelper.FindProjectByPath"/>
        /// 

        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            if(currentValue == null)
            {
                newValue = GetValue();
                if(newValue != null)
                {
                    return true;
                }
            }
            newValue = currentValue;
            return false;
        }

        /// <summary>
        /// Contains code that will be called before actions are executed.
        /// </summary>
        /// <param name="currentValue">An <see cref="T:System.Object"/> that contains the current value of the argument.</param>
        /// <param name="newValue">When this method returns, contains an <see cref="T:System.Object"/> that contains
        /// the new value of the argument, if the returned value
        /// of the method is <see langword="true"/>. Otherwise, it is ignored.</param>
        /// <returns>
        /// 	<see langword="true"/> if the argument value should be replaced with
        /// the value in <paramref name="newValue"/>; otherwise, <see langword="false"/>.
        /// </returns>
        /// <remarks>By default, always returns <see langword="false"/>, unless overridden by a derived class.</remarks>
        public override bool OnBeforeActions(object currentValue, out object newValue)
        {            
            if(currentValue == null)
            {
                newValue = GetValue();
                if(newValue != null)
                {
                    return true;
                }
            }
            newValue = currentValue;
            return false;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <returns></returns>
        private object GetValue()
        {
            DTE dte = (DTE)GetService(typeof(DTE));
            object newValue;

            ExpressionEvaluationService evaluator = new ExpressionEvaluationService();
            IDictionaryService dictservice = (IDictionaryService)GetService(typeof(IDictionaryService));

            string path = evaluator.Evaluate(
                                             this.pathExpression,
                                             new ServiceAdapterDictionary(dictservice)).ToString();

            newValue = DteHelper.FindProjectByPath(dte.Solution, path);
            return newValue;
        }

        #endregion

        #region IAttributesConfigurable Members

        /// <summary>
        /// Configures the component using the dictionary of attributes specified
        /// in the configuration file.
        /// </summary>
        /// <param name="attributes">The attributes in the configuration element.</param>
        void IAttributesConfigurable.Configure(System.Collections.Specialized.StringDictionary attributes)
        {
            pathExpression = attributes["PathExpression"];
        }

        #endregion
    }
}


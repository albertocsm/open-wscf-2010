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
using Microsoft.Practices.Common;
using EnvDTE;
using Microsoft.Practices.RecipeFramework.Library;
using System.ComponentModel.Design;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers;
using Microsoft.Practices.RecipeFramework.Extensions.Actions.CodeDom.Helpers;
using System.Globalization;

namespace Microsoft.Practices.RecipeFramework.Extensions.ValueProviders.VisualStudio
{
    /// <summary>
    /// Returns a project name
    /// </summary>
    public class ProjectNameProvider : ValueProvider, IAttributesConfigurable
    {
        #region Constants
        private const string ProjectTypeArgument = "ProjectType";
        private const string DefaultNameExpressionArgument = "DefaultNameExpression";
        #endregion

        #region Fields
        private LanguageType language;
        private string expression;
        #endregion

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
            DTE dte = (DTE)GetService(typeof(DTE));

            ExpressionEvaluationService evaluator = new ExpressionEvaluationService();
            IDictionaryService dictservice = (IDictionaryService)GetService(typeof(IDictionaryService));

            string defaultName = evaluator.Evaluate(
                                             this.expression,
                                             new ServiceAdapterDictionary(dictservice)).ToString();

            string projectName = defaultName;

            while(DteHelperEx.ProjectExists(dte.Solution, projectName, this.language))
            {
                projectName = ProvideNewDefaultValue(defaultName, projectName);
            }

            newValue = projectName;
            return true;
        }
        #endregion

        #region Private Implementation
        private string ProvideNewDefaultValue(string defaultName, string projectName)
        {
            string number = projectName.Replace(defaultName, "");
            int copyNumber = 0;

            if(number != string.Empty)
            {
                Int32.TryParse(number, out copyNumber);
            }

            copyNumber++;

            return projectName + copyNumber.ToString(NumberFormatInfo.InvariantInfo);
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
            if(attributes.ContainsKey(DefaultNameExpressionArgument))
            {
                expression = attributes[DefaultNameExpressionArgument];
                language = (LanguageType)Enum.Parse(typeof(LanguageType), attributes[ProjectTypeArgument]);
            }
        }
        #endregion
    }
}
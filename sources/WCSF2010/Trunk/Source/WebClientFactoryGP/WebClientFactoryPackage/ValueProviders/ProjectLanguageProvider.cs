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
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework;
using EnvDTE;
using Microsoft.Practices.Common;
using Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers;
using System.ComponentModel.Design;

namespace Microsoft.Practices.WebClientFactory.ValueProviders
{
    /// <summary>
    /// Returns the containing project of the selected item
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class ProjectLanguageProvider : ValueProvider, IAttributesConfigurable
    {
        private string _projectExpression;

        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            return Evaluate(currentValue, out newValue);
        }

        public override bool OnBeforeActions(object currentValue, out object newValue)
        {
            return Evaluate(currentValue, out newValue);
        }

        public override bool OnArgumentChanged(string changedArgumentName, object changedArgumentValue, object currentValue, out object newValue)
        {
            return Evaluate(currentValue, out newValue);
        }

        private bool Evaluate(object currentValue, out object newValue)
        {
            Project project = ExpressionEvaluationHelper.EvaluateExpression((IDictionaryService)GetService(typeof(IDictionaryService)),
                                                                        _projectExpression) as Project;
            if (project != null)
            {
                newValue = GetProjectLanguage(project);
                return !newValue.Equals(currentValue);
            }
            else
            {
                newValue = null;
                return false;
            }
        }

        public static string GetProjectLanguage(Project project)
        {
            if (project != null)
            {
                switch (project.CodeModel.Language)
                {
                    case CodeModelLanguageConstants.vsCMLanguageCSharp:
                        return "cs";
                    case CodeModelLanguageConstants.vsCMLanguageVB:
                        return "vb";
                }
            }
            return string.Empty;
        }


        public void Configure(System.Collections.Specialized.StringDictionary attributes)
        {
            if (attributes != null)
            {
                if (attributes.ContainsKey("ProjectExpression"))
                {
                    _projectExpression = attributes["ProjectExpression"];
                }
            }
        }
    }
}

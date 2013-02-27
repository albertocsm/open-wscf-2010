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
    public class WebsiteLanguageProvider : ValueProvider, IAttributesConfigurable
    {
        private string _projectExpression;
        private string _createAsFolderInWebsite;
        private string _moduleProjectLanguage;
        private string _isWCSFSolutionWAP;

        #region Overrides

        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            return Evaluate(currentValue, out newValue);
        }

        public override bool OnBeforeActions(object currentValue, out object newValue)
        {
            return Evaluate(currentValue, out newValue);
        }
        #endregion

        private bool Evaluate(object currentValue, out object newValue)
        {
            IDictionaryService dictionaryService = (IDictionaryService)GetService(typeof(IDictionaryService));

            string moduleProjectLanguage = ExpressionEvaluationHelper.EvaluateExpression(dictionaryService, _moduleProjectLanguage) as string;
            bool isWCSFSolutionWAP = (bool)ExpressionEvaluationHelper.EvaluateExpression(dictionaryService, _isWCSFSolutionWAP);
            bool createAsFolderInWebsite = (bool)ExpressionEvaluationHelper.EvaluateExpression(dictionaryService, _createAsFolderInWebsite);

            if (isWCSFSolutionWAP && createAsFolderInWebsite)
            {
                Project project = ExpressionEvaluationHelper.EvaluateExpression(dictionaryService, _projectExpression) as Project;
                if (project != null)
                {
                    newValue = null;
                    switch (project.CodeModel.Language)
                    {
                        case CodeModelLanguageConstants.vsCMLanguageCSharp:
                            newValue = "cs";
                            break;
                        case CodeModelLanguageConstants.vsCMLanguageVB:
                            newValue = "vb";
                            break;
                    }
                    return true;
                }
            }
            else
            {
                newValue = moduleProjectLanguage;
                return true;
            }
            newValue = null;
            return false;
        }
        public void Configure(System.Collections.Specialized.StringDictionary attributes)
        {
            if (attributes != null)
            {
                if (attributes.ContainsKey("ProjectExpression"))
                {
                    _projectExpression = attributes["ProjectExpression"];
                }
                if (attributes.ContainsKey("CreateAsFolderInWebsite"))
                {
                    _createAsFolderInWebsite = attributes["CreateAsFolderInWebsite"];
                }
                if (attributes.ContainsKey("IsWCSFSolutionWAP"))
                {
                    _isWCSFSolutionWAP = attributes["IsWCSFSolutionWAP"];
                }
                if (attributes.ContainsKey("ModuleProjectLanguage"))
                {
                    _moduleProjectLanguage = attributes["ModuleProjectLanguage"];
                }
            }
        }
    }
}

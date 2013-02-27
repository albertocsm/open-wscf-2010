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
    public class WebProjectProvider : ValueProvider, IAttributesConfigurable
    {
        private string _webProjectExpression;
        private string _webFolderExpression;

        #region Overrides

        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            if (currentValue == null)
            {
                Project webproject = ExpressionEvaluationHelper.EvaluateExpression((IDictionaryService)GetService(typeof(IDictionaryService)),
                                                                            _webProjectExpression) as Project;
                if (webproject != null)
                {
                    newValue = webproject;
                    return true;
                }
                else
                {
                    ProjectItem webfolder = ExpressionEvaluationHelper.EvaluateExpression((IDictionaryService)GetService(typeof(IDictionaryService)),
                                                                            _webFolderExpression) as ProjectItem;
                    if (webfolder == null)
                    {
                        newValue = null;
                        return false;
                    }
                    if (webfolder.ContainingProject != null)
                    {
                        newValue = webfolder.ContainingProject;
                        return true;
                    }
                }
            }
            newValue = null;
            return false;
        }
        #endregion

        public void Configure(System.Collections.Specialized.StringDictionary attributes)
        {
            if (attributes != null)
            {
                if (attributes.ContainsKey("WebProjectExpression"))
                {
                    _webProjectExpression = attributes["WebProjectExpression"];
                }
                if (attributes.ContainsKey("WebFolderExpression"))
                {
                    _webFolderExpression = attributes["WebFolderExpression"];
                }
            }
        }
    }
}

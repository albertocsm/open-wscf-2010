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
using EnvDTE;
using Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers;
using System.Globalization;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.Common;
using Microsoft.Practices.ComponentModel;
using System.ComponentModel.Design;
using Microsoft.Practices.RecipeFramework.Extensions.DteWrapper;

namespace Microsoft.Practices.WebClientFactory.ValueProviders
{
    /// <summary>
    /// Returns the next unexisting numbered identifier beginning with a given prefix
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class NextNumberedTypeNameProvider : ValueProvider, IAttributesConfigurable
    {
        private string _projectExpression;
        private string _generatingPrefix;
        private string _searchingPrefix;

        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            if (currentValue == null)
            {
                Project project = ExpressionEvaluationHelper.EvaluateExpression((IDictionaryService)GetService(typeof(IDictionaryService)),
                                                                            _projectExpression) as Project;
                if (project != null)
                {
                    IProjectModel projectModel = new DteProjectModel(project, Site);
                    string searchingPrefix = ExpressionEvaluationHelper.EvaluateExpression((IDictionaryService)GetService(typeof(IDictionaryService)),
                                                                                _searchingPrefix) as string;
                    string generatingPrefix = ExpressionEvaluationHelper.EvaluateExpression((IDictionaryService)GetService(typeof(IDictionaryService)),
                                                                                _generatingPrefix) as string;
                    int numericSuffix = 1;
                    string searchingTypeName = "";
                    string searchingTypeFullName = "";
                    string generatingTypeName = "";
                    bool typeExists = true; //Dummy value forcing enter while structure

                    while (typeExists)
                    {
                        searchingTypeName = string.Format(CultureInfo.InvariantCulture, "{0}{1}", searchingPrefix, numericSuffix);
                        generatingTypeName = string.Format(CultureInfo.InvariantCulture, "{0}{1}", generatingPrefix, numericSuffix);

                        if (!projectModel.IsWebProject)
                        {
                            searchingTypeFullName = string.Format(CultureInfo.InvariantCulture, "{1}.{0}", searchingTypeName, project.Properties.Item("DefaultNamespace").Value);
                        }
                        else
                        {
                            searchingTypeFullName = searchingTypeName;
                        }

                        typeExists = IsTypeAlreadyDeclared(projectModel, searchingTypeFullName);
                        numericSuffix++;
                    }

                    newValue = generatingTypeName;
                    return true;
                }
            }
            newValue = null;
            return false;
        }
        private bool IsTypeAlreadyDeclared(IProjectModel project, string typeName)
        {
            foreach (ITypeModel type in project.Types)
            {
                if (type.FullName == typeName)
                {
                    return true;
                }
            }
            return false;
        }

        public void Configure(System.Collections.Specialized.StringDictionary attributes)
        {
            if (attributes != null)
            {
                if (attributes.ContainsKey("ProjectExpresion"))
                {
                    _projectExpression = attributes["ProjectExpresion"];
                }
                if (attributes.ContainsKey("GeneratingPrefixExpresion"))
                {
                    _generatingPrefix = attributes["GeneratingPrefixExpresion"];
                }
                if (attributes.ContainsKey("SearchingPrefixExpresion"))
                {
                    _searchingPrefix = attributes["SearchingPrefixExpresion"];
                }
                else
                {
                    _searchingPrefix = _generatingPrefix;
                }
            }
        }
    }
}

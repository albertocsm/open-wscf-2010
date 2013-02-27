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
using Microsoft.Practices.Common;
using Microsoft.Practices.CompositeWeb.Configuration;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Practices.WebClientFactory.Properties;
using System.Globalization;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.RecipeFramework.Library;
using System.ComponentModel.Design;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.CompositeWeb.Services;
using System.Reflection;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.WebClientFactory.Mappers;

namespace Microsoft.Practices.WebClientFactory.ValueProviders
{
    public class ModuleInfoListProvider : ValueProvider, IAttributesConfigurable
    {
        public const string PathExpressionAttribute = "PathExpression";

        private string _pathExpression;

        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            if (currentValue == null)
            {
                ExpressionEvaluationService evaluator = new ExpressionEvaluationService();
                IDictionaryService dictservice = (IDictionaryService)GetService(typeof(IDictionaryService));

                string path = evaluator.Evaluate(
                                                 this._pathExpression,
                                                 new ServiceAdapterDictionary(dictservice)).ToString();

                WebConfigXmlParseModuleInfoStore store = new WebConfigXmlParseModuleInfoStore(path);                
                WebModuleEnumerator moduleEnumerator = new WebModuleEnumerator(store);

                IModuleInfo[] modules = moduleEnumerator.EnumerateModules();

                newValue = new ModuleInfoMapper().Translate(modules);

                return true;                
            }

            newValue = null;
            return false;
        }       
       
        public void Configure(System.Collections.Specialized.StringDictionary attributes)
        {
            if (attributes != null)
            {
                _pathExpression = attributes[PathExpressionAttribute];
            }
        }
    }
}

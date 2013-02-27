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
using System.IO;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.Common;
using Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers;
using System.ComponentModel.Design;
using Microsoft.Practices.RecipeFramework.Library;
using Microsoft.Practices.ComponentModel;

namespace Microsoft.Practices.WebClientFactory.ValueProviders
{
    public class NegateValueProvider : ValueProvider, IAttributesConfigurable
    {
        public const string ValueToNegateAttribute = "ValueToNegate";
        private string _valueToNegateExpression;

        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            newValue = null;
            bool solvedExpression = (bool)ExpressionEvaluationHelper.EvaluateExpression(
                                (IDictionaryService)GetService(typeof(IDictionaryService)),
                                _valueToNegateExpression);

            newValue = !solvedExpression;
            return true;
        }

        public override bool OnBeforeActions(object currentValue, out object newValue)
        {
            newValue = null;
            bool solvedExpression = (bool)ExpressionEvaluationHelper.EvaluateExpression(
                                (IDictionaryService)GetService(typeof(IDictionaryService)),
                                _valueToNegateExpression);

            newValue = !solvedExpression;
            return true;
        }


        #region IAttributesConfigurable Members

        public void Configure(System.Collections.Specialized.StringDictionary attributes)
        {
            if (attributes != null)
            {
                if (attributes.ContainsKey(ValueToNegateAttribute))
                {
                    _valueToNegateExpression = attributes[ValueToNegateAttribute];
                }
            }
        }

        #endregion
    }
}

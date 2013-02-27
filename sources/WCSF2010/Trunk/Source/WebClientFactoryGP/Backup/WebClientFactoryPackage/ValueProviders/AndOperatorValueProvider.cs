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
    public class AndOperatorValueProvider : ValueProvider, IAttributesConfigurable
    {
        public const string FirstOperandAttribute = "FirstOperand";
        public const string SecondOperandAttribute = "SecondOperand";
        private string _firstOperandExpression;
        private string _secondOperandExpression;

        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            if (string.IsNullOrEmpty(currentValue as string))
            {
                bool result = Evaluate();
                newValue = result;
                return true;
            }
            newValue = null;
            return false;
        }

        private bool Evaluate()
        {
            IDictionaryService dictionary = (IDictionaryService)GetService(typeof(IDictionaryService));
            bool _firstOperand = (bool)ExpressionEvaluationHelper.EvaluateExpression(
                                dictionary,
                                _firstOperandExpression);
            bool _secondOperand = (bool)ExpressionEvaluationHelper.EvaluateExpression(
                                dictionary,
                                _secondOperandExpression);

            return (_firstOperand && _secondOperand);
        }

        public override bool OnBeforeActions(object currentValue, out object newValue)
        {
            if (string.IsNullOrEmpty(currentValue as string))
            {
                bool result = Evaluate();
                newValue = result;
                return true;
            }
            newValue = null;
            return false;
        }

        #region IAttributesConfigurable Members

        public void Configure(System.Collections.Specialized.StringDictionary attributes)
        {
            if (attributes != null)
            {
                if (attributes.ContainsKey(FirstOperandAttribute))
                {
                    _firstOperandExpression = attributes[FirstOperandAttribute];
                }
                if (attributes.ContainsKey(SecondOperandAttribute))
                {
                    _secondOperandExpression = attributes[SecondOperandAttribute];
                }
            }
        }

        #endregion
    }
}

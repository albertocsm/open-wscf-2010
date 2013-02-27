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
using Microsoft.Practices.RecipeFramework.Library;
using System.ComponentModel.Design;
using Microsoft.Practices.ComponentModel;

namespace Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers
{
    /// <summary>
    /// Helper class to evaluate expressions using <see cref="ExpressionEvaluationService"/>
    /// </summary>
    public static class ExpressionEvaluationHelper
    {
        /// <summary>
        /// Evaluates an expression using <see cref="ExpressionEvaluationService"/>
        /// </summary>
        /// <param name="dictionary">An instance of <see cref="IDictionaryService"/></param>
        /// <param name="expression">Expression to evaluate</param>
        /// <returns>The result as an object</returns>
        public static object EvaluateExpression(IDictionaryService dictionary, string expression)
        {
            ExpressionEvaluationService evaluator = new ExpressionEvaluationService();
            object result = null;
            try
            {
                result = evaluator.Evaluate(expression, new ServiceAdapterDictionary(dictionary));
            }
            catch { }
            return result;
        }
    }
}

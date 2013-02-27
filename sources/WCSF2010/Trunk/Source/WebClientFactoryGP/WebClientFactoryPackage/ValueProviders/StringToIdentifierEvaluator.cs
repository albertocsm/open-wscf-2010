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
using System.ComponentModel.Design;
using Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers;
using Microsoft.Practices.Common;
using Microsoft.Practices.RecipeFramework;
using System.Text.RegularExpressions;

namespace Microsoft.Practices.WebClientFactory.ValueProviders
{
	public class StringToIdentifierEvaluator : ValueProvider, IAttributesConfigurable
	{
		public const string ExpressionAttribute = "Expression";
		private string _expression;

		public override bool OnBeginRecipe(object currentValue, out object newValue)
		{
			
			string solvedExpression = (string) ExpressionEvaluationHelper.EvaluateExpression(
								(IDictionaryService)GetService(typeof(IDictionaryService)),
								_expression);

			newValue = ConvertToValidIdentifier(solvedExpression);
			
			return true;
		}

		public override bool OnBeforeActions(object currentValue, out object newValue)
		{
			string solvedExpression = (string)ExpressionEvaluationHelper.EvaluateExpression(
								(IDictionaryService)GetService(typeof(IDictionaryService)),
								_expression);

			newValue = ConvertToValidIdentifier(solvedExpression);

			return true;
		}


		#region IAttributesConfigurable Members

		public void Configure(System.Collections.Specialized.StringDictionary attributes)
		{
			if (attributes != null)
			{
				if (attributes.ContainsKey(ExpressionAttribute))
				{
					_expression = attributes[ExpressionAttribute];
				}
			}
		}

		#endregion

		private string ConvertToValidIdentifier(string value)
		{
			value = value.Trim();
			value = Regex.Replace(value, @"\W", "_");
			if (Char.IsDigit(value[0]))
			{
				value = "_" + value.Substring(1);
			}
			return value;
		}
	}
}

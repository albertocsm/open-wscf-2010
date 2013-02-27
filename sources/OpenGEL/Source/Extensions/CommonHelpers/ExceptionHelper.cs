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
using System.Globalization;

namespace Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers
{
	/// <summary>
	/// Helper class for handling exceptions.
	/// </summary>
	public static class ExceptionHelper
	{
		/// <summary>
		/// Builds the error messsage.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <returns></returns>
		public static string BuildErrorMessage(Exception exception)
		{
			string message = string.Empty;

			if (exception != null)
			{
                message = string.Format(CultureInfo.CurrentUICulture, Properties.Resources.InnerExceptionMessage, 
					exception.Message, Environment.NewLine, BuildErrorMessage(exception.InnerException)) ;
			}

			return message;
		}
	}
}

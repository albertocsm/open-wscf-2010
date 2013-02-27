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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.Practices.CompositeWeb.Properties;

namespace Microsoft.Practices.CompositeWeb.Utility
{
	/// <summary>
	/// Utility class used to check for exceptions.
	/// </summary>
	public static class Guard
	{
		/// <summary>
		/// Checks a string argument to ensure it isn't null or empty
		/// </summary>
		/// <param name="argumentValue">The argument value to check.</param>
		/// <param name="argumentName">The name of the argument.</param>
		[SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods",
			Justification = "Validation done by ArgumentNotNull method.")]
		public static void ArgumentNotNullOrEmptyString(string argumentValue, string argumentName)
		{
			ArgumentNotNull(argumentValue, argumentName);

			if (argumentValue.Length == 0)
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.StringCannotBeEmpty, argumentName));
		}

		/// <summary>
		/// Checks an argument to ensure it isn't null
		/// </summary>
		/// <param name="argumentValue">The argument value to check.</param>
		/// <param name="argumentName">The name of the argument.</param>
		public static void ArgumentNotNull(object argumentValue, string argumentName)
		{
			if (argumentValue == null)
				throw new ArgumentNullException(argumentName);
		}

		/// <summary>
		/// Checks an Enum argument to ensure that its value is defined by the specified Enum type.
		/// </summary>
		/// <param name="enumType">The Enum type the value should correspond to.</param>
		/// <param name="value">The value to check for.</param>
		/// <param name="argumentName">The name of the argument holding the value.</param>
		[SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods",
			Justification = "Validation done by Enum.IsDefined method.")]
		public static void EnumValueIsDefined(Type enumType, object value, string argumentName)
		{
			if (Enum.IsDefined(enumType, value) == false)
				throw new ArgumentException(String.Format(CultureInfo.CurrentCulture,
				                                          Resources.InvalidEnumValue,
				                                          argumentName, enumType.ToString()));
		}

		/// <summary>
		/// Verifies that an argument type is assignable from the provided type (meaning
		/// interfaces are implemented, or classes exist in the base class hierarchy).
		/// </summary>
		/// <param name="assignee">The argument type.</param>
		/// <param name="providedType">The type it must be assignable from.</param>
		/// <param name="argumentName">The argument name.</param>
		[SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
		public static void TypeIsAssignableFromType(Type assignee, Type providedType, string argumentName)
		{
			if (!assignee.IsAssignableFrom(providedType))
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
				                                          Resources.TypeNotCompatible, providedType, assignee), argumentName);
		}
	}
}
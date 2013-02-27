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
using System.Globalization;
using System.Reflection;
using System.Web;

namespace Microsoft.Practices.CompositeWeb.Utility
{
	/// <summary>
	/// Utility calss used to encode output values.
	/// </summary>
	public sealed class OutputValidationUtility
	{
		private OutputValidationUtility()
		{
		}

		/// <summary>
		/// Encodes the specified entity values.
		/// </summary>
		/// <typeparam name="T">The type of the entity to encode.</typeparam>
		/// <param name="entityToEncode">The entity instance to encode.</param>
		/// <returns>The encoded entity.</returns>
		public static T Encode<T>(T entityToEncode)
		{
			if (entityToEncode != null)
			{
				Type entityType = typeof (T);
				PropertyInfo[] propertyInfos = entityType.GetProperties();
				foreach (PropertyInfo info in propertyInfos)
				{
					if (info.PropertyType == typeof (string) && info.CanWrite && info.CanRead)
					{
						string value =
							(string) entityType.InvokeMember(info.Name, BindingFlags.GetProperty, null, entityToEncode, null,
							                                 CultureInfo.CurrentCulture);
						entityType.InvokeMember(info.Name, BindingFlags.SetProperty, null, entityToEncode,
						                        new object[] {HttpUtility.HtmlEncode(value)},
						                        CultureInfo.CurrentCulture);
					}
				}
			}

			return entityToEncode;
		}

		/// <summary>
		/// Encodes a set of entities of the same type.
		/// </summary>
		/// <typeparam name="T">The type of the entities to encode.</typeparam>
		/// <param name="entitiesToEncode">The list of entities to encode.</param>
		/// <returns>A list of encoded entities.</returns>
		public static IList<T> Encode<T>(IList<T> entitiesToEncode)
		{
			foreach (T item in entitiesToEncode)
			{
				Encode<T>(item);
			}
			return entitiesToEncode;
		}
	}
}
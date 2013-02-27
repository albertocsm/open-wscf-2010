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
using System.Reflection;
using System.Web.SessionState;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.CompositeWeb.BuilderStrategies
{
	/// <summary>
	/// Declares a <see cref="BuilderStrategy"/> that manages the values to get or set from the session.
	/// </summary>
	/// <remarks>This strategy deals with the <see cref="SessionStateKeyAttribute"/> attribute.</remarks>
	public class SessionStateBindingStrategy : BuilderStrategy
	{
		/// <summary>
		/// Inspect  the object for <see cref="BuilderStrategy"/> and set the object field values accordingly.
		/// </summary>
		public override object BuildUp(IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
		{
			// Get the session state
			ISessionStateLocatorService sessionLocator =
				context.Locator.Get<ISessionStateLocatorService>(
					new DependencyResolutionLocatorKey(typeof (ISessionStateLocatorService), null));
			if (sessionLocator != null)
			{
				IHttpSessionState sessionState = sessionLocator.GetSessionState();

				foreach (FieldInfo fieldInfo in typeToBuild.GetFields())
				{
					SetFieldValue(existing, fieldInfo, sessionState);
				}
			}
			return base.BuildUp(context, typeToBuild, existing, idToBuild);
		}

		private void SetFieldValue(object existing, FieldInfo fieldInfo, IHttpSessionState sessionState)
		{
			if (IsStateField(fieldInfo))
			{
				IStateValue value = (IStateValue) Activator.CreateInstance(fieldInfo.FieldType);
				value.KeyName = GetKeyName(fieldInfo);
				value.SessionState = sessionState;
				fieldInfo.SetValue(existing, value);
			}
		}

		private static bool IsStateField(FieldInfo fieldInfo)
		{
			return typeof (IStateValue).IsAssignableFrom(fieldInfo.FieldType);
		}

		private static string GetKeyName(FieldInfo fieldInfo)
		{
			return GetSessionKeyNameFromAttribute(fieldInfo) ?? fieldInfo.DeclaringType.Name + ";" + fieldInfo.Name;
		}

		private static string GetSessionKeyNameFromAttribute(FieldInfo fieldInfo)
		{
			object[] attr = fieldInfo.GetCustomAttributes(typeof (SessionStateKeyAttribute), false);
			return (attr != null && attr.Length > 0) ? ((SessionStateKeyAttribute) attr[0]).SessionKeyName : null;
		}
	}
}
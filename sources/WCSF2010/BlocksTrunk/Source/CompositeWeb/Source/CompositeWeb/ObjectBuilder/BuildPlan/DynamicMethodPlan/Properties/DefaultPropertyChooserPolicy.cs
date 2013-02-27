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
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.Practices.CompositeWeb.ObjectBuilder.BuildPlan.DynamicMethodPlan.Properties
{
	/// <summary>
	/// 
	/// </summary>
	public class DefaultPropertyChooserPolicy : IPropertyChooserPolicy
	{
		#region IPropertyChooserPolicy Members

		/// <summary>
		/// 
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		public IEnumerable<PropertyInfo> GetInjectionProperties(Type t)
		{
			foreach (PropertyInfo prop in t.GetProperties(BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.Instance)
				)
			{
				yield return prop;
			}
		}

		#endregion
	}
}
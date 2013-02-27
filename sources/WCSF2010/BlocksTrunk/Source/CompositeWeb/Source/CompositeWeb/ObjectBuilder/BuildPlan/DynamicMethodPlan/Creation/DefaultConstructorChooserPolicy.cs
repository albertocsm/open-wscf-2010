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
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Practices.CompositeWeb.Utility;

namespace Microsoft.Practices.CompositeWeb.ObjectBuilder.BuildPlan.DynamicMethodPlan.Creation
{
	/// <summary>
	/// A Default implementation of <see cref="IConstructorChooserPolicy"/>
	/// </summary>
	public class DefaultConstructorChooserPolicy : IConstructorChooserPolicy
	{
		#region IConstructorChooserPolicy Members

		/// <summary>
		/// See <see cref="IConstructorChooserPolicy.ChooseConstructor"/> for more information.
		/// </summary>
		/// <param name="t"> Type to choose a constructor from. </param>
		/// <returns></returns>
		[SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods",
			Justification = "Validated using Guard class")]
		public ConstructorInfo ChooseConstructor(Type t)
		{
			Guard.ArgumentNotNull(t, "DefaultConstructorChooserPolicy requires a Type instance.");

			ConstructorInfo[] ctors = t.GetConstructors();
			if (ctors.Length > 0)
			{
				return ctors[0];
			}

			return t.GetConstructor(new Type[0]);
		}

		#endregion
	}
}
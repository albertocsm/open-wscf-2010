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
using System.Reflection;
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.CompositeWeb.ObjectBuilder.Strategies
{
	/// <summary>
	/// Strategy that describes method call via attributes.
	/// </summary>
	public class MethodReflectionStrategy : ReflectionStrategy<MethodInfo>
	{
		/// <summary>
		/// See <see cref="ReflectionStrategy{T}.GetMembers"/> for more information.
		/// </summary>
		protected override IEnumerable<IReflectionMemberInfo<MethodInfo>> GetMembers(IBuilderContext context, Type typeToBuild,
		                                                                             object existing, string idToBuild)
		{
			foreach (MethodInfo method in typeToBuild.GetMethods())
				yield return new ReflectionMemberInfo<MethodInfo>(method);
		}

		/// <summary>
		/// See <see cref="ReflectionStrategy{T}.AddParametersToPolicy"/> for more information.
		/// </summary>
		protected override void AddParametersToPolicy(IBuilderContext context, Type typeToBuild, string idToBuild,
		                                              IReflectionMemberInfo<MethodInfo> member,
		                                              IEnumerable<IParameter> parameters)
		{
			MethodPolicy result = context.Policies.Get<IMethodPolicy>(typeToBuild, idToBuild) as MethodPolicy;

			if (result == null)
			{
				result = new MethodPolicy();
				context.Policies.Set<IMethodPolicy>(result, typeToBuild, idToBuild);
			}

			result.Methods.Add(member.Name, new MethodCallInfo(member.MemberInfo, parameters));
		}

		/// <summary>
		/// See <see cref="ReflectionStrategy{T}.MemberRequiresProcessing"/> for more information.
		/// </summary>
		protected override bool MemberRequiresProcessing(IReflectionMemberInfo<MethodInfo> member)
		{
			return (member.GetCustomAttributes(typeof (InjectionMethodAttribute), true).Length > 0);
		}
	}
}
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
	/// Strategy that performs injection of constructor policies.
	/// </summary>
	public class ConstructorReflectionStrategy : ReflectionStrategy<ConstructorInfo>
	{
		/// <summary>
		/// See <see cref="ReflectionStrategy{T}.GetMembers"/> for more information.
		/// </summary>
		protected override IEnumerable<IReflectionMemberInfo<ConstructorInfo>> GetMembers(IBuilderContext context,
		                                                                                  Type typeToBuild, object existing,
		                                                                                  string idToBuild)
		{
			List<IReflectionMemberInfo<ConstructorInfo>> result = new List<IReflectionMemberInfo<ConstructorInfo>>();
			ICreationPolicy existingPolicy = context.Policies.Get<ICreationPolicy>(typeToBuild, idToBuild);

			if (existing == null && (existingPolicy == null || existingPolicy is DefaultCreationPolicy))
			{
				ConstructorInfo injectionCtor = null;
				ConstructorInfo[] ctors = typeToBuild.GetConstructors();

				if (ctors.Length == 1)
					injectionCtor = ctors[0];
				else
				{
					foreach (ConstructorInfo ctor in ctors)
					{
						if (Attribute.IsDefined(ctor, typeof (InjectionConstructorAttribute)))
						{
							// Multiple decorated constructors aren't valid
							if (injectionCtor != null)
								throw new InvalidAttributeException();

							injectionCtor = ctor;
						}
					}
				}

				if (injectionCtor != null)
					result.Add(new ReflectionMemberInfo<ConstructorInfo>(injectionCtor));
			}

			return result;
		}

		/// <summary>
		/// See <see cref="ReflectionStrategy{T}.AddParametersToPolicy"/> for more information.
		/// </summary>
		protected override void AddParametersToPolicy(IBuilderContext context, Type typeToBuild, string idToBuild,
		                                              IReflectionMemberInfo<ConstructorInfo> member,
		                                              IEnumerable<IParameter> parameters)
		{
			ConstructorPolicy policy = new ConstructorPolicy();

			foreach (IParameter parameter in parameters)
				policy.AddParameter(parameter);

			context.Policies.Set<ICreationPolicy>(policy, typeToBuild, idToBuild);
		}

		/// <summary>
		/// See <see cref="ReflectionStrategy{T}.MemberRequiresProcessing"/> for more information.
		/// </summary>
		protected override bool MemberRequiresProcessing(IReflectionMemberInfo<ConstructorInfo> member)
		{
			return true;
		}
	}
}
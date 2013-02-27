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
	/// Base generic strategy for all injection attribute processors.
	/// </summary>
	public abstract class ReflectionStrategy<TMemberInfo> : BuilderStrategy
	{
		private volatile Dictionary<TMemberInfo, bool> _memberRequiresProcessingCache = new Dictionary<TMemberInfo, bool>();

		private volatile Dictionary<Type, IEnumerable<IReflectionMemberInfo<TMemberInfo>>> _membersCache =
			new Dictionary<Type, IEnumerable<IReflectionMemberInfo<TMemberInfo>>>();

		private object _writeLockerGm = new object();
		private object _writeLockerMrp = new object();

		/// <summary>
		/// Retrieves the list of members to iterate looking for 
		/// injection attributes, such as properties and constructor 
		/// parameters.
		/// </summary>
		/// <param name="context">The build context.</param>
		/// <param name="typeToBuild">Type being built.</param>
		/// <param name="existing">Existing object being built, if available.</param>
		/// <param name="idToBuild">The ID being built.</param>
		/// <returns>An enumerable wrapper around the IReflectionMemberInfo{T} interfaces that
		/// represent the members to be inspected for reflection.</returns>
		protected abstract IEnumerable<IReflectionMemberInfo<TMemberInfo>> GetMembers(IBuilderContext context,
		                                                                              Type typeToBuild, object existing,
		                                                                              string idToBuild);

		/// <summary>
		/// See <see cref="BuilderStrategy.BuildUp"/> for more information.
		/// </summary>
		public override object BuildUp(IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
		{
			IEnumerable<IReflectionMemberInfo<TMemberInfo>> members = InnerGetMembers(context, typeToBuild, existing, idToBuild);
			foreach (IReflectionMemberInfo<TMemberInfo> member in members)
			{
				if (InnerMemberRequiresProcessing(member))
				{
					IEnumerable<IParameter> parameters = GenerateIParametersFromParameterInfos(member.GetParameters());
					AddParametersToPolicy(context, typeToBuild, idToBuild, member, parameters);
				}
			}

			return base.BuildUp(context, typeToBuild, existing, idToBuild);
		}

		private IEnumerable<IReflectionMemberInfo<TMemberInfo>> InnerGetMembers(IBuilderContext context, Type typeToBuild,
		                                                                        object existing, string idToBuild)
		{
			IEnumerable<IReflectionMemberInfo<TMemberInfo>> cachedMembers;

			if (!_membersCache.TryGetValue(typeToBuild, out cachedMembers))
			{
				lock (_writeLockerGm)
				{
					if (!_membersCache.TryGetValue(typeToBuild, out cachedMembers))
					{
						Dictionary<Type, IEnumerable<IReflectionMemberInfo<TMemberInfo>>> clonedMembersCache =
							new Dictionary<Type, IEnumerable<IReflectionMemberInfo<TMemberInfo>>>(_membersCache);
						cachedMembers = GetMembers(context, typeToBuild, existing, idToBuild);
						clonedMembersCache.Add(typeToBuild, cachedMembers);
						_membersCache = clonedMembersCache;
					}
				}
			}
			return cachedMembers;
		}

		private bool InnerMemberRequiresProcessing(IReflectionMemberInfo<TMemberInfo> member)
		{
			bool requires;

			if (!_memberRequiresProcessingCache.TryGetValue(member.MemberInfo, out requires))
			{
				lock (_writeLockerMrp)
				{
					if (!_memberRequiresProcessingCache.TryGetValue(member.MemberInfo, out requires))
					{
						Dictionary<TMemberInfo, bool> tempMemberRequiresProcessingCache =
							new Dictionary<TMemberInfo, bool>(_memberRequiresProcessingCache);
						requires = MemberRequiresProcessing(member);
						tempMemberRequiresProcessingCache.Add(member.MemberInfo, requires);
						_memberRequiresProcessingCache = tempMemberRequiresProcessingCache;
					}
				}
			}
			return requires;
		}

		/// <summary>
		/// Abstract method which takes parameters and adds them to the appropriate policy.
		/// Must be overridden in derived classes.
		/// </summary>
		/// <param name="context">The build context.</param>
		/// <param name="typeToBuild">The type being built.</param>
		/// <param name="idToBuild">The ID being built.</param>
		/// <param name="member">The member that's being reflected over.</param>
		/// <param name="parameters">The parameters used to satisfy the member call.</param>
		protected abstract void AddParametersToPolicy(IBuilderContext context, Type typeToBuild, string idToBuild,
		                                              IReflectionMemberInfo<TMemberInfo> member,
		                                              IEnumerable<IParameter> parameters);

		private IEnumerable<IParameter> GenerateIParametersFromParameterInfos(ParameterInfo[] parameterInfos)
		{
			List<IParameter> result = new List<IParameter>();

			foreach (ParameterInfo parameterInfo in parameterInfos)
			{
				ParameterAttribute attribute = GetInjectionAttribute(parameterInfo);
				result.Add(attribute.CreateParameter(parameterInfo.ParameterType));
			}

			return result;
		}

		private ParameterAttribute GetInjectionAttribute(ParameterInfo parameterInfo)
		{
			ParameterAttribute[] attributes =
				(ParameterAttribute[]) parameterInfo.GetCustomAttributes(typeof (ParameterAttribute), true);

			switch (attributes.Length)
			{
				case 0:
					return new DependencyAttribute();

				case 1:
					return attributes[0];

				default:
					throw new InvalidAttributeException();
			}
		}

		/// <summary>
		/// Abstract method used to determine whether a member should be processed. Must be overridden
		/// in derived classes.
		/// </summary>
		/// <param name="member">The member being reflected over.</param>
		/// <returns>Returns true if the member should get injection; false otherwise.</returns>
		protected abstract bool MemberRequiresProcessing(IReflectionMemberInfo<TMemberInfo> member);
	}
}
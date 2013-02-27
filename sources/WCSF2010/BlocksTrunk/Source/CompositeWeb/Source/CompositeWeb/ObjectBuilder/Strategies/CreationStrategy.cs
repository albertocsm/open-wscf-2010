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
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Microsoft.Practices.CompositeWeb.Properties;
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.CompositeWeb.ObjectBuilder.Strategies
{
	/// <summary>
	/// Implementation of <see cref="IBuilderStrategy"/> which creates objects.
	/// </summary>
	/// <remarks>
	/// <para>This strategy looks for policies in the context registered under the interface type
	/// <see cref="ICreationPolicy"/>. If it cannot find a policy on how to create the object,
	/// it will select the first constructor that returns from reflection, and re-runs the chain
	/// to create all the objects required to fulfill the constructor's parameters.</para>
	/// <para>If the Build method is passed an object via the existing parameter, then it
	/// will do nothing (since the object already exists). This allows this strategy to be
	/// in the chain when running dependency injection on existing objects, without fear that
	/// it will attempt to re-create the object.</para>
	/// </remarks>
	public class CreationStrategy : BuilderStrategy
	{
		/// <summary>
		/// Override of <see cref="IBuilderStrategy.BuildUp"/>. Creates the requested object.
		/// </summary>
		/// <param name="context">The build context.</param>
		/// <param name="typeToBuild">The type of object to be created.</param>
		/// <param name="existing">The existing object. If not null, this strategy does nothing.</param>
		/// <param name="idToBuild">The ID of the object to be created.</param>
		/// <returns>The created object</returns>
		public override object BuildUp(IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
		{
			if (existing != null)
				BuildUpExistingObject(context, typeToBuild, existing, idToBuild);
			else
				existing = BuildUpNewObject(context, typeToBuild, existing, idToBuild);

			return base.BuildUp(context, typeToBuild, existing, idToBuild);
		}

		private static void BuildUpExistingObject(IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
		{
			RegisterObject(context, typeToBuild, existing, idToBuild);
		}

		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		private static object BuildUpNewObject(IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
		{
			ICreationPolicy policy = context.Policies.Get<ICreationPolicy>(typeToBuild, idToBuild);

			if (policy == null)
			{
				if (idToBuild == null)
					throw new ArgumentException(String.Format(CultureInfo.CurrentCulture,
					                                          Resources.MissingPolicyUnnamed, typeToBuild));
				else
					throw new ArgumentException(String.Format(CultureInfo.CurrentCulture,
					                                          Resources.MissingPolicyNamed, typeToBuild, idToBuild));
			}

			try
			{
				existing = FormatterServices.GetSafeUninitializedObject(typeToBuild);
			}
			catch (MemberAccessException exception)
			{
				throw new ArgumentException(
					String.Format(CultureInfo.CurrentCulture, Resources.CannotCreateInstanceOfType, typeToBuild), exception);
			}

			RegisterObject(context, typeToBuild, existing, idToBuild);
			InitializeObject(context, existing, idToBuild, policy);
			return existing;
		}

		private static void RegisterObject(IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
		{
			if (context.Locator != null)
			{
				ILifetimeContainer lifetime = context.Locator.Get<ILifetimeContainer>(typeof (ILifetimeContainer), SearchMode.Local);

				if (lifetime != null)
				{
					ISingletonPolicy singletonPolicy = context.Policies.Get<ISingletonPolicy>(typeToBuild, idToBuild);

					if (singletonPolicy != null && singletonPolicy.IsSingleton)
					{
						lock (context.Locator)
						{
							context.Locator.Add(new DependencyResolutionLocatorKey(typeToBuild, idToBuild), existing);
						}

						lock (lifetime)
						{
							lifetime.Add(existing);
						}
					}
				}
			}
		}

		private static void InitializeObject(IBuilderContext context, object existing, string id, ICreationPolicy policy)
		{
			Type type = existing.GetType();
			ConstructorInfo constructor = policy.SelectConstructor(context, type, id);

			if (constructor == null)
			{
				if (type.IsValueType)
					return;
				throw new ArgumentException(Resources.NoAppropriateConstructor);
			}

			object[] parms = policy.GetParameters(context, type, id, constructor);

			MethodBase method = (MethodBase) constructor;
			ValidateMethodParameters(method, parms, existing.GetType());
			method.Invoke(existing, parms);
		}

		private static void ValidateMethodParameters(MethodBase methodInfo, object[] parameters, Type typeBeingBuilt)
		{
			ParameterInfo[] paramInfos = methodInfo.GetParameters();

			for (int i = 0; i < paramInfos.Length; i++)
				if (parameters[i] != null)
					TypeIsAssignableFromType(paramInfos[i].ParameterType, parameters[i].GetType(), typeBeingBuilt);
		}

		private static void TypeIsAssignableFromType(Type assignee, Type providedType, Type classBeingBuilt)
		{
			if (!assignee.IsAssignableFrom(providedType))
				throw new IncompatibleTypesException(string.Format(CultureInfo.CurrentCulture,
				                                                   Resources.TypeNotCompatible, assignee, providedType,
				                                                   classBeingBuilt));
		}
	}
}
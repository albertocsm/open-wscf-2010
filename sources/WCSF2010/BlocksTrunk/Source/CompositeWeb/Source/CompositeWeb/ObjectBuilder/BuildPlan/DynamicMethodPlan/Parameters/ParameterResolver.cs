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
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.Practices.CompositeWeb.Utility;
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.CompositeWeb.ObjectBuilder.BuildPlan.DynamicMethodPlan.Parameters
{
	/// <summary>
	/// A simple implementation of IParameterResolver that provides some
	/// useful utility methods, mainly around method lookup.
	/// </summary>
	public abstract class ParameterResolver : IParameterResolver
	{
		/// <summary>
		/// 
		/// </summary>
		protected static readonly MethodInfo GetTypeFromHandle =
			GetMethodInfo<Type>("GetTypeFromHandle", typeof (RuntimeTypeHandle));

		#region IParameterResolver Members

		/// <summary>
		/// 
		/// </summary>
		/// <param name="il"></param>
		/// <param name="paramAttr"></param>
		/// <param name="parameterType"></param>
		public abstract void EmitParameterResolution(
			ILGenerator il, ParameterAttribute paramAttr, Type parameterType);

		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TypeToSearch"></typeparam>
		/// <param name="methodName"></param>
		/// <param name="argumentTypes"></param>
		/// <returns></returns>
		protected static MethodInfo GetMethodInfo<TypeToSearch>(string methodName, params Type[] argumentTypes)
		{
			return typeof (TypeToSearch).GetMethod(methodName, argumentTypes);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TypeToSearch"></typeparam>
		/// <param name="propertyName"></param>
		/// <param name="propertyType"></param>
		/// <returns></returns>
		protected static MethodInfo GetPropertyGetter<TypeToSearch>(string propertyName, Type propertyType)
		{
			return typeof (TypeToSearch).GetProperty(propertyName, propertyType).GetGetMethod();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TypeToSearch"></typeparam>
		/// <param name="propertyName"></param>
		/// <param name="propertyType"></param>
		/// <returns></returns>
		protected static MethodInfo GetPropertySetter<TypeToSearch>(string propertyName, Type propertyType)
		{
			return typeof (TypeToSearch).GetProperty(propertyName, propertyType).GetSetMethod();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TypeToSearch"></typeparam>
		/// <param name="ctorArguments"></param>
		/// <returns></returns>
		protected static ConstructorInfo GetConstructor<TypeToSearch>(params Type[] ctorArguments)
		{
			return typeof (TypeToSearch).GetConstructor(ctorArguments);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="il"></param>
		/// <param name="t"></param>
		[SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods",
			Justification = "Validated using Guard class")]
		protected static void EmitLoadType(ILGenerator il, Type t)
		{
			Guard.ArgumentNotNull(il, "An IL Generator is required.");

			il.Emit(OpCodes.Ldtoken, t);
			il.EmitCall(OpCodes.Call, GetTypeFromHandle, null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected static MethodInfo ObtainGetFromLocatorMethod()
		{
			MethodInfo[] methods =
				typeof (IReadableLocator).GetMethods();
			foreach (MethodInfo method in methods)
			{
				if (method.Name == "Get" && !method.IsGenericMethod)
				{
					return method;
				}
			}
			throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
			                                                  "If you got here somethings seriously broken"));
		}
	}
}
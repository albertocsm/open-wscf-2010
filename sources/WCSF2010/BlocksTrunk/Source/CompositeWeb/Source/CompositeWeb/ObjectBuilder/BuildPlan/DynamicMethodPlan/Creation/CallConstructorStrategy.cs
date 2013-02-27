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
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.Practices.CompositeWeb.ObjectBuilder.BuildPlan.DynamicMethodPlan.Parameters;
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.CompositeWeb.ObjectBuilder.BuildPlan.DynamicMethodPlan.Creation
{
	/// <summary>
	/// 
	/// </summary>
	public class CallConstructorStrategy : PlanBuilderStrategy
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="typeToBuild"></param>
		/// <param name="existing"></param>
		/// <param name="idToBuild"></param>
		/// <param name="il"></param>
		protected override void BuildUp(IBuilderContext context, Type typeToBuild, object existing, string idToBuild,
		                                ILGenerator il)
		{
			IConstructorChooserPolicy ctorChooser = GetConstructorChooser(context, typeToBuild, idToBuild);
			ConstructorInfo ctor = ctorChooser.ChooseConstructor(typeToBuild);
			Label existingObjectNotNull = il.DefineLabel();
			Label done = il.DefineLabel();

			// If existing object (arg 2) is null, call the constructor
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Ldnull);
			il.Emit(OpCodes.Ceq);
			il.Emit(OpCodes.Brfalse, existingObjectNotNull);

			// resolve all constructor parameters...
			if (ctor != null)
			{
				foreach (ParameterInfo parameter in ctor.GetParameters())
				{
					EmitResolveParameter(il, parameter);
					FixupParameterType(il, parameter.ParameterType);
				}
			}
			// invoke constructor, leaving the new object on the top of the stack...
			EmitCallConstructor(il, ctor, typeToBuild);

			// And skip around the else clause
			il.Emit(OpCodes.Br_S, done);

			// We have an existing object, just get it on top of the stack
			il.MarkLabel(existingObjectNotNull);
			il.Emit(OpCodes.Ldarg_2);
			il.MarkLabel(done);
		}

		private static IConstructorChooserPolicy GetConstructorChooser(IBuilderContext context, Type typeToBuild,
		                                                               string idToBuild)
		{
			IConstructorChooserPolicy ctorChooser =
				context.Policies.Get<IConstructorChooserPolicy>(typeToBuild, idToBuild);

			if (ctorChooser == null)
			{
				ctorChooser = new DefaultConstructorChooserPolicy();
				context.Policies.Set<IConstructorChooserPolicy>(ctorChooser, typeToBuild, idToBuild);
			}
			return ctorChooser;
		}

		private static void EmitResolveParameter(ILGenerator il, ParameterInfo param)
		{
			ParameterAttribute attr = GetParameterAttribute(param);
			IParameterResolver resolver = ParameterResolverMap.GetResolver(attr);
			resolver.EmitParameterResolution(il, attr, param.ParameterType);
		}

		private static void EmitCallConstructor(ILGenerator il, ConstructorInfo ctor, Type t)
		{
			if (ctor != null)
			{
				il.Emit(OpCodes.Newobj, ctor);
			}
			else
			{
				// Things like int don't have a constructor at all. Fall back on Activator.CreateInstance
				// We'll figure out better IL for this special case later
				MethodInfo getTypeFromHandle =
					typeof (Type).GetMethod("GetTypeFromHandle",
					                        new Type[] {typeof (RuntimeTypeHandle)});
				MethodInfo createInstance =
					typeof (Activator).GetMethod("CreateInstance",
					                             new Type[] {typeof (Type)});
				il.Emit(OpCodes.Ldtoken, t);
				il.EmitCall(OpCodes.Call, getTypeFromHandle, null);
				il.EmitCall(OpCodes.Call, createInstance, null);
			}
		}

		private static void FixupParameterType(ILGenerator il, Type parameterType)
		{
			if (parameterType.IsValueType)
			{
				il.Emit(OpCodes.Unbox_Any, parameterType);
			}
		}

		private static ParameterAttribute GetParameterAttribute(ParameterInfo param)
		{
			object[] attrs = param.GetCustomAttributes(typeof (ParameterAttribute), false);
			if (attrs.Length == 0)
			{
				return new DependencyAttribute();
			}
			if (attrs.Length > 1)
			{
				throw new InvalidAttributeException(string.Format(CultureInfo.CurrentCulture,
				                                                  "Can only have one parameter attribute for parameter {0}",
				                                                  param.Name));
			}

			return (ParameterAttribute) attrs[0];
		}
	}
}
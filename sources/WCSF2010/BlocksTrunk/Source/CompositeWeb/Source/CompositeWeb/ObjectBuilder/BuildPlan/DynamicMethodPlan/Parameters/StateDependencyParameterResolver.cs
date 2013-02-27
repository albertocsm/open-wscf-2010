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
using System.Reflection;
using System.Reflection.Emit;
using System.Web.SessionState;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.CompositeWeb.ObjectBuilder.BuildPlan.DynamicMethodPlan.Parameters
{
	/// <summary>
	/// 
	/// </summary>
	public class StateDependencyParameterResolver : ParameterResolver
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="il"></param>
		/// <param name="paramAttr"></param>
		/// <param name="parameterType"></param>
		public override void EmitParameterResolution(
			ILGenerator il, ParameterAttribute paramAttr, Type parameterType)
		{
			if (!typeof (IStateValue).IsAssignableFrom(parameterType))
			{
				throw new ArgumentException("StateDependency parameters must be of type IStateValue");
			}

			StateDependencyAttribute stateAttr = (StateDependencyAttribute) paramAttr;

			MethodInfo getLocator = GetPropertyGetter<IBuilderContext>("Locator", typeof (IReadWriteLocator));
			MethodInfo getFromLocator = ObtainGetFromLocatorMethod();
			MethodInfo getSessionState = GetMethodInfo<ISessionStateLocatorService>("GetSessionState");
			MethodInfo setSessionState = GetPropertySetter<IStateValue>("SessionState", typeof (IHttpSessionState));
			MethodInfo setKeyName = GetPropertySetter<IStateValue>("KeyName", typeof (string));
			ConstructorInfo stateValueCtor = parameterType.GetConstructor(new Type[0]);
			ConstructorInfo keyCtor = GetConstructor<DependencyResolutionLocatorKey>(typeof (Type), typeof (string));

			// Get session locator from context, store in a local
			LocalBuilder sessionStateLocator = il.DeclareLocal(typeof (ISessionStateLocatorService));

			// Get locator 
			il.Emit(OpCodes.Ldarg_0);
			il.EmitCall(OpCodes.Callvirt, getLocator, null);

			// Get Session state provider

			// Create key
			EmitLoadType(il, typeof (ISessionStateLocatorService));
			il.Emit(OpCodes.Ldnull);
			il.Emit(OpCodes.Newobj, keyCtor);

			// Look up in locator
			il.EmitCall(OpCodes.Callvirt, getFromLocator, null);
			il.Emit(OpCodes.Stloc, sessionStateLocator);

			Label noLocator = il.DefineLabel();
			Label done = il.DefineLabel();

			// Do we have a session locator?
			il.Emit(OpCodes.Ldloc, sessionStateLocator);
			il.Emit(OpCodes.Ldnull);
			il.Emit(OpCodes.Ceq);

			il.Emit(OpCodes.Brtrue, noLocator);

			// Session state locator, new up our state value
			LocalBuilder valueLocal = il.DeclareLocal(typeof (IStateValue));

			il.Emit(OpCodes.Newobj, stateValueCtor);
			il.Emit(OpCodes.Castclass, typeof (IStateValue));
			il.Emit(OpCodes.Stloc, valueLocal);

			il.Emit(OpCodes.Ldloc, valueLocal);
			il.Emit(OpCodes.Ldloc, sessionStateLocator);
			il.EmitCall(OpCodes.Callvirt, getSessionState, null);
			il.EmitCall(OpCodes.Callvirt, setSessionState, null);
			il.Emit(OpCodes.Ldloc, valueLocal);
			il.Emit(OpCodes.Ldstr, stateAttr.KeyName);
			il.EmitCall(OpCodes.Callvirt, setKeyName, null);
			il.Emit(OpCodes.Ldloc, valueLocal);
			il.Emit(OpCodes.Br_S, done);
			il.MarkLabel(noLocator);
			il.Emit(OpCodes.Ldnull);
			il.MarkLabel(done);
		}
	}
}
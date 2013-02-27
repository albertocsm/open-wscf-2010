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
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.CompositeWeb.ObjectBuilder.BuildPlan.DynamicMethodPlan.Parameters
{
	/// <summary>
	/// 
	/// </summary>
	public class CreateNewParameterResolver : ParameterResolver
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="il"></param>
		/// <param name="paramAttr"></param>
		/// <param name="parameterType"></param>
		public override void EmitParameterResolution(ILGenerator il, ParameterAttribute paramAttr, Type parameterType)
		{
			MethodInfo getHeadOfChain = GetPropertyGetter<IBuilderContext>("HeadOfChain", typeof (IBuilderStrategy));
			MethodInfo buildUp = GetMethodInfo<IBuilderStrategy>("BuildUp",
			                                                     typeof (IBuilderContext), typeof (Type), typeof (object),
			                                                     typeof (string));

			// Get the head of the context chain
			il.Emit(OpCodes.Ldarg_0); // Get context onto the stack
			il.EmitCall(OpCodes.Callvirt, getHeadOfChain, null); // Now head of chain is on the stack

			// Build up parameters to the BuildUp call - context, type, null existing, id
			il.Emit(OpCodes.Ldarg_0); // Push context onto stack
			EmitLoadType(il, parameterType);

			// Existing object is null
			il.Emit(OpCodes.Ldnull);

			// And the id
			il.Emit(OpCodes.Ldarg_3);

			// Call buildup on head of the chain
			il.EmitCall(OpCodes.Callvirt, buildUp, null);
		}
	}
}
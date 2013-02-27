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
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.Practices.CompositeWeb.ObjectBuilder.BuildPlan.DynamicMethodPlan.Parameters;
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.CompositeWeb.ObjectBuilder.BuildPlan.DynamicMethodPlan.Properties
{
	/// <summary>
	/// 
	/// </summary>
	public class SetPropertiesStrategy : PlanBuilderStrategy
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="typeToBuild"></param>
		/// <param name="existing"></param>
		/// <param name="idToBuild"></param>
		/// <param name="il"></param>
		protected override void BuildUp(
			IBuilderContext context, Type typeToBuild, object existing, string idToBuild, ILGenerator il)
		{
			List<PropertyInfo> properties = new List<PropertyInfo>(GetInjectionProperties(context, typeToBuild, idToBuild));
			if (properties.Count > 0)
			{
				// At entry, object we're building up is on top of the stack.
				// We'll need that a lot, so create a local to store it, and 
				// put it in there
				LocalBuilder existingObject = il.DeclareLocal(typeToBuild);
				il.Emit(OpCodes.Stloc, existingObject);

				foreach (PropertyInfo prop in properties)
				{
					il.Emit(OpCodes.Ldloc, existingObject);
					EmitResolveProperty(il, prop);
					il.EmitCall(OpCodes.Callvirt, prop.GetSetMethod(), null);
				}

				// On exit we need to make sure the existing object is top of stack again
				il.Emit(OpCodes.Ldloc, existingObject);
			}
		}

		private IEnumerable<PropertyInfo> GetInjectionProperties(IBuilderContext context, Type typeToBuild, string idToBuild)
		{
			IPropertyChooserPolicy chooser = GetPropertyChooser(context, idToBuild, typeToBuild);
			return chooser.GetInjectionProperties(typeToBuild);
		}

		private static IPropertyChooserPolicy GetPropertyChooser(IBuilderContext context, string idToBuild, Type typeToBuild)
		{
			IPropertyChooserPolicy chooser =
				context.Policies.Get<IPropertyChooserPolicy>(typeToBuild, idToBuild);
			if (chooser == null)
			{
				chooser = new DefaultPropertyChooserPolicy();
				context.Policies.Set(chooser, typeToBuild, idToBuild);
			}
			return chooser;
		}

		private void EmitResolveProperty(ILGenerator il, PropertyInfo prop)
		{
			ParameterAttribute attr = GetParameterAttribute(prop);
			IParameterResolver resolver = ParameterResolverMap.GetResolver(attr);
			resolver.EmitParameterResolution(il, attr, prop.PropertyType);
			if (prop.PropertyType.IsValueType)
			{
				il.Emit(OpCodes.Unbox_Any, prop.PropertyType);
			}
		}

		private static ParameterAttribute GetParameterAttribute(PropertyInfo prop)
		{
			object[] attrs = prop.GetCustomAttributes(typeof (ParameterAttribute), true);
			if (attrs.Length != 1)
			{
				throw new InvalidAttributeException(string.Format(CultureInfo.CurrentCulture,
				                                                  "Must have exactly one parameter attribute"));
			}
			return (ParameterAttribute) attrs[0];
		}
	}
}
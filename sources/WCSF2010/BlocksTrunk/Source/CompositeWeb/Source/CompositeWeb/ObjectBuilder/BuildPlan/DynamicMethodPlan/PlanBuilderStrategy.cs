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
using System.Reflection.Emit;
using Microsoft.Practices.CompositeWeb.Utility;
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.CompositeWeb.ObjectBuilder.BuildPlan.DynamicMethodPlan
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class PlanBuilderStrategy : BuilderStrategy
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="typeToBuild"></param>
		/// <param name="existing"></param>
		/// <param name="idToBuild"></param>
		/// <returns></returns>
		public override object BuildUp(
			IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
		{
			ILGenerator il = existing as ILGenerator;
			Guard.ArgumentNotNull(existing, "Must have existing IL generator");
			Guard.ArgumentNotNull(il, "Existing object must be IL Generator");

			BuildUp(context, typeToBuild, existing, idToBuild, il);

			return base.BuildUp(context, typeToBuild, existing, idToBuild);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="typeToBuild"></param>
		/// <param name="existing"></param>
		/// <param name="idToBuild"></param>
		/// <param name="il"></param>
		protected abstract void BuildUp(IBuilderContext context, Type typeToBuild, object existing, string idToBuild,
		                                ILGenerator il);
	}
}
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
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.CompositeWeb.ObjectBuilder.BuildPlan
{
	/// <summary>
	/// A builder strategy that will use a build plan to do the construction and injection.
	/// </summary>
	public class BuildPlanStrategy : BuilderStrategy
	{
		/// <summary>
		/// Implementation of <see cref="IBuilderStrategy.BuildUp"/>.
		/// </summary>
		/// <param name="context">The build context.</param>
		/// <param name="typeToBuild">The type of the object being built.</param>
		/// <param name="existing">The existing instance of the object.</param>
		/// <param name="idToBuild">The ID of the object being built.</param>
		/// <returns>The built object.</returns>
		public override object BuildUp(
			IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
		{
			IBuildPlan buildPlan = GetPlanFromContext(context, typeToBuild, idToBuild);
			existing = buildPlan.BuildUp(context, typeToBuild, existing, idToBuild);
			return base.BuildUp(context, typeToBuild, existing, idToBuild);
		}

		private static IBuildPlan GetPlanFromContext(IBuilderContext context, Type typeToBuild, string idToBuild)
		{
			IBuildPlanPolicy planPolicy = GetPlanPolicyFromContext(context, typeToBuild, idToBuild);
			IBuildPlan plan = planPolicy.Get(typeToBuild);
			if (plan == null)
			{
				IPlanBuilderPolicy builderPolicy = GetPlanBuilderFromContext(context, typeToBuild, idToBuild);
				plan = builderPolicy.CreatePlan(typeToBuild, idToBuild);
				planPolicy.Set(typeToBuild, plan);
			}
			return plan;
		}

		private static IPlanBuilderPolicy GetPlanBuilderFromContext(IBuilderContext context, Type typeToBuild,
		                                                            string idToBuild)
		{
			IPlanBuilderPolicy planBuilder = context.Policies.Get<IPlanBuilderPolicy>(typeToBuild, idToBuild);
			if (planBuilder == null)
			{
				throw new InvalidOperationException("No Plan builder configured");
			}
			return planBuilder;
		}

		private static IBuildPlanPolicy GetPlanPolicyFromContext(IBuilderContext context, Type typeToBuild, string idToBuild)
		{
			IBuildPlanPolicy policy = context.Policies.Get<IBuildPlanPolicy>(typeToBuild, idToBuild);
			if (policy == null)
			{
				policy = new BuildPlanPolicy();
				context.Policies.Set<IBuildPlanPolicy>(policy, typeToBuild, idToBuild);
			}
			return policy;
		}
	}
}
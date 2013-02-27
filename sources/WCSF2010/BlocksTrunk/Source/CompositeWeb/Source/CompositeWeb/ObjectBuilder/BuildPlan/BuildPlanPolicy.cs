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

namespace Microsoft.Practices.CompositeWeb.ObjectBuilder.BuildPlan
{
	/// <summary>
	/// 
	/// </summary>
	public class BuildPlanPolicy : IBuildPlanPolicy
	{
		private volatile Dictionary<Type, IBuildPlan> buildPlans;
		private object lockObject;

		/// <summary>
		/// 
		/// </summary>
		public BuildPlanPolicy()
		{
			buildPlans = new Dictionary<Type, IBuildPlan>();
			lockObject = new object();
		}

		#region IBuildPlanPolicy Members

		/// <summary>
		/// 
		/// </summary>
		/// <param name="typeToBuild"></param>
		/// <returns></returns>
		public IBuildPlan Get(Type typeToBuild)
		{
			if (buildPlans.ContainsKey(typeToBuild))
			{
				return buildPlans[typeToBuild];
			}
			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="typeToBuild"></param>
		/// <param name="plan"></param>
		public void Set(Type typeToBuild, IBuildPlan plan)
		{
			lock (lockObject)
			{
				Dictionary<Type, IBuildPlan> newPlans =
					new Dictionary<Type, IBuildPlan>(buildPlans);
				newPlans[typeToBuild] = plan;
				buildPlans = newPlans;
			}
		}

		#endregion
	}
}
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
	/// This policy stores the currently available build plans based
	/// on type and ID.
	/// </summary>
	public interface IBuildPlanPolicy : IBuilderPolicy
	{
		/// <summary>
		/// Return a BuildPlan for the specified Type.
		/// </summary>
		/// <param name="typeToBuild">The Type to build</param>
		/// <returns>The BuildPlan</returns>
		IBuildPlan Get(Type typeToBuild);

		/// <summary>
		/// Sets a BuildPlan for a specific Type.
		/// </summary>
		/// <param name="typeToBuild">The Type to be built</param>
		/// <param name="plan">The BuildPlan to asociate.</param>
		void Set(Type typeToBuild, IBuildPlan plan);
	}
}
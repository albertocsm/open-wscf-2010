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

namespace Microsoft.Practices.CompositeWeb.ObjectBuilder.BuildPlan.DynamicMethodPlan
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	/// <param name="typeToBuild"></param>
	/// <param name="existing"></param>
	/// <param name="id"></param>
	/// <returns></returns>
	public delegate object BuildUpHandler(
		IBuilderContext context, Type typeToBuild, object existing, string id);

	/// <summary>
	/// 
	/// </summary>
	public class DynamicMethodBuildPlan : IBuildPlan
	{
		private BuildUpHandler buildUpMethod;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="buildUpMethod"></param>
		public DynamicMethodBuildPlan(BuildUpHandler buildUpMethod)
		{
			this.buildUpMethod = buildUpMethod;
		}

		#region IBuildPlan Members

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="typeToBuild"></param>
		/// <param name="existing"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		public object BuildUp(IBuilderContext context, Type typeToBuild, object existing, string id)
		{
			return buildUpMethod(context, typeToBuild, existing, id);
		}

		#endregion
	}
}
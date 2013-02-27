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
using Microsoft.Practices.CompositeWeb.ObjectBuilder.BuildPlan.DynamicMethodPlan;
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.CompositeWeb.Tests.Mocks.ObjectBuilder.BuildPlan.DynamicMethodPlan
{
	internal class MockReturnExistingPlanBuilderStrategy : PlanBuilderStrategy
	{
		private bool _buildUpCalled;

		public bool BuildUpCalled
		{
			get { return _buildUpCalled; }
			set { _buildUpCalled = value; }
		}

		protected override void BuildUp(IBuilderContext context, Type typeToBuild, object existing, string idToBuild,
		                                ILGenerator il)
		{
			_buildUpCalled = true;

			Label done = il.DefineLabel();

			il.Emit(OpCodes.Ldarg_2);
			il.MarkLabel(done);
		}
	}
}
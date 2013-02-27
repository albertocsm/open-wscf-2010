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
using Microsoft.Practices.CompositeWeb.ObjectBuilder.BuildPlan;

namespace Microsoft.Practices.CompositeWeb.Tests.Mocks.ObjectBuilder.BuildPlan
{
	internal class MockPlanBuilderPolicy : IPlanBuilderPolicy
	{
		private IBuildPlan _buildPlan;

		public MockPlanBuilderPolicy(IBuildPlan builPlan)
		{
			_buildPlan = builPlan;
		}

		#region IPlanBuilderPolicy Members

		public IBuildPlan CreatePlan(Type typeToBuild, string id)
		{
			return _buildPlan;
		}

		#endregion
	}
}
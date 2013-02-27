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
//===============================================================================
// Microsoft patterns & practices
// Web Client Software Factory
//-------------------------------------------------------------------------------
// Copyright (C) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//-------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================
using System;
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.CompositeWeb.BuilderStrategies
{
	/// <summary>
	/// A simple strategy that uses the current container to swap types as
	/// we work down the strategy chain.
	/// </summary>
	public class ContainerAwareTypeMappingStrategy : BuilderStrategy
	{
		/// <summary>
		/// Build up the requested object.
		/// </summary>
		/// <param name="context">Current build context</param>
		/// <param name="typeToBuild">Type of object to build.</param>
		/// <param name="existing">Existing object (if any)</param>
		/// <param name="idToBuild">Id of object to build.</param>
		/// <returns>The constructed object.</returns>
		public override object BuildUp(
			IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
		{
			if (existing == null)
			{
				IContainerAwareTypeMappingPolicy policy =
					context.Policies.Get<IContainerAwareTypeMappingPolicy>(typeToBuild, idToBuild);
				if (policy != null)
				{
					DependencyResolutionLocatorKey key = policy.Map(context.Locator,
					                                                new DependencyResolutionLocatorKey(typeToBuild,
					                                                                                   idToBuild));
					return base.BuildUp(context, key.Type, existing, idToBuild);
				}
			}
			return base.BuildUp(context, typeToBuild, existing, idToBuild);
		}
	}
}
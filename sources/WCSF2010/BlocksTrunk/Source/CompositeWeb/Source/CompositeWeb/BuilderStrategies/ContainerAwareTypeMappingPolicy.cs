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
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.CompositeWeb.BuilderStrategies
{
	/// <summary>
	/// Implementation of the <see cref="IContainerAwareTypeMappingPolicy"/> interface that
	/// finds the current composition container and reads the type mapping from it.
	/// </summary>
	public class ContainerAwareTypeMappingPolicy : IContainerAwareTypeMappingPolicy
	{
		#region IContainerAwareTypeMappingPolicy Members

		/// <summary>
		/// Maps one type/ID pair to another, using the locator to find the
		/// current <see cref="CompositionContainer"/>. The container holds
		/// the current set of type mappings.
		/// </summary>
		/// <param name="locator">The <see cref="IReadableLocator"/> being used in the
		/// current BuildUp operation.</param>
		/// <param name="incomingTypeIdPair"><see cref="DependencyResolutionLocatorKey"/> that identifies
		/// the type of the current BuildUp operation.</param>
		/// <returns>The new type to return; returns the original key if no type mapping is defined.</returns>
		public DependencyResolutionLocatorKey Map(
			IReadableLocator locator, DependencyResolutionLocatorKey incomingTypeIdPair)
		{
			DependencyResolutionLocatorKey result = incomingTypeIdPair;
			CompositionContainer container = GetCompositionContainerFromLocator(locator);
			if (container != null)
			{
				result = new DependencyResolutionLocatorKey(container.GetMappedType(result.Type),
				                                            incomingTypeIdPair.ID);
			}
			return result;
		}

		#endregion

		private CompositionContainer GetCompositionContainerFromLocator(IReadableLocator locator)
		{
			return (CompositionContainer)
			       locator.Get(
			       	new DependencyResolutionLocatorKey(typeof (CompositionContainer), null));
		}
	}
}
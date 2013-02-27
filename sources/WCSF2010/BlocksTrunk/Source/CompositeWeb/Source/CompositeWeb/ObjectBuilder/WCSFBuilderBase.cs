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
using Microsoft.Practices.CompositeWeb.Properties;
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.CompositeWeb.ObjectBuilder
{
	/// <summary>
	/// An implementation helper class for <see cref="IBuilder{TStageEnum}"/>.
	/// </summary>
	/// <typeparam name="TStageEnum">The build stage enumeration.</typeparam>
	public class WCSFBuilderBase<TStageEnum> : IBuilder<TStageEnum>
	{
		private PolicyList _policies = new PolicyList();
		private StrategyList<TStageEnum> _strategies = new StrategyList<TStageEnum>();

		/// <summary>
		/// Initializes a new instance of the <see cref="WCSFBuilderBase{T}"/> class.
		/// </summary>
		public WCSFBuilderBase()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="WCSFBuilderBase{T}"/> class using the
		/// provided configurator.
		/// </summary>
		/// <param name="configurator">The configurator that will configure the builder.</param>
		public WCSFBuilderBase(IBuilderConfigurator<TStageEnum> configurator)
		{
			configurator.ApplyConfiguration(this);
		}

		#region IBuilder<TStageEnum> Members

		/// <summary>
		/// See <see cref="IBuilder{TStageEnum}.Policies"/> for more information.
		/// </summary>
		public PolicyList Policies
		{
			get { return _policies; }
		}

		/// <summary>
		/// See <see cref="IBuilder{TStageEnum}.Strategies"/> for more information.
		/// </summary>
		public StrategyList<TStageEnum> Strategies
		{
			get { return _strategies; }
		}

		/// <summary>
		/// See <see cref="IBuilder{TStageEnum}.BuildUp{T}"/> for more information.
		/// </summary>
		public TTypeToBuild BuildUp<TTypeToBuild>(IReadWriteLocator locator,
		                                          string idToBuild, object existing, params PolicyList[] transientPolicies)
		{
			return (TTypeToBuild) BuildUp(locator, typeof (TTypeToBuild), idToBuild, existing, transientPolicies);
		}

		/// <summary>
		/// See <see cref="IBuilder{TStageEnum}.BuildUp"/> for more information.
		/// </summary>
		public virtual object BuildUp(IReadWriteLocator locator, Type typeToBuild,
		                              string idToBuild, object existing, params PolicyList[] transientPolicies)
		{
			return DoBuildUp(locator, typeToBuild, idToBuild, existing, transientPolicies);
		}

		/// <summary>
		/// See <see cref="IBuilder{TStageEnum}.TearDown{T}"/> for more information.
		/// </summary>
		public TItem TearDown<TItem>(IReadWriteLocator locator, TItem item)
		{
			if (typeof (TItem).IsValueType == false && item == null)
				throw new ArgumentNullException("item");

			return DoTearDown<TItem>(locator, item);
		}

		#endregion

		private object DoBuildUp(IReadWriteLocator locator, Type typeToBuild, string idToBuild, object existing,
		                         PolicyList[] transientPolicies)
		{
			IBuilderStrategyChain chain = _strategies.MakeStrategyChain();
			ThrowIfNoStrategiesInChain(chain);

			IBuilderContext context = MakeContext(chain, locator, transientPolicies);

			object result = chain.Head.BuildUp(context, typeToBuild, existing, idToBuild);
			return result;
		}

		private IBuilderContext MakeContext(IBuilderStrategyChain chain,
		                                    IReadWriteLocator locator, params PolicyList[] transientPolicies)
		{
			PolicyList policies = new PolicyList(_policies);

			foreach (PolicyList policyList in transientPolicies)
				policies.AddPolicies(policyList);

			return new BuilderContext(chain, locator, policies);
		}

		private static void ThrowIfNoStrategiesInChain(IBuilderStrategyChain chain)
		{
			if (chain.Head == null)
				throw new InvalidOperationException(Resources.BuilderHasNoStrategies);
		}

		private TItem DoTearDown<TItem>(IReadWriteLocator locator, TItem item)
		{
			IBuilderStrategyChain chain = _strategies.MakeReverseStrategyChain();
			ThrowIfNoStrategiesInChain(chain);

			IBuilderContext context = MakeContext(chain, locator);

			TItem result = (TItem) chain.Head.TearDown(context, item);
			return result;
		}
	}
}
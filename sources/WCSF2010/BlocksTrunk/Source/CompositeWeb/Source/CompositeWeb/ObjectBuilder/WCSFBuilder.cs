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
using Microsoft.Practices.CompositeWeb.ObjectBuilder.BuildPlan;
using Microsoft.Practices.CompositeWeb.ObjectBuilder.BuildPlan.DynamicMethodPlan;
using Microsoft.Practices.CompositeWeb.ObjectBuilder.BuildPlan.DynamicMethodPlan.Creation;
using Microsoft.Practices.CompositeWeb.ObjectBuilder.BuildPlan.DynamicMethodPlan.Method;
using Microsoft.Practices.CompositeWeb.ObjectBuilder.BuildPlan.DynamicMethodPlan.Properties;
using Microsoft.Practices.CompositeWeb.ObjectBuilder.Strategies;
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.CompositeWeb.ObjectBuilder
{
	/// <summary>
	/// An implementation of <see cref="IBuilder{TStageEnum}"/> which uses <see cref="WCSFBuilderStage"/>
	/// as the stages of the build process. It is very similar to <see cref="Builder"/>, but has a custom
	/// locking strategy, which is finer grained than the previous locking strategy.
	/// </summary>
	public class WCSFBuilder : WCSFBuilderBase<WCSFBuilderStage>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="WCSFBuilder"/> class 
		/// </summary>
		public WCSFBuilder()
			: this(null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="WCSFBuilder"/> class using the provided
		/// configurator.
		/// </summary>
		/// <param name="configurator">The configurator that will configure the builder.</param>
		public WCSFBuilder(IBuilderConfigurator<WCSFBuilderStage> configurator)
		{
			Strategies.AddNew<TypeMappingStrategy>(WCSFBuilderStage.PreCreation);
			Strategies.AddNew<SimplifiedSingletonStrategy>(WCSFBuilderStage.PreCreation);
			Strategies.AddNew<BuildPlanStrategy>(WCSFBuilderStage.Creation);

			Strategies.AddNew<BuilderAwareStrategy>(WCSFBuilderStage.PostInitialization);

			Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
			Policies.SetDefault<IBuildPlanPolicy>(new BuildPlanPolicy());
			Policies.SetDefault<IPlanBuilderPolicy>(CreatePlanBuilder());

			if (configurator != null)
			{
				configurator.ApplyConfiguration(this);
			}
		}

		private static IPlanBuilderPolicy CreatePlanBuilder()
		{
			BuilderStrategyChain chain = new BuilderStrategyChain();
			chain.Add(new CallConstructorStrategy());
			chain.Add(new SetPropertiesStrategy());
			chain.Add(new CallMethodsStrategy());

			PolicyList policies = new PolicyList();
			policies.SetDefault<IConstructorChooserPolicy>(new AttributeBasedConstructorChooser());
			policies.SetDefault<IPropertyChooserPolicy>(new AttributeBasedPropertyChooser());
			policies.SetDefault<IMethodChooserPolicy>(new AttributeBasedMethodChooser());

			return new DynamicMethodPlanBuilderPolicy(chain, policies);
		}
	}
}
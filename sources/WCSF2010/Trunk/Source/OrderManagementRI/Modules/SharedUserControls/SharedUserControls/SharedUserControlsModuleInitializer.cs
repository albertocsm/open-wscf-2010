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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Services;
using Microsoft.Practices.CompositeWeb.Configuration;
using Microsoft.Practices.CompositeWeb.EnterpriseLibrary.Services;
using OrderManagement.SharedUserControls.Services;
using OrdersRepository.Interfaces.Services;
using OrdersRepository.Services;

namespace OrderManagement.SharedUserControls
{
	public class SharedUserControlsModuleInitializer : ModuleInitializer
	{
		private const string AuthorizationSection = "compositeWeb/authorization";

		public override void Load(CompositionContainer container)
		{
			base.Load(container);

			AddGlobalServices(container.Parent.Services);
			AddModuleServices(container.Services);
		}

		protected virtual void AddGlobalServices(IServiceCollection globalServices)
		{
			// Registration for this service has moved to the Customers\Web.config file to show how config based registration works.
			//globalServices.AddNew<CustomerService, ICustomerService>();

			globalServices.AddNew<FindCustomerService, IFindCustomerService>();
		}

		protected virtual void AddModuleServices(IServiceCollection moduleServices)
		{
			// TODO: add a service that will be visible to this module
		}

		public override void Configure(IServiceCollection services, System.Configuration.Configuration moduleConfiguration)
		{
			IAuthorizationRulesService authorizationRuleService = services.Get<IAuthorizationRulesService>();
			if (authorizationRuleService != null)
			{
				AuthorizationConfigurationSection authorizationSection = moduleConfiguration.GetSection(AuthorizationSection) as AuthorizationConfigurationSection;
				if (authorizationSection != null)
				{
					foreach (AuthorizationRuleElement ruleElement in authorizationSection.ModuleRules)
					{
						authorizationRuleService.RegisterAuthorizationRule(ruleElement.AbsolutePath, ruleElement.RuleName);
					}
				}
			}
		}
	}
}

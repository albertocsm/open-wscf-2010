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
using RealTimeSearchQuickstart.CustomerManager.Services;

namespace RealTimeSearchQuickstart.CustomerManager
{
	public class CustomerManagerModuleInitializer : ModuleInitializer
	{
		private const string AuthorizationSection = "compositeWeb/authorization";

		public override void Load(CompositionContainer container)
		{
			base.Load(container);

			AddGlobalServices(container.Parent.Services);
			AddModuleServices(container.Services);
			RegisterSiteMapInformation(container.Services.Get<ISiteMapBuilderService>(true));

			container.RegisterTypeMapping<ICustomerManagerController, CustomerManagerController>();
		}

		protected virtual void AddGlobalServices(IServiceCollection globalServices)
		{
			// TODO: add a service that will be visible to any module
		}

		protected virtual void AddModuleServices(IServiceCollection moduleServices)
		{
			moduleServices.AddNew<CustomerService, ICustomerService>();
		}

		protected virtual void RegisterSiteMapInformation(ISiteMapBuilderService siteMapBuilderService)
		{
			SiteMapNodeInfo moduleNode = new SiteMapNodeInfo("CustomerManager", "~/CustomerManager/Default.aspx", "Customers");
			siteMapBuilderService.AddNode(moduleNode);

			SiteMapNodeInfo searchNode = new SiteMapNodeInfo("SearchCustomer", "~/CustomerManager/SearchCustomer.aspx", "Search");
			siteMapBuilderService.AddNode(searchNode, moduleNode);

			// TODO: register other site map nodes that CustomerManager module might provide            
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

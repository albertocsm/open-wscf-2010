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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Services;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Configuration;
using Microsoft.Practices.CompositeWeb.Authorization;
using Microsoft.Practices.CompositeWeb.EnterpriseLibrary.Services;

namespace $ShellNamespace$
{
    public class ShellModuleInitializer : ModuleInitializer
    {        
        private const string AuthorizationSection = "compositeWeb/authorization";        

        public override void Load(CompositionContainer container)
		{
			base.Load(container);

			AddGlobalServices(container.Parent.Services);
            AddModuleServices(container.Services);
            RegisterSiteMapInformation(container.Services.Get<ISiteMapBuilderService>(true));
		}

        protected virtual void AddGlobalServices(IServiceCollection globalServices)
		{
            globalServices.AddNew<EnterpriseLibraryAuthorizationService, IAuthorizationService>();
            globalServices.AddNew<SiteMapBuilderService, ISiteMapBuilderService>();
		}

		protected virtual void AddModuleServices(IServiceCollection moduleServices)
		{
            // TODO: register services that can be accesed only by the Shell module
		}

        protected virtual void RegisterSiteMapInformation(ISiteMapBuilderService siteMapBuilderService)
        {
            SiteMapNodeInfo moduleNode = new SiteMapNodeInfo("Home", "~/Default.aspx", "Home", "Home");
            siteMapBuilderService.AddNode(moduleNode);

            siteMapBuilderService.RootNode.Url = "~/Default.aspx";
            siteMapBuilderService.RootNode.Title = "$ApplicationNameArgument$";            
            
            // TODO: register other site map nodes for pages that belong to the website root
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

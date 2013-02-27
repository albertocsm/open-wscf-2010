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


using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Authorization;
using Microsoft.Practices.CompositeWeb.Configuration;
using Microsoft.Practices.CompositeWeb.Interfaces;
using GlobalBank.Commercial.EBanking.Modules.Reports.Constants;

namespace GlobalBank.Commercial.EBanking.Modules.Reports
{
    public class ReportsModuleInitializer : ModuleInitializer
    {
        private const string AuthorizationSection = "compositeWeb/authorization";
    	
        public override void Load(CompositionContainer moduleContainer)
		{
			base.Load(moduleContainer);
        	
			RegisterSiteMapInformation(moduleContainer.Services.Get<ISiteMapBuilderService>(true));
			RegisterRequiredPermissions(moduleContainer.Services.Get<IPermissionsCatalog>(true));
		}

		protected virtual void RegisterSiteMapInformation(ISiteMapBuilderService siteMapBuilderService)
		{
			NameValueCollection attributes = new NameValueCollection(1);
			attributes["imageName"] = "reports";
			SiteMapNodeInfo moduleNode = new SiteMapNodeInfo("Reports", "~/Reports/Default.aspx", "Reports", "Reports Module", null, attributes, null, null);
			SiteMapNodeInfo viewReportNode = new SiteMapNodeInfo("AccountsSummary", "~/Reports/AccountsSummaryView.aspx", "Accounts Summary");
			SiteMapNodeInfo paymentHistory = new SiteMapNodeInfo("PaymentHistory", "~/Reports/PaymentHistoryView.aspx", "Payment History");

			siteMapBuilderService.AddNode(moduleNode, 500);
			siteMapBuilderService.AddNode(viewReportNode, moduleNode, Permissions.AllowViewAccountsSummary);
			siteMapBuilderService.AddNode(paymentHistory, moduleNode, Permissions.AllowViewPaymentHistory);
		}

		protected virtual void RegisterRequiredPermissions(IPermissionsCatalog permissionsCatalog)
		{
			List<Action> moduleActions = new List<Action>();
			moduleActions.Add(new Action("Allow view Accounts Summary", Permissions.AllowViewAccountsSummary));
			moduleActions.Add(new Action("Allow view Payment History", Permissions.AllowViewPaymentHistory));
			ModuleActionSet set = new ModuleActionSet("Reports", moduleActions);
			
			permissionsCatalog.RegisterPermissionSet(set);
		}
        
        public override void Configure(IServiceCollection services, System.Configuration.Configuration moduleConfiguration)
        {
            // Cofigure module authorization if needed
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

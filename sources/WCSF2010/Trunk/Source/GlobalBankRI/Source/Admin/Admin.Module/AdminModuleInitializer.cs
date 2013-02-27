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
using System.Web;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Authorization;
using Microsoft.Practices.CompositeWeb.Configuration;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Services;
using GlobalBank.Commercial.EBanking.Modules.Admin.Constants;

namespace GlobalBank.Commercial.EBanking.Modules.Admin
{
	public class AdminModuleInitializer : ModuleInitializer
	{
        private const string AuthorizationSection = "compositeWeb/authorization";

		public override void Load(CompositionContainer moduleContainer)
		{
			base.Load(moduleContainer);

			RegisterSiteMapInformation(moduleContainer.Services.Get<ISiteMapBuilderService>(true));
			RegisterRequiredPermissions(moduleContainer.Services.Get<IPermissionsCatalog>(true));
        }

		protected void RegisterSiteMapInformation(ISiteMapBuilderService siteMapBuilderService)
		{
			// Register the nodes published by this module
            NameValueCollection attributes = new NameValueCollection(1);
            attributes["imageName"] = "admin";
			SiteMapNodeInfo adminNode = new SiteMapNodeInfo("Admin", "~/Admin/Default.aspx", "Administration", "Admin Module", null, attributes, null, null);
			SiteMapNodeInfo permissionNode = new SiteMapNodeInfo("ViewRolePermissions", "~/Admin/ViewRolePermissionsView.aspx", "Permissions", "View Role Permissions");
			siteMapBuilderService.AddNode(adminNode, 1000);
			siteMapBuilderService.AddNode(permissionNode, adminNode, Permissions.AllowAdministerPermissions);
		}

		protected void RegisterRequiredPermissions(IPermissionsCatalog permissionsCatalog)
		{
			// Register the roles allowed to use this module
			Action allowAdministerPermissions = new Action("Allow Display of Role Permissions", Permissions.AllowAdministerPermissions);
			List<Action> actions = new List<Action>();
			actions.Add(allowAdministerPermissions);
			ModuleActionSet set = new ModuleActionSet("Admin", actions);
			permissionsCatalog.RegisterPermissionSet(set);
		}

        public override void Configure(IServiceCollection services, System.Configuration.Configuration moduleConfiguration)
        {
            // Configure module authorization if needed
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

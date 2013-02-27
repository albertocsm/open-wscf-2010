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
using System.Configuration;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Authorization;
using Microsoft.Practices.CompositeWeb.Configuration;
using Microsoft.Practices.CompositeWeb.Interfaces;
using GlobalBank.Commercial.EBanking.Modules.EFT.Constants;
using GlobalBank.Commercial.EBanking.Modules.EFT.ServiceProxies.EntityTranslators;
using GlobalBank.Commercial.EBanking.Modules.EFT.ServiceProxies.Services;
using GlobalBank.Commercial.EBanking.Modules.EFT.Services;

namespace GlobalBank.Commercial.EBanking.Modules.EFT
{
	public class EFTModuleInitializer : ModuleInitializer
	{
		private const string AuthorizationSection = "compositeWeb/authorization";

		public override void Load(CompositionContainer container)
		{
			base.Load(container);

			AddModuleServices(container.Services);
			RegisterTranslators(container.Services.Get<IEntityTranslatorService>(true));
			RegisterRequiredPermissions(container.Services.Get<IPermissionsCatalog>(true));
			RegisterSiteMapInformation(container.Services.Get<ISiteMapBuilderService>(true));
		}

		protected virtual void AddModuleServices(IServiceCollection moduleServices)
		{
			moduleServices.AddNew<AccountServiceAgent, IAccountServiceAgent>();
		}

		protected virtual void RegisterSiteMapInformation(ISiteMapBuilderService siteMapBuilderService)
		{
			NameValueCollection attributes = new NameValueCollection(1);
			attributes["imageName"] = "funds";
			SiteMapNodeInfo moduleRoot =
				new SiteMapNodeInfo("EFT", "~/EFT/Default.aspx", "Funds",
				                    "The Electronic Funds Transfer Module", null, attributes, null, null);
			SiteMapNodeInfo createTransferNode =
				new SiteMapNodeInfo("CreateTransfer", "~/EFT/CreateTransfer.aspx", "Create Transfer",
				                    "Creates a new transfer");

			SiteMapNodeInfo confirmTransfersViewNode =
				new SiteMapNodeInfo("ConfirmTransfersView", "~/EFT/ConfirmTransfersView.aspx",
				                    "Confirm Transfers");
			SiteMapNodeInfo createAccountViewNode =
				new SiteMapNodeInfo("CreateAccountView", "~/EFT/CreateAccountView.aspx", "Create Account");
			SiteMapNodeInfo newTransferViewNode =
				new SiteMapNodeInfo("NewTransferView", "~/EFT/NewTransferView.aspx", "New Transfer");
			SiteMapNodeInfo summaryViewNode =
				new SiteMapNodeInfo("SummaryView", "~/EFT/SummaryView.aspx", "Summary");

			siteMapBuilderService.AddNode(moduleRoot, 1);
			siteMapBuilderService.AddNode(createTransferNode, moduleRoot, "AllowCreateTransfer");
			siteMapBuilderService.AddNode(confirmTransfersViewNode, createTransferNode);
			siteMapBuilderService.AddNode(createAccountViewNode, createTransferNode);
			siteMapBuilderService.AddNode(newTransferViewNode, createTransferNode);
			siteMapBuilderService.AddNode(summaryViewNode, createTransferNode);
		}

		protected virtual void RegisterTranslators(IEntityTranslatorService entityTranslatorService)
		{
			entityTranslatorService.RegisterEntityTranslator(new AccountTranslator());
			entityTranslatorService.RegisterEntityTranslator(new TransferTableEntryTranslator());
			entityTranslatorService.RegisterEntityTranslator(new ProcessTransfersRequestTypeTranslator());
			entityTranslatorService.RegisterEntityTranslator(new ProcessTransferResponseTypeTranslator());
		}

		protected virtual void RegisterRequiredPermissions(IPermissionsCatalog permissionsCatalog)
		{
			Action allowCreateTransfer = new Action("Allow Create Transfers", Permissions.AllowCreateTransfers);
			List<Action> moduleActions = new List<Action>();
			moduleActions.Add(allowCreateTransfer);
			ModuleActionSet set = new ModuleActionSet("Electronic Funds Transfers", moduleActions);
			permissionsCatalog.RegisterPermissionSet(set);
		}

		public override void Configure(IServiceCollection services, Configuration moduleConfiguration)
		{
			// Configure module authorization if needed
			IAuthorizationRulesService authorizationRuleService = services.Get<IAuthorizationRulesService>();
			if (authorizationRuleService != null)
			{
				AuthorizationConfigurationSection authorizationSection =
					moduleConfiguration.GetSection(AuthorizationSection) as AuthorizationConfigurationSection;
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

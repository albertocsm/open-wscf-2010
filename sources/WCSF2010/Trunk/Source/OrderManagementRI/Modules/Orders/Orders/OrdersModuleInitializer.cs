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
using OrdersRepository.Services.Authentication;
using System.Web.Security;
using OrderManagement.Orders.Converters;
using OrdersRepository.Services;
using OrdersRepository.Interfaces.Services;
using OrdersRepository.BusinessEntities;
using Orders.PresentationEntities;

namespace OrderManagement.Orders
{
    public class OrdersModuleInitializer : ModuleInitializer
    {
        private const string AuthorizationSection = "compositeWeb/authorization";

        public override void Load(CompositionContainer container)
        {
            base.Load(container);

            AddGlobalServices(container.Parent.Services);
            AddModuleServices(container.Services);
            RegisterSiteMapInformation(container.Services.Get<ISiteMapBuilderService>(true));

            RepositoryMembershipProvider membershipProvider = Membership.Provider as RepositoryMembershipProvider;
            if (membershipProvider != null)
            {
                membershipProvider.EmployeeService = container.Services.Get<IEmployeeService>();
                membershipProvider.ValidPassword = "P@ssw0rd";
            }

            container.RegisterTypeMapping<IOrdersController, OrdersController>();
        }

        protected virtual void AddGlobalServices(IServiceCollection globalServices)
        {
            // TODO: add a service that will be visible to any module
        }

        protected virtual void AddModuleServices(IServiceCollection moduleServices)
        {
            moduleServices.AddNew<ProductService, IProductService>();
            moduleServices.AddNew<EmployeeService, IEmployeeService>();
            moduleServices.AddNew<OrdersService, IOrdersService>();

            moduleServices.AddNew<EmployeeConverter, IBusinessPresentationConverter<Employee, EmployeeDisplay>>();
            moduleServices.AddNew<OrdersConverter, IBusinessPresentationConverter<Order, OrderInfo>>();
            moduleServices.AddNew<OrderDetailsConverter, IBusinessPresentationConverter<OrderDetail, OrderItemLine>>();

            // This service has been registered in the Orders\Web.config file to provide an example.
			// moduleServices.AddNew<OrderEntryFlowService, IOrderEntryFlowService>();
        }

        protected virtual void RegisterSiteMapInformation(ISiteMapBuilderService siteMapBuilderService)
        {
            SiteMapNodeInfo moduleNode = new SiteMapNodeInfo("Orders", "~/Orders/Default.aspx", "Orders");
            siteMapBuilderService.AddNode(moduleNode, "AllowBrowseOrders");

            SiteMapNodeInfo createNewNode = new SiteMapNodeInfo("CreateNewOrder", "~/Orders/OrderEntry.aspx", "Create New Order");
            siteMapBuilderService.AddNode(createNewNode, moduleNode, "AllowCreateOrders");

            SiteMapNodeInfo savedDraftNode = new SiteMapNodeInfo("MySavedDrafts", "~/Orders/MySavedDrafts.aspx", "My Saved Drafts");
            siteMapBuilderService.AddNode(savedDraftNode, moduleNode, "AllowCreateOrders");

            SiteMapNodeInfo approvalsNode = new SiteMapNodeInfo("MyApprovals", "~/Orders/MyApprovals.aspx", "My Approvals");
            siteMapBuilderService.AddNode(approvalsNode, moduleNode, "AllowApprovals");

            SiteMapNodeInfo searchOrderNode = new SiteMapNodeInfo("SearchOrder", "~/Orders/SearchOrders.aspx", "Search Orders");
            siteMapBuilderService.AddNode(searchOrderNode, moduleNode, "AllowBrowseOrders");
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

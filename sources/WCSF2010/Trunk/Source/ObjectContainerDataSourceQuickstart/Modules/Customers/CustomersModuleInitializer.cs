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
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Interfaces;

namespace ObjectContainerDataSourceQuickstart.Modules.Customers
{
    public class CustomersModuleInitializer : ModuleInitializer
    {
        public override void Load(CompositionContainer moduleContainer)
        {
            base.Load(moduleContainer);

            RegisterSiteMapInformation(moduleContainer.Services.Get<ISiteMapBuilderService>(true));
        }

        protected virtual void RegisterSiteMapInformation(ISiteMapBuilderService siteMapBuilderService)
        {
            SiteMapNodeInfo moduleRootNode = new SiteMapNodeInfo("Customers", null, "Customers");
            SiteMapNodeInfo customersSimpleViewNode = new SiteMapNodeInfo("CustomersSimpleView", "~/Customers/CustomersSimpleView.aspx", "Customers (simple)", "Shows simple usage patterns of the ObjectContainerDataSource control, including select/update/insert/delete operations with sorting and paging provided by the control");
            SiteMapNodeInfo customersAdvancedViewNode = new SiteMapNodeInfo("CustomersAdvancedView", "~/Customers/CustomersAdvancedView.aspx", "Customers (advanced)", "Shows advanced usage patterns of the ObjectContainerDataSource control, including select/update/insert/delete operations and server-side sorting and paging");
            siteMapBuilderService.AddNode(moduleRootNode);
            siteMapBuilderService.AddNode(customersSimpleViewNode, moduleRootNode);
            siteMapBuilderService.AddNode(customersAdvancedViewNode, moduleRootNode);
        }
    }
}

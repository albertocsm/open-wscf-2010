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
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Authorization;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Services;
using GlobalBank.Commercial.EBanking.Modules.Admin;
using GlobalBank.Commercial.EBanking.Modules.Admin.Constants;

namespace GlobalBank.Commercial.EBanking.Modules.Admin.Tests
{
    [TestClass]
    public class AdminModuleInitializerFixture
    {
        [TestMethod]
        public void AdminModuleInitializerImplementsIModuleInitializer()
        {
            Assert.IsTrue(new AdminModuleInitializer() is IModuleInitializer);
        }
        
        [TestMethod]
        public void AdminModuleInitializerGetsRegisteredOnSiteMap()
        {
            TestableModule module = new TestableModule();
			SiteMapBuilderService service = new SiteMapBuilderService();
            
            module.TestRegisterSiteMapInformation(service);

			SiteMapNodeInfo node = service.GetChildren(service.RootNode.Key)[0];
            Assert.AreEqual("Admin", node.Key);
        }

        [TestMethod]
        public void AdminModuleInitializerGetsRegisterOnSiteMapAdministerPermissionsAsChildNode()
        {
			TestableModule module = new TestableModule();
			SiteMapBuilderService service = new SiteMapBuilderService();
			
            module.TestRegisterSiteMapInformation(service);
        	
			SiteMapNodeInfo node = service.GetChildren(service.RootNode.Key)[0];
			
            Assert.AreEqual(1, service.GetChildren(node.Key).Count);
            Assert.AreEqual("AllowAdministerPermissions", service.GetAuthorizationRule(service.GetChildren(node.Key)[0].Key));
        }

        [TestMethod]
        public void AdminModuleInitializerRegistersTheAllowViewRolePermissionActionSet()
        {
            TestableModule module = new TestableModule();
			MockPermissionsCatalog catalog = new MockPermissionsCatalog();
			module.TestRegisterRequiredPermissions(catalog);

            Assert.IsNotNull(catalog.RegisteredPermissions["Admin"]);
            Assert.AreEqual(Permissions.AllowAdministerPermissions, catalog.RegisteredPermissions["Admin"].Actions[0].RuleName);
        }

    }


	class TestableModule : AdminModuleInitializer
	{
		public void TestRegisterSiteMapInformation(ISiteMapBuilderService siteMapBuilderService)
		{
			RegisterSiteMapInformation(siteMapBuilderService);
		}

		public void TestRegisterRequiredPermissions(IPermissionsCatalog permissionsCatalog)
		{
			RegisterRequiredPermissions(permissionsCatalog);
		}
	}

    class MockSiteMapService : StaticSiteMapProvider
    {
        public SiteMapNode Node;

        #region ISiteMapService Members

        public void AddSiteMapNode(SiteMapNode node)
        {
            Node = node;
        }

        public SiteMapNode GetSiteMap()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

		public override SiteMapNode BuildSiteMap()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override SiteMapNode GetRootNodeCore()
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}

    class MockCompositionContainer : CompositionContainer
    {

    }

    class MockPermissionsCatalog : IPermissionsCatalog
    {
        ModuleActionSet _registeredSet = null;
        #region IPermissionsCatalog Members

        public void RegisterPermissionSet(ModuleActionSet set)
        {
            _registeredSet = set;
        }

        public Dictionary<string, ModuleActionSet> RegisteredPermissions
        {
            get
            {
                Dictionary<string, ModuleActionSet> regPermissions = new Dictionary<string, ModuleActionSet>();
                regPermissions[_registeredSet.ModuleName] = _registeredSet;
                return regPermissions;
            }
        }

        public ModuleActionSet RegisteredSet
        {
            get { return _registeredSet; }
            set { _registeredSet = value; }
        }


        #endregion
    }
}

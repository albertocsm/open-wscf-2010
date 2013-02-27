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
using System.Data;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Authorization;
using Microsoft.Practices.CompositeWeb.Services;
using Microsoft.Practices.CompositeWeb.Interfaces;
using GlobalBank.Commercial.EBanking.Modules.Admin;
using GlobalBank.Commercial.EBanking.Modules.Admin.BussinesEntities;

namespace GlobalBank.Commercial.EBanking.Modules.Admin.Tests
{
    [TestClass]
    public class AdminControllerFixture
    {
        [TestMethod]
        public void GetModulePermissionSetReturnsSingleActionAndSingleRolePermission()
        {
            MockRolesCatalog catalog = new MockRolesCatalog();
            MockAuthorizationService authSerivce = new MockAuthorizationService();
            IPermissionsCatalog permissionsService = new MockPermissionCatalog();
            AdminController controller = new AdminController(authSerivce, permissionsService, catalog);

            ModulePermissionSet set = controller.GetModulePermissionSet("TestModule");
            ModulePermission singleModulePermission = set.ModulePermissions[0];

            Assert.AreEqual("TestModule", singleModulePermission.ModuleName);
            Assert.AreEqual("Friendly", singleModulePermission.ActionName);
            Assert.IsTrue(singleModulePermission.Permissions is Dictionary<string, bool>);
            Assert.IsFalse(singleModulePermission.Permissions["Mock01"]);
        }

        [TestMethod]
        public void GetModulePermissionSetWorksForTwoModules()
        {
            MockRolesCatalog catalog = new MockRolesCatalog();
            MockAuthorizationService authSerivce = new MockAuthorizationService();
            IPermissionsCatalog permissionsService = new MockPermissionCatalog();
            AdminController controller = new AdminController(authSerivce, permissionsService, catalog);

            ModulePermissionSet testModuleSet = controller.GetModulePermissionSet("TestModule");
            ModulePermission testModulePermission = testModuleSet.ModulePermissions[0];

            Assert.AreEqual("TestModule", testModulePermission.ModuleName);
            Assert.AreEqual("Friendly", testModulePermission.ActionName);
            Assert.IsTrue(testModulePermission.Permissions is Dictionary<string, bool>);
            Assert.IsFalse(testModulePermission.Permissions["Mock01"]);

            ModulePermissionSet testModule02Set = controller.GetModulePermissionSet("TestModule");
            ModulePermission testModule02Permission = testModule02Set.ModulePermissions[0];

            Assert.AreEqual("TestModule", testModule02Permission.ModuleName);
            Assert.AreEqual("Friendly", testModule02Permission.ActionName);
            Assert.IsTrue(testModule02Permission.Permissions is Dictionary<string, bool>);
            Assert.IsFalse(testModule02Permission.Permissions["Mock01"]);
        }

        [TestMethod]
        public void GetModulePermissionSetWorksForSingleModule()
        {
            MockRolesCatalog catalog = new MockRolesCatalog();
            MockAuthorizationService authSerivce = new MockAuthorizationService();
            IPermissionsCatalog permissionsService = new MockPermissionCatalog();
            AdminController controller = new AdminController(authSerivce, permissionsService, catalog);

            ModulePermissionSet set = controller.GetModulePermissionSet("TestModule02");

            ModulePermission[] permissions = set.ModulePermissions.ToArray();

            Assert.AreEqual("TestModule02", permissions[0].ModuleName);
            Assert.AreEqual("Friendly", permissions[0].ActionName);
            Assert.IsTrue(permissions[0].Permissions is Dictionary<string, bool>);
            Assert.IsFalse(permissions[0].Permissions["Mock01"]);
        }

        [TestMethod]
        public void GetAllModulePermissionSetsReturnsAllExpectedPermissionSets()
        {
            MockRolesCatalog catalog = new MockRolesCatalog();
            MockAuthorizationService authSerivce = new MockAuthorizationService();
            IPermissionsCatalog permissionsService = new MockPermissionCatalog();
            AdminController controller = new AdminController(authSerivce, permissionsService, catalog);

            ModulePermissionSet[] allSets = controller.GetModulePermissionSets();

            Assert.AreEqual(permissionsService.RegisteredPermissions["TestModule"].ModuleName, allSets[0].ModuleName);
            Assert.AreEqual(permissionsService.RegisteredPermissions.Keys.Count, allSets.Length);
        }
    }

    class MockRolesCatalog : IRolesCatalog
    {
        #region IRolesCatalog Members

        public void LoadRoles(string[] roles)
        {

        }

        public IList<string> Roles
        {
            get
            {
                return new List<string>(new string[] { "Mock01" });
            }
        }

        #endregion
    }

    class MockPermissionCatalog : IPermissionsCatalog
    {
        #region IPermissionsCatalog Members

        public void RegisterPermissionSet(ModuleActionSet set)
        {
            throw new System.Exception("The method or operation is not implemented.");
        }

        Dictionary<string, ModuleActionSet> IPermissionsCatalog.RegisteredPermissions
        {
            get
            {
                Dictionary<string, ModuleActionSet> dictionary = new Dictionary<string, ModuleActionSet>();
                List<Action> actions = new List<Action>();
                actions.Add(new Action("Friendly", "Rule"));
                dictionary.Add("TestModule", new ModuleActionSet("TestModule", actions));
                dictionary.Add("TestModule02", new ModuleActionSet("TestModule02", actions));
                return dictionary;
            }
        }

        #endregion
    }

    class MockAuthorizationService : IAuthorizationService
    {

        #region IAuthorizationService Members

        public bool IsAuthorized(string context)
        {
            return false;
        }

        public bool IsAuthorized(string role, string context)
        {
            return false;
        }

        #endregion
    }
}

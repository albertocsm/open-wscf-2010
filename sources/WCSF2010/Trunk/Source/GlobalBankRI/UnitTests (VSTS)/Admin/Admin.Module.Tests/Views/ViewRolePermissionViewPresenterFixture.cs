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
using System.Data;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GlobalBank.Commercial.EBanking.Modules.Admin;
using GlobalBank.Commercial.EBanking.Modules.Admin.BussinesEntities;
using GlobalBank.Commercial.EBanking.Modules.Admin.Views;

namespace GlobalBank.Commercial.EBanking.Modules.Admin.Tests.Views
{
    [TestClass]
    public class ViewRolePermissionViewPresenterFixture
    {
        [TestMethod]
        public void OnViewLoadedCallsControllerGetModulePermissionAndSetsRolePermissionsInView()
        {
            MockAdminController controller = new MockAdminController();
            ModulePermissionSet[] modulePermissionSets = controller.ModulePermissionSets = PermissionHelper.GetNewModulePermissionSet();
            ViewRolePermissionViewPresenter presenter = new ViewRolePermissionViewPresenter(controller);
            MockView view = new MockView();
            presenter.View = view;

            presenter.OnViewLoaded();

            Assert.IsTrue(controller.GetModulePermissionCalled);
            Assert.IsTrue(view.RolePermissionsSet);
            Assert.IsTrue(PermissionHelper.AreEqual(modulePermissionSets, view.RolePermissions));
        }

        [TestMethod]
        public void SecondRowModuleNameIsEmpty()
        {
            MockAdminController controller = new MockAdminController();
            ModulePermissionSet[] modulePermissionSets = controller.ModulePermissionSets = PermissionHelper.GetNewModulePermissionSet();
            ViewRolePermissionViewPresenter presenter = new ViewRolePermissionViewPresenter(controller);
            MockView view = new MockView();
            presenter.View = view;

            presenter.OnViewLoaded();

            Assert.IsTrue(view.RolePermissions[0].Rows[1][0] is DBNull);
        }
    }

    class MockView : IViewRolePermissionsView
    {
        public bool RolePermissionsSet = false;
        private DataTable[] _rolePermissions = null;
        
        #region IViewRolePermissionsView Members

        public DataTable[] RolePermissions
        {
            get { return _rolePermissions; }
            set
            {
                _rolePermissions = value;
                RolePermissionsSet = true;
            }
        }

        #endregion
    }

    class MockAdminController : AdminController
    {
        public bool GetModulePermissionCalled = false;
        public ModulePermissionSet[] ModulePermissionSets = null;

        public MockAdminController() : base(null, null, null)
        {

        }

        public override ModulePermissionSet[] GetModulePermissionSets()
        {
            GetModulePermissionCalled = true;
            return PermissionHelper.GetNewModulePermissionSet();
        }
    }

    static class PermissionHelper
    {
        public static ModulePermissionSet[] GetNewModulePermissionSet()
        {
            Dictionary<string, bool> permissions = new Dictionary<string, bool>();
            permissions["role01"] = true;
            ModulePermissionSet group = new ModulePermissionSet("moduleName", new List<ModulePermission>(new ModulePermission[] { new ModulePermission("moduleName", "rule", permissions), new ModulePermission("moduleName", "rule", permissions) }));
            return new ModulePermissionSet[] { group };
        }

        public static bool AreEqual(ModulePermissionSet[] permissionSets, DataTable[] dataTables)
        {
            if (permissionSets.Length != dataTables.Length)
            {
                return false;
            }

            for (int i = 0; i < permissionSets.Length - 1; i++)
            {
                if (!AreEqual(permissionSets[i], dataTables[1]))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool AreEqual(ModulePermissionSet permissionSet, DataTable dataTable)
        {
            return ((permissionSet.ModulePermissions.Count == dataTable.Rows.Count) &&
                    (permissionSet.ModuleName == dataTable.TableName) &&
                    (permissionSet.ModulePermissions[0].Permissions.Keys.Count + 2 == dataTable.Columns.Count)
                );
        }
    }
}

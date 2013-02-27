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
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Authorization;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Services;
using GlobalBank.Commercial.EBanking.Modules.Admin.BussinesEntities;

namespace GlobalBank.Commercial.EBanking.Modules.Admin
{
    public class AdminController
    {
        private IAuthorizationService _authorizationService;
        private IPermissionsCatalog _permissionsCatalog;
        private IRolesCatalog _rolesCatalog;

        public AdminController
            (
                [ServiceDependency] IAuthorizationService authorizationService,
                [ServiceDependency] IPermissionsCatalog permissionsCatalog,
                [ServiceDependency] IRolesCatalog rolesCatalog
            )
        {
            _authorizationService = authorizationService;
            _permissionsCatalog = permissionsCatalog;
            _rolesCatalog = rolesCatalog;
        }

        public virtual ModulePermissionSet GetModulePermissionSet(string moduleName)
        {
            ModuleActionSet set = _permissionsCatalog.RegisteredPermissions[moduleName];
            List<ModulePermission> modulePermissions = new List<ModulePermission>();

            foreach (Action action in set.Actions)
            {
                Dictionary<string, bool> permissions = new Dictionary<string, bool>();

                foreach (string role in _rolesCatalog.Roles)
                {
                    permissions.Add(role, _authorizationService.IsAuthorized(role, action.RuleName));
                }

                modulePermissions.Add(new ModulePermission(moduleName, action.FriendlyName, permissions));
            }

            return new ModulePermissionSet(moduleName, modulePermissions);
        }

        public virtual ModulePermissionSet[] GetModulePermissionSets()
        {
            List<ModulePermissionSet> groups = new List<ModulePermissionSet>();
            foreach (ModuleActionSet  set in _permissionsCatalog.RegisteredPermissions.Values)
            {
                groups.Add(GetModulePermissionSet(set.ModuleName));
            }
            return groups.ToArray();
        }
    }
}

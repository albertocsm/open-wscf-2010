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
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.ObjectBuilder;
using GlobalBank.Commercial.EBanking.Modules.Admin.BussinesEntities;

namespace GlobalBank.Commercial.EBanking.Modules.Admin.Views
{
    public class ViewRolePermissionViewPresenter : Presenter<IViewRolePermissionsView>
    {
        private AdminController _controller;

        public ViewRolePermissionViewPresenter([CreateNew] AdminController controller)
        {
            _controller = controller;
        }

        public override void OnViewLoaded()
        {
            ModulePermissionSet[] moduleSets = _controller.GetModulePermissionSets();
            View.RolePermissions = TranslateToDataTablesArray(moduleSets);
        }

        private DataTable[] TranslateToDataTablesArray(ModulePermissionSet[] moduleSets)
        {
            List<DataTable> tableList = new List<DataTable>();

            foreach (ModulePermissionSet set in moduleSets)
            {
                tableList.Add(TranslateToTable(set));
            }
            return tableList.ToArray();
        }

        private DataTable TranslateToTable(ModulePermissionSet set)
        {
            DataTable table = new DataTable(set.ModuleName);
            ModulePermission firstOne = set.ModulePermissions[0];

            table.Columns.Add("Module", typeof(string));
            table.Columns.Add("Action", typeof(string));

            foreach (string key in firstOne.Permissions.Keys)
            {
                table.Columns.Add(key, typeof(bool));
            }

            int lineNumber = 0;

            foreach (ModulePermission line in set.ModulePermissions)
            {
                DataRow row = table.Rows.Add();

                if (lineNumber == 0)
                {
                    row[0] = line.ModuleName;
                }

                row["Action"] = line.ActionName;
                foreach (string key in line.Permissions.Keys)
                {
                    row[key] = line.Permissions[key];
                }
                lineNumber++;
            }

            return table;
        }
    }
}

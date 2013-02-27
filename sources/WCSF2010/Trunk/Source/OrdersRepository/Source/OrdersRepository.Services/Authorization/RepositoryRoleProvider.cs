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
using System.Web.Security;
using OrdersRepository.Services.Properties;

namespace OrdersRepository.Services.Authorization
{
    public class RepositoryRoleProvider : RoleProvider
    {
        public const string ApproverPrefix = "a-";

        public override string[] GetRolesForUser(string username)
        {
            List<string> roles = new List<string>();
            roles.Add("User");

            if (username.StartsWith(ApproverPrefix))
            {
                roles.Add("Approver");
            }

            return roles.ToArray();
        }

        #region Not implemented methods
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
            }
            set
            {
                throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
        }
        #endregion
    }
}

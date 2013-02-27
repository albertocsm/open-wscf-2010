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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrdersRepository.Services.Authorization;

namespace OrdersRepository.Services.Tests
{
    [TestClass]
    public class RepositoryRoleProviderFixture
    {
        [TestMethod]
        public void ADashUsersAreApproversAndUsers()
        {
            RepositoryRoleProvider provider = new RepositoryRoleProvider();
            string[] roles = provider.GetRolesForUser("a-user");

            AssertExists(roles, "User");
            AssertExists(roles, "Approver");
        }

        [TestMethod]
        public void NonDashUsersAreUsers()
        {
            RepositoryRoleProvider provider = new RepositoryRoleProvider();

            string[] roles = provider.GetRolesForUser("anotherUser");
			AssertExists(roles, "User");
			AssertNoExists(roles, "Approver");
		}

		static void AssertExists<T>(T[] source, T t)
		{
			Assert.IsTrue(Array.IndexOf(source, t) != -1);
		}

		static void AssertNoExists<T>(T[] source, T t)
		{
			Assert.IsFalse(Array.IndexOf(source, t) != -1);
		}
    }
}

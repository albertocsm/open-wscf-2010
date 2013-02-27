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
using System.Web.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrdersRepository.BusinessEntities;
using OrdersRepository.Services.Authentication;
using OrdersRepository.Services.Tests.Fakes;

namespace OrderManagement.Orders.Tests
{
    [TestClass]
    public class InMemoryMembershipProviderFixture
    {
		[TestMethod]
		public void AutenticateUserWithValidPassword()
		{
			RepositoryMembershipProvider provider = GetProvider();
			provider.ValidPassword = "MyPass";
			provider.EmployeeService = MakeInitializedEmployeeService();

			bool valid = provider.ValidateUser("user42", "MyPass");

			Assert.IsTrue(valid);
		}

		[TestMethod]
		public void AutenticateUserWithOtherCaseWithValidPassword()
		{
			RepositoryMembershipProvider provider = GetProvider();
			provider.ValidPassword = "MyPass";
			provider.EmployeeService = MakeInitializedEmployeeService();

			bool valid = provider.ValidateUser("USER42", "MyPass");

			Assert.IsTrue(valid);
		}

		[TestMethod]
		public void NotAutenticateUserWithInvalidPassword()
		{
			RepositoryMembershipProvider provider = GetProvider();
			provider.ValidPassword = "MyPass";
			provider.EmployeeService = MakeInitializedEmployeeService();

			bool valid = provider.ValidateUser("user42", "AnotherPassword");

			Assert.IsFalse(valid);
		}

		[TestMethod]
		public void NotAutenticateInvalidUser()
		{
			RepositoryMembershipProvider provider = GetProvider();
			provider.ValidPassword = "MyPass";
			provider.EmployeeService = MakeInitializedEmployeeService();

			bool valid = provider.ValidateUser("fooUser2", "whoCares");

			Assert.IsFalse(valid);
		}

		private static RepositoryMembershipProvider GetProvider()
        {
			RepositoryMembershipProvider provider = (RepositoryMembershipProvider)Membership.Providers["RepositoryMembershipProvider"];
            return provider;
        }

		private static FakeEmployeeService MakeInitializedEmployeeService()
		{
			FakeEmployeeService service = new FakeEmployeeService();
			service.Employees.Add(new Employee("user42", "John", "Doe"));
			return service;
		}
    }
}

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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Authorization.Tests
{
	[TestClass]
	public class AuthorizationRulesServiceFixture
	{
		public AuthorizationRulesServiceFixture()
		{
		}

		[TestMethod]
		public void CanRegisterAuthorizationRule()
		{
			TestableAuthorizationRulesService service = new TestableAuthorizationRulesService();

			service.RegisterAuthorizationRule("test1", "test2");

			string fixedKey = service.TestableFixKey("test1");
			Assert.AreEqual(1, service.TestableRulesIndex.Count);
			Assert.AreEqual(1, service.TestableRulesIndex[fixedKey].Count);
			Assert.AreEqual("test2", service.TestableRulesIndex[fixedKey][0]);
		}

		[TestMethod]
		public void CanRegisterDuplicateAuthorizationRule()
		{
			TestableAuthorizationRulesService service = new TestableAuthorizationRulesService();
			service.RegisterAuthorizationRule("test1", "test2");
			service.RegisterAuthorizationRule("test1", "test3");

			string fixedKey = service.TestableFixKey("test1");
			Assert.AreEqual(1, service.TestableRulesIndex.Count);
			Assert.AreEqual(2, service.TestableRulesIndex[fixedKey].Count);
			Assert.AreEqual("test2", service.TestableRulesIndex[fixedKey][0]);
			Assert.AreEqual("test3", service.TestableRulesIndex[fixedKey][1]);
		}


		[TestMethod]
		public void CanGetAuthorizationRules()
		{
			TestableAuthorizationRulesService service = new TestableAuthorizationRulesService();
			service.RegisterAuthorizationRule("test1", "test2");
			service.RegisterAuthorizationRule("test1", "test3");

			string[] rules = service.GetAuthorizationRules("test1");
			Assert.IsNotNull(rules);
			Assert.AreEqual(2, rules.Length);
			Assert.AreEqual("test2", rules[0]);
			Assert.AreEqual("test3", rules[1]);
		}

		[TestMethod]
		public void CaseDoesNotMatterWhenRegisteringAndGettingRules()
		{
			TestableAuthorizationRulesService service = new TestableAuthorizationRulesService();
			service.RegisterAuthorizationRule("test1", "test2");
			service.RegisterAuthorizationRule("TEST1", "test3");

			string[] rules = service.GetAuthorizationRules("test1");
			Assert.IsNotNull(rules);
			Assert.AreEqual(2, rules.Length);
			Assert.AreEqual("test2", rules[0]);
			Assert.AreEqual("test3", rules[1]);
		}

		#region Nested type: TestableAuthorizationRulesService

		public class TestableAuthorizationRulesService : AuthorizationRulesService
		{
			public Dictionary<string, List<string>> TestableRulesIndex
			{
				get { return RulesIndex; }
			}

			public string TestableFixKey(string key)
			{
				return base.FixKey(key);
			}
		}

		#endregion
	}
}
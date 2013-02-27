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
using System.Web;
using Microsoft.Practices.CompositeWeb.Authorization;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests.Authorization
{
	[TestClass]
	public class WebClientAuthorizationModuleFixture
	{
		[TestMethod]
		public void TestUserIsAuthorizedIfNotRuleIsDefinedForUrl()
		{
			MockWebClientApplication mockApplication = new MockWebClientApplication();
			MockAuthorizationRuleCatalogService mockRulesCatalog =
				mockApplication.RootContainer.Services.AddNew<MockAuthorizationRuleCatalogService, IAuthorizationRulesService>();
			MockVirtualPathUtility mockVirtualPathUtility =
				mockApplication.RootContainer.Services.AddNew<MockVirtualPathUtility, IVirtualPathUtilityService>();
			mockVirtualPathUtility.ToAppRelativeResult = "~/files.asx";
			MockHttpContext mockContext = new MockHttpContext();
			mockContext.Request = new HttpRequest("file.aspx", "http://MyWebApp/files.aspx", null);

			TestableWebClientAuthorizationModule module = new TestableWebClientAuthorizationModule();
			module.TestHandleAuthorization(mockApplication, mockContext);

			Assert.AreEqual("~/files.asx", mockRulesCatalog.LastGetRuleUrl);
		}


		[TestMethod]
		public void TestAuthorizationServiceIsAskedForAuthorizationRule()
		{
			MockWebClientApplication mockApplication = new MockWebClientApplication();
			MockAuthorizationService mockAuthorizationService =
				mockApplication.RootContainer.Services.AddNew<MockAuthorizationService, IAuthorizationService>();
			MockAuthorizationRuleCatalogService mockRulesCatalog =
				mockApplication.RootContainer.Services.AddNew<MockAuthorizationRuleCatalogService, IAuthorizationRulesService>();
			MockVirtualPathUtility mockVirtualPathUtility =
				mockApplication.RootContainer.Services.AddNew<MockVirtualPathUtility, IVirtualPathUtilityService>();
			mockVirtualPathUtility.ToAppRelativeResult = "~/files.asx";
			mockRulesCatalog.GetRuleReturnValue = new string[] {"AuthorizationRule"};
			MockHttpContext mockContext = new MockHttpContext();
			mockContext.Request = new HttpRequest("file.aspx", "http://MyWebApp/files.aspx", null);

			TestableWebClientAuthorizationModule module = new TestableWebClientAuthorizationModule();
			module.TestHandleAuthorization(mockApplication, mockContext);

			Assert.AreEqual("~/files.asx", mockRulesCatalog.LastGetRuleUrl);
			Assert.AreEqual("AuthorizationRule", mockAuthorizationService.LastContext);
		}

		[TestMethod]
		[ExpectedException(typeof (HttpException))]
		public void TestWebAuthorizationExceptionThrownIfNotAuthorized()
		{
			MockWebClientApplication mockApplication = new MockWebClientApplication();
			MockAuthorizationService mockAuthorizationService =
				mockApplication.RootContainer.Services.AddNew<MockAuthorizationService, IAuthorizationService>();
			MockVirtualPathUtility mockVirtualPathUtility =
				mockApplication.RootContainer.Services.AddNew<MockVirtualPathUtility, IVirtualPathUtilityService>();
			mockVirtualPathUtility.ToAppRelativeResult = "~/files.asx";
			mockAuthorizationService.ShouldAuthorize = false;
			MockAuthorizationRuleCatalogService mockRulesCatalog =
				mockApplication.RootContainer.Services.AddNew<MockAuthorizationRuleCatalogService, IAuthorizationRulesService>();
			mockRulesCatalog.GetRuleReturnValue = new string[] {"AuthorizationRule"};
			MockHttpContext mockContext = new MockHttpContext();
			mockContext.Request = new HttpRequest("file.aspx", "http://MyWebApp/files.aspx", null);

			TestableWebClientAuthorizationModule module = new TestableWebClientAuthorizationModule();
			module.TestHandleAuthorization(mockApplication, mockContext);
		}

		#region Nested type: MockAuthorizationRuleCatalogService

		private class MockAuthorizationRuleCatalogService : IAuthorizationRulesService
		{
			public string[] GetRuleReturnValue = null;
			public string LastGetRuleUrl = null;

			#region IAuthorizationRulesService Members

			public void RegisterAuthorizationRule(string urlPath, string rule)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public string[] GetAuthorizationRules(string urlPath)
			{
				LastGetRuleUrl = urlPath;
				return GetRuleReturnValue;
			}

			#endregion
		}

		#endregion

		#region Nested type: TestableWebClientAuthorizationModule

		private class TestableWebClientAuthorizationModule : WebClientAuthorizationModule
		{
			public void TestHandleAuthorization(IWebClientApplication application, IHttpContext context)
			{
				base.HandleAuthorization(application.RootContainer, context);
			}
		}

		#endregion
	}
}
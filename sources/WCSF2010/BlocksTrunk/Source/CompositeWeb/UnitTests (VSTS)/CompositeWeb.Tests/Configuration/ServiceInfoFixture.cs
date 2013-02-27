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
using Microsoft.Practices.CompositeWeb.Configuration;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests.Configuration
{
	[TestClass]
	public class ServiceInfoFixture
	{
		[TestMethod]
		public void InitializesCorrectlyWithStringScope()
		{
			ServiceInfo sInfo = new ServiceInfo(typeof (IServiceLoaderService), typeof (ServiceLoaderService), "Global");

			Assert.AreEqual(typeof (IServiceLoaderService), sInfo.RegisterAs);
			Assert.AreEqual(typeof (ServiceLoaderService), sInfo.Type);
			Assert.AreEqual(ServiceScope.Global, sInfo.Scope);
		}

		[TestMethod]
		public void InitializesCorrectlyWithEnumScope()
		{
			ServiceInfo sInfo =
				new ServiceInfo(typeof (IServiceLoaderService), typeof (ServiceLoaderService), ServiceScope.Module);

			Assert.AreEqual(typeof (IServiceLoaderService), sInfo.RegisterAs);
			Assert.AreEqual(typeof (ServiceLoaderService), sInfo.Type);
			Assert.AreEqual(ServiceScope.Module, sInfo.Scope);
		}

		[TestMethod]
		[ExpectedException(typeof (ArgumentException))]
		public void ShouldFailIfScopeNotExists()
		{
			ServiceInfo sInfo = new ServiceInfo(typeof (IServiceLoaderService), typeof (ServiceLoaderService), "Any");
		}
	}
}
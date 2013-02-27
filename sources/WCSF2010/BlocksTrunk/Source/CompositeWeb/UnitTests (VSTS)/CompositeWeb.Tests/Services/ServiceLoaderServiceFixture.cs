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
using Microsoft.Practices.CompositeWeb.Services;
using Microsoft.Practices.CompositeWeb.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests.Services
{
	[TestClass]
	public class ServiceLoaderServiceFixture
	{
		[TestMethod]
		[ExpectedException(typeof (ArgumentNullException))]
		public void NullCompositionContainerThrows()
		{
			ServiceLoaderService serviceLoader = new ServiceLoaderService();
			serviceLoader.Load(null, new ServiceInfo[0]);
		}

		[TestMethod]
		public void RegisteredGlobalServiceInstanceGetsAddedToGlobalServicesCollection()
		{
			CompositionContainer rootContainer = new TestableRootCompositionContainer();
			CompositionContainer childContainer = rootContainer.Containers.AddNew<CompositionContainer>();
			ServiceInfo info = new ServiceInfo(typeof (IMockService), typeof (MockService), ServiceScope.Global);
			ServiceLoaderService serviceLoader = new ServiceLoaderService();

			serviceLoader.Load(childContainer, info);

			Assert.IsTrue(rootContainer.Services.Contains(typeof (IMockService)));
			Assert.AreEqual(typeof (MockService), rootContainer.Services.Get<IMockService>(true).GetType());
		}

		[TestMethod]
		public void RegisteredModuleServiceInstanceGetsAddedToModuleServicesCollection()
		{
			CompositionContainer rootContainer = new TestableRootCompositionContainer();
			CompositionContainer childContainer = rootContainer.Containers.AddNew<CompositionContainer>();
			ServiceInfo info = new ServiceInfo(typeof (IMockService), typeof (MockService), ServiceScope.Module);
			ServiceLoaderService serviceLoader = new ServiceLoaderService();

			serviceLoader.Load(childContainer, info);

			Assert.IsTrue(childContainer.Services.Contains(typeof (IMockService)));
			Assert.AreEqual(typeof (MockService), childContainer.Services.Get<IMockService>(true).GetType());
		}

		#region Nested type: IMockService

		private interface IMockService
		{
		}

		#endregion

		#region Nested type: MockService

		private class MockService : IMockService
		{
		}

		#endregion
	}
}
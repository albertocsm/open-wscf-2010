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
using Microsoft.Practices.CompositeWeb.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests
{
	[TestClass]
	public class ServiceMissingExceptionFixture
	{
		[TestMethod]
		public void TestInitialization()
		{
			ServiceMissingException exception = new ServiceMissingException();

			Assert.IsNotNull(exception as Exception);
		}

		[TestMethod]
		public void TestInitializationWithExceptionMessage()
		{
			string message = "Message";
			ServiceMissingException exception = new ServiceMissingException(message);

			Assert.AreEqual(message, exception.Message);
		}

		[TestMethod]
		public void TestInitializationWithServiceType()
		{
			Type serviceType = typeof (Foo);
			ServiceMissingException exception = new ServiceMissingException(serviceType);

			Assert.IsTrue(exception.Message.Contains(serviceType.ToString()));
		}

		[TestMethod]
		public void TestInitializationWithServiceTypeAndComponent()
		{
			Type serviceType = typeof (Foo);
			Foo2 component = new Foo2();
			ServiceMissingException exception = new ServiceMissingException(serviceType, component);

			Assert.IsTrue(exception.Message.Contains(serviceType.ToString()));
			Assert.IsTrue(exception.Message.Contains(component.ToString()));
		}

		[TestMethod]
		public void TestInitializationWithInnerException()
		{
			Exception innerException = new Exception();
			ServiceMissingException exception = new ServiceMissingException("", innerException);

			Assert.AreEqual(innerException, exception.InnerException);
		}

		[TestMethod]
		public void TestInitializationWithServiceTypeAndComponentAndInnerException()
		{
			Type serviceType = typeof (Foo);
			Foo2 component = new Foo2();
			Exception innerException = new Exception();
			ServiceMissingException exception = new ServiceMissingException(serviceType, component, innerException);

			Assert.IsTrue(exception.Message.Contains(serviceType.ToString()));
			Assert.IsTrue(exception.Message.Contains(component.ToString()));
			Assert.AreEqual(innerException, exception.InnerException);
		}
	}
}
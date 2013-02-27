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

namespace Microsoft.Practices.CompositeWeb.Tests
{
	[TestClass]
	public class ModuleDependencySolverExceptionFixture
	{
		[TestMethod]
		public void TestInitialization()
		{
			ModuleDependencySolverException exception = new ModuleDependencySolverException();

			Assert.IsNotNull(exception as Exception);
		}

		[TestMethod]
		public void TestInitializationWithExceptionMessage()
		{
			string message = "Message";
			ModuleDependencySolverException exception = new ModuleDependencySolverException(message);

			Assert.AreEqual(message, exception.Message);
		}

		[TestMethod]
		public void TestInitializationWithInnerException()
		{
			Exception innerException = new Exception();
			ModuleDependencySolverException exception = new ModuleDependencySolverException("", innerException);

			Assert.AreEqual(innerException, exception.InnerException);
		}

		//protected ModuleDependencySolverException(SerializationInfo info, StreamingContext context)
		//    : base(info, context) { }
	}
}
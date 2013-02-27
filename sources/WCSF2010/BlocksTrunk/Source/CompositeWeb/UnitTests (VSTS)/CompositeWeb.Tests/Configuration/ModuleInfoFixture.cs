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
using Microsoft.Practices.CompositeWeb.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests.Configuration
{
	[TestClass]
	public class ModuleInfoFixture
	{
		[TestMethod]
		public void InitializesCorrectly()
		{
			ModuleInfo mInfo = new ModuleInfo("name", "type", "location");

			Assert.AreEqual("name", mInfo.Name);
			Assert.AreEqual("type", mInfo.AssemblyName);
			Assert.AreEqual("location", mInfo.VirtualPath);
		}

		[TestMethod]
		public void CanChangeNameAndTypeAndLocation()
		{
			ModuleInfo mInfo = new ModuleInfo("name", "type", "location");
			mInfo.Name = "OtherName";
			mInfo.AssemblyName = "OtherType";
			mInfo.VirtualPath = "OtherLocation";

			Assert.AreEqual("OtherName", mInfo.Name);
			Assert.AreEqual("OtherType", mInfo.AssemblyName);
			Assert.AreEqual("OtherLocation", mInfo.VirtualPath);
		}
	}
}
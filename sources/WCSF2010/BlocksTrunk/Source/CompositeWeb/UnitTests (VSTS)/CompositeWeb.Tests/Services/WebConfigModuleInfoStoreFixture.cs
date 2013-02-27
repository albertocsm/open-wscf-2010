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
	/// <summary>
	/// Summary description for WebConfigModuleInfoStoreFixture
	/// </summary>
	[TestClass]
	public class WebConfigModuleInfoStoreFixture
	{
		public WebConfigModuleInfoStoreFixture()
		{
			AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
		}

		[TestMethod]
		[DeploymentItem(@"Services\Support\OneModule\Web.config", @"Services\Support\OneModule")]
		public void ReadsOneModuleWebConfig()
		{
			WebConfigModuleInfoStore store = new WebConfigModuleInfoStore(@"Services\Support\OneModule");

			ModulesConfigurationSection section = store.GetModuleConfigurationSection();

			Assert.AreEqual(1, section.Modules.Count);
			Assert.AreEqual("Module1.Name", section.Modules[0].Name);
			Assert.AreEqual("Module1.AssemblyName", section.Modules[0].AssemblyName);
			Assert.AreEqual("Module1.VirtualPath", section.Modules[0].VirtualPath);
			Assert.AreEqual(1, section.Modules[0].Services.Count);
			Assert.AreEqual(typeof (IFoo), section.Modules[0].Services[0].RegisterAs);
			Assert.AreEqual(typeof (Foo), section.Modules[0].Services[0].Type);
			Assert.AreEqual("Global", section.Modules[0].Services[0].Scope);
		}

		[TestMethod]
		[DeploymentItem(@"Services\Support\TwoModulesWithDependency\Web.config", @"Services\Support\TwoModulesWithDependency")
		]
		public void ReadsTwoModulesWithDependency()
		{
			WebConfigModuleInfoStore store = new WebConfigModuleInfoStore(@"Services\Support\TwoModulesWithDependency");

			ModulesConfigurationSection section = store.GetModuleConfigurationSection();

			Assert.AreEqual(2, section.Modules.Count);
			Assert.AreEqual("Module1.Name", section.Modules[0].Name);
			Assert.AreEqual("Module1.AssemblyName", section.Modules[0].AssemblyName);
			Assert.AreEqual("Module1.VirtualPath", section.Modules[0].VirtualPath);
			Assert.AreEqual("Module2.Name", section.Modules[0].Dependencies[0].Module);
			Assert.AreEqual("Module2.Name", section.Modules[1].Name);
			Assert.AreEqual("Module2.AssemblyName", section.Modules[1].AssemblyName);
			Assert.AreEqual("Module2.VirtualPath", section.Modules[1].VirtualPath);
		}

		[TestMethod]
		[DeploymentItem(@"Services\Support\TwoFiles\Web.config", @"Services\Support\TwoFiles")]
		[DeploymentItem(@"Services\Support\TwoFiles\SubFolder\Web.config", @"Services\Support\TwoFiles\SubFolder")]
		public void ReadsTwoModulesFromTwoFiles()
		{
			WebConfigModuleInfoStore store = new WebConfigModuleInfoStore(@"Services\Support\TwoFiles");

			ModulesConfigurationSection section = store.GetModuleConfigurationSection();

			Assert.AreEqual(2, section.Modules.Count);
			Assert.AreEqual("Module1.Name", section.Modules[0].Name);
			Assert.AreEqual("Module1.AssemblyName", section.Modules[0].AssemblyName);
			Assert.AreEqual("Module1.VirtualPath", section.Modules[0].VirtualPath);
			Assert.AreEqual("Module2.Name", section.Modules[1].Name);
			Assert.AreEqual("Module2.AssemblyName", section.Modules[1].AssemblyName);
			Assert.AreEqual("Module2.VirtualPath", section.Modules[1].VirtualPath);
		}

		[TestMethod]
		[DeploymentItem(@"Services\Support\WebConfigWithRepeatedModules\Web.config",
			@"Services\Support\WebConfigWithRepeatedModules")]
		public void IfModuleIsLoadedDoesNotLoadItAgain()
		{
			WebConfigModuleInfoStore store = new WebConfigModuleInfoStore(@"Services\Support\WebConfigWithRepeatedModules");

			ModulesConfigurationSection section = store.GetModuleConfigurationSection();

			Assert.AreEqual(1, section.Modules.Count);
		}
	}
}
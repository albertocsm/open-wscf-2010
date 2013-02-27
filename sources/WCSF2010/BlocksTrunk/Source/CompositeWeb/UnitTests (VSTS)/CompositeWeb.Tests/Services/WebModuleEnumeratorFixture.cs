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
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.CompositeWeb.Configuration;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Services;
using Microsoft.Practices.CompositeWeb.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests.Services
{
	[TestClass]
	public class WebModuleEnumeratorFixture
	{
		[TestMethod]
		public void EnumeratesOneModule()
		{
			MockModuleInfoStore store = new MockModuleInfoStore();
			store.Modules =
				new ModuleConfigurationElement[]
					{new ModuleConfigurationElement("Module1.Name", "Module1.Assembly", "Module1.VirtualPath")};
			WebModuleEnumerator enumerator = new WebModuleEnumerator(store);

			IModuleInfo[] modules = enumerator.EnumerateModules();

			Assert.AreEqual(1, modules.Length);
			Assert.AreEqual("Module1.Name", modules[0].Name);
			Assert.AreEqual("Module1.Assembly", modules[0].AssemblyName);
			Assert.AreEqual("Module1.VirtualPath", modules[0].VirtualPath);
		}

		[TestMethod]
		public void EnumeratesOneModuleWithoutVirtualPath()
		{
			MockModuleInfoStore store = new MockModuleInfoStore();
			store.Modules =
				new ModuleConfigurationElement[] {new ModuleConfigurationElement("Module1.Name", "Module1.Assembly", null)};
			WebModuleEnumerator enumerator = new WebModuleEnumerator(store);

			IModuleInfo[] modules = enumerator.EnumerateModules();

			Assert.AreEqual(1, modules.Length);
			Assert.AreEqual("Module1.Name", modules[0].Name);
			Assert.AreEqual("Module1.Assembly", modules[0].AssemblyName);
			Assert.AreEqual(null, modules[0].VirtualPath);
		}

		[TestMethod]
		public void Enumerates0Modules()
		{
			WebModuleEnumerator enumerator = new WebModuleEnumerator(new MockModuleInfoStore());

			IModuleInfo[] modules = enumerator.EnumerateModules();

			Assert.AreEqual(0, modules.Length);
		}

		[TestMethod]
		public void EnumeratesThreeModulesWithDependencies()
		{
			MockModuleInfoStore store = new MockModuleInfoStore();
			ModuleConfigurationElement module1 =
				new ModuleConfigurationElement("Module1.Name", "Module1.Assembly", "Module1.VirtualPath");
			module1.Dependencies =
				new ModuleDependencyCollection(
					new ModuleDependencyConfigurationElement[] {new ModuleDependencyConfigurationElement("Module2.Name")});
			ModuleConfigurationElement module2 =
				new ModuleConfigurationElement("Module2.Name", "Module2.Assembly", "Module2.VirtualPath");
			module2.Dependencies =
				new ModuleDependencyCollection(
					new ModuleDependencyConfigurationElement[] {new ModuleDependencyConfigurationElement("Module3.Name")});
			ModuleConfigurationElement module3 =
				new ModuleConfigurationElement("Module3.Name", "Module3.Assembly", "Module3.VirtualPath");
			store.Modules = new ModuleConfigurationElement[] {module3, module2, module1};
			WebModuleEnumerator enumerator = new WebModuleEnumerator(store);

			List<IModuleInfo> modules = new List<IModuleInfo>(enumerator.EnumerateModules());

			Assert.AreEqual(3, modules.Count);
			Assert.IsTrue(modules.Exists(delegate(IModuleInfo module) { return module.Name == "Module1.Name"; }));
			Assert.IsTrue(modules.Exists(delegate(IModuleInfo module) { return module.Name == "Module2.Name"; }));
			Assert.IsTrue(modules.Exists(delegate(IModuleInfo module) { return module.Name == "Module3.Name"; }));
		}

		[TestMethod]
		public void EnumeratesThreeModulesWithoutDependenciesInCorrectOrder()
		{
			MockModuleInfoStore store = new MockModuleInfoStore();
			ModuleConfigurationElement module1 =
				new ModuleConfigurationElement("Module1.Name", "Module1.Assembly", "Module1.VirtualPath");
			ModuleConfigurationElement module2 =
				new ModuleConfigurationElement("Module2.Name", "Module2.Assembly", "Module2.VirtualPath");
			ModuleConfigurationElement module3 =
				new ModuleConfigurationElement("Module3.Name", "Module3.Assembly", "Module3.VirtualPath");
			store.Modules = new ModuleConfigurationElement[] {module3, module2, module1};
			WebModuleEnumerator enumerator = new WebModuleEnumerator(store);

			IModuleInfo[] modules = enumerator.EnumerateModules();

			Assert.AreEqual(3, modules.Length);
			Assert.AreEqual("Module1.Name", modules[0].Name);
			Assert.AreEqual("Module2.Name", modules[1].Name);
			Assert.AreEqual("Module3.Name", modules[2].Name);
		}

		[TestMethod]
		public void EnumeratesTwoModulesWithoutVirtualPath()
		{
			MockModuleInfoStore store = new MockModuleInfoStore();
			ModuleConfigurationElement module1 = new ModuleConfigurationElement("Module1.Name", "Module1.Assembly", null);
			ModuleConfigurationElement module2 = new ModuleConfigurationElement("Module2.Name", "Module2.Assembly", string.Empty);
			store.Modules = new ModuleConfigurationElement[] {module2, module1};
			WebModuleEnumerator enumerator = new WebModuleEnumerator(store);

			IModuleInfo[] modules = enumerator.EnumerateModules();

			Assert.AreEqual(2, modules.Length);
			Assert.AreEqual("Module1.Name", modules[0].Name);
			Assert.AreEqual("Module2.Name", modules[1].Name);
		}

		[TestMethod]
		[ExpectedException(typeof (ConfigurationErrorsException))]
		public void EnumerateThrowsIfDuplicateModuleVirtualPaths()
		{
			MockModuleInfoStore store = new MockModuleInfoStore();
			ModuleConfigurationElement module1 =
				new ModuleConfigurationElement("Module1.Name", "Module1.Assembly", "Module1.VirtualPath");
			ModuleConfigurationElement module2 =
				new ModuleConfigurationElement("Module2.Name", "Module2.Assembly", "Module1.VirtualPath");
			store.Modules = new ModuleConfigurationElement[] {module2, module1};
			WebModuleEnumerator enumerator = new WebModuleEnumerator(store);

			IModuleInfo[] modules = enumerator.EnumerateModules();
		}

		[TestMethod]
		[ExpectedException(typeof (ConfigurationErrorsException))]
		public void EnumerateThrowsIfDuplicateNames()
		{
			MockModuleInfoStore store = new MockModuleInfoStore();
			ModuleConfigurationElement module1 =
				new ModuleConfigurationElement("Module1.Name", "Module1.Assembly", "Module1.VirtualPath");
			ModuleConfigurationElement module2 =
				new ModuleConfigurationElement("Module1.Name", "Module2.Assembly", "Module2.VirtualPath");
			store.Modules = new ModuleConfigurationElement[] {module2, module1};
			WebModuleEnumerator enumerator = new WebModuleEnumerator(store);

			IModuleInfo[] modules = enumerator.EnumerateModules();
		}

		[TestMethod]
		[ExpectedException(typeof (ConfigurationErrorsException))]
		public void EnumerateThrowsIfDuplicateAssemblyNames()
		{
			MockModuleInfoStore store = new MockModuleInfoStore();
			ModuleConfigurationElement module1 =
				new ModuleConfigurationElement("Module1.Name", "Module1.Assembly", "Module1.VirtualPath");
			ModuleConfigurationElement module2 =
				new ModuleConfigurationElement("Module2.Name", "Module1.Assembly", "Module2.VirtualPath");
			store.Modules = new ModuleConfigurationElement[] {module2, module1};
			WebModuleEnumerator enumerator = new WebModuleEnumerator(store);

			IModuleInfo[] modules = enumerator.EnumerateModules();
		}

		[TestMethod]
		public void EnumeratesOneModuleWithServicesForRegistration()
		{
			Type registerAs = typeof (string);
			Type concreteType = typeof (Int32);
			string scope = "Global";
			MockModuleInfoStore store = new MockModuleInfoStore();
			ModuleConfigurationElement module1 =
				new ModuleConfigurationElement("Module1.Name", "Module1.Assembly", "Module1.VirtualPath");
			module1.Services =
				new ServiceConfigurationElementCollection(
					new ServiceConfigurationElement[] {new ServiceConfigurationElement(registerAs, concreteType, scope)});
			store.Modules = new ModuleConfigurationElement[] {module1};
			WebModuleEnumerator enumerator = new WebModuleEnumerator(store);

			IModuleInfo[] modules = enumerator.EnumerateModules();

			Assert.AreEqual(1, modules.Length);
			Assert.IsNotNull(modules[0] as DependantModuleInfo);
			DependantModuleInfo moduleInfo = (DependantModuleInfo) modules[0];
			Assert.IsNotNull(moduleInfo.Services);
			Assert.AreEqual(1, moduleInfo.Services.Length);
			Assert.AreEqual(registerAs, moduleInfo.Services[0].RegisterAs);
			Assert.AreEqual(concreteType, moduleInfo.Services[0].Type);
			Assert.AreEqual(ServiceScope.Global, moduleInfo.Services[0].Scope);
		}
	}
}
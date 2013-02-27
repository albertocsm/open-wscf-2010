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
using Microsoft.Practices.CompositeWeb.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests.Services
{
	/// <summary>
	/// Summary description for ModuleContainerLocatorServiceFixture
	/// </summary>
	[TestClass]
	public class ModuleContainerLocatorServiceFixture
	{
		[TestMethod]
		public void GetContainerReturnsRootContainerIfNoModules()
		{
			TestableRootCompositionContainer rootContainer = new TestableRootCompositionContainer();
			rootContainer.Services.AddNew<MockEnumeratorService, IModuleEnumerator>();
			ModuleContainerLocatorService service = rootContainer.Services.AddNew<ModuleContainerLocatorService>();

			CompositionContainer result = service.GetContainer("~/SubFolder/Page.aspx");
			CompositionContainer result2 = service.GetContainer("~/Page.aspx");

			Assert.AreSame(rootContainer, result);
			Assert.AreSame(result, result2);
		}

		[TestMethod]
		public void GetContainerReturnsContainerOfModuleInSubFolder()
		{
			TestableRootCompositionContainer rootContainer = new TestableRootCompositionContainer();
			CompositionContainer module1Container = rootContainer.Containers.AddNew<CompositionContainer>("Module1");
			rootContainer.Containers.AddNew<CompositionContainer>("Shell");
			MockEnumeratorService enumerator = rootContainer.Services.AddNew<MockEnumeratorService, IModuleEnumerator>();
			enumerator.ModuleInfos = new ModuleInfo[]
				{
					new ModuleInfo("Shell", "Shell.Assembly", "~/"),
					new ModuleInfo("Module1", "Module1.Assembly", "~/Module1")
				};
			ModuleContainerLocatorService service = rootContainer.Services.AddNew<ModuleContainerLocatorService>();

			CompositionContainer result = service.GetContainer("~/Module1/Page.aspx");

			Assert.AreSame(module1Container, result);
		}

		[TestMethod]
		public void GetContainerReturnsContainerOfModuleInSubFolderWithDifferentCase()
		{
			TestableRootCompositionContainer rootContainer = new TestableRootCompositionContainer();
			CompositionContainer module1Container = rootContainer.Containers.AddNew<CompositionContainer>("Module1");
			rootContainer.Containers.AddNew<CompositionContainer>("Shell");
			MockEnumeratorService enumerator = rootContainer.Services.AddNew<MockEnumeratorService, IModuleEnumerator>();
			enumerator.ModuleInfos = new ModuleInfo[]
				{
					new ModuleInfo("Shell", "Shell.Assembly", "~/"),
					new ModuleInfo("Module1", "Module1.Assembly", "~/module1")
				};
			ModuleContainerLocatorService service = rootContainer.Services.AddNew<ModuleContainerLocatorService>();

			CompositionContainer result = service.GetContainer("~/Module1/Page.aspx");

			Assert.AreSame(module1Container, result);
		}

		[TestMethod]
		public void GetContainerReturnsShellContainer()
		{
			TestableRootCompositionContainer rootContainer = new TestableRootCompositionContainer();
			rootContainer.Containers.AddNew<CompositionContainer>("Module1");
			CompositionContainer shellContainer = rootContainer.Containers.AddNew<CompositionContainer>("Shell");
			MockEnumeratorService enumerator = rootContainer.Services.AddNew<MockEnumeratorService, IModuleEnumerator>();
			enumerator.ModuleInfos = new ModuleInfo[]
				{
					new ModuleInfo("Module1", "Module1.Assembly", "~/Module1"),
					new ModuleInfo("Shell", "Shell.Assembly", "~/")
				};
			ModuleContainerLocatorService service = rootContainer.Services.AddNew<ModuleContainerLocatorService>();

			CompositionContainer result = service.GetContainer("~/Page.aspx");

			Assert.AreSame(shellContainer, result);
		}

		[TestMethod]
		public void GetContainerReturnsRootContainerEvenIfThereIsAFoundationalModule()
		{
			TestableRootCompositionContainer rootContainer = new TestableRootCompositionContainer();
			rootContainer.Containers.AddNew<CompositionContainer>("Module1");
			MockEnumeratorService enumerator = rootContainer.Services.AddNew<MockEnumeratorService, IModuleEnumerator>();
			enumerator.ModuleInfos = new ModuleInfo[]
				{
					new ModuleInfo("FoundationalModule1", "FoundationalModule1.Assembly", String.Empty),
					new ModuleInfo("Module1", "Module1.Assembly", "~/Module1")
				};
			ModuleContainerLocatorService service = rootContainer.Services.AddNew<ModuleContainerLocatorService>();

			CompositionContainer result = service.GetContainer("~/Page.aspx");

			Assert.AreSame(rootContainer, result);
		}

		[TestMethod]
		public void GetContainerReturnsContainerOfModuleInSubFolderEvenIfThereIsAFoundationalModule()
		{
			TestableRootCompositionContainer rootContainer = new TestableRootCompositionContainer();
			CompositionContainer module1Container = rootContainer.Containers.AddNew<CompositionContainer>("Module1");
			rootContainer.Containers.AddNew<CompositionContainer>("Shell");
			MockEnumeratorService enumerator = rootContainer.Services.AddNew<MockEnumeratorService, IModuleEnumerator>();
			enumerator.ModuleInfos = new ModuleInfo[]
				{
					new ModuleInfo("Shell", "Shell.Assembly", "~/"),
					new ModuleInfo("FoundationalModule", "FoundationalModule.Assembly", null),
					new ModuleInfo("Module1", "Module1.Assembly", "~/Module1")
				};
			ModuleContainerLocatorService service = rootContainer.Services.AddNew<ModuleContainerLocatorService>();

			CompositionContainer result = service.GetContainer("~/Module1/Page.aspx");

			Assert.AreSame(module1Container, result);
		}

		[TestMethod]
		public void GetContainerReturnsShellContainerEvenIfThereIsAFoundationalModule()
		{
			TestableRootCompositionContainer rootContainer = new TestableRootCompositionContainer();
			rootContainer.Containers.AddNew<CompositionContainer>("Module1");
			CompositionContainer shellContainer = rootContainer.Containers.AddNew<CompositionContainer>("Shell");
			MockEnumeratorService enumerator = rootContainer.Services.AddNew<MockEnumeratorService, IModuleEnumerator>();
			enumerator.ModuleInfos = new ModuleInfo[]
				{
					new ModuleInfo("Shell", "Shell.Assembly", "~/"),
					new ModuleInfo("FoundationalModule", "FoundationalModule.Assembly", null),
					new ModuleInfo("Module1", "Module1.Assembly", "~/Module1")
				};
			ModuleContainerLocatorService service = rootContainer.Services.AddNew<ModuleContainerLocatorService>();

			CompositionContainer result = service.GetContainer("~/Page.aspx");

			Assert.AreSame(shellContainer, result);
		}

		[TestMethod]
		public void GetContainerReturnsRootContainerIfNoModulesFoundWithGivenVirtualPath()
		{
			TestableRootCompositionContainer rootContainer = new TestableRootCompositionContainer();
			rootContainer.Containers.AddNew<CompositionContainer>("Module1");
			MockEnumeratorService enumerator = rootContainer.Services.AddNew<MockEnumeratorService, IModuleEnumerator>();
			enumerator.ModuleInfos = new ModuleInfo[] {new ModuleInfo("Module1", "Module1.Assembly", "~/Module1")};
			ModuleContainerLocatorService service = rootContainer.Services.AddNew<ModuleContainerLocatorService>();

			CompositionContainer result = service.GetContainer("~/Page.aspx");

			Assert.AreSame(rootContainer, result);
		}

		[TestMethod]
		public void GetContainerReturnsModule1ContainerIfModuleIsInSubfolder()
		{
			TestableRootCompositionContainer rootContainer = new TestableRootCompositionContainer();
			CompositionContainer module1Container = rootContainer.Containers.AddNew<CompositionContainer>("Module1");
			rootContainer.Containers.AddNew<CompositionContainer>("Shell");
			MockEnumeratorService enumerator = rootContainer.Services.AddNew<MockEnumeratorService, IModuleEnumerator>();
			enumerator.ModuleInfos = new ModuleInfo[]
				{
					new ModuleInfo("Shell", "Shell.Assembly", "~/"),
					new ModuleInfo("Module1", "Module1.Assembly", "~/Modules/Module1")
				};
			ModuleContainerLocatorService service = rootContainer.Services.AddNew<ModuleContainerLocatorService>();

			CompositionContainer result = service.GetContainer("~/Modules/Module1/Page.aspx");

			Assert.AreSame(module1Container, result);
		}

		[TestMethod]
		public void GetContainerReturnsModule1ContainerIfAPageInSubfolderIsRequested()
		{
			TestableRootCompositionContainer rootContainer = new TestableRootCompositionContainer();
			CompositionContainer module1Container = rootContainer.Containers.AddNew<CompositionContainer>("Module1");
			rootContainer.Containers.AddNew<CompositionContainer>("Shell");
			MockEnumeratorService enumerator = rootContainer.Services.AddNew<MockEnumeratorService, IModuleEnumerator>();
			enumerator.ModuleInfos = new ModuleInfo[]
				{
					new ModuleInfo("Shell", "Shell.Assembly", "~/"),
					new ModuleInfo("Module1", "Module1.Assembly", "~/Module1")
				};
			ModuleContainerLocatorService service = rootContainer.Services.AddNew<ModuleContainerLocatorService>();

			CompositionContainer result = service.GetContainer("~/Module1/Subfolder/Page.aspx");

			Assert.AreSame(module1Container, result);
		}

		[TestMethod]
		public void GetContainerReturnsContainerFromNestedModule()
		{
			TestableRootCompositionContainer rootContainer = new TestableRootCompositionContainer();
			rootContainer.Containers.AddNew<CompositionContainer>("Module1");
			CompositionContainer module2Container = rootContainer.Containers.AddNew<CompositionContainer>("Module2");
			rootContainer.Containers.AddNew<CompositionContainer>("Shell");
			MockEnumeratorService enumerator = rootContainer.Services.AddNew<MockEnumeratorService, IModuleEnumerator>();
			enumerator.ModuleInfos = new ModuleInfo[]
				{
					new ModuleInfo("Shell", "Shell.Assembly", "~/"),
					new ModuleInfo("Module1", "Module1.Assembly", "~/Module1"),
					new ModuleInfo("Module2", "Module2.Assembly", "~/Module1/Module2")
				};
			ModuleContainerLocatorService service = rootContainer.Services.AddNew<ModuleContainerLocatorService>();

			CompositionContainer result = service.GetContainer("~/Module1/Module2/Page.aspx");

			Assert.AreSame(module2Container, result);
		}

		[TestMethod]
		public void GetContainerReturnsShellContainerEvenIfThereAreTwoFoundationalModules()
		{
			TestableRootCompositionContainer rootContainer = new TestableRootCompositionContainer();
			CompositionContainer shellContainer = rootContainer.Containers.AddNew<CompositionContainer>("Shell");
			MockEnumeratorService enumerator = rootContainer.Services.AddNew<MockEnumeratorService, IModuleEnumerator>();
			enumerator.ModuleInfos = new ModuleInfo[]
				{
					new ModuleInfo("Shell", "Shell.Assembly", "~/"),
					new ModuleInfo("Module1", "Module1.Assembly", null),
					new ModuleInfo("Module2", "Module2.Assembly", string.Empty)
				};
			ModuleContainerLocatorService service = rootContainer.Services.AddNew<ModuleContainerLocatorService>();

			CompositionContainer result = service.GetContainer("~/Page.aspx");

			Assert.AreSame(shellContainer, result);
		}

		//[TestMethod]
		//public void GetContainerReturnsModuleContainer()
		//{
		//    TestableRootCompositionContainer rootContainer = new TestableRootCompositionContainer();
		//    CompositionContainer moduleContainer = rootContainer.Containers.AddNew<CompositionContainer>("ModuleName");
		//    MockEnumeratorService enumerator = rootContainer.Services.AddNew<MockEnumeratorService, IModuleEnumerator>();
		//    enumerator.ModuleInfos = new ModuleInfo[] { new ModuleInfo("ModuleName", "Module", "~/ModuleRoot/") };
		//    ModuleContainerLocatorService service = rootContainer.Services.AddNew<ModuleContainerLocatorService>();

		//    ICompositionContainer container = service.GetContainer("http://myApplication/ModuleRoot/page.aspx");

		//    Assert.AreSame(moduleContainer, container);
		//}

		//[TestMethod]
		//[ExpectedException(typeof(ArgumentNullException))]
		//public void ThrowsForNullUrl()
		//{
		//    TestableRootCompositionContainer rootContainer = new TestableRootCompositionContainer();
		//    ModuleContainerLocatorService service = rootContainer.Services.AddNew<ModuleContainerLocatorService>();

		//    service.GetContainer(null);
		//}

		//[TestMethod]
		//[ExpectedException(typeof(ArgumentException))]
		//public void ThrowsForEmptyUrl()
		//{
		//    TestableRootCompositionContainer rootContainer = new TestableRootCompositionContainer();
		//    ModuleContainerLocatorService service = rootContainer.Services.AddNew<ModuleContainerLocatorService>();

		//    service.GetContainer(String.Empty);
		//}

		//[TestMethod]
		//public void ModuleLocatorUsesModuleEnumeratorToMapTheUrlToTheModuleContainer()
		//{
		//    TestableRootCompositionContainer rootContainer = new TestableRootCompositionContainer();
		//    CompositionContainer moduleContainer = rootContainer.Containers.AddNew<CompositionContainer>("Module1");
		//    MockEnumeratorService enumerator = rootContainer.Services.AddNew<MockEnumeratorService, IModuleEnumerator>();
		//    enumerator.ModuleInfos = new ModuleInfo[] { new ModuleInfo("Module1", "Module", "http://myApplication/MyModuleLocation/") };

		//    ModuleContainerLocatorService service = rootContainer.Services.AddNew<ModuleContainerLocatorService>();

		//    ICompositionContainer container = service.GetContainer("http://myApplication/MyModuleLocation/page.aspx");

		//    Assert.AreSame(moduleContainer, container);
		//}

		//[TestMethod]
		//public void GetContainerWorksForSeveralModules()
		//{
		//    TestableRootCompositionContainer rootContainer = new TestableRootCompositionContainer();
		//    CompositionContainer module1Container = rootContainer.Containers.AddNew<CompositionContainer>("Module1Name");
		//    CompositionContainer module2Container = rootContainer.Containers.AddNew<CompositionContainer>("Module2Name");
		//    MockEnumeratorService enumerator = rootContainer.Services.AddNew<MockEnumeratorService, IModuleEnumerator>();
		//    enumerator.ModuleInfos = new ModuleInfo[] { 
		//        new ModuleInfo("Module1Name", "Module", "~/Module1/") , 
		//        new ModuleInfo("Module2Name", "Module", "http://myApplication/SomeModule/") };

		//    ModuleContainerLocatorService service = rootContainer.Services.AddNew<ModuleContainerLocatorService>();

		//    ICompositionContainer container1 = service.GetContainer("http://myApplication/Module1/page.aspx");
		//    ICompositionContainer container2 = service.GetContainer("http://myApplication/SomeModule/page.aspx");

		//    Assert.IsNotNull(container1);
		//    Assert.AreSame(module1Container, container1);
		//    Assert.AreSame(module2Container, container2);

		//}

		//[TestMethod]
		//[ExpectedException(typeof(ServiceMissingException))]
		//public void GetContainerThowsWhenEnumeratorServiceIsMissing()
		//{
		//    TestableRootCompositionContainer rootContainer = new TestableRootCompositionContainer();
		//    ModuleContainerLocatorService service = rootContainer.Services.AddNew<ModuleContainerLocatorService>();

		//    ICompositionContainer container = service.GetContainer("http://myApplication/SomeInexistentModule/page.aspx");

		//}

		//[TestMethod]
		//public void GetContainerReturnsRootContainerWhenRootContainerContainsNoModules()
		//{
		//    TestableRootCompositionContainer rootContainer = new TestableRootCompositionContainer();
		//    CompositionContainer moduleContainer = rootContainer.Containers.AddNew<CompositionContainer>("ModuleName");
		//    MockEnumeratorService enumerator = rootContainer.Services.AddNew<MockEnumeratorService, IModuleEnumerator>();
		//    ModuleContainerLocatorService service = rootContainer.Services.AddNew<ModuleContainerLocatorService>();

		//    ICompositionContainer container = service.GetContainer("http://myApplication/SomeInexistentModule/page.aspx");

		//    Assert.IsNotNull(container);
		//    Assert.AreSame(rootContainer, container);
		//}

		//[TestMethod]
		//public void GetContainerReturnsRootContainerWhenRequestUrlAddressNoModule()
		//{
		//    TestableRootCompositionContainer rootContainer = new TestableRootCompositionContainer();
		//    CompositionContainer moduleContainer = rootContainer.Containers.AddNew<CompositionContainer>("ModuleName");
		//    MockEnumeratorService enumerator = rootContainer.Services.AddNew<MockEnumeratorService, IModuleEnumerator>();
		//    enumerator.ModuleInfos = new ModuleInfo[] {	new ModuleInfo("Module", "Module", "http://application/module/") };
		//    ModuleContainerLocatorService service = rootContainer.Services.AddNew<ModuleContainerLocatorService>();

		//    ICompositionContainer container = service.GetContainer("http://myApplication/SomeInexistentModule/page.aspx");

		//    Assert.IsNotNull(container);
		//    Assert.AreSame(rootContainer, container);
		//}

		#region Nested type: MockEnumeratorService

		private class MockEnumeratorService : IModuleEnumerator
		{
			public ModuleInfo[] ModuleInfos = null;

			#region IModuleEnumerator Members

			public IModuleInfo[] EnumerateModules()
			{
				return ModuleInfos;
			}

			#endregion
		}

		#endregion
	}
}
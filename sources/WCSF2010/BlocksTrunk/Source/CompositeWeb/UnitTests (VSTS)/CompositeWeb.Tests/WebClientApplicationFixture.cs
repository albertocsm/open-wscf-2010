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
using System.Web.UI;
using Microsoft.Practices.CompositeWeb.Authorization;
using Microsoft.Practices.CompositeWeb.Configuration;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Services;
using Microsoft.Practices.CompositeWeb.Tests.Mocks;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests
{
	[TestClass]
	public class WebClientApplicationFixture
	{
		[TestMethod]
		public void TestApplicationBuilderIsCreatedByCreateApplicationBuilder()
		{
			TestableWebClientApplication application = new TestableWebClientApplication();

			application.TestRun();

			Assert.IsTrue(application.BuilderCreated);
			Assert.IsNotNull(application.ApplicationBuilder);
		}

		[TestMethod]
		public void TestPageBuilderIsCreatedByCreatePageBuilder()
		{
			TestableWebClientApplication application = new TestableWebClientApplication();

			application.TestRun();

			Assert.IsTrue(application.PageBuilderCreated);
			Assert.IsNotNull(application.PageBuilder);
		}

		[TestMethod]
		public void TestRootContainerCreatedByCreateRootContainer()
		{
			TestableWebClientApplication application = new TestableWebClientApplication();

			application.TestRun();

			Assert.IsTrue(application.RooContainerCreated);
			Assert.IsNotNull(application.RootContainer);
		}


		[TestMethod]
		public void TestModulesAreLoadedUsingEnumeratorAndLoaderServices()
		{
			TestableWebClientApplication application = new TestableWebClientApplication();

			application.TestRun();
		}

		//[TestMethod]
		//[Ignore] // Aspx pages are no longer supposed to be build up automatically.
		//public void AspxPagesAreBuiltupAndTornDown()
		//{
		//    TestableWebClientApplication application = new TestableWebClientApplication();
		//    MockHttpContext mockContext = new MockHttpContext();
		//    MockHandler mockHandler = new MockHandler();
		//    mockContext.Request = new HttpRequest("page.aspx", "http://application/page.aspx", null);
		//    mockContext.Handler = mockHandler;
		//    TestableRootCompositionContainer container = new TestableRootCompositionContainer();
		//    application.ModuleContainer = container;

		//    application.TestRun();

		//    application.TestPreRequestHandlerExecute(mockContext);
		//    Assert.IsTrue(application.PrePageExecuteCalled);
		//    Assert.IsTrue(mockHandler.OnBuiltUpCalled);
		//    Assert.IsFalse(mockHandler.OnTearingDownCalled);

		//    application.TestPostRequestHandlerExecute(mockContext);
		//    Assert.IsTrue(application.PostPageExecuteCalled);
		//    Assert.IsTrue(mockHandler.OnBuiltUpCalled);
		//    Assert.IsTrue(mockHandler.OnTearingDownCalled);
		//}

		[TestMethod]
		public void ExtensionPointsAreBeingCalled()
		{
			TestableWebClientApplication application = new TestableWebClientApplication();
			MockHttpContext mockContext = new MockHttpContext();
			MockHandler mockHandler = new MockHandler();
			mockContext.Handler = mockHandler;

			application.TestPreRequestHandlerExecute(mockContext);
			Assert.IsTrue(application.PrePageExecuteCalled);

			application.TestPostRequestHandlerExecute(mockContext);
			Assert.IsTrue(application.PostPageExecuteCalled);
		}

		[TestMethod]
		public void AspxPagesAreNotAddedToLifetimeContainer()
		{
			TestableWebClientApplication application = new TestableWebClientApplication();
			MockHttpContext mockContext = new MockHttpContext();
			MockPage mockPage = new MockPage();
			mockContext.Request = new HttpRequest("page.aspx", "http://application/page.aspx", null);
			mockContext.Handler = mockPage;
			mockContext.ApplicationInstance = application;
			TestableRootCompositionContainer container = new TestableRootCompositionContainer();
			application.ModuleContainer = container;
			application.SetTestCurrentContext(mockContext);

			application.TestRun();

			int lifetimeObjects = container.Locator.Count;
			//application.TestPreRequestHandlerExecute(mockContext);
			mockPage.CallOnInit();

			Assert.IsNotNull(mockPage.Presenter);
			Assert.IsNull(container.Locator.Get<MockPresenter>());
			Assert.IsNull(container.Locator.Get<MockPage>());

			//application.TestPostRequestHandlerExecute(mockContext);

			Assert.AreEqual(lifetimeObjects, container.Locator.Count);
		}


		[TestMethod]
		public void NestedUserControlsAreBuiltCorrectly()
		{
			TestableWebClientApplication application = new TestableWebClientApplication();
			MockHttpContext mockContext = new MockHttpContext();
			MockPage mockPage = new MockPage();
			mockContext.Request = new HttpRequest("page.aspx", "http://application/page.aspx", null);
			mockContext.Handler = mockPage;
			mockContext.ApplicationInstance = application;
			TestableRootCompositionContainer container = new TestableRootCompositionContainer();
			application.ModuleContainer = container;
			application.SetTestCurrentContext(mockContext);

			application.TestRun();
			mockPage.CallOnInit();
			mockPage.MyControl.CallOnInit();
			mockPage.MyControl.Nested.CallOnInit();

			Assert.IsNotNull(mockPage.Presenter);
			Assert.IsNotNull(mockPage.MyControl.MyControlPresenter);
			Assert.IsNotNull(mockPage.MyControl.Nested.MyControlPresenter);
		}

		[TestMethod]
		public void WebApplicationCreatesBuilderThatDoesContainerAwareTypeMapping()
		{
			TestableWebClientApplication app = new TestableWebClientApplication();
			MockHttpContext context = new MockHttpContext();
			app.TestRun();

			CompositionContainer root = app.RootContainer;
			root.RegisterTypeMapping<IFoo, Foo>();

			IFoo created = (IFoo) (root.BuildNewItem(typeof (IFoo)));
			Assert.IsNotNull(created);
			Assert.IsTrue(created is Foo);
		}

		[TestMethod]
		public void AddServiceOnlyIfMissing()
		{
			TestableWebClientApplication app = new TestableWebClientApplication();
			TestableRootCompositionContainer container = new TestableRootCompositionContainer();

			Foo foo = app.TestAddServiceIfMissing<Foo, IFoo>(container);
			Assert.AreSame(foo, container.Services.Get<IFoo>());

			Foo foo2 = app.TestAddServiceIfMissing<Foo, IFoo>(container);
			Assert.AreNotSame(foo2, container.Services.Get<IFoo>());
			Assert.AreSame(foo, container.Services.Get<IFoo>());
		}

		[TestMethod]
		public void LoadModulesCallsModuleLoaderService()
		{
			TestableWebClientApplication app = new TestableWebClientApplication();
			app.SetTestRootCompositionContainer(new TestableRootCompositionContainer());

			IModuleLoaderService loaderService =
				app.RootContainer.Services.AddNew<MockModuleLoaderService, IModuleLoaderService>();
			app.RootContainer.Services.AddNew<MockModuleEnumerator, IModuleEnumerator>();

			app.TestLoadModules();

			Assert.IsTrue(((MockModuleLoaderService) loaderService).LoadCalled);
		}


		[TestMethod]
		public void ConfigureModulesUsesModuleEnumeratorAndModuleLoaderService()
		{
			TestableWebClientApplication app = new TestableWebClientApplication();
			TestableRootCompositionContainer container = new TestableRootCompositionContainer();
			app.SetTestRootCompositionContainer(container);
			MockModuleEnumerator moduleEnumerator = new MockModuleEnumerator();

			ModuleInfo moduleInfo = new ModuleInfo("TestModuleName", string.Empty, string.Empty);
			moduleEnumerator.ModulesData.Add(moduleInfo);
			container.Services.Add(typeof (IModuleEnumerator), moduleEnumerator);

			MockModuleLoaderService moduleLoader = new MockModuleLoaderService();
			container.Services.Add(typeof (IModuleLoaderService), moduleLoader);

			MockModuleConfigurationLocatorService configurationLocator = new MockModuleConfigurationLocatorService();
			container.Services.Add(typeof (IModuleConfigurationLocatorService), configurationLocator);

			app.TestConfigureModules();

			Assert.AreEqual("TestModuleName", configurationLocator.ModuleName);
			Assert.AreEqual("TestModuleName", moduleLoader.ModuleName);
		}

		[TestMethod]
		public void AddRequiredServicesAddsModuleConfigurationLocatorService()
		{
			TestableWebClientApplication app = new TestableWebClientApplication();
			TestableRootCompositionContainer container = new TestableRootCompositionContainer();
			app.SetTestRootCompositionContainer(container);

			app.TestAddRequiredServices();

			container.Services.Contains<ModuleConfigurationLocatorService>();
		}

		[TestMethod]
		public void AddRequiredServicesAddsVirtualPathUtilityService()
		{
			TestableWebClientApplication app = new TestableWebClientApplication();
			TestableRootCompositionContainer container = new TestableRootCompositionContainer();
			app.SetTestRootCompositionContainer(container);

			app.TestAddRequiredServices();

			container.Services.Contains<VirtualPathUtilityService>();
		}

		[TestMethod]
		public void AddRequiredServicesAddsAuthorizationRulesService()
		{
			TestableWebClientApplication app = new TestableWebClientApplication();
			TestableRootCompositionContainer container = new TestableRootCompositionContainer();
			app.SetTestRootCompositionContainer(container);

			app.TestAddRequiredServices();

			container.Services.Contains<AuthorizationRulesService>();
		}

		[TestMethod]
		public void AddRequiredServicesAddsSessionStateLocatorService()
		{
			TestableWebClientApplication app = new TestableWebClientApplication();
			TestableRootCompositionContainer container = new TestableRootCompositionContainer();
			app.SetTestRootCompositionContainer(container);

			app.TestAddRequiredServices();

			container.Services.Contains<SessionStateLocatorService>();
		}

		[TestMethod]
		public void AddRequiredServicesAddsHttpContextLocatorService()
		{
			TestableWebClientApplication app = new TestableWebClientApplication();
			TestableRootCompositionContainer container = new TestableRootCompositionContainer();
			app.SetTestRootCompositionContainer(container);

			app.TestAddRequiredServices();

			container.Services.Contains<HttpContextLocatorService>();
		}

		[TestMethod]
		public void AddRequiredServicesAddsModuleLoaderService()
		{
			TestableWebClientApplication app = new TestableWebClientApplication();
			TestableRootCompositionContainer container = new TestableRootCompositionContainer();
			app.SetTestRootCompositionContainer(container);

			app.TestAddRequiredServices();

			container.Services.Contains<ModuleLoaderService>();
		}

		[TestMethod]
		public void AddRequiredServicesAddsWebConfigModuleInfoStore()
		{
			TestableWebClientApplication app = new TestableWebClientApplication();
			TestableRootCompositionContainer container = new TestableRootCompositionContainer();
			app.SetTestRootCompositionContainer(container);

			app.TestAddRequiredServices();

			container.Services.Contains<WebConfigModuleInfoStore>();
		}

		[TestMethod]
		public void AddRequiredServicesAddsWebModuleEnumerator()
		{
			TestableWebClientApplication app = new TestableWebClientApplication();
			TestableRootCompositionContainer container = new TestableRootCompositionContainer();
			app.SetTestRootCompositionContainer(container);

			app.TestAddRequiredServices();

			container.Services.Contains<WebModuleEnumerator>();
		}

		[TestMethod]
		public void AddRequiredServicesAddsModuleContainerLocatorService()
		{
			TestableWebClientApplication app = new TestableWebClientApplication();
			TestableRootCompositionContainer container = new TestableRootCompositionContainer();
			app.SetTestRootCompositionContainer(container);

			app.TestAddRequiredServices();

			container.Services.Contains<ModuleContainerLocatorService>();
		}

		[TestMethod]
		public void CreateRootContainerInitializesContainer()
		{
			TestableWebClientApplication app = new TestableWebClientApplication();
			app.InitializeApplicationBuilder();

			app.TestCreateRootContainer();

			Assert.IsNotNull(app.RootContainer);
			Assert.IsNotNull(app.RootContainer.Builder);
			Assert.IsNotNull(app.RootContainer.Locator);
		}

		#region Nested type: ControlPresenter

		private class ControlPresenter
		{
		}

		#endregion

		#region Nested type: MockHandler

		private class MockHandler : Page, IBuilderAware
		{
			public bool OnBuiltUpCalled = false;
			public bool OnTearingDownCalled = false;

			#region IBuilderAware Members

			public void OnBuiltUp(string id)
			{
				OnBuiltUpCalled = true;
			}

			public void OnTearingDown()
			{
				OnTearingDownCalled = true;
			}

			#endregion
		}

		#endregion

		#region Nested type: MockModuleLoaderService

		private class MockModuleLoaderService : IModuleLoaderService
		{
			public bool LoadCalled;
			public string ModuleName;

			#region IModuleLoaderService Members

			public void Load(CompositionContainer rootContainer, IModuleInfo[] moduleInfo)
			{
				LoadCalled = true;
			}

			public IModuleInitializer FindInitializer(string moduleName)
			{
				ModuleName = moduleName;
				return new TestableModuleInitializer();
			}

			#endregion
		}

		#endregion

		#region Nested type: MockPage

		private class MockPage : Page
		{
			private MyUserControl _myControl = new MyUserControl();
			private MockPresenter _presenter = null;

			[CreateNew]
			public MockPresenter Presenter
			{
				get { return _presenter; }
				set { _presenter = value; }
			}

			public MyUserControl MyControl
			{
				get { return _myControl; }
				set { _myControl = value; }
			}

			protected override void OnInit(EventArgs e)
			{
				WebClientApplication.BuildItemWithCurrentContext(this);
				base.OnInit(e);
			}

			public void CallOnInit()
			{
				OnInit(new EventArgs());
			}
		}

		#endregion

		#region Nested type: MockPresenter

		private class MockPresenter
		{
		}

		#endregion

		#region Nested type: MyUserControl

		private class MyUserControl : UserControl
		{
			private ControlPresenter _controlPresenter = null;
			private NestedControl nested = new NestedControl();

			public MyUserControl()
			{
				Controls.Add(nested);
			}

			[CreateNew]
			public ControlPresenter MyControlPresenter
			{
				get { return _controlPresenter; }
				set { _controlPresenter = value; }
			}

			public NestedControl Nested
			{
				get { return nested; }
				set { nested = value; }
			}

			protected override void OnInit(EventArgs e)
			{
				WebClientApplication.BuildItemWithCurrentContext(this);
				base.OnInit(e);
			}

			public void CallOnInit()
			{
				OnInit(new EventArgs());
			}
		}

		#endregion

		#region Nested type: NestedControl

		private class NestedControl : UserControl
		{
			private ControlPresenter _controlPresenter = null;

			[CreateNew]
			public ControlPresenter MyControlPresenter
			{
				get { return _controlPresenter; }
				set { _controlPresenter = value; }
			}

			protected override void OnInit(EventArgs e)
			{
				WebClientApplication.BuildItemWithCurrentContext(this);
				base.OnInit(e);
			}

			public void CallOnInit()
			{
				OnInit(new EventArgs());
			}
		}

		#endregion

		#region Nested type: TestableModuleInitializer

		private class TestableModuleInitializer : ModuleInitializer
		{
		}

		#endregion

		#region Nested type: TestableWebClientApplication

		private class TestableWebClientApplication : WebClientApplication
		{
			private CompositionContainer _root;
			public bool BuilderCreated = false;
			public CompositionContainer ModuleContainer = null;
			public bool PageBuilderCreated = false;
			public bool PostPageExecuteCalled = false;
			public bool PrePageExecuteCalled = false;
			public bool RooContainerCreated = false;

			public override CompositionContainer RootContainer
			{
				get { return _root; }
				protected set { _root = value; }
			}

			public void TestPreRequestHandlerExecute(IHttpContext context)
			{
				InnerPreRequestHandlerExecute(context);
			}

			public void TestPostRequestHandlerExecute(IHttpContext context)
			{
				InnerPostRequestHandlerExecute(context);
			}

			protected override void PrePageExecute(Page page)
			{
				PrePageExecuteCalled = true;
				base.PrePageExecute(page);
			}

			protected override void PostPageExecute(Page page)
			{
				PostPageExecuteCalled = true;
				base.PostPageExecute(page);
			}

			protected override CompositionContainer GetModuleContainer(IHttpContext context)
			{
				return ModuleContainer;
			}

			public void SetTestRootCompositionContainer(CompositionContainer container)
			{
				_root = container;
			}

			internal void TestRun()
			{
				base.Application_Start(null, EventArgs.Empty);
			}

			internal TService TestAddServiceIfMissing<TService, TRegisterAs>(CompositionContainer container)
				where TService : TRegisterAs
			{
				return AddServiceIfMissing<TService, TRegisterAs>(container);
			}

			protected override void AddRequiredServices()
			{
				// No Op
			}

			protected override void LoadModules()
			{
				// No Op
			}


			protected override void CreateApplicationBuilder()
			{
				base.CreateApplicationBuilder();

				BuilderCreated = ApplicationBuilder != null;
			}

			protected override void CreatePageBuilder()
			{
				base.CreatePageBuilder();
				PageBuilderCreated = PageBuilder != null;
			}

			protected override void CreateRootContainer()
			{
				//base.CreateRootContainer();
				_root = new TestableRootCompositionContainer(ApplicationBuilder);
				RooContainerCreated = RootContainer != null;
			}

			public void SetTestCurrentContext(IHttpContext context)
			{
				CurrentContext = context;
			}

			public void TestLoadModules()
			{
				base.LoadModules();
			}

			public void TestConfigureModules()
			{
				base.ConfigureModules();
			}

			public void TestAddRequiredServices()
			{
				base.AddRequiredServices();
			}

			public void TestCreateRootContainer()
			{
				base.CreateRootContainer();
			}

			public void InitializeApplicationBuilder()
			{
				CreateApplicationBuilder();
			}
		}

		#endregion
	}
}
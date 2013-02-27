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
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.ObjectBuilder;
using Microsoft.Practices.CompositeWeb.Tests.Mocks;
using Microsoft.Practices.CompositeWeb.Web.UI;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests.Web.UI
{
	/// <summary>
	/// Summary description for PageFixture
	/// </summary>
	[TestClass]
	public class BaseClassesFixture
	{
		[TestMethod]
		public void PageIsBuiltUpOnPreInit()
		{
			MockWebClientApplication application = new MockWebClientApplication();
			MockHttpContext mockContext = new MockHttpContext();
			mockContext.Request = new HttpRequest("page.aspx", "http://application/page.aspx", null);
			mockContext.ApplicationInstance = application;
			application.SetTestCurrentContext(mockContext);
			MockPage page = new MockPage();

			Assert.IsFalse(page.OnBuiltUpCalled);

			page.FireOnPreInit();

			Assert.IsTrue(page.OnBuiltUpCalled);
		}

		[TestMethod]
		public void UserControlIsBuiltUpOnInit()
		{
			MockWebClientApplication application = new MockWebClientApplication();
			MockHttpContext mockContext = new MockHttpContext();
			mockContext.Request = new HttpRequest("page.aspx", "http://application/page.aspx", null);
			mockContext.ApplicationInstance = application;
			application.SetTestCurrentContext(mockContext);
			System.Web.UI.Page page = new System.Web.UI.Page();
			MockUserControl uc = (MockUserControl) page.LoadControl(typeof (MockUserControl), null);
			page.Controls.Add(uc);

			Assert.IsFalse(uc.OnBuiltUpCalled);

			uc.FireOnInit();

			Assert.IsTrue(uc.OnBuiltUpCalled);
		}

		[TestMethod]
		public void MasterPageIsBuiltUpOnInit()
		{
			MockWebClientApplication application = new MockWebClientApplication();
			MockHttpContext mockContext = new MockHttpContext();
			mockContext.Request = new HttpRequest("page.aspx", "http://application/page.aspx", null);
			mockContext.ApplicationInstance = application;
			application.SetTestCurrentContext(mockContext);
			MockMasterPage master = new MockMasterPage();

			Assert.IsFalse(master.OnBuiltUpCalled);

			master.FireOnInit();

			Assert.IsTrue(master.OnBuiltUpCalled);
		}
	}


	internal class MockPage : Page, IBuilderAware
	{
		public bool OnBuiltUpCalled;

		#region IBuilderAware Members

		public void OnBuiltUp(string id)
		{
			OnBuiltUpCalled = true;
		}

		public void OnTearingDown()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion

		internal void FireOnPreInit()
		{
			OnPreInit(EventArgs.Empty);
		}
	}

	internal class MockUserControl : UserControl, IBuilderAware
	{
		public bool OnBuiltUpCalled;

		#region IBuilderAware Members

		public void OnBuiltUp(string id)
		{
			OnBuiltUpCalled = true;
		}

		public void OnTearingDown()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion

		internal void FireOnInit()
		{
			OnInit(EventArgs.Empty);
		}
	}

	internal class MockMasterPage : MasterPage, IBuilderAware
	{
		public bool OnBuiltUpCalled;

		#region IBuilderAware Members

		public void OnBuiltUp(string id)
		{
			OnBuiltUpCalled = true;
		}

		public void OnTearingDown()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion

		internal void FireOnInit()
		{
			OnInit(EventArgs.Empty);
		}
	}

	internal class MockWebClientApplication : WebClientApplication
	{
		public override IBuilder<WCSFBuilderStage> PageBuilder
		{
			get { return new WCSFBuilder(); }
		}

		public void SetTestCurrentContext(IHttpContext context)
		{
			CurrentContext = context;
		}

		protected override CompositionContainer GetModuleContainer(IHttpContext context)
		{
			return new TestableRootCompositionContainer();
		}
	}
}
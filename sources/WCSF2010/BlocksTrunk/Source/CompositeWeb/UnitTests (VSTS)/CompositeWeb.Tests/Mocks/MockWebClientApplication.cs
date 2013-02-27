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
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.CompositeWeb.Tests.Mocks
{
	public class MockWebClientApplication : IWebClientApplication
	{
		private TestableRootCompositionContainer _rootContainer;
		public bool GetBuilderCalled = false;
		public bool GetRootContainerCalled = false;

		public MockWebClientApplication()
		{
			_rootContainer = new TestableRootCompositionContainer();
		}

		#region IWebClientApplication Members

		public IBuilder<WCSFBuilderStage> ApplicationBuilder
		{
			get
			{
				GetBuilderCalled = true;
				return RootContainer.Builder;
			}
		}

		public IBuilder<WCSFBuilderStage> PageBuilder
		{
			get { throw new NotImplementedException(); }
		}

		public CompositionContainer RootContainer
		{
			get
			{
				GetRootContainerCalled = true;
				return _rootContainer;
			}
		}

		#endregion
	}
}
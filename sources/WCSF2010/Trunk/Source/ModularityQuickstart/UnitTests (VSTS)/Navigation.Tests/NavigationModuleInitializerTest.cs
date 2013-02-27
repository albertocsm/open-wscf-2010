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
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using ModularityQuickstart.Navigation.Tests.Mocks;
using Microsoft.Practices.CompositeWeb.Services;
using ModularityQuickstart.Navigation.Services;
using Microsoft.Practices.CompositeWeb.ObjectBuilder;

namespace ModularityQuickstart.Navigation.Tests
{
    [TestClass]
    public class NavigationModuleInitializerTest
    {
        [TestMethod]
        public void LoadRegistersRedirectNavigationServiceAsINavigationService()
        {
            MockRootCompositionContainer rootContainer = new MockRootCompositionContainer();
            NavigationModuleInitializer moduleInitializer = new NavigationModuleInitializer();

            moduleInitializer.Load(rootContainer);

            INavigationService navigationService = rootContainer.Services.Get<INavigationService>(true);
            Assert.IsTrue(navigationService is RedirectNavigationService);
        }
    }

    namespace Mocks
    {
        class MockRootCompositionContainer : CompositionContainer
        {
            public MockRootCompositionContainer()
            {
                InitializeRootContainer(CreateBuilder());
            }

            private WCSFBuilder CreateBuilder()
            {
                WCSFBuilder builder = new WCSFBuilder();
                builder.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));
                return builder;
            }
        }
    }
}

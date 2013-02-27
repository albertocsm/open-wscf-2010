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
using GlobalBank.Commercial.EBanking.Modules.EFT.Views;
using GlobalBank.Commercial.EBanking.Modules.EFT.Tests.Mocks;

namespace GlobalBank.Commercial.EBanking.Modules.EFT.Tests.Views
{
    /// <summary>
    /// Summary description for DefaultViewPresenterFixture
    /// </summary>
    [TestClass]
    public class CreateTransferViewPresenterFixture
    {
        [TestMethod]
        public void OnViewLoadedCallsControllerTransferFounds()
        {
            MockEFTController controller = new MockEFTController();
			CreateTransferViewPresenter presenter = new CreateTransferViewPresenter(controller);

            presenter.OnViewLoaded();

            Assert.IsTrue(controller.TransferFundsCalled);
        }
    }
}

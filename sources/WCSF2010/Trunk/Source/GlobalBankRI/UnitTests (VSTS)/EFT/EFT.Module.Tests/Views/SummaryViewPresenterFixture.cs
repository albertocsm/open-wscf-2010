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
using GlobalBank.Commercial.EBanking.Modules.EFT.BusinessEntities;

namespace GlobalBank.Commercial.EBanking.Modules.EFT.Tests.Views
{
    /// <summary>
    /// Summary description for SummaryViewPresenterFixture
    /// </summary>
    [TestClass]
    public class SummaryViewPresenterFixture
    {
        [TestMethod]
        public void OnViewInitializedCallsControllerGetTransfersAndSetsTransfersInView()
        {
            MockEFTController controller = new MockEFTController();
            Transfer transfer = new Transfer();
            controller.Transfers = new Transfer[] { transfer };
            SummaryViewPresenter presenter = new SummaryViewPresenter(controller);
            MockSummaryView view = new MockSummaryView();
            presenter.View = view;

            presenter.OnViewInitialized();

            Assert.IsTrue(controller.GetTransfersCalled);
            Assert.IsTrue(view.TransfersSet);
            Assert.AreEqual(1, view.Transfers.Length);
            Assert.AreSame(transfer, view.Transfers[0]);
        }

        [TestMethod]
        public void OnViewInitializedCallsControllerFundsTransferComplete()
        {
            MockEFTController controller = new MockEFTController();
            SummaryViewPresenter presenter = new SummaryViewPresenter(controller);
            MockSummaryView view = new MockSummaryView();
            presenter.View = view;

            presenter.OnViewInitialized();

            Assert.IsTrue(controller.FundsTransferCompleteCalled);
        }
    }

    class MockSummaryView : ISummaryView
    {
        public bool TransfersSet = false;
        private Transfer[] _transfers = null;

        #region ISummaryView Members

        public Transfer[] Transfers
        {
            get { return _transfers; }
            set
            {
                TransfersSet = true;
                _transfers = value;
            }
        }

        #endregion
    }
}

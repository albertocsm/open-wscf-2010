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
    /// Summary description for ConfirmTransfersViewPresenterFixture
    /// </summary>
    [TestClass]
    public class ConfirmTransfersViewPresenterFixture
    {
        [TestMethod]
        public void OnViewLoadedCallsControllerGetTransfersAndSetsTransfersInView()
        {
            MockEFTController controller = new MockEFTController();
            Transfer transfer = GetNewTransfer();
            controller.Transfers = new Transfer[] { transfer };
            ConfirmTransfersViewPresenter presenter = new ConfirmTransfersViewPresenter(controller);
            MockConfirmTransfersView view = new MockConfirmTransfersView();
            presenter.View = view;

            presenter.OnViewLoaded();

            Assert.IsTrue(controller.GetTransfersCalled);
            Assert.IsTrue(view.TransfersSet);
            Assert.AreSame(transfer, view.Transfers[0]);
            Assert.AreEqual(1, view.Transfers.Length);
        }

        [TestMethod]
        public void SubmitTransfersCallsControllerSubmitTransfers()
        {
            MockEFTController controller = new MockEFTController();
            ConfirmTransfersViewPresenter presenter = new ConfirmTransfersViewPresenter(controller);

            presenter.OnSubmit();

            Assert.IsTrue(controller.SubmitTransfersCalled);
        }

        [TestMethod]
        public void ChangeTransfersCallsControllerChangeTransfers()
        {
            MockEFTController controller = new MockEFTController();
            ConfirmTransfersViewPresenter presenter = new ConfirmTransfersViewPresenter(controller);

            presenter.OnPrevious();

            Assert.IsTrue(controller.ChangeTransfersCalled);
        }

        private Transfer GetNewTransfer()
        {
            return new Transfer(Guid.NewGuid());
        }
    }

    class MockConfirmTransfersView : IConfirmTransfersView
    {
        public bool TransfersSet = false;
        private Transfer[] _transfers = null;

        #region IConfirmTransfersView Members

        public Transfer[] Transfers
        {
            set
            {
                TransfersSet = true;
                _transfers = value;
            }
            get { return _transfers; }
        }

        #endregion
    }
}

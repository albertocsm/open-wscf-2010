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
using GlobalBank.Commercial.EBanking.Modules.EFT.Exceptions;

namespace GlobalBank.Commercial.EBanking.Modules.EFT.Tests.Views
{
    /// <summary>
    /// Summary description for NewTransferViewPresenter
    /// </summary>
    [TestClass]
    public class NewTransferViewPresenterFixture
    {
        [TestMethod]
        public void OnViewLoadedCallsControllerGetTransfersAndSetsTransfersInView()
        {
            MockEFTController controller = new MockEFTController();
            Transfer transfer = GetNewTransfer();
            controller.Transfers = new Transfer[] { transfer };
            NewTransferViewPresenter presenter = new NewTransferViewPresenter(controller);
            MockNewTransferView view = new MockNewTransferView();
            presenter.View = view;

            presenter.OnViewLoaded();

            Assert.IsTrue(controller.GetTransfersCalled);
            Assert.IsTrue(view.TransfersSet);
            Assert.AreSame(transfer, view.Transfers[0]);
            Assert.AreEqual(1, view.Transfers.Length);
        }

        [TestMethod]
        public void OnViewLoadedCallsControllerGetAccountsAndSetsAccountsInView()
        {
            MockEFTController controller = new MockEFTController();
            Account account = new Account();
            controller.Accounts = new Account[] { account };
            NewTransferViewPresenter presenter = new NewTransferViewPresenter(controller);
            MockNewTransferView view = new MockNewTransferView();
            presenter.View = view;

            presenter.OnViewLoaded();

            Assert.IsTrue(controller.GetAccountsCalled);
            Assert.IsTrue(view.AccountsSet);
            Assert.AreSame(account, view.Accounts[0]);
            Assert.AreEqual(1, view.Accounts.Length);
        }

        [TestMethod]
        public void InsertTransferCallsControllerAddTransferToBatch()
        {
            MockEFTController controller = new MockEFTController();
            NewTransferViewPresenter presenter = new NewTransferViewPresenter(controller);
            MockNewTransferView view = new MockNewTransferView();
			Transfer insertedTransfer = GetNewTransfer();
            presenter.View = view;

            presenter.OnTransferInserted(insertedTransfer);

            Assert.IsTrue(controller.AddTransferToBatchCalled);
            Assert.AreSame(insertedTransfer, controller.TransferAddedToBatch);
        }

        [TestMethod]
        public void UpdateTransferCallsControllerUpdateTransferInBatch()
        {
            MockEFTController controller = new MockEFTController();
            NewTransferViewPresenter presenter = new NewTransferViewPresenter(controller);
            MockNewTransferView view = new MockNewTransferView();
            presenter.View = view;
			Transfer updatedTransfer = GetNewTransfer();
           
			presenter.OnTransferUpdated(updatedTransfer);

            Assert.IsTrue(controller.UpdateTransferInBatchCalled);
            Assert.AreSame(updatedTransfer, controller.TransferUpdatedInBatch);
        }

        [TestMethod]
        public void DeleteTransferCallsControllerRemoveTransferFromBatch()
        {
            MockEFTController controller = new MockEFTController();
            NewTransferViewPresenter presenter = new NewTransferViewPresenter(controller);
            MockNewTransferView view = new MockNewTransferView();
            Transfer deletedTransfer = GetNewTransfer();
            presenter.View = view;
			
            presenter.OnTransferDeleted(deletedTransfer);

            Assert.IsTrue(controller.RemoveTransferFromBatchCalled);
            Assert.AreSame(deletedTransfer, controller.TransferRemovedFromBatch);
        }

        [TestMethod]
        public void NextButtonClickedCallsControllerConfirmTransfers()
        {
            MockEFTController controller = new MockEFTController();
            NewTransferViewPresenter presenter = new NewTransferViewPresenter(controller);

            presenter.OnNext();

            Assert.IsTrue(controller.ConfirmTransfersCalled);
        }

        [TestMethod]
        public void GetAccountNameReturnsCorrectAccountName()
        {
            MockEFTControllerWithAccountData controller = new MockEFTControllerWithAccountData();
            NewTransferViewPresenter presenter = new NewTransferViewPresenter(controller);

            string AccountName = presenter.GetAccountName("GC20003004");

            Assert.AreEqual("GlobalBank Savings 20003004", AccountName);
        }

        [TestMethod]
        [ExpectedException(typeof(AccountNotFoundException))]
        public void LookupAccountNameThrowsAnErrorIfAccountNumberNotFound()
        {
            MockEFTControllerWithAccountData controller = new MockEFTControllerWithAccountData();
            NewTransferViewPresenter presenter = new NewTransferViewPresenter(controller);

            string AccountName = presenter.GetAccountName("NOTFOUND");
        }

		[TestMethod]
		public void AddIsDisabledAfterFiveItemsAdded()
		{
			MockEFTController controller = new MockEFTController();
            NewTransferViewPresenter presenter = new NewTransferViewPresenter(controller);
            MockNewTransferView view = new MockNewTransferView();
            presenter.View = view;
			
			Assert.IsTrue(view.EnableAddTransfer);

            presenter.OnTransferInserted(GetNewTransfer());
			presenter.OnTransferInserted(GetNewTransfer());
			presenter.OnTransferInserted(GetNewTransfer());
			presenter.OnTransferInserted(GetNewTransfer());
			presenter.OnTransferInserted(GetNewTransfer());

			presenter.OnViewLoaded();
			Assert.IsFalse(view.EnableAddTransfer);
		}

		[TestMethod]
		public void AddIsEnabledAfterFithItemDeleted()
		{
			MockEFTController controller = new MockEFTController();
			NewTransferViewPresenter presenter = new NewTransferViewPresenter(controller);
			MockNewTransferView view = new MockNewTransferView();
			presenter.View = view;
			Transfer deletedTransfer = GetNewTransfer();

			Assert.IsTrue(view.EnableAddTransfer);

			presenter.OnTransferInserted(GetNewTransfer());
			presenter.OnTransferInserted(GetNewTransfer());
			presenter.OnTransferInserted(GetNewTransfer());
			presenter.OnTransferInserted(GetNewTransfer());
			presenter.OnTransferInserted(deletedTransfer);

			presenter.OnViewLoaded();
			Assert.IsFalse(view.EnableAddTransfer);

			presenter.OnTransferDeleted(deletedTransfer);
			presenter.OnViewLoaded();
			Assert.IsTrue(view.EnableAddTransfer);
		}

        private Transfer GetNewTransfer()
        {
            return new Transfer(Guid.NewGuid());
        }
    }

    class MockNewTransferView : INewTransferView
    {
        public bool TransfersSet  = false;
        public bool AccountsSet = false;
        public bool UpdatedTransferRetrieved = false;
        public bool DeletedTransferRetrieved = false;
        private Transfer[] _transfers;
        private Account[] _accounts;
		private bool _addButtonEnabled = true;

        private Transfer _updatedTransfer;
        private Transfer _deletedTransfer;
        private Transfer _newTransfer;
        public bool NewTransferRetrieved = false;

        #region INewTransferView Members

		public bool EnableAddTransfer
		{
			get
			{
				return _addButtonEnabled;
			}
			set
			{
				_addButtonEnabled = value;
			}
		}

        public Transfer[] Transfers
        {
            get { return _transfers; }
            set
            {
                TransfersSet = true;
                _transfers = value;
            }
        }

        public Account[] Accounts
        {
            get { return _accounts; }
            set
            {
                AccountsSet = true;
                _accounts = value;
            }
        }

        public Transfer UpdatedTransfer
        {
            get
            {
                UpdatedTransferRetrieved = true;
                return _updatedTransfer;
            }
            set { _updatedTransfer = value; }
        }

        public Transfer DeletedTransfer
        {
            get
            {
                DeletedTransferRetrieved = true;
                return _deletedTransfer;
            }
            set { _deletedTransfer = value; }
        }

        public Transfer NewTransfer
        {
            get
            {
                NewTransferRetrieved = true;
                return _newTransfer;
            }
            set { _newTransfer = value; }
        }

        #endregion
    }

    class MockEFTControllerWithAccountData : MockEFTController
    {
		public override GlobalBank.Commercial.EBanking.Modules.EFT.BusinessEntities.Account[] GetAccounts()
        {
            List<Account> accounts = new List<Account>();
            Account account = new Account();
            account.Id = Guid.NewGuid();
            account.Number = "GC20003004";
            account.Name = "GlobalBank Savings 20003004";
            accounts.Add(account);
            return accounts.ToArray();
        }
    }
}

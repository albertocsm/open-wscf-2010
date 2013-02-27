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
using System.Text;
using GlobalBank.Commercial.EBanking.Modules.EFT.BusinessEntities;

namespace GlobalBank.Commercial.EBanking.Modules.EFT.Tests.Mocks
{
    class MockEFTController : EFTController
    {
        public bool TransferFundsCalled = false;
        public bool GetAccountsCalled = false;
        public bool GetTransfersCalled = false;
        public bool AddTransferToBatchCalled = false;
        public bool UpdateTransferInBatchCalled = false;
        public bool RemoveTransferFromBatchCalled = false;
        public bool ConfirmTransfersCalled = false;
        public bool SubmitTransfersCalled = false;
        public bool ChangeTransfersCalled = true;
        public bool CreateAccountCalled = false;
        public bool FundsTransferCompleteCalled = false;
		public List<Transfer> _transfers = new List<Transfer>();
#pragma warning disable 0108
		public Account[] Accounts = null;
        public new Transfer[] Transfers 
		{
			get
			{
				return _transfers.ToArray();
			}
			set
			{
				_transfers = new List<Transfer>();
				_transfers.InsertRange(0, value);
			}
		}
#pragma warning restore 0108
		public Transfer TransferAddedToBatch = null;
        public Transfer TransferUpdatedInBatch = null;
        public Transfer TransferRemovedFromBatch = null;
        public Account CreatedAccount = null;

        public MockEFTController()
            : base(new MockAccountServiceAgent(), new MockSessionStateLocatorService())
        {

        }

        public override void TransferFunds()
        {
            TransferFundsCalled = true;
        }

        public override Account[] GetAccounts()
        {
            GetAccountsCalled = true;
            return Accounts;
        }

        public override Transfer[] GetTransfers()
        {
            GetTransfersCalled = true;
            return Transfers;
        }

        public override void AddTransferToBatch(Transfer transfer)
        {
            AddTransferToBatchCalled = true;
            TransferAddedToBatch = transfer;
			_transfers.Add(transfer);
        }

        public override void UpdateTransferInBatch(Transfer updated)
        {
            UpdateTransferInBatchCalled = true;
            TransferUpdatedInBatch = updated;
        }

        public override void RemoveTransferFromBatch(Transfer transfer)
        {
            RemoveTransferFromBatchCalled = true;
            TransferRemovedFromBatch = transfer;
			_transfers.Remove(transfer);
        }

        public override void ConfirmTransfers()
        {
            ConfirmTransfersCalled = true;
        }

        public override void SubmitTransfers()
        {
            SubmitTransfersCalled = true;
        }

        public override void ChangeTransfers()
        {
            ChangeTransfersCalled = true;
        }

        public override void CreateAccount(Account account)
        {
            CreateAccountCalled = true;
            CreatedAccount = account;
        }

        public override void FundsTransferComplete()
        {
            FundsTransferCompleteCalled = true;
        }
    }
}

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
using Microsoft.Practices.ObjectBuilder;
using GlobalBank.Commercial.EBanking.Modules.EFT.BusinessEntities;
using GlobalBank.Commercial.EBanking.Modules.EFT.Exceptions;
using Microsoft.Practices.CompositeWeb;

namespace GlobalBank.Commercial.EBanking.Modules.EFT.Views
{
    public class NewTransferViewPresenter : Presenter<INewTransferView>
    {
        private EFTController _controller;
		private const int MaxBatchSize = 5;
        public NewTransferViewPresenter([CreateNew] EFTController controller)
        {
            _controller = controller;
        }

        public override void OnViewLoaded()
        {
            View.Accounts = _controller.GetAccounts();
            View.Transfers = _controller.GetTransfers();
			UpdateAllowAddTransfer();
        }

		public void OnTransferInserted(Transfer transfer)
        {
            _controller.AddTransferToBatch(transfer);
			UpdateAllowAddTransfer();
        }

		public void OnTransferDeleted(Transfer transfer)
        {
            _controller.RemoveTransferFromBatch(transfer);
			UpdateAllowAddTransfer();
        }

        public void OnTransferUpdated(Transfer transfer)
        {
            _controller.UpdateTransferInBatch(transfer);
			UpdateAllowAddTransfer();
        }

        public void OnNext()
        {
            _controller.ConfirmTransfers();
        }

        public string GetAccountName(string accountNumber)
        {
            Account[] accounts = _controller.GetAccounts();
            foreach (Account currentAccount in accounts)
            {
                if (currentAccount.Number == accountNumber)
                {
                    return currentAccount.Name;
                }
            }
            throw new AccountNotFoundException(accountNumber);
        }

		private void UpdateAllowAddTransfer()
		{
			if (_controller.GetTransfers().Length >= MaxBatchSize)
			{
				View.EnableAddTransfer = false;
			}
			else
			{
				View.EnableAddTransfer = true;
			}
		}
    }
}

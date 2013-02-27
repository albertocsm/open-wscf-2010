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
using GlobalBank.Commercial.EBanking.Modules.EFT.ServiceProxies.AccountService;
using GlobalBank.Commercial.EBanking.Modules.EFT.ServiceProxies.Services;

namespace GlobalBank.Commercial.EBanking.Modules.EFT.ServiceProxies.EntityTranslators
{
	public class TransferTableEntryTranslator : EntityMapperTranslator<Transfer, GlobalBank.Commercial.EBanking.Modules.EFT.ServiceProxies.AccountService.TransferTableEntry>
    {
		protected override GlobalBank.Commercial.EBanking.Modules.EFT.ServiceProxies.AccountService.TransferTableEntry BusinessToService(IEntityTranslatorService service, Transfer value)
        {
            TransferTableEntry to = new TransferTableEntry ();
            to.id = value.Id.ToString();
            to.sourceAccount = value.SourceAccount;
            to.destinationAccount = value.DestinationAccount;
            to.amount = value.Amount;
            to.description = value.Description;
            to.status = value.Status;
            to.sourceAccountName = value.SourceAccountName;
            to.destinationAccountName = value.DestinationAccountName;

            return to;
        }

		protected override Transfer ServiceToBusiness(IEntityTranslatorService service, GlobalBank.Commercial.EBanking.Modules.EFT.ServiceProxies.AccountService.TransferTableEntry value)
        {
            Transfer to = new Transfer();
            to.Id = new Guid(value.id);
            to.SourceAccount = value.sourceAccount;
            to.DestinationAccount = value.destinationAccount;
            to.Amount = value.amount;
            to.Description = value.description;
            to.Status = value.status;
            to.SourceAccountName = value.sourceAccountName;
            to.DestinationAccountName = value.destinationAccountName;
            to.TransactionId = value.transactionid;

            return to;
        }
    }
}

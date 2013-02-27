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
using GlobalBank.Commercial.EBanking.Modules.EFT.ServiceProxies.Services;
using GlobalBank.Commercial.EBanking.Modules.EFT.ServiceProxies.AccountService;

namespace GlobalBank.Commercial.EBanking.Modules.EFT.ServiceProxies.EntityTranslators
{
	public class ProcessTransfersRequestTypeTranslator : EntityMapperTranslator<Transfer[], GlobalBank.Commercial.EBanking.Modules.EFT.ServiceProxies.AccountService.ProcessTransfersRequestType>
    {
		protected override GlobalBank.Commercial.EBanking.Modules.EFT.ServiceProxies.AccountService.ProcessTransfersRequestType BusinessToService(IEntityTranslatorService service, Transfer[] value)
        {
            List<TransferTableEntry> transfers = new List<TransferTableEntry>();
            
            foreach (Transfer transfer in value)
            {
                TransferTableEntry to = service.Translate<TransferTableEntry>(transfer);
                transfers.Add(to);
            }

            ProcessTransfersRequestType request = new ProcessTransfersRequestType();
            request.accountsToProcess = transfers.ToArray();

            return request;
        }

        protected override Transfer[] ServiceToBusiness(IEntityTranslatorService service, ProcessTransfersRequestType value)
        {
            List<Transfer> transfers = new List<Transfer>();

            foreach (TransferTableEntry transfer in value.accountsToProcess)
            {
                Transfer to = service.Translate<Transfer>(transfer);
                transfers.Add(to);
            }

            return transfers.ToArray();
        }
    }
}

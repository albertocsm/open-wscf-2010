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
using GlobalBank.Commercial.EBanking.Modules.EFT.ServiceProxies.Services;
using GlobalBank.Commercial.EBanking.Modules.EFT.BusinessEntities;
using GlobalBank.Commercial.EBanking.Modules.EFT.ServiceProxies.AccountService;

namespace GlobalBank.Commercial.EBanking.Modules.EFT.ServiceProxies.EntityTranslators
{
    public class AccountTranslator : EntityMapperTranslator<Account, AccountService.AccountTableEntityType>
    {
		protected override GlobalBank.Commercial.EBanking.Modules.EFT.ServiceProxies.AccountService.AccountTableEntityType BusinessToService(IEntityTranslatorService service, Account value)
        {
            AccountTableEntityType to = new AccountTableEntityType();
            to.id = value.Id.ToString();
            to.ownerId = value.CustomerId;
            to.name = value.Name;
            to.number = value.Number;

            return to;
        }

		protected override Account ServiceToBusiness(IEntityTranslatorService service, GlobalBank.Commercial.EBanking.Modules.EFT.ServiceProxies.AccountService.AccountTableEntityType value)
        {
            Account to = new Account();
            to.CustomerId = value.ownerId;
            to.Id = new Guid(value.id);
            to.Name = value.name;
            to.Owned = true;
            to.Number = value.number;

            return to;
        }
    }
}

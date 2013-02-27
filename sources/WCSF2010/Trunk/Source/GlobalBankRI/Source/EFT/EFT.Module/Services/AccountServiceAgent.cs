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
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Services;
using Microsoft.Practices.ObjectBuilder;
using GlobalBank.Commercial.EBanking.Modules.EFT.BusinessEntities;
using GlobalBank.Commercial.EBanking.Modules.EFT.ServiceProxies.AccountService;
using GlobalBank.Commercial.EBanking.Modules.EFT.ServiceProxies.Services;

namespace GlobalBank.Commercial.EBanking.Modules.EFT.Services
{
	public class AccountServiceAgent : IAccountServiceAgent
	{
		private GlobalBank.Commercial.EBanking.Modules.EFT.ServiceProxies.AccountService.IAccountServiceProxy _proxy; 
        private IEntityTranslatorService _translator = null;

        [InjectionConstructor]
        public AccountServiceAgent
            (
                [ServiceDependency] IEntityTranslatorService translator
            )
        {
            _translator = translator;
			_proxy = new GlobalBank.Commercial.EBanking.Modules.EFT.ServiceProxies.AccountService.AccountServiceProxy();
        }

        public AccountServiceAgent(IEntityTranslatorService translator, IAccountServiceProxy proxy)
        {
            _translator = translator;
            _proxy = proxy;

        }

		public GlobalBank.Commercial.EBanking.Modules.EFT.BusinessEntities.Account[] GetAccounts(string userName)
        {
			GetAccountRequestType request = new GetAccountRequestType();
			request.userName = userName;
			GetAccountResponseType response = _proxy.GetAccounts(request);
			List<Account> userAccounts = new List<Account>();
			foreach (AccountTableEntityType account in response.userAccounts)
			{
				userAccounts.Add(_translator.Translate<Account>(account));
			}
			return userAccounts.ToArray();
        }

        public void CreateAccount(Account account)
        {
            CreateAccountRequestType request = new CreateAccountRequestType();
            request.account = _translator.Translate<AccountTableEntityType>(account);
            _proxy.CreateAccount(request);
        }

        public Transfer[] ProcessTransfers(Transfer[] transfers)
        {
            ProcessTransfersRequestType request = _translator.Translate<ProcessTransfersRequestType>(transfers);
            return _translator.Translate<Transfer[]>(_proxy.ProcessTransfers(request));
        }
    }
}

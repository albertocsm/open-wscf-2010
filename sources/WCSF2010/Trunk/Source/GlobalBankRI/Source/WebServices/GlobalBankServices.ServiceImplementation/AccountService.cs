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
using GlobalBankServices.DataTypes;
using GlobalBankServices.ServiceContracts;

namespace GlobalBankServices.ServiceImplementation
{
    // IMPORTANT: There is a bug in ASP. NET 2.0 by which the Namespace parameter of the System.Web.Services.WebService attribute has no effect when that attribute is applied to an interface
    [System.Web.Services.WebService(Namespace = "http://GlobalBankServices.ServiceContracts/2006/11", Name = "AccountService")]
    [System.Web.Services.WebServiceBindingAttribute(ConformsTo = System.Web.Services.WsiProfiles.BasicProfile1_1, EmitConformanceClaims = true)]
    public class AccountService : IAccountService
    {
        private static List<AccountTableEntityType> _accounts;

        static AccountService()
        {
            _accounts = new List<AccountTableEntityType>();
            _accounts.Add(CreateAccount("GC10002309", "admin", "GlobalBank Checkings 10002309", Guid.NewGuid().ToString()));
            _accounts.Add(CreateAccount("GC20003004", "admin", "GlobalBank Savings 20003004", Guid.NewGuid().ToString()));
            _accounts.Add(CreateAccount("WB10002309", "admin", "Woodgrove Bank 10002309", Guid.NewGuid().ToString()));
            _accounts.Add(CreateAccount("WB20002304", "admin", "Woodgrove Bank  20002304", Guid.NewGuid().ToString()));
            _accounts.Add(CreateAccount("WT10002309", "oper", "WCSF TestBank Checkings 10002309", Guid.NewGuid().ToString()));
            _accounts.Add(CreateAccount("WT20003004", "oper", "WCSF TestBank Savings 20003004", Guid.NewGuid().ToString()));
            _accounts.Add(CreateAccount("WS10002309", "oper", "WCSF SampleBank 10002309", Guid.NewGuid().ToString()));
            _accounts.Add(CreateAccount("WS20002304", "oper", "WCSF SampleBank Bank  20002304", Guid.NewGuid().ToString()));
        }

        public GlobalBankServices.DataTypes.GetAccountResponseType GetAccounts(GlobalBankServices.DataTypes.GetAccountRequestType request)
        {
            GetAccountResponseType response = new GetAccountResponseType();
            AccountTableEntityType[] userAccounts = _accounts.FindAll(new Predicate<AccountTableEntityType>(delegate(AccountTableEntityType expectedAccount)
													{
														// Do not do this. This is not secure. 
														// This is for only simplicity in our perf and 
														// stress testing so we don't need 10000 lines in our ctor
														return request.userName.StartsWith(expectedAccount.ownerId);
														
														// Do this instead
														//return expectedAccount.ownerId == request.userName;
													}
													)).ToArray();
            response.userAccounts = userAccounts;
            return response;
        }

        public void CreateAccount(CreateAccountRequestType request)
        {
            _accounts.Add(request.account);
        }

        public ProcessTransfersResponseType ProcessTransfers(ProcessTransfersRequestType processTransfersRequest)
        {
            TransferTableEntry[] transfersToProcess = processTransfersRequest.accountsToProcess;

            foreach (TransferTableEntry transfer in transfersToProcess)
            {
                transfer.status = "Completed";
                transfer.transactionid = Guid.NewGuid().ToString();
            }

            ProcessTransfersResponseType response = new ProcessTransfersResponseType();
            response.processedAccounts = transfersToProcess;
            return response;
        }

        private static AccountTableEntityType CreateAccount(string number, string ownerId, string name, string id)
        {
            AccountTableEntityType account = new AccountTableEntityType();
            account.id = id;
            account.name = name;
            account.number = number;
            account.ownerId = ownerId;

            return account;
        }
    }
}

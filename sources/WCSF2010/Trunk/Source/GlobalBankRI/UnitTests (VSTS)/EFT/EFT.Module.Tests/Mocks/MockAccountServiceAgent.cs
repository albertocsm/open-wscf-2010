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
using GlobalBank.Commercial.EBanking.Modules.EFT.Services;
using GlobalBank.Commercial.EBanking.Modules.EFT.BusinessEntities;

namespace GlobalBank.Commercial.EBanking.Modules.EFT.Tests.Mocks
{
    public class MockAccountServiceAgent : IAccountServiceAgent
    {
        public bool GetAccountsCalled = false;
        public bool CreateAccountCalled = false;
        public bool ProcessTransfersCalled = false;

        #region IAccountServiceAgent Members

        public Account[] GetAccounts(string userName)
        {
            GetAccountsCalled = true;
            return new List<Account>().ToArray();
        }

        public void CreateAccount(Account account)
        {
            CreateAccountCalled = true;
        }

        #endregion

        #region IAccountServiceAgent Members


        public Transfer[] ProcessTransfers(Transfer[] transfers)
        {
            ProcessTransfersCalled = true;
            return new Transfer[] { new Transfer() };
        }

        #endregion
    }
}

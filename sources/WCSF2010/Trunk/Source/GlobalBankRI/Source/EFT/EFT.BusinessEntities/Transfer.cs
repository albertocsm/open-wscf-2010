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

namespace GlobalBank.Commercial.EBanking.Modules.EFT.BusinessEntities
{
    [Serializable]
    public class Transfer
    {
        private Guid _id;
        private string _sourceAccount;
        private string _sourceAccountName;
        private string _destinationAccount;
        private string _destinationAccountName;
        private float _amount;
        private string _description;

        public Transfer()
        {
            _id = Guid.NewGuid();
        }

        public Transfer(Guid id)
        {
            _id = id;
        }

        public Transfer(Guid id, string sourceAccount, string destinationAccount, float amount, string description)
            : this(id)
        {
            _sourceAccount = sourceAccount;
            _destinationAccount = destinationAccount;
            _amount = amount;
            _description = description;
        }

        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }


        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public float Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        public string DestinationAccount
        {
            get { return _destinationAccount; }
            set { _destinationAccount = value; }
        }

        public string DestinationAccountName
        {
            get { return _destinationAccountName; }
            set { _destinationAccountName = value; }
        }

        public string SourceAccount
        {
            get { return _sourceAccount; }
            set { _sourceAccount = value; }
        }

        public string SourceAccountName
        {
            get { return _sourceAccountName; }
            set { _sourceAccountName = value; }
        }

        private string _transactionId;

        public string TransactionId
        {
            get { return _transactionId; }
            set { _transactionId = value; }
        }

        private string _status;

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

    }
}

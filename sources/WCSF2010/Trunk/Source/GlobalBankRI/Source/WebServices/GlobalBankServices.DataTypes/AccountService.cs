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


namespace GlobalBankServices.DataTypes {
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://GlobalBankServices.ServiceContracts/2006/11/IAccountService")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://GlobalBankServices.ServiceContracts/2006/11/IAccountService", IsNullable=true)]
    public partial class GetAccountRequestType {
        
        private string userNameField;
        
        /// <remarks/>
        public string userName {
            get {
                return userNameField;
            }
            set {
                userNameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://GlobalBankServices.ServiceContracts/2006/11/IAccountService")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://GlobalBankServices.ServiceContracts/2006/11/IAccountService", IsNullable=true)]
    public partial class GetAccountResponseType {
        
        private AccountTableEntityType[] userAccountsField;
        
        /// <remarks/>
        public AccountTableEntityType[] userAccounts {
            get {
                return userAccountsField;
            }
            set {
                userAccountsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://GlobalBankServices.ServiceContracts/2006/11/IAccountService")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://GlobalBankServices.ServiceContracts/2006/11/IAccountService", IsNullable=true)]
    public partial class AccountTableEntityType {
        
        private string idField;
        
        private string nameField;
        
        private string numberField;
        
        private string ownerIdField;
        
        /// <remarks/>
        public string id {
            get {
                return idField;
            }
            set {
                idField = value;
            }
        }
        
        /// <remarks/>
        public string name {
            get {
                return nameField;
            }
            set {
                nameField = value;
            }
        }
        
        /// <remarks/>
        public string number {
            get {
                return numberField;
            }
            set {
                numberField = value;
            }
        }
        
        /// <remarks/>
        public string ownerId {
            get {
                return ownerIdField;
            }
            set {
                ownerIdField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://GlobalBankServices.ServiceContracts/2006/11/IAccountService")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://GlobalBankServices.ServiceContracts/2006/11/IAccountService", IsNullable=true)]
    public partial class CreateAccountRequestType {
        
        private AccountTableEntityType accountField;
        
        /// <remarks/>
        public AccountTableEntityType account {
            get {
                return accountField;
            }
            set {
                accountField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://GlobalBankServices.ServiceContracts/2006/11/IAccountService")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://GlobalBankServices.ServiceContracts/2006/11/IAccountService", IsNullable=true)]
    public partial class TransferTableEntry {
        
        private string idField;
        
        private string sourceAccountField;

        private string sourceAccountNameField;
        
        private string destinationAccountField;

        private string destinationAccountNameField;
        
        private float amountField;
        
        private string descriptionField;

        private string statusField;

        private string transactionidField;
        
        /// <remarks/>
        public string id {
            get {
                return idField;
            }
            set {
                idField = value;
            }
        }
        
        /// <remarks/>
        public string sourceAccount {
            get {
                return sourceAccountField;
            }
            set {
                sourceAccountField = value;
            }
        }

        public string sourceAccountName
        {
            get
            {
                return sourceAccountNameField;
            }
            set
            {
                sourceAccountNameField = value;
            }
        }
        
        /// <remarks/>
        public string destinationAccount {
            get {
                return destinationAccountField;
            }
            set {
                destinationAccountField = value;
            }
        }

        public string destinationAccountName
        {
            get
            {
                return destinationAccountNameField;
            }
            set
            {
                destinationAccountNameField = value;
            }
        }
        
        /// <remarks/>
        public float amount {
            get {
                return amountField;
            }
            set {
                amountField = value;
            }
        }
        
        /// <remarks/>
        public string description {
            get {
                return descriptionField;
            }
            set {
                descriptionField = value;
            }
        }

        /// <remarks/>
        public string status
        {
            get
            {
                return statusField;
            }
            set
            {
                statusField = value;
            }
        }


        public string transactionid
        {
            get
            {
                return transactionidField;
            }
            set
            {
                transactionidField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://GlobalBankServices.ServiceContracts/2006/11/IAccountService")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://GlobalBankServices.ServiceContracts/2006/11/IAccountService", IsNullable=true)]
    public partial class ProcessTransfersRequestType {
        
        private TransferTableEntry[] accountsToProcessField;
        
        /// <remarks/>
        public TransferTableEntry[] accountsToProcess {
            get {
                return accountsToProcessField;
            }
            set {
                accountsToProcessField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://GlobalBankServices.ServiceContracts/2006/11/IAccountService")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://GlobalBankServices.ServiceContracts/2006/11/IAccountService", IsNullable=true)]
    public partial class ProcessTransfersResponseType {
        
        private TransferTableEntry[] processedAccountsField;
        
        /// <remarks/>
        public TransferTableEntry[] processedAccounts {
            get {
                return processedAccountsField;
            }
            set {
                processedAccountsField = value;
            }
        }
    }
}
namespace GlobalBankServices.DataTypes {
    
    
    public partial class NewDataSet {
    }
}

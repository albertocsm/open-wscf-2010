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

namespace GlobalBankServices.ServiceContracts
{
    // IMPORTANT: There is a bug in ASP. NET 2.0 by which the Namespace parameter of the System.Web.Services.WebService attribute has no effect when that attribute is applied to an interface
    [System.Web.Services.WebService(Namespace = "http://GlobalBankServices/2006/06", Name = "IAccountService")]
    [System.Web.Services.WebServiceBindingAttribute(ConformsTo = System.Web.Services.WsiProfiles.BasicProfile1_1, EmitConformanceClaims = true, Name = "QuoteService")]
    public interface IAccountService
    {
        [System.Web.Services.WebMethodAttribute(MessageName = "GetAccounts")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute(Action = "http://GlobalBankServices/2006/06/GetAccounts", ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        GlobalBankServices.DataTypes.GetAccountResponseType GetAccounts(GlobalBankServices.DataTypes.GetAccountRequestType getAccountsRequest);
        [System.Web.Services.WebMethodAttribute(MessageName = "CreateAccount")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute(Action = "http://GlobalBankServices/2006/06/CreateAccount", ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        void CreateAccount(CreateAccountRequestType createAccountRequest);
        [System.Web.Services.WebMethodAttribute(MessageName = "ProcessTransfers")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute(Action = "http://GlobalBankServices/2006/06/ProcessTransfers", ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        ProcessTransfersResponseType ProcessTransfers(ProcessTransfersRequestType processTransfersRequest);
    }
}

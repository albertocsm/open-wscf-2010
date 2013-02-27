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
using OrderManagement.Orders.Converters;

namespace OrderManagement.Orders.Tests.Mocks
{
    internal class MockGenericConverter<TBusinessEntity, TPresentationEntity>
        : IBusinessPresentationConverter<TBusinessEntity, TPresentationEntity>
        where TBusinessEntity : new()
        where TPresentationEntity : new()
    {
        public TBusinessEntity BusinessEntity = new TBusinessEntity();
        public TPresentationEntity PresentationEntity = new TPresentationEntity();
        public TBusinessEntity ConvertPresentationToBusiness(TPresentationEntity presentationEntity)
        {
            return BusinessEntity;
        }

        public TPresentationEntity ConvertBusinessToPresentation(TBusinessEntity businessEntity)
        {
            return PresentationEntity;
        }
    }
}

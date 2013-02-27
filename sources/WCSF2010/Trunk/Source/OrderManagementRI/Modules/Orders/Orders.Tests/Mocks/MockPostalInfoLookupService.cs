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
using OrdersRepository.Interfaces.Services;
using OrdersRepository.BusinessEntities;

namespace OrderManagement.Orders.Tests.Mocks
{
    public class MockPostalInfoLookupService : IPostalInfoLookupService
    {
        private IEnumerable<State> _allStates;

        public IEnumerable<State> AllStates
        {
            get { return _allStates; }
            set { _allStates = value; }
        }

        public IEnumerable<State> GetStates(string prefix, int maxResults)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IEnumerable<string> GetCities(string prefix, string state, int maxResults)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IEnumerable<ZipCode> GetZipCodes(string prefix, string city, string state, int maxResults)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public State GetStateById(string stateId)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}

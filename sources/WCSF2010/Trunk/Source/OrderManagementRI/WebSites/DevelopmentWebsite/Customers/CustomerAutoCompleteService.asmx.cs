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
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using OrdersRepository.Interfaces.Services;
using Microsoft.Practices.CompositeWeb;
using System.Web.Script.Services;
using System.Collections.Generic;
using OrdersRepository.BusinessEntities;

namespace WebApplication.Customers
{
    /// <summary>
    /// Summary description for CustomerAutoCompleteService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class CustomerAutoCompleteService : WebService
    {
        ICustomerService _customerService;

        [ServiceDependency]
        public ICustomerService CustomerService
        {
            set { _customerService = value; }
            get { return _customerService; }
        }

        public CustomerAutoCompleteService()
        {
            WebClientApplication.BuildItemWithCurrentContext(this);
        }

        [WebMethod]
        public string[] GetCustomersName(string prefixText, int count)
        {
            ICollection<Customer> customers = _customerService.GetCustomersByNamePrefix(prefixText);
            SortedList<string, object> names = new SortedList<string, object>(count);
            foreach (Customer customer in customers)
            {
                if (!names.ContainsKey(customer.CompanyName))
                    names.Add(customer.CompanyName, null);

                if (names.Count == count)
                    break;
            }

            string[] result = new string[names.Count];
            names.Keys.CopyTo(result, 0);
            return result;
        }
    }
}

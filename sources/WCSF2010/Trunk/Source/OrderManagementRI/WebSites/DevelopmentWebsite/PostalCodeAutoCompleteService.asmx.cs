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

namespace WebApplication
{
    /// <summary>
    /// Summary description for PostalCodeAutoCompleteService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class PostalCodeAutoCompleteService : WebService
    {
        private IPostalInfoLookupService _lookupService;

        [ServiceDependency]
        public IPostalInfoLookupService LookupService
        {
            get { return _lookupService; }
            set { _lookupService = value; }
        }


        public PostalCodeAutoCompleteService()
        {
            WebClientApplication.BuildItemWithCurrentContext(this);
            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }

        [WebMethod]
        public string[] GetZipCodes(string prefixText, int count, Dictionary<string, string> contextValues)
        {
            List<ZipCode> zips = new List<ZipCode>(_lookupService.GetZipCodes(prefixText,
                                                                 contextValues["City"],
                                                                 contextValues["State"],
                                                                 count));
            return zips.ConvertAll<string>(delegate(ZipCode zip) { return zip.PostalCode; }).ToArray();
        }

        [WebMethod]
        public string[] GetCities(string prefixText, int count, Dictionary<string, string> contextValues)
        {
            List<string> cities = new List<string>(_lookupService.GetCities(prefixText,
                                                                            contextValues["State"],
                                                                            count));
            return cities.ToArray();
        }
    }
}

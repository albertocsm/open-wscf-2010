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

namespace OrdersRepository.BusinessEntities
{
    public partial class Customer
    {
		public Customer()
		{
		}

        private String addressField;

        public String Address
        {
            get { return this.addressField; }
            set { this.addressField = value; }
        }

        private String cityField;

        public String City
        {
            get { return this.cityField; }
            set { this.cityField = value; }
        }

        private String companyNameField;

        public String CompanyName
        {
            get { return this.companyNameField; }
            set { this.companyNameField = value; }
        }

        private String customerIdField;

        public String CustomerId
        {
            get { return this.customerIdField; }
            set { this.customerIdField = value; }
        }

        private String postalCodeField;

        public String PostalCode
        {
            get { return this.postalCodeField; }
            set { this.postalCodeField = value; }
        }

        private String regionField;

        public String Region
        {
            get { return this.regionField; }
            set { this.regionField = value; }
        }

    }
}


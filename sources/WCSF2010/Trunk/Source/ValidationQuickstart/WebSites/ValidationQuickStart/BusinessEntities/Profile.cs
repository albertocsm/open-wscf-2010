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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using ValidationQuickStart.Validators;
using System.Globalization;
using ValidationQuickStart.Properties;
using ValidationQuickStart.PostalData;

namespace ValidationQuickStart.BusinessEntities
{
    [HasSelfValidation]
    public class Profile
    {

        private string _accountNumber;
        [AccountNumberValidator(MessageTemplateResourceType = typeof(Resources), MessageTemplateResourceName = "InvalidAccountNumberTemplateMessage")]
        public string AccountNumber
        {
            get { return _accountNumber; }
            set { _accountNumber = value; }
        }

        private string _email;
        [RegexValidator(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*")]
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }


        private string _name;
        [StringLengthValidator(25, MessageTemplateResourceType = typeof(Resources), MessageTemplateResourceName = "InvalidNameLengthTemplateMessage")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private int? _age;
        [IgnoreNulls(MessageTemplateResourceType = typeof(Resources), MessageTemplateResourceName = "AgeInvalidTemplateMessage")]
        [RangeValidator(16, RangeBoundaryType.Exclusive, 30, RangeBoundaryType.Inclusive)]
        public int? Age
        {
            get { return _age; }
            set { _age = value; }
        }

        private string _homepage;

		// The view specifies a custom validator for this field.  As a result, this validator is not called by the view via Ajax calls.
		// However, this validation is valid for the business object.
        [RegexValidator(@"http(s)?://([\w-]+\.)*[\w-]+(:\d+)?(/[\w- ./?%&=]*)?", MessageTemplateResourceType = typeof(Resources), MessageTemplateResourceName = "HomepageInvalidTemplateMessage")]
        public string Homepage
        {
            get { return _homepage; }
            set { _homepage = value; }
        }

        private string _state;
        [StateValidator(MessageTemplateResourceType = typeof(Resources), MessageTemplateResourceName = "InvalidStateTemplateMessage")]
        public string State
        {
            get { return _state; }
            set { _state = value; }
        }

        private string _zip;
        [ZipCodeValidator(MessageTemplateResourceType = typeof(Resources), MessageTemplateResourceName = "InvalidZipCodeTemplateMessage")]
        public string Zip
        {
            get { return _zip; }
            set { _zip = value; }
        }


        [SelfValidation]
        public void ValidateAddress(ValidationResults results)
        {
            if (!PostalInfoLookupDataSet.Instance.ZipBelongsToState(_zip, _state))
            {
                string errorMessage = string.Format(CultureInfo.CurrentCulture, Resources.InvalidAddressTemplateMessage, _zip, _state);
                ValidationResult result = new ValidationResult(errorMessage, this, "Zip", "Address", null);
                results.AddResult(result);
            }
        }
    }
}

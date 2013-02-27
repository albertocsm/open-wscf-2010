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
using System.Runtime.Serialization;

namespace AjaxControlToolkit.WCSFExtensions
{
    /// <summary>
    /// Class used for Client-Server communication on validation
    /// </summary>
    [DataContract]
    public class ServerValidationResults
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"> Value present in validated control</param>
        /// <param name="isValid"> Result of validation</param>
        /// <param name="errorMessage"> Error message that will be shown in the Validator</param>
        public ServerValidationResults(string value, bool isValid, string errorMessage)
        {
            _value = value;
            _isValid = isValid;
            _errorMessage = errorMessage;
        }

        private string _value;
        /// <summary>
        /// Value present in the validated control
        /// </summary>
        [DataMember]
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        private bool _isValid;
        /// <summary>
        /// Result of the validation
        /// </summary>
        [DataMember]
        public bool IsValid
        {
            get { return _isValid; }
            set { _isValid = value; }
        }

        private string _errorMessage;
        /// <summary>
        /// Error message that will be shown in the Validator
        /// </summary>
        [DataMember]
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }
    }
}

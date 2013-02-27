//===============================================================================
// Microsoft patterns & practices
//  GAX Extension Library
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
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Microsoft.Practices.RecipeFramework.Extensions.Validators
{
    /// <summary/>
    public class TypeNameValidatorAttribute : ConfigurationValidatorAttribute
    {
        private bool multipleTypes;
        private bool optional;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TypeNameValidatorAttribute"/> class.
        /// </summary>
        public TypeNameValidatorAttribute()
        {
        }

        /// <summary>
        /// Gets the validator attribute instance.
        /// </summary>
        /// <value></value>
        /// <returns>The current <see cref="T:System.Configuration.ConfigurationValidatorBase"></see>.</returns>
        public override ConfigurationValidatorBase ValidatorInstance
        {
            get
            {
                TypeNameValidator validator = new TypeNameValidator();
                validator.MultipleTypes = multipleTypes;
                validator.Optional = optional;
                return validator;
            }
        }

        /// <summary>
        /// Especifies if the value can contain more than one type separated with ","
        /// </summary>
        public bool MultipleTypes
        {
            get { return multipleTypes; }
            set { multipleTypes = value; }
        }

        /// <summary>
        /// Especifies if the value is optional
        /// </summary>
        public bool Optional
        {
            get { return optional; }
            set { optional = value; }
        }
    }
}

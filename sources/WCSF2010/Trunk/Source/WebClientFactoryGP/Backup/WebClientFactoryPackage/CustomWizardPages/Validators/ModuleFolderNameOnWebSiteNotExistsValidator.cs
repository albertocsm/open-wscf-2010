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
using Microsoft.Practices.WebClientFactory.CustomWizardPages;
using Microsoft.Practices.WebClientFactory.Properties;
using System.IO;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Validators
{
    public class ModuleFolderNameOnWebSiteNotExistsValidator : Validator<string>
    {
        public ModuleFolderNameOnWebSiteNotExistsValidator()
            : base(null, null)
        {
        }

        protected override void DoValidate(string objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            ICreateModulePageModel model = currentTarget as ICreateModulePageModel;
            if (model != null)
            {
                if (model.WebProject.Project != null)
                {
                    WebFolderNameValidator webfolderValidator = new WebFolderNameValidator();
                    ValidationResults results= webfolderValidator.Validate(objectToValidate);
                    if (results.IsValid)
                    {
                        string moduleFolderPath = Path.Combine(model.WebProject.ProjectPath, objectToValidate);

                        if (Directory.Exists(moduleFolderPath))
                        {
                            this.LogValidationResult(validationResults, DefaultMessageTemplate, currentTarget, key);
                        }
                    }
                }
            }
        }

        protected override string DefaultMessageTemplate
        {
            get { return "Folder already exists"; }
        }
    }
}

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
using System.Globalization;
using System.IO;
using Microsoft.Practices.WebClientFactory.CustomWizardPages;
using Microsoft.Practices.WebClientFactory.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Validators
{
    public class ViewNameFileNotExistsValidator : Validator<string>
    {
        public const string DefaultFailureMessage = "View filename already exists";

        public ViewNameFileNotExistsValidator()
            :base(null,null)
        {
        }

        protected override void DoValidate(string objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            ICreateViewPageBaseModel pageModel = currentTarget as ICreateViewPageBaseModel;
            if (pageModel != null)
            {
                string physicalPath = string.Empty;

                if (pageModel.WebFolder != null)
                {
                    physicalPath = pageModel.WebFolder.ItemPath;
                }
                else
                {
                    physicalPath = pageModel.WebProject.ProjectPath;
                }

                string viewFileName = String.Format(CultureInfo.CurrentCulture, "{0}.{1}", objectToValidate, pageModel.ViewFileExtension);
                string viewExistsMessage = DefaultFailureMessage;

            	string pathToValidate = String.Empty;

				try
				{
					pathToValidate = Path.Combine(physicalPath, viewFileName);
				}
				catch (ArgumentException ex)
				{
					validationResults.AddResult(new ValidationResult(ex.Message, objectToValidate, key, "Bad file name", this));
					return;
				}
            	

                Validator<string> _viewViewFileValidator = new FileNotExistsValidator(
                                    physicalPath,
									String.Format(CultureInfo.CurrentCulture, viewExistsMessage, pathToValidate));
                _viewViewFileValidator.Validate(viewFileName, validationResults);
            }
        }

        protected override string DefaultMessageTemplate
        {
            get { return DefaultFailureMessage; }
        }
    }
}

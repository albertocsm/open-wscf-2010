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
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Practices.WebClientFactory.CustomWizardPages;
using Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers;
using Microsoft.Practices.WebClientFactory.CustomWizardPages.Validators;
using EnvDTE;
using Microsoft.Practices.RecipeFramework.Extensions.DteWrapper;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Validators
{
	public class ViewNameFileNameValidator : AndCompositeValidator
	{
		/// <summary>
		/// Creates a new instance of <see cref="FileNameValidator"/>
		/// </summary>
		public ViewNameFileNameValidator()
			: base(new Validator[] {new ViewNameFileNameLengthValidator(), new ReservedSystemWordsFileNameValidator(), new ViewNameFileNameNotExistsValidator()})
		{
		}

		private class ViewNameFileNameLengthValidator : Validator<string>
		{
			// Due to the generated helper classes, whose names are based on the 
			// View's name, we need to shorten the max length allowed by 30 to be safe
			private const int maxFileNameLength = 80; // 110 - 30
			private const int maxFullFileNameLength = 230; // 260 - 30

			public ViewNameFileNameLengthValidator()
				: base("The view file name is too long", null)
			{
			}

			protected override void DoValidate(string objectToValidate, object currentTarget, string key,
			                                   ValidationResults validationResults)
			{
                ICreateViewPageBaseModel pageModel = currentTarget as ICreateViewPageBaseModel;
                if (pageModel != null)
                {
                    string physicalPath = string.Empty;
                    if (pageModel.ModuleProject!= null)
                    {
                        physicalPath = Path.Combine(pageModel.ModuleProject.ProjectPath, "Views");
                    }
                    
                    string fileName =
                        String.Format(CultureInfo.CurrentCulture, "{0}Presenter.{1}", objectToValidate, pageModel.ModuleProjectLanguage);
                	string fullFileName = String.Empty; 

					try
					{
						fullFileName = Path.Combine(physicalPath, fileName);
					}
					catch (ArgumentException ex)
					{
						validationResults.AddResult(new ValidationResult(ex.Message, objectToValidate, key, "Bad file name", this));
						return;
					}


                    if (objectToValidate == null || fullFileName.Length > maxFullFileNameLength ||
                        objectToValidate.Length > maxFileNameLength)
                        LogValidationResult(validationResults, MessageTemplate, currentTarget, key);
                }
			}

			protected override string DefaultMessageTemplate
			{
				get { return "The view file name is too long"; }
			}
		}

		private class ViewNameFileNameNotExistsValidator : Validator<string>
        {
            public ViewNameFileNameNotExistsValidator()
				: base("The view name is already in use", null)
			{
			}

			protected override void DoValidate(string objectToValidate, object currentTarget, string key,
			                                   ValidationResults validationResults)
			{
                ICreateViewPageBaseModel pageModel = currentTarget as ICreateViewPageBaseModel;
                if (pageModel != null)
                {
                    string presenterFileName = String.Format(CultureInfo.CurrentCulture, "{0}Presenter.{1}", objectToValidate, pageModel.ModuleProjectLanguage);
                    string viewInterfaceFileName = String.Format(CultureInfo.CurrentCulture, "I{0}View.{1}", objectToValidate, pageModel.ModuleProjectLanguage);

                    if (FileExists(pageModel, presenterFileName) || FileExists(pageModel, viewInterfaceFileName))
                        LogValidationResult(validationResults, MessageTemplate, currentTarget, key);
                }
			}

            private bool FileExists(ICreateViewPageBaseModel pageModel, string fileName)
            {
                if (pageModel.ModuleProject == null 
                    || !(pageModel.ModuleProject is IProjectModel)
                    || pageModel.ModuleProject.Project == null
                    || !(pageModel.ModuleProject.Project is Project))
                {
                    return false;
                }
                else
                {
                    ProjectItem item = DteHelperEx.FindItemByName(((Project)pageModel.ModuleProject.Project).ProjectItems, fileName, true);
                    return item != null;
                }
            }

			protected override string DefaultMessageTemplate
			{
                get { return "The view name is already in use"; }
			}
        }
	}
}

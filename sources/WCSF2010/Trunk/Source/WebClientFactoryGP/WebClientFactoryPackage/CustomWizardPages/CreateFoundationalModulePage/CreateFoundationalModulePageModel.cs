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
using System.ComponentModel.Design;
using Microsoft.Practices.RecipeFramework.Extensions.DteWrapper;
using EnvDTE;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Microsoft.Practices.WebClientFactory.CustomWizardPages
{
    public interface ICreateFoundationalModulePageModel : ILanguageDependentPageModel
    {
        string ModuleName { get; set; }
        IList<IProjectModel> WebProjects { get; }
        IProjectModel WebProject { get; set; }
        object SelectedSolutionFolder { get; set;}
        bool CreateTestProject { get; set; }
        bool CreateModuleInterfaceLibrary { get; set; }
        bool IsValid { get; }
        bool ShowDocumentation { get; set; }
    }

    public class CreateFoundationalModulePageModel : ICreateFoundationalModulePageModel
    {
        private const string ModuleNameKey = "ModuleName";
        private const string CreateTestProjectKey = "CreateTestProject";
        private const string CreateModuleInterfaceLibraryKey = "CreateModuleInterfaceLibrary";
        private const string WebUIProjectKey = "WebUIProject";
        private const string ShowDocumentationKey = "ShowDocumentation";
        private const string SelectedSolutionFolderKey = "SelectedSolutionFolder";
        private const string RecipeLanguageKey = "RecipeLanguage";

        private ISolutionModel solutionModel;
        private IDictionaryService dictionary;
        private IServiceProvider serviceProvider;
        private object _selectedSolutionFolder;

        private Validator _validator;

        public CreateFoundationalModulePageModel(IDictionaryService dictionary, ISolutionModel solutionModel, IServiceProvider serviceProvider)
        {
            this.dictionary = dictionary;
            this.solutionModel = solutionModel;
            this.serviceProvider = serviceProvider;
        }

        [NotNullValidator]
        [StringLengthValidator(1, 250)]
        [IdentifierValidator]
        public string ModuleName
        {
            get { return GetDictString(ModuleNameKey); }
            set { dictionary.SetValue(ModuleNameKey, value); }
        }

        public bool CreateTestProject
        {
            get { return (bool)dictionary.GetValue(CreateTestProjectKey); }
            set { dictionary.SetValue(CreateTestProjectKey, value); }
        }

        public bool CreateModuleInterfaceLibrary
        {
            get { return (bool)dictionary.GetValue(CreateModuleInterfaceLibraryKey); }
            set { dictionary.SetValue(CreateModuleInterfaceLibraryKey, value); }
        }

        public bool ShowDocumentation
        {
            get { return (bool)dictionary.GetValue(ShowDocumentationKey); }
            set { dictionary.SetValue(ShowDocumentationKey, value); }
        }

        public IList<IProjectModel> WebProjects
        {
            get { return solutionModel.FindProjectsWithResponsibility("IsWebProject"); }
        }

        [NotNullValidator]
        public IProjectModel WebProject
        {
            get
            {
                if (dictionary.GetValue(WebUIProjectKey) != null)
                    return new DteProjectModel(dictionary.GetValue(WebUIProjectKey) as Project, serviceProvider);
                else
                    return null;
            }
            set
            {
                if (value != null)
                {
                    dictionary.SetValue(WebUIProjectKey, value.Project);
                }
            }
        }

        public object SelectedSolutionFolder
        {
            get
            {
                if (dictionary.GetValue(SelectedSolutionFolderKey) != null)
                {
                    _selectedSolutionFolder = new DteProjectModel(dictionary.GetValue(SelectedSolutionFolderKey) as Project, serviceProvider);
                }
                return _selectedSolutionFolder;
            }
            set
            {
                _selectedSolutionFolder = value;
            }
        }

        private string GetDictString(string key)
        {
            return (string)(dictionary.GetValue(key));
        }

        public bool IsValid
        {
            get
            {
                if (_validator == null)
                {
                    _validator = ValidationFactory.CreateValidator<CreateFoundationalModulePageModel>();
                }

                return _validator.Validate(this).IsValid;
            }
        }

        public string Language
        {
            get { return GetDictString(RecipeLanguageKey); }
        }

    }
}

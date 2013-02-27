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
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Microsoft.Practices.WebClientFactory.CustomWizardPages
{
    public interface ISolutionPropertiesModel : ILanguageDependentPageModel
    {
        string SupportLibrariesPath { get; set; }
        string CompositeWebDlls { get; set; }
        string EnterpriseLibraryDlls { get; set; }
        string RootNamespace { get; set; }
        string WebUIProjectName { get;}
        bool IsValid { get; }
        bool ShowDocumentation { get; set; }
        string[] GetSupportingLibraries();
        bool[] GetMissingLibraries();
    }

    [HasSelfValidation]
    public class SolutionPropertiesModel : ISolutionPropertiesModel
    {
        private const string SupportLibrariesPathKey = "SupportLibrariesPath";
        private const string CompositeWebDllsKey = "CompositeWebDlls";
        private const string EnterpriseLibraryDllsKey = "EnterpriseLibraryDlls";
        private const string RootNamespaceKey = "RootNamespace";
        private const string ShowDocumentationKey = "ShowDocumentation";
        private const string RecipeLanguageKey = "RecipeLanguage";
        private const string WebUIProjectNameKey = "WebUIProjectName";

        private IDictionaryService dictionary;
        private Validator _validator;

        public SolutionPropertiesModel(IDictionaryService dictionary)
        {
            this.dictionary = dictionary;
        }

        [NotNullValidator]
        [StringLengthValidator(1, 250)]
        [PathExistsValidator]
        public string SupportLibrariesPath
        {
            get { return GetDictString(SupportLibrariesPathKey); }
            set { dictionary.SetValue(SupportLibrariesPathKey, value); }
        }

        [NotNullValidator]
        public string CompositeWebDlls
        {
            get { return GetDictString(CompositeWebDllsKey); }
            set { dictionary.SetValue(CompositeWebDllsKey, value); }
        }

        [NotNullValidator]
        public string EnterpriseLibraryDlls
        {
            get { return GetDictString(EnterpriseLibraryDllsKey); }
            set { dictionary.SetValue(EnterpriseLibraryDllsKey, value); }
        }

        [NotNullValidator]
        [StringLengthValidator(1, 250)]
        [NamespaceValidator]
        public string RootNamespace
        {
            get { return GetDictString(RootNamespaceKey); }
            set { dictionary.SetValue(RootNamespaceKey, value); }
        }

        public bool ShowDocumentation
        {
            get { return (bool)dictionary.GetValue(ShowDocumentationKey); }
            set { dictionary.SetValue(ShowDocumentationKey, value); }
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
                    _validator = ValidationFactory.CreateValidator<SolutionPropertiesModel>();
                }

                return _validator.Validate(this).IsValid;
            }
        }

        public string[] GetSupportingLibraries()
        {
            string libraries = String.Join(";", new string[]
                {
                    CompositeWebDlls,
                    EnterpriseLibraryDlls
                });
            return libraries.Split(';');
        }

        public bool[] GetMissingLibraries()
        {
            string[] libraries = GetSupportingLibraries();
            List<bool> missing = new List<bool>(libraries.Length);
            foreach (string file in libraries)
            {
                if (string.IsNullOrEmpty(SupportLibrariesPath))
                {
                    missing.Add(true);
                }
                else
                {
                    FileExistsValidator fileValidator =
                        new FileExistsValidator(SupportLibrariesPath);
                    bool fileExists = fileValidator.Validate(file).IsValid;
                    missing.Add(!fileExists);
                }
            }
            return missing.ToArray();
        }

        [SelfValidation()]
        public void Validate(ValidationResults validationResults)
        {
            Validator _supportLibrariesValidator =
                new FileArrayExistsValidator(SupportLibrariesPath);

            _supportLibrariesValidator.Validate(GetSupportingLibraries(), validationResults);
        }

        public string Language
        {
            get { return GetDictString(RecipeLanguageKey); }
        }

        public string WebUIProjectName
        {
            get { return GetDictString(WebUIProjectNameKey); }
        }
    }
}

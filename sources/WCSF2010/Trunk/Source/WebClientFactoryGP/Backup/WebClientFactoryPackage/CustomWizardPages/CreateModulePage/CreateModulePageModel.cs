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
using System.Globalization;

namespace Microsoft.Practices.WebClientFactory.CustomWizardPages
{
    public interface ICreateModulePageModel : ILanguageDependentPageModel
    {
        string ModuleName { get; set; }
        string ModuleFolderNameOnWebSite { get; set; }
        IList<IProjectModel> WebProjects { get; }
        IProjectModel WebProject { get; set; }
        object SelectedSolutionFolder { get; set;}
        bool CreateModuleInterfaceLibrary { get; set; }
        bool CreateTestProject { get; set; }
        bool ShowDocumentation { get; set; }
        bool IsValid { get; }

        bool CreateAsFolderInWebsite { get; set; }
        bool IsWCSFSolutionWAP { get; }
        string ModuleWebsiteName { get; set; }
        string ValidationErrorMessage { get; }

    }

    public class CreateModulePageModel : ICreateModulePageModel
    {
        private const string ModuleNameKey = "ModuleName";
        private const string ModuleFolderNameOnWebSiteKey = "ModuleFolderNameOnWebSite";
        private const string CreateTestProjectKey = "CreateTestProject";
        private const string CreateModuleInterfaceLibraryKey = "CreateModuleInterfaceLibrary";
        private const string WebUIProjectKey = "WebUIProject";
        private const string ShowDocumentationKey = "ShowDocumentation";
        private const string SelectedSolutionFolderKey = "SelectedSolutionFolder";

        private const string CreateAsFolderInWebsiteKey = "CreateAsFolderInWebsite";
        private const string IsWCSFSolutionWAPKey = "IsWCSFSolutionWAP";
        private const string ModuleWebsiteNameKey = "ModuleWebsiteName";
        private const string RecipeLanguageKey = "RecipeLanguage";
        
        private ISolutionModel solutionModel;
        private IDictionaryService dictionary;
        private IServiceProvider serviceProvider;
        private object _selectedSolutionFolder;

        private Validator _validator;

        public CreateModulePageModel(IDictionaryService dictionary, ISolutionModel solutionModel, IServiceProvider serviceProvider)
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

        [NotNullValidator]
        [WebFolderNameValidator]
        [ReservedSystemWordsFileNameValidator]
        [ModuleFolderNameOnWebSiteNotExistsValidator()]
        [StringLengthValidator(1, 250)]
        public string ModuleFolderNameOnWebSite
        {
            get { return GetDictString(ModuleFolderNameOnWebSiteKey); }
            set { dictionary.SetValue(ModuleFolderNameOnWebSiteKey, value); }
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
            get
            {
                List<IProjectModel> webProjects = solutionModel.FindProjectsWithResponsibility("IsWebProject");
                //exclude projects that are sub web projects
                List<IProjectModel> subWebProjects = solutionModel.FindProjectsWithResponsibility("IsFolderOfRootWebProject");
                int i = 0;
                bool found;
                while (i < webProjects.Count)
                {
                    found = false;
                    foreach (IProjectModel subWebProject in subWebProjects)
                    {
                        if (webProjects[i].ProjectPath != null)
                        {
                            if (subWebProject.ProjectPath == webProjects[i].ProjectPath)
                            {
                                webProjects.RemoveAt(i);
                                found = true;
                                break;
                            }
                        }
                        else if (subWebProject.ProjectPath == null)
                        {
                            if (subWebProject == webProjects[i])
                            {
                                webProjects.RemoveAt(i);
                                found = true;
                                break;
                            }
                        }
                    }
                    if (!found)
                    {
                        i++;
                    }
                }
                return webProjects;
            }
        }

        [NotNullValidator]
        public IProjectModel WebProject
        {
            get
            {
                IProjectModel projectModel = dictionary.GetValue(WebUIProjectKey) as IProjectModel;
                if (projectModel != null)
                    return projectModel;
                else
                {
                    Project project = dictionary.GetValue(WebUIProjectKey) as Project;
                    return new DteProjectModel(project, serviceProvider);
                }
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
                    _selectedSolutionFolder = new DteProjectModel(dictionary.GetValue(SelectedSolutionFolderKey) as Project,serviceProvider);
                }
                return _selectedSolutionFolder;
            }
            set
            {
                _selectedSolutionFolder = value;
            }
        }


        public bool CreateAsFolderInWebsite
        {
            get { return (bool)dictionary.GetValue(CreateAsFolderInWebsiteKey); }
            set { dictionary.SetValue(CreateAsFolderInWebsiteKey, value); }
        }

        public string Language
        {
            get { return GetDictString(RecipeLanguageKey); }
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
                    _validator = ValidationFactory.CreateValidator<CreateModulePageModel>();
                }

                return _validator.Validate(this).IsValid;
            }
        }

        public bool IsWCSFSolutionWAP
        {
            get { return (bool)(dictionary.GetValue(IsWCSFSolutionWAPKey) ?? false); }
        }


        public string ModuleWebsiteName
        {
            get { return (string)dictionary.GetValue(ModuleWebsiteNameKey); }
            set { dictionary.SetValue(ModuleWebsiteNameKey, value); }
        }

        public string ValidationErrorMessage
        {
            get
            {
                if (_validator == null)
                {
                    _validator = ValidationFactory.CreateValidator<CreateModulePageModel>();
                }
                return FormatErrorMessage(_validator.Validate(this));
            }
        }

        private string FormatErrorMessage(ValidationResults validationResults)
        {
            StringBuilder builder = new StringBuilder();
            IEnumerator<ValidationResult> enumerator = ((IEnumerable<ValidationResult>)validationResults).GetEnumerator();
            if (enumerator.MoveNext())
            {
                while (true)
                {
					
                    builder.Append(enumerator.Current.Message);
                    if (!enumerator.MoveNext())
                    {
                        break;
                    }
                    builder.AppendLine();
                }
            }
            return builder.ToString();
        }

    }
}

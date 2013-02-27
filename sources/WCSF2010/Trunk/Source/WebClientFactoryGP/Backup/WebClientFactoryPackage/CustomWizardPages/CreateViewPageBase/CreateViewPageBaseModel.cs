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
using Microsoft.Practices.RecipeFramework.Extensions.DteWrapper;
using System.ComponentModel.Design;
using EnvDTE;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using System.Globalization;

namespace Microsoft.Practices.WebClientFactory.CustomWizardPages
{
    public interface ICreateViewPageBaseModel : ILanguageDependentPageModel, IModuleLanguageDependentPageModel
    {
        string ViewName { get; set; }
        string ViewsFolder { get; }
        string ViewFileExtension { get; }
        IList<IProjectModel> ModuleProjects { get; }
        IProjectModel ModuleProject { get; set; }
        IList<IProjectModel> WebProjects { get; }
        IProjectModel WebProject { get; set; }
        IList<IProjectItemModel> WebFolders { get; }
        IProjectItemModel WebFolder { get; set; }
        DependantModuleInfo[] ModuleInfoCollection { get; }
        bool TestProjectExists { get; }
        bool ShowDocumentation { get; set; }
        bool IsValid { get; }
        string ValidationErrorMessage { get; }
    }

    public class CreateViewPageBaseModel : ICreateViewPageBaseModel
    {
        private IDictionaryService _dictionary;
        private ISolutionModel _solutionModel;
        private IProjectModel _moduleProject;
        private IProjectModel _webProject;
        private IServiceProvider _serviceProvider;
        private IProjectItemModel _webFolder;
        private Validator _validator;

        private const string ModuleProjectKey = "ModuleProject";
        private const string ViewNameKey = "ViewName";
        private const string ViewFileExtensionKey = "ViewFileExtension";
        private const string ViewsFolderKey = "ViewsFolder";
        private const string WebProjectKey = "WebUIProject";
        private const string WebFolderKey = "SelectedWebProjectFolder";
        private const string TestProjectExistsKey = "TestProjectExists";
        private const string ModuleInfoCollectionKey = "ModuleInfos";
        private const string ShowDocumentationKey = "ShowDocumentation";
        private const string RecipeLanguageKey = "RecipeLanguage";
        private const string ModuleProjectLanguageKey = "ModuleProjectLanguage";


		public CreateViewPageBaseModel(IDictionaryService dictionary, ISolutionModel solutionModel, IServiceProvider serviceProvider)
        {
            _dictionary = dictionary;
            _solutionModel = solutionModel;
            _serviceProvider = serviceProvider;
        }

        [StringLengthValidator(1, 80)]
        [IdentifierValidator]
        [ViewNameFileNameValidator]
        [ViewNameFileNotExistsValidator]
        public string ViewName
        {
            get { return _dictionary.GetValue(ViewNameKey) as string; }
            set { _dictionary.SetValue(ViewNameKey, value); }
        }

        public string ViewFileExtension
        {
            get { return _dictionary.GetValue(ViewFileExtensionKey) as string; }
        }

        public string ViewsFolder
        {
            get { return _dictionary.GetValue(ViewsFolderKey) as string; }
        }

        public IList<IProjectModel> ModuleProjects
        {
            get { 
                List<IProjectModel> allModuleProjects = _solutionModel.FindProjectsWithResponsibility("IsModuleProject");
                return FilterProjectsWithVirtualPathNotNullAndContainedInCurrentWebsite(allModuleProjects, ModuleInfoCollection, WebFolders);
            }
        }        

        [NotNullValidator]
        public IProjectModel ModuleProject
        {
            get
            {
                if (_moduleProject == null)
                {
                    if (_dictionary.GetValue(ModuleProjectKey) != null)
                    {
                        _moduleProject = new DteProjectModel(_dictionary.GetValue(ModuleProjectKey) as Project, _serviceProvider);
                    }
                }
                return _moduleProject;                
            }
            set
            {
                _moduleProject = value;
                if (_moduleProject != null)
                {
                    _dictionary.SetValue(ModuleProjectKey, _moduleProject.Project);
                }
            }
        }

        public IList<IProjectModel> WebProjects
        {
            get { return _solutionModel.FindProjectsWithResponsibility("IsWebProject"); }
        }

        [NotNullValidator]
        public IProjectModel WebProject
        {
            get
            {
                if (_webProject == null)
                {
                    if (_dictionary.GetValue(WebProjectKey) != null)
                    {
                        _webProject = new DteProjectModel(_dictionary.GetValue(WebProjectKey) as Project, _serviceProvider);
                    }
                }
                return _webProject;
            }
            set
            {
                _webProject = value;
                if (_webProject != null)
                {
                    _dictionary.SetValue(WebProjectKey, _webProject.Project);
                }
            }
        }

        public IList<IProjectItemModel> WebFolders
        {
            get {
                if (WebProject != null)
                {
                    return WebProject.Folders;
                }
                else
                {
                    return null;
                }
            }
        }


        public IProjectItemModel WebFolder
        {
            get
            {
                if (_webFolder == null)
                {
                    if (_dictionary.GetValue(WebFolderKey) != null)
                    {
                        _webFolder = new DteProjectItemModel(_dictionary.GetValue(WebFolderKey) as ProjectItem);
                    }
                }
                return _webFolder;
            }
            set
            {
                _webFolder = value;
                if (_webFolder != null)
                {
                    _dictionary.SetValue(WebFolderKey, _webFolder.ProjectItem);
                }
                else
                {
                    _dictionary.SetValue(WebFolderKey, null);
                }
            }
        }


        public bool TestProjectExists
        {
            get
            {                
                return (bool)_dictionary.GetValue(TestProjectExistsKey); 
            }
        }

        public bool ShowDocumentation
        {
            get { return (bool)_dictionary.GetValue(ShowDocumentationKey); }
            set { _dictionary.SetValue(ShowDocumentationKey, value); }
        }

        public DependantModuleInfo[] ModuleInfoCollection
        {
            get
            {
                return (DependantModuleInfo[])_dictionary.GetValue(ModuleInfoCollectionKey);
            }
        }

        public string Language
        {
            get { return (string)_dictionary.GetValue(RecipeLanguageKey); }
        }

        public string ModuleProjectLanguage
        {
            get { return (string)_dictionary.GetValue(ModuleProjectLanguageKey); }
        }        
        
        public bool IsValid
        {
            get 
            {
                if (_validator == null)
                {
                    _validator = ValidationFactory.CreateValidator<CreateViewPageBaseModel>();
                }

                return _validator.Validate(this).IsValid;
            }
        }

        public string ValidationErrorMessage
        {
            get
            {
                if (_validator == null)
                {
                    _validator = ValidationFactory.CreateValidator<CreateViewPageBaseModel>();
                }
                return FormatErrorMessage(_validator.Validate(this));
            }
        }

        private string FormatErrorMessage(ValidationResults validationResults)
        {
            StringBuilder builder = new StringBuilder();

            foreach (ValidationResult result in validationResults)
            {
                builder.AppendLine(result.Message);
            }
            return builder.ToString();
        }

        private List<IProjectModel> FilterProjectsWithVirtualPathNotNullAndContainedInCurrentWebsite(List<IProjectModel> moduleProjects, DependantModuleInfo[] moduleInfos, IList<IProjectItemModel> webFolders)
        {
            List<IProjectModel> filtered = new List<IProjectModel>();
            foreach (IProjectModel project in moduleProjects)
            {
                IModuleInfo projectModuleInfo = FindModuleInfo(project.AssemblyName, moduleInfos);
                if (projectModuleInfo != null)
                {
                    if (!string.IsNullOrEmpty(projectModuleInfo.VirtualPath))
                    {
                        if ((projectModuleInfo.AssemblyName == this.ModuleProject.AssemblyName)
                        || WebFolderExistsInWebsite(projectModuleInfo.VirtualPath, webFolders))
                            filtered.Add(project);
                    }
                }
            }
            return filtered;
        }

        private static string RemoveTrailingWack(string folderPath)
        {
            folderPath = folderPath.EndsWith(@"\") ? folderPath.Remove(folderPath.LastIndexOf(@"\"), 1) : folderPath;
            folderPath = folderPath.EndsWith(@"/") ? folderPath.Remove(folderPath.LastIndexOf(@"/"), 1) : folderPath;
            return folderPath;
        }

        private bool WebFolderExistsInWebsite(string virtualPath, IList<IProjectItemModel> webFolders)
        {
            virtualPath = RemoveTrailingWack(virtualPath);
            int indexOfWack = virtualPath.LastIndexOf('/');
            string folderName;
            if (indexOfWack != -1 && virtualPath.Length > indexOfWack + 1)
            {
                folderName = virtualPath.Substring(indexOfWack + 1);
            }
            else
            {
                folderName = virtualPath;
            }
            List<IProjectItemModel> foldersList = new List<IProjectItemModel>(webFolders);
            IProjectItemModel result = foldersList.Find(delegate(IProjectItemModel match)
            {
                return match.Name == folderName;
            });
            return result != null;
        }


        private IModuleInfo FindModuleInfo(string assemblyName, IModuleInfo[] modules)
        {
            List<IModuleInfo> moduleList = new List<IModuleInfo>(modules);
            return moduleList.Find(delegate(IModuleInfo match)
            {
                return match.AssemblyName == assemblyName;
            });
        }
    }
}

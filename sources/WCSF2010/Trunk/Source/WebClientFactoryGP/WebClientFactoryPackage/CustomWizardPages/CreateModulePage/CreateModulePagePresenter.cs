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
using Microsoft.Practices.RecipeFramework.Library;
using Microsoft.Practices.WebClientFactory.Properties;
using Microsoft.Practices.RecipeFramework.Extensions;
using Microsoft.Practices.RecipeFramework.Extensions.DteWrapper;
using EnvDTE;
using System.IO;
using System.Globalization;

namespace Microsoft.Practices.WebClientFactory.CustomWizardPages
{
    public class CreateModulePagePresenter
    {
        private ICreateModulePageModel _model;
        private ICreateModulePage _view;

        public CreateModulePagePresenter(ICreateModulePage view, ICreateModulePageModel model)
        {
            Guard.ArgumentNotNull(view, "view");
            Guard.ArgumentNotNull(view, "model");
            
            _view = view;
            _model = model;

            _view.ModuleNameChanging += new EventHandler<EventArgs>(OnModuleNameChanging);
            _view.ModuleFolderNameOnWebSiteChanged += new EventHandler<EventArgs>(OnModuleFolderNameOnWebSiteChanged);
            _view.CreateTestProjectChanged += new EventHandler<EventArgs>(OnCreateTestProjectChanged);
            _view.CreateModuleInterfaceLibraryChanged += new EventHandler<EventArgs>(OnCreateModuleInterfaceLibraryChanged);
            _view.WebProjectSelected += new EventHandler<EventArgs>(OnWebProjectSelected);
            _view.RequestingValidation += new EventHandler<EventArgs<bool>>(OnValidating);
            _view.ShowDocumentationChanged += new EventHandler<EventArgs>(OnShowDocumentationChanged);
            _view.CreateAsFolderInWebsiteChanged += new EventHandler<EventArgs>(OnCreateAsFolderInWebsiteChanged);
        }

        void OnCreateAsFolderInWebsiteChanged(object sender, EventArgs e)
        {
            _model.CreateAsFolderInWebsite = _view.CreateAsFolderInWebsite;
        }

        void OnShowDocumentationChanged(object sender, EventArgs e)
        {
            _model.ShowDocumentation = _view.ShowDocumentation;
        }

        void OnWebProjectSelected(object sender, EventArgs e)
        {
            _model.WebProject = _view.SelectedWebProject;            
        }

        void OnCreateTestProjectChanged(object sender, EventArgs e)
        {
            _model.CreateTestProject = _view.CreateTestProject;
        }

        void OnCreateModuleInterfaceLibraryChanged(object sender, EventArgs e)
        {
            _model.CreateModuleInterfaceLibrary = _view.CreateModuleInterfaceLibrary;
        }

        void OnModuleFolderNameOnWebSiteChanged(object sender, EventArgs e)
        {
            _model.ModuleFolderNameOnWebSite = _view.ModuleFolderNameOnWebSite.Trim();
        }

        void OnModuleNameChanging(object sender, EventArgs e)
        {
            _model.ModuleName = _view.ModuleName;
            _model.ModuleFolderNameOnWebSite = _view.ModuleName;
            _view.ModuleFolderNameOnWebSite = _view.ModuleName;
        }

        // Implement Directory not exists with VAB
        //private void ValidateModuleWebFolderNotExist(object sender, EventArgs e)
        //{
        //    string webFolderPath = _model.WebProject.ProjectPath;

        //    string moduleWebFolderPath = Path.Combine(webFolderPath, _view.ModuleFolderNameOnWebSite);

        //    Validator<string> _moduleWebFolderPathValidator = new DirectoryNotExistsValidator(
        //                        String.Format(CultureInfo.CurrentCulture, Resources.WebFolderAlreadyExists, moduleWebFolderPath));

        //    _moduleWebFolderPathValidator.Validate(moduleWebFolderPath, sender, e);
        //}    

        public void OnViewReady()
        {
            string solutionFolderName;
            if (_model.SelectedSolutionFolder as IProjectModel != null
                && ((IProjectModel)_model.SelectedSolutionFolder).Project is Project
                && ((Project)((IProjectModel)_model.SelectedSolutionFolder).Project).Kind == EnvDTE.Constants.vsProjectKindSolutionItems)
            {
                solutionFolderName = ((IProjectModel)_model.SelectedSolutionFolder).Name;
            }
            else
            {
                solutionFolderName = "Solution";
            }

            _model.ModuleFolderNameOnWebSite = _model.ModuleName;

            _view.SetupView(_model.ModuleName, _model.ModuleFolderNameOnWebSite, solutionFolderName, _model.WebProjects, _model.IsWCSFSolutionWAP, _model.Language);
        }

        void OnValidating(object sender, EventArgs<bool> e)
        {
            bool validModel = _model.IsValid;
            if (!validModel)
            {
                _view.ShowValidationErrorMessage(_model.ValidationErrorMessage);
            }
            e.Data = _model.IsValid;
        }


        internal string GetModuleWebsiteName()
        {
            return _model.ModuleWebsiteName;
        }
    }    
}

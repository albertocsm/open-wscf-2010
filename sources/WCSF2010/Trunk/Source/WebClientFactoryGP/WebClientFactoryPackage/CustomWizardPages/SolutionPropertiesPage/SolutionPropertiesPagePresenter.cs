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
using System.IO;
using Microsoft.Practices.RecipeFramework.Extensions;
using System.Globalization;

namespace Microsoft.Practices.WebClientFactory.CustomWizardPages
{
    public class SolutionPropertiesPagePresenter
    {
        private ISolutionPropertiesPage _view;
        private ISolutionPropertiesModel _model;

        public SolutionPropertiesPagePresenter(ISolutionPropertiesPage view, ISolutionPropertiesModel model)
        {
            Guard.ArgumentNotNull(view, "view");
            Guard.ArgumentNotNull(view, "model");

            _view = view;
            _model = model;

            _view.SupportLibrariesPathChanging += new EventHandler<EventArgs>(OnSupportingLibrariesPathChanging);
            _view.RootNamespaceChanging += new EventHandler<EventArgs>(OnRootNamespaceChanging);
            _view.RequestingValidation += new EventHandler<EventArgs<bool>>(OnValidating);
            _view.ShowDocumentationChanging += new EventHandler<EventArgs>(OnShowDocumentationChanging);
        }

        void OnShowDocumentationChanging(object sender, EventArgs e)
        {
            _model.ShowDocumentation = _view.ShowDocumentation;
        }

        public void OnViewReady()
        {
            _view.SetLanguage(_model.Language);
            _view.SetWebUIProjectName(_model.WebUIProjectName);
            _view.ShowSupportLibraries(_model.GetSupportingLibraries(), _model.GetMissingLibraries());
            _view.ShowSupportLibrariesPath(_model.SupportLibrariesPath);
            _view.ShowRootNamespace(_model.RootNamespace);
            _view.RefreshSolutionPreview();
        }              

        void OnValidating(object sender, EventArgs<bool> e)
        {
            e.Data = _model.IsValid;
        }

        void OnSupportingLibrariesPathChanging(object sender, EventArgs e)
        {
            _model.SupportLibrariesPath = _view.SupportLibrariesPath;
            _view.ShowSupportLibraries(_model.GetSupportingLibraries(), _model.GetMissingLibraries());
        }

        void OnRootNamespaceChanging(object sender, EventArgs e)
        {
            _model.RootNamespace = _view.RootNamespace;
        }       
    }
}

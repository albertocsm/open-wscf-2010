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
using Microsoft.Practices.RecipeFramework.Extensions;
using Microsoft.Practices.RecipeFramework.Extensions.DteWrapper;
using EnvDTE;

namespace Microsoft.Practices.WebClientFactory.CustomWizardPages
{
    public interface ICreateModulePage
    {
        void SetupView(string moduleName, string webSiteModuleFolderName, string solutionFolderName, IList<IProjectModel> webProjects, bool subProjectOptionsVisible, string language);

        string ModuleName { get; }
        string ModuleFolderNameOnWebSite { get; set; }
        bool CreateTestProject { get; }
        bool CreateModuleInterfaceLibrary { get; }
        bool ShowDocumentation { get; }

        IProjectModel SelectedWebProject { get; }
        bool CreateAsFolderInWebsite { get; set; }

        event EventHandler<EventArgs> ModuleNameChanging;
        event EventHandler<EventArgs> ModuleFolderNameOnWebSiteChanged;
        event EventHandler<EventArgs> CreateModuleInterfaceLibraryChanged;
        event EventHandler<EventArgs> CreateTestProjectChanged;
        event EventHandler<EventArgs> WebProjectSelected;
        event EventHandler<EventArgs<bool>> RequestingValidation;

        event EventHandler<EventArgs> ShowDocumentationChanged;
        event EventHandler<EventArgs> CreateAsFolderInWebsiteChanged;

        void ShowValidationErrorMessage(string errorMessage);
    }
}

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
using Microsoft.Practices.RecipeFramework.Extensions;

namespace Microsoft.Practices.WebClientFactory.CustomWizardPages
{
    public interface ICreateViewPageBase
    {
        void ShowViewName(string viewName, string viewFileExtension);
        void ShowModuleProjects(IList<IProjectModel> modules, IProjectModel selected);
        void ShowWebProjects(IList<IProjectModel> webprojects, IProjectModel selected);
        void ShowWebFolders(IList<IProjectItemModel> folders, IProjectItemModel selected);
        void SelectModuleProject(IProjectModel module);
        void SelectWebFolder(IProjectItemModel moduleFolder);
        void RefreshSolutionPreview();
        void EnableModuleProjectList(bool enable);
        void SetLanguage(string recipeLanguage, string moduleLanguage);
        void ShowTestProject(bool testProjectExists);
        void ShowValidationErrorMessage(string errorMessage);

        event EventHandler<EventArgs> ModuleProjectChanged;
        event EventHandler<EventArgs> WebFolderChanged;
        event EventHandler<EventArgs> ViewNameChanged;
        event EventHandler<EventArgs<bool>> RequestingValidation;
        event EventHandler<EventArgs> ShowDocumentationChanged;
        
        IProjectModel ActiveModuleProject { get; }
        IProjectItemModel ActiveWebFolder { get; }
        string ViewName { get; }
        bool ShowDocumentation { get; }
        string Language { get; }
        string ModuleProjectLanguage { get; }
    }
}

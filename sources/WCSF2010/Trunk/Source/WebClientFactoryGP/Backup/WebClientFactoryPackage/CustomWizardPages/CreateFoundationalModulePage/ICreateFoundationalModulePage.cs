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

namespace Microsoft.Practices.WebClientFactory.CustomWizardPages
{
    public interface ICreateFoundationalModulePage
    {
        void ShowModuleName(string moduleName);
        void ShowWebProjects(IList<IProjectModel> projects);
        void RefreshSolutionPreview();
        void SetLanguage(string language);

        string ModuleName { get; }
        bool CreateTestProject { get; }
        bool CreateModuleInterfaceLibrary { get; }
        bool ShowDocumentation { get; }
        string Language { get; }

        IProjectModel SelectedWebProject { get; }
        string SelectedSolutionFolderName { get; set;}

        event EventHandler<EventArgs> ModuleNameChanging;
        event EventHandler<EventArgs> CreateTestProjectChanged;
        event EventHandler<EventArgs> CreateModuleInterfaceLibraryChanged;
        event EventHandler<EventArgs> WebProjectSelected;
        event EventHandler<EventArgs<bool>> RequestingValidation;
        event EventHandler<EventArgs> ShowDocumentationChanged;
    }
}

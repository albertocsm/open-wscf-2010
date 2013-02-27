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

namespace Microsoft.Practices.WebClientFactory.CustomWizardPages
{
    public interface ISolutionPropertiesPage
    {
        event EventHandler<EventArgs> SupportLibrariesPathChanging;
        event EventHandler<EventArgs> RootNamespaceChanging;
        event EventHandler<EventArgs<bool>> RequestingValidation;
        event EventHandler<EventArgs> ShowDocumentationChanging;
        void SetLanguage(string language);
        void SetWebUIProjectName(string webUIProjectName);
        void RefreshSolutionPreview();


        string SupportLibrariesPath { get; }
        string RootNamespace { get; }
        void ShowSupportLibraries(string[] libraries, bool[] missing);
        void ShowSupportLibrariesPath(string path);
        void ShowRootNamespace(string rootNamespace);
        string Language { get; }
        bool ShowDocumentation { get; }
    }
}

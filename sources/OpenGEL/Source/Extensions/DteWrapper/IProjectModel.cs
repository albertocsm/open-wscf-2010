//===============================================================================
// Microsoft patterns & practices
//  GAX Extension Library
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
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Design;
using EnvDTE;

namespace Microsoft.Practices.RecipeFramework.Extensions.DteWrapper
{
    /// <summary>
    /// Interface providing various services for querying or updating
    /// an open Visual Studio project.
    /// </summary>
    public interface IProjectModel
    {
        /// <summary>
        /// Projects the contains file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        bool ProjectContainsFile(string filename);

        /// <summary>
        /// Returns an instance of <see cref="ITypeResolutionService"/> for the project scope
        /// </summary>
        ITypeResolutionService TypeResolutionService { get; }

        /// <summary>
        /// Returns a list with the types defined in the project
        /// </summary>
        IList<ITypeModel> Types { get; }

        /// <summary>
        /// Returns a list of folders for the project
        /// </summary>
        IList<IProjectItemModel> Folders { get; }


        /// <summary>
        /// Returns true if the project is a website project
        /// </summary>
        bool IsWebProject { get; }

        /// <summary>
        /// Returns the underlying project
        /// </summary>
        object Project { get; }

        /// <summary>
        /// Returns the name of the project
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns the assembly name of the project
        /// </summary>
        string AssemblyName { get; }

        /// <summary>
        /// Returns the physical path of the project
        /// </summary>
        string ProjectPath { get; }
    }
}

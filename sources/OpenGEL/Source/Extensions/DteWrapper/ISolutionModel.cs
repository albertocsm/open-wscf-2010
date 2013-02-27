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
using EnvDTE;

namespace Microsoft.Practices.RecipeFramework.Extensions.DteWrapper
{
    /// <summary>
    /// Interface providing various services for querying or updating
    /// an open Visual Studio Solution.
    /// </summary>
    public interface ISolutionModel
    {
        /// <summary>
        /// Gets the current solution.
        /// </summary>
        object Solution { get; }

        /// <summary>
        /// Finds across the projects in the solution for projects with a given responsibility.       
        /// </summary>
        /// <param name="responsibility">Project responsibility</param>
        /// <returns>List of <see cref="IProjectModel"/></returns>
        List<IProjectModel> FindProjectsWithResponsibility(string responsibility);
        
        /// <summary>
        /// Finds across the projects in the solution for projects with a given set of responsibilities.       
        /// </summary>
        /// <param name="responsibilities">Project responsibilities</param>
        /// <returns>List of <see cref="IProjectModel"/></returns>
        List<IProjectModel> FindProjectsWithResponsibility(string[] responsibilities);

        /// <summary>
        /// Searches the longest path for a project item in the solution,
        /// relative to the path of the solution itself
        /// </summary>
        int LongestItemPath { get; }
    }
}

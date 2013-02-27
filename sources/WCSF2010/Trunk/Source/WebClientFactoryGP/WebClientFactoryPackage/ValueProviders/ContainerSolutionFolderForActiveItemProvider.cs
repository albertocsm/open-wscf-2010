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
using System.Diagnostics;
using EnvDTE;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework;
using EnvDTE80;

namespace Microsoft.Practices.WebClientFactory.ValueProviders
{
    /// <summary>
    /// ValueProvider that returns the first selected project
    /// in the solution explorer
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class ContainerSolutionFolderForActiveItemProvider : ValueProvider
    {
        /// <summary>
        /// Sets the newValue to the first selected project
        /// </summary>
        /// <param name="currentValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            newValue = currentValue;
            DTE vs = (DTE)GetService(typeof(DTE));
            object[] activeProjects = (object[])vs.ActiveSolutionProjects;
            if (activeProjects != null && activeProjects.Length > 0)
            {
                if (activeProjects[0] is Project &&
                    (activeProjects[0] as Project).ParentProjectItem != null &&
                    (activeProjects[0] as Project).ParentProjectItem.ContainingProject != null &&
                    (activeProjects[0] as Project).ParentProjectItem.ContainingProject.Object is SolutionFolder)
                {
                    newValue = (activeProjects[0] as Project).ParentProjectItem.ContainingProject as Project;
                }
            }
            if (newValue != null && newValue!=currentValue)
            {
                return true;
            }
            return false;
        }
    }
}

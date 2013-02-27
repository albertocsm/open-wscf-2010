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

namespace Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers
{
    /// <summary>
    /// Helper methods to work with project responsibilities
    /// </summary>
    public static class ResponsibilityHelper
    {
        private const string AssemblyNamePropertyName = "AssemblyName";

        /// <summary>
        /// Verifies if a project has a specified responsibility
        /// </summary>
        /// <param name="project">The project</param>
        /// <param name="responsibility">The responsibility</param>
        public static bool IsProjectResponsible(Project project, string responsibility)
        {
            if (project == null)
                throw new ArgumentNullException("project");

            if (project.Globals.get_VariableExists(responsibility))
            {
                string propertyValue = (string)project.Globals[responsibility];
                bool propertyValueBoolean;

                if (Boolean.TryParse(propertyValue, out propertyValueBoolean))
                {
                    if (propertyValueBoolean)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Verifies if a type is defined in a project with a specific responsibility
        /// </summary>
        /// <param name="dte">DTE context</param>
        /// <param name="type">The type to verify</param>
        /// <param name="responsibility">The responsibility</param>
        public static bool TypeDefinedInResponsibleProject(DTE dte, Type type, string responsibility)
        {
            string assemblyName = type.Assembly.FullName.Split(',')[0];
            ResponsibleProjectFinder projectFinder = new ResponsibleProjectFinder(dte);
            foreach (Project project in projectFinder.FindProjectsWithResponsibility(responsibility))
            {
                string projectAssembly = project.Properties.Item(AssemblyNamePropertyName).Value.ToString();
                if (string.Equals(assemblyName, projectAssembly, StringComparison.InvariantCulture))
                    return true;
            }

            return false;
        }
    }
}

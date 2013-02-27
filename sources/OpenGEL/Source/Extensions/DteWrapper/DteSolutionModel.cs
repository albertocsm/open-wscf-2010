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
using System.IO;
using Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers;

namespace Microsoft.Practices.RecipeFramework.Extensions.DteWrapper
{
    /// <summary>
    /// Abstracts the DTE solution model
    /// </summary>
    public class DteSolutionModel : ISolutionModel
    {
        private Solution solution;
        private IServiceProvider serviceProvider;

        /// <summary>
        /// Returns the actual DTE solution
        /// </summary>
        public object Solution
        {
            get { return solution; }
        }

        /// <summary>
        /// Creates an instance based on the DTE Solution and an <see cref="IServiceProvider"/>
        /// </summary>
        /// <param name="solution"></param>
        /// <param name="serviceProvider"></param>
        public DteSolutionModel(Solution solution, IServiceProvider serviceProvider)
        {
            this.solution = solution;
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Finds across the projects in the solution for projects with a given responsibility.       
        /// </summary>
        /// <param name="responsibility">Project responsibility</param>
        /// <returns>List of <see cref="IProjectModel"/></returns>
        public List<IProjectModel> FindProjectsWithResponsibility(string responsibility)
        {
            return FindProjectsWithResponsibility(new string[] { responsibility });
        }

        /// <summary>
        /// Finds across the projects in the solution for projects with a given set of responsibilities.       
        /// </summary>
        /// <param name="responsibilities">Project responsibilities</param>
        /// <returns>List of <see cref="IProjectModel"/></returns>
        public List<IProjectModel> FindProjectsWithResponsibility(string[] responsibilities)
        {
            ResponsibleProjectFinder projectFinder = new ResponsibleProjectFinder(solution.DTE);

            List<IProjectModel> foundProjects = new List<IProjectModel>();

            foreach (string responsibility in responsibilities)
            {
                List<Project> projects = projectFinder.FindProjectsWithResponsibility(responsibility);
                foundProjects.AddRange(projects.ConvertAll<IProjectModel>(new Converter<Project, IProjectModel>(ProjectToIProjectModel)));
            }

            return foundProjects;
        }

        private IProjectModel ProjectToIProjectModel(Project project)
        {
            return new DteProjectModel(project, serviceProvider);
        }

        /// <summary>
        /// Returns the lenght of the longest item path in the solution
        /// </summary>
        /// <remarks>Useful to detect paths longer than 260 chars</remarks>
        public int LongestItemPath
        {
            get
            {
                int maxLength = 0;
                int solutionDirLength = Path.GetDirectoryName(solution.FileName).Length + 1;

                foreach (Project project in new DteHelperEx.ProjectIterator(solution.DTE))
                {
                    maxLength = Math.Max(maxLength, project.FileName.Length - solutionDirLength);

                    foreach (ProjectItem item in new DteHelperEx.ProjectItemIterator(project))
                    {
                        if (item.FileCount > 0 && item.get_FileNames(1) != null)
                            maxLength = Math.Max(maxLength, item.get_FileNames(1).Length - solutionDirLength);
                    }
                }

                return maxLength;
            }
        }
    }
}

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
using EnvDTE80;
using VsWebSite;

namespace Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers
{
    /// <summary>
    /// Walk through the given solution and find the
    /// projects that have been given particular responsibilities.
    /// </summary>
    public class ResponsibleProjectFinder
    {
        private readonly Guid folderKindGuid = new Guid(ProjectKinds.vsProjectKindSolutionFolder);
        private readonly Guid csProjKindGuid = new Guid(VSLangProj.PrjKind.prjKindCSharpProject);
        private readonly Guid vbProjKindGuid = new Guid(VSLangProj.PrjKind.prjKindVBProject);
        private readonly Guid webProjKindGuid = new Guid(VsWebSite.PrjKind.prjKindVenusProject);

        private DTE dte;
        private List<Project> allProjects = new List<Project>();

        /// <summary>
        /// Create finder object.
        /// </summary>
        /// <param name="dte">The VS environment to search for projects.</param>
        public ResponsibleProjectFinder(DTE dte)
        {
            this.dte = dte;
            ReadProjects();
        }

        private void ReadProjects()
        {
            Solution solution = (Solution)dte.Solution;
            foreach (Project proj in solution.Projects)
            {
                ProcessProject(proj, allProjects);
            }
        }

        private void ProcessProject(Project proj, List<Project> projects)
        {
            if (IsCodeProject(proj))
            {
                projects.Add(proj);
            }

            if (IsSolutionFolder(proj))
            {
                ProcessSolutionFolder(proj, projects);
            }
        }

        private void ProcessSolutionFolder(Project proj, List<Project> projects)
        {
            foreach (ProjectItem item in proj.ProjectItems)
            {
                Project childProject = item.Object as Project;
                if (childProject != null)
                {
                    ProcessProject(childProject, projects);
                }
            }
        }

        private bool IsCodeProject(Project proj)
        {
            Guid kindGuid = new Guid(proj.Kind);
            return
                kindGuid == csProjKindGuid ||
                kindGuid == vbProjKindGuid ||
                kindGuid == webProjKindGuid;
        }

        private bool IsSolutionFolder(Project proj)
        {
            Guid itemKindGuid = new Guid(proj.Kind);
            return itemKindGuid == folderKindGuid;
        }

        /// <summary>
        /// Finds a list of projects in the solution with a responsibility
        /// </summary>
        /// <param name="responsibility"></param>
        /// <returns>The list of projects with that reponsibility</returns>
        public List<Project> FindProjectsWithResponsibility(string responsibility)
        {
            List<Project> foundProjects = new List<Project>();
            foreach (Project project in allProjects)
            {
                if (ResponsibilityHelper.IsProjectResponsible(project, responsibility))
                    foundProjects.Add(project);
            }

            return foundProjects;
        }
    }
}

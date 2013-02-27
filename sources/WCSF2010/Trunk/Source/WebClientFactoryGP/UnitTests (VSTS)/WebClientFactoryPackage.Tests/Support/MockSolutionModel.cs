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

namespace WebClientFactoryPackage.Tests.Support
{
    class MockSolutionModel : ISolutionModel
    {
        private object solution;
        private Dictionary<string, MockProjectModel> projects;
        private int longestItemPath;

        public MockSolutionModel(object solution)
        {
            this.solution = solution;
        }

        public object Solution
        {
            get { return solution; }
        }

        public void AddProject(string name, MockProjectModel project)
        {
            if (projects == null)
                projects = new Dictionary<string, MockProjectModel>();

            projects.Add(name, project);
        }

        public MockProjectModel GetProject(string name)
        {
            return projects[name];
        }

        public List<IProjectModel> FindProjectsWithResponsibility(string responsibility)
        {
            return FindProjectsWithResponsibility(new string[] { responsibility });
        }

        public List<IProjectModel> FindProjectsWithResponsibility(string[] responsibilities)
        {
            List<IProjectModel> foundProjects = new List<IProjectModel>();

            if (projects != null)
            {
                foreach (MockProjectModel project in projects.Values)
                {
                    foreach (string responsibility in responsibilities)
                    {
                        if (!foundProjects.Contains(project) && project.Responsibilities != null)
                        {
                            foreach (string projectResponsability in project.Responsibilities)
                            {
                                if (projectResponsability == responsibility)
                                {
                                    foundProjects.Add(project);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return foundProjects;
        }

        public int LongestItemPath
        {
            get { return longestItemPath; }
            set { longestItemPath = value; }
        }
    }
}

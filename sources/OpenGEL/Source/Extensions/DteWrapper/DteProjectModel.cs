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
using Microsoft.Practices.RecipeFramework.Library;
using Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell.Design;
using Microsoft.VisualStudio.Shell.Interop;
using System.Diagnostics;
using System.IO;

namespace Microsoft.Practices.RecipeFramework.Extensions.DteWrapper
{
    /// <summary>
    /// Implementation of the IProjectModel interface that talks to the
    /// visual studio DTE object model.
    /// </summary>
    public class DteProjectModel : IProjectModel
    {
        private Project project;
        private IServiceProvider serviceProvider;
        private ITypeResolutionService typeResolutionService;

        /// <summary>
        /// Returns the actual DTE project
        /// </summary>
        public object Project
        {
            get { return this.project; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DteProjectModel"/> class.
        /// </summary>
        /// <param name="project">The current project.</param>
        /// <param name="serviceProvider"></param>
        public DteProjectModel(Project project, IServiceProvider serviceProvider)
        {
            this.project = project;
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Returns true if the file is in the current project.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public bool ProjectContainsFile(string filename)
        {
            ProjectItem itemFound = DteHelperEx.FindItemByName(project.ProjectItems, filename, true);
            return (itemFound != null);
        }

        /// <summary>
        /// Returns an instance of <see cref="ITypeResolutionService"/> for the project scope
        /// </summary>
        public ITypeResolutionService TypeResolutionService
        {
            get
            {
                if (typeResolutionService == null)
                {
                    DynamicTypeService typeService = (DynamicTypeService)serviceProvider.GetService(typeof(DynamicTypeService));
                    Debug.Assert(typeService != null, "No dynamic type service registered.");

                    IVsHierarchy hier = DteHelper.GetCurrentSelection(serviceProvider);
                    Debug.Assert(hier != null, "No active hierarchy is selected.");

                    typeResolutionService = typeService.GetTypeResolutionService(hier);
                    Debug.Assert(typeResolutionService != null, "No type resolution service is returned");
                }

                return typeResolutionService;
            }
        }

        /// <summary>
        /// A list of types on the project
        /// </summary>
        public IList<ITypeModel> Types
        {
            get
            {
                List<ITypeModel> types = new List<ITypeModel>();
                foreach (CodeElement codeElement in EnumerateCodeTypes(project.CodeModel.CodeElements))
                {
                    if (codeElement.IsCodeType &&
                        codeElement.InfoLocation == vsCMInfoLocation.vsCMInfoLocationProject)
                    {
                        types.Add(new DteTypeModel((CodeType)codeElement));
                    }
                }
                return types;
            }
        }

        /// <summary>
        /// Returns a list of folders for the project
        /// </summary>
        public IList<IProjectItemModel> Folders
        {
            get
            {
                List<IProjectItemModel> folders = new List<IProjectItemModel>();
                string projectPath = Path.GetDirectoryName(project.Properties.Item("FullPath").Value.ToString());

                foreach (ProjectItem item in project.ProjectItems)
                {
                    folders.AddRange(RecursiveGetFolders(item));
                }
                return folders;                
            }
        }

        private IList<IProjectItemModel> RecursiveGetFolders(ProjectItem projectItem)
        {
            List<IProjectItemModel> items = new List<IProjectItemModel>();

            if (Directory.Exists(projectItem.Properties.Item("FullPath").Value.ToString()))
            {
                items.Add(new DteProjectItemModel(projectItem));

                foreach (ProjectItem item in projectItem.ProjectItems)
                {
                    items.AddRange(RecursiveGetFolders(item));
                }
            }

            return items;
        }
        /// <summary>
        /// Returns true if the project is a website project
        /// </summary>
        public bool IsWebProject
        {
            get
            {
                return DteHelper.IsWebProject(project);
            }
        }

        private IEnumerable<CodeType> EnumerateCodeTypes(CodeElements elements)
        {
            foreach (CodeElement codeElement in elements)
            {
                if (codeElement.IsCodeType)
                {
                    yield return (CodeType)codeElement;
                }
                else if (codeElement.Kind == vsCMElement.vsCMElementNamespace)
                {
                    foreach (CodeType codeType in EnumerateCodeTypes(((CodeNamespace)codeElement).Members))
                    {
                        yield return codeType;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the project name
        /// </summary>
        /// <returns>Project name</returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Returns the name of the project
        /// </summary>
        public string Name
        {
            get 
            {
                string projectName;
                if (this.IsWebProject && this.project.Name.Contains(@"\"))
                {
                    string[] parts = this.project.Name.Split('\\');
                    projectName = parts[parts.Length - 2];
                }
                else
                {
                    projectName = this.project.Name;
                }
                return projectName;
             }
        }

        /// <summary>
        /// Returns the assembly name of the project
        /// </summary>        
        public string AssemblyName
        {
            get 
            {
                string assemblyName;
                if (this.IsWebProject && this.project.Name.Contains(@"\"))
                {
                    string[] parts = this.project.Name.Split('\\');
                    assemblyName = parts[parts.Length - 2];
                }
                else
                {
                    assemblyName = this.project.Properties.Item("AssemblyName").Value.ToString();
                }
                return assemblyName;                
            }
        }

        /// <summary>
        /// Returns the physical path of the project
        /// </summary>
        public string ProjectPath
        {
            get { return this.project.Properties.Item("FullPath").Value.ToString(); }
        }
    }
}

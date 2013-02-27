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
#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE;
using System.Xml;
using EnvDTE80;
using System.IO;
using Microsoft.Practices.RecipeFramework.Extensions.Common.Templates.Helpers;
using VsWebSite;
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.Common.Templates
{
    /// <summary>
    /// The VS solution template exporter.
    /// </summary>
    public class VSSolutionTemplateExporter : IVSTemplateExporter
    {
        #region Fields
        private string outputProjectsDirectory;
        #endregion

        #region Properties
        private Solution solution;

        /// <summary>
        /// Gets or sets the solution.
        /// </summary>
        /// <value>The solution.</value>
        public Solution Solution
        {
            get { return solution; }
            set { solution = value; }
        }

        private string outputDirectory;

        /// <summary>
        /// Gets or sets the output directory.
        /// </summary>
        /// <value>The output directory.</value>
        public string OutputDirectory
        {
            get { return outputDirectory; }
            set { outputDirectory = value; }
        }

        private string projectsDirectory;

        /// <summary>
        /// Gets or sets the projects directory.
        /// </summary>
        /// <value>The projects directory.</value>
        public string ProjectsDirectory
        {
            get { return projectsDirectory; }
            set { projectsDirectory = value; }
        }

        private string solutionTemplateIconFile;

        /// <summary>
        /// Gets or sets the solution template icon file.
        /// </summary>
        /// <value>The solution template icon file.</value>
        public string SolutionTemplateIconFile
        {
            get { return solutionTemplateIconFile; }
            set { solutionTemplateIconFile = value; }
        }

        private string projectsTemplateIconFile;

        /// <summary>
        /// Gets or sets the projects template icon file.
        /// </summary>
        /// <value>The projects template icon file.</value>
        public string ProjectsTemplateIconFile
        {
            get { return projectsTemplateIconFile; }
            set { projectsTemplateIconFile = value; }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VSSolutionTemplateExporter"/> class.
        /// </summary>
        /// <param name="solution">The solution.</param>
        /// <param name="outputDirectory">The output directory.</param>
        /// <param name="projectsDirectory">The projects directory.</param>
        /// <param name="solutionTemplateIconFile">The solution template icon file.</param>
        /// <param name="projectsTemplateIconFile">The projects template icon file.</param>
        public VSSolutionTemplateExporter(Solution solution, string outputDirectory, string projectsDirectory, string solutionTemplateIconFile, string projectsTemplateIconFile)
        {
            this.solution = solution;
            this.OutputDirectory = outputDirectory;
            this.ProjectsDirectory = projectsDirectory;
            this.solutionTemplateIconFile = solutionTemplateIconFile;
            this.projectsTemplateIconFile = projectsTemplateIconFile;
            outputProjectsDirectory = Path.Combine(outputDirectory, projectsDirectory);
        }
        #endregion

        #region Public Implementation

        /// <summary>
        /// Exports this instance.
        /// </summary>
        public void Export()
        {
            string solutionName = this.Solution.Properties.Item("Name").Value.ToString();

            //Clean or create output directory
            VSTemplateExporterHelper.CreateDirectoryIfNotExists(this.OutputDirectory);

            //Copy solution template icon file
            string outputSolutionTemplateIconFile = Path.Combine(
                                        this.OutputDirectory, 
                                        Path.GetFileName(this.solutionTemplateIconFile));
            // Check if we need to copy to output or not
            if (!this.solutionTemplateIconFile.Equals(outputSolutionTemplateIconFile, 
                StringComparison.InvariantCultureIgnoreCase))
            {
                File.Copy(this.solutionTemplateIconFile, outputSolutionTemplateIconFile, true);
            }

            VSSolutionTemplate vsTemplate = new VSSolutionTemplate();
            vsTemplate.Name = solutionName;
            vsTemplate.Description = solutionName;
            vsTemplate.DefaultName = solutionName;
            vsTemplate.Icon = Path.GetFileName(this.SolutionTemplateIconFile);

            XmlNode rootNode = vsTemplate.CreateRootNode();
            XmlNode templateContentNode = vsTemplate.CreateTemplateContentNode();
            XmlNode projectCollectionNode = vsTemplate.CreateProjecCollectiontNode();

            foreach(Project project in this.Solution.Projects)
            {
                //Add ProjectTemplateLink and SolutionFolder nodes
                //Create project templates
                AddAssetsToTemplate(project, vsTemplate, projectCollectionNode);
            }

            rootNode.AppendChild(vsTemplate.CreateTemplateDataNode());
            templateContentNode.AppendChild(projectCollectionNode);
            rootNode.AppendChild(templateContentNode);
            rootNode.AppendChild(vsTemplate.CreateWizardExtensionNode());
            rootNode.AppendChild(vsTemplate.CreateWizardDataNode());

            //Save solution template
            vsTemplate.Save(
                this.OutputDirectory,
                string.Concat(solutionName,VSTemplateExporterHelper.TEMPLATE_EXTENSION));
        }
        #endregion

        #region Private Implementation
        /// <summary>
        /// Adds the project to template.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="vsTemplate">The vs template.</param>
        /// <param name="contextNode">The context node.</param>
        private void AddAssetsToTemplate(Project project, VSSolutionTemplate vsTemplate, XmlNode contextNode)
        {
            if(project.Object is SolutionFolder)
            {
                //Add SolutionFolder node
                SolutionFolder solutionFolder = project.Object as SolutionFolder;
                XmlNode solutionFolderNode = vsTemplate.CreateSolutionFolderNode(solutionFolder.Parent.Name);
                contextNode.AppendChild(solutionFolderNode);

                foreach(ProjectItem item in project.ProjectItems)
                {
                    if(item.Object != null &&
                        (item.Object is Project ||
                        ((Project)item.Object).Object is SolutionFolder))
                    {
                        AddAssetsToTemplate((Project)item.Object, vsTemplate, solutionFolderNode);
                    }
                }
            }
            else
            {
                string projectName;

                if(project.Object is VSWebSite)
                {
                    projectName = VSTemplateExporterHelper.GetProjectName(project.UniqueName);
                }
                else
                {
                    projectName = project.Name;
                }

                string relativeTemplatePath = Path.Combine
                                                (
                                                    Path.Combine(this.ProjectsDirectory, projectName),
                                                    string.Concat(projectName, VSTemplateExporterHelper.TEMPLATE_EXTENSION)
                                                );

                XmlNode projectTemplateLinkNode = vsTemplate.CreateProjectTemplateLinkNode(projectName, relativeTemplatePath);

                contextNode.AppendChild(projectTemplateLinkNode);

                //Create project template
                VSProjectTemplateExporter webProjectExporter = new VSProjectTemplateExporter(
                        project,
                        this.projectsTemplateIconFile,
                        outputProjectsDirectory);

                webProjectExporter.Export();
            }
        }
        #endregion
    }
}
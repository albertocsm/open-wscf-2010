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
using System.Xml;
using System.IO;
using Microsoft.Practices.RecipeFramework.Extensions.Common.Templates.Helpers;
using EnvDTE;
using VsWebSite;
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.Common.Templates
{
    /// <summary>
    /// Template exporter.
    /// </summary>
    public class VSProjectTemplateExporter : IVSTemplateExporter
    {
        #region Constants

        private const string WebProjectExtension = ".webproj";
        private const string InternalFolderName = "Generated___Files";
        private const string ExtensionAttribute = "Extension";

        #endregion

        #region Fields
        private string projectFileName;
        private string projectName;
        private bool isWebProject;
        #endregion

        #region Properties
        private Project project;

        /// <summary>
        /// Gets or sets the project.
        /// </summary>
        /// <value>The project.</value>
        public Project Project
        {
            get { return project; }
            set { project = value; }
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

        private string projectTemplateIconFile;

        /// <summary>
        /// Gets or sets the projects template icon file.
        /// </summary>
        /// <value>The projects template icon file.</value>
        public string ProjectTemplateIconFile
        {
            get { return projectTemplateIconFile; }
            set { projectTemplateIconFile = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="T:VSWebProjectTemplateExporter"/> class.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="projectTemplateIconFile">The project template icon file.</param>
        /// <param name="outputDirectory">The output directory.</param>
        public VSProjectTemplateExporter(Project project, string projectTemplateIconFile, string outputDirectory)
        {
            this.Project = project;
            this.projectTemplateIconFile = projectTemplateIconFile;
            this.outputDirectory = outputDirectory;
            isWebProject = (project.Object is VSWebSite);

            if(isWebProject)
            {
                this.projectName = VSTemplateExporterHelper.GetProjectName(project.UniqueName);
                this.projectFileName = Path.Combine(project.FullName, this.projectName + WebProjectExtension);
            }
            else
            {
                this.projectName = project.Name;
                this.projectFileName = project.FullName;
            }
        }
        #endregion

        #region Public Implementation
        /// <summary>
        /// Exports this instance.
        /// </summary>
        public void Export()
        {
            string sourceBaseDir = Path.GetDirectoryName(this.projectFileName);
            string destinationBaseDir = Path.Combine(this.OutputDirectory, this.projectName);

            VSTemplateExporterHelper.CreateDirectoryIfNotExists(destinationBaseDir);

            if(isWebProject)
            {
                //Create csproj file because it doesnt exists in WebProjects
                File.Create(Path.Combine(destinationBaseDir, Path.GetFileName(this.projectFileName)));
            }
            else
            {
                CopyProjectFile(this.projectFileName, destinationBaseDir);
            }

            //Copy project template icon file
            File.Copy(
                this.projectTemplateIconFile,
                Path.Combine(
                    destinationBaseDir,
                    Path.GetFileName(this.projectTemplateIconFile)),
                    true);

            VSProjectTemplate vsTemplate = new VSProjectTemplate();
            vsTemplate.Name = this.projectName;
            vsTemplate.Description = this.projectName;
            
            if(isWebProject)
            {
                vsTemplate.ProjectType = ProjectType.Web;
                vsTemplate.ProjectSubType = ProjectType.CSharp;
            }
            else
            {
                vsTemplate.ProjectType = ProjectType.CSharp;
                vsTemplate.ProjectSubType = ProjectType.None;
            }

            vsTemplate.DefaultName = this.projectName;
            vsTemplate.Icon = Path.GetFileName(this.ProjectTemplateIconFile);

            XmlNode rootNode = vsTemplate.CreateRootNode();
            XmlNode templateContentNode = vsTemplate.CreateTemplateContentNode();
            XmlNode projectNode = vsTemplate.CreateProjectNode(Path.GetFileName(this.projectFileName));

            //Add ProjectItem and ProjectItem nodes
            AddAssetsToTemplate(this.Project.ProjectItems, vsTemplate, projectNode, sourceBaseDir, destinationBaseDir);

            rootNode.AppendChild(vsTemplate.CreateTemplateDataNode());
            templateContentNode.AppendChild(projectNode);
            rootNode.AppendChild(templateContentNode);
            rootNode.AppendChild(vsTemplate.CreateWizardExtensionNode());
            rootNode.AppendChild(vsTemplate.CreateWizardDataNode());

            vsTemplate.Save(
                destinationBaseDir,
                string.Concat(
                    this.projectName,
                    VSTemplateExporterHelper.TEMPLATE_EXTENSION));
        }

        private void AddAssetsToTemplate(ProjectItems projectItems, VSProjectTemplate vsTemplate, XmlNode contextNode, string sourceBaseDir, string destinationBaseDir)
        {
            if (projectItems == null)
            {
                return;
            }

            foreach(ProjectItem item in projectItems)
            {
                bool isFolder = true;
                // if we don't find the ExtensionAttribute property or the value is null, then this is a folder
                foreach (Property property in item.Properties)
                {
                    if (string.Compare(property.Name, ExtensionAttribute, StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        isFolder = (property.Value == null);
                        break;
                    }
                }

                if(isFolder)
                {
                    if(item.Name != InternalFolderName)
                    {
                        string sourcePath = Path.Combine(sourceBaseDir, item.Name);
                        string destPath = Path.Combine(destinationBaseDir, item.Name);

                        //Create Folder
                        VSTemplateExporterHelper.CreateDirectoryIfNotExists(destPath);

                        //Add Folder node
                        XmlNode folderNode = vsTemplate.CreateFolderNode(item.Name);
                        contextNode.AppendChild(folderNode);

                        AddAssetsToTemplate(
                            item.ProjectItems, 
                            vsTemplate, 
                            folderNode, 
                            sourcePath, 
                            destPath);
                    }
                }
                else
                {
                    if (!item.Name.EndsWith(WebProjectExtension, StringComparison.InvariantCultureIgnoreCase))
                    {
                        string sourcePath = Path.Combine(sourceBaseDir, item.Name);
                        string destPath = Path.Combine(destinationBaseDir, item.Name);

                        //Add ProjectItem node

                        XmlNode itemNode =
                            vsTemplate.CreateProjectItemNode(
                                item.Name,
                                ReplaceableExtension.IsReplaceable(sourcePath));

                        contextNode.AppendChild(itemNode);

                        if(isWebProject)
                        {                            
                            File.Copy(sourcePath, destPath, true);
                            //TODO: Verify if we need to Replace project variables
                        }
                        else
                        {
                            if(item.FileCodeModel != null)
                            {
                                string fileContent = 
                                    VSTemplateExporterHelper.ReplaceVariables(
                                        sourcePath, 
                                        this.Project.Properties.Item("DefaultNamespace").Value.ToString());

                                File.WriteAllText(destPath, fileContent);
                            }
                            else
                            {
                                if (File.Exists(sourcePath))
                                {
                                    File.Copy(sourcePath, destPath, true);
                                }
                                else if (Directory.Exists(sourcePath))
                                {
                                    // Copy all files in this folder
                                    foreach(string file in Directory.GetFiles(sourcePath, "*.*", SearchOption.TopDirectoryOnly))
                                    {
                                        File.Copy(file, Path.Combine(destPath, Path.GetFileName(file)), true);
                                    }
                                }
                            }                        
                        }

                        //Web project items doesn't have subitems
                        if(!isWebProject)
                        {
                            AddAssetsToTemplate(item.ProjectItems,
                                vsTemplate,
                                contextNode,
                                Path.GetDirectoryName(sourcePath),
                                Path.GetDirectoryName(destPath));
                        }
                    }
                }
            }
        }
        #endregion

        #region Private Implementation
        /// <summary>
        /// Copies the project file.
        /// </summary>
        /// <param name="projectFileName">Name of the project file.</param>
        /// <param name="destinationBaseDir">The destination base dir.</param>
        private void CopyProjectFile(string projectFileName, string destinationBaseDir)
        {
            const string ROOTNAMESPACE = "$safeprojectname$";
            const string ASSEMBLYNAME = "$safeprojectname$";
            const string VSPROJECTNS = "http://schemas.microsoft.com/developer/msbuild/2003";

            XmlDocument projectXml = new XmlDocument();
            XmlNamespaceManager manager = new XmlNamespaceManager(projectXml.NameTable);
            manager.AddNamespace("ns", VSPROJECTNS);

            projectXml.Load(projectFileName);
            projectXml.SelectSingleNode("//ns:PropertyGroup/ns:RootNamespace", manager).InnerText = ROOTNAMESPACE;
            projectXml.SelectSingleNode("//ns:PropertyGroup/ns:AssemblyName", manager).InnerText = ASSEMBLYNAME;

            projectXml.Save(
                Path.Combine(
                    destinationBaseDir,
                    Path.GetFileName(this.projectFileName)));
        }
        #endregion
    }
}
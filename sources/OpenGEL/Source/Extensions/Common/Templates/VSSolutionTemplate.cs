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
using System.Globalization;
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.Common.Templates
{
    internal class VSSolutionTemplate
    {
        #region Fields
        private XmlDocument vsTemplate; 
        #endregion

        #region Properties
        private string name;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string description = "No description available";
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private ProjectType projectType = ProjectType.CSharp;

        /// <summary>
        /// Gets or sets the type of the project.
        /// </summary>
        /// <value>The type of the project.</value>
        public ProjectType ProjectType
        {
            get { return projectType; }
            set { projectType = value; }
        }

        private int sortOrder = 10;

        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        /// <value>The sort order.</value>
        public int SortOrder
        {
            get { return sortOrder; }
            set { sortOrder = value; }
        }

        private bool createNewFolder = true;

        /// <summary>
        /// Gets or sets a value indicating whether [create new folder].
        /// </summary>
        /// <value><c>true</c> if [create new folder]; otherwise, <c>false</c>.</value>
        public bool CreateNewFolder
        {
            get { return createNewFolder; }
            set { createNewFolder = value; }
        }

        private string defaultName;

        /// <summary>
        /// Gets or sets the name of the default.
        /// </summary>
        /// <value>The name of the default.</value>
        public string DefaultName
        {
            get { return defaultName; }
            set { defaultName = value; }
        }

        private bool provideDefaultName = true;

        /// <summary>
        /// Gets or sets a value indicating whether [provide default name].
        /// </summary>
        /// <value><c>true</c> if [provide default name]; otherwise, <c>false</c>.</value>
        public bool ProvideDefaultName
        {
            get { return provideDefaultName; }
            set { provideDefaultName = value; }
        }

        private string icon = "SolutionTemplateIcon.ico";

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>The icon.</value>
        public string Icon
        {
            get { return icon; }
            set { icon = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="T:VSSolutionTemplate"/> class.
        /// </summary>
        public VSSolutionTemplate()
        {
            vsTemplate = new XmlDocument();
        }
        #endregion

        #region internal Implementation
        /// <summary>
        /// Creates the root node.
        /// </summary>
        /// <returns></returns>
        internal XmlNode CreateRootNode()
        {
            const string VSTEMPLATE_NODE = "VSTemplate";
            const string VSTEMPLATE_NS = "http://schemas.microsoft.com/developer/vstemplate/2005";
            const string VERSION_ATT_NAME = "Version";
            const string VERSION_ATT_VALUE = "2.0.0";
            const string TYPE_ATT_NAME = "Type";
            const string TYPE_ATT_VALUE = "ProjectGroup";

            XmlNode root = vsTemplate.CreateElement(VSTEMPLATE_NODE, VSTEMPLATE_NS);
            root.Attributes.Append(VSTemplateExporterHelper.CreateAttribute(vsTemplate, VERSION_ATT_NAME, VERSION_ATT_VALUE));
            root.Attributes.Append(VSTemplateExporterHelper.CreateAttribute(vsTemplate, TYPE_ATT_NAME, TYPE_ATT_VALUE));
            vsTemplate.AppendChild(root);
            return root;
        }

        /// <summary>
        /// Creates the template data node.
        /// </summary>
        /// <returns></returns>
        internal XmlNode CreateTemplateDataNode()
        {
            const string TEMPLATEDATA_NODE = "TemplateData";
            const string NAME_ATT = "Name";
            const string DESCRIPTION_ATT = "Description";
            const string PROJECTTYPE_ATT = "ProjectType";
            const string SORTORDER_ATT = "SortOrder";
            const string CREATENEWFOLDER_ATT = "CreateNewFolder";
            const string DEFAULTNAME_ATT_NAME = "DefaultName";
            const string PROVIDEDEFAULTNAME_ATT = "ProvideDefaultName";
            const string ICON_ATT = "Icon";

            XmlNode templateDataNode = vsTemplate.CreateElement(TEMPLATEDATA_NODE);

            templateDataNode.AppendChild(VSTemplateExporterHelper.CreateNode(vsTemplate, NAME_ATT, this.Name));
            templateDataNode.AppendChild(VSTemplateExporterHelper.CreateNode(vsTemplate, DESCRIPTION_ATT, this.Description));
            templateDataNode.AppendChild(VSTemplateExporterHelper.CreateNode(vsTemplate, PROJECTTYPE_ATT, this.ProjectType.ToString()));
            templateDataNode.AppendChild(VSTemplateExporterHelper.CreateNode(vsTemplate, SORTORDER_ATT, this.SortOrder.ToString(NumberFormatInfo.InvariantInfo)));
            templateDataNode.AppendChild(VSTemplateExporterHelper.CreateNode(vsTemplate, ICON_ATT, this.Icon));
            templateDataNode.AppendChild(VSTemplateExporterHelper.CreateNode(vsTemplate, CREATENEWFOLDER_ATT, this.CreateNewFolder.ToString().ToLowerInvariant()));
            templateDataNode.AppendChild(VSTemplateExporterHelper.CreateNode(vsTemplate, DEFAULTNAME_ATT_NAME, this.DefaultName));
            templateDataNode.AppendChild(VSTemplateExporterHelper.CreateNode(vsTemplate, PROVIDEDEFAULTNAME_ATT, this.ProvideDefaultName.ToString().ToLowerInvariant()));

            return templateDataNode;
        }

        /// <summary>
        /// Creates the template content node.
        /// </summary>
        /// <returns></returns>
        internal XmlNode CreateTemplateContentNode()
        {
            const string TEMPLATECONTENT_NODE = "TemplateContent";

            XmlNode templateContent = VSTemplateExporterHelper.CreateNode(vsTemplate, TEMPLATECONTENT_NODE);

            return templateContent;
        }

        /// <summary>
        /// Creates the projec collectiont node.
        /// </summary>
        /// <returns></returns>
        internal XmlNode CreateProjecCollectiontNode()
        {
            const string PROJECTCOLLECTION_NODE = "ProjectCollection";

            XmlNode projectCollection = VSTemplateExporterHelper.CreateNode(vsTemplate, PROJECTCOLLECTION_NODE);

            return projectCollection;
        }

        /// <summary>
        /// Creates the solution folder node.
        /// </summary>
        /// <param name="solutionFolderName">Name of the solution folder.</param>
        /// <returns></returns>
        internal XmlNode CreateSolutionFolderNode(string solutionFolderName)
        {
            const string SOLUTIONFOLDER_NODE = "SolutionFolder";
            const string NAME_ATT = "Name";

            XmlNode solutionFolderNode = VSTemplateExporterHelper.CreateNode(vsTemplate, SOLUTIONFOLDER_NODE);
            solutionFolderNode.Attributes.Append(VSTemplateExporterHelper.CreateAttribute(vsTemplate, NAME_ATT, solutionFolderName));

            return solutionFolderNode;
        }

        /// <summary>
        /// Creates the project template link node.
        /// </summary>
        /// <param name="projectName">Name of the project.</param>
        /// <param name="relativeTemplatePath">The relative template path.</param>
        /// <returns></returns>
        internal XmlNode CreateProjectTemplateLinkNode(string projectName, string relativeTemplatePath)
        {
            const string PROJECTTEMPLATELINK_NODE = "ProjectTemplateLink";
            const string PROJECTNAME_ATT = "ProjectName";

            XmlNode projectTemplateLinkNode = VSTemplateExporterHelper.CreateNode(vsTemplate, PROJECTTEMPLATELINK_NODE, relativeTemplatePath);
            projectTemplateLinkNode.Attributes.Append(VSTemplateExporterHelper.CreateAttribute(vsTemplate, PROJECTNAME_ATT, projectName));

            return projectTemplateLinkNode;
        }

        /// <summary>
        /// Creates the wizard extension node.
        /// </summary>
        /// <returns></returns>
        internal XmlNode CreateWizardExtensionNode()
        {
            return VSTemplateExporterHelper.CreateWizardExtensionNode(vsTemplate);
        }

        /// <summary>
        /// Creates the wizard data node.
        /// </summary>
        /// <returns></returns>
        internal XmlNode CreateWizardDataNode()
        {
            return VSTemplateExporterHelper.CreateWizardDataNode(vsTemplate);
        }

        /// <summary>
        /// Saves the specified output directory.
        /// </summary>
        /// <param name="outputDirectory">The output directory.</param>
        /// <param name="templateName">Name of the template.</param>
        internal void Save(string outputDirectory, string templateName)
        {
            string content = vsTemplate.InnerXml;
            content = content.Replace(" xmlns=\"\"", "");
            File.WriteAllText(Path.Combine(outputDirectory, templateName), content);

            //vsTemplate.Save(Path.Combine(outputDirectory, templateName));
        }
        #endregion
    }
}
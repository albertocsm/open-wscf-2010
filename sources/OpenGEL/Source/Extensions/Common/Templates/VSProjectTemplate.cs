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
    internal class VSProjectTemplate
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

        private ProjectType projectType;

        /// <summary>
        /// Gets or sets the type of the project.
        /// </summary>
        /// <value>The type of the project.</value>
        public ProjectType ProjectType
        {
            get { return projectType; }
            set { projectType = value; }
        }

        private ProjectType projectSubType;

        /// <summary>
        /// Gets or sets the type of the sub project.
        /// </summary>
        /// <value>The type of the sub project.</value>
        public ProjectType ProjectSubType
        {
            get { return projectSubType; }
            set { projectSubType = value; }
        }

        private int sortOrder = 100;

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

        private LocationField locationField = LocationField.Enabled;

        /// <summary>
        /// Gets or sets the location field.
        /// </summary>
        /// <value>The location field.</value>
        public LocationField LocationField
        {
            get { return locationField; }
            set { locationField = value; }
        }


        private bool enableLocationBrowseButton = true;

        /// <summary>
        /// Gets or sets a value indicating whether [enable location browse button].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [enable location browse button]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableLocationBrowseButton
        {
            get { return enableLocationBrowseButton; }
            set { enableLocationBrowseButton = value; }
        }

        private string icon = "ProjectTemplateIcon.ico";

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
        /// Initializes a new instance of the <see cref="T:VSProjectTemplate"/> class.
        /// </summary>
        public VSProjectTemplate()
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
            const string TYPE_ATT_VALUE = "Project";

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
            const string PROJECTSUBTYPE_ATT = "ProjectSubType";
            const string SORTORDER_ATT = "SortOrder";
            const string CREATENEWFOLDER_ATT = "CreateNewFolder";
            const string DEFAULTNAME_ATT_NAME = "DefaultName";
            const string PROVIDEDEFAULTNAME_ATT = "ProvideDefaultName";
            const string LOCATIONFIELD_ATT_NAME = "LocationField";
            const string ENABLELOCATIONBROWSEBUTTON_ATT = "EnableLocationBrowseButton";
            const string ICON_ATT = "Icon";

            XmlNode templateDataNode = vsTemplate.CreateElement(TEMPLATEDATA_NODE, string.Empty);

            templateDataNode.AppendChild(VSTemplateExporterHelper.CreateNode(vsTemplate, NAME_ATT, this.Name));
            templateDataNode.AppendChild(VSTemplateExporterHelper.CreateNode(vsTemplate, DESCRIPTION_ATT, this.Description));
            templateDataNode.AppendChild(VSTemplateExporterHelper.CreateNode(vsTemplate, PROJECTTYPE_ATT, this.ProjectType.ToString()));
            
            if(this.projectSubType != ProjectType.None)
            {
                templateDataNode.AppendChild(VSTemplateExporterHelper.CreateNode(vsTemplate, PROJECTSUBTYPE_ATT, this.projectSubType.ToString()));
            }
            
            templateDataNode.AppendChild(VSTemplateExporterHelper.CreateNode(vsTemplate, SORTORDER_ATT, this.SortOrder.ToString(NumberFormatInfo.InvariantInfo)));
            templateDataNode.AppendChild(VSTemplateExporterHelper.CreateNode(vsTemplate, CREATENEWFOLDER_ATT, this.CreateNewFolder.ToString(CultureInfo.InvariantCulture).ToLowerInvariant()));
            templateDataNode.AppendChild(VSTemplateExporterHelper.CreateNode(vsTemplate, DEFAULTNAME_ATT_NAME, this.DefaultName));
            templateDataNode.AppendChild(VSTemplateExporterHelper.CreateNode(vsTemplate, PROVIDEDEFAULTNAME_ATT, this.ProvideDefaultName.ToString(CultureInfo.InvariantCulture).ToLowerInvariant()));
            templateDataNode.AppendChild(VSTemplateExporterHelper.CreateNode(vsTemplate, LOCATIONFIELD_ATT_NAME, this.LocationField.ToString()));
            templateDataNode.AppendChild(VSTemplateExporterHelper.CreateNode(vsTemplate, ENABLELOCATIONBROWSEBUTTON_ATT, this.EnableLocationBrowseButton.ToString(CultureInfo.InvariantCulture).ToLowerInvariant()));
            templateDataNode.AppendChild(VSTemplateExporterHelper.CreateNode(vsTemplate, ICON_ATT, this.Icon));

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
        /// Creates the project node.
        /// </summary>
        /// <returns></returns>
        internal XmlNode CreateProjectNode(string projectFileName)
        {
            const string PROJECT_NODE = "Project";
            const string TARGETFILENAME_ATT = "TargetFileName";
            const string FILE_ATT = "File";
            const string REPLACEPARAMETERS_ATT_NAME = "ReplaceParameters";
            const string REPLACEPARAMETERS_ATT_VALUE = "true";

            XmlNode projectContent = VSTemplateExporterHelper.CreateNode(vsTemplate, PROJECT_NODE);

            projectContent.Attributes.Append(VSTemplateExporterHelper.CreateAttribute(vsTemplate, TARGETFILENAME_ATT, projectFileName));
            projectContent.Attributes.Append(VSTemplateExporterHelper.CreateAttribute(vsTemplate, FILE_ATT, projectFileName));
            projectContent.Attributes.Append(VSTemplateExporterHelper.CreateAttribute(vsTemplate, REPLACEPARAMETERS_ATT_NAME, REPLACEPARAMETERS_ATT_VALUE));

            return projectContent;
        }

        /// <summary>
        /// Creates the folder node.
        /// </summary>
        /// <param name="folderName">Name of the folder.</param>
        /// <returns></returns>
        internal XmlNode CreateFolderNode(string folderName)
        {
            const string FOLDER_NODE = "Folder";
            const string NAME_ATT = "Name";
            const string TARGETFOLDERNAME_ATT = "TargetFolderName";

            XmlNode folderNode = VSTemplateExporterHelper.CreateNode(vsTemplate, FOLDER_NODE);
            folderNode.Attributes.Append(VSTemplateExporterHelper.CreateAttribute(vsTemplate, NAME_ATT, folderName));
            folderNode.Attributes.Append(VSTemplateExporterHelper.CreateAttribute(vsTemplate, TARGETFOLDERNAME_ATT, folderName));
            return folderNode;
        }

        /// <summary>
        /// Creates the project item node.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="replaceParameters">if set to <c>true</c> [replace parameters].</param>
        /// <returns></returns>
        internal XmlNode CreateProjectItemNode(string item, bool replaceParameters)
        {
            const string PROJECTITEM_NODE = "ProjectItem";
            const string REPLACEPARAMETERS_ATT_NAME = "ReplaceParameters";
            const string TARGETFILENAME_ATT = "TargetFileName";

            XmlNode projectItemNode = VSTemplateExporterHelper.CreateNode(vsTemplate, PROJECTITEM_NODE, item);
            projectItemNode.Attributes.Append(VSTemplateExporterHelper.CreateAttribute(vsTemplate, REPLACEPARAMETERS_ATT_NAME, replaceParameters.ToString().ToLowerInvariant()));
            projectItemNode.Attributes.Append(VSTemplateExporterHelper.CreateAttribute(vsTemplate, TARGETFILENAME_ATT, item));
            return projectItemNode;
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
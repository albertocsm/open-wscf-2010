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
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System.Globalization;
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.Common.Templates.Helpers
{
    internal static class VSTemplateExporterHelper
    {
        #region Constants

        internal const string TEMPLATE_EXTENSION = ".vstemplate";

        #endregion

        #region Internal Implementation
        /// <summary>
        /// Creates the directory if not exists.
        /// </summary>
        /// <param name="directory">The directory.</param>
        internal static void CreateDirectoryIfNotExists(string directory)
        {
            if(!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        /// <summary>
        /// Creates the wizard extension node.
        /// </summary>
        /// <param name="vsTemplate">The vs template.</param>
        /// <returns></returns>
        internal static XmlNode CreateWizardExtensionNode(XmlDocument vsTemplate)
        {
            const string WIZARDEXTENSION_NODE = "WizardExtension";
            const string ASSEMBLY_NODE = "Assembly";
            const string ASSEMBLY_TEXT = "Microsoft.Practices.RecipeFramework.VisualStudio, Version=1.0.51206.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
            const string FULLCLASSNAME_NODE = "FullClassName";
            const string FULLCLASSNAME_TEXT = "Microsoft.Practices.RecipeFramework.VisualStudio.Templates.UnfoldTemplate";

            XmlNode wizardExtension = CreateNode(vsTemplate, WIZARDEXTENSION_NODE);
            XmlNode assembly = CreateNode(vsTemplate, ASSEMBLY_NODE);
            XmlNode fullClassName = CreateNode(vsTemplate, FULLCLASSNAME_NODE);

            assembly.InnerText = ASSEMBLY_TEXT;
            fullClassName.InnerText = FULLCLASSNAME_TEXT;

            wizardExtension.AppendChild(assembly);
            wizardExtension.AppendChild(fullClassName);

            return wizardExtension;
        }

        /// <summary>
        /// Creates the wizard data node.
        /// </summary>
        /// <param name="vsTemplate">The vs template.</param>
        /// <returns></returns>
        internal static XmlNode CreateWizardDataNode(XmlDocument vsTemplate)
        {
            const string WIZARDDATA_NODE = "WizardData";
            const string TEMPLATE_NODE = "Template";
            const string TEMPLATE_NS = "http://schemas.microsoft.com/pag/gax-template";
            const string REFERENCES_NODE = "References";
            const string SCHEMAVERSION_ATT_NAME = "SchemaVersion";
            const string SCHEMAVERSION_ATT_VALUE = "1.0";

            XmlNode wizardData = CreateNode(vsTemplate, WIZARDDATA_NODE);
            XmlNode template = CreateNode(vsTemplate, TEMPLATE_NODE, TEMPLATE_NS, string.Empty);
            template.Attributes.Append(CreateAttribute(vsTemplate, SCHEMAVERSION_ATT_NAME, SCHEMAVERSION_ATT_VALUE));

            XmlNode references = CreateNode(vsTemplate, REFERENCES_NODE);

            wizardData.AppendChild(template);
            wizardData.AppendChild(references);

            return wizardData;
        }

        /// <summary>
        /// Creates the node.
        /// </summary>
        /// <param name="vsTemplate">The vs template.</param>
        /// <param name="nodeName">Name of the node.</param>
        /// <returns></returns>
        internal static XmlNode CreateNode(XmlDocument vsTemplate, string nodeName)
        {
            return CreateNode(vsTemplate, nodeName, string.Empty, string.Empty);
        }

        /// <summary>
        /// Creates the node.
        /// </summary>
        /// <param name="vsTemplate">The vs template.</param>
        /// <param name="nodeName">Name of the node.</param>
        /// <param name="nodeInnerText">The node inner text.</param>
        /// <returns></returns>
        internal static XmlNode CreateNode(XmlDocument vsTemplate, string nodeName, string nodeInnerText)
        {
            return CreateNode(vsTemplate, nodeName, string.Empty, nodeInnerText);
        }

        /// <summary>
        /// Creates the node.
        /// </summary>
        /// <param name="vsTemplate">The vs template.</param>
        /// <param name="nodeName">Name of the node.</param>
        /// <param name="nodeNamespace">The node namespace.</param>
        /// <param name="nodeInnerText">The node inner text.</param>
        /// <returns></returns>
        internal static XmlNode CreateNode(XmlDocument vsTemplate, string nodeName, string nodeNamespace, string nodeInnerText)
        {
            XmlNode node = vsTemplate.CreateElement(nodeName, nodeNamespace);
            node.InnerText = nodeInnerText;
            return node;
        }

        /// <summary>
        /// Creates the attribute.
        /// </summary>
        /// <param name="vsTemplate">The vs template.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="attributeValue">The attribute value.</param>
        /// <returns></returns>
        internal static XmlAttribute CreateAttribute(XmlDocument vsTemplate, string attributeName, string attributeValue)
        {
            XmlAttribute attribute = vsTemplate.CreateAttribute(attributeName);
            attribute.Value = attributeValue;
            return attribute;
        }

        /// <summary>
        /// Replaces the variables.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="rootNamespace">The root namespace.</param>
        /// <returns></returns>
        internal static string ReplaceVariables(string fileName, string rootNamespace)
        {
            const string variable = "$safeprojectname$";
            string fileContent = File.ReadAllText(fileName);

            Regex regex = new Regex(string.Format(CultureInfo.InvariantCulture, "namespace\\s+{0}", rootNamespace));
            fileContent = regex.Replace(fileContent, string.Format(CultureInfo.InvariantCulture, "namespace {0}", variable));

            fileContent = fileContent.Replace(
                string.Format(CultureInfo.InvariantCulture, "{0}.", rootNamespace),
                string.Format(CultureInfo.InvariantCulture, "{0}.", variable));

            fileContent = fileContent.Replace(
                string.Format(CultureInfo.InvariantCulture, "[assembly: AssemblyTitle(\"{0}\")]", rootNamespace),
                string.Format(CultureInfo.InvariantCulture, "[assembly: AssemblyTitle(\"{0}\")]", variable));

            fileContent = fileContent.Replace(
                string.Format(CultureInfo.InvariantCulture, "[assembly: AssemblyProduct(\"{0}\")]", rootNamespace),
                string.Format(CultureInfo.InvariantCulture, "[assembly: AssemblyProduct(\"{0}\")]", variable));

            return fileContent;
        }

        /// <summary>
        /// Gets the name of the project.
        /// </summary>
        /// <param name="uniqueName">Name of the unique.</param>
        /// <returns></returns>
        internal static string GetProjectName(string uniqueName)
        {
            string response = null;

            if(uniqueName != string.Empty)
            {
                string[] values = uniqueName.Split('\\');
                response = values[values.Length - 1];
            }

            return response;
        }

        #endregion
    }
}

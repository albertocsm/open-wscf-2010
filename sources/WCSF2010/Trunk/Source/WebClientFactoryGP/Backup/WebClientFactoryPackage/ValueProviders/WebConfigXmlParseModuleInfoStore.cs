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
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Configuration;
using System.Configuration;
using System.Web.Configuration;
using System.IO;
using System.Xml;

namespace Microsoft.Practices.WebClientFactory
{

    public class WebConfigXmlParseModuleInfoStore : IModuleInfoStore
    {
        string _baseDirectory;

        private const string NameAttribute = "name";
        private const string AssemblyNameAttribute = "assemblyName";
        private const string VirtualPathAttribute = "virtualPath";
        private const string ModuleElementName = "module";
        private const string XpathCompositeWebModulesNode = "//compositeWeb/modules";
        private const string XpathCompositeWebModuleNode = "//compositeWeb/modules/module";
        private const string QualifiedXpathCompositeWebModulesNode = "//ns:compositeWeb/ns:modules";
        private const string QualifiedXpathCompositeWebModuleNode = "//ns:compositeWeb/ns:modules/ns:module";
        private const string QualifierXpath = "ns";
        private const string NetConfigurationNamespace = "http://schemas.microsoft.com/.NetConfiguration/v2.0";


        public WebConfigXmlParseModuleInfoStore()
            : this(String.Empty)
        {
        }

        public WebConfigXmlParseModuleInfoStore(string baseDirectory)
        {
            _baseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, baseDirectory);
        }

        public ModulesConfigurationSection GetModuleConfigurationSection()
        {
            ModulesConfigurationSection globalSection = new ModulesConfigurationSection();
            PopulateSection(globalSection, _baseDirectory, true);
            return globalSection;
        }

        /// <summary>
        ///  Extra method specific for this store to be used by the GP to modify the Module configuration
        /// </summary>
        /// <param name="moduleConfig"></param>
        public void AddModuleConfigurationElement(ModuleConfigurationElement moduleConfig)
        {
            string[] found = Directory.GetFiles(_baseDirectory, "Web.Config", SearchOption.TopDirectoryOnly);
            if (found.Length > 0)
            {
                SaveConfigurationElement(found[0], moduleConfig);
            }
        }
        
        private void SaveConfigurationElement(string filePath, ModuleConfigurationElement moduleConfig)
        {
            XmlDocument config = new XmlDocument();
            config.Load(filePath);
            XmlNode modulesNode = config.SelectSingleNode(XpathCompositeWebModulesNode);
            XmlElement moduleNode;
            if (modulesNode == null)
            {
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(config.NameTable);
                nsmgr.AddNamespace(QualifierXpath, NetConfigurationNamespace);
                modulesNode = config.SelectSingleNode(QualifiedXpathCompositeWebModulesNode, nsmgr);
                moduleNode = config.CreateElement(ModuleElementName, NetConfigurationNamespace);
            }
            else
            {
                moduleNode = config.CreateElement(ModuleElementName);
            }

            AddAttribute(NameAttribute, config, moduleConfig, moduleNode);
            AddAttribute(AssemblyNameAttribute, config, moduleConfig, moduleNode);
            if (!string.IsNullOrEmpty(moduleConfig.VirtualPath))
            {
                AddAttribute(VirtualPathAttribute, config, moduleConfig, moduleNode);
            }
            modulesNode.AppendChild(moduleNode);
            config.Save(filePath);
        }
       
        private static void AddAttribute(string attributeName, XmlDocument config, ModuleConfigurationElement moduleConfig, XmlElement moduleNode)
        {
            XmlAttribute attribute = config.CreateAttribute(attributeName);
            attribute.Value = moduleConfig.Name;
            moduleNode.Attributes.Append(attribute);
        }

        private void PopulateSection(ModulesConfigurationSection section, string rootDirectory, bool recursive)
        {
            foreach (string fileName in Directory.GetFiles(rootDirectory, "Web.Config", SearchOption.TopDirectoryOnly))
            {
                ModulesConfigurationSection localSection = ParseSection(Path.Combine(rootDirectory, fileName));
                if (localSection != null)
                {
                    foreach (ModuleConfigurationElement module in localSection.Modules)
                    {
                        if (!section.Modules.Contains(module.Name))
                        {
                            section.Modules.Add(module);
                        }
                    }
                }
            }
            if (recursive)
            {
                foreach (string childDirectory in Directory.GetDirectories(rootDirectory))
                {
                    PopulateSection(section, childDirectory, recursive);
                }
            }
        }

        private ModulesConfigurationSection ParseSection(string filePath)
        {
            ModulesConfigurationSection section = new ModulesConfigurationSection();

            XmlDocument config = new XmlDocument();
            config.Load(filePath);
            XmlNodeList moduleList = config.SelectNodes(XpathCompositeWebModuleNode);
            if (moduleList == null || moduleList.Count == 0) 
            {
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(config.NameTable);
                nsmgr.AddNamespace(QualifierXpath, NetConfigurationNamespace);
                moduleList = config.SelectNodes(QualifiedXpathCompositeWebModuleNode, nsmgr);  
            }
            foreach (XmlNode module in moduleList)
            {
                string name = module.Attributes[NameAttribute].Value;
                string assemblyName = module.Attributes[AssemblyNameAttribute].Value;
                string virtualPath = null;
                if (module.Attributes[VirtualPathAttribute] != null)
                {
                    virtualPath = module.Attributes[VirtualPathAttribute].Value;
                }
                ModuleConfigurationElement data = new ModuleConfigurationElement(name, assemblyName, virtualPath);
                section.Modules.Add(data);
            }

            return section;
        }        

    } 
}

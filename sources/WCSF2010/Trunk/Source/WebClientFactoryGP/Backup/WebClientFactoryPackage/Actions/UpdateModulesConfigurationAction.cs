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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using EnvDTE;
using Microsoft.Practices.CompositeWeb.Configuration;

namespace Microsoft.Practices.WebClientFactory.Actions
{
    public class UpdateModulesConfigurationAction : Action
    {
        private Project _webProject;

        [Input(Required = true)]
        public Project WebProject
        {
            get { return _webProject; }
            set { _webProject = value; }
        }

        private Project _moduleProject;

        [Input(Required = true)]
        public Project ModuleProject
        {
            get { return _moduleProject; }
            set { _moduleProject = value; }
        }

        private string _moduleName;

        [Input(Required = true)]
        public string ModuleName
        {
            get { return _moduleName; }
            set { _moduleName = value; }
        }
	
        public override void Execute()
        {
            string webProjectPath = _webProject.Properties.Item("FullPath").Value.ToString();
            string assemlyName = _moduleProject.Properties.Item("AssemblyName").Value.ToString();

            WebConfigXmlParseModuleInfoStore store = new WebConfigXmlParseModuleInfoStore(webProjectPath);
            ModuleConfigurationElement moduleConfig = new ModuleConfigurationElement(_moduleName, assemlyName, null);
            store.AddModuleConfigurationElement(moduleConfig);            
        }

        public override void Undo()
        {
            // Not supported
        }
    }
}

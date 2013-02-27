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

namespace Microsoft.Practices.WebClientFactory.Mappers
{
    public class ModuleInfoMapper
    {        
        public GuidancePackageModuleInfo[] Translate(IModuleInfo[] dependantModules)
        {
            if (dependantModules == null)
            {
                return null;
            }

            GuidancePackageModuleInfo[] moduleInfo = new GuidancePackageModuleInfo[dependantModules.Length];

            for (int i = 0; i < dependantModules.Length; i++)
            {
                moduleInfo[i] = Translate(dependantModules[i]);
            }

            return moduleInfo;
        }

        private GuidancePackageModuleInfo Translate(IModuleInfo moduleInfo)
        {
            DependantModuleInfo dependantModuleInfo = moduleInfo as DependantModuleInfo;

            GuidancePackageModuleInfo wcsfModuleInfo = new GuidancePackageModuleInfo(moduleInfo.Name, moduleInfo.AssemblyName, moduleInfo.VirtualPath);

            if (dependantModuleInfo != null)
            {
                wcsfModuleInfo.Dependencies = dependantModuleInfo.Dependencies;
            }

            return wcsfModuleInfo;
        }
    }
}

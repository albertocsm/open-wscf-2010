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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.CompositeWeb.Configuration;
using Microsoft.Practices.WebClientFactory.Mappers;

namespace WebClientFactoryPackage.Tests.Mappers
{
    [TestClass]
    public class ModuleInfoMapperTestFixture
    {

        [TestMethod]
        public void TranslateDependantModuleInfo()
        {
            DependantModuleInfo dependantModuleInfo = new DependantModuleInfo("Module1", "Module1.dll", "Module1");
            dependantModuleInfo.Dependencies = new ModuleDependency[1];
         
            ModuleDependency dependency = new ModuleDependency();
            dependency.Name = "ModuleDependency";

            dependantModuleInfo.Dependencies[0] = dependency;

            DependantModuleInfo[] modules = new DependantModuleInfo[1];
            modules[0] = dependantModuleInfo;

            GuidancePackageModuleInfo[] guidanceModules = new ModuleInfoMapper().Translate(modules);

            Assert.AreEqual(guidanceModules.Length, modules.Length);
            Assert.AreEqual(guidanceModules[0].Name, dependantModuleInfo.Name);
            Assert.AreEqual(guidanceModules[0].AssemblyName, dependantModuleInfo.AssemblyName);
            Assert.AreEqual(guidanceModules[0].VirtualPath, dependantModuleInfo.VirtualPath);
            Assert.AreEqual(guidanceModules[0].Dependencies.Length, dependantModuleInfo.Dependencies.Length);
            Assert.AreEqual(guidanceModules[0].Dependencies[0].Name, dependantModuleInfo.Dependencies[0].Name);
        }
    }
}

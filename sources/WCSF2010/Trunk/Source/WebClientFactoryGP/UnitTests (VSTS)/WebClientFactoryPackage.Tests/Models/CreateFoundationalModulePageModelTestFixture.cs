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
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.Design;
using Microsoft.Practices.WebClientFactory.CustomWizardPages;
using System.Collections;
using WebClientFactoryPackage.Tests.Support;
using Microsoft.Practices.RecipeFramework.Extensions;
using Microsoft.Practices.RecipeFramework.Extensions.DteWrapper;

namespace WebClientFactoryPackage.Tests.Models
{
    /// <summary>
    /// Summary description for CreateFoundationalModulePagePresenterTestFixture
    /// </summary>
    [TestClass]
    public class CreateFoundationalModulePageModelTestFixture
    {
        public CreateFoundationalModulePageModelTestFixture()
        {
        }

        private IDictionaryService dictionary;
        private MockSolutionModel solutionModel;
        private CreateFoundationalModulePageModel model;


        [TestInitialize]
        public void SetUp()
        {
            dictionary = new MockDictionaryService();
            solutionModel = new MockSolutionModel(null);
            model = new CreateFoundationalModulePageModel(dictionary, solutionModel, null);
        }

        public void SetModelDefaultArguments()
        {
            dictionary.SetValue("WebUIProject",new MockProjectModel());
            dictionary.SetValue("RecipeLanguage", "cs");
        }

        [TestMethod]
        public void ModelIsValidWithValidArguments()
        {
            SetModelDefaultArguments();
            model.ModuleName = "AnyModuleName";

            Assert.IsTrue(model.IsValid);
        }

        [TestMethod]
        public void ModelIsNotValidWhenModuleNameIsNotValidIdentifier()
        {
            SetModelDefaultArguments();
            model.ModuleName = "Not a valid identifier";

            Assert.IsFalse(model.IsValid);
        }
    }
}

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
    public class CreateModulePageModelTestFixture
    {
        public CreateModulePageModelTestFixture()
        {
        }

        private IDictionaryService dictionary;
        private MockSolutionModel solutionModel;
        private CreateModulePageModel model;


        [TestInitialize]
        public void SetUp()
        {
            dictionary = new MockDictionaryService();
            solutionModel = new MockSolutionModel(null);
            model = new CreateModulePageModel(dictionary, solutionModel, null);
        }

        public void SetModelDefaultArguments()
        {
            dictionary.SetValue("RecipeLanguage", "cs");
            dictionary.SetValue("ModuleFolderNameOnWebSite", (new System.IO.DirectoryInfo(@".\Support\MockWebSite")).Name);
            dictionary.SetValue("ModuleName", "AnyModuleName");
        }

        [TestMethod]
        public void ModelIsValidWithValidArguments()
        {
            SetModelDefaultArguments();

            Assert.IsTrue(model.IsValid);
        }

        [TestMethod]
        public void ModelIsNotValidWhenModuleNameIsNotValidIdentifier()
        {
            SetModelDefaultArguments();
            model.ModuleName = "Not a valid identifier";

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void ModelIsNotValidWhenModuleFolderOnWebSiteInvalid()
        {
            SetModelDefaultArguments();
            dictionary.SetValue("ModuleFolderNameOnWebSite", string.Empty);

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void ModelIsNotValidWhenModuleWebFolderHasNumerals()
        {
            SetModelDefaultArguments();
            dictionary.SetValue("ModuleFolderNameOnWebSite", "Module5#1");

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void ModelIsNotValidWhenModuleWebFolderHasATSign()
        {
            SetModelDefaultArguments();
            dictionary.SetValue("ModuleFolderNameOnWebSite", "Module1 @Hotmail");

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void ModelIsNotValidWhenModuleWebFolderHasPercentage()
        {
            SetModelDefaultArguments();
            dictionary.SetValue("ModuleFolderNameOnWebSite", "Module1 %20");

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void ModelIsNotValidWhenModuleWebFolderHasPercentageAtEnd()
        {
            SetModelDefaultArguments();
            dictionary.SetValue("ModuleFolderNameOnWebSite", "Module1 #");

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void ModelIsNotValidWhenModuleWebFolderHasADotAtEnd()
        {
            SetModelDefaultArguments();
            dictionary.SetValue("ModuleFolderNameOnWebSite", "Module1.");

            Assert.IsFalse(model.IsValid);
        }

		[TestMethod]
		public void ModelIsNotValidWhenModuleWebFolderHasAOpenBrace()
		{
			SetModelDefaultArguments();
			dictionary.SetValue("ModuleFolderNameOnWebSite", "Module1{");

			Assert.IsFalse(model.IsValid);
		}

		[TestMethod]
		public void ModelIsNotValidWhenModuleWebFolderHasACloseBrace()
		{
			SetModelDefaultArguments();
			dictionary.SetValue("ModuleFolderNameOnWebSite", "Module1}");

			Assert.IsFalse(model.IsValid);
		}
		[TestMethod]
		public void ModelIsNotValidWhenModuleWebFolderHasAOpenParen()
		{
			SetModelDefaultArguments();
			dictionary.SetValue("ModuleFolderNameOnWebSite", "Module(1");

			Assert.IsFalse(model.IsValid);
		}
		[TestMethod]
		public void ModelIsNotValidWhenModuleWebFolderHasACloseParen()
		{
			SetModelDefaultArguments();
			dictionary.SetValue("ModuleFolderNameOnWebSite", "Mod)ule1");

			Assert.IsFalse(model.IsValid);
		}

		[TestMethod]
		public void ModelIsNotValidWhenModuleWebFolderHasAStar()
		{
			SetModelDefaultArguments();
			dictionary.SetValue("ModuleFolderNameOnWebSite", "*Module1");

			Assert.IsFalse(model.IsValid);
		}

		[TestMethod]
		public void ModelIsNotValidWhenModuleWebFolderHasAnAmpersand()
		{
			SetModelDefaultArguments();
			dictionary.SetValue("ModuleFolderNameOnWebSite", "Mod&ule1");

			Assert.IsFalse(model.IsValid);
		}

		[TestMethod]
		public void ModelIsNotValidWhenModuleWebFolderHasAnDollarSign()
		{
			SetModelDefaultArguments();
			dictionary.SetValue("ModuleFolderNameOnWebSite", "Modu$le1");

			Assert.IsFalse(model.IsValid);
		}

		[TestMethod]
		public void ModelIsNotValidWhenModuleWebFolderHasABang()
		{
			SetModelDefaultArguments();
			dictionary.SetValue("ModuleFolderNameOnWebSite", "Mod!ule1");

			Assert.IsFalse(model.IsValid);
		}

		[TestMethod]
		public void ModelIsNotValidWhenModuleWebFolderHasAColon()
		{
			SetModelDefaultArguments();
			dictionary.SetValue("ModuleFolderNameOnWebSite", "Mod:ule1");

			Assert.IsFalse(model.IsValid);
		}

		[TestMethod]
		public void ModelIsNotValidWhenModuleWebFolderHasASemiColon()
		{
			SetModelDefaultArguments();
			dictionary.SetValue("ModuleFolderNameOnWebSite", "M;odule1");

			Assert.IsFalse(model.IsValid);
		}

		[TestMethod]
		public void ModelIsNotValidWhenModuleWebFolderHasASingleQuote()
		{
			SetModelDefaultArguments();
			dictionary.SetValue("ModuleFolderNameOnWebSite", "'Module1");

			Assert.IsFalse(model.IsValid);
		}

		[TestMethod]
		public void ModelIsNotValidWhenModuleWebFolderHasADoubleQuote()
		{
			SetModelDefaultArguments();
			dictionary.SetValue("ModuleFolderNameOnWebSite", "Mod\"ule1");

			Assert.IsFalse(model.IsValid);
		}

		[TestMethod]
		public void ModelIsNotValidWhenModuleWebFolderHasASlash()
		{
			SetModelDefaultArguments();
			dictionary.SetValue("ModuleFolderNameOnWebSite", "Mod/ule1");

			Assert.IsFalse(model.IsValid);
		}

		[TestMethod]
		public void ModelIsNotValidWhenModuleWebFolderHasABackSlash()
		{
			SetModelDefaultArguments();
			dictionary.SetValue("ModuleFolderNameOnWebSite", @"Mod\ule1");

			Assert.IsFalse(model.IsValid);
		}

		[TestMethod]
		public void ModelIsNotValidWhenModuleWebFolderHasAPeriod()
		{
			SetModelDefaultArguments();
			dictionary.SetValue("ModuleFolderNameOnWebSite", "Mod.ule1");

			Assert.IsFalse(model.IsValid);
		}
		[TestMethod]
		public void ModelIsNotValidWhenModuleWebFolderHasAnComma()
		{
			SetModelDefaultArguments();
			dictionary.SetValue("ModuleFolderNameOnWebSite", "Mod,ule1");

			Assert.IsFalse(model.IsValid);
		}

		[TestMethod]
		public void ModelIsNotValidWhenModuleWebFolderHasAOpenAngleBracket()
		{
			SetModelDefaultArguments();
			dictionary.SetValue("ModuleFolderNameOnWebSite", "Mod<ule1");

			Assert.IsFalse(model.IsValid);
		}

		[TestMethod]
		public void ModelIsNotValidWhenModuleWebFolderHasACloseAngleBracket()
		{
			SetModelDefaultArguments();
			dictionary.SetValue("ModuleFolderNameOnWebSite", "Modul>e1");

			Assert.IsFalse(model.IsValid);
		}

        [TestMethod]
        public void ModelIsNotValidWhenModuleWebFolderHasAPipe()
        {
            SetModelDefaultArguments();
            dictionary.SetValue("ModuleFolderNameOnWebSite", "Module|1");

            Assert.IsFalse(model.IsValid);
        }
		
//		^[^#@%{})(*&$!:;'"/?\.,><\\]*[^\.#@%{})(*&$!:;'"/?\.,><\\]$
    }
}

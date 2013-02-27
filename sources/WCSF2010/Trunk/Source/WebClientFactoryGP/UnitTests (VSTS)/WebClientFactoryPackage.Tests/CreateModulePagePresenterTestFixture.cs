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

namespace WebClientFactoryPackage.Tests
{
    /// <summary>
    /// Summary description for CreateModulePagePresenterTestFixture
    /// </summary>
    [TestClass]
    public class CreateModulePagePresenterTestFixture
    {
        public CreateModulePagePresenterTestFixture()
        {
        }

        private IDictionaryService dictionary;
        private MockCreateModulePage view;

        private CreateModulePagePresenter presenter;
        private MockSolutionModel solutionModel;
        private CreateModulePageModel model;


        [TestInitialize]
        public void SetUp()
        {
            dictionary = new MockDictionaryService();
            view = new MockCreateModulePage();
            solutionModel = new MockSolutionModel(null);
            model = new CreateModulePageModel(dictionary, solutionModel, null);
            presenter = new CreateModulePagePresenter(view, model);

            MockProjectModel moduleProject1 = new MockProjectModel();
            moduleProject1.ProjectPath = (new System.IO.DirectoryInfo(@".\Support\MockWebSite")).FullName;
            dictionary.SetValue("WebUIProject", moduleProject1);
        }

        [TestMethod]
        public void ShowModuleNameOnViewReady()
        {
            dictionary.SetValue("ModuleName", "Module1");
            model.SelectedSolutionFolder = new MockProjectModel();

            presenter.OnViewReady();

            Assert.AreEqual(view.ModuleName, model.ModuleName);
            Assert.AreEqual(view.ModuleName, "Module1");
        }

        [TestMethod]
        public void ChangeModuleFolderNameOnWebSiteIfModuleNameChanges()
        {
            //view.ModuleFolderOnWebSiteIsSameAsModuleName = true;
            view.FireModuleFolderOnWebSiteChanging("ModuleFolder");
            view.FireModuleNameChanging("ChangedModuleName");

            Assert.AreEqual(view.ModuleName, model.ModuleName);
            Assert.AreEqual(view.ModuleName, model.ModuleFolderNameOnWebSite);
        }

        [TestMethod]
        public void ChangeCreateModuleInterfaceLibrary()
        {
            view.FireCreateModuleInterfaceLibraryChanged(true);
            Assert.AreEqual(view.CreateModuleInterfaceLibrary, model.CreateModuleInterfaceLibrary);
        }

        [TestMethod]
        public void ChangeCreateTestProject()
        {
            view.FireCreateTestProjectChanged(true);
            Assert.AreEqual(view.CreateTestProject, model.CreateTestProject);
        }

        [TestMethod]
        public void ShowDocumentation()
        {
            view.FireShowDocumentationChanged(true);

            Assert.AreEqual(view.ShowDocumentation, model.ShowDocumentation);
        }

        [TestMethod]
        public void ModuleNameEmptyNotValidate()
        {
            dictionary.SetValue("RecipeLanguage", "cs");
            view.FireModuleNameChanging(String.Empty);

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void ModuleNameBeginningWithNumbersNotValidate()
        {
            dictionary.SetValue("RecipeLanguage", "cs");
            view.FireModuleNameChanging("1MyBusinessModule");

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void ModuleNameWithBlanksNotValidate()
        {
            dictionary.SetValue("RecipeLanguage", "cs");
            view.FireModuleNameChanging("My Business Module");

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void ModuleFolderNameOnWebSiteEmptyNotValidate()
        {
            dictionary.SetValue("RecipeLanguage", "cs");
            model.ModuleName = "Module1";
            view.FireModuleFolderOnWebSiteChanging(String.Empty);

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void ModuleFolderNameOnWebSiteContainingInvalidPeriodNotInvalidate()
        {
            dictionary.SetValue("RecipeLanguage", "cs");
            model.ModuleName = "Module1";
            view.FireModuleFolderOnWebSiteChanging(".");

            Assert.IsFalse(model.IsValid);
        }

		[TestMethod]
		public void ModuleFolderNameOnWebSiteContainingInvalidCharsNotInvalidate()
		{
			dictionary.SetValue("RecipeLanguage", "cs");
			model.ModuleName = "Module1";
			view.FireModuleFolderOnWebSiteChanging("{}*");

			Assert.IsFalse(model.IsValid);
		}

        [TestMethod]
        public void ModuleFolderNameOnWebSiteWithOnlyBlanksNotValidate()
        {
            dictionary.SetValue("RecipeLanguage", "cs");
            model.ModuleName = "Module1";
            view.FireModuleFolderOnWebSiteChanging("  ");

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        [DeploymentItem(@"Support\MockWebsite", @"Support\MockWebsite")]
        public void ExistingModuleFolderOnWebSiteNotValidate()
        {
            dictionary.SetValue("RecipeLanguage", "cs");
            model.ModuleName = "Module1";
            view.FireModuleFolderOnWebSiteChanging("Module1");
            
            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void ModelIsValidByDefaultOnViewReady()
        {
            MockProjectModel project1 = new MockProjectModel();
            project1.Responsibilities = new string[] { "IsWebProject" };
            project1.Project = new object();
            MockProjectModel project2 = new MockProjectModel();
            MockProjectModel solutionFolder = new MockProjectModel();
            solutionFolder.Name = "Modules";
            model.SelectedSolutionFolder = solutionFolder;

            solutionModel.AddProject("WebSite1", project1);
            solutionModel.AddProject("Modules", solutionFolder);
            solutionModel.AddProject("SomeLibrary", project2);

            dictionary.SetValue("ModuleName", "MyModule");
            dictionary.SetValue("WebUIProject", project1.Project);
            dictionary.SetValue("RecipeLanguage", "cs");

            presenter.OnViewReady();

            Assert.IsTrue(model.IsValid);
        }

        [TestMethod]
        public void OnViewReadyShowWebProjectsOnly()
        {
            MockProjectModel project1 = new MockProjectModel();
            project1.Responsibilities = new string[] { "IsWebProject" };
            MockProjectModel project2 = new MockProjectModel();

            model.SelectedSolutionFolder = new MockProjectModel();

            solutionModel.AddProject("WebSite1", project1);
            solutionModel.AddProject("SomeLibrary", project2);

            presenter.OnViewReady();

            Assert.AreEqual(view.WebProjectsShown, 1);
        }

        [TestMethod]
        public void OnViewReadyDontShowWebSubProjects()
        {
            //model.IsWCSFSolutionWAP = true;

            MockProjectModel project1 = new MockProjectModel();
            project1.Responsibilities = new string[] { "IsWebProject" };
            MockProjectModel project2 = new MockProjectModel();
            project2.Responsibilities = new string[] { "IsWebProject", "IsFolderOfRootWebProject" };

            model.SelectedSolutionFolder = new MockProjectModel();

            solutionModel.AddProject("WebSite1", project1);
            solutionModel.AddProject("Module1SubWebsite", project2);

            presenter.OnViewReady();

            Assert.AreEqual(view.WebProjectsShown, 1);
        }

        [TestMethod]
        public void WebProjectOnModelIsUpdatedWhenViewSelectsProject()
        {
            MockProjectModel project1 = new MockProjectModel();
            project1.Responsibilities = new string[] { "IsWebProject" };
            project1.Project = new object();
            MockProjectModel project2 = new MockProjectModel();

            solutionModel.AddProject("WebSite1", project1);
            solutionModel.AddProject("SomeLibrary", project2);

            view.FireWebProjectSelected(project1);

            Assert.AreSame(view.SelectedWebProject, project1);
            Assert.IsNotNull(model.WebProject);
        }

    }

    class MockCreateModulePage : ICreateModulePage
    {
        private string _moduleName;
        private string _moduleFolderOnWebSite;
        private bool _createTestProject;
        private bool _createModuleInterfaceLibrary;
        private bool _showDocumentation;
        private int _webProjectsShown;
        private bool _createAsFolderInWebsite;
        private bool _errorMessageShowed;

        public MockCreateModulePage()
        {
        }

        public string ModuleName
        {
            get { return _moduleName; }
        }

        public string ModuleFolderNameOnWebSite
        {
            get { return _moduleFolderOnWebSite; }
            set { _moduleFolderOnWebSite = value; }
        }

        public bool CreateTestProject
        {
            get { return _createTestProject; }
        }

        public bool CreateModuleInterfaceLibrary
        {
            get { return _createModuleInterfaceLibrary; }
        }

        public bool ShowDocumentation
        {
            get { return _showDocumentation; }
        }

        public int WebProjectsShown
        {
            get { return _webProjectsShown; }
        }

        public bool ErrorMessageShowed
        {
            get { return _errorMessageShowed; }
        }

        private IProjectModel _projectSelected;

        public IProjectModel SelectedWebProject
        {
            get { return _projectSelected; }
        }

        public event EventHandler<EventArgs> ModuleNameChanging;
        public event EventHandler<EventArgs> ModuleFolderNameOnWebSiteChanged;
        public event EventHandler<EventArgs> CreateModuleInterfaceLibraryChanged;
        public event EventHandler<EventArgs> CreateTestProjectChanged;
        public event EventHandler<EventArgs> WebProjectSelected;
        public event EventHandler<EventArgs<bool>> RequestingValidation;
        public event EventHandler<EventArgs> ShowDocumentationChanged;

        public void FireRequestingValidation(bool value)
        {
            if (RequestingValidation != null)
            {
                RequestingValidation(this, new EventArgs<bool>(value));
            }
        }

        public void FireModuleNameChanging(string value)
        {
            _moduleName = value;
            if (ModuleNameChanging != null)
                ModuleNameChanging(this, new EventArgs());
        }

        public void FireModuleFolderOnWebSiteChanging(string value)
        {
            _moduleFolderOnWebSite = value;
            if (ModuleFolderNameOnWebSiteChanged != null)
                ModuleFolderNameOnWebSiteChanged(this, new EventArgs());
        }

        public void FireCreateModuleInterfaceLibraryChanged(bool value)
        {
            _createModuleInterfaceLibrary = value;
            if (CreateModuleInterfaceLibraryChanged != null)
                CreateModuleInterfaceLibraryChanged(this, new EventArgs());
        }

        public void FireCreateTestProjectChanged(bool value)
        {
            _createTestProject = value;
            if (CreateTestProjectChanged != null)
                CreateTestProjectChanged(this, new EventArgs());
        }

        public void FireShowDocumentationChanged(bool value)
        {
            _showDocumentation = value;
            if (ShowDocumentationChanged != null)
                ShowDocumentationChanged(this, new EventArgs());
        }

        public void FireWebProjectSelected(IProjectModel value)
        {
            _projectSelected = value;
            if (WebProjectSelected != null)
                WebProjectSelected(this, new EventArgs());
        }

        public bool CreateAsFolderInWebsite
        {
            get { return _createAsFolderInWebsite; }
            set { _createAsFolderInWebsite = value; }
        }

        public event EventHandler<EventArgs> CreateAsFolderInWebsiteChanged;

        public void FireCreateAsFolderInWebsiteChanged(bool value)
        {
            _createAsFolderInWebsite = value;
            if (CreateAsFolderInWebsiteChanged != null)
                CreateAsFolderInWebsiteChanged(this, new EventArgs());
        }

        public void SetupView(string moduleName, string webSiteModuleFolderName, string solutionFolderName, IList<IProjectModel> webProjects, bool subProjectOptionsVisible, string language)
        {
            _moduleName = moduleName;
            _moduleFolderOnWebSite = webSiteModuleFolderName;
            _webProjectsShown = webProjects.Count;
        }

        public void ShowValidationErrorMessage(string errorMessage)
        {
            _errorMessageShowed = true;
        }
    }
}

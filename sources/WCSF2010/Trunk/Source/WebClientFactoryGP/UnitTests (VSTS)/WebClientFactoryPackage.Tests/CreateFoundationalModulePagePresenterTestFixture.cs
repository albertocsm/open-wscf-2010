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
    /// Summary description for CreateFoundationalModulePagePresenterTestFixture
    /// </summary>
    [TestClass]
    public class CreateFoundationalModulePagePresenterTestFixture
    {
        public CreateFoundationalModulePagePresenterTestFixture()
        {
        }

        private IDictionaryService dictionary;
        private MockCreateFoundationalModulePage view;
        private CreateFoundationalModulePagePresenter presenter;
        private MockSolutionModel solutionModel;
        private CreateFoundationalModulePageModel model;

        [TestInitialize]
        public void SetUp()
        {
            dictionary = new MockDictionaryService();
            view = new MockCreateFoundationalModulePage();
            solutionModel = new MockSolutionModel(null);
            model = new CreateFoundationalModulePageModel(dictionary, solutionModel, null);
            presenter = new CreateFoundationalModulePagePresenter(view, model);           
        }       

        [TestMethod]
        public void ShowModuleNameOnViewReady()
        {
            model.SelectedSolutionFolder = new MockProjectModel();
            dictionary.SetValue("ModuleName", "Module1");

            presenter.OnViewReady();

            Assert.AreEqual(view.ModuleName, model.ModuleName);
            Assert.AreEqual(view.ModuleName, "Module1");            
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

            view.FireModuleNameChanging("1MyFoudationalModule");

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void ModuleNameWithBlanksNotValidate()
        {
            dictionary.SetValue("RecipeLanguage", "cs");
            view.FireModuleNameChanging("My Foundational Module");

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void ModelIsValidByDefaultOnViewReady()
        {
            MockProjectModel project1 = new MockProjectModel();
            project1.Responsibilities = new string[] { "IsWebProject" };
            project1.Project = new object();
            MockProjectModel project2 = new MockProjectModel();

            solutionModel.AddProject("WebSite1", project1);
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
        
        [TestMethod]
        public void OnViewReadyShowSolutionPreview()
        {
            model.SelectedSolutionFolder = new MockProjectModel();

            presenter.OnViewReady();

            Assert.IsTrue(view.PreviewRefreshed);
        }

        [TestMethod]
        public void ShowDocumentation()
        {
            view.FireShowDocumentationChanged(true);
            Assert.AreEqual(view.ShowDocumentation, model.ShowDocumentation);
        }

        [TestMethod]
        public void SetLanguageOnViewReady()
        {
            model.SelectedSolutionFolder = new MockProjectModel();
            dictionary.SetValue("RecipeLanguage", "cs");
            presenter.OnViewReady();

            Assert.AreEqual(model.Language, view.Language);
            Assert.AreEqual("cs", view.Language);
        }
    }

    class MockCreateFoundationalModulePage : ICreateFoundationalModulePage
    {
        private string _moduleName;
        private bool _createTestProject;
        private bool _createModuleInterfaceLibrary;
        private bool _previewRefreshed;
        private int _webProjectsShown;
        private string _language;
        
        public MockCreateFoundationalModulePage()
        {
        }


        public string ModuleName
        {
            get { return _moduleName; }
        }

        public bool CreateTestProject
        {
            get { return _createTestProject; }
        }

        public bool CreateModuleInterfaceLibrary
        {
            get { return _createModuleInterfaceLibrary; }
        }

        public int WebProjectsShown
        {
            get { return _webProjectsShown; }
        }

        private IProjectModel _projectSelected;

        public IProjectModel SelectedWebProject
        {
            get { return _projectSelected; }
        }

        public bool PreviewRefreshed
        {
            get { return _previewRefreshed; }
        }

        public string SelectedSolutionFolderName
        {
            get { return null; }
            set { }
        }

        public event EventHandler<EventArgs> ModuleNameChanging;
        public event EventHandler<EventArgs> CreateModuleInterfaceLibraryChanged;
        public event EventHandler<EventArgs> CreateTestProjectChanged;
        public event EventHandler<EventArgs> WebProjectSelected;
        public event EventHandler<EventArgs<bool>> RequestingValidation;

		public void FireRequestingValidation(bool value)
		{
			if (RequestingValidation != null)
			{
				RequestingValidation(this, new EventArgs<bool>(value));
			}
		}

        public void FireCreateModuleInterfaceLibraryChanged(bool value)
        {
            _createModuleInterfaceLibrary = value;
            if (CreateModuleInterfaceLibraryChanged != null)
                CreateModuleInterfaceLibraryChanged(this, new EventArgs());
        }

        public void FireModuleNameChanging(string value)
        {
            _moduleName = value;
            if (ModuleNameChanging != null)
                ModuleNameChanging(this, new EventArgs());
        }

        public void ShowModuleName(string moduleName)
        {
            _moduleName = moduleName;
            FireModuleNameChanging(moduleName);
        }

        public void ShowWebProjects(IList<IProjectModel> projects)
        {
            _webProjectsShown = projects.Count;
        }

        public void FireCreateTestProjectChanged(bool value)
        {
            _createTestProject = value;
            if (CreateTestProjectChanged != null)
                CreateTestProjectChanged(this, new EventArgs());
        }


        public void FireWebProjectSelected(IProjectModel value)
        {
            _projectSelected = value;
            if (WebProjectSelected != null)
                WebProjectSelected(this, new EventArgs());
        }
        
        public void RefreshSolutionPreview()
        {
            _previewRefreshed = true;
        }

        #region ICreateFoundationalModulePage Members


        private bool _showdocs;
        public bool ShowDocumentation
        {
            get { return _showdocs; }
            set { _showdocs = value; }
        }

        public event EventHandler<EventArgs> ShowDocumentationChanged;

        public void FireShowDocumentationChanged(bool value)
        {
            _showdocs = value;
            if (ShowDocumentationChanged != null)
                ShowDocumentationChanged(this, new EventArgs());
        }

        public void SetLanguage(string language)
        {
            _language = language;
        }

        public string Language
        {
            get { return _language; }
        }

        #endregion
    }
   
}

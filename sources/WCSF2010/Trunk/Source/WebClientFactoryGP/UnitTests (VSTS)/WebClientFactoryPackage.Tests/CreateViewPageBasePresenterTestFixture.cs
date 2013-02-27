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
using WebClientFactoryPackage.Tests.Support;
using Microsoft.Practices.RecipeFramework.Extensions.DteWrapper;
using Microsoft.Practices.WebClientFactory.CustomWizardPages;
using System.IO;
using Microsoft.Practices.RecipeFramework.Extensions;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Configuration;
using Microsoft.Practices.WebClientFactory;
using Microsoft.Practices.CompositeWeb.Services;

namespace WebClientFactoryPackage.Tests
{
    /// <summary>
	/// Summary description for CreateViewPageBasePresenterTestFixture
    /// </summary>
    [TestClass]
    public class CreateViewPageBasePresenterTestFixture
    {
		public CreateViewPageBasePresenterTestFixture()
        {            
        }   

        private CreateViewPageBasePresenter presenter;
		private MockCreateViewPageBase view;
        private IDictionaryService dictionary;
        private MockSolutionModel solutionModel;
        private MockCreateViewPageBaseModel mockModel;
        private CreateViewPageBaseModel model;

        [TestInitialize]
        public void Setup()
        {
            dictionary = new MockDictionaryService();
            solutionModel = new MockSolutionModel(null);
            mockModel = new MockCreateViewPageBaseModel();
            view = new MockCreateViewPageBase();
            presenter = new CreateViewPageBasePresenter(view, mockModel);
            model = new CreateViewPageBaseModel(dictionary, solutionModel, null);
        }

        [TestMethod]
        public void ShowViewNameOnViewReady()
        {
            mockModel.ViewName = "View1";

            presenter.OnViewReady();

            Assert.AreEqual(view.ViewNameDisplayed, "View1");
        }

        [TestMethod]
        public void ShowModuleProjectsOnViewReady()
        {
            CreateMockSolutionModel();

            List<IProjectModel> moduleProjects = solutionModel.FindProjectsWithResponsibility("IsModuleProject");
            List<IProjectModel> businessModuleProjects = FilterProjectsWithVirtualPath(moduleProjects, GetModuleInfoCollection());
            mockModel.ModuleProjects = businessModuleProjects;

            presenter.OnViewReady();

            IList<IProjectModel> displayed = view.ModuleProjectsDisplayed;

            Assert.AreEqual(displayed.Count, 4);
            Assert.AreSame(displayed[0], mockModel.ModuleProjects[0]);
            Assert.AreSame(displayed[1], mockModel.ModuleProjects[1]);
            Assert.AreSame(displayed[2], mockModel.ModuleProjects[2]);
            Assert.AreSame(displayed[3], mockModel.ModuleProjects[3]);
        }       

        [TestMethod]
        public void SelectsDefaultModuleProjectOnViewReady()
        {
            CreateMockSolutionModel();

            mockModel.ModuleProject = solutionModel.GetProject("Module2");

            presenter.OnViewReady();

            Assert.IsNotNull(view.ModuleProjectSelected);
            Assert.AreSame(view.ModuleProjectSelected, mockModel.ModuleProject);
        }

        [TestMethod]
        public void ShowWebProjectsOnViewReady()
        {
            CreateMockSolutionModel();

            mockModel.WebProjects = solutionModel.FindProjectsWithResponsibility("IsWebProject");

            presenter.OnViewReady();

            IList<IProjectModel> displayed = view.WebProjectsDisplayed;

            Assert.AreEqual(displayed.Count, 1);
            Assert.AreSame(displayed[0], mockModel.WebProjects[0]);
        }

        [TestMethod]
        public void SelectsDefaultWebProjectOnViewReady()
        {
            CreateMockSolutionModel();

            mockModel.WebProject = solutionModel.GetProject("WcsfWebSite");

            presenter.OnViewReady();

            Assert.IsNotNull(view.WebProjectSelected);
            Assert.AreSame(view.WebProjectSelected, mockModel.WebProject);
        }

        [TestMethod]
        public void SetLanguagesOnViewReady()
        {
            mockModel.Language="vb";
            mockModel.ModuleProjectLanguage= "cs";

            presenter.OnViewReady();

            Assert.AreEqual("vb", view.Language);
            Assert.AreEqual("cs", view.ModuleProjectLanguage);
        }

        [TestMethod]
        [DeploymentItem(@"Support\MockWebsite", @"Support\MockWebsite")]
        public void ShowWebSiteFoldersOnViewReady()
        {
            CreateMockSolutionModel();

            mockModel.WebFolders = GetMockWebsiteFolders(); ;

            presenter.OnViewReady();

            Assert.IsNotNull(view.WebFoldersDisplayed);
            Assert.AreEqual(7, view.WebFoldersDisplayed.Count); 
            Assert.AreEqual("Root", view.WebFoldersDisplayed[0].Name);
            Assert.AreEqual("images", view.WebFoldersDisplayed[1].Name);
            Assert.AreEqual("Module1", view.WebFoldersDisplayed[2].Name);
            Assert.AreEqual("Module3", view.WebFoldersDisplayed[3].Name);
            Assert.AreEqual("scripts", view.WebFoldersDisplayed[4].Name);
            Assert.AreEqual("Module2", view.WebFoldersDisplayed[5].Name);
            Assert.AreEqual("_bin", view.WebFoldersDisplayed[6].Name);
        }

        [TestMethod]
        [DeploymentItem(@"Support\MockWebsite", "MockWebsite")]
        public void SelectsDefaultWebSiteFolderOnViewReady()
        {
            CreateMockSolutionModel();

            IList<IProjectItemModel> folders = GetMockWebsiteFolders();
            mockModel.WebFolder = folders[2]; // Module1

            presenter.OnViewReady();

            Assert.IsNotNull(view.WebFolderSelected);
            Assert.AreSame(view.WebFolderSelected, mockModel.WebFolder);
        }

        [TestMethod]
        public void ChangeModuleProjectChangesWebFolder()
        {
            CreateMockSolutionModel();
            IList<IProjectItemModel> folders = GetMockWebsiteFolders();
            mockModel.ModuleInfoCollection = GetModuleInfoCollection();
            mockModel.WebProject = solutionModel.GetProject("WcsfWebSite"); 
            mockModel.WebFolders = folders;
            mockModel.WebFolder = folders[1]; // Module1 folder

            // change to Module2 project but the active folder is Module1
            IProjectModel selected = solutionModel.GetProject("Module3");
            view.FireModuleProjectSelected(selected);

            Assert.AreSame(view.ActiveModuleProject, selected);            
            Assert.AreSame(view.WebFolderSelected, folders[2]);
        }

        [TestMethod]
        public void ChangeModuleProjectToShellSetWebFolderToNull()
        {
            CreateMockSolutionModel();
            IList<IProjectItemModel> folders = GetMockWebsiteFolders();
            mockModel.ModuleInfoCollection = GetModuleInfoCollection();
            mockModel.WebProject = solutionModel.GetProject("WcsfWebSite");
            mockModel.WebFolders = folders;
            mockModel.WebFolder = folders[1]; // Module1 folder
            IModuleInfo rootModuleInfo = FindModuleInfo("Shell", mockModel.ModuleInfoCollection);

            IProjectModel selected = solutionModel.GetProject("Shell");
            view.FireModuleProjectSelected(selected);

            Assert.AreSame(selected, view.ActiveModuleProject);
            Assert.AreEqual(rootModuleInfo.VirtualPath, "~/");
            Assert.IsNull(view.WebFolderSelected);
            Assert.IsNull(mockModel.WebFolder);
        }

        [TestMethod]
        public void ChangeWebFolderOnViewChangesModel()
        {
            IList<IProjectItemModel> folders = GetMockWebsiteFolders();
            mockModel.WebFolders = folders; 
            mockModel.WebFolder = folders[1]; // Module1 folder

            // change to Module2 folder
            IProjectItemModel selected = folders[2];
            view.FireWebFolderSelected(selected);

            Assert.AreSame(view.ActiveWebFolder, selected);
            Assert.AreSame(mockModel.WebFolder, folders[2]);
        }

        [TestMethod]
        public void ChangeViewNameOnViewChangesModel()
        {
            CreateMockSolutionModel();
            mockModel.WebProject = solutionModel.GetProject("WcsfWebSite");
            mockModel.ModuleProject = solutionModel.GetProject("Module2");
            IList<IProjectItemModel> folders = GetMockWebsiteFolders();
            mockModel.WebFolder = GetFolderbyName(folders, "Module1");
            mockModel.ViewsFolder = string.Empty;
            mockModel.ViewFileExtension = "aspx";

            string expected = "View1";
            view.FireViewNameChanged(expected);

            Assert.AreSame(expected, view.ViewName);
            Assert.AreSame(expected, mockModel.ViewName);
        }       

        [TestMethod]
        public void GetTestProjectExistsOnModuleProjectChanges()
        {
            CreateMockSolutionModel();

            mockModel.TestProjectExists = true;

            IProjectModel selected = solutionModel.GetProject("Module2");
            view.FireModuleProjectSelected(selected);                         

            presenter.OnViewReady();

            Assert.IsTrue(view.TestProjectExists);
        }

        [TestMethod]
        public void EmptyViewNameCauseModelNotValid()
        {
            CreateMockSolutionModel();
            dictionary.SetValue("RecipeLanguage", "CS");
            model.ViewName = string.Empty;
            model.WebProject = solutionModel.GetProject("WcsfWebSite");
            model.ModuleProject = solutionModel.GetProject("Module1");

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void ViewNameBeginningWithNumbersCauseModelNotValid()
        {
            CreateMockSolutionModel();
            dictionary.SetValue("RecipeLanguage", "CS");
			model.ViewName = "1MyView";
            model.WebProject = solutionModel.GetProject("WcsfWebSite");
            model.ModuleProject = solutionModel.GetProject("Module1");

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void ModelIsNotValidOnViewNameWithBlanks()
        {
            CreateMockSolutionModel();
            dictionary.SetValue("RecipeLanguage", "CS");
            model.ViewName = "My View";
            model.WebProject = solutionModel.GetProject("WcsfWebSite");
            model.ModuleProject = solutionModel.GetProject("Module1");

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void ModelIsNotValidOnViewNameReservedSystemWord()
        {
            CreateMockSolutionModel();
            dictionary.SetValue("RecipeLanguage", "CS");
            model.ViewName = "CON";
            model.WebProject = solutionModel.GetProject("WcsfWebSite");
            model.ModuleProject = solutionModel.GetProject("Module1");

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void ExistingViewNameCauseModelNotValid()
        {
            CreateMockSolutionModel();
            dictionary.SetValue("RecipeLanguage", "CS");
            dictionary.SetValue("ViewFileExtension", "aspx");
            model.WebProject = solutionModel.GetProject("WcsfWebSite");
            model.ModuleProject = solutionModel.GetProject("Module1");

            IList<IProjectItemModel> folders = GetMockWebsiteFolders();
            model.WebFolder = GetFolderbyName(folders,"Module1");

            model.ViewName = "View2";

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void ModelIsNotValidOnViewNameWithAtSign()
        {
            CreateMockSolutionModel();
            dictionary.SetValue("RecipeLanguage", "CS");
            model.ViewName = "@ModulePage";
            model.WebProject = solutionModel.GetProject("WcsfWebSite");
            model.ModuleProject = solutionModel.GetProject("Module1");

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void ShowDocumentation()
        {
            view.FireShowDocumentChanged(true);
            Assert.AreEqual(view.ShowDocumentation, mockModel.ShowDocumentation);
        }
        
        #region Support methods

        private List<IProjectItemModel> GetMockWebsiteFolders()
        {
            List<IProjectItemModel> folders = new List<IProjectItemModel>();
            string path = (new System.IO.DirectoryInfo(@".\Support\MockWebsite")).FullName;

            folders.AddRange(RecursiveGetSiteFolders(path));

            return folders;
        }

        private List<IProjectItemModel> RecursiveGetSiteFolders(string path)
        {
            List<IProjectItemModel> items = new List<IProjectItemModel>();
            string[] foldersOnWebSite = Directory.GetDirectories(path);
            foreach (string folder in foldersOnWebSite)
            {
                items.Add(new MockProjectItemModel(new object(), new DirectoryInfo(folder).Name, folder));
                items.AddRange(RecursiveGetSiteFolders(folder));
            }
            return items;
        }

        private DependantModuleInfo[] GetModuleInfoCollection()
        {
            string path = (new System.IO.DirectoryInfo(@".\Support\MockWebsite")).FullName;
            WebConfigModuleInfoStore store = new WebConfigModuleInfoStore(path);
            WebModuleEnumerator moduleEnumerator = new WebModuleEnumerator(store);
            return (DependantModuleInfo[])moduleEnumerator.EnumerateModules();
        }

        private IModuleInfo FindModuleInfo(string assemblyName, IModuleInfo[] modules)
        {
            List<IModuleInfo> moduleList = new List<IModuleInfo>(modules);
            return moduleList.Find(delegate(IModuleInfo match)
            {
                return match.AssemblyName == assemblyName;
            });

        }

        private List<IProjectModel> FilterProjectsWithVirtualPath(List<IProjectModel> moduleProjects, IModuleInfo[] moduleInfos)
        {
            List<IProjectModel> filtered = new List<IProjectModel>();
            foreach (IProjectModel project in moduleProjects)
            {
                IModuleInfo projectModuleInfo = FindModuleInfo(project.AssemblyName, moduleInfos);
                if (projectModuleInfo != null)
                {
                    if (!string.IsNullOrEmpty(projectModuleInfo.VirtualPath))
                    {
                        filtered.Add(project);
                    }
                }
            }
            return filtered;
        }

        private void CreateMockSolutionModel()
        {
            MockProjectModel moduleProject1 = new MockProjectModel();
            moduleProject1.Responsibilities = new string[] { "IsModuleProject" };
            moduleProject1.Project = new object();
            moduleProject1.AssemblyName = "Module1";
            moduleProject1.ProjectPath = (new System.IO.DirectoryInfo(@".\Support\MockWebSite\Module1")).FullName;
            MockProjectModel moduleProject2 = new MockProjectModel();
            moduleProject2.Responsibilities = new string[] { "IsModuleProject" };
            moduleProject2.Project = new object();
            moduleProject2.AssemblyName = "Module2";
            moduleProject2.ProjectPath = (new System.IO.DirectoryInfo(@".\Support\MockWebSite\Module2")).FullName;
            MockProjectModel moduleProject3 = new MockProjectModel();
            moduleProject3.Responsibilities = new string[] { "IsModuleProject" };
            moduleProject3.Project = new object();
            moduleProject3.AssemblyName = "Module3";
            moduleProject3.ProjectPath = (new System.IO.DirectoryInfo(@".\Support\MockWebSite\Module1\Module3")).FullName;

            MockProjectModel rootModuleProject = new MockProjectModel();
            rootModuleProject.Responsibilities = new string[] { "IsModuleProject" };
            rootModuleProject.Project = new object();
            rootModuleProject.AssemblyName = "Shell";
            rootModuleProject.ProjectPath = (new System.IO.DirectoryInfo(@".\Support\MockWebSite\Shell")).FullName;
            MockProjectModel foundationalProject = new MockProjectModel();
            foundationalProject.Responsibilities = new string[] { "IsModuleProject" };
            foundationalProject.Project = new object();
            foundationalProject.AssemblyName = "Foundational1";
            foundationalProject.ProjectPath = (new System.IO.DirectoryInfo(@".\Support\MockWebSite\Foundational1")).FullName;
            MockProjectModel webSiteProject1 = new MockProjectModel();
            webSiteProject1.Responsibilities = new string[] { "IsWebProject" };
            webSiteProject1.Project = new object();
            webSiteProject1.ProjectPath = (new System.IO.DirectoryInfo(@".\Support\MockWebsite")).FullName;          
            MockProjectModel webSiteProject2 = new MockProjectModel();
            webSiteProject2.Project = new object();
            webSiteProject2.ProjectPath = (new System.IO.DirectoryInfo(@".\MockWebSiteProject2")).FullName;
            MockProjectModel otherProject = new MockProjectModel();
            otherProject.Project = new object();
            otherProject.ProjectPath = (new System.IO.DirectoryInfo(@".\OtherProject")).FullName;

            solutionModel.AddProject("Module1", moduleProject1);
            solutionModel.AddProject("Module2", moduleProject2);
            solutionModel.AddProject("Module3", moduleProject3);
            solutionModel.AddProject("Shell", rootModuleProject);
            solutionModel.AddProject("Foundational1", foundationalProject);
            solutionModel.AddProject("WcsfWebSite", webSiteProject1);
            solutionModel.AddProject("SomeWebService", webSiteProject2);
            solutionModel.AddProject("SomeLibrary", otherProject);
        }

        private IProjectItemModel GetFolderbyName(IList<IProjectItemModel> folders, string name)
        {
            foreach (IProjectItemModel item in folders)
            {
                if (item.Name == name)
                    return item;
            }
            return null;
        }
        #endregion
    }

	class MockCreateViewPageBase : ICreateViewPageBase
    {
        public string ViewNameDisplayed;
        public string ViewFileExtensionDisplayed; 
        public IList<IProjectModel> ModuleProjectsDisplayed;
        public IProjectModel ModuleProjectSelected;
        public IList<IProjectModel> WebProjectsDisplayed;
        public IProjectModel WebProjectSelected;
        public IList<IProjectItemModel> WebFoldersDisplayed;
        public IProjectItemModel WebFolderSelected;
        public bool RefreshCalled;
        public bool CreateViewAtRootChecked;
        public bool ModuleProjectsDisabled;
        public bool ErrorMessageShowed;
        private string _recipeLanguage;
        private string _moduleLanguage;

        public event EventHandler<EventArgs> ModuleProjectChanged;
        public event EventHandler<EventArgs> WebFolderChanged;
        public event EventHandler<EventArgs> ViewNameChanged;
        public event EventHandler<EventArgs> CreateViewAtRootChanged;
        public event EventHandler<EventArgs<bool>> RequestingValidation;

		public void FireRequestingValidation(bool value)
		{
			if (RequestingValidation != null)
			{
				RequestingValidation(this, new EventArgs<bool>(value));
			}
		}

        public IProjectModel ActiveModuleProject
        {
            get { return ModuleProjectSelected; }
        }

        public IProjectItemModel ActiveWebFolder
        {
            get { return WebFolderSelected; }
        }

        public string ViewName
        {
            get { return ViewNameDisplayed; }
        }

        public bool CreateViewAtRoot
        {
            get { return CreateViewAtRootChecked; }
        }

        public void ShowViewName(string viewName,string viewFileExtension)
        {
            ViewNameDisplayed = viewName;
            ViewFileExtensionDisplayed = viewFileExtension;
        }

        public void ShowModuleProjects(IList<IProjectModel> modules, IProjectModel selected)
        {
            ModuleProjectsDisplayed = modules;
            ModuleProjectSelected = selected;
        }

        public void ShowWebProjects(IList<IProjectModel> webprojects, IProjectModel selected)
        {
            WebProjectsDisplayed = webprojects;
            WebProjectSelected = selected;
        }

        public void ShowWebFolders(IList<IProjectItemModel> folders, IProjectItemModel selected)
        {
            WebFoldersDisplayed = folders;
            WebFolderSelected = selected;
        }

        public void FireModuleProjectSelected(IProjectModel module)
        {
            ModuleProjectSelected = module;
            if (ModuleProjectChanged != null)
                ModuleProjectChanged(this, new EventArgs());
        }

        internal void FireWebFolderSelected(IProjectItemModel folder)
        {
            WebFolderSelected = folder;
            if ( WebFolderChanged != null )
                WebFolderChanged(this, new EventArgs());
        }

        public void SelectWebFolder(IProjectItemModel moduleFolder)
        {
            WebFolderSelected = moduleFolder;
            if (WebFolderChanged != null)
                WebFolderChanged(this, new EventArgs());
        }

        public void FireViewNameChanged(string viewName)
        {
            ViewNameDisplayed = viewName;
            if ( ViewNameChanged != null )
                ViewNameChanged(this, new EventArgs());
        }

        public void FireCreateViewAtRootChanged(bool value)
        {
            CreateViewAtRootChecked = value;
            if (CreateViewAtRootChanged != null)
                CreateViewAtRootChanged(this, new EventArgs());
        }

        public void ShowTestProject(bool testProjectExists)
        {
            _testProjectExists = testProjectExists;
        }

        private bool _testProjectExists;
        public bool TestProjectExists
        {
            get { return _testProjectExists; }
        }


        public void RefreshSolutionPreview()
        {
            RefreshCalled = true;
        }


        public void EnableModuleProjectList(bool enable)
        {
            ModuleProjectsDisabled = !enable;
        }


        public void SelectModuleProject(IProjectModel module)
        {
            ModuleProjectSelected = module;
		}

		public event EventHandler<EventArgs> ShowDocumentationChanged;

        private bool _ShowDocumentation;
        public bool ShowDocumentation
        {
            get { return _ShowDocumentation; }
            set { _ShowDocumentation = value; }
        }

        public void FireShowDocumentChanged(bool value)
        {
            _ShowDocumentation = value;
            if (ShowDocumentationChanged != null)
                ShowDocumentationChanged(this, new EventArgs());
        }

        public void SetLanguage(string recipeLanguage, string moduleLanguage)
        {
            _recipeLanguage = recipeLanguage;
            _moduleLanguage = moduleLanguage;
        }

        public string Language
        {
            get { return _recipeLanguage; }
        }

        public string ModuleProjectLanguage
        {
            get { return _moduleLanguage; }
        }

        public void ShowValidationErrorMessage(string errorMessage)
        {
            ErrorMessageShowed = true;
        }
    }

	class MockCreateViewPageBaseModel : ICreateViewPageBaseModel
    {
        private string _viewsFolder;
        public string ViewsFolder
        {
            get { return _viewsFolder; }
            set { _viewsFolder = value; }
        }

        private string _viewFileExtension;
        public string ViewFileExtension
        {
            get { return _viewFileExtension; }
            set { _viewFileExtension = value; }
        }

        private string _viewName;
        public string ViewName
        {
            get
            {
                return _viewName;
            }
            set
            {
                _viewName = value;
            }
        }

        private IList<IProjectModel> _moduleProjects;
        public IList<IProjectModel> ModuleProjects
        {
            get { return _moduleProjects; }
            set { _moduleProjects = value; }
        }

        private IProjectModel _moduleProject;
        public IProjectModel ModuleProject
        {
            get
            {
                return _moduleProject;
            }
            set
            {
                _moduleProject = value;
            }
        }

        private IList<IProjectModel> _webProjects;
        public IList<IProjectModel> WebProjects
        {
            get { return _webProjects; }
            set { _webProjects = value; }
        }

        private IProjectModel _webProject;        
        public IProjectModel WebProject
        {
            get
            {
                return _webProject;
            }
            set
            {
                _webProject = value;
            }
        }

        private IList<IProjectItemModel> _webFolders;
        public IList<IProjectItemModel> WebFolders
        {
            get { return _webFolders; }
            set { _webFolders = value; }
        }

        private IProjectItemModel _webFolder;
        public IProjectItemModel WebFolder
        {
            get { return _webFolder; }
            set { _webFolder = value; }
        }

        private bool _testProjectExists;
        public bool TestProjectExists
        {
            get { return _testProjectExists; }
            set { _testProjectExists = value; }
        }


        public bool IsValid
        {
            get { return !String.IsNullOrEmpty( _viewName ); }
        }

        private DependantModuleInfo[] _moduleInfoCollection;
        public DependantModuleInfo[] ModuleInfoCollection
        {
            get 
            {
                return _moduleInfoCollection;
            }
            set
            {
                _moduleInfoCollection = value;
            }
        }

        private bool _ShowDocumentation;
        public bool ShowDocumentation
        {
            get
            {
                return _ShowDocumentation;
            }
            set
            {
                _ShowDocumentation = value;
            }
        }

        private string _language;
        public string Language
        {
            get
            {
                return _language;
            }
            set
            {
                _language = value;
            }
        }

        private string _moduleProjectLanguage;
        public string ModuleProjectLanguage
        {
            get
            {
                return _moduleProjectLanguage;
            }
            set
            {
                _moduleProjectLanguage = value;
            }
        }

        public string ValidationErrorMessage
        {
            get
            {
                return string.Empty;
            }
        }
    }
}

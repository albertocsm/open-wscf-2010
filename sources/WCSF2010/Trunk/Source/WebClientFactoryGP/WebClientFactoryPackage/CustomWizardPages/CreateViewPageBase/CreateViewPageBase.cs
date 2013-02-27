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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.WizardFramework;
using System.ComponentModel.Design;
using Microsoft.Practices.RecipeFramework.Extensions;
using Microsoft.Practices.RecipeFramework.Extensions.DteWrapper;
using EnvDTE;
using System.Globalization;
using System.IO;

namespace Microsoft.Practices.WebClientFactory.CustomWizardPages
{
    public partial class CreateViewPageBase : CustomWizardPage, ICreateViewPageBase
    {
        private CreateViewPageBasePresenter _presenter;
        private IProjectModel _activeModuleProject;
        private IProjectItemModel _activeWebFolder;
        private bool _testProjectExists;
        private string _recipeLanguage;
        private string _moduleProjectLanguage;
        private string _viewFileExtension;
        
        public event EventHandler<EventArgs> ModuleProjectChanged;
        public event EventHandler<EventArgs> WebFolderChanged;
        public event EventHandler<EventArgs> ViewNameChanged;
        public event EventHandler<EventArgs<bool>> RequestingValidation;
        public event EventHandler<EventArgs> CreateViewAtRootChanged;
        public event EventHandler<EventArgs> ShowDocumentationChanged;

        public CreateViewPageBase()
        {
            InitializeComponent();
        }

		public CreateViewPageBase(WizardForm wizard)
            : base(wizard)
        {
            InitializeComponent();
            _showDocumentation.CheckedChanged += new EventHandler(_showDocumentation_CheckedChanged);
        }

        void _showDocumentation_CheckedChanged(object sender, EventArgs e)
        {
            if (ShowDocumentationChanged != null)
            {
                ShowDocumentationChanged(sender, CreateValidationEventArgs());
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
			if (!DesignMode)
			{
				InitializePresenterAndModel();

				Wizard.OnValidationStateChanged(this);
			}
        }

        void InitializePresenterAndModel()
        {
            IDictionaryService dictionary = (IDictionaryService)GetService(typeof(IDictionaryService));
            DTE dte = (DTE)GetService(typeof(DTE));
			ISolutionModel solution = new DteSolutionModel(dte.Solution, Site);
            CreateViewPageBaseModel model = CreateModel(dictionary, solution, Site);
            _presenter = CreatePresenter(this, model);
            _presenter.OnViewReady();                       
        }

		protected CreateViewPageBaseModel CreateModel(IDictionaryService dictionary, ISolutionModel solution, ISite Site)
		{
			return new CreateViewPageBaseModel(dictionary, solution, Site);
		}

		protected CreateViewPageBasePresenter CreatePresenter(CreateViewPageBase createViewPageBase, CreateViewPageBaseModel model)
		{
			return new CreateViewPageBasePresenter(this, model);
		}

        public IProjectModel ActiveModuleProject
        {
            get { return _activeModuleProject; }
        }

        public IProjectItemModel ActiveWebFolder
        {
            get { return _activeWebFolder; }
        }

        private IProjectModel ActiveWebProject
        {
            get { return _webProjectsBindingSource.Current as IProjectModel; }
        }

        public string ViewName
        {
            get { return _viewNameTextBox.Text; }
        }        

        public void ShowTestProject(bool testProjectExists)
        {
            _testProjectExists = testProjectExists;
        }

        public bool ShowDocumentation
        {
            get { return _showDocumentation.Checked; }
            set { _showDocumentation.Checked = value; }
        }


        public void ShowViewName(string viewName, string viewFileExtension)
        {
            _viewNameTextBox.Text = viewName;
            _viewFileExtension = viewFileExtension;
        }


        public void ShowModuleProjects(IList<IProjectModel> modules, IProjectModel selected)
        {
            _moduleProjectsBindingSource.DataSource = modules;
            int index = selected == null ? 0 : FindModuleProject(selected);                        
            _moduleProjectsBindingSource.Position = index;
        }

        public void ShowWebProjects(IList<IProjectModel> webprojects, IProjectModel selected)
        {
            _webProjectsBindingSource.DataSource = webprojects;
            int index = selected == null ? 0 : FindWebProject(selected);            
            _webProjectsBindingSource.Position = index;
        }        

        public void ShowWebFolders(IList<IProjectItemModel> folders, IProjectItemModel selected)
        {
            _foldersBindingSource.DataSource = folders;
            int index = selected == null ? 0 : FindFolder(selected);
            _foldersBindingSource.Position = index;
        }

        private void _viewNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ViewNameChanged != null)
                ViewNameChanged(sender, CreateValidationEventArgs());

            this.ValidateChildren();

            Wizard.OnValidationStateChanged(this);
            RefreshSolutionPreview();       
        }                               

        private void _moduleProjectsBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            _activeModuleProject = _moduleProjectsBindingSource.Current as IProjectModel;
            if (ModuleProjectChanged != null)
                ModuleProjectChanged(sender, CreateValidationEventArgs());

			this.ValidateChildren();

            Wizard.OnValidationStateChanged(this);
            RefreshSolutionPreview();
        }

        private void _foldersBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            _activeWebFolder = _foldersBindingSource.Current as IProjectItemModel;
            if (WebFolderChanged != null)
                WebFolderChanged(sender, CreateValidationEventArgs());

			this.ValidateChildren();

			Wizard.OnValidationStateChanged(this);
            RefreshSolutionPreview();
        }

        private void _createViewAtRootCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (CreateViewAtRootChanged != null)
                CreateViewAtRootChanged(sender, CreateValidationEventArgs());

			this.ValidateChildren();

			Wizard.OnValidationStateChanged(this);
            RefreshSolutionPreview();
        }    

        private EventArgs CreateValidationEventArgs()
        {
            return new EventArgs();
        }
       
        public void SelectWebFolder(IProjectItemModel moduleFolder)
        {
            int index = FindFolder(moduleFolder);
            _foldersBindingSource.Position = index;
            if (ViewNameChanged != null)
                ViewNameChanged(_viewNameTextBox, CreateValidationEventArgs());
        }

        public void SelectModuleProject(IProjectModel module)
        {
            int index = FindModuleProject(module);
            _moduleProjectsBindingSource.Position = index;
        }

        public void EnableModuleProjectList(bool enable)
        {
            _moduleProjectsComboBox.Enabled = enable;
        }

        public void SetLanguage(string recipeLanguage, string moduleLanguage)
        {
            _recipeLanguage = recipeLanguage;
            _moduleProjectLanguage = moduleLanguage;
        }

        public string Language
        {
            get { return _recipeLanguage; }
        }

        public string ModuleProjectLanguage
        {
            get { return _moduleProjectLanguage; }
        }

        private int FindWebProject(IProjectModel selected)
        {
            int index = ((List<IProjectModel>)_webProjectsBindingSource.DataSource).
                FindIndex(delegate(IProjectModel match)
                {
                    return match.Project.Equals(selected.Project);
                });
            return index;
        }

        private int FindModuleProject(IProjectModel selected)
        {
            int index = ((List<IProjectModel>)_moduleProjectsBindingSource.DataSource).
                FindIndex(delegate(IProjectModel match)
                {
                    return match.Project.Equals(selected.Project);
                });
            return index;
        }

        private int FindFolder(IProjectItemModel selected)
        {
            int index = 0;
            if (selected != null)
            {
                List<IProjectItemModel> folders = _foldersBindingSource.DataSource as List<IProjectItemModel>;
                if (folders != null)
                {
                    index = folders.FindIndex(delegate(IProjectItemModel match)
                    {
                        if (match != null)
                        {
                            return match.ProjectItem.Equals(selected.ProjectItem);
                        }
                        else
                        {
                            return false;
                        }
                    });
                }
            }
            return index;
        }

        public void RefreshSolutionPreview()
        {
            GenerateWebsiteTree();
            GenerateAssembliesTree();
        }

        public override bool IsDataValid
        {
            get
            {
                EventArgs<bool> args = new EventArgs<bool>();
                if (RequestingValidation != null)
                {
                    RequestingValidation(this, args);
                    return args.Data;
                }
                return false;
            }
        }

        private void GenerateWebsiteTree()
        {
            SolutionPreviewHelper helper = new SolutionPreviewHelper(webSiteTreeView, Language);
            helper.GetAddViewWebsitePreview(ViewName, _viewFileExtension, ActiveWebFolder, ActiveWebProject);
        }


        private void GenerateAssembliesTree()
        {
            if (ActiveModuleProject != null)
            {
                SolutionPreviewHelper helper = new SolutionPreviewHelper(assembliesTreeView, ModuleProjectLanguage);
                helper.GetAddViewModulePreview(ActiveModuleProject.Name, ViewName, _viewFileExtension, _testProjectExists);
            }
        }

        public void ShowValidationErrorMessage(string errorMessage)
        {
            _errorProvider.SetError(_viewNameTextBox, errorMessage);
        }
    }    
}

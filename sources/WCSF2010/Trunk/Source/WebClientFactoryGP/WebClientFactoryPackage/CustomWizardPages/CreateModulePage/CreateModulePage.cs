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
using Microsoft.Practices.WebClientFactory.Properties;

namespace Microsoft.Practices.WebClientFactory.CustomWizardPages
{
    public partial class CreateModulePage : CustomWizardPage, ICreateModulePage
    {
        private CreateModulePagePresenter _presenter;
        private string _selectedSolutionFolderName;
        private string _language;
		private string _moduleProjectName;

        public CreateModulePage()
        {
            InitializeComponent();
        }

        public CreateModulePage(WizardForm wizard)
            : base(wizard)
        {
            InitializeComponent();

            _createModuleInterfaceLibrary.CheckedChanged += new EventHandler(_createModuleInterfaceLibrary_CheckedChanged);
            _testProject.CheckedChanged += new EventHandler(_testProject_CheckedChanged);
            _showDocumentation.CheckedChanged += new EventHandler(_showDocumentation_CheckedChanged);
        }

        public event EventHandler<EventArgs> ModuleNameChanging;
        public event EventHandler<EventArgs> ModuleFolderNameOnWebSiteChanged;
        public event EventHandler<EventArgs> CreateTestProjectChanged;
        public event EventHandler<EventArgs> CreateModuleInterfaceLibraryChanged;
        public event EventHandler<EventArgs> WebProjectSelected;
        public event EventHandler<EventArgs<bool>> RequestingValidation;
        public event EventHandler<EventArgs> ShowDocumentationChanged;
        public event EventHandler<EventArgs> CreateAsFolderInWebsiteChanged;

        public void SetupView(string moduleName, string webSiteModuleFolderName, string solutionFolderName, IList<IProjectModel> webProjects, bool subProjectOptionsVisible, string language)
        {
            _moduleNameTextBox.Text = moduleName;
            _moduleFolderName.Text = webSiteModuleFolderName;
            _selectedSolutionFolderName = solutionFolderName;
            List<ProjectListItem> projectList = (new List<IProjectModel>(webProjects)).ConvertAll<ProjectListItem>(new Converter<IProjectModel, ProjectListItem>(ProjectModelToProjectListItemConverter));
            _webProjectsBindingSource.DataSource = projectList;
            _createSubWap.Visible = subProjectOptionsVisible;
            _language = language;

            RefreshSolutionPreview();

            _moduleNameTextBox.TextChanged += new EventHandler(_moduleNameTextBox_TextChanged);
            _moduleFolderName.TextChanged += new EventHandler(_moduleFolderName_TextChanged);
        }

        void _createModuleInterfaceLibrary_CheckedChanged(object sender, EventArgs e)
        {
            if (CreateModuleInterfaceLibraryChanged != null)
            {
                CreateModuleInterfaceLibraryChanged(sender, CreateValidationEventArgs());
            }

            GenerateAssembliesTree();
        }

        void _testProject_CheckedChanged(object sender, EventArgs e)
        {
            if (CreateTestProjectChanged != null)
            {
                CreateTestProjectChanged(sender, CreateValidationEventArgs());                
            }
            GenerateAssembliesTree();
        }

        void _showDocumentation_CheckedChanged(object sender, EventArgs e)
        {
            if (ShowDocumentationChanged != null)
            {
                ShowDocumentationChanged(sender, CreateValidationEventArgs());
            }
        }

        void _moduleNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ModuleNameChanging != null)
            {
                ModuleNameChanging(sender, CreateValidationEventArgs());
            }

            this.ValidateChildren();
            Wizard.OnValidationStateChanged(this);
            RefreshSolutionPreview();
        }       

        void _moduleFolderName_TextChanged(object sender, EventArgs e)
        {
            if (ModuleFolderNameOnWebSiteChanged != null)
            {
                ModuleFolderNameOnWebSiteChanged(sender, CreateValidationEventArgs());
            }

            this.ValidateChildren();
            Wizard.OnValidationStateChanged(this);
            GenerateWebsiteTree();
        }

        private void _webProjectsBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (WebProjectSelected != null)
            {
                WebProjectSelected(sender, CreateValidationEventArgs());
                Wizard.OnValidationStateChanged(this);
            }
        }

        private void _createSubWap_CheckedChanged(object sender, EventArgs e)
        {
            if (CreateAsFolderInWebsiteChanged != null)
            {
                CreateAsFolderInWebsiteChanged(sender, CreateValidationEventArgs());
            }
            GenerateWebsiteTree();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);            
            InitializePresenterAndModel();

            Wizard.OnValidationStateChanged(this);
        }

        void InitializePresenterAndModel()
        {
            IDictionaryService dictionary = (IDictionaryService)GetService(typeof(IDictionaryService));
            DTE dte = (DTE)GetService(typeof(DTE));
			ISolutionModel solution = new DteSolutionModel(dte.Solution, Site);
            CreateModulePageModel model = new CreateModulePageModel(dictionary, solution, Site);
            _presenter = new CreateModulePagePresenter(this, model);
            _presenter.OnViewReady();                       
        }
               
        private EventArgs CreateValidationEventArgs()
        {            
            return new EventArgs();
        }        	                      

        private string Language
        {
            get { return _language; }
        }
        
        [RecipeArgument]
        public string ModuleName
        {
            get { return _moduleNameTextBox.Text; }
            set { }
        }

		[RecipeArgument]
		public string ModuleProjectName
		{
			get { return _moduleProjectName; }
			set { _moduleProjectName = value; }
		}

        [RecipeArgument]
        public string ModuleFolderNameOnWebSite
        {
            get { return _moduleFolderName.Text; }
            set { }
        } 

        public IProjectModel SelectedWebProject
        {
            get {
                ProjectListItem item = _webProjectsBindingSource.Current as ProjectListItem;
                if (item != null)
                {
                    return item.ProjectModel;
                }
                else
                {
                    return null;
                }
            }
        }               

        [RecipeArgument]
        public bool CreateTestProject
        {
            get { return _testProject.Checked; }
            set { _testProject.Checked = value; }
        }

        [RecipeArgument]
        public bool CreateModuleInterfaceLibrary
        {
            get { return _createModuleInterfaceLibrary.Checked; }
            set { _createModuleInterfaceLibrary.Checked = value; }
        }

        [RecipeArgument]
        public bool ShowDocumentation
        {
            get { return _showDocumentation.Checked; }
            set { _showDocumentation.Checked = value; }
        }

        [RecipeArgument]
        public bool CreateAsFolderInWebsite
        {
            get
            {
                return !_createSubWap.Checked;
            }
            set
            {
                _createSubWap.Checked = !value;
            }
        }

 

        private void GenerateAssembliesTree()
        {
            SolutionPreviewHelper helper = new SolutionPreviewHelper(assembliesTreeView, Language);
			helper.GetBusinessModuleProjectPreview(ModuleName, ModuleProjectName, _selectedSolutionFolderName, CreateTestProject, CreateModuleInterfaceLibrary);
        }

        private void GenerateWebsiteTree()
        {
            if (SelectedWebProject != null)
            {
                //Ignore the web project language in the preview
                SolutionPreviewHelper helper = new SolutionPreviewHelper(webSiteTreeView, Language);
                helper.GetBusinessModuleWebsitePreview(ProjectModelToProjectListItemConverter(SelectedWebProject).ProjectName, _moduleFolderName.Text, GetModuleWebsiteName(), CreateAsFolderInWebsite);
            }
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

        private ProjectListItem ProjectModelToProjectListItemConverter(IProjectModel project)
        {
            string projectName;
            if (project.IsWebProject && project.ToString().Contains(@"\"))
            {
                string[] parts = ((Project)project.Project).Name.Split('\\');
                projectName = parts[parts.Length - 2];
            }
            else
            {
                projectName = project.ToString();
            }
            return new ProjectListItem(projectName, project);            
        }



        private string GetModuleWebsiteName()
        {
            return _presenter.GetModuleWebsiteName();
        }

        private void RefreshSolutionPreview()
        {
            GenerateAssembliesTree();
            GenerateWebsiteTree();
        }

        public void ShowValidationErrorMessage(string errorMessage)
        {
            _errorProvider.SetError(_moduleFolderName, errorMessage);
        }

    }
}

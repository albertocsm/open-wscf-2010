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
using System.IO;
using System.Globalization;
using Microsoft.Practices.RecipeFramework.Extensions.DteWrapper;
using Microsoft.Practices.CompositeWeb.Configuration;
using System.Xml.Serialization;
using Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers;
using Microsoft.Practices.WebClientFactory.Properties;
using Microsoft.Practices.RecipeFramework.Extensions;
using Microsoft.Practices.CompositeWeb.Interfaces;

namespace Microsoft.Practices.WebClientFactory.CustomWizardPages
{
    public class CreateViewPageBasePresenter
    {
        private ICreateViewPageBase _view;
        private ICreateViewPageBaseModel _model;

        private const string ModuleInfoFilePattern = "Web.config";

		public CreateViewPageBasePresenter(ICreateViewPageBase view, ICreateViewPageBaseModel model)
        {
            _view = view;
            _model = model;

            _view.ModuleProjectChanged += new EventHandler<EventArgs>(OnModuleProjectChanged);
            _view.WebFolderChanged += new EventHandler<EventArgs>(OnWebFolderChanged);
            _view.ViewNameChanged += new EventHandler<EventArgs>(OnViewNameChanged);
            _view.RequestingValidation += new EventHandler<EventArgs<bool>>(OnRequestingValidation);
            _view.ShowDocumentationChanged += new EventHandler<EventArgs>(OnShowDocumentationChanged);
        }

        void OnShowDocumentationChanged(object sender, EventArgs e)
        {
            _model.ShowDocumentation = _view.ShowDocumentation;
        }

        public void OnViewReady()
        {
            _view.SetLanguage(_model.Language, _model.ModuleProjectLanguage);
            _view.ShowViewName(_model.ViewName,_model.ViewFileExtension);
            _view.ShowModuleProjects(_model.ModuleProjects, _model.ModuleProject);
            _view.ShowWebProjects(_model.WebProjects, _model.WebProject);
            List<IProjectItemModel> moduleFolders = GetModuleFolders(_model.WebFolders);
            _view.ShowWebFolders(moduleFolders, _model.WebFolder);
            _view.RefreshSolutionPreview();
        }
      
        void OnModuleProjectChanged(object sender, EventArgs e)
        {
            _model.ModuleProject = _view.ActiveModuleProject;
            _view.ShowTestProject(_model.TestProjectExists);
            List<IProjectItemModel> moduleFolders = GetModuleFolders(_model.WebFolders);
            IProjectItemModel selectedModuleFolder = SelectWebFolder(moduleFolders, _model.ModuleProject, _model.WebProject, _model.ModuleInfoCollection);
            _view.SelectWebFolder(selectedModuleFolder);
        }

        private List<IProjectItemModel> GetModuleFolders(IList<IProjectItemModel> webFolders)
        {
            List<IProjectItemModel> moduleFolders = new List<IProjectItemModel>();
            moduleFolders.Add(new RootProjectItemModel("Root"));
            moduleFolders.AddRange(FilterModuleFolders(webFolders));
            if (_model.WebFolder!=null && !moduleFolders.Contains(_model.WebFolder))
            {
                moduleFolders.Add(_model.WebFolder);
            }
            return moduleFolders;
        }

        private IProjectItemModel SelectWebFolder(IList<IProjectItemModel> moduleFolders, IProjectModel moduleProject, IProjectModel webProject, IModuleInfo[] moduleInfoCollection)
        {
            if (moduleInfoCollection != null)
            {
                foreach (IModuleInfo moduleInfo in moduleInfoCollection)
                {
                    if (String.Compare(moduleInfo.AssemblyName, moduleProject.AssemblyName, StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        foreach (IProjectItemModel moduleFolder in moduleFolders)
                        {
                            if (moduleInfo.VirtualPath != null)
                            {
                                string moduleFolderPath = Path.Combine(webProject.ProjectPath, ConvertToPhysicalPath(moduleInfo.VirtualPath));
                                if (String.Compare(moduleFolder.ItemPath, moduleFolderPath, StringComparison.InvariantCultureIgnoreCase) == 0)
                                {
                                    return moduleFolder;
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

		private static string ConvertToPhysicalPath(string virtualPath)
		{
			string result = virtualPath.Replace("/", @"\");
			if (result.StartsWith("~"))
			{
				result = result.Remove(0, 1);
			}
			if (result.StartsWith(@"\"))
			{
				result = result.Remove(0, 1);
			}
			return result;
		}

        void OnWebFolderChanged(object sender, EventArgs e)
        {            
            _model.WebFolder = (_view.ActiveWebFolder is RootProjectItemModel) ? null : _view.ActiveWebFolder;            
        }

        void OnViewNameChanged(object sender, EventArgs e)
        {
            _model.ViewName = _view.ViewName;
        }
                   
        void OnRequestingValidation(object sender, EventArgs<bool> e)
        {
            bool validModel = _model.IsValid;
            if (!validModel)
            {
                _view.ShowValidationErrorMessage(_model.ValidationErrorMessage);
            }
            e.Data = validModel;
        }        

        private IList<IProjectItemModel> FilterModuleFolders(IList<IProjectItemModel> folders)
        {
            IList<IProjectItemModel> moduleFolders = new List<IProjectItemModel>();
            if (folders != null)
            {
                foreach (IProjectItemModel folder in folders)
                {
                    //DirectoryInfo di = new DirectoryInfo(folder.ItemPath);
                    //FileInfo[] modinfos = di.GetFiles(ModuleInfoFilePattern, SearchOption.TopDirectoryOnly);
                    //if (modinfos.Length > 0)
                    //{
                        moduleFolders.Add( folder );
                    //}
                }
            }
            return moduleFolders;
        }       

		class RootProjectItemModel : IProjectItemModel
		{
			private string _name;

			public RootProjectItemModel(string name)
			{
				_name = name;
			}
			public object ProjectItem
			{
				get { return new object(); }
			}

			public string Name
			{
				get { return _name; }
			}

			public string ItemPath
			{
				get { return null; }
			}
		}
	}
}

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
using System.Globalization;
using System.Text;
using System.ComponentModel.Design;
using Microsoft.Practices.RecipeFramework.Extensions.DteWrapper;

namespace WebClientFactoryPackage.Tests.Support
{
    /// <summary>
    /// Mock class for testing things that need the IProjectModel interface
    /// </summary>
    class MockProjectModel : IProjectModel
    {
        private string _containedFile = null;
        private ITypeResolutionService _typeResolutionService;
        private object _mockProject;
        //private string _responsibility;
        private string[] _responsibilities;
        private string _assemblyName;
        private string _name;
        private string _projectPath;
        private List<ITypeModel> _types = new List<ITypeModel>();
        private List<IProjectItemModel> _folders = new List<IProjectItemModel>();

        public object Project
        {
            get { return _mockProject; }
            set { _mockProject = value; }
        }        

        public MockProjectModel()
        {
            _mockProject = new object();
        }

        public bool ProjectContainsFile(string filename)
        {
            if (string.Compare(filename, _containedFile, true, CultureInfo.InvariantCulture) == 0)
            {
                return true;
            }
            return false;
        }

        public ITypeResolutionService TypeResolutionService
        {
            get { return _typeResolutionService; }
            set { _typeResolutionService = value; }
        }

        public IList<ITypeModel> Types
        {
            get { return _types; }
        }

        public void AddType(ITypeModel type)
        {
            _types.Add(type);
        }

        public void AddFolder(IProjectItemModel item)
        {
            _folders.Add(item);
        }

        //public string Responsibility
        //{
        //    get { return _responsibility; }
        //    set { _responsibility = value; }
        //}

        public string[] Responsibilities
        {
            get { return _responsibilities; }
            set { _responsibilities = value; }
        }

        public bool IsWebProject
        {
            get { return false; }
        }


        public IList<IProjectItemModel> Folders
        {
            get {
                return _folders;
            }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }


        public string AssemblyName
        {
            get { return _assemblyName; }
            set { _assemblyName = value; }
        }


        public string ProjectPath
        {
            get { return _projectPath; }
            set { _projectPath = value; }
        }
    }
}

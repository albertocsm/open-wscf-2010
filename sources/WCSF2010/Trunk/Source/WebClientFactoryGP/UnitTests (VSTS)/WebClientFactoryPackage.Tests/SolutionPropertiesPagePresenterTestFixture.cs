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
using System.IO;

namespace WebClientFactoryPackage.Tests
{
    /// <summary>
    /// Summary description for SolutionPropertiesPagePresenterTestFixture
    /// </summary>
    [TestClass]
    public class SolutionPropertiesPagePresenterTestFixture
    {
        public SolutionPropertiesPagePresenterTestFixture()
        {
        }

        private IDictionaryService dictionary;
        private MockSolutionPropertiesPage view;
        private SolutionPropertiesPagePresenter presenter;
        private SolutionPropertiesModel model;

        [TestInitialize]
        public void SetUp()
        {
            dictionary = new MockDictionaryService();
            view = new MockSolutionPropertiesPage();
            model = new SolutionPropertiesModel( dictionary );
            presenter = new SolutionPropertiesPagePresenter(view, model);

            dictionary.SetValue("CompositeWebDlls", "CompositeWeb.dll.txt");
            dictionary.SetValue("EnterpriseLibraryDlls", "EntLib.dll.txt");
        }

        [TestMethod]
        public void ShowDocumentationChanges()
        {
            view.FireShowDocumentationChanging(true);
            Assert.AreEqual(model.ShowDocumentation, view.ShowDocumentation);
        }

        [TestMethod]
        [DeploymentItem(@"Support\CompositeWeb.dll.txt", "Support")]
        [DeploymentItem(@"Support\EntLib.dll.txt", "Support")]
        public void ModelIsValidWithValidArguments()
        {
            view.FireSupportingLibrariesPathChanging((new System.IO.DirectoryInfo(@".\Support")).FullName);
            view.FireRootNamespaceChanging(@"SomeNamespace");
            
            Assert.AreEqual(view.SupportLibrariesPath, model.SupportLibrariesPath);
            Assert.AreEqual(view.RootNamespace, model.RootNamespace);
            Assert.IsTrue(model.IsValid);
        }

        [TestMethod]
        [DeploymentItem(@"Support\CompositeWeb.dll.txt", "Support")]
        public void ModelNotValidWhenSupportingLibrariesFolderIncorrect()
        {
            view.FireSupportingLibrariesPathChanging(@"not existis");
          
            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void ModelIsNotValidOnNullRootNamespace()
        {
            model.RootNamespace = null;

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void ModelIsNotValidOnEmptyRootNamespace()
        {
            model.RootNamespace = string.Empty;

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void ModelIsNotValidOnInvalidRootNamespace()
        {
            model.RootNamespace = "Root.Name Space";

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void ModelIsNotValidOnNullSupportLibrariesPath()
        {
            model.SupportLibrariesPath = null;

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void ModelIsNotValidOnEmptySupportLibrariesPath()
        {
            model.SupportLibrariesPath = string.Empty;

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void ModelIsNotValidOnNotExistingSupportLibrariesPath()
        {
            model.SupportLibrariesPath = "c:\\some path that should not exists";

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void ModelIsNotValidOnNullCWABDlls()
        {
            dictionary.SetValue("CompositeWebDlls", null);

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        [DeploymentItem(@"Support\CompositeWeb.dll.txt", "Support")]
        [DeploymentItem(@"Support\EntLib.dll.txt", "Support")]
        public void ModelIsValidWithValidArgumentsForVB()
        {
            dictionary.SetValue("RecipeLanguage", "VB");
            dictionary.SetValue("CompositeWebDlls", "CompositeWeb.dll.txt");
            dictionary.SetValue("EnterpriseLibraryDlls", "EntLib.dll.txt");

            model.SupportLibrariesPath = (new DirectoryInfo(".\\Support")).FullName;
            model.RootNamespace = @"SomeNamespace";

            Assert.IsTrue(model.IsValid);
        }
        
        [TestMethod]
        [DeploymentItem(@"Support\CompositeWeb.dll.txt")]
        public void ShouldNotValidateWhenFolderNotContainsCompositeDlls()
        {
            // simulate a missing dll by looking for an unexisting one
            dictionary.SetValue("CompositeWebDlls", "NonExists.dll");
            view.FireSupportingLibrariesPathChanging((new System.IO.DirectoryInfo(@".")).FullName);

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void ShouldNotValidateOnEmptyRootNamespace()
        {
            view.FireRootNamespaceChanging(String.Empty);

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void ShouldNotValidateInvalidRootNamespace()
        {
            view.FireRootNamespaceChanging("Root.Name Space");

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void ShowRootNamespaceOnViewReady()
        {
            dictionary.SetValue("RootNamespace", "Some.Namespace");

            presenter.OnViewReady();

            Assert.AreEqual( "Some.Namespace", view.RootNamespace);
        }

        [TestMethod]
        public void ShowSupportLibrariesPathOnViewReady()
        {
            dictionary.SetValue("SupportLibrariesPath", @"c:\some path");

            presenter.OnViewReady();

            Assert.AreEqual(@"c:\some path", view.SupportLibrariesPath);
        }

        [TestMethod]
        public void ShowSupportingLibrariesOnViewReady()
        {
            dictionary.SetValue("CompositeWebDlls", "Microsoft.Practices.CompositeWeb.dll;Microsoft.Practices.CompositeWeb.EnterpriseLibrary.dll;Microsoft.Practices.ObjectBuilder.dll");
            dictionary.SetValue("EnterpriseLibraryDlls", "Microsoft.Practices.EnterpriseLibrary.Common.dll;Microsoft.Practices.EnterpriseLibrary.Security.dll");
            
            presenter.OnViewReady();
            
            Assert.IsTrue(view.SupportLibraries.Length > 0);
        }

        [TestMethod]
        [DeploymentItem(@"Support\CompositeWeb.dll.txt","Support")]
        [DeploymentItem(@"Support\EntLib.dll.txt", "Support")]
        public void WhenSolutionSupportingLibrariesPathChangesUpdateMissingLibraries()
        {
            dictionary.SetValue("CompositeWebDlls", "CompositeWeb.dll.txt;MissingCompositeLibrary.dll");
            dictionary.SetValue("EnterpriseLibraryDlls", "EntLib.dll.txt;MissingEntlibLibrary.dll");
            view.FireSupportingLibrariesPathChanging((new System.IO.DirectoryInfo(@".\Support")).FullName);

            Assert.AreEqual(4, view.SupportLibraries.Length);
            Assert.AreEqual(4, view.MissingLibraries.Length);
            Assert.AreEqual("CompositeWeb.dll.txt", view.SupportLibraries[0]); 
            Assert.IsFalse(view.MissingLibraries[0]);
            Assert.AreEqual("MissingCompositeLibrary.dll", view.SupportLibraries[1]);
            Assert.IsTrue(view.MissingLibraries[1]);
            Assert.AreEqual("EntLib.dll.txt", view.SupportLibraries[2]);
            Assert.IsFalse(view.MissingLibraries[2]);
            Assert.AreEqual("MissingEntlibLibrary.dll", view.SupportLibraries[3]);
            Assert.IsTrue(view.MissingLibraries[3]);
        }

        [TestMethod]
        public void SetLanguageOnViewReady()
        {
            dictionary.SetValue("RecipeLanguage", "cs");
            presenter.OnViewReady();

            Assert.AreEqual(model.Language, view.Language);
            Assert.AreEqual("cs", view.Language);
        }

        [TestMethod]
        public void SetWebUIProjectNameOnViewReady()
        {
            dictionary.SetValue("WebUIProjectName", "WebApplication");
            presenter.OnViewReady();

            Assert.AreEqual(model.WebUIProjectName, view.WebUIProjectName);
            Assert.AreEqual("WebApplication", view.WebUIProjectName);
        }

        private class MockSolutionPropertiesPage : ISolutionPropertiesPage
        {
            public string[] SupportLibraries;
            public bool[] MissingLibraries;
            private string _language;
            private string _webUIProjectName;
            private bool _previewRefreshed;

            public void FireSupportingLibrariesPathChanging( string value)
            {
                _supportingLibrariesPath = value;
                if (SupportLibrariesPathChanging != null)
                    SupportLibrariesPathChanging(this, new EventArgs());
            }
            
            public void FireRootNamespaceChanging(string value)
            {
                _rootNamespace = value;
                if (RootNamespaceChanging != null)
                    RootNamespaceChanging(this, new EventArgs());
            }
            
            public event EventHandler<EventArgs> SupportLibrariesPathChanging;
            public event EventHandler<EventArgs> RootNamespaceChanging;

#pragma warning disable 0067
			public event EventHandler<Microsoft.Practices.RecipeFramework.Extensions.EventArgs<bool>> RequestingValidation;
#pragma warning restore 0067

			private string _supportingLibrariesPath;

            public string SupportLibrariesPath
            {
                get { return _supportingLibrariesPath; }
                set { _supportingLibrariesPath = value; }
            }            

            private string _rootNamespace;

            public string RootNamespace
            {
                get { return _rootNamespace; }
                set { _rootNamespace = value; }
            }           

            public void ShowSupportLibraries(string[] libraries, bool[] missing)
            {
                SupportLibraries = libraries;
                MissingLibraries = missing;
            }

            public void ShowSupportLibrariesPath(string path)
            {
                _supportingLibrariesPath = path;
            }

            public void ShowRootNamespace(string rootNamespace)
            {
                _rootNamespace = rootNamespace;
            }

            public bool PreviewRefreshed
            {
                get { return _previewRefreshed; }
            }

            public event EventHandler<EventArgs> ShowDocumentationChanging;

            private bool _showdocs; 
            public bool ShowDocumentation
            {
                get { return _showdocs; }
            }

            public void FireShowDocumentationChanging(bool value)
            {
                _showdocs = value;
                if (ShowDocumentationChanging != null)
                    ShowDocumentationChanging(this, new EventArgs());
            }

            public void SetLanguage(string language)
            {
                _language = language;
            }

             public void RefreshSolutionPreview()
            {
                _previewRefreshed = true;
            }

            public string Language
            {
                get { return _language; }
            }

            public void SetWebUIProjectName(string webUIProjectName)
            {
                _webUIProjectName = webUIProjectName;
            }

            public string WebUIProjectName
            {
                get
                {
                    return _webUIProjectName;
                }
            }
        }
    }
    

    class MockDictionaryService : IDictionaryService
    {
        IDictionary dictionary = new Hashtable();

        #region IDictionaryService Members

        public object GetKey(object value)
        {
            throw new NotImplementedException();
        }

        public object GetValue(object key)
        {
            return dictionary[key];
        }

        public void SetValue(object key, object value)
        {
            dictionary[key] = value;
        }

        #endregion
    }
   
}

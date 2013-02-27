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
using System.Windows.Forms;
using System.Globalization;
using Microsoft.Practices.RecipeFramework.Extensions.DteWrapper;

namespace Microsoft.Practices.WebClientFactory.CustomWizardPages
{
    public sealed class SolutionPreviewHelper
    {
        private string extension = "cs";
        private string projectIcon = "CSharpProjectIcon";
        private string itemIcon = "CSharpItemIcon";
        private string solutionFolderIcon = "SolutionFolderIcon";
        private string folderIcon = "FolderIcon";
        private string webUserControlIcon = "WebUserControlIcon";
        private string masterPageIcon = "MasterPageIcon";
        private string webProjectIcon = "WebProjectIcon";
        //private string componentProjectItemIcon = "ComponentProjectItemIcon";
        //private string designerProjectItemIcon = "DesignerProjectItemIcon";
        private string webFormIcon = "WebFormIcon";
        private string webConfigIcon = "WebConfigIcon";
        private string excludedFolderIcon = "ExcludedFolderIcon";
        private const string default_aspx_FileName = "Default.aspx";
        private const string web_config_FileName = "Web.config";
        private const string ellipsis = "...";
        private const string viewsFolderName = "Views";
        private const string servicesFolderName = "Services";
        private TreeView assembliesTreeView;

        //private TreeNode GetNode(TreeNode parent, string name, string imageKey, string selectedImageKey)
        //{
        //    TreeNode node = parent.Nodes.Add(name);
        //    node.ImageKey = imageKey;
        //    node.SelectedImageKey = selectedImageKey;

        //    return node;
        //}

        public SolutionPreviewHelper(TreeView assembliesTreeView, string language)
        {
            this.assembliesTreeView = assembliesTreeView;

            if ((language!=null) && (language.Equals("vb",StringComparison.InvariantCultureIgnoreCase)))
            {
                projectIcon = "VisualBasicProjectIcon";
                extension = "vb";
                itemIcon = "VisualBasicItemIcon";
            }
        }

        public void GetFoundationalModulePreview(bool withInterface, bool withTestProject, string solutionFolderName, string moduleName, string moduleProjectName)
        {
            assembliesTreeView.BeginUpdate();
            assembliesTreeView.Nodes.Clear();

            TreeNode rootNode = assembliesTreeView.Nodes.Add(solutionFolderName);
            rootNode.ImageKey = solutionFolderIcon;
            rootNode.SelectedImageKey = solutionFolderIcon;

			TreeNode moduleProjectNode = rootNode.Nodes.Add(moduleProjectName);
            moduleProjectNode.ImageKey = projectIcon;
            moduleProjectNode.SelectedImageKey = projectIcon;

            TreeNode servicesNode = moduleProjectNode.Nodes.Add(String.Format(CultureInfo.InvariantCulture,servicesFolderName));
            servicesNode.ImageKey = folderIcon;
            servicesNode.SelectedImageKey = folderIcon;

            TreeNode moduleInitializerNode = moduleProjectNode.Nodes.Add(string.Format(CultureInfo.InvariantCulture, "{0}ModuleInitializer.{1}", moduleName, extension));
            moduleInitializerNode.ImageKey = itemIcon;
            moduleInitializerNode.SelectedImageKey = itemIcon;

            if (withTestProject)
            {
				string testProjectName = string.Format(CultureInfo.InvariantCulture, "{0}.Tests", moduleProjectName);
                TreeNode testProjectNode = rootNode.Nodes.Add(testProjectName);
                testProjectNode.ImageKey = projectIcon;
                testProjectNode.SelectedImageKey = projectIcon;

                string initializerFixtureName = string.Format(CultureInfo.InvariantCulture, "{0}ModuleInitializerFixture.{1}", moduleName, extension);
                TreeNode initializerFixtureNode = testProjectNode.Nodes.Add(initializerFixtureName);
                initializerFixtureNode.ImageKey = itemIcon;
                initializerFixtureNode.SelectedImageKey = itemIcon;

                testProjectNode.Expand();
            }

            if (withInterface)
            {
                string moduleInterfaceName = String.Format(CultureInfo.InvariantCulture, "{0}.Interface", moduleName);
                TreeNode moduleInterfaceProjectNode = rootNode.Nodes.Add(moduleInterfaceName);
                moduleInterfaceProjectNode.ImageKey = projectIcon;
                moduleInterfaceProjectNode.SelectedImageKey = projectIcon;

                TreeNode servicesInterfaceNode = moduleInterfaceProjectNode.Nodes.Add(String.Format(CultureInfo.InvariantCulture, servicesFolderName));
                servicesInterfaceNode.ImageKey = folderIcon;
                servicesInterfaceNode.SelectedImageKey = folderIcon;

                moduleInterfaceProjectNode.Expand();
            }

            rootNode.Expand();
            moduleProjectNode.Expand();
            assembliesTreeView.EndUpdate();
        }

        public void GetSolutionPreview(string webUIProjectName)
        {
            assembliesTreeView.BeginUpdate();
            assembliesTreeView.Nodes.Clear();

            TreeNode rootNode;

            rootNode = assembliesTreeView.Nodes.Add(String.Format(CultureInfo.InvariantCulture,"Source"));
            rootNode.ImageKey = solutionFolderIcon;
            rootNode.SelectedImageKey = solutionFolderIcon;

            TreeNode modulesNode = rootNode.Nodes.Add(String.Format(CultureInfo.InvariantCulture,"Modules"));
            modulesNode.ImageKey = solutionFolderIcon;
            modulesNode.SelectedImageKey = solutionFolderIcon;

            TreeNode shellProjectNode = modulesNode.Nodes.Add("Shell", String.Format(CultureInfo.InvariantCulture,"Shell"));
            shellProjectNode.ImageKey = projectIcon;
            shellProjectNode.SelectedImageKey = projectIcon;

            TreeNode dotNode = shellProjectNode.Nodes.Add(String.Format(CultureInfo.InvariantCulture,ellipsis));
            dotNode.ImageKey = itemIcon;
            dotNode.SelectedImageKey = itemIcon;

            TreeNode moduleInitializerNode = shellProjectNode.Nodes.Add(string.Format(CultureInfo.InvariantCulture, "ShellModuleInitializer.{0}", extension));
            moduleInitializerNode.ImageKey = itemIcon;
            moduleInitializerNode.SelectedImageKey = itemIcon;

            TreeNode webSitesNode = rootNode.Nodes.Add(String.Format(CultureInfo.InvariantCulture,"WebSites"));
            webSitesNode.ImageKey = solutionFolderIcon;
            webSitesNode.SelectedImageKey = solutionFolderIcon;

            TreeNode webProjectNode = webSitesNode.Nodes.Add("DevelopmentWebsite", String.Format(CultureInfo.InvariantCulture, webUIProjectName));
            webProjectNode.ImageKey = webProjectIcon;
            webProjectNode.SelectedImageKey = webProjectIcon;

            TreeNode errorsNode = webProjectNode.Nodes.Add("Errors", String.Format(CultureInfo.InvariantCulture,"Errors"));
            errorsNode.ImageKey = folderIcon;
            errorsNode.SelectedImageKey = folderIcon;

            TreeNode sharedNode = webProjectNode.Nodes.Add("Shared", String.Format(CultureInfo.InvariantCulture,"Shared"));
            sharedNode.ImageKey = folderIcon;
            sharedNode.SelectedImageKey = folderIcon;

            TreeNode defaultNode = webProjectNode.Nodes.Add(String.Format(CultureInfo.InvariantCulture,default_aspx_FileName));
            defaultNode.ImageKey = webFormIcon;
            defaultNode.SelectedImageKey = webFormIcon;

            TreeNode webConfigNode = webProjectNode.Nodes.Add(String.Format(CultureInfo.InvariantCulture,web_config_FileName));
            webConfigNode.ImageKey = webConfigIcon;
            webConfigNode.SelectedImageKey = webConfigIcon;

            rootNode.Expand();
            modulesNode.Expand();
            webSitesNode.Expand();
            shellProjectNode.Expand();
            webProjectNode.Expand();

            assembliesTreeView.EndUpdate();
        }


        public void GetAddViewWebsitePreview(string viewName, string viewFileExtension, IProjectItemModel webFolder, IProjectModel webProject)
        {
            if ((webProject != null) 
                && (webFolder != null))
            {

                assembliesTreeView.BeginUpdate();
                assembliesTreeView.Nodes.Clear();

                string website=string.Empty;
                if (webProject != null)
                {
                    website = webProject.Name;
                }
                TreeNode websiteNode = assembliesTreeView.Nodes.Add(website);
                websiteNode.ImageKey = webProjectIcon;
                websiteNode.SelectedImageKey = webProjectIcon;

                TreeNode viewContainerNode;
                if (webFolder.Name == "Root")
                {
                    viewContainerNode = websiteNode;
                }
                else
                {
                    TreeNode parentNode = websiteNode;
                    string folderPath = webFolder.ItemPath;
                    string webSitePath = webProject.ProjectPath;

                    if (folderPath.StartsWith(webSitePath))
                    {
                        folderPath = folderPath.Replace(webSitePath, String.Empty);
                        folderPath = System.IO.Path.GetDirectoryName(folderPath);

                        string[] folders = folderPath.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string innerFolder in folders)
                        {
                            parentNode = parentNode.Nodes.Add(innerFolder);
                            parentNode.ImageKey = solutionFolderIcon;
                            parentNode.SelectedImageKey = solutionFolderIcon;
                        }
                    }

                    string folder = webFolder.Name;
                    viewContainerNode = parentNode.Nodes.Add(folder);
                    viewContainerNode.ImageKey = solutionFolderIcon;
                    viewContainerNode.SelectedImageKey = solutionFolderIcon;
                }

                TreeNode genericNode1 = viewContainerNode.Nodes.Add(String.Format(CultureInfo.InvariantCulture,ellipsis));
                genericNode1.ImageKey = webFormIcon;
                genericNode1.SelectedImageKey = webFormIcon;

                TreeNode viewNode = viewContainerNode.Nodes.Add(string.Format(CultureInfo.InvariantCulture, "{0}.{1}", viewName ,viewFileExtension));
                if (viewFileExtension == "ascx")
                {
                    viewNode.ImageKey = webUserControlIcon;
                    viewNode.SelectedImageKey = webUserControlIcon;
                }
                else if (viewFileExtension == "master")
                {
                    viewNode.ImageKey = masterPageIcon;
                    viewNode.SelectedImageKey = masterPageIcon;
                }
                else
                {
                    viewNode.ImageKey = webFormIcon;
                    viewNode.SelectedImageKey = webFormIcon;
                }

                TreeNode viewCodebehindNode = viewNode.Nodes.Add(string.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}", viewName,viewFileExtension, extension));
                viewCodebehindNode.ImageKey = itemIcon;
                viewCodebehindNode.SelectedImageKey = itemIcon;

                TreeNode genericNode2 = viewContainerNode.Nodes.Add(String.Format(CultureInfo.InvariantCulture, ellipsis));
                genericNode2.ImageKey = webFormIcon;
                genericNode2.SelectedImageKey = webFormIcon;

                assembliesTreeView.ExpandAll();
                assembliesTreeView.EndUpdate();
            }
        }

        public void GetAddViewModulePreview(string moduleName, string viewName, string viewFileExtension, bool testProjectExists)
        {
            assembliesTreeView.BeginUpdate();
            assembliesTreeView.Nodes.Clear();

            TreeNode moduleProjectNode = assembliesTreeView.Nodes.Add(moduleName);
            moduleProjectNode.ImageKey = projectIcon;
            moduleProjectNode.SelectedImageKey = projectIcon;

            TreeNode servicesNode = moduleProjectNode.Nodes.Add(String.Format(CultureInfo.InvariantCulture,servicesFolderName));
            servicesNode.ImageKey = folderIcon;
            servicesNode.SelectedImageKey = folderIcon;

            string folderName;
            if (viewFileExtension == "master")
            {
                folderName = "MasterPages";
            }
            else
            {
                folderName = "Views";
            }
            TreeNode viewsNode = moduleProjectNode.Nodes.Add(folderName);
            viewsNode.ImageKey = folderIcon;
            viewsNode.SelectedImageKey = folderIcon;

            TreeNode viewPresenterNode = viewsNode.Nodes.Add(string.Format(CultureInfo.InvariantCulture, "{0}Presenter.{1}", viewName, extension));
            viewPresenterNode.ImageKey = itemIcon;
            viewPresenterNode.SelectedImageKey = itemIcon;

            TreeNode iViewNode = viewsNode.Nodes.Add(string.Format(CultureInfo.InvariantCulture, "I{0}View.{1}", viewName, extension));
            iViewNode.ImageKey = itemIcon;
            iViewNode.SelectedImageKey = itemIcon;

            TreeNode genericNode2 = viewsNode.Nodes.Add(String.Format(CultureInfo.InvariantCulture,ellipsis));
            genericNode2.ImageKey = itemIcon;
            genericNode2.SelectedImageKey = itemIcon;

            TreeNode genericNode3 = moduleProjectNode.Nodes.Add(String.Format(CultureInfo.InvariantCulture,ellipsis));
            genericNode3.ImageKey = itemIcon;
            genericNode3.SelectedImageKey = itemIcon;

            TreeNode moduleInitializerNode = moduleProjectNode.Nodes.Add(string.Format(CultureInfo.InvariantCulture, "{0}ModuleInitializer.{1}", moduleName, extension));
            moduleInitializerNode.ImageKey = itemIcon;
            moduleInitializerNode.SelectedImageKey = itemIcon;

            if (testProjectExists)
            {
                string testProjectName = string.Format(CultureInfo.InvariantCulture, "{0}.Tests", moduleName);
                TreeNode testProjectNode = assembliesTreeView.Nodes.Add(testProjectName);
                testProjectNode.ImageKey = projectIcon;
                testProjectNode.SelectedImageKey = projectIcon;
                testProjectNode.Expand();

                TreeNode testViewsNode = testProjectNode.Nodes.Add(folderName);
                testViewsNode.ImageKey = folderIcon;
                testViewsNode.SelectedImageKey = folderIcon;

                TreeNode fixtureNode = testViewsNode.Nodes.Add(string.Format(CultureInfo.InvariantCulture, "{0}PresenterFixture.{1}", viewName, extension));
                fixtureNode.ImageKey = itemIcon;
                fixtureNode.SelectedImageKey = itemIcon;
            }

            moduleProjectNode.ExpandAll();
            assembliesTreeView.EndUpdate();
        }


        public void GetBusinessModuleProjectPreview(string moduleName, string moduleProjectName, string moduleSolutionFolder, bool withTestProject, bool withInterface)
        {
            assembliesTreeView.BeginUpdate();
            assembliesTreeView.Nodes.Clear();

            TreeNode rootNode;
            rootNode = assembliesTreeView.Nodes.Add(moduleSolutionFolder);
            rootNode.ImageKey = solutionFolderIcon;
            rootNode.SelectedImageKey = solutionFolderIcon;

			TreeNode moduleProjectNode = rootNode.Nodes.Add(moduleProjectName);
            moduleProjectNode.ImageKey = projectIcon;
            moduleProjectNode.SelectedImageKey = projectIcon;

            TreeNode servicesNode = moduleProjectNode.Nodes.Add(String.Format(CultureInfo.InvariantCulture,servicesFolderName));
            servicesNode.ImageKey = folderIcon;
            servicesNode.SelectedImageKey = folderIcon;

            TreeNode viewsNode = moduleProjectNode.Nodes.Add(String.Format(CultureInfo.InvariantCulture,viewsFolderName));
            viewsNode.ImageKey = folderIcon;
            viewsNode.SelectedImageKey = folderIcon;

            TreeNode defaultViewPresenterNode = viewsNode.Nodes.Add(string.Format(CultureInfo.InvariantCulture, "DefaultViewPresenter.{0}", extension));
            defaultViewPresenterNode.ImageKey = itemIcon;
            defaultViewPresenterNode.SelectedImageKey = itemIcon;
            defaultViewPresenterNode.ToolTipText = "Presenter corresponding to the Default.aspx view";

            TreeNode iDefaultViewNode = viewsNode.Nodes.Add(string.Format(CultureInfo.InvariantCulture, "IDefaultView.{0}", extension));
            iDefaultViewNode.ImageKey = itemIcon;
            iDefaultViewNode.SelectedImageKey = itemIcon;

            string controllerNodeName = string.Format(CultureInfo.InvariantCulture, "{0}Controller.{1}", moduleName, extension);

            TreeNode controllerInterfaceNode = moduleProjectNode.Nodes.Add(string.Format(CultureInfo.InvariantCulture, "I{0}", controllerNodeName));
            controllerInterfaceNode.ImageKey = itemIcon;
            controllerInterfaceNode.SelectedImageKey = itemIcon;

            TreeNode controllerNode = moduleProjectNode.Nodes.Add(controllerNodeName);
            controllerNode.ImageKey = itemIcon;
            controllerNode.SelectedImageKey = itemIcon;

            TreeNode moduleInitializerNode = moduleProjectNode.Nodes.Add(string.Format(CultureInfo.InvariantCulture, "{0}ModuleInitializer.{1}", moduleName, extension));
            moduleInitializerNode.ImageKey = itemIcon;
            moduleInitializerNode.SelectedImageKey = itemIcon;

            if (withTestProject)
            {
				string testProjectName = string.Format(CultureInfo.InvariantCulture, "{0}.Tests", moduleProjectName);
                TreeNode testProjectNode = rootNode.Nodes.Add(testProjectName);
                testProjectNode.ImageKey = projectIcon;
                testProjectNode.SelectedImageKey = projectIcon;

                TreeNode mocksNode = testProjectNode.Nodes.Add(String.Format(CultureInfo.InvariantCulture,"Mocks"));
                mocksNode.ImageKey = folderIcon;
                mocksNode.SelectedImageKey = folderIcon;

                string mockControllerName = string.Format(CultureInfo.InvariantCulture, "Mock{0}Controller.{1}", moduleName, extension);
                TreeNode mockControllerNode = mocksNode.Nodes.Add(mockControllerName);
                mockControllerNode.ImageKey = itemIcon;
                mockControllerNode.SelectedImageKey = itemIcon;

                TreeNode testViewsNode = testProjectNode.Nodes.Add(String.Format(CultureInfo.InvariantCulture,viewsFolderName));
                testViewsNode.ImageKey = folderIcon;
                testViewsNode.SelectedImageKey = folderIcon;

                TreeNode fixtureNode = testViewsNode.Nodes.Add(string.Format(CultureInfo.InvariantCulture, "DefaultViewPresenterFixture.{0}", extension));
                fixtureNode.ImageKey = itemIcon;
                fixtureNode.SelectedImageKey = itemIcon;

                string controllerFixtureName = string.Format(CultureInfo.InvariantCulture, "{0}ModuleControllerFixture.{1}", moduleName, extension);
                TreeNode controllerFixtureNode = testProjectNode.Nodes.Add(controllerFixtureName);
                controllerFixtureNode.ImageKey = itemIcon;
                controllerFixtureNode.SelectedImageKey = itemIcon;

                string initializerFixtureName = string.Format(CultureInfo.InvariantCulture, "{0}ModuleInitializerFixture.{1}", moduleName, extension);
                TreeNode initializerFixtureNode = testProjectNode.Nodes.Add(initializerFixtureName);
                initializerFixtureNode.ImageKey = itemIcon;
                initializerFixtureNode.SelectedImageKey = itemIcon;

                testProjectNode.Expand();
            }

            if (withInterface)
            {
                string moduleInterfaceName = String.Format(CultureInfo.InvariantCulture, "{0}.Interface", moduleName);
                TreeNode moduleInterfaceProjectNode = rootNode.Nodes.Add(moduleInterfaceName);
                moduleInterfaceProjectNode.ImageKey = projectIcon;
                moduleInterfaceProjectNode.SelectedImageKey = projectIcon;

                TreeNode servicesInterfaceNode = moduleInterfaceProjectNode.Nodes.Add(String.Format(CultureInfo.InvariantCulture, servicesFolderName));
                servicesInterfaceNode.ImageKey = folderIcon;
                servicesInterfaceNode.SelectedImageKey = folderIcon;

                moduleInterfaceProjectNode.Expand();
            }

            rootNode.Expand();
            moduleProjectNode.Expand();
            assembliesTreeView.EndUpdate();
        }

        public void GetBusinessModuleWebsitePreview(string projectName, string moduleFolderName, string moduleWebsiteName, bool createAsFolderInWebsite)
        {
            assembliesTreeView.BeginUpdate();
            assembliesTreeView.Nodes.Clear();

            TreeNode moduleFolderNode = null;

            if (createAsFolderInWebsite)
            {
                TreeNode websiteNode = assembliesTreeView.Nodes.Add(projectName);
                websiteNode.ImageKey = webProjectIcon;
                websiteNode.SelectedImageKey = webProjectIcon;

                moduleFolderNode = websiteNode.Nodes.Add(moduleFolderName);
                moduleFolderNode.ImageKey = folderIcon;
                moduleFolderNode.SelectedImageKey = folderIcon;
            }
            else
            {
                TreeNode mainWebsiteNode = assembliesTreeView.Nodes.Add(projectName);
                mainWebsiteNode.ImageKey = webProjectIcon;
                mainWebsiteNode.SelectedImageKey = webProjectIcon;

                TreeNode moduleFolderInMainWebNode = mainWebsiteNode.Nodes.Add(moduleFolderName);
                moduleFolderInMainWebNode.ImageKey = excludedFolderIcon;
                moduleFolderInMainWebNode.SelectedImageKey = excludedFolderIcon;

                TreeNode websiteNode = assembliesTreeView.Nodes.Add(moduleWebsiteName);
                websiteNode.ImageKey = webProjectIcon;
                websiteNode.SelectedImageKey = webProjectIcon;

                moduleFolderNode = websiteNode;
            }

            //Ignore the language in the preview

            TreeNode defaultPageNode = moduleFolderNode.Nodes.Add(String.Format(CultureInfo.InvariantCulture,default_aspx_FileName));
            defaultPageNode.ImageKey = webFormIcon;
            defaultPageNode.SelectedImageKey = webFormIcon;

            TreeNode webConfigNode = moduleFolderNode.Nodes.Add(String.Format(CultureInfo.InvariantCulture,web_config_FileName));
            webConfigNode.ImageKey = webConfigIcon;
            webConfigNode.SelectedImageKey = webConfigIcon;
            assembliesTreeView.ExpandAll();
            assembliesTreeView.EndUpdate();
        }

    }
}

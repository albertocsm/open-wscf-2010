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
using Microsoft.Practices.Common;
using Microsoft.Practices.CompositeWeb.Configuration;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Practices.WebClientFactory.Properties;
using System.Globalization;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.RecipeFramework.Library;
using System.ComponentModel.Design;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.CompositeWeb.Services;
using System.Reflection;
using Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers;
using Microsoft.Practices.CompositeWeb.Interfaces;
using EnvDTE;

namespace Microsoft.Practices.WebClientFactory.ValueProviders
{
    public class SelectedModuleInfoProvider : ValueProvider, IAttributesConfigurable
    {
        private string _webProjectExpression;
        private string _webFolderExpression;
        private string _moduleInfoListExpression;

        #region Overrides

        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            if (currentValue == null)
            {
                return Evaluate(out newValue);
            }
            newValue = currentValue;
            return false;
        }

        private bool Evaluate(out object newValue)
        {
            newValue = null;
            IModuleInfo[] moduleInfoList = (IModuleInfo[])ExpressionEvaluationHelper.EvaluateExpression((IDictionaryService)GetService(typeof(IDictionaryService)),
                                                                                                        _moduleInfoListExpression);
            Project webProject;
            ProjectItem folder;
            IsWebProjectSelected(out webProject, out folder);
            IModuleInfo found;
            if (folder != null)
            {
                found = FindModuleInfo(moduleInfoList, folder);
            }
            else if (IsFolderOfRootWebProject(webProject))
            {
                found = FindModuleInfoInSubWebProject(moduleInfoList, webProject);
            }
            else
            {
                found = FindRootVirtualPathModuleInfo(moduleInfoList);
            }
            newValue = found;
            return true;
        }

        private IModuleInfo FindModuleInfoInSubWebProject(IModuleInfo[] moduleInfos, Project webProject)
        {
            string projectAbosultePath = RemoveTrailingWack(webProject.Properties.Item("FullPath").Value.ToString());

            while (projectAbosultePath.Length > 0)
            {
                foreach (IModuleInfo moduleInfo in moduleInfos)
                {
                    if (moduleInfo.VirtualPath != null)
                    {
                        string moduleInfoAbsolutePath = BuildModuleAbsoluteFolderPath(webProject, moduleInfo);
                        if (String.Equals(projectAbosultePath, moduleInfoAbsolutePath, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return moduleInfo;
                        }
                    }
                }
                projectAbosultePath = Path.GetDirectoryName(projectAbosultePath);
            }
            return null;
        }

        private static bool IsFolderOfRootWebProject(Project webProject)
        {
            string IsFolderOfRootWebProjectKey = "IsFolderOfRootWebProject";
            if (webProject.Globals.get_VariableExists(IsFolderOfRootWebProjectKey))
            {
                string propertyValue = webProject.Globals[IsFolderOfRootWebProjectKey] as string;
                bool propertyValueBoolean;

                if (propertyValue != null && Boolean.TryParse(propertyValue, out propertyValueBoolean))
                {
                    return propertyValueBoolean;
                }
            }
            return false;
        }
        #endregion

        private IModuleInfo FindModuleInfo(IModuleInfo[] moduleInfos, ProjectItem selectedFolder)
        {
            string folderAbosultePath = RemoveTrailingWack(selectedFolder.Properties.Item("FullPath").Value.ToString());

            while (folderAbosultePath.Length > 0)
            {
                foreach (IModuleInfo moduleInfo in moduleInfos)
                {
                    if (moduleInfo.VirtualPath != null)
                    {
                        string moduleInfoAbsolutePath = BuildModuleAbsoluteFolderPath(selectedFolder.ContainingProject, moduleInfo);
                        if (String.Equals(folderAbosultePath, moduleInfoAbsolutePath, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return moduleInfo;
                        }
                    }
                }
                folderAbosultePath = Path.GetDirectoryName(folderAbosultePath);
            }
            return null;
        }

        private IModuleInfo FindRootVirtualPathModuleInfo(IModuleInfo[] moduleInfos)
        {
            foreach (IModuleInfo moduleInfo in moduleInfos)
            {
                if (moduleInfo.VirtualPath != null && RemoveTrailingWack(moduleInfo.VirtualPath).Equals("~", StringComparison.InvariantCultureIgnoreCase))
                {
                    return moduleInfo;
                }                
            }
            return null;
        }

        private static string BuildModuleAbsoluteFolderPath(Project webProject, IModuleInfo moduleInfo)
        {
            string moduleFolderPath = ConvertToPhysicalPath(moduleInfo.VirtualPath);
            string rootWebProjectPath = webProject.Properties.Item("FullPath").Value.ToString();
            if (IsFolderOfRootWebProject(webProject))
            {
                rootWebProjectPath = rootWebProjectPath.Remove(RemoveTrailingWack(rootWebProjectPath).LastIndexOf('\\'));
            }
            return RemoveTrailingWack(Path.Combine(rootWebProjectPath, moduleFolderPath));
        }

        private static string RemoveTrailingWack(string folderPath)
        {
            folderPath = folderPath.EndsWith(@"\") ? folderPath.Remove(folderPath.LastIndexOf(@"\"), 1) : folderPath;
            folderPath = folderPath.EndsWith(@"/") ? folderPath.Remove(folderPath.LastIndexOf(@"/"), 1) : folderPath;
            return folderPath;
        }

        private bool IsWebProjectSelected(out Project webProject, out ProjectItem folder)
        {
            webProject = null;
            folder = null;
            object project = ExpressionEvaluationHelper.EvaluateExpression((IDictionaryService)GetService(typeof(IDictionaryService)),
                                                                            _webProjectExpression);
            if (project == null)
            {
                object projectItem = ExpressionEvaluationHelper.EvaluateExpression((IDictionaryService)GetService(typeof(IDictionaryService)),
                                                                                _webFolderExpression);
                if (projectItem != null)
                {
                    folder = projectItem as ProjectItem;
                    return false;
                }
            }
            else
            {
                webProject = project as Project;
                return true;
            }
            return true;
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

        public void Configure(System.Collections.Specialized.StringDictionary attributes)
        {
            if (attributes != null)
            {
                if (attributes.ContainsKey("WebProjectExpression"))
                {
                    _webProjectExpression = attributes["WebProjectExpression"];
                }
                if (attributes.ContainsKey("WebFolderExpression"))
                {
                    _webFolderExpression = attributes["WebFolderExpression"];
                }
                if (attributes.ContainsKey("ModuleInfoListExpression"))
                {
                    _moduleInfoListExpression = attributes["ModuleInfoListExpression"];
                }
            }
        }
    }
}

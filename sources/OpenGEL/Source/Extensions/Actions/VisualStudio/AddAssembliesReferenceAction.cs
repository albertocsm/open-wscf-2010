//===============================================================================
// Microsoft patterns & practices
//  GAX Extension Library
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
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.RecipeFramework.Library;
using VsWebSite;
using System.IO;
using VSLangProj;
using EnvDTE;

namespace Microsoft.Practices.RecipeFramework.Extensions.Actions.VisualStudio
{
    /// <summary>
    /// Add multiple assemblies reference to a project given a base path and the file names separated by semi-colon. GACs can also be added.
    /// </summary>
    public class AddAssembliesReferenceAction : ConfigurableAction
    {
        #region Input Properties
        private Project _referringProject;
        private string _fileNames;
        private string _assembliesPath;

        /// <summary>
        /// Assembly filenames separated by semi-colon
        /// </summary>
        /// <remarks>
        /// If the assemblies are in the GAC this property can have assembly fullname. Don't need to use AssembliesPath.
        /// </remarks>
        [Input(Required = true)]
        public string FileNames
        {
            get { return _fileNames; }
            set { _fileNames = value; }
        }

        /// <summary>
        /// The project where the reference/s is been added
        /// </summary>
        [Input(Required = true)]
        public Project ReferringProject
        {
            get { return _referringProject; }
            set { _referringProject = value; }
        }

        /// <summary>
        /// Directory where the assemblies reside
        /// </summary>
        [Input(Required = false)]
        public string AssembliesPath
        {
            get { return _assembliesPath; }
            set { _assembliesPath = value; }
        }

        #endregion

        #region IAction Members

        private void GuardFolderExists(string folder)
        {
            if (!Directory.Exists(folder))
            {
                throw new InvalidOperationException(
                    String.Format(
                        Properties.Resources.FolderDoesNotExist,
                        folder));
            }
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            bool gaced = (_assembliesPath == null);
            List<string> assemblies = BuildAssembliesPathList(gaced);       

            if (DteHelper.IsWebProject(_referringProject))
            {
                // This is a web project
                VSWebSite webProject = _referringProject.Object as VSWebSite;
                if (webProject != null)
                {
                    foreach (string assemblyFilePath in assemblies) 
                    {
                        if (gaced)
                        {
                            webProject.References.AddFromGAC(assemblyFilePath); 
                        }
                        else
                        {
                            webProject.References.AddFromFile(assemblyFilePath);
                        }
                    }                                       
                }
            }
            else
            {
                // standard project
                VSProject vsProject = _referringProject.Object as VSProject;
                if (vsProject != null)
                {
                    foreach (string assemblyFilePath in assemblies)
                    {
                        vsProject.References.Add(assemblyFilePath);                        
                    } 
                    
                }                
            }
        }

        private List<string> BuildAssembliesPathList(bool gaced)
        {
            List<string> assembliesPath = new List<string>();

            if (gaced)
            {
                assembliesPath.AddRange(_fileNames.Split(';'));
            }
            else
            {
                GuardFolderExists(_assembliesPath);

                foreach (string file in _fileNames.Split(';'))
                {
                    string assemblyFilePath = Path.Combine(_assembliesPath, file);

                    if (File.Exists(assemblyFilePath))
                    {
                        assembliesPath.Add(assemblyFilePath);
                    }
                }
            }
            return assembliesPath;
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        public override void Undo()
        {
            // No undo supported as no Remove method exists on the VSProject.References property.
        }

        #endregion
    }
}

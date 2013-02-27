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
using Microsoft.Practices.RecipeFramework;
using EnvDTE;
using System.IO;

namespace Microsoft.Practices.RecipeFramework.Extensions.Actions.OS
{
    /// <summary>
    /// Create a directory under the solution path
    /// </summary>
    public class CreateDirectoryAction : ConfigurableAction
    {
        private string _fullPath;
        private string _directoryName;

        /// <summary>
        /// Directory name to create
        /// </summary>
        [Input(Required = true)]
        public string DirectoryName
        {
            get
            {
                return _directoryName;
            }
            set
            {
                _directoryName = value;
            }
        }

        
        /// <summary>
        /// Full path of the directory created
        /// </summary>
        [Output]
        public string FullPath
        {
            get
            {
                return _fullPath;
            }
            set
            {
                _fullPath = value;
            }
        }

        /// <summary>
        /// Gets the solution path
        /// </summary>
        /// <returns></returns>
        protected virtual string GetSolutionPath()
        {
            DTE dte = GetService<DTE>();
            return Path.GetDirectoryName(dte.Solution.Properties.Item("Path").Value.ToString());
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            string solutionPath = GetSolutionPath();
            _fullPath = Path.Combine(solutionPath, _directoryName);
            if (_fullPath.Contains(".."))
            {
                throw new InvalidOperationException(
                    String.Format(Properties.Resources.CreateSolutionDirectory_NoDots, _fullPath));
            }
            Directory.CreateDirectory(_fullPath);
        }

        /// <summary>
        /// Undo the action. Not implemented
        /// </summary>
        public override void Undo()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}

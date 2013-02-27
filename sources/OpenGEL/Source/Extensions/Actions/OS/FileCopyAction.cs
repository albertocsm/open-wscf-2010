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
using System.IO;
using Microsoft.Practices.RecipeFramework;

namespace Microsoft.Practices.RecipeFramework.Extensions.Actions.OS
{
    /// <summary>
    /// Copy the files (separated by ;) from source dir to target dir
    /// </summary>
    public class FileCopyAction : ConfigurableAction
    {
        private string _fileNames;
        private string _sourceDir;
        private string _targetDir;

        /// <summary>
        /// Target directory
        /// </summary>
        [Input(Required = true)]        
        public string TargetDir
        {
            get { return _targetDir; }
            set { _targetDir = value; }
        }

        /// <summary>
        /// Source directory
        /// </summary>
        [Input(Required = true)]
        public string SourceDir
        {
            get { return _sourceDir; }
            set { _sourceDir = value; }
        }

        /// <summary>
        /// Filenames to copy separated by semi-colon
        /// </summary>
        [Input(Required = true)]
        public string FileNames
        {
            get { return _fileNames; }
            set { _fileNames = value; }
        }

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
            GuardFolderExists(this._sourceDir);
            GuardFolderExists(this._targetDir);

            string[] fileNames = _fileNames.Split(';');

            foreach (string file in fileNames)
            {
                string sourceFile = Path.Combine(this._sourceDir, file);

                if (File.Exists(sourceFile))
                {
                    string targetFile = Path.Combine(this._targetDir, file);
                    File.Copy(sourceFile, targetFile, true);
                }
            }
        }

        /// <summary>
        /// Undo the action
        /// </summary>
        public override void Undo()
        {
            string[] fileNames = _fileNames.Split(';');

            if (Directory.Exists(_targetDir))
            {
                foreach (string file in fileNames)
                {
                    string targetFile = Path.Combine(this._targetDir, file);
                    File.Delete(targetFile);
                }
            }
        }
    }
}

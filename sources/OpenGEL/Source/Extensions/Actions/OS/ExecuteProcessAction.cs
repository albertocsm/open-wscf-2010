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
#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using System.Diagnostics;
using System.IO; 
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.Actions.OS
{
    /// <summary>
    /// Action that executes a process
    /// </summary>
    public class ExecuteProcessAction : ConfigurableAction
    {
        #region Input Properties
        private string executablePath = "";
        /// <summary>
        /// Gets or sets the executable path.
        /// </summary>
        /// <value>The executable path.</value>
        [Input(Required = false)]
        public string ExecutablePath
        {
            get { return executablePath; }
            set { executablePath = value; }
        }
        private string executableFile = "";
        /// <summary>
        /// Gets or sets the executable file.
        /// </summary>
        /// <value>The executable file.</value>
        [Input(Required = false)]
        public string ExecutableFile
        {
            get { return executableFile; }
            set { executableFile = value; }
        }

        private string executableArguments = "";
        /// <summary>
        /// Gets or sets the executable arguments.
        /// </summary>
        /// <value>The executable arguments.</value>
        [Input(Required = false)]
        public string ExecutableArguments
        {
            get { return executableArguments; }
            set { executableArguments = value; }
        }

        private bool createNoWindow = true;
        /// <summary>
        /// Gets or sets a value indicating whether [create no window].
        /// </summary>
        /// <value><c>true</c> if [create no window]; otherwise, <c>false</c>.</value>
        [Input(Required = false)]
        public bool CreateNoWindow
        {
            get { return createNoWindow; }
            set { createNoWindow = value; }
        }

        private bool waitForExit = true;
        /// <summary>
        /// Gets or sets a value indicating whether [wait for exit].
        /// </summary>
        /// <value><c>true</c> if [wait for exit]; otherwise, <c>false</c>.</value>
        [Input(Required = false)]
        public bool WaitForExit
        {
            get { return waitForExit; }
            set { waitForExit = value; }
        }

        private ProcessWindowStyle processWindowStyle = ProcessWindowStyle.Hidden;
        /// <summary>
        /// Gets or sets the process window style.
        /// </summary>
        /// <value>The process window style.</value>
        [Input(Required = false)]
        public ProcessWindowStyle ProcessWindowStyle
        {
            get { return processWindowStyle; }
            set { processWindowStyle = value; }
        }
        #endregion

        #region ConfigurableAction Implementation
        /// <summary>
        /// See <see cref="M:Microsoft.Practices.RecipeFramework.IAction.Execute"/>.
        /// </summary>
        public override void Execute()
        {
            if(this.executableFile != "")
            {
                executablePath = string.Concat(this.executablePath, this.executableFile);
            }

            if(!File.Exists(executablePath))
            {
                //TODO Throw the corresponding exception
                return;
            }

            if(this.executablePath.Contains(" "))
            {
                this.executablePath = string.Concat("\"", this.executablePath, "\"");
            }

            Process process = new Process();
            process.StartInfo.FileName = this.executablePath;
            process.StartInfo.Arguments = this.executableArguments;
            process.StartInfo.CreateNoWindow = this.createNoWindow;
            process.StartInfo.WindowStyle = this.processWindowStyle;
            process.Start();

            if(this.waitForExit)
            {
                process.WaitForExit();
            }
        }

        /// <summary>
        /// See <see cref="M:Microsoft.Practices.RecipeFramework.IAction.Undo"/>.
        /// </summary>
        public override void Undo()
        {
        } 
        #endregion
    }
}

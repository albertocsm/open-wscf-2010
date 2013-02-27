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
using EnvDTE; 
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.Actions.VisualStudio
{
    /// <summary>
    /// Action that executes a generic Vs Command
    /// </summary>
    public class ExecuteVSCommandAction : ConfigurableAction
    {
        #region Input Properties
        private string vsCommand;

        /// <summary>
        /// Gets or sets the VS command.
        /// </summary>
        /// <value>The VS command.</value>
        [Input(Required = true)]
        public string VSCommand
        {
            get { return vsCommand; }
            set { vsCommand = value; }
        }

        private string vsCommandArguments;

        /// <summary>
        /// Gets or sets the VS command arguments.
        /// </summary>
        /// <value>The VS command arguments.</value>
        [Input(Required = false)]
        public string VSCommandArguments
        {
            get { return vsCommandArguments; }
            set { vsCommandArguments = value; }
        }

        #endregion

        #region ConfigurableAction Implementation
        /// <summary>
        /// See <see cref="M:Microsoft.Practices.RecipeFramework.IAction.Execute"/>.
        /// </summary>
        public override void Execute()
        {
            DTE vs = (DTE)GetService(typeof(DTE));
            vs.ExecuteCommand(this.vsCommand, this.vsCommandArguments);
        }

        /// <summary>
        /// See <see cref="M:Microsoft.Practices.RecipeFramework.IAction.Undo"/>.
        /// </summary>
        public override void Undo()
        {
            //Do Nothing
        }
        #endregion
    }
}

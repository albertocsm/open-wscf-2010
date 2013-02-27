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
using Microsoft.Practices.RecipeFramework;
using System.Globalization; 
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.Actions.VisualStudio
{
    /// <summary>
    /// Action that compiles the solution
    /// </summary>
    public class CompileSolutionAction : ConfigurableAction
    {
        #region Input Properties
        private bool throwException = false;

        /// <summary>
        /// Gets or sets a value indicating whether [throw exception].
        /// </summary>
        /// <value><c>true</c> if [throw exception]; otherwise, <c>false</c>.</value>
        [Input(Required = false)]
        public bool ThrowException
        {
            get { return throwException; }
            set { throwException = value; }
        }
        #endregion

        #region ConfigurableAction Implementation
        /// <summary>
        /// See <see cref="M:Microsoft.Practices.RecipeFramework.IAction.Execute"/>.
        /// </summary>
        public override void Execute()
        {
            DTE vs = (DTE)GetService(typeof(DTE));

            vs.Solution.SolutionBuild.Build(true);

            if(vs.Solution.SolutionBuild.LastBuildInfo != 0)
            {
                ((EnvDTE80.DTE2)vs).ToolWindows.ErrorList.Parent.Activate();

                if(this.throwException)
                {
                    throw new ActionExecutionException(String.Format(
                        CultureInfo.CurrentCulture,
                        Properties.Resources.CompilationFailed,
                        vs.Solution.SolutionBuild.LastBuildInfo));
                }
            }
        }

        /// <summary>
        /// See <see cref="M:Microsoft.Practices.RecipeFramework.IAction.Undo"/>.
        /// </summary>
        public override void Undo()
        {
            //Not Implemented
        }
        #endregion
    }
}

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

namespace Microsoft.Practices.RecipeFramework.Extensions.Actions.VisualStudio
{
	/// <summary>
	/// GetSolutionGlobalsEntryAction class.
	/// </summary>
    public class GetSolutionGlobalsEntryAction : Action
    {
        #region Input Properties

        private Solution solution;

		/// <summary>
		/// Gets or sets the solution.
		/// </summary>
		/// <value>The solution.</value>
        [Input(Required = true)]
        public Solution Solution
        {
            get { return solution; }
            set { solution = value; }
        }

        private string variableName;

		/// <summary>
		/// Gets or sets the name of the variable.
		/// </summary>
		/// <value>The name of the variable.</value>
        [Input(Required = true)]
        public string VariableName
        {
            get { return variableName; }
            set { variableName = value; }
        }
        #endregion

        #region Output Properties

        private string variableValue;

		/// <summary>
		/// Gets or sets the variable value.
		/// </summary>
		/// <value>The variable value.</value>
        [Output]
        public string VariableValue
        {
            get { return variableValue; }
            set { variableValue = value; }
        }

        #endregion

        #region Action Implementation

		/// <summary>
		/// See <see cref="M:Microsoft.Practices.RecipeFramework.IAction.Execute"/>.
		/// </summary>
        public override void Execute()
        {
            if(this.solution.Globals.get_VariableExists(this.variableName))
            {
                this.variableValue = (string)this.solution.Globals[this.variableName];
            }
            else
            {
                this.variableValue = null;
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

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
using System.Runtime.Serialization;
using EnvDTE; 
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.References.VisualStudio
{
    /// <summary>
    /// UnBoundRecipeReference that allows to be executed only on a Vs Project
    /// </summary>
    [Serializable]
    public class ProjectReference : CSharpLanguageReference
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ProjectReference"/> class.
        /// </summary>
        /// <param name="recipe">The recipe.</param>
        public ProjectReference(string recipe)
            : base(recipe)
        {
        }

        /// <summary>
        /// See <see cref="P:Microsoft.Practices.RecipeFramework.IAssetReference.AppliesTo"/>.
        /// </summary>
        /// <value></value>
        public override string AppliesTo
        {
            get { return Properties.Resources.ProjectReference; }
        }

        /// <summary>
        /// Performs the validation of the item passed as target
        /// Returns true if the reference is allowed to be executed in the target
        /// that is if the target is a project.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public override bool OnIsEnabledFor(object target)
        {
            Project project = target as Project;

            return project != null;
        }

        #region ISerializable Members

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ProjectReference"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the reference.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected ProjectReference(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion ISerializable Members
    }
}

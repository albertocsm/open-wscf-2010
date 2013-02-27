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
    /// UnBoundRecipeReference that allows to be executed on any language Vs Project
    /// </summary>
    [Serializable]
    public class AnyProjectReference : UnboundRecipeReference
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:AnyProjectReference"/> class.
        /// </summary>
        /// <param name="recipe">The recipe.</param>
        public AnyProjectReference(string recipe)
            : base(recipe)
        {
        }

        /// <summary>
        /// See <see cref="P:Microsoft.Practices.RecipeFramework.IAssetReference.AppliesTo"/>.
        /// </summary>
        /// <value></value>
        public override string AppliesTo
        {
            get { return Properties.Resources.AnyProjectReference; }
        }

        /// <summary>
        /// Determines whether the reference is enabled for a particular target item,
        /// based on the condition contained in the reference.
        /// </summary>
        /// <param name="target">The <see cref="T:System.Object"/> to check for references.</param>
        /// <returns>
        /// 	<see langword="true"/> if the reference is enabled for the given <paramref name="target"/>.
        /// Otherwise, <see langword="false"/>.
        /// </returns>
        public override bool IsEnabledFor(object target)
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
        protected AnyProjectReference(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion ISerializable Members
    }
}

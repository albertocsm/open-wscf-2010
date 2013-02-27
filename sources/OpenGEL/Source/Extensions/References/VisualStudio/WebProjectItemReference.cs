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
using VsWebSite;
using System.IO;
using System.Runtime.Serialization;
using EnvDTE;
using Microsoft.Practices.RecipeFramework.Library;

namespace Microsoft.Practices.RecipeFramework.Extensions.References.VisualStudio
{
    /// <summary>
    /// UnBoundRecipe that allows to be executed only on a WebProjectItem
    /// </summary>
    [Serializable]
    public class WebProjectItemReference : ProjectItemReference
    {
        /// <summary>
        /// Constructor of the <see cref="WebProjectItemReference"/> that must specify the 
		/// recipe name that will be used by the reference
		/// </summary>
		/// <param name="recipe"></param>
        public WebProjectItemReference(string recipe)
			: base(recipe)
		{
		}

        /// <summary>
        /// Performs the validation of the item passed as target
        /// Returns true if the reference is allowed to be executed in the target
        /// that is if the target is a <see cref="VSWebProjectItem"/> file.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public override bool OnIsEnabledFor(object target)
        {
            ProjectItem item = target as ProjectItem;

            if (item == null ||
                (item.ContainingProject != null && !DteHelper.IsWebProject(item.ContainingProject)))
            {
                return false;
            }

            return base.OnIsEnabledFor(target);
        }

        #region ISerializable Members

        /// <summary>
        /// Initializes a new instance of the <see cref="T:WebProjectItemReference"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the reference.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected WebProjectItemReference(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion ISerializable Members
    }
}

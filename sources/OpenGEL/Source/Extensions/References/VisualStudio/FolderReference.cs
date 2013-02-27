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
using EnvDTE;
using EnvDTE80;
using VSLangProj;
using System.Runtime.Serialization;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.Common; 
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.References.VisualStudio
{
	/// <summary>
	/// UnBoundRecipe that allows to be executed only on Solution Folders
	/// </summary>
	[Serializable]
    public class FolderReference : UnboundRecipeReference, IAttributesConfigurable
	{
        private string folderName;
        
        /// <summary>
		/// Constructor of the FolderReference that must specify the 
		/// recipe name that will be used by the reference
		/// </summary>
		/// <param name="recipe"></param>
		public FolderReference(string recipe)
			: base(recipe)
		{
		}

        /// <summary>
        /// See <see cref="P:Microsoft.Practices.RecipeFramework.IAssetReference.AppliesTo"/>.
        /// </summary>
        /// <value></value>
		public override string AppliesTo
		{
			get { return Properties.Resources.SolutionFolderReference; }
		}

		/// <summary>
		/// Performs the validation of the item passed as target
		/// Returns true if the reference is allowed to be executed in the target
		/// that is if the target is a solution folder
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		public override bool IsEnabledFor(object target)
		{
            Project folder = target as Project;

            if (folder != null)
			{
                if (!string.IsNullOrEmpty(folderName))
                {
                    if (folder.Name.Equals(folderName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
			}

			return false;
		}

		#region ISerializable Members

        /// <summary>
        /// Initializes a new instance of the <see cref="T:FolderReference"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the reference.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
		protected FolderReference(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion ISerializable Members

        #region IAttributesConfigurable Members

        /// <summary>
        /// Configures the component using the dictionary of attributes specified
        /// in the configuration file.
        /// </summary>
        /// <param name="attributes">The attributes in the configuration element.</param>
        public void Configure(System.Collections.Specialized.StringDictionary attributes)
        {
            if (attributes.ContainsKey("FolderName"))
            {
                folderName = attributes["FolderName"];
            }
        }

        #endregion	
    }
}

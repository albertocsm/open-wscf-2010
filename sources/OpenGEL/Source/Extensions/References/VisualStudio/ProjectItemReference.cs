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
using System.Security.Permissions;
using EnvDTE;
using EnvDTE80;
using VSLangProj;
using System.Runtime.Serialization;
using Microsoft.Practices.RecipeFramework;
using System.Xml;
using System.IO;
using Microsoft.Practices.RecipeFramework.VisualStudio;
using Microsoft.Practices.Common;
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.References.VisualStudio
{
    /// <summary>
    /// UnBoundRecipe that allows to be executed only on ProjectItem
    /// </summary>
    [Serializable]
    public class ProjectItemReference : CSharpLanguageReference, IAttributesConfigurable
    {
        /// <summary>
        /// The file extension configuration argument.
        /// </summary>
        protected string fileExtension;
        private const string FileExtensionAttribute = "FileExtension";

        /// <summary>
        /// Constructor of the <see cref="ProjectItemReference"/> that must specify the 
		/// recipe name that will be used by the reference
		/// </summary>
		/// <param name="recipe"></param>
        public ProjectItemReference(string recipe)
			: base(recipe)
		{
		}

        /// <summary>
        /// See <see cref="P:Microsoft.Practices.RecipeFramework.IAssetReference.AppliesTo"/>.
        /// </summary>
        /// <value></value>
        public override string AppliesTo
        {
            get { return Properties.Resources.ProjectItemReference; }
        }

        /// <summary>
        /// Performs the validation of the item passed as target
        /// Returns true if the reference is allowed to be executed in the target
        /// that is if the target is a <see cref="ProjectItem"/> file.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public override bool OnIsEnabledFor(object target)
        {
            ProjectItem item = target as ProjectItem;

            if(item != null)
            {
                if(!string.IsNullOrEmpty(fileExtension))
                {
                    return fileExtension.Equals(Path.GetExtension(item.Name),
                        StringComparison.InvariantCultureIgnoreCase);
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
        /// Initializes a new instance of the <see cref="T:ProjectItemReference"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the reference.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected ProjectItemReference(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            fileExtension = info.GetString("FileExtension");
        }

        /// <summary>
        /// Provides a serialization member that derived classes can override to add
        /// data to be stored for serialization.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the reference.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter=true)]
        protected override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("FileExtension", fileExtension);
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
            if(attributes.ContainsKey(FileExtensionAttribute))
            {
                fileExtension = attributes[FileExtensionAttribute];
            }
        }

        #endregion
    }
}
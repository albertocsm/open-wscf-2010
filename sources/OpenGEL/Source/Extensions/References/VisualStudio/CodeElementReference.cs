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
using System.Security.Permissions;
using System.Text;
using Microsoft.Practices.Common;
using Microsoft.Practices.RecipeFramework;
using EnvDTE;
using System.ComponentModel.Design;
using System.Runtime.Serialization;
using Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers; 
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.References.VisualStudio
{
    /// <summary>
    /// UnBoundRecipeReference that allows to be executed only on a CodeElement specified in the CodeElementTypeName property
    /// </summary>
    [Serializable]
    public class CodeElementReference : UnboundRecipeReference, IAttributesConfigurable
    {
        private string codeElementTypeName;
        private const string CodeElementTypeNameAttribute = "CodeElementTypeName";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CodeElementReference"/> class.
        /// </summary>
        /// <param name="recipe">The recipe.</param>
        public CodeElementReference(string recipe)
            : base(recipe)
        {

        }
        /// <summary>
        /// Performs the validation of the item passed as target
        /// Returns true if the reference is allowed to be executed in the target
        /// that is if the target is the CodeElement specified in the CodeElementTypeName property.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public override bool IsEnabledFor(object target)
        {
            DTE vs = GetService<DTE>();
			if (vs == null || vs.ActiveDocument == null)
				return false;

            ProjectItem item = vs.ActiveDocument.ProjectItem;
            ITypeResolutionService typeResolutionService = GetService<ITypeResolutionService>();

            if(item != null && item.FileCodeModel != null)
            {
                object element = FileCodeModelHelper.GetCodeElement(vs, item.FileCodeModel, typeResolutionService.GetType(codeElementTypeName));
                return element != null;
            }

            return false;
        }

        /// <summary>
        /// See <see cref="P:Microsoft.Practices.RecipeFramework.IAssetReference.AppliesTo"/>.
        /// </summary>
        /// <value></value>
        public override string AppliesTo
        {
            get { return Properties.Resources.CodeElementReference; }
        }

        #region IAttributesConfigurable Members

        /// <summary>
        /// Configures the component using the dictionary of attributes specified
        /// in the configuration file.
        /// </summary>
        /// <param name="attributes">The attributes in the configuration element.</param>
        public void Configure(System.Collections.Specialized.StringDictionary attributes)
        {
            if(attributes.ContainsKey(CodeElementTypeNameAttribute))
            {
                codeElementTypeName = attributes[CodeElementTypeNameAttribute];
            }
        }

        #endregion

        #region ISerializable Members
        /// <summary>
        /// Initializes a new instance of the <see cref="T:CodeElementReference"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the reference.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected CodeElementReference(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            codeElementTypeName = info.GetString(CodeElementTypeNameAttribute);
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
            info.AddValue(CodeElementTypeNameAttribute, codeElementTypeName);
        }
        #endregion ISerializable Members
    }
}

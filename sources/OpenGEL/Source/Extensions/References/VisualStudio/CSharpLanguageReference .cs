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
using System.Security.Permissions;
using System.Runtime.Serialization;
using System.Collections.Specialized;
using System.CodeDom.Compiler;
using System.Globalization;
using EnvDTE;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.Common;
using Microsoft.Practices.RecipeFramework.Library.Solution;
using Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers;
using Microsoft.Practices.RecipeFramework.Library;

namespace Microsoft.Practices.RecipeFramework.Extensions.References.VisualStudio
{
    /// <summary>
    /// UnBoundRecipe that allows to be executed only on CSharp targets
    /// </summary>
    [Serializable]
    public class CSharpLanguageReference : UnboundRecipeReference
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:LanguageTypeReference"/> class.
        /// </summary>
        /// <param name="recipe">The recipe.</param>
        public CSharpLanguageReference(string recipe)
            : base(recipe)
        {
        }

        /// <summary>
        /// Required constructor for deserialization.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the reference.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected CSharpLanguageReference(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// See <see cref="P:Microsoft.Practices.RecipeFramework.IAssetReference.AppliesTo"/>.
        /// </summary>
        /// <value></value>
        public override string AppliesTo
        {
            get { return string.Empty; }
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
            if (!ReferenceUtil.IsCSharpProject(target) &&
                !DteHelperEx.IsWebCSharpProject(target))
            {
                return false;
            }

            return OnIsEnabledFor(target);
        }

        /// <summary>
        /// Called when [is enabled for].
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public virtual bool OnIsEnabledFor(object target)
        {
            return true;
        }

        /// <summary>
        /// Provides a serialization member that derived classes can override to add
        /// data to be stored for serialization.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the reference.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The info object is null. </exception>
        /// <filterpriority>2</filterpriority>
        /// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" /></PermissionSet>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        protected override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Guard.ArgumentNotNull(info, "info");
            base.GetObjectData(info, context);
        }
    }
}

//===============================================================================
// Microsoft patterns & practices
// Web Client Software Factory 2010
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
//===============================================================================
// Microsoft patterns & practices
// Web Client Software Factory
//-------------------------------------------------------------------------------
// Copyright (C) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//-------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.RecipeFramework.Extensions.References.VisualStudio;
using Microsoft.Practices.Common;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using EnvDTE;

namespace Microsoft.Practices.WebClientFactory.References
{
    /// <summary>
    /// UnBoundRecipeReference that allows to be executed only on a Vs Project based on property
    /// </summary>
    [Serializable]
    public class ProjectPropertyReferenceCS : CSharpLanguageReference, IAttributesConfigurable
    {
        #region Constants & Fields

        private const string IsEnabledForPropertyAttribute = "IsEnabledForProperty";
        private const string AppliesToTextAttribute = "AppliesToText";
        private string propertyName;
        private string appliesToText;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ProjectPropertyReferenceCS"/> class.
        /// </summary>
        /// <param name="recipe">The recipe.</param>
        public ProjectPropertyReferenceCS(string recipe)
            : base(recipe)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// See <see cref="P:Microsoft.Practices.RecipeFramework.IAssetReference.AppliesTo"/>.
        /// </summary>
        /// <value></value>
        public override string AppliesTo
        {
            get
            {
                if (string.IsNullOrEmpty(appliesToText))
                {
                    return Properties.Resources.ProjectReference;
                }
                return appliesToText;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs the validation of the item passed as target
        /// Returns true if the reference is allowed to be executed in the target
        /// that is if the target is a project that has the mapping property.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public override bool OnIsEnabledFor(object target)
        {
            Project project = target as Project;

            if (project != null)
            {
                try
                {
                    if (project.Globals.get_VariableExists(propertyName))
                    {
                        string propertyValue = (string)project.Globals[propertyName];
                        bool propertyValueBoolean;

                        if (Boolean.TryParse(propertyValue, out propertyValueBoolean))
                        {
                            return propertyValueBoolean;
                        }
                    }
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        #endregion

        #region ISerializable Members

        /// <summary>
        /// Required constructor for deserialization.
        /// </summary>
        protected ProjectPropertyReferenceCS(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info != null)
            {
                this.propertyName = info.GetString(IsEnabledForPropertyAttribute);
                try
                {
                    this.appliesToText = info.GetString(AppliesToTextAttribute);
                }
                catch (Exception)
                {
                    // Do nothing, if it fails it means the optional AppliesToText isn't specified.
                }
            }
        }

        /// <summary>
        /// Provides a serialization member that derived classes can override to add
        /// data to be stored for serialization.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the reference.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The info object is null. </exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*"/></PermissionSet>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        protected override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(IsEnabledForPropertyAttribute, this.propertyName);
            info.AddValue(AppliesToTextAttribute, this.appliesToText);
        }

        #endregion ISerializable Members

        #region IAttributesConfigurable Members

        /// <summary>
        /// Configures the component using the dictionary of attributes specified
        /// in the configuration file.
        /// </summary>
        /// <param name="attributes">The attributes in the configuration element.</param>
        public virtual void Configure(System.Collections.Specialized.StringDictionary attributes)
        {
            if (attributes == null)
                throw new ArgumentNullException("attributes");

            if (attributes.ContainsKey(IsEnabledForPropertyAttribute))
            {
                propertyName = attributes[IsEnabledForPropertyAttribute];
            }
            if (attributes.ContainsKey(AppliesToTextAttribute))
            {
                appliesToText = attributes[AppliesToTextAttribute];
            }
        }

        #endregion
    }
}
